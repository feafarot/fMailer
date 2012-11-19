// ----------------------------------------------------------------------
// <copyright file="Contact.cs" company="feafarot">
//  Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public class Contact : IUnique
    {
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string MiddleName { get; set; }

        public virtual string Email { get; set; }

        public virtual IList<ContactsGroup> Groups { get; set; }

        public virtual IList<Distribution> Distributions { get; set; }

        [ScriptIgnore]
        public  virtual User User { get; set; }
    }
}
