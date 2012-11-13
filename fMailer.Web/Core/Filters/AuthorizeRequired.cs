// ------------------------------------------------------------------------
// <copyright file="AuthorizeRequired.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web.Mvc;
using fMailer.Domain.DataAccess;

namespace fMailer.Web.Core.Filters
{
    public class AuthorizeRequired : BaseActionFilter
    {
        public string AuthorizationRouteName { get; set; }

        protected override bool OnActionExecuting(ActionExecutingContext filterContext, int? uid, IRepository repository, ISessionManager sessionManager)
        {
            if (!uid.HasValue)
            {
                filterContext.Result = new RedirectToRouteResult(RouteNames.SignIn, null);
                return false;
            }

            if (!sessionManager.ValidateSession(filterContext.HttpContext.Request.Cookies))
            {
                filterContext.Result = new RedirectToRouteResult(RouteNames.SignIn, null);
                return false;
            }

            return base.OnActionExecuting(filterContext, uid, repository, sessionManager);
        }
    }
}