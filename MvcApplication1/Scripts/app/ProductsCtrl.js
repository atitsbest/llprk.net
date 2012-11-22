/**
 */
function ProductsCtrl($scope, CartItems, Products) {
    /**
     * Product in den Warenkorb legen.
     */
    $scope.addToCart = function (productId) {
        console.log(productId);
        var product = Products.queryById(productId)
        CartItems.add(product);
    };
}

ProductsCtrl.$inject = ["$scope", "CartItems", "Products"];