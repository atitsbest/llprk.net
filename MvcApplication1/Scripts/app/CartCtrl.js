/**
 */
function CartCtrl($scope, CartItems) {
    $scope.items = CartItems.query();
    // Soll der Warenkorb angezeigt werden?
    $scope.visible = true;

    /**
     * Anstatt dem Hash gibt's ein Array der Values.
     */
    $scope.itemsAsArray = function () {
        return _($scope.items).values();
    };

    /**
     * Gesamtpreis für alle Produkte im Warenkorb.
     */
    $scope.totalPrice = function () {
        return _.chain($scope.items)
                    .values()
                    .reduce(function (memo, item) { return memo + (item.Qty * item.Product.Price); }, 0)
                    .value();
    };

    $scope.toogleVisibility = function () {
        $scope.visible = !$scope.visible;
    };

}

CartCtrl.$inject = ["$scope", "CartItems"];