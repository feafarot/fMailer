// ------------------------------------------------------------------------
// <copyright file="NavigationItem.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fMailer.Web.Core.NonDomainModels
{
    public class NavigationItem
    {
        public NavigationItem(string routeName, string caption, string imageUrl)
        {
            RouteName = routeName;
            Caption = caption;
            ImageUrl = imageUrl;
        }

        public string ImageUrl { get; set; }

        public string RouteName { get; set; }

        public string Caption { get; set; }
    }
}