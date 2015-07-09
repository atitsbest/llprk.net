define("source",
    ["jquery", "underscore", "knockout", "knockout.mapping", "ko.extenders.withPrevious"],
    function ($, _, ko, mapping) {
        /**
         * Provides data from remote sources, that can be sortet, filtered an paged.
         */
        function Source(settings) {
            var self = this;
       
            settings = _.extend(
                {
                    dataProperty: 'data',
                    totalProperty: 'total',
                    autoload: false
                },
                settings);

            // save filter from settings.
            self.filter = settings.filter;

            /**
             * Currently filtered/sorted/pages data.
             */
            self.data = ko.observableArray([]);

            /**
             * paging-infos.
             */
            self.paging = mapping.fromJS(_.extend({
                page: 1,
                total: 0,
                entriesPerPage: 10000000,
            }, settings.paging || {}));

            self.paging.pages = ko.pureComputed(function () {
                if (this.total() > 0 && this.entriesPerPage() > 0) {
                    return Math.ceil(this.total() / this.entriesPerPage());
                }
                return 0;
            }, self.paging);


            /**
             * Sortierung.
             */
            self.sort = mapping.fromJS(_.extend({
                by: ko.observable().extend({ notify: 'always', withPrevious: true }),
                desc: false
            }, settings.sort || {}));
            self.sort.by.extend({ notify: 'always', withPrevious: true });
            self.sort.by.subscribe(function (oldValue) {
                self.sort.desc(!self.sort.desc() && !self.sort.by.changed());
            });


            /**
             * Build url-filter.
             */
            self.filterFn = ko.pureComputed(function () {
                var p = self.paging,
                    s = self.sort;

                return {
                    page: p.page(),
                    count: p.entriesPerPage(),
                    orderBy: s.by(),
                    desc: s.desc() ? true : undefined,
                    query: mapping.toJS(self.filter || {})
                };
            }, self);
            
            /**
             * Load the data filtered/sorted/paged.
             */
            self.load = _(function () {
                var request = settings.post ? $.postJSON : $.getJSON;
               
                $.when(request(ko.utils.unwrapObservable(settings.url), self.filterFn()))
                    .then(function (result) {
                        stopPagingObserver();
                       
                        if (existy(settings.dataMapper)) {
                            self.data(_(result[settings.dataProperty]).map(settings.dataMapper));
                        }
                        else {
                            self.data(result[settings.dataProperty]);
                      
                        }
                        self.paging.total(result[settings.totalProperty]);
                        
                        startPagingObserver();
                    });
            }).debounce(50);


            // paging surveillance
            var pagingSubscriptions = [];

            function stopPagingObserver() {
                for (var i = 0; i < pagingSubscriptions.length; i += 1) {
                    pagingSubscriptions[i].dispose();
                }
                pagingSubscriptions = [];
            }

            function startPagingObserver() {
                stopPagingObserver();
                pagingSubscriptions.push(self.paging.page.subscribe(function () { self.load(); }));
            }

            // sort surveillance.
            self.sort.by.subscribe(function () { self.load(); });
            self.sort.desc.subscribe(function () { self.load(); });


            // filter surveillance.
            _.each(self.filter, function (v) {

                if (ko.isObservable(v)) {
                    v.subscribe(function () { self.load(); });
                }
            });

            // url surveillance.
            if (ko.isObservable(settings.url)) {
                settings.url.subscribe(function () { self.load(); });
            }

            // Laden?
            if (settings.autoload) {
                self.load();
            }
        }

        return Source;
    });
