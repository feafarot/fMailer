﻿// ------------------------------------------------------------------------
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
        public JsonResult LoadContacts()
        {
            return Json(User.Contacts);
        }

        [HttpPost]
        public JsonResult LoadGroups()
        {
            return Json(User.ContactsGroups);
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
    }
}