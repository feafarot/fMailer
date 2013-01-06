// ------------------------------------------------------------------------
// <copyright file="services-manager.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

var authService = new function ()
{
    var extended = $.extend({}, new baseService("AuthDomain"));
    return extended;
};

var templatesService = new function ()
{
    var extended = $.extend({}, new baseService("TemplatesDomain"));
    return extended;
};

var contactsService = new function ()
{
    var extended = $.extend({}, new baseService("ContactsDomain"));
    return extended;
};

var distributionsService = new function ()
{
    var extended = $.extend({}, new baseService("DistributionsDomain"));
    return extended;
};

function baseService(service)
{
    var serviceName = service;
    var instance =
        {
            call: function (method, data, successCall, errorCall)
            {
                // !TODO: BE CARE WITH DEPLOYMENT!!! in 'getLocation' method should be passed 'false' on real server.
                callService(getLocation(true) + "/" + serviceName, method, data, successCall, errorCall);
            }
        };

    return instance;
};

function callService(serviveLink, method, data, successCall, errorCall)
{
    this.self = this;
    self.errorCall = errorCall;
    $.ajax(
        {
            url: serviveLink + "/" + method,
            data: ko.toJSON(data),
            type: "POST",
            processData: true,
            success: successCall,
            error: function (jqXHR, textStatus, errorThrown)
            {
                if (self.errorCall != null)
                {
                    self.errorCall();
                }

                timersManager.closeAll();
                if (textStatus != null)
                {
                    if (true)
                    {
                        var generator = window.open('', 'Server Error', 'width=1024,height=800,toolbar=no,location=yes,directories=yes,status=yes,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes');
                        generator.document.write(jqXHR.responseText);
                        generator.document.close();
                    }
                    else
                    {
                        $("#errorModal").modal({ backdrop: true });
                    }
                }
            }
        });
};