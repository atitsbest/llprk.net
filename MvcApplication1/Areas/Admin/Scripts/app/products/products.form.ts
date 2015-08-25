/// <reference path="../../../../../Scripts/underscore.d.ts" />
/// <reference path="../../../../../Scripts/almond.d.ts" />
/// <reference path="../../../../../scripts/knockout.d.ts" />
/// <reference path="../DTOs.d.ts" />

define("app/products/form",
    ["jquery", "knockout", "knockout.mapping", "underscore", "app/BaseViewModel", "jquery.ui.sortable", "dropzone"],
    ($, ko, mapping, _, BaseViewModel, jqueryUI, Dropzone) => {

    return function Vm(model) {
        var self = this;

        Dropzone.autoDiscover = false;
        var dz = new Dropzone('#images', {
            url: '/admin/products/newimage/' + model.id(),
            autoProcessQueue: false,
            addRemoveLinks: true,
            previewTemplate: '<li><img data-dz-thumbnail /></li>'
        });

        dz.on('addedfile', function () {
            $("#images").sortable({
                forceHelperSize: true,
                cancel: '.dz-default',
                stop: function (e, ui) {
                    $('#images > li').each(function (idx, li) {
                        // TODO: Update array here.
                        console.log($(li).find('img').data('id'));
                        console.log(dz.getQueuedFiles());
                    });
                }
            });
            $("#images").disableSelection();
        });


        /**
         * Submit Product AND new images to server.
         */
        self.submit = function (form) {
            var data = mapping.toJS(model);
            function success(result) {
                self.handleEntityResult(result);
            }

            function fail(result) {
                self.handleFormFailure(result);
                self.handleServerError(result);              
            }

            $.when($.postJSON(form.action, data, { global: true }))
                .then(function(response) {
                    dz.processQueue();
                    success(response);
                })
                .fail(fail.bind(self));
        };
         
    };
});