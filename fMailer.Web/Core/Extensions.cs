// ------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;

namespace fMailer.Web.Core
{
    public static class Extensions
    {
        public static User GetUser(this HttpRequestBase request)
        {
            int uid;
            if (!int.TryParse(request.Cookies["uid"].Value, out uid))
            {
                return null;
            }

            var repository = DependencyResolver.Current.GetService<IRepository>();            
            return repository.GetById<User>(uid);
        }
    }
}