define("app/pages/new", ["knockout", "underscore", "app/BaseViewModel", "app/pages/form"], function (ko, _, BaseViewModel, Form) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys());

        self.model = {
            title: ko.observable(),
            content: ko.observable(),
            isPublished: ko.observable(false),
            urlHandle: ko.observable()
        };

        _.extend(self, new Form(self.model));
        _.extend(self, new BaseViewModel(self.model));
    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});