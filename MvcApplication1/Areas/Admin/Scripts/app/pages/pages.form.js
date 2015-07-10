define("app/pages/form", ["knockout", "knockout.mapping", "underscore", "app/BaseViewModel"], function (ko, mapping, _, BaseViewModel) {
    return function Vm(model) {
        var self = this;

        self.publishedProxy = ko.observable(model.isPublished().toString());
        self.publishedProxy.subscribe(function (nv) { model.isPublished(nv === 'true'); });

        // Update url handle if title changes
        model.title.subscribe(function (title) {
            var handle = (title || "")
                .replace(/\s/g, '_')
                .replace(/[äÄ]/g, 'ae')
                .replace(/[öÖ]/g, 'oe')
                .replace(/[üÜ]/g, 'ue')
                .replace(/ß/g, 'ss')
                .toLowerCase();
            model.urlHandle(handle);
        });
    }
});