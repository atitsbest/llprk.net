/**
 */
function CartCtrl($scope, $http, CartItems, countries) {
    $scope.items = CartItems.query();
    // Soll der Warenkorb angezeigt werden?
    $scope.visible = true;

    /**
     * Anstatt dem Hash gibt's ein Array der Values.
     */
    $scope.itemsAsArray = function () { return _($scope.items).values(); };

    /**
     * Nur die Ids der Produkte.
     */
    $scope.productIds = function () { return _($scope.items).keys(); };

    /**
     * Gesamtpreis für alle Produkte im Warenkorb + Versandkosten.
     */
    $scope.totalPrice = function () {
    	var result = $scope.subTotalPrice() + $scope.shippingCosts();
    	return result;
    };

    /**
     * Gesamtpreis für alle Produkte im Warenkorb.
     */
    $scope.subTotalPrice = function () {
        return _.chain($scope.items)
                    .values()
                    .reduce(function (memo, item) { return memo + (item.Qty * item.Product.Price); }, 0)
                    .value();
    };

    _shippingCostsForItem = function (country, product) {
    	var category = _(country.ShippingCosts).find(function (sc) { return sc.Id === product.ShippingCategoryId });
    	return category ? category.Amount : 0;
    }

    _shippingCostsForCountry = function (country) {
    	return _($scope.items).reduce(function (memo, item) {
			return memo + _shippingCostsForItem(country, item.Product);
		}, 0);
    }

    /**
     * Versandkosten.
     */
    $scope.shippingCosts = function () {
    	var country = _(countries).find(function (c) { return c.Id == $scope.countryCode; }),
			costs = 0;
    	if (country != null) {
    		costs = _shippingCostsForCountry(country);
        }

        return costs;
    }

    /**
     * Zeile zum WK hinzufügen.
     */
    $scope.add = function (item) {
        CartItems.add(item.Product);
    };
    /**
     * Zeile aus dem WK entfernen
     */
    $scope.remove = function (item) {
        CartItems.remove(item.Product.Id);
    };

    $scope.toogleVisibility = function () {
        $scope.visible = !$scope.visible;
    };

}

CartCtrl.$inject = ["$scope", "$http", "CartItems", "countries"];