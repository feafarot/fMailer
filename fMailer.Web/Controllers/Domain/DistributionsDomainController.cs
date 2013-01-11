// ------------------------------------------------------------------------
// <copyright file="DistributionsDomainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.HashProviders;
using fMailer.Web.Core.Settings;
using HigLabo.Net.Mail;
using HigLabo.Net.Smtp;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace fMailer.Web.Controllers.Domain
{
    public class DistributionsDomainController : BaseController
    {
        public DistributionsDomainController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager)
            :base(repository, settings, sessionManager)
        {
        }

        [HttpPost]
        public JsonResult MarkReplyAsRead(Reply reply)
        {
            var realReply = Repository.GetById<Reply>(reply.Id);
            realReply.IsNew = false;
            return Json(true);
        }

        [HttpPost]
        public JsonResult CloseDistribution(Distribution distribution)
        {
            var rds = Repository.GetById<Distribution>(distribution.Id);
            rds.IsClosed = true;
            return Json(true);
        }

        [HttpPost]
        public JsonResult LoadDistributions()
        {
            var messages = LoadMessages();
            if (messages.Count != 0)
            {
                UpdateRepliesAndFails(messages);
            }

            return Json(User.Distributions.OrderByDescending(x => x.Id));
        }

        [HttpPost]
        public JsonResult LoadDistributionsWithoutProcessing()
        {
            return Json(User.Distributions.OrderByDescending(x => x.Id));
        }

        [HttpPost]
        public JsonResult SubmitDistribution(Distribution distribution)
        {
            var distributionToInsert = new Distribution();
            distributionToInsert.Name = distribution.Name;
            distributionToInsert.Contacts = new List<Contact>(distribution.Contacts.Select(x => Repository.GetById<Contact>(x.Id)));
            distributionToInsert.Groups = new List<ContactsGroup>(distribution.Groups.Select(x => Repository.GetById<ContactsGroup>(x.Id)));
            distributionToInsert.Template = Repository.GetById<MailTemplate>(distribution.Template.Id);

            User.AddDistribution(distributionToInsert);
            ProcessDistribution(distributionToInsert);
            return Json(true);
        }

        private List<System.Net.Mail.MailMessage> LoadMessages()
        {
            var pop3Client = CreatePop3Client();
            var count = pop3Client.GetMessageCount();

            var threadsCount = 4;
            var messages = new Dictionary<int, Message>();
            var countPerThread = count / threadsCount;
            var tasks = new List<Task>();
            var current = 1;
            var off = count % threadsCount;
            for (int i = 0; i < threadsCount; i++)
            {
                var contextThread = i;
                var contextCurrent = current;
                var t = new Task(() =>
                {
                    var pop3 = CreatePop3Client();
                    for (var j = contextCurrent; j < contextCurrent + countPerThread; j++)
                    {
                        var msg = pop3.GetMessage(j);
                        messages.Add(j, msg);
                    }

                    pop3.Disconnect();
                });
                t.Start();
                tasks.Add(t);
                current += countPerThread;
            }

            if (off > 0)
            {
                var pop3 = CreatePop3Client();
                for (var j = current; j < current + off; j++)
                {
                    var msg = pop3.GetMessage(j);
                    messages.Add(j, msg);
                }

                pop3.Disconnect();

            }

            Task.WaitAll(tasks.ToArray());

            return messages.OrderByDescending(x => x.Key).Select(x => x.Value.ToMailMessage()).ToList();
        }

        private void ProcessDistribution(Distribution distribution)
        {
            var recipients = new List<Contact>(distribution.Contacts);
            foreach (var contact in distribution.Groups.SelectMany(x => x.Contacts))
            {
                if (recipients.FirstOrDefault(x => x.Id == contact.Id) == null)
                {
                    recipients.Add(contact);
                }
            }

            var allMessages = new List<SmtpMessage>();
            foreach (var recipient in recipients)
            {
                allMessages.Add(CreateMail(recipient, distribution.Template));
            }

            var smtpClient = CreateSmtpClient();
            smtpClient.SendMailList(allMessages);
        }

        private void UpdateRepliesAndFails(IList<System.Net.Mail.MailMessage> messages)
        {
            foreach (var distribution in User.Distributions.Where(x => !x.IsClosed))
            {
                var recipients = new List<Contact>(distribution.Contacts);
                foreach (var contact in distribution.Groups.SelectMany(x => x.Contacts))
                {
                    if (recipients.FirstOrDefault(x => x.Id == contact.Id) == null)
                    {
                        recipients.Add(contact);
                    }
                }

                foreach (var recipient in recipients)
                {

                    var message = messages
                                    .FirstOrDefault(x => x.From.Address.Contains(recipient.Email) && 
                                                         IsSubjectReply(GetCompleteText(recipient, distribution.Template.Subject), x.Subject));
                    if (message != null)
                    {
                        if (distribution.Replies.FirstOrDefault(x => x.From.Equals(recipient)) != null)
                        {
                            continue;
                        }

                        distribution.AddReply(CreateReplyFromMail(message, recipient));
                    }
                    else
                    {
                        if (distribution.FailedDeliveries.FirstOrDefault(x => x.To.Equals(recipient)) != null)
                        {
                            continue;
                        }
                        var failedMessage = messages.FirstOrDefault(x => IsSubjectDeliveryFailed(x.Subject) && x.Body.Contains(recipient.Email));
                        if (failedMessage != null)
                        {
                            distribution.AddDeliveryFailed(CreateFailedDeliveryFromMail(failedMessage, recipient));
                        }
                    }
                }
            }
        }

        private bool IsSubjectDeliveryFailed(string inboxSubject)
        {
            var lowerInbox = inboxSubject.ToLower();
            return lowerInbox.Contains("delivery") && lowerInbox.Contains("failed");
        }

        private bool IsSubjectReply(string originalSubject, string inboxSubject)
        {
            var lowerOriginal = originalSubject.ToLower();
            var lowerInbox = inboxSubject.ToLower();
            return "re: " + lowerOriginal == lowerInbox ||
                   "re:" + lowerOriginal == lowerInbox ||
                   lowerOriginal == lowerInbox ||
                   lowerInbox.Contains(lowerOriginal);
        }

        private SmtpMessage CreateMail(Contact contact, MailTemplate template)
        {
            var message = new SmtpMessage();
            message.ContentEncoding = Encoding.UTF8;
            message.ContentTransferEncoding = TransferEncoding.Base64;
            message.Subject = GetCompleteText(contact, template.Subject);
            message.BodyText = GetCompleteText(contact, template.Text);
            foreach (var attachment in template.Attachments)
            {
                var smtpContent = new SmtpContent();
                smtpContent.ContentType.Value = attachment.ContentType;
                smtpContent.ContentType.Name = attachment.Name;
                smtpContent.ContentDisposition.FileName = attachment.Name;
                smtpContent.LoadData(attachment.Content);
                message.Contents.Add(smtpContent);
            }

            message.IsHtml = true;
            message.From = User.Settings.Username;
            message.To.Add(new MailAddress(contact.Email));
            return message;
        }

        private SmtpClient CreateSmtpClient()
        {
            var settings = User.Settings;
            var smtpClient = new SmtpClient(
                settings.SmtpAddress, 
                settings.SmtpUseSsl ? settings.SmtpSslPort.Value : settings.SmtpTlsPort.Value, 
                settings.Username, 
                settings.Password);
            smtpClient.Ssl = settings.SmtpUseSsl;
            return smtpClient;
        }

        private Pop3Client CreatePop3Client()
        {
            var settings = User.Settings;
            var pop3Client = new Pop3Client();
            pop3Client.Connect(settings.Pop3Address, settings.Pop3Prot.Value, settings.Pop3UseSsl);
            pop3Client.Authenticate(settings.IsGmail ? "recent:" + settings.Username : settings.Username, settings.Password);
            return pop3Client;
        }

        private string GetCompleteText(Contact contact, string template)
        {
            return template.Replace("†qte†", "\"")
                           .Replace("†rvn†", "=")
                           .Replace(Settings.FirstNameKeyword, contact.FirstName)
                           .Replace(Settings.LastNameKeyword, contact.LastName)
                           .Replace(Settings.MiddleNameKeyword, contact.MiddleName)
                           .Replace(Settings.FullNameKeyword, 
                                    string.IsNullOrEmpty(contact.MiddleName) ?
                                        string.Format("{0} {1}", contact.LastName, contact.FirstName) :
                                        string.Format("{0} {1} {2}", contact.LastName, contact.FirstName, contact.MiddleName))
                           .Trim();
        }

        private Reply CreateReplyFromMail(System.Net.Mail.MailMessage message, Contact from)
        {
            return new Reply
            {
                From = from,
                EmailText = message.Body,
                RecievedOn = DateTime.Now,
                Subject = message.Subject,
                IsNew = true
            };
        }

        private FailedDelivery CreateFailedDeliveryFromMail(System.Net.Mail.MailMessage message, Contact to)
        {
            return new FailedDelivery
            {
                To = to,
                EmailText = message.Body,
                Subject = message.Subject,
                IsNew = true
            };
        }
    }
}
