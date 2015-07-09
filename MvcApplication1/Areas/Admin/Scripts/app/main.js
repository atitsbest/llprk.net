define("app/main", ["knockout.validation"], function (validation) {

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