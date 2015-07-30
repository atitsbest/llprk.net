define("app/checkout/index", ["jquery", "knockout", "underscore", "app/BaseViewModel"], function ($, ko, _, BaseViewModel) {
    function Address() {
        var self = this;
        self.salutation = ko.observable().extend({ required: true });
        self.firstname = ko.observable().extend({ required: true });
        self.lastname = ko.observable().extend({ required: true });
        self.address1 = ko.observable();
        self.address2 = ko.observable().extend({ required: true });
        self.zip = ko.observable().extend({ required: true });
        self.city = ko.observable().extend({ required: true });
        
        self.countryId = ko.observable().extend({ required: true });

        return self;
    };

    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys('lineItems', 'subTotal', 'tax', 'shippingCosts', 'total', 'checkoutShippingCostsUrl'));

        self.model = {
            email: ko.observable().extend({ required: true, email: true}),

            deliveryAddress: new Address(),
            billingAddress: new Address(),
            sameAddress: ko.observable(true),

            accepted: ko.observable().extend({ required: true })
        }

        // Shipping costs
        self.shippingCosts = ko.observable(settings.shippingCosts);
        // Tax
        self.tax = ko.observable(settings.tax);
        // Total price.
        self.total = ko.observable(settings.total);

        // All the models' errors.
        self.errors = ko.validation.group(self.model, { deep: true });

        // Is all valid
        self.isValid = ko.pureComputed(function () {
            return self.errors().length == 0
                && self.modle.accepted();
        });

        // Delivery-country changed => update shipping costs.
        self.model.deliveryAddress.countryId.subscribe(function (countryId) {
            $.when($.getJSON(settings.checkoutShippingCostsUrl, { country: countryId }))
            .then(function(data) {
                // Update prices.
                self.shippingCosts(data.shippingCosts);
                self.tax(data.tax);
                self.total(data.total);
            })
            .fail(function() {
                alert('Beim Berechnen der Versandkosten ist ein Fehler passiert.');
                //window.location.reload();
            });

        });

        _.extend(self, new BaseViewModel(self.model));
    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});