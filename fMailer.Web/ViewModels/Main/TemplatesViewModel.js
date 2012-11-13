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
    self.modalHeader = ko.observable("");
    self.currentTemplate = ko.observable({ Id: 0, Name: "", Text: "", Description: "" });
    self.templates = ko.observableArray([]);
    self.openCreateTemplateModal = function ()
    {
        self.currentTemplate({ Id: 0, Name: "", Text: "", Description: "" });
        self.modalHeader("Add Template");
        $("#newTemplateModal").modal({ backdrop: true, keyboard: false });
    };
    self.saveChanges = function ()
    {
        templatesService.call(
            "UpdateTemplate",
            { template: currentTemplate() },
            function (response)
            {

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

    self.loadTemplates();
}