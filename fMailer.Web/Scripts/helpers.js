// ------------------------------------------------------------------------
// <copyright file="helpers.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

function getLocation(isLocal)
{
    return isLocal ? window.location.protocol + "//" + window.location.hostname + "/fmailer" : window.location.protocol + "//" + window.location.hostname;
}

function navigate(url)
{
    window.location.href = getLocation(true) + "/" + url;
}

function _(property)
{
    if (property.name == "observable") 
    {
        return property();
    }
    else
    {
        return property;
    }
}

ko.bindingHandlers.accordion = 
{
    init: function (element, valueAccessor)
    {
        var options = valueAccessor() || {};
        setTimeout(
            function ()
            {
                $(element).accordion(options);
            }, 
            0);

        ko.utils.domNodeDisposal.addDisposeCallback(
            element, 
            function ()
            {
                $(element).accordion("destroy");
            });
    },
    update: function (element, valueAccessor)
    {
        var options = valueAccessor() || {};
        $(element).accordion("destroy").accordion(options);
    }
}

ko.bindingHandlers.enterPress = 
{
    init: function (element, valueAccessor, allBindingsAccessor, viewModel)
    {
        var allBindings = allBindingsAccessor();
        $(element).keypress(function (event)
        {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13)
            {
                allBindings.enterPress.call(viewModel);
                return false;
            }
            return true;
        });
    }
};

$.toJson = function (object)
{
    return JSON.stringify(object);
}