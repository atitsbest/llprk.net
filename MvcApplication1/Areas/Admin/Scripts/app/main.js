define("app/main", ["knockout.validation"], function (validation) {
    // JSON-Datum in DotNet und ISO Format nach JavaScript Date() konvertieren.
    (function () {
        var iso8601Regex = new RegExp("(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}([\\.\\d]*\\+\\d{2}:\\d{2})?)", 'gm'),
            originalParse = JSON.parse;

        JSON.parse = function (str) {
            return originalParse(str, function (key, value) {
                var match = /\/Date\(-?(\d+)\+?\d*\)\//.exec(value);
                if (match) {
                    return new Date(+match[1]);
                }

                match = iso8601Regex.exec(value);
                if (match) {
                    return new Date(match[1]);
                }

                return value;
            });
        }
    })();

    /**
     * Globales Setup von AJAX-Requests.
     */
    $.ajaxSetup({
        cache: false, // Kein Caching, mag der IE nicht.
    });

    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        var code = jqxhr.status;

        if (code == 500) {
            BaseViewModel.handleServerError(jqxhr);
        }

    });

    $.ajaxSettings.traditional = true;
    //var spinner = new Spinner();

    //// Global functions to show/hide on ajax requests
    //$(document).ajaxStart(function () { spinner.start(); });
    //$(document).ajaxStop(function () { spinner.stop(); });

    // Validation configuration
    validation.init({
        decorateInputElement: true
    });

    // Dummy-rule to cope with server-side Validations from DataAnnotations.
    validation.rules['__dataAnnotations__'] = {
        validator: function (val, otherVal) { return true; },
        message: ''
    };
    validation.registerExtenders();
});