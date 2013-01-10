// ------------------------------------------------------------------------
// <copyright file="ContactsDomainController.cs" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using fMailer.Domain.DataAccess;
using fMailer.Domain.Model;
using fMailer.Web.Core;
using fMailer.Web.Core.HashProviders;
using fMailer.Web.Core.Settings;
using System.Threading;
using System.Text;
using System;
using System.Collections.Generic;

namespace fMailer.Web.Controllers.Domain
{
    public class ContactsDomainController : BaseController
    {
        public ContactsDomainController(IRepository repository, IMailerSettings settings, ISessionManager sessionManager)
            :base(repository, settings, sessionManager)
        {
        }

        [HttpPost]
        public JsonResult UpdateOrCreateContact(Contact contact)
        {
            if (contact.Id < 1)
            {
                contact.Id = 0;
                UpdateGroups(ref contact);
                User.AddContact(contact);
            }
            else
            {
                var realContact = Repository.GetById<Contact>(contact.Id);
                realContact.Email = contact.Email;
                realContact.FirstName = contact.FirstName;
                realContact.LastName = contact.LastName;
                realContact.MiddleName = contact.MiddleName;
                realContact.Groups.Clear();
                foreach (var item in contact.Groups)
                {
                    TryAddGroupToContact(realContact, item);
                }
            }

            return Json(true);  
        }

        [HttpPost]
        public JsonResult DeleteContact(Contact contact)
        {
            var temp = Repository.GetById<Contact>(contact.Id);
            Repository.Delete(temp);
            return Json(true);
        }

        [HttpPost]
        public JsonResult LoadContacts()
        {
            return Json(User.Contacts);
        }

        [HttpPost]
        public JsonResult LoadGroups()
        {
            return Json(User.ContactsGroups);
        }

        [HttpPost]
        public JsonResult ImportContacts(Attachment attachment)
        {
            var text = Encoding.UTF8.GetString(attachment.Content);
            var contacts = text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (contacts.Count < 2)
            {
                return Json(false);
            }

            var indexes = Indexes.IndexString(contacts[0]);
            contacts.RemoveAt(0);
            foreach (var contactText in contacts)
            {
                var parts = contactText.Split(new [] { "," }, StringSplitOptions.None);
                var contact = new Contact
                {
                    Email = parts[indexes.Email],
                    FirstName = parts[indexes.FirstName],
                    LastName = parts[indexes.LastName],
                    MiddleName = parts[indexes.MiddleName]
                };                
                if (User.Contacts.Any(x => x.Email == contact.Email))
                {
                    continue; 
                }

                var groups = parts[indexes.Grouops].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var groupName in groups)
                {
                    var group = User.ContactsGroups.FirstOrDefault(x => x.Name.ToLower() == groupName.ToLower());
                    if (group == null)
                    {
                        group = new ContactsGroup { Name = groupName };
                        User.AddContactsGroup(group);
                    }

                    contact.AddGroup(group);
                }

                User.AddContact(contact);
            }

            return Json(true);
        }

        private void TryAddGroupToContact(Contact contact, ContactsGroup group)
        {
            if (contact.Groups.FirstOrDefault(x => x.Id == group.Id) == null)
            {
                if (group.Id < 1)
                {
                    group.Id = 0;
                    User.AddContactsGroup(group);
                    contact.AddGroup(group);                 
                }
                else
                {
                    var realGroup = User.ContactsGroups.First(x => x.Id == group.Id);
                    contact.AddGroup(realGroup);
                }
            }
        }

        private void UpdateGroups(ref Contact contact)
        {
            if (contact.Groups == null)
            {
                return;
            }

            for (int i = 0; i < contact.Groups.Count; i++)
            {
                var id = contact.Groups[i].Id;
                if (id > 0)
                {
                    contact.Groups[i] = Repository.GetById<ContactsGroup>(id);
                }
                else
                {
                    contact.Groups[i].Id = 0;
                    User.AddContactsGroup(contact.Groups[i]);
                }
            }
        }

        private class Indexes
        {
            public int Email { get; set; }

            public int FirstName { get; set; }

            public int LastName { get; set; }

            public int MiddleName { get; set; }

            public int Grouops { get; set; }

            public static Indexes IndexString(string contact)
            {
                var index = new Indexes();
                var parts = contact.Split(new[] { "," }, StringSplitOptions.None).ToList();
                index.Email = parts.IndexOf("email");
                index.FirstName = parts.IndexOf("firstname");
                index.LastName = parts.IndexOf("lastname");
                index.MiddleName = parts.IndexOf("middlename");
                index.Grouops = parts.IndexOf("groups");
                return index;
            }
        }
    }
}
