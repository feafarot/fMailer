using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fMailer.Web.Core.Helpers
{
    public static class CookiesHelper
    {
        public static int GetUserId(this HttpCookieCollection cookies)
        {
            var uidCookie = cookies["uid"];
            int uid;
            if (uidCookie != null && int.TryParse(uidCookie.Value, out uid))
            {
                return uid;
            }

            throw new KeyNotFoundException("Can not find user identity");
        }
    }
}