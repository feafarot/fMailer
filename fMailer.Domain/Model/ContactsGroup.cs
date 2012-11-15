// ------------------------------------------------------------------------
// <copyright file="ContactsGroup.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public class ContactsGroup : IUnique
    {
        public virtual int Id { get; set; }

        public virtual IList<Contact> Contacts { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        [ScriptIgnore]
        public virtual User User { get; set; }

        public virtual IList<Distribution> Distributions { get; set; }
    }
}
