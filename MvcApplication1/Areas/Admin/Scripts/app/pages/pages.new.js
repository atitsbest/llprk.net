define("app/pages/new", ["knockout", "underscore", "app/BaseViewModel"], function (ko, _, BaseViewModel) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys());

        self.model = {
            title: ko.observable(),
            content: ko.observable(),
            isPublished: ko.observable('Hidden'),
            urlHandle: ko.observable()
        };

        // Update url handle if title changes
        self.model.title.subscribe(function (title) {
            var handle = (title || "")
                .replace(/\s/g, '_')
                .replace(/[äÄ]/g, 'ae')
                .replace(/[öÖ]/g, 'oe')
                .replace(/[üÜ]/g, 'ue')
                .replace(/ß/g, 'ss')
                .toLowerCase();
            self.model.urlHandle(handle);
        });

        _.extend(self, new BaseViewModel(self.model));
    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});