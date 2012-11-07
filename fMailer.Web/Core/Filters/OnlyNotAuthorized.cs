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
                filterContext.Result = GetRedirect(filterContext.HttpContext.Request, "Main", "Hub");
                return false;
            }

            return base.OnActionExecuting(filterContext, uid, repository, sessionManager);
        }
    }
}