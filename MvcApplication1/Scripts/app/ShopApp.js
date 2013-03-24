/**
 */
function run ($rootScope, CartItems) {
    $rootScope.cartItemCount = function () {
        var items = _(CartItems.query()).values();
        return _(items).reduce(function (memo, n) { return memo + n.Qty; }, 0);
    }
}
run.$inject = ['$rootScope', 'CartItems'];

angular.module('ShopApp.Values', []);

angular.module('ShopApp', ['ShopApp.Values', 'ShopApp.Services'])
    .run(run);
