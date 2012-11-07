// ------------------------------------------------------------------------
// <copyright file="Utils.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Web.Core
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using fMailer.Web.Core.Settings;
    
    public static class Utils
    {
        public static HttpCookie CreateSessionsDependCookie(string name, string value)
        {
            var settings = DependencyResolver.Current.GetService<IMailerSettings>();
            return new HttpCookie(name, value) { Expires = DateTime.Now + settings.DefaultSessionLiveTime };
        }
    }
}