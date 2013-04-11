/**
 */
function ProductsCtrl($rootScope, $scope, CartItems, Products, CartUI) {
    /**
     * Product in den Warenkorb legen.
     */
    $scope.addToCart = function (productId) {
        var product = Products.queryById(productId)
        if ($rootScope.cartItemCount() == 0) {
        	CartUI.showCart();
            scrollToTop();
        }
        CartItems.add(product);
    };
}

ProductsCtrl.$inject = ["$rootScope", "$scope", "CartItems", "Products", "CartUI"];