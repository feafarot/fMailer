// ------------------------------------------------------------------------
// <copyright file="DistributionsDomainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Collections.Generic;
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
        public JsonResult LoadDistributions()
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

        private SmtpMessage CreateMail(Contact contact, MailTemplate template)
        {
            var message = new SmtpMessage();
            message.Subject = GetCompleteText(contact, template.Subject);
            message.BodyText = GetCompleteText(contact, template.Text);
            foreach (var attachment in template.Attachments)
            {
                var smtpContent = new SmtpContent();
                smtpContent.LoadData(attachment.Content);
                smtpContent.Name = attachment.Name;
                smtpContent.FileName = attachment.Name;
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
            smtpClient.Pop3Client.ServerName = settings.Pop3Address;
            smtpClient.Pop3Client.UserName = settings.IsGmail ? "recent:" + settings.Username : settings.Username;
            smtpClient.Pop3Client.Password = settings.Password;
            smtpClient.Pop3Client.Port = settings.Pop3Prot.Value;
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
            return pop3Client;
        }

        private string GetCompleteText(Contact contact, string template)
        {
            return template.Replace("†qte†", "\"")
                           .Replace("†rvn†", "=")
                           .Replace(Settings.FirstNameKeyword, contact.FirstName)
                           .Replace(Settings.LastNameKeyword, contact.LastName)
                           .Replace(Settings.MiddleNameKeyword, contact.MiddleName)
                           .Replace(Settings.FullNameKeyword, string.Format("{0} {1} {2}", contact.LastName, contact.FirstName, contact.MiddleName));
        }
    }
}
