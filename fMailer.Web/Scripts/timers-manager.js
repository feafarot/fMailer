// ------------------------------------------------------------------------
// <copyright file="timers-manager.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

var timersManager = (function ()
{
    var timersIds = [];
    var defferedCallsIds = [];
    var instance =
        {
            startTimer: function (func, interval, callFirst)
            {
                if (callFirst)
                {
                    func();
                }

                var timerId = window.setInterval(func, interval);
                timersIds.push(timerId);
                return timerId;
            },
            setDefferedCall: function (func, interval)
            {
                var defferedCallId = window.setTimeout(func, interval);
                defferedCallsIds.push(defferedCallId);
                return defferedCallId;
            },
            closeTimer: function (timerId)
            {
                window.clearInterval(timerId);
                timersIds.slice(timersIds.indexOf(timerId), 1);
            },
            closeDefferedCall: function (defferedCallId)
            {
                window.clearTimeout(defferedCallId);
                defferedCallsIds.slice(defferedCallsIds.indexOf(defferedCallId));
            },
            closeAllTimers: function ()
            {
                for (var i = 0; i < timersIds.length; i++)
                {
                    window.clearInterval(timersIds[i]);
                }

                timersIds = [];
            },
            closeAllDefferedCalls: function ()
            {
                for (var i = 0; i < defferedCallsIds.length; i++)
                {
                    window.clearTimeout(defferedCallsIds[i]);
                }

                defferedCallsIds = [];
            },
            closeAll: function ()
            {
                instance.closeAllTimers();
                instance.closeAllDefferedCalls();
            }
        };

    return instance;
})();