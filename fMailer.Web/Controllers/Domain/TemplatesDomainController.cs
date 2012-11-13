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
                template.User = User;
                User.Templates.Add(template);
            }
            else
            {
                // Update Item
                Repository.Update(template);
            }

            return Json(template.Id);
        }
    }
}
