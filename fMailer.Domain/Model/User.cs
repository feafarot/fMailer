// ------------------------------------------------------------------------
// <copyright file="User.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

namespace fMailer.Domain.Model
{
    using System;
    using System.Collections.Generic;

    public class User : IUnique
    {
        public User()
        {
            Contacts = new List<Contact>();
            ContactsGroups = new List<ContactsGroup>();
            Distributions = new List<Distribution>();
            Templates = new List<MailTemplate>();
        }

        public virtual int Id { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual string Email { get; set; }

        public virtual IList<ContactsGroup> ContactsGroups { get; set; }

        public virtual IList<Contact> Contacts { get; set; }

        public virtual IList<MailTemplate> Templates { get; set; }

        public virtual IList<Distribution> Distributions { get; set; }

        public virtual IList<Session> Sessions { get; set; }

        public virtual Settings Settings { get; set; }

        public virtual void AddTemplate(MailTemplate template)
        {
            template.User = this;
            Templates.Add(template);
        }

        public virtual void AddContact(Contact contact)
        {
            contact.User = this;
            Contacts.Add(contact);
        }

        public virtual void AddContactsGroup(ContactsGroup group)
        {
            group.User = this;
            ContactsGroups.Add(group);
        }
    }
}
