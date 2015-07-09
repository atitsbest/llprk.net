/**
 * INFO: Der Unterschied zwischen "selectedPictures" und "assignedPictures":
 *  selectedPictures: Das sind die Bilder, die der Benutzer gerade mi Dialog ausgewählt hat.
 *  assignedPictures: Das sind die Bilder, die der Benutzer aus dem Dialog übernommen hat.
 */
function PicturesCtrl($scope, $http) {
    // Vom Server generierte Daten holen.
    // TODO: Könnte man über angular.value() wohl besser machen.
    $scope.pictures = allPictures;
    $scope.assignedPictures = _(assignedPictureIds).map(function (id) { return _(allPictures).find(function (p) { return p.Id == id; }); });
    $scope.selectedPictures = _(assignedPictureIds).map(function (id) { return _(allPictures).find(function (p) { return p.Id == id; }); });

    $scope.assignedIdsAsString = function () {
        return (_($scope.assignedPictures).pluck("Id")).join(',');
    };

    $scope.isSelected = function (picture) {
        return _($scope.selectedPictures).contains(picture);
    };

    $scope.toggleSelect = function (picture) {
        if ($scope.isSelected(picture)) {
            $scope.selectedPictures = _($scope.selectedPictures).reject(function (p) { return p.Id == picture.Id; });
        }
        else {
            $scope.selectedPictures.push(picture);
        }
    };

    $scope.closeDlg = function () {
        $('#myModal').modal('hide');
        $scope.selectedPictures = _(allPictures).filter(function (p) { return _($scope.assignedPictures).contains(p); });
    };

    $scope.assignDlg = function () {
        $scope.assignedPictures = $scope.selectedPictures;
        $scope.closeDlg();
    };

    $scope.dragStart = function(e, ui) {
        ui.item.data('start', ui.item.index());
    }

    $scope.dragEnd = function(e, ui) {
        var start = ui.item.data('start'),
        end = ui.item.index(),
        t = $scope.assignedPictures;

        t.splice(end, 0, t.splice(start, 1)[0]);

        $scope.$apply();
    }

    $('ul.thumbnails').sortable({
        start: $scope.dragStart,
        update: $scope.dragEnd
    });
}

PicturesCtrl.$inject = ["$scope", "$http"];