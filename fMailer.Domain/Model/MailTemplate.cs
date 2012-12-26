// ------------------------------------------------------------------------
// <copyright file="MailTemplate.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public class MailTemplate : IUnique
    {
        public virtual int Id { get; set; }

        public virtual string Text { get; set; }

        public virtual string Name { get; set; }

        public virtual string Subject { get; set; }

        [ScriptIgnore]
        public virtual IList<Distribution> Distributions { get; set; }

        [ScriptIgnore]
        public virtual User User { get; set; }
    }
}
