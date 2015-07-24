define("app/themes/edit",
    ["knockout", "knockout.mapping", "jquery", "underscore", "toastr", "app/BaseViewModel", "codemirror",
        "codemirror/addons/modes/overlay",
        "codemirror/modes/htmlmixed/htmlmixed",
        "codemirror/modes/liquid/liquid",
    ],
    function (ko, mapping, $, _, toastr, BaseViewModel, CodeMirror) {
        function Vm(settings) {
            var self = this;

            must(settings, validator("Must be a map", aMap),
                hasKeys('themeName', 'items', 'itemContentUrl', 'createItemUrl', 'renameItemUrl', 'deleteItemUrl'));

            // Add observables with assets, templates, ...
            _.extend(self, mapping.fromJS(settings.items));
            //_.extend(self, new BaseViewModel(self.model));

            // All keys of the items. 
            // This just a helper for knockout's foreach.
            self.itemKeys = ko.pureComputed(function () {
                return _.keys(self.items);
            });

            self.openFiles = ko.observableArray();
            self.currentFile = ko.observable();

            self.openFileInTab = function (item) {
                if (!_(self.openFiles()).some(function (f) { return f === item; })) {
                    $.when($.get(settings.itemContentUrl, { id: item.name, type: item.type, theme: settings.themeName })).then(function (content) {
                        item.content = content;
                        item.handle = item.name().replace(/\./, '');
                        self.openFiles.push(item);
                    });
                }
                // Show new file in tab (open it).
                self.currentFile(item);
            };

            /**
             */
            self.closeFileTab = function (item, options) {
                if (options.dontAsk || confirm('Close ' + item.name() + '?\nAll unsaved changes will be lost!')) {
                    var idx = self.openFiles().indexOf(item);

                    if (self.currentFile() === item) {
                        self.currentFile(_.first(self.openFiles()));
                    }
                    self.openFiles.splice(idx, 1);
                }
            }

            /**
             */
            self.addItem = function (itemType) {
                var name = prompt('Name of the new file (with extension)');

                if (existy(name)) {
                    $.when($.postJSON(settings.createItemUrl, { id: name, type: itemType.name, theme: settings.themeName }))
                    .then(function (item) {
                        item = mapping.fromJS(item);
                        itemType.items.push(item);
                        
                        // Show new file.
                        self.openFileInTab(item);

                        toastr.success(item.name + ' added.');
                    })
                    .fail(function () { toastr.error('O_o'); });
                }
            }

            self.renameItem = function (itemType, item) {
                var newName = prompt('New name of the file (with extension)', item.name());

                if (existy(newName)) {
                    $.when($.postJSON(settings.renameItemUrl, { id: item.name(), type: itemType.name, theme: settings.themeName, newName: newName }))
                    .then(function (renamedItem) {
                        mapping.fromJS(renamedItem, {}, item);
                        toastr.success(item.name() + ' renamed.');
                    })
                    .fail(function () { toastr.error('O_o'); });
                }
            }

            self.deleteItem = function (itemType, item) {
                if (confirm('Please confirm to delete ' + item.name())) {
                    $.when($.postJSON(settings.deleteItemUrl, { id: item.name(), type: itemType.name, theme: settings.themeName}))
                    .then(function () {
                        self.closeFileTab(item, { dontAsk: true });
                        itemType.items.remove(item);
                        toastr.success(item.name() + ' removed.');
                    })
                    .fail(function () { toastr.error('O_o'); });
                }
            }

           

            self.iconFromMimetype = function (item) {
                switch (_getItemType(item)) {
                    case'image': return "glyphicon glyphicon-picture";
                    case 'javascript': return "glyphicon glyphicon-picture";
                    case 'css': return "glyphicon glyphicon-picture";
                    case 'liquid': return "glyphicon glyphicon-list-alt";
                    default: return "glyphicon glyphicon-file";
                }
            }

            var _getItemType = function (item) {
                if (item.contentType().indexOf('image') === 0) return 'image';
                else if (item.contentType().indexOf('javascript') > -1) return 'javascript';
                else if (item.contentType() === 'text/css') return 'css';
                else if (item.name().indexOf('.liquid') != -1) return 'liquid';
                else return null;
            }
        }

        return function (settings) {
            var vm = new Vm(settings);
            ko.applyBindings(vm);
        };
    });