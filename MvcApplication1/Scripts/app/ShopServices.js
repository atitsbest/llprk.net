'use strict'
/**
 */
angular.module('ShopApp.Services', [])
    .factory('CartItems', function () {
        // Item = Produkt mit Menge
        // Hash<ProduktId, {Produkt, Menge}
        var items = {};

        return {
            query: function () {
                return items;
            },

            add: function (product) {
                if (items[product.Id] != null) {
                    items[product.Id].Qty += 1;
                }
                else {
                    items[product.Id] = {
                        Product: product,
                        Qty: 1
                    }
                }
            }

        };
    })
    .factory('Products', function (products) {
        return {
            query: function () {
                return products;
            },

            queryById: function (productId) {
                return _(products).find(function (p) { return p.Id === productId; });
            }
        };
    });