/**
 */
function HeaderCtrl($scope) {
	/**
	 * Warenkorb anzeigen.
	 */
	$scope.showCart = function () {
		// Backdrop einfügen.
		$('body').append('<div class="modal-backdrop" style="opacity:.1;cursor:pointer;"></div>');
		$('#cart').show();
		$('.modal-backdrop').click(function () {
			$scope.hideCart();
		});
	}

	/**
	 * Warenkorb wieder ausblenden.
	 */
	$scope.hideCart = function () {
		$('#cart').hide();
		$('.modal-backdrop').remove();
	}
}

HeaderCtrl.$inject = ["$scope"];