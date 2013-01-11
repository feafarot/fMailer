// ------------------------------------------------------------------------
// <copyright file="MainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web.Mvc;
using AutoMapper;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
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

        public new ActionResult Settings()
        {
            ViewBag.IsSuccess = null;
            return View(User.Settings);
        }

        [HttpPost]
        public new ActionResult Settings(Settings model)
        {
            ViewBag.IsSuccess = true;

            User.Settings.Username = model.Username;
            if (!string.IsNullOrEmpty(model.Password))
            {
                User.Settings.Password = model.Password;
            }

            User.Settings.Password = model.Password;
            User.Settings.EmailAddressFrom = model.EmailAddressFrom;
            User.Settings.Signature = model.Signature;

            User.Settings.SmtpAddress = model.SmtpAddress;
            User.Settings.SmtpSslPort = model.SmtpSslPort;
            User.Settings.SmtpTlsPort = model.SmtpTlsPort;
            User.Settings.SmtpUseAuth = model.SmtpUseAuth;
            User.Settings.SmtpUseSsl = model.SmtpUseSsl;

            User.Settings.IsGmail = model.IsGmail;
            User.Settings.Pop3Address = model.Pop3Address;
            User.Settings.Pop3Prot = model.Pop3Prot;
            User.Settings.Pop3UseSsl = model.Pop3UseSsl;

            return View(User.Settings);
        }
    }
}