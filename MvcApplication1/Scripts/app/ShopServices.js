'use strict'

function productsFn (products) {
    return {
        query: function () {
            return products;
        },

        queryById: function (productId) {
            return _(products).find(function (p) { return p.Id === productId; });
        }
    };
}
productsFn.$inject = ['products']

/**
 */
angular.module('ShopApp.Services', [])
    .factory('CartItems', function () {
        var STORAGE_ID = 'lillypark-cartitems';

        // Item = Produkt mit Menge
        // Hash<ProduktId, {Produkt, Menge}
        var items = get();

        /**
         * Items in den Warenkorb legen.
         */
        function put() {
            localStorage.setItem(STORAGE_ID, JSON.stringify(items));
        }

        /**
         * Liefert alle Warenkorb-Items.
         */
        function get() {
            var result = JSON.parse(localStorage.getItem(STORAGE_ID) || '{}');
            if (!_.isObject(result)) { result = {}; }
            return result;
        }

        return {
            query: function () {
                return items;
            },

            add: function (product) {
                if (items[product.Id]) {
                    items[product.Id].Qty += 1;
                }
                else {
                    items[product.Id] = {
                        Product: product,
                        Qty: 1
                    }
                }
                put();
            },

            remove: function (productId) {
                var item = items[productId];

                if (item) {
                    item.Qty -= 1;
                    if (item.Qty <= 0) {
                        delete items[productId];
                    }
                    put();
                }
            },

            /**
             * Warenkorb leeren.
             */
            clear: function () {
                items = {};
                put();
            }

        };
    })
    .factory('Products', productsFn);