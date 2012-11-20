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

    // Modal part
    self.isBusy = ko.observable(false);
    self.currentContact = ko.observable({ FirstName: "", LastName: "", MiddleName: "", Groups: [] });
    self.selectedGroup = ko.observable({ Id: null, Name: "" });
    self.allGroups = ko.observableArray([]);
    self.contextGroups = ko.observableArray([]);
    self.selectedGroup = ko.observable(null);

    self.modalHeader = ko.observable("");
    
    // Main part
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
    };
    self.createNewContact = function ()
    {
        self.currentContact({ FirstName: "", LastName: "", MiddleName: "", Groups: [] });
        self.modalHeader("Create new contact");
        $("#contactsModal").modal(options);
    };
    self.addGroup = function ()
    {

    };

    self.loadContacts();
}