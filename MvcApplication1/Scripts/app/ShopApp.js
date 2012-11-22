﻿/**
 */
function run ($rootScope, CartItems) {
    $rootScope.cartItemCount = function () {
        return _(CartItems.query()).values().length;
    }
}
run.$inject = ['$rootScope', 'CartItems'];

angular.module('ShopApp', ['ShopApp.Values', 'ShopApp.Services'])
    .run(run);
