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
    self.clearContact = { FirstName: "", LastName: "", MiddleName: "", Email: "", Groups: ko.observableArray([]) };
    self.currentContact = ko.observable(self.clearContact);
    self.selectedGroup = ko.observable({ Id: null, Name: "" });
    self.allGroups = ko.observableArray([]);
    self.contextGroups = ko.observableArray([""]);
    self.selectedGroup = ko.observable("");
    self.rbcContact = ko.observable({});
    self.modalHeader = ko.observable("");

    self.deleteCandidate = ko.observable(self.clearContact);
    self.deleteCandidateName = ko.observable("");

    // Import part
    self.imageFile = ko.observable();
    self.imageObjectURL = ko.observable();
    self.imageBinary = ko.observable();
    self.importContacts = function ()
    {
        $("#loadingModal").modal(options);
        contactsService.call(
            "ImportContacts",
            { attachment:
                  {
                    Id: 0,
                    Name: self.imageFile().name,
                    ContentType: self.imageFile().type,
                    Content: getArrayFromUint8Array(new Uint8Array(self.imageBinary())),
                    Size: self.imageFile().size
                }
            },
            function (response)
            {
                self.loadContacts();
                self.loadGroups();
                $("#loadingModal").modal("toggle");
            });
        self.imageFile(null);
    }

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
    self.loadGroups = function ()
    {
        contactsService.call(
            "LoadGroups",
            null,
            function (response)
            {
                ko.mapping.fromJS(response, cmapping, self.allGroups);
            });
    };

    self.cancelEdit = function ()
    {
        self.loadContacts();
        self.loadGroups();
        self.currentContact(self.rbcContact);
        $('#contactsModal').modal("toggle");
    }
    self.saveChanges = function ()
    {
        self.isBusy(true);
        contactsService.call(
            "UpdateOrCreateContact",
            { contact: unwrapObs(self.currentContact) },
            function (response)
            {
                self.loadContacts();
                self.loadGroups();
                $('#contactsModal').modal("toggle");
                self.isBusy(false);
            });
    };
    self.createNewContact = function ()
    {
        self.currentContact(self.clearContact);
        self.contextGroups(self.allGroups.Select("$.Name()"));
        self.selectedGroup("");
        self.modalHeader("Create new contact");
        $("#contactsModal").modal(options);
    };
    self.editContact = function (contact)
    {
        self.currentContact(contact);
        self.rbcContact(jQuery.extend({}, contact));
        self.contextGroups(self.allGroups.Select("$.Name()"));
        for (var i = 0; i < unwrapObs(contact.Groups).length; i++)
        {
            self.contextGroups.remove(unwrapObs(contact.Groups)[i].Name());
        }

        self.modalHeader("Edit contact");
        $("#contactsModal").modal(options);
    };
    self.addGroup = function ()
    {
        var existedInAllGroups = self.allGroups.FirstOrDefault(null, "$.Name() === self.selectedGroup()");
        var existedInContextGroups = self.contextGroups.FirstOrDefault(null, "$ === self.selectedGroup()");
        var existedInCurrentConttact = self.currentContact().Groups.FirstOrDefault(null, "unwrapObs($).Name === self.selectedGroup()");

        if (existedInAllGroups != null && existedInContextGroups != null)
        {
            self.currentContact().Groups.push(existedInAllGroups);
            self.contextGroups.remove(existedInAllGroups.Name());
        }
        else if (existedInCurrentConttact == null)
        {
            self.currentContact().Groups.push({ Id: 0, Name: self.selectedGroup() });
        }
    };
    self.removeGroup = function (group)
    {
        self.currentContact().Groups.remove(group);
        self.contextGroups.push(unwrapObs(group.Name));
    };

    self.deleteContact = function (contact)
    {
        self.deleteCandidate(contact);
        self.deleteCandidateName(unwrapObs(contact.LastName) + " " + unwrapObs(contact.FirstName) + " " + unwrapObs(contact.MiddleName));
        $("#confirmationModal").modal(options);
    };
    self.cancelDelete = function ()
    {
        self.deleteCandidate(self.clearContact);
        self.deleteCandidateName("");
    };
    self.confirmDelete = function ()
    {
        self.isBusy(true);
        contactsService.call(
            "DeleteContact",
            { contact: unwrapObs(self.deleteCandidate) },
            function (response)
            {
                self.loadContacts();
                self.loadGroups();
                $("#confirmationModal").modal("toggle");
                self.deleteCandidateName("");
                self.isBusy(false);
            });
    };

    self.loadContacts();
    self.loadGroups();
}