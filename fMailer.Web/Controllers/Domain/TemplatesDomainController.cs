// ------------------------------------------------------------------------
// <copyright file="TemplatesDomainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.HashProviders;
using fMailer.Web.Core.Settings;

namespace fMailer.Web.Controllers.Domain
{
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
        public JsonResult UpdateTemplate(MailTemplate template)
        {
            if (template.Id < 1)
            {
                // New Item
                User.AddTemplate(template);
            }
            else
            {
                // Update Item
                var temp = Repository.GetById<MailTemplate>(template.Id);
                temp.Name = template.Name;
                temp.Text = template.Text;
                temp.Description = template.Description;                
            }

            return Json(template.Id);
        }
    }
}
