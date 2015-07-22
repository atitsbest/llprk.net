define("app/themes/edit",
    ["knockout", "knockout.mapping", "jquery", "underscore", "app/BaseViewModel", "codemirror",
        "codemirror/addons/modes/overlay",
        "codemirror/modes/htmlmixed/htmlmixed",
        "codemirror/modes/liquid/liquid",
    ],
    function (ko, mapping, $, _, BaseViewModel, CodeMirror) {
        function Vm(settings) {
            var self = this;

            must(settings, validator("Must be a map", aMap),
                hasKeys('items', 'itemContentUrl'));

            // Add observables with assets, templates, ...
            _.extend(self, mapping.fromJS(settings.items));

            //_.extend(self, new BaseViewModel(self.model));

            self.openFiles = ko.observableArray();
            self.currentFile = ko.observable();

            self.openFileInTab = function (item) {
                $.when($.get(settings.itemContentUrl, { id: item.name, type: item.type, theme: settings.themeName })).then(function (content) {
                    item.content = content;
                    item.handle = item.name().replace(/\./, '');
                    self.openFiles.push(item);
                    // Show new file in tab (open it).
                    self.currentFile(item);
                });
            };

            self.closeFileTab = function (item) {
                if (confirm('Close ' + item.name() + '?\nAll unsaved changes will be lost!')) {
                    var idx = self.openFiles().indexOf(item);

                    if (self.currentFile() === item) {
                        self.currentFile(_.first(self.openFiles()));
                    }
                    self.openFiles.splice(idx, 1);
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
                if (item.mimeType().indexOf('image') === 0) return 'image';
                else if (item.mimeType().indexOf('javascript') > -1) return 'javascript';
                else if (item.mimeType() === 'text/css') return 'css';
                else if (item.name().indexOf('.liquid') != -1) return 'liquid';
                else return null;
            }
        }

        return function (settings) {
            var vm = new Vm(settings);
            ko.applyBindings(vm);
        };
    });