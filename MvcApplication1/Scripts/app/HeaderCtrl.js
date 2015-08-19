/**
 */
function HeaderCtrl($scope, CartUI) {
	/**
	 * Warenkorb anzeigen.
	 */
	$scope.showCart = function () {
		CartUI.showCart();
	}

	/**
	 * Warenkorb wieder ausblenden.
	 */
	$scope.hideCart = function () {
		CartUI.hideCart();
	}
}

HeaderCtrl.$inject = ["$scope", "CartUI"];