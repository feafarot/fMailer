// ------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core.Settings;

namespace fMailer.Web.Core
{
    public class SessionManager : ISessionManager
    {
        private readonly IRepository repository;

        private readonly IMailerSettings settings;

        public SessionManager(IRepository repository, IMailerSettings settings)
        {
            this.repository = repository;
            this.settings = settings;
        }

        public string BeginSession(User user)
        {
            return BeginSession(user.Id);
        }

        public string BeginSession(int userId)
        {
            var sessions = repository.GetAll<Session>();
            if (!settings.AllowMultipleLogins)
            {
                foreach (var session in sessions.Where(session => !session.Outdated && session.User.Id == userId))
                {
                    session.Outdated = true;
                }
            }

            var user = repository.GetById<User>(userId);
            //user.IsOnline = true;
            var newSession = new Session
                                 {
                                     User = user,
                                     Outdated = false,
                                     SessionGuid = Guid.NewGuid(),
                                     StartedAt = DateTime.Now
                                 };
            repository.Add(newSession);
            repository.Submit();
            return newSession.SessionGuid.ToString();
        }

        public void UpdateSessionCookies(ref HttpCookieCollection cookieCollection)
        {
            var uidCookie = cookieCollection["uid"];
            var sidCookie = cookieCollection["sid"];
            if (sidCookie != null && uidCookie != null)
            {
                sidCookie.Expires = DateTime.Now + settings.DefaultSessionLiveTime;
                uidCookie.Expires = DateTime.Now + settings.DefaultSessionLiveTime;
            }
        }

        public bool EndSession(int uid, string sid)
        {
            var session = GetSessioinByGuid(sid);
            if (!ValidateSession(uid, session))
            {
                return false;
            }

            var user = repository.GetById<User>(uid);
            if (user == null)
            {
                return false;
            }

            //user.IsOnline = false;
            session.Outdated = true;
            repository.Submit();
            return true;
        }

        public bool EndSession(HttpCookieCollection cookieCollection)
        {
            int uid;
            var uidCookie = cookieCollection["uid"];
            var sidCookie = cookieCollection["sid"];
            if (uidCookie == null || !int.TryParse(uidCookie.Value, out uid) || sidCookie == null)
            {
                return false;
            }

            return EndSession(uid, sidCookie.Value);
        }

        public bool ValidateSession(int uid, string sid)
        {
            try
            {
                var user = repository.GetById<User>(uid);
                if (user == null)
                {
                    return false;
                }

                var session = GetSessioinByGuid(sid);
                return ValidateSession(user.Id, session);
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateSession(HttpCookieCollection cookieCollection)
        {
            int uid;
            var uidCookie = cookieCollection["uid"];
            var sidCookie = cookieCollection["sid"];
            if (uidCookie == null || !int.TryParse(uidCookie.Value, out uid) || sidCookie == null)
            {
                return false;
            }

            var validateResult = ValidateSession(uid, sidCookie.Value);
            if (!validateResult)
            {
                cookieCollection["uid"].Value = string.Empty;
                cookieCollection["sid"].Value = string.Empty;
            }

            return validateResult;
        }

        public bool UpdateSession(int uid, string sid, bool validate = false)
        {
            if (validate)
            {
                if (!ValidateSession(uid, sid))
                {
                    return false;
                }
            }

            var session = GetSessioinByGuid(sid);
            if (session.Outdated || session.User.Id != uid)
            {
                return false;
            }

            session.StartedAt = DateTime.Now;
            repository.Submit();
            return true;
        }

        public bool UpdateSession(HttpCookieCollection cookieCollection, bool validate = false)
        {
            int uid;
            var uidCookie = cookieCollection["uid"];
            var sidCookie = cookieCollection["sid"];
            if (uidCookie == null || !int.TryParse(uidCookie.Value, out uid) || sidCookie == null)
            {
                return false;
            }

            return UpdateSession(uid, sidCookie.Value, validate);
        }

        private Session GetSessioinByGuid(string guid)
        {
            var session = repository.GetAll<Session>().SingleOrDefault(x => x.SessionGuid.ToString() == guid);
            return session;
        }

        private bool ValidateSession(int userId, Session session)
        {
            if (session == null)
            {
                return false;
            }

            if (userId != session.User.Id || session.StartedAt + settings.DefaultSessionLiveTime < DateTime.Now || session.Outdated)
            {
                return false;
            }

            return true;
        }
    }
}