/**
 */
function ProductsCtrl($rootScope, $scope, CartItems, Products) {
    /**
     * Product in den Warenkorb legen.
     */
    $scope.addToCart = function (productId) {
        var product = Products.queryById(productId)
        if ($rootScope.cartItemCount() == 0) {
            $('#cart').show();
            scrollToTop();
        }
        CartItems.add(product);
    };
}

ProductsCtrl.$inject = ["$rootScope", "$scope", "CartItems", "Products"];