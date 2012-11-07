// ------------------------------------------------------------------------
// <copyright file="MailTemplate.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MailTemplate : IUnique
    {
        public virtual int Id { get; set; }

        public virtual string Text { get; set; }

        // TODO: Add some options.
    }
}
