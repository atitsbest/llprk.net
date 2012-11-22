/**
 */
function ProductsCtrl($scope, CartItems, Products) {
    /**
     * Product in den Warenkorb legen.
     */
    $scope.addToCart = function (productId) {
        var product = Products.queryById(productId)
        CartItems.add(product);
    };
}

ProductsCtrl.$inject = ["$scope", "CartItems", "Products"];