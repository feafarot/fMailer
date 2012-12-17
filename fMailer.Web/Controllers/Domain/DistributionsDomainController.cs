// ------------------------------------------------------------------------
// <copyright file="DistributionsDomainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.HashProviders;
using fMailer.Web.Core.Settings;

namespace fMailer.Web.Controllers.Domain
{
    public class DistributionsDomainController : BaseController
    {
        public DistributionsDomainController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager)
            :base(repository, settings, sessionManager)
        {
        }

        [HttpPost]
        public JsonResult LoadDistributions()
        {
            return Json(User.Distributions);
        }
    }
}
