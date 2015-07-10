require(["jquery", "knockout"], function ($, ko) {
    ko.bindingHandlers.date = {
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = ko.utils.unwrapObservable(valueAccessor()),
				format = allBindingsAccessor().dateFormat || 'dd.MM.yyyy';

            // Bei einem NULL geben wir einen leeren String aus.
            var output = shouldBeEmpty(value)
				? ''
				: Globalize.format(value, format);

            set(element, output);
        }
    };

    /**
	 * Soll dieser Datumswert angezeigt werden?
	 */
    function shouldBeEmpty(/*{Date}*/date) {
        return date === null
			|| date === undefined
			|| date.valueOf() == -621356004E5;
    }

    /**
	 * Setzte den Text, oder Value, je nach DOM-Element.
	 */
    function set(/*Element*/e, /*string*/date) {
        var $e = $(e);

        $e.is(':input') ? $e.val(date) : $e.text(date);
    }

});
