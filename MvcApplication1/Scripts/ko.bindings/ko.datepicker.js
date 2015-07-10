require(["jquery", "knockout"], function ($, ko) {
    ko.bindingHandlers.datepicker = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var $el = $(element),
                value = _.isFunction(valueAccessor) ? valueAccessor() : valueAccessor;

            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || {};
            $el.datepicker(options);

            //handle the field changing
            if (ko.isObservable(value)) { //Manuel 16.6.2015 -> observable wird sonst nicht updated
                ko.utils.registerEventHandler(element, "change", function () {
                    value($el.datepicker("getDate"));
                });
            }

            //handle disposal (if KO removes by the template binding)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $el.datepicker("destroy");
            });

            return ko.bindingHandlers['validationCore'].init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
        },
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor()),
                $el = $(element),
                current = $el.datepicker("getDate");

            if (value - current !== 0) {
                $el.datepicker("setDate", value);
            }
        }
    };
});
