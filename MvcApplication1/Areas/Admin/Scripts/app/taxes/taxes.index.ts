/// <reference path="../../../../../Scripts/underscore.d.ts" />
/// <reference path="../../../../../Scripts/almond.d.ts" />
/// <reference path="../../../../../scripts/knockout.d.ts" />
/// <reference path="../DTOs.d.ts" />

define("app/taxes/index", ["jquery", "underscore", "knockout", "knockout.mapping", "toastr", "app/BaseViewModel"], function ($, _, ko, mapping, toastr, BaseViewModel) {

    function Vm(settings: DTOs.Tax.TaxIndex) {
        var self = this;

        _.extend(self, new BaseViewModel({}));

        self.countries = mapping.fromJS(settings.countries);

        /**
            Change one countries' tax.
         */
        self.changeCountryTax = function (country: DTOs.Tax.Ko.Country) {
            var newPercent = parseInt(prompt("New percentage (must be a number)", country.taxPercent().toString()), 10);

            if (_.isFinite(newPercent)) {

                // Let user confirm.
                if (confirm('Change taxes for ' + country.id() + ' from ' + country.taxPercent() + '% to ' + newPercent + '% ?')) {

                    $.when($.post(settings.changeCountryTaxUrl, { country: country.id(), percent: newPercent }))
                        .then(function (result) {
                            country.taxPercent(newPercent);
                            self.handleEntityResult(result);
                        })
                        .fail(function () { toastr.error('O_o'); });
                }
            }

        }

    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});