// ------------------------------------------------------------------------
// <copyright file="TemplatesViewModel.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

function TemplatesViewModel()
{
    this.self = this;

    var mapping = {
        key: function (template)
        {
            if (template != null) { return template.Id; }
            return -1;
        }
    };

    var options = { backdrop: "static", keyboard: false };
    self.clerAttachment = { Id: 0, Content: ko.observable(), ContentType: ko.observable(""), Size: 0, Name: ko.observable() };
    self.clearTemplate = { Id: 0, Name: "", Text: ko.observable(""), Subject: "", Attachments: ko.observableArray([]) };

    self.imageFile = ko.observable();
    self.imageObjectURL = ko.observable();
    self.imageBinary = ko.observable();

    self.deleteCandidate = ko.observable(clearTemplate);
    self.deleteCandidateName = ko.observable("");
    self.modalHeader = ko.observable("");
    self.currentTemplate = ko.observable(clearTemplate);
    self.currentTemplateText = ko.observable("");
    self.templates = ko.observableArray([]);
    self.isBusy = ko.observable(false);

    self.addAttachment = function ()
    {
        self.currentTemplate().Attachments.push({
            Id: 0,
            Name: self.imageFile().name,
            ContentType: self.imageFile().type,
            Content: getArrayFromUint8Array(new Uint8Array(self.imageBinary())),
            Size: self.imageFile().size
        });
        self.imageFile(null);
        $("#upload").fileupload('reset');
    };
    self.removeAttachment = function (attachment)
    {
        self.currentTemplate().Attachments.remove(attachment);
    };
    self.openCreateTemplateModal = function ()
    {
        self.currentTemplate(clearTemplate);
        self.modalHeader("Create new template");
        $("#newTemplateModal").modal(options);
    };
    self.editTemplate = function (template)
    {
        self.currentTemplate(template);
        self.modalHeader("Edit template");
        $("#newTemplateModal").modal(options);
    };
    self.saveChanges = function ()
    {
        self.isBusy(true);
        templatesService.call(
            "UpdateTemplate",
            { template: unwrapObs(self.currentTemplate) },
            function (response)
            {
                self.loadTemplates();
                $('#newTemplateModal').modal("toggle")
                self.isBusy(false);
            });
    };
    self.cancelChanges = function ()
    {
        $('#newTemplateModal').modal("toggle")
    };
    self.loadTemplates = function ()
    {
        templatesService.call(
            "LoadTemplates",
            null,
            function (response)
            {
                ko.mapping.fromJS(response, mapping, self.templates);
            });
    };
    self.deleteTemplate = function (template)
    {
        self.deleteCandidate(template);
        self.deleteCandidateName(template.Name);
        $("#confirmationModal").modal(options);
    };
    self.cancelDelete = function ()
    {
        self.deleteCandidate = null;
        self.deleteCandidateName("");
    };
    self.confirmDelete = function ()
    {
        self.isBusy(true);
        templatesService.call(
            "DeleteTemplate",
            { template: unwrapObs(self.deleteCandidate) },
            function (response)
            {
                self.loadTemplates();
                $("#confirmationModal").modal("toggle");
                self.deleteCandidateName("");
                self.isBusy(false);
            });
    };


    self.loadTemplates();
}