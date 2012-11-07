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

        // TODO: Add properties to configure Mail server
        #region IMAP



        #endregion

        #region SMTP



        #endregion

        #region POP3



        #endregion
    }
}
