// ------------------------------------------------------------------------
// <copyright file="ContactsViewModel.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

function ContactsViewModel()
{
    this.self = this;
    var cmapping =
    {
        key: function (contact)
        {
            if (contact != null)
            {
                return contact.Id;
            }

            return -1;
        }
    };
    var options = { backdrop: "static", keyboard: false };

    self.isBusy = ko.observable(false);
    self.currentContact = ko.observable({});
    self.modalHeader = ko.observable("");
    self.contacts = ko.observableArray([]);
    self.loadContacts = function ()
    {
        contactsService.call(
            "LoadContacts",
            null,
            function (response)
            {
                ko.mapping.fromJS(response, cmapping, self.contacts);
            });
    };
    self.saveChanges = function ()
    {
    }
    self.createNewContact = function ()
    {
        self.modalHeader("Create new contact");
        $("#contactsModal").modal(options);
    }

    self.loadContacts();
}