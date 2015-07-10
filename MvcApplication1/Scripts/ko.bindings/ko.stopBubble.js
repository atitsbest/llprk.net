require(["jquery", "knockout"], function ($, ko) {
    /**
     * Prevents events to bubble up. 
     * Can be combined with other bindings.
     */
    ko.bindingHandlers.stopBubble = {
        init: function (element) {
            ko.utils.registerEventHandler(element, "click", function (event) {
                event.cancelBubble = true;
                if (event.stopPropagation) {
                    event.stopPropagation();
                }
            });
        }
    };
});