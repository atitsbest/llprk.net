/**
 * INFO: Der Unterschied zwischen "selectedPictures" und "assignedPictures":
 *  selectedPictures: Das sind die Bilder, die der Benutzer gerade mi Dialog ausgewählt hat.
 *  assignedPictures: Das sind die Bilder, die der Benutzer aus dem Dialog übernommen hat.
 */
PicturesCtrl = function ($scope, $http) {
    // Vom Server generierte Daten holen.
    // TODO: Könnte man über angular.value() wohl besser machen.
    $scope.pictures = allPictures;
    console.log('init');
    $scope.assignedPictures = _(allPictures).filter(function (p) { return _(assignedPictureIds).contains(p.Id); });
    $scope.selectedPictures = _(allPictures).filter(function (p) { return _(assignedPictureIds).contains(p.Id); });

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
}