define("app/checkout/index", ["knockout"], function (ko) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys());

        self.model = {
            email: ko.observable().extend({ required: true, email: true}),

            lieferadresse: {
                anrede: ko.observable().extend({ required: true }),
                vorname: ko.observable().extend({ required: true }),
                nachname: ko.observable().extend({ required: true })
            },

            rechnungsadresse: {},
            gleicheAdresse: ko.observable(false)
        }

    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});