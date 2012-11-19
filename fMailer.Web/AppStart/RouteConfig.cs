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
            routes.MapRoute(RouteNames.Contacts, "Contacts", new { controller = "Main", action = "Contacts" });
            routes.MapRoute(RouteNames.Templates, "Templates", new { controller = "Main", action = "Templates" });
            routes.MapRoute(RouteNames.Distributions, "Distributions", new { controller = "Main", action = "Distributions" });
            routes.MapRoute(RouteNames.SignOff, "SignOff", new { controller = "Auth", action = "SignOff" });
            routes.MapRoute(RouteNames.SignIn, "SignIn", new { controller = "Auth", action = "SignIn" });
            routes.MapRoute(RouteNames.Default, "{controller}/{action}/{id}", new { controller = "Main", action = "Hub", id = UrlParameter.Optional });
        }
    }
}