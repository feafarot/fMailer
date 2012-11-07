// ------------------------------------------------------------------------
// <copyright file="AuthController.cs" company="feafarot">
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
    public class AuthController : BaseController
    {
        public AuthController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager) 
            : base(repository, settings, sessionManager)
        {
        }

        [OnlyNotAuthorized]
        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignOff()
        {
            SessionManager.EndSession(Request.Cookies);
            return View();
        }
    }
}
