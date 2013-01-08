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
        public Contact()
        {
            Groups = new List<ContactsGroup>();
            Distributions = new List<Distribution>();
        }

        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string MiddleName { get; set; }

        public virtual string Email { get; set; }

        public virtual IList<ContactsGroup> Groups { get; set; }

        [ScriptIgnore]
        public virtual IList<Distribution> Distributions { get; set; }

        [ScriptIgnore]
        public virtual User User { get; set; }

        public virtual void AddGroup(ContactsGroup group)
        {
            Groups.Add(group);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Contact))
            {
                return false;
            }

            return ((Contact)obj).ToString() == this.ToString();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(MiddleName) 
                ? string.Format("{0} {1} <{2}>", LastName, FirstName, Email)
                : string.Format("{0} {1} {2} <{3}>", LastName, FirstName, MiddleName, Email);
        }
    }
}
