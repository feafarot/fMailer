// ------------------------------------------------------------------------
// <copyright file="MainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Web.Core;
using fMailer.Web.Core.Filters;
using fMailer.Web.Core.Settings;

namespace fMailer.Web.Controllers
{
    [AuthorizeRequired]
    public class MainController : BaseController
    {
        public MainController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager) 
            : base(repository, settings, sessionManager)
        {
        }

        public ActionResult Distributions()
        {
            return View();
        }

        public ActionResult Templates()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }
    }
}