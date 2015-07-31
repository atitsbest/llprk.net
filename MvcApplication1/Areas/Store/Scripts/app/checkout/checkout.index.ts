/// <reference path="../../../../../Scripts/underscore.d.ts" />
/// <reference path="../../../../../Scripts/almond.d.ts" />
/// <reference path="../../../../../scripts/knockout.d.ts" />
/// <reference path="../DTOs.d.ts" />
define("app/checkout/index", ["jquery", "knockout", "knockout.mapping", "underscore", "app/BaseViewModel"], function ($, ko : KnockoutStatic, mapping, _ : UnderscoreStatic, BaseViewModel) {


    function Vm(settings : Store.DTOs.CheckoutIndex) {
        var self = this;

        self.model = mapping.fromJS(settings);

        self.sameAddress = ko.observable(true);

        // Delivery-country changed => update shipping costs.
        self.model.deliveryAddress.countryId.subscribe(function (countryId) {

            $.when($.getJSON(settings.checkoutShippingCostsUrl, { country: countryId }))
                .then(function (costs:Store.DTOs.CheckoutVariableCosts) {
                // Update prices.
                self.model.shippingCosts(costs.shippingCosts);
                self.model.tax(costs.tax);
                self.model.total(costs.total);
            })
                .fail(function () {
                alert('Beim Berechnen der Versandkosten ist ein Fehler passiert.');
            });

        });

        _.extend(self, new BaseViewModel(self.model));
    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});