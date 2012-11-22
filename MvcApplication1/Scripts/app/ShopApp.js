/**
 */
angular.module('ShopApp', ['ShopApp.Values', 'ShopApp.Services'])
    .run(function ($rootScope, CartItems) {
        $rootScope.cartItemCount = function () {
            return _(CartItems.query()).values().length;
        }
    });
