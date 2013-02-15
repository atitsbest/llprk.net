/**
 */
function CartCtrl($scope, $http, CartItems, countries) {
    $scope.items = CartItems.query();
    // Soll der Warenkorb angezeigt werden?
    $scope.visible = true;

    // Alle Länder verfügbar machen.
    $scope.countries = countries;
    $scope.countryCode = countries[0].Id;

    /**
     * Anstatt dem Hash gibt's ein Array der Values.
     */
    $scope.itemsAsArray = function () { return _($scope.items).values(); };

    /**
     * Nur die Ids der Produkte.
     */
    $scope.productIds = function () { return _($scope.items).keys(); };

    /**
     * Gesamtpreis für alle Produkte im Warenkorb + Versandkosten.
     */
    $scope.totalPrice = function () {
        return $scope.subTotalPrice() + $scope.shippingCosts();
    };

    /**
     * Gesamtpreis für alle Produkte im Warenkorb.
     */
    $scope.subTotalPrice = function () {
        return _.chain($scope.items)
                    .values()
                    .reduce(function (memo, item) { return memo + (item.Qty * item.Product.Price); }, 0)
                    .value();
    };

    /**
     * Versandkosten.
     */
    $scope.shippingCosts = function () {
        var country = _(countries).find(function (c) { return c.Id == $scope.countryCode; })
        return country == null
            ? 0
            : country.ShippingCosts;
    }

    /**
     * Zeile zum WK hinzufügen.
     */
    $scope.add = function (item) {
        CartItems.add(item.Product);
    };
    /**
     * Zeile aus dem WK entfernen
     */
    $scope.remove = function (item) {
        CartItems.remove(item.Product.Id);
    };

    $scope.toogleVisibility = function () {
        $scope.visible = !$scope.visible;
    };

    $scope.confirmAddress = function () {
        if ($scope.addressForm.$valid) {
            $('#step2').collapse('hide');
            $('#step3').collapse('show');
        }
    };

    $scope.confirmPayment = function () {
        $('#step3').collapse('hide');
        $('#step4').collapse('show');
    };


    $scope.submitOrder = function () {
        if ($scope.summaryForm.$valid) {

            $('#step4').collapse('hide');
            $('#step5').collapse('show');

            $http.post('/shop/new', _generateNewOrder())
                .success(function (d, s, h, c) {
                    $('#step5').collapse('hide');
                    $('#cart').hide();
                    CartItems.clear();
                    alert('Deine Bestellung ist eingegangen und wird von uns bearbeitet.');
                    // Auf eine neue Seite verweisen.
                    window.location = "/shop/";
                })
                .error(function (d, s, h, c) {
                    $('#step5').hide('hide');
                    $('#step1').collapse('show');
                    alert(d);
                });

        }
        else {
            alert('Da stimmt was nicht mit den Eingaben. Bitte kontrollieren.');
        }
    };

    function _generateNewOrder() {
        var products = _($scope.items).map(function (k, v) {
            return { id: k.Product.Id, qty: k.Qty };
        });

        var result = {
            salutation: $scope.title,
            firstname:  $scope.firstname,
            name:       $scope.name,
            address1:   $scope.address1,
            address2:   $scope.address2,
            zip:        $scope.zip,
            city:       $scope.city,
            countryCode:$scope.countryCode,
            email:      $scope.email,
            products: products
        };
        return result;
    }

}

CartCtrl.$inject = ["$scope", "$http", "CartItems", "countries"];