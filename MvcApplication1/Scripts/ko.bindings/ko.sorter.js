require(["jquery", "knockout"], function ($, ko) {
    ko.bindingHandlers.sorter = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var sort = bindingContext.$data.sort,
                value = ko.utils.unwrapObservable(valueAccessor)();

            ko.applyBindingsToNode(element, {
                click: function () { sort.by(value); },
                css: {
                    sorted: ko.pureComputed(function () {
                        return sort.by() == value;
                    }),
                    desc: ko.pureComputed(function() {
                        return sort.desc() === true;
                    })
                }
            });

            $(element).append('<span class="sign asc"></span><span class="sign desc"></span>');
        },

        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        }
    };
});