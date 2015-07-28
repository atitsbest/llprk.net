define("app/shippingcosts/index", ["knockout", "knockout.mapping", "toastr", "app/BaseViewModel"], function (ko, mapping, toastr, BaseViewModel) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys('countries'));

        _.extend(self, new BaseViewModel({}));


        self.countries = mapping.fromJS(settings.countries);

    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});