// ------------------------------------------------------------------------
// <copyright file="LoginViewModel.js" company="feafarot">
//   Copyright © 2012 feafarot
// </copyright>
// ------------------------------------------------------------------------

function SignInViewModel()
{
    this.self = this;
    self.login = ko.observable("");
    self.password = ko.observable("");
    self.isInvalidUser = ko.observable(false);
    self.isBusy = ko.observable(false);
    self.changeTrigger = ko.computed(function ()
    {
        self.isInvalidUser(false);
        return self.login() + self.password();
    });
    self.signin = function ()
    {
        self.isBusy(true);
        authService.call(
            "SignIn",
            {
                login: login(),
                password: password()
            },
            function (result)
            {
                self.isBusy(false);
                if (eval(result))
                {
                    self.isInvalidUser(false);                    
                    navigate("Distributions");
                }
                else
                {
                    self.password("");
                    self.isInvalidUser(true);
                    $("#password").focus();
                }
            },
            function ()
            {
                self.isBusy(false);
            });
    };
}