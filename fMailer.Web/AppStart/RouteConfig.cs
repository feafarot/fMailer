// ------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Routing;

namespace fMailer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(RouteNmaes.Login, "Login", new { controller = "Auth", action = "Login" });
            routes.MapRoute(RouteNmaes.Default, "{controller}/{action}/{id}", new { controller = "Main", action = "Hub", id = UrlParameter.Optional });
        }
    }
}