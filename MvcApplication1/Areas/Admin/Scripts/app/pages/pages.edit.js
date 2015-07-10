define("app/pages/edit", ["knockout", "knockout.mapping", "underscore", "app/BaseViewModel", "app/pages/form"], function (ko, mapping, _, BaseViewModel, Form) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys('model'));

        self.model = mapping.fromJS(settings.model);

        _.extend(self, new Form(self.model));
        _.extend(self, new BaseViewModel(self.model));
    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});