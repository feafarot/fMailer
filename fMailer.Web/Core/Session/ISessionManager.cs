// ------------------------------------------------------------------------
// <copyright file="ISessionManager.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Web.Core
{
    using System.Web;
    using fMailer.Domain.Model;

    public interface ISessionManager
    {
        string BeginSession(User user);

        string BeginSession(int userId);
        
        void UpdateSessionCookies(ref HttpCookieCollection cookieCollection);

        bool EndSession(int uid, string sid);

        bool EndSession(HttpCookieCollection cookieCollection);

        bool ValidateSession(int uid, string sid);

        bool ValidateSession(HttpCookieCollection cookieCollection);

        bool UpdateSession(int uid, string sid, bool validate = false);

        bool UpdateSession(HttpCookieCollection cookieCollection, bool validate = false);
    }
}