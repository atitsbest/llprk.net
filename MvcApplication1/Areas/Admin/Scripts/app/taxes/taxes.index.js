define("app/taxes/index", ["knockout", "knockout.mapping", "toastr", "app/BaseViewModel"], function (ko, mapping, toastr, BaseViewModel) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys('changeCountryTaxUrl', 'countries'));

        _.extend(self, new BaseViewModel({}));


        self.countries = mapping.fromJS(settings.countries);

        /**
            Change one countries' tax.
         */
        self.changeCountryTax = function (country) {
            var newPercent = parseInt(prompt("New percentage (must be a number)", country.taxPercent()), 10);

            if (_.isFinite(parseInt(newPercent, 10))) {

                $.when($.post(settings.changeCountryTaxUrl, { country: country.id(), percent: parseInt(newPercent, 10) }))
                    .then(function(result) {
                        country.taxPercent(newPercent);
                        self.handleEntityResult(result);
                    })
                    .fail(function() { toastr.error('O_o'); });
            }
            
        }

    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});