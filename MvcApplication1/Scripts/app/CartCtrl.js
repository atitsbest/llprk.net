/**
 */
function CartCtrl($scope, CartItems) {
    $scope.items = CartItems.query();

    /**
     * Anstatt dem Hash gibt's ein Array der Values.
     */
    $scope.itemsAsArray = function () {
        return _($scope.items).values();
    };
}

CartCtrl.$inject = ["$scope", "CartItems"];