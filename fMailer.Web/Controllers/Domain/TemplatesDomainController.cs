// ------------------------------------------------------------------------
// <copyright file="TemplatesDomainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.Settings;

namespace fMailer.Web.Controllers.Domain
{
    [ValidateInput(false)]
    public class TemplatesDomainController : BaseController
    {
        public TemplatesDomainController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager)
            :base(repository, settings, sessionManager)
        {
        }

        [HttpPost]
        public JsonResult LoadTemplates()
        {
            return Json(User.Templates);
        }

        [HttpPost]
        public JsonResult LoadPureTemplates()
        {
            var tr = new List<MailTemplate>();
            var templates = User.Templates.ToArray();
            foreach (var template in templates)
            {
                var clone = template.Clone();
                clone.Text = "";
                tr.Add(clone);
            }

            return Json(tr);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateTemplate(MailTemplate template)
        {
            if (template.Id < 1)
            {
                // New Item
                if (User.Templates.Any(x => x.Name.ToLower() == template.Name.ToLower()))
                {
                    return Json(false);
                }

                template.UpdateAttachments();
                User.AddTemplate(template);                
            }
            else
            {
                // Update Item
                var temp = Repository.GetById<MailTemplate>(template.Id);
                temp.Name = template.Name;
                temp.Text = template.Text;
                temp.Subject = template.Subject;
                temp.Attachments.Clear();
                foreach (var attachment in template.Attachments)
                {
                    if (attachment.Id < 1)
                    {
                        temp.AddAttachment(attachment);
                    }
                    else
                    {
                        temp.Attachments.Add(Repository.GetById<Attachment>(attachment.Id));
                    }
                }
            }

            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteTemplate(MailTemplate template)
        {
            var temp = Repository.GetById<MailTemplate>(template.Id);
            Repository.Delete(temp);
            return Json(true);
        }
        
        [HttpGet]
        public FileContentResult LoadAttachment(int id)
        {
            var attachment = Repository.GetById<Attachment>(id);
            var fileResult = new FileContentResult(attachment.Content, attachment.ContentType);
            fileResult.FileDownloadName = attachment.Name;
            return fileResult;
        }
    }
}
