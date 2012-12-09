// ------------------------------------------------------------------------
// <copyright file="Settings.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Settings : IUnique
    {
        public virtual int Id { get; set; }

        public virtual string Signature { get; set; }

        public virtual string EmailAddressFrom { get; set; }

        public virtual string Username { get; set; }

        public virtual string Password { get; set; }

        #region IMAP
        
        #endregion

        #region SMTP

        public virtual string SmtpAddress { get; set; }

        public virtual bool SmtpUseAuth { get; set; }

        public virtual bool SmtpUseSsl { get; set; }

        public virtual int? SmtpTlsPort { get; set; }

        public virtual int? SmtpSslPort { get; set; }

        #endregion

        #region POP3

        public virtual string Pop3Address { get; set; }

        public virtual int? Pop3Prot { get; set; }

        public virtual bool Pop3UseSsl { get; set; }

        #endregion
    }
}
