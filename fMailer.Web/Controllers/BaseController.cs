// ------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.Helpers;
using fMailer.Web.Core.Settings;

namespace fMailer.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IRepository repository;

        private readonly IMailerSettings settings;

        private readonly ISessionManager sessionManager;

        public BaseController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager)
        {
            this.repository = repository;
            this.settings = settings;
            this.sessionManager = sessionManager;
        }
        
        protected IRepository Repository { get { return repository; } }

        protected IMailerSettings Settings { get { return settings; } }

        protected ISessionManager SessionManager { get { return sessionManager; } }

        protected User User
        {
            get
            {
                return Repository.GetById<User>(Request.Cookies.GetUserId());
            }
        }
    }
}
