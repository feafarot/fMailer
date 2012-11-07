// ------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;

namespace fMailer.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}