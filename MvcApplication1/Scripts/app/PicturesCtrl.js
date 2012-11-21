PicturesCtrl = function ($scope, $http) {
    $scope.name = 'Test';
    $scope.pictures = [];
    $scope.assignedPictures = initialPictureId;
    $scope.selectedPictures = function () {
        return _($scope.pictures).where({ selected: true });
    };
    $scope.selectedIds = function () {
        return (_($scope.selectedPictures()).pluck("Id")).join(',');
    };
    $scope.selectedThumbnailUrls = function () {
        return _($scope.selectedIdsAsArray()).pluck("ThumbnailUrl");
    };

    $http.get('/api/pictures').success(function (data) {
        $scope.pictures = data;
    });

    $scope.select = function (picture) {
        if (picture.selected) { delete picture.selected; }
        else { picture.selected = true; }
    };

    $scope.closeDlg = function () {
        $('#myModal').modal('hide');
    };

    $scope.assignDlg = function () {
        $scope.assignedPictures = $scope.selectedPictures();
        $scope.closeDlg();
    };
}