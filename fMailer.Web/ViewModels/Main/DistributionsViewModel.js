// ------------------------------------------------------------------------
// <copyright file="DistributionsViewModel.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

function DistributionsViewModel()
{
    this.self = this;
    var options = { backdrop: "static", keyboard: false };
    var mapping = {
        key: function (item)
        {
            if (item != null) { return item.Id; }
            return -1;
        }
    };

    // Modal part
    self.currentDistr = ko.observable(self.clearDistr);
    self.isBusy = ko.observable(false);

    self.templates = ko.observableArray([]);
    self.selectedTemplate = ko.observable("");
    self.contextTemplates = ko.observableArray([""]);

    self.allGroups = ko.observableArray([]);
    self.contextGroups = ko.observableArray([""]);
    self.selectedGroup = ko.observable("");

    self.allContacts = ko.observableArray([]);
    self.contextContacts = ko.observableArray([""]);
    self.selectedContact = ko.observable("");
    self.addGroup = function ()
    {
        var existedInAllGroups = self.allGroups.FirstOrDefault(null, "$.Name() === self.selectedGroup()");
        var existedInContextGroups = self.contextGroups.FirstOrDefault(null, "$ === self.selectedGroup()");
        var existedInCurrentDistr = self.currentDistr().Groups.FirstOrDefault(null, "unwrapObs($).Name === self.selectedGroup()");

        if (existedInAllGroups != null && existedInContextGroups != null)
        {
            self.currentDistr().Groups.push(existedInAllGroups);
            self.contextGroups.remove(existedInAllGroups.Name());
        }
    };
    self.removeGroup = function (group)
    {
        self.currentDistr().Groups.remove(group);
        self.contextGroups.push(unwrapObs(group.Name));
    };
    self.addContact = function ()
    {
        var existedInAllContacts = self.allContacts.FirstOrDefault(null, "$.Email() === self.selectedContact()");
        var existedInContextContacts = self.contextContacts.FirstOrDefault(null, "$ === self.selectedContact()");
        var existedInCurrentDistr = self.currentDistr().Contacts.FirstOrDefault(null, "unwrapObs($).Email === self.selectedContact()");

        if (existedInAllContacts != null && existedInContextContacts != null)
        {
            self.currentDistr().Contacts.push(existedInAllContacts);
            self.contextContacts.remove(existedInAllContacts.Email());
        }
    };
    self.removeContact = function (contact)
    {
        self.currentDistr().Contacts.remove(contact);
        self.contextContacts.push(unwrapObs(contact.Email));
    };
    self.submitDistr = function ()
    {
        self.isBusy(true);
        self.currentDistr().Template = self.templates.First("$.Name() === self.selectedTemplate()");
        distributionsService.call(
            "SubmitDistribution",
            { distribution: unwrapObs(self.currentDistr) },
            function (response)
            {
                self.isBusy(false);
                self.loadDistrs();
                $("#distrModal").modal("toggle");
            });
    };
    self.closeDistr = function (distr)
    {
        if (!distr.IsClosed())
        {
            distributionsService.call(
                "CloseDistribution",
                { distribution: { Id: distr.Id() } },
                function (response)
                {
                    distr.IsClosed(true);
                });
        }
    };

    self.currentReply = ko.observable({ Subject: "", EmailText: "" });
    self.showReply = function (reply)
    {
        if (reply.IsNew())
        {
            distributionsService.call(
                "MarkReplyAsRead",
                { reply: unwrapObs(reply) },
                function (response)
                {
                    reply.IsNew(false);
                });
        }
        
        self.currentReply(reply);
        $("#replyModal").modal("show");
    };
    self.closeReply = function ()
    {
        $("#replyModal").modal("toggle");
    };


    // Main part
    self.clearDistr = { Id: 0, Name: "", Contacts: ko.observableArray([]), Groups: ko.observableArray([]), Template: { Id: 0, Name: "" } };
    self.distrs = ko.observableArray([]);
    self.loadDistrs = function ()
    {
        $("#loadingModal").modal(options);
        distributionsService.call(
            "LoadDistributions",
            null,
            function (response)
            {
                ko.mapping.fromJS(response, mapping, self.distrs);
                $("#loadingModal").modal("toggle");
            });
    };
    self.loadGroups = function ()
    {
        contactsService.call(
            "LoadGroups",
            null,
            function (response)
            {
                ko.mapping.fromJS(response, mapping, self.allGroups);
            });
    };
    self.loadContacts = function ()
    {
        contactsService.call(
            "LoadContacts",
            null,
            function (response)
            {
                ko.mapping.fromJS(response, mapping, self.allContacts);
            });
    };
    self.loadTemplates = function ()
    {
        templatesService.call(
            "LoadPureTemplates",
            null,
            function (response)
            {
                ko.mapping.fromJS(response, mapping, self.templates);
            });
    };
    self.createDistr = function ()
    {
        self.currentDistr(self.clearDistr);
        self.contextGroups(self.allGroups.Select("$.Name()"));
        self.contextContacts(self.allContacts.Select("$.Email()"));
        self.contextTemplates(self.templates.Select("$.Name()"));
        self.selectedGroup("");
        self.selectedContact("");
        $("#distrModal").modal(options);
    };

    self.loadDistrs();
    self.loadContacts();
    self.loadGroups();
    self.loadTemplates();
}