// ------------------------------------------------------------------------
// <copyright file="NavigationModel.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace fMailer.Web.Core.NonDomainModels
{
    public class NavigationModel
    {
        public HttpRequestBase Request { get; set; }

        public IList<NavigationItem> NavigationItems
        {
            get
            {
                return new List<NavigationItem> 
                {
                    new NavigationItem(RouteNames.Distributions, "Distributions", "~/Content/Images/Mail Out.png"),
                    new NavigationItem(RouteNames.Templates, "Templates", "~/Content/Images/Mail write.png"),
                    new NavigationItem(RouteNames.Contacts, "Contacts and Groups", "~/Content/Images/User-Message.png")
                };
            }
        }

        public string SelectedRouteName { get; set; }
    }
}