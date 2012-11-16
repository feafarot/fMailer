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
            if (template != null)
            {
                return template.Id;
            }

            return -1;
        }
    };
    var options = { backdrop: "static", keyboard: false };
    self.deleteCandidate = null;
    self.deleteCandidateName = ko.observable("");
    self.modalHeader = ko.observable("");
    self.currentTemplate = ko.observable({ Id: 0, Name: "", Text: "", Description: "" });
    self.templates = ko.observableArray([]);
    self.isBusy = ko.observable(false);
    self.openCreateTemplateModal = function ()
    {
        self.currentTemplate({ Id: 0, Name: "", Text: "", Description: "" });
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
            { template: ko.toJSON(currentTemplate) },
            function (response)
            {
                self.loadTemplates();
                $('#newTemplateModal').modal("toggle")
                self.isBusy(false);
            });
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
        self.deleteCandidate = template;
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
            { template: ko.toJSON(self.deleteCandidate) },
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