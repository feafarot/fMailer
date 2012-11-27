// ------------------------------------------------------------------------
// <copyright file="helpers.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

function unwrapObs(observable)
{
    return ko.utils.unwrapObservable(observable);
}

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

ko.observableArray.fn.Where = function (predicate)
{
    return Enumerable.From(this())
           .Where(predicate)
           .ToArray();
};
ko.observableArray.fn.Select = function (predicate)
{
    return Enumerable.From(this())
           .Select(predicate)
           .ToArray();
};
ko.observableArray.fn.First = function (predicate)
{
    return Enumerable.From(this())
           .First(predicate);
};
ko.observableArray.fn.FirstOrDefault = function (def, predicate)
{
    return Enumerable.From(this())
           .FirstOrDefault(def, predicate);
};

ko.observable.fn.track = function ()
{
    var cache = ko.observable(this()),
        self = this;

    this.commit = function ()
    {
        cache(self());
    };
    this.cancel = function ()
    {
        self(cache());
    };
    this.hasChanges = ko.computed(function ()
    {
        return self() !== cache();
    });
    return this;
};
//ko.fn.trackedObservable = function (val)
//{
//    return ko.observable(val).track();
//};

// Use like so: data-bind="typeahead: { target: selectedNamespace, source: namespaces }"
// Thanks  to: http://blogs.msdn.com/b/rebond/archive/2012/07/18/knockout-js-binding-for-bootstrap-typeahead-plugin.aspx
ko.bindingHandlers.typeahead =
{
    init: function (element, valueAccessor)
    {
        var binding = this;
        var value = valueAccessor();

        this.self = this;
        self.elem = $(element);

        // Setup Bootstrap Typeahead for this element.
        self.elem.typeahead(
        {
            source: function () { return ko.utils.unwrapObservable(value.source); },
            onselect: function (val)
            {
                value.target(val);
            }
        });

        // Set the value of the target when the field is blurred.
        self.elem.blur(function ()
        {
            value.target(self.elem.val());
            self.elem.val("");
        });
        self.elem.keypress(function (event)
        {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13)
            {
                self.elem.blur();
                self.elem.focus();
                self.elem.val("");
                return false;
            }

            return true;
        });
    },
    update: function (element, valueAccessor)
    {
        var elem = $(element);
        var value = valueAccessor();
        elem.val(value.target());
    }
};

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