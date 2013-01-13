// ------------------------------------------------------------------------
// <copyright file="helpers.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

var windowURL = window.URL || window.webkitURL;

function trackable(item)
{
}

function unwrapObs(observable)
{
    return ko.utils.unwrapObservable(observable);
}

function htmlEncode(value)
{
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function htmlDecode(value)
{
    return $('<div/>').html(value).text();
}

function getLocation()
{
    var isLocal = true;
    return isLocal ? window.location.protocol + "//" + window.location.hostname + "/fmailer" : window.location.protocol + "//" + window.location.hostname;
}

function navigate(url)
{
    window.location.href = getLocation() + "/" + url;
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
        var value = valueAccessor();
        var elem = $(element);
        var options = value.options;

        // Setup Bootstrap Typeahead for this element.
        elem.typeahead(
        {
            source: function () { return ko.utils.unwrapObservable(value.source); },
            onselect: function (val)
            {
                value.target(val);
            }
        });

        // Set the value of the target when the field is blurred.
        elem.blur(function ()
        {
            value.target(elem.val());
            if (options == null || options.clearOnBlur == null || options.clearOnBlur == true)
            {
                elem.val("");
            }
        });
        elem.keypress(function (event)
        {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13)
            {
                elem.blur();
                elem.focus();
                if (options == null || options.clearOnBlur == null || options.clearOnBlur == true)
                {
                    elem.val("");
                }

                return false;
            }

            return true;
        });
    },
    update: function (element, valueAccessor)
    {
        var elem = $("#" + element.id);
        var value = valueAccessor();
        elem.val(value.target());
    }
};

ko.bindingHandlers.advancedEditor = {
    init: function (element, valueAccessor, allBindingsAccessor)
    {
        var value = valueAccessor();
        var elem = $("#" + element.id);
        elem.wysihtml5({
            "font-styles": true, //Font styling, e.g. h1, h2, etc. Default true
            "emphasis": true, //Italics, bold, etc. Default true
            "lists": true, //(Un)ordered lists, e.g. Bullets, Numbers. Default true
            "html": true, //Button which allows you to edit the generated HTML. Default false
            "link": true, //Button to insert a link. Default true
            "image": true, //Button to insert an image. Default true,
            "color": true, //Button to change color of font
            "events":
                {
                    "change": changeHandler,
                    "load": function ()
                    {
                        elem[0].isloaded = true;
                    }
                }
        });

        function changeHandler()
        {
            value.target(escape(elem.val().replace(/"/g, "†qte†").replace(/=/g, "†rvn†")));
        }
    },
    update: function (element, valueAccessor)
    {
        var elem = $("#" + element.id);
        var interval = window.setInterval(
            function ()
            {
                if (elem[0].isloaded === true)
                {
                    var value = valueAccessor();
                    elem.data("wysihtml5").editor.setValue(unescape(value.target()).replace(/†qte†/g, '"').replace(/†rvn†/g, '='));
                    ko.bindingHandlers.value.update(element, valueAccessor);
                    window.clearInterval(interval);
                }
            },
            100);
        if (elem[0].isloaded === true)
        {
            var value = valueAccessor();
            elem.data("wysihtml5").editor.setValue(unescape(value.target()).replace(/†qte†/g, '"').replace(/†rvn†/g, '='));
            ko.bindingHandlers.value.update(element, valueAccessor);
            window.clearInterval(interval);
        }
    }
};

ko.bindingHandlers.file = {
    init: function (element, valueAccessor)
    {
        $(element).change(function ()
        {
            var file = this.files[0];
            if (ko.isObservable(valueAccessor()))
            {
                valueAccessor()(file);
            }
        });
    },

    update: function (element, valueAccessor, allBindingsAccessor)
    {
        var file = ko.utils.unwrapObservable(valueAccessor());
        var bindings = allBindingsAccessor();

        if (bindings.fileObjectURL && ko.isObservable(bindings.fileObjectURL))
        {
            var oldUrl = bindings.fileObjectURL();
            if (oldUrl)
            {
                windowURL.revokeObjectURL(oldUrl);
            }
            bindings.fileObjectURL(file && windowURL.createObjectURL(file));
        }

        if (bindings.fileBinaryData && ko.isObservable(bindings.fileBinaryData))
        {
            if (!file)
            {
                bindings.fileBinaryData(null);
            } else
            {
                var reader = new FileReader();
                reader.onload = function (e)
                {
                    bindings.fileBinaryData(e.target.result);
                };
                reader.readAsArrayBuffer(file);
            }
        }
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


function getArrayFromUint8Array(array)
{
    var result = [array.length];
    for (var i = 0; i < array.length; i++)
    {
        result[i] = array[i];
    }

    return result;
}