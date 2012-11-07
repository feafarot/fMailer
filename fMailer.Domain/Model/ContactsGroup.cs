// ------------------------------------------------------------------------
// <copyright file="ContactsGroup.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ContactsGroup : IUnique
    {
        public virtual int Id { get; set; }

        public virtual IList<Contact> Contacts { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
