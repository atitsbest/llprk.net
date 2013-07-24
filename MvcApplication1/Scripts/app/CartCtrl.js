/**
 */
function CartCtrl($scope, $http, CartItems, countries) {
    $scope.items = CartItems.query();
    // Soll der Warenkorb angezeigt werden?
    $scope.visible = true;

    // Alle Länder verfügbar machen.
    $scope.countries = countries;
    $scope.countryCode = countries[0].Id;
    $scope.county = countries[0].Name;

    $scope.countryName = function () {
    	return _($scope.countries).where({ Id: $scope.countryCode })[0].Name;
    };

	// Zahlungsmethoden.
    $scope.paymentTypes = [
		//{ id: "PAYPAL", name: "PayPal" },
		{ id: "WIRE", name: "Überweisung" }];

    $scope.paymentType = $scope.paymentTypes[0].id;
    $scope.humanReadablePaymentType = function () {
    	return _($scope.paymentTypes).where({ id: $scope.paymentType })[0].name;
    }

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
    	var result = $scope.subTotalPrice() + $scope.shippingCosts();
    	return result;
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

    _shippingCostsForItem = function (country, product) {
    	var category = _(country.ShippingCosts).find(function (sc) { return sc.Id === product.ShippingCategoryId });
    	return category ? category.Amount : 0;
    }

    _shippingCostsForCountry = function (country) {
    	return _($scope.items).reduce(function (memo, item) {
			return memo + _shippingCostsForItem(country, item.Product);
		}, 0);
    }

    /**
     * Versandkosten.
     */
    $scope.shippingCosts = function () {
    	var country = _(countries).find(function (c) { return c.Id == $scope.countryCode; }),
			costs = 0;
    	if (country != null) {
    		costs = _shippingCostsForCountry(country);
        }

        return costs;
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

        	// Wenn Paypal als Bezahlmethode gewählt wurde,
        	// muss die Bankverbindung nicht mehr angezeigt werden.
            var nextStep = ($scope.paymentType == "PAYPAL")
				? '#step4'
				: '#step3'
			$(nextStep).collapse('show');
        }
    };

    $scope.confirmPayment = function () {
        $('#step3').collapse('hide');
        $('#step4').collapse('show');
    };

    $scope.backFromSummary = function () {
    	$('#step4').collapse('hide');
		// Wenn Paypal als Bezahlmethode gewählt wurde,
		// muss die Bankverbindung nicht mehr angezeigt werden.
		var nextStep = ($scope.paymentType == "PAYPAL")
			? '#step2'
			: '#step3'
    	$(nextStep).collapse('show');
    }

    $scope.submitOrder = function () {
        if ($scope.summaryForm.$valid) {

            $('#step4').collapse('hide');
            $('#step5').collapse('show');

        	$http.post('/api/shop/new', _generateNewOrder())
                .success(function (orderId, s, h, c) {
                	_handlePayment(orderId);
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

	/**
	 * 
	 */
    $scope.proceedeToPayPal = function () {
    	CartItems.clear();
    	$('#cart').hide();

    	// "Bitte warten..." Dialog anzeigen.
    	$('	<div class="modal"> \
				<div class="modal-body"> \
					<p>Wir leiten Dich weiter zu PayPal...</p> \
				</div> \
			</div>').modal();

    	return true;
    }

	/**
	 * Erstellt ein Bestellung, die an den Server geschickt werden kann.
	 */
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
            comment:	$scope.comment,
			paymentType:$scope.paymentType,
            products: products
        };
        return result;
    }

    function _handlePayment(orderId) {
    	switch ($scope.paymentType) {
    		case "PAYPAL": return _handlePayPalPayment(orderId);
    		case "WIRE": return _handleWirePayment();
    		default: throw Error("Ungültige Zahlungsart " + $scope.paymentType + "!");
    	}
    }

    function _handlePayPalPayment(orderId) {
    	$scope.orderId = orderId;
    	$('#step5').collapse('hide');
    	$('#step_paypal').collapse('show');
    }

    function _handleWirePayment() {
    	CartItems.clear();
    	$('#step5').collapse('hide');
		$('#cart').hide();
    	alert('Deine Bestellung ist eingegangen und wird von uns bearbeitet.');
		// Auf eine neue Seite verweisen.
		window.location = "/shop/";
    }

}

CartCtrl.$inject = ["$scope", "$http", "CartItems", "countries"];