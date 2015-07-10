﻿require(["jquery", "knockout"], function ($, ko) {
    ko.bindingHandlers.numericValue = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            var underlyingObservable = valueAccessor();
            var interceptor = ko.dependentObservable({
                read: underlyingObservable,
                write: function (value) {
                    if (value == '') {
                        underlyingObservable(undefined);
                    }
                    else if (!isNaN(value)) {
                        underlyingObservable(parseFloat(value));
                    }
                },
                disposeWhenNodeIsRemoved: element
            });
            ko.bindingHandlers.value.init(element, function () { return interceptor }, allBindingsAccessor);
        },
        update: ko.bindingHandlers.value.update
    };
});