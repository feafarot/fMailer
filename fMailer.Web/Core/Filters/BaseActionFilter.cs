// ------------------------------------------------------------------------
// <copyright file="BaseActionFilter.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;

namespace fMailer.Web.Core.Filters
{
    public class BaseActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repository = DependencyResolver.Current.GetService<IRepository>();
            var sessionManager = DependencyResolver.Current.GetService<ISessionManager>();
            var uidCookie = filterContext.HttpContext.Request.Cookies["uid"];
            int uid;
            int? sendUid = null;
            if (uidCookie != null && int.TryParse(uidCookie.Value, out uid))
            {
                sendUid = uid;
            }

            if (OnActionExecuting(filterContext, sendUid, repository, sessionManager))
            {
                base.OnActionExecuting(filterContext);
            }
        }

        protected virtual bool OnActionExecuting(ActionExecutingContext filterContext, int? uid, IRepository repository, ISessionManager sessionManager)
        {
            return true;
        }
        
        protected ActionResult GetRedirect(HttpRequestBase request, string conrtollerName, string actionName)
        {
            return new RedirectResult(string.Format("{0}://{1}{2}/{3}/{4}", request.Url.Scheme, request.Url.Authority, request.ApplicationPath, conrtollerName, actionName));
        }

        protected RedirectToRouteResult GetRouteRedirect(string routeName)
        {
            return new RedirectToRouteResult(routeName, null);
        }
    }
}