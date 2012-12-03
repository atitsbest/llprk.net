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

    $scope.confirmAddress = function () {
        if ($scope.addressForm.$valid) {
            $('#step2').collapse('hide');
            $('#step3').collapse('show');
        }
    };

    $scope.confirmPayment = function () {
        $('#step3').collapse('hide');
        $('#step4').collapse('show');
    };

    $scope.submitOrder = function () {
        if ($scope.summaryForm.$valid) {
            $('#step4').collapse('hide');
            $('#step5').collapse('show');
        }
    };

}

CartCtrl.$inject = ["$scope", "CartItems"];