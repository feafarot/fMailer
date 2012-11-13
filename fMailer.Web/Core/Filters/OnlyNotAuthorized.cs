// ------------------------------------------------------------------------
// <copyright file="AuthorizeRequired.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web.Mvc;
using fMailer.Domain.DataAccess;

namespace fMailer.Web.Core.Filters
{
    public class OnlyNotAuthorized : BaseActionFilter
    {
        protected override bool OnActionExecuting(ActionExecutingContext filterContext, int? uid, IRepository repository, ISessionManager sessionManager)
        {
            if (uid.HasValue && sessionManager.ValidateSession(filterContext.HttpContext.Request.Cookies))
            {
                filterContext.Result = new RedirectToRouteResult(RouteNames.Distributions, null);
                return false;
            }

            return base.OnActionExecuting(filterContext, uid, repository, sessionManager);
        }
    }
}