// ------------------------------------------------------------------------
// <copyright file="IMailerSettings.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System;

namespace fMailer.Web.Core.Settings
{
    public interface IMailerSettings
    {
        TimeSpan DefaultSessionLiveTime { get; }

        bool UsePasswordsHashing { get; }

        string FirstNameKeyword { get; }

        string LastNameKeyword { get; }

        string MiddleNameKeyword { get; }

        string FullNameKeyword { get; }
    }
}