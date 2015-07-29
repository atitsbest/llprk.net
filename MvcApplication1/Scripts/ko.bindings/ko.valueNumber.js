require(["jquery", "knockout"], function ($, ko) {
    ko.bindingHandlers.valueNumber = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var observable = valueAccessor(),
                        properties = allBindingsAccessor();
            var interceptor = ko.computed({
                read: function () {
                    var format = properties.numberFormat || "n2";
                    return Globalize.format(observable(), format);
                },
                write: function (newValue) {
                    var number = Globalize.parseFloat(newValue);
                    if (number) {
                        observable(number);
                    }
                }
            });
            if (element.tagName.toLowerCase() === 'input') {
                ko.applyBindingsToNode(element, { value: interceptor });
            } else {
                ko.applyBindingsToNode(element, { text: interceptor });
            }
        }
    };
});