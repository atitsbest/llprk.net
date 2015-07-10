define("app/main", ["knockout.validation"], function (validation) {
    // JSON-Datum in DotNet und ISO Format nach JavaScript Date() konvertieren.
    (function () {
        var originalParse = JSON.parse;
        JSON.parse = function (str) {
            return originalParse(str, function (key, value) {
                var match = /\/Date\(-?(\d+)\+?\d*\)\//.exec(value);
                if (match) {
                    var date = new Date(parseInt(match[0].substr(6)));   //http://stackoverflow.com/questions/206384/format-a-microsoft-json-date 
                    return new Date(+match[1]);
                }
                return value;
            });
        }
    })();

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