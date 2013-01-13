// ------------------------------------------------------------------------
// <copyright file="DistributionsDomainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.Settings;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Attachment = fMailer.Domain.Model.Attachment;

namespace fMailer.Web.Controllers.Domain
{
    public class DistributionsDomainController : BaseController
    {
        public DistributionsDomainController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager)
            :base(repository, settings, sessionManager)
        {
        }

        [HttpPost]
        public JsonResult LoadReply(int replyId)
        {
            var realReply = Repository.GetById<Reply>(replyId);
            if (realReply.IsNew)
            {
                realReply.IsNew = false;
            }

            return Json(realReply);
        }

        [HttpGet]
        public FileContentResult LoadAttachment(int id)
        {
            var attachment = Repository.GetById<ReplyAttachment>(id);
            return new FileContentResult(attachment.Content, attachment.ContentType) { FileDownloadName = attachment.Name };
        }

        public JsonResult MarkFailAsRead(int failedId)
        {
            var fail = Repository.GetById<FailedDelivery>(failedId);
            if (fail.IsNew)
            {
                fail.IsNew = false;
            }

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

            return Json(User.Distributions.OrderByDescending(x => x.Id).Select(x => x.GetLightClone()));
        }

        [HttpPost]
        public JsonResult LoadDistributionsWithoutProcessing()
        {
            return Json(User.Distributions.OrderByDescending(x => x.Id).Select(x => x.GetLightClone()));
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

            return messages.OrderByDescending(x => x.Key).Select(x => GetMailMessageFromMessage(x.Value)).ToList();
        }

        private MailMessage GetMailMessageFromMessage(Message msg)
        {
            var mailMessage = msg.ToMailMessage();
            mailMessage.Attachments.Clear();

            var attachParts = msg.FindAllAttachments();
            foreach (var part in attachParts)
            {
                var attachment = new System.Net.Mail.Attachment(new MemoryStream(part.Body), part.FileName, part.ContentType.MediaType);
                mailMessage.Attachments.Add(attachment);
            }
            
            return mailMessage;
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

            var smtpClient = CreateSmtpClient();
            var allMessages = new List<MailMessage>();
            foreach (var recipient in recipients)
            {
                var email = CreateMail(recipient, distribution.Template);
                allMessages.Add(email);
                smtpClient.Send(email);
            }
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
                   lowerInbox.Contains(lowerOriginal) ||
                   "re:" + lowerOriginal.Replace(" ", "") == lowerInbox.Replace(" ", "");
        }

        private MailMessage CreateMail(Contact contact, MailTemplate template)
        {
            var message = new MailMessage();
            
            message.Subject = GetCompleteText(contact, template.Subject);
            message.SubjectEncoding = Encoding.UTF8;

            message.Body = GetCompleteText(contact, template.Text);
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            foreach (var attachment in template.Attachments)
            {
                var stream = new MemoryStream(attachment.Content);
                var mailAttachment = new System.Net.Mail.Attachment(stream, attachment.Name, attachment.ContentType);
                message.Attachments.Add(mailAttachment);
            }

            message.From = new MailAddress(User.Settings.EmailAddressFrom);
            message.To.Add(new MailAddress(contact.Email));
            return message;
        }

        private SmtpClient CreateSmtpClient()
        {
            var settings = User.Settings;
            var smtpClient = new SmtpClient
            {
                Host = settings.SmtpAddress,
                Port = settings.SmtpUseSsl ? settings.SmtpSslPort.Value : settings.SmtpTlsPort.Value,
                EnableSsl = settings.SmtpUseSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 100000,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.Username, settings.Password)
            };
            return smtpClient;
        }

        private Pop3Client CreatePop3Client()
        {
            var settings = User.Settings;
            var pop3Client = new Pop3Client();
            pop3Client.Connect(settings.Pop3Address, settings.Pop3Prot.Value, settings.Pop3UseSsl);
            pop3Client.Authenticate(settings.Pop3IsGmail ? "recent:" + settings.Username : settings.Username, settings.Password);
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
            var reply = new Reply
            {
                From = from,
                EmailText = message.Body,
                RecievedOn = DateTime.Now,
                Subject = message.Subject,
                IsNew = true
            };
            foreach (var mailAttachment in message.Attachments)
            {
                reply.AddAttachment(CreateAttachment(mailAttachment));
            }

            return reply;
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

        private ReplyAttachment CreateAttachment(System.Net.Mail.Attachment mailAttachmetn)
        {
            var size = (int)mailAttachmetn.ContentStream.Length;
            byte[] content = new byte[size];
            mailAttachmetn.ContentStream.Read(content, 0, size);
            return new ReplyAttachment { Name = mailAttachmetn.Name, Content = content, ContentType = mailAttachmetn.ContentType.MediaType, Size = size };
        }
    }
}
