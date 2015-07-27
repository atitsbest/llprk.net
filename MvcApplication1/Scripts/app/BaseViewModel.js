/**
 * Base "class" for all ViewModels. 
 *
 * @example
 *  function IrgendeinViewModel(data) {
 *      _.extend(this, new BaseViewModel());
 *      var self = this;
 *      ...
 * }
 *
 */ 
define("app/BaseViewModel", ["knockout", "knockout.mapping", "jquery", "toastr"], function (ko, mapping, $, toastr) {
    function BaseViewModel(model) {
        var self = this;

        // Validation
        must(model, validator("Must be a map", aMap));

        /**
         * Handles an form post error, sets validation on http 400, ... 
         *
         * @param jqxhr The XHR object from the failure (first argument of $.fail).
         * 
         * @param model The model with the Knockout-Observables of the form
         * .
         */
        self.handleFormFailure = function (jqxhr) {
            if (jqxhr.status === 400) {
                var modelstate = JSON.parse(jqxhr.responseText);
                if (existy(modelstate)) {
                    var keys = _.keys(modelstate);
                    for (var k = 0; k < keys.length; k += 1) {
                        var key = keys[k];
                        var errors = modelstate[key].errors;
                        
                        if (errors && errors.length > 0) {
                            var p = model[key];
                            //p.extend({ __dataAnnotations__: true });
                            p.setError(errors[0].ErrorMessage);
                            p.isModified(true);
                        }
                    }
                }
            }
        };

        /**
         *
         */
        self.handleServerError = function (jqxhr) {
            if (jqxhr.status === 500) {
                var msg = jqxhr.responseText;
                if (jqxhr.responseJSON) {
                    msg = jqxhr.responseJSON.Message;
                }
                toastr.error(msg);
            }
        };


        /**
         * Handles an EntityResult (Show Message and redirect)
         *
         * @param result {object} of Message and RedirectUrl.
         * @param alertFn {function} Function to display message (default: toastr.success
         * )
         */
        self.handleEntityResult = function (result, alertFn) {
       
            alertFn = alertFn || toastr.success;

            if (result) {
                if (_.isString(result.Message) && existy(alertFn)) {
                    var sanitizedMsg = result.Message.replace(/\\n/g, '\n');
                    alertFn(sanitizedMsg);
                }
                if (_.isString(result.RedirectUrl)) {
                    window.location.href = result.RedirectUrl;
                }
            }
        };

        /**
         * Base function for submitting a from to the server and handle the response.
         * 
         * @param form The Html-Form-Element.
         * @param model {object} The konockout model that is posted to the server.
         *
         */
        self.submit = function (form,successFn) {
            var data = mapping.toJS(model);
            function success(result) {
                self.handleEntityResult(result);
                
                if (existy(successFn)) {
                    successFn();
                }
            }
            

            function fail(result) {
                self.handleFormFailure(result);
                self.handleServerError(result);              
            }

            // Any <input type="file">?
            var $file_inputs = $(form).find(':file');

            if ($file_inputs.length > 0) {
                $file_inputs.ajaxFileUpload({
                    url: form.action,
                    dataType: 'json',
                    contentType: "application/json",
                    data: data,
                    global: false,
                    success: success.bind(self),
                    error: fail.bind(self)
                });
            }
            else {
                $.when($.postJSON(form.action, data, { global: true }))
                    .then(success.bind(self))
                    .fail(fail.bind(self));
              
            }
           
        };


        /**
         * Loads dialog view, creates the viewmodel, binds both together and shows the dialog.
         *
         * @param viewUrl {string} Url where the View for the dialog is available.
         * @param createViewModelFn {function} Function that returns an instanziated viewmodel (Parameter: Dialog-$Element)
         *
         */
        this.openDialog = function (viewUrl, createViewModelFn) {
            var $d = $.Deferred();
            $.when(
                $.get(viewUrl)
            )
            .then(function (html) {
                var $e = $(html),
                    vm = createViewModelFn($e);
                ko.applyBindings(vm, $e[0]);

                $('body').append($e);
                // Cancel?
                $e.on('cancel', function () {
                    $e.modal('hide');
                    $d.reject();
                });
                // Was closed.
                $e.on('hidden.bs.modal', function () {
                    $e.remove();
                    if ($d.state() === 'pending') { $d.resolve(); }
                });
                $e.modal({ backdrop: 'static', keyboard: true });
            }).fail(function (jqxhr) {
                self.handleServerError(jqxhr);
                $d.reject();
            });

            return $d.promise();
        };


        /**
         * Enables a model to handle/show validations comming from the server (.NET DataAnnotations).
         * Internally each ko.observable/Array() is extended with the dummy-validation-rule '__dataAnnotations__'.
         * 
         * @example
         *  var model = self.enableValidation(ko.mapping.fromJS(settings.Model));
         * 
         * @param model {object} The model to be extended with validation.
         * 
         */
        var _enableValidation = function () {
            _(model).each(function (v) {
                try {
                    must(v.extend, validator("Must be a function", aFunction));
                    v.extend({ __dataAnnotations__: true });
                }
                catch (e) {
                    // ko.mapping adds a '__ko_mapping__' property to the object.
                    // during the iteration this property causes an exception, that we don't care about, so...
                    // ...eat my dust.
                }
            });

            return model;
        };


        // Enable Validation for the provided model.
        _enableValidation(model);

        return self;
    }

    /**
     * Static version of the instance method with the same name.
     */
    BaseViewModel.handleServerError = function (jqxhr) {
        // TODO: Remove allocation!
        return new BaseViewModel({}).handleServerError(jqxhr);
    };

    return BaseViewModel;
});