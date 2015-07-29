define("app/shippingcosts/index", ["knockout", "knockout.mapping", "toastr", "app/BaseViewModel"], function (ko, mapping, toastr, BaseViewModel) {
    function Vm(settings) {
        var self = this;

        must(settings, validator("Must be a map", aMap),
            hasKeys('countries', 'shippingCategories', 'updateShippingCategoryUrl', 'createShippingCategoryUrl'));

        _.extend(self, mapping.fromJS(settings));
        _.extend(self, new BaseViewModel({
            countries: self.countries
        }));

        /**
            Change the name of a shipping category.
         */
        self.changeShippingCategory = function (shippingCategory) {
            var newName = prompt('New name for shipping category.\nHint: To delete a shopping category leave the name blank.', shippingCategory.name());

            if (existy(newName)) {

                $.when($.post(settings.updateShippingCategoryUrl, { id: shippingCategory.id(), name: newName }))
                    .then(function (result) {
                        if (_.isEmpty(newName)) {
                            self.shippingCategories.remove(shippingCategory);
                        }
                        else {
                            shippingCategory.name(newName);
                        }
                        self.handleEntityResult(result);
                    })
                    .fail(function () { toastr.error('O_o'); });

            }
        }

        /**
            Add a new shipping category
         */
        self.addShippingCategory = function () {
            var newName = prompt('Name for new shipping category');

            if (existy(newName)) {

                $.when($.post(settings.createShippingCategoryUrl, { name: newName }))
                    .then(function (result) {
                        self.shippingCategories.push(mapping.fromJS(result));
                        self.handleEntityResult(result);
                    })
                    .fail(function () { toastr.error('O_o'); });

            }
        }

    }

    return function (settings) {
        var vm = new Vm(settings);
        ko.applyBindings(vm);
    };
});