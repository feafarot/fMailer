// ------------------------------------------------------------------------
// <copyright file="AuthDomainController.cs" company="feafarot">
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
    public class AuthDomainController : BaseController
    {
        public AuthDomainController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager)
            :base(repository, settings, sessionManager)
        {
        }

        public JsonResult SignIn(string login, string password)
        {
            User user;
            if (Settings.UsePasswordsHashing)
            {
                var hashProvider = DependencyResolver.Current.GetService<IHashProvider>();
                user = Repository.GetAll<User>().SingleOrDefault(x => (x.Login == login || x.Email == login) && x.Password == hashProvider.CalculateHash(password));
            }
            else
            {
                user = Repository.GetAll<User>().SingleOrDefault(x => (x.Login == login || x.Email == login) && x.Password == password);
            }            
            
            if (user == null)
            {
                return Json(false);
            }

            var sid = SessionManager.BeginSession(user);
            Response.Cookies.Add(Utils.CreateSessionsDependCookie("uid", user.Id.ToString(CultureInfo.InvariantCulture)));
            Response.Cookies.Add(Utils.CreateSessionsDependCookie("sid", sid));
            return Json(true);
        }
    }
}
