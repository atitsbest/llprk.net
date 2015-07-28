require(["jquery", "knockout"], function ($, ko) {
    ko.bindingHandlers.currency = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = ko.utils.unwrapObservable(valueAccessor());

			var output = Globalize.format(value || 0, "C");

            set(element, output);
        }
    };

    /**
	 * Setzte den Text, oder Value, je nach DOM-Element.
	 */
    function set(/*Element*/e, /*string*/date) {
        var $e = $(e);

        $e.is(':input') ? $e.val(date) : $e.text(date);
    }
});
