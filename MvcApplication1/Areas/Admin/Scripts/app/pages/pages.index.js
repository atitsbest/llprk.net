define("app/pages/index", ["knockout", "source"], function (ko, Source) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys('pagesUrl'));

        self.pages = new Source({
            url: settings.pagesUrl,
            filter: {
                query: ko.observable().extend({ throttle: 500 })
            },
            sort: {
                by: 'CreatedAt',
                desc: false
            },
            paging: {
                entriesPerPage: 10
            },
            autoload: true
        });
    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});