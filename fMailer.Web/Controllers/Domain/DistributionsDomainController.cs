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
using System.Threading.Tasks;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.HashProviders;
using fMailer.Web.Core.Settings;
using HigLabo.Net.Mail;
using HigLabo.Net.Pop3;
using HigLabo.Net.Smtp;

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
            //var messages = LoadMessages();
            //if (messages.Count != 0)
            //{
            //    UpdateReplies(messages);
            //}

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

        private List<MailMessage> LoadMessages()
        {
            var messages = new List<MailMessage>();
            var pop3Client = CreatePop3Client();
            if (!pop3Client.Authenticate())
            {
                Debug.WriteLine("POP3 authentification failed");
                return messages;
            }

            var count = pop3Client.GetTotalMessageCount();
            for (long i = count; i > 0; i--)
            {
                messages.Add(pop3Client.GetMessage(i));
            }

            return messages;
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
            var sendTask = new Task(() => smtpClient.SendMailList(allMessages));
            sendTask.Start();
            sendTask.Wait();
        }

        private void UpdateRepliesAndFails(IList<MailMessage> messages)
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
                    if (distribution.Replies.FirstOrDefault(x => x.From.Equals(recipient)) != null)
                    {
                        continue;
                    }

                    var message = messages
                                    .FirstOrDefault(x => x.From.Contains(recipient.Email) && 
                                                         IsSubjectReply(GetCompleteText(recipient, distribution.Template.Subject), x.Subject));
                    if (message != null)
                    {
                        distribution.AddReply(CreateReplyFromMail(message, recipient));
                    }
                    else
                    {
                        // TODO: Implement handling of delivery failed messages
                    }
                }
            }
        }

        private bool IsSubjectReply(string originalSubject, string inboxSubject)
        {
            var lowerOriginal = originalSubject.ToLower();
            var lowerInbox = inboxSubject.ToLower();
            return "re: " + lowerOriginal == lowerInbox ||
                   lowerOriginal == lowerInbox ||
                   lowerInbox.Contains(lowerOriginal);
        }

        private SmtpMessage CreateMail(Contact contact, MailTemplate template)
        {
            var message = new SmtpMessage();
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
            var pop3Client = new Pop3Client(
                settings.Pop3Address, 
                settings.Pop3Prot.Value, 
                settings.IsGmail ? "recent:" + settings.Username : settings.Username, 
                settings.Password);
            pop3Client.Ssl = settings.Pop3UseSsl;
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

        private Reply CreateReplyFromMail(MailMessage message, Contact from)
        {
            return new Reply
            {
                From = from,
                EmailText = message.BodyText,
                RecievedOn = DateTime.Now,
                Subject = message.Subject,
                IsNew = true
            };
        }
    }
}
