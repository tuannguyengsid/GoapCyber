
window.tinymceGSIDFileManager = function (opts) {
    // elFinder node
    let elfNode = $('<div/>');
    if (opts.nodeId) {
        elfNode.attr('id', opts.nodeId);
        delete opts.nodeId;
    }

    // upload target folder hash
    const uploadTargetHash = opts.uploadTargetHash || 'L1_Lw';
    delete opts.uploadTargetHash;

    // get elFinder insrance
    const getfm = open => {
        // CSS class name of TinyMCE conntainer
        const cls = (tinymce.majorVersion < 5) ? 'mce-container' : 'tox';

        console.log(open);
        return new Promise((resolve, reject) => {
            // elFinder ins
            // Execute when the elFinder instance is created
            const done = () => {
                if (open) {
                    tance
                    let elf;
                    console.log('open');
                    // request to open folder specify
                    if (!Object.keys(elf.files()).length) {
                        console.log('!Object.keys(elf.files()).length');
                        // when initial request
                        elf.one('open', () => {
                            elf.file(open) ? resolve(elf) : reject(elf, 'errFolderNotFound');
                        });
                    } else {
                        console.log('elFinder has already been initialized');
                        // elFinder has already been initialized
                        new Promise((res, rej) => {
                            if (elf.file(open)) {
                                res();
                            } else {
                                // To acquire target folder information
                                elf.request({ cmd: 'parents', target: open }).done(e => {
                                    elf.file(open) ? res() : rej();
                                }).fail(() => {
                                    rej();
                                });
                            }
                        }).then(() => {

                            console.log('then(() => {');
                            if (elf.cwd().hash == open) {
                                console.log('if (elf.cwd().hash == open) {');
                                resolve(elf);
                            } else {
                                console.log("elf.exec('open', open).done(() => {");
                                // Open folder after folder information is acquired
                                elf.exec('open', open).done(() => {
                                    resolve(elf);
                                }).fail(err => {
                                    reject(elf, err ? err : 'errFolderNotFound');
                                });
                            }
                        }).catch((err) => {
                            console.log('}).catch((err) => {');
                            reject(elf, err ? err : 'errFolderNotFound');
                        });
                    }
                } else {
                    console.log(' resolve(elf);');
                    // show elFinder manager only
                    resolve(elf);
                }
            };

            // Check elFinder instance
            if (elf = elfNode.elfinder('instance')) {
                console.log("if (elf = elfNode.elfinder('instance')) {");
                // elFinder instance has already been created
                done();
            } else {
                console.log(' var myCommands = elFinder.prototype._options.commands;');

                var myCommands = elFinder.prototype._options.commands;
                var disabled = ['extract', 'archive', 'help', 'select'];
                $.each(disabled, function (i, cmd) {
                    (idx = $.inArray(cmd, myCommands)) !== -1 && myCommands.splice(idx, 1);
                });

                // To create elFinder instance
                elf = elfNode.dialogelfinder(Object.assign({
                    // dialog title
                    title: opts.title,
                    commands: [
                        'custom', 'open', 'reload', 'home', 'up', 'back', 'forward', 'getfile', 'quicklook',
                        'download', 'rm', 'duplicate', 'rename', 'mkdir', 'mkfile', 'upload', 'copy',
                        'cut', 'paste', 'edit', 'extract', 'archive', 'search', 'info', 'view', 'resize', 'sort', 'netmount'
                    ],
                    contextmenu: {
                        // navbarfolder menu
                        navbar: ['open', '|', 'copy', 'cut', 'paste', 'duplicate', '|', 'rm', '|', 'info'],
                        // current directory menu
                        cwd: ['reload', 'back', '|', 'upload', 'mkdir', 'mkfile', 'paste', '|', 'sort', '|', 'info'],
                        // current directory file menu
                        files: ['getfile', '|', 'custom', 'quicklook', '|', 'download', '|', 'copy', 'cut', 'paste', 'duplicate', '|', 'rm', '|', 'edit', 'rename', 'resize', '|', 'archive', 'extract', '|', 'info']
                    },
                    uiOptions: {
                        toolbar: [
                            // toolbar configuration
                            ['open'],
                            ['back', 'forward'],
                            ['reload'],
                            ['home', 'up'],
                            ['mkfile', 'upload'],
                            ['info'],
                            ['quicklook'],
                            ['copy', 'cut', 'paste'],
                            ['rm'],
                            ['duplicate', 'rename', 'edit', 'resize'],
                            ['extract', 'archive'],
                            ['search'],
                            ['view'],
                        ]
                    },
                    // start folder setting
                    startPathHash: open ? open : void (0),
                    // Set to do not use browser history to un-use location.hash
                    useBrowserHistory: false,
                    // Disable auto open
                    autoOpen: false,
                    // elFinder dialog width
                    width: '90%',
                    // elFinder dialog height
                    height: '90%',
                    // set getfile command options
                    commandsOptions: {
                        getfile: {
                            onlyPath: true,
                            folders: false,
                            multiple: false,
                            oncomplete: 'close',
                        }
                    },
                    bootCallback: (fm) => {
                        // set z-index
                        fm.getUI().css('z-index', parseInt($('body>.' + cls + ':last').css('z-index')) + 100);
                    },
                    //uiOptions: {
                    //    toolbar: [
                    //        // toolbar configuration
                    //        ['open'],
                    //        ['back', 'forward'],
                    //        ['reload'],
                    //        ['home', 'up'],
                    //        ['mkdir', 'mkfile', 'upload'],
                    //        ['info'],
                    //        ['quicklook'],
                    //        ['copy', 'cut', 'paste'],
                    //        ['rm'],
                    //        ['duplicate', 'rename', 'edit'],
                    //        ['extract', 'archive'],
                    //        ['search'],
                    //        ['view']
                    //    ]
                    //},// toolbar extra options
                    toolbarExtra: {
                        showPreferenceButton: 'none',
                        // show Preference button into contextmenu of the toolbar (true / false)
                        preferenceInContextmenu: false
                    },
                    //contextmenu: {
                    //    // navbarfolder menu
                    //    navbar: ['open', 'download', '|', 'upload', 'mkdir', '|', 'copy', 'cut', 'paste', 'duplicate', '|', '|', 'rename', '|', 'places', 'info', 'chmod', 'netunmount'],
                    //    // current directory menu
                    //    cwd: ['undo', 'redo', '|', 'back', 'up', 'reload', '|', 'upload', 'mkdir', 'mkfile', 'paste', '|', '|', 'view', 'sort', 'selectall', 'colwidth', '|', 'info', '|', 'fullscreen', '|'],
                    //    // current directory file menu
                    //    files: ['getfile', '|', 'open', 'download', 'opendir', 'quicklook', '|', 'upload', 'mkdir', '|', 'copy', 'cut', 'paste', 'duplicate', '|', 'empty', '|', 'rename', 'edit', 'resize', '|', 'archive', 'extract', '|', 'selectall', 'selectinvert', '|', 'places', 'info', 'chmod', 'netunmount']
                    //},
                    getFileCallback: (files, fm) => {
                        console.log('getFileCallback');
                    }
                }, opts)).elfinder('instance');
                done();
            }
        });
    };

    this.browser = function (callback, value, meta) {
        console.log('browse');
        console.log(getfm());
        getfm().then(fm => {
            console.log(fm);
            let cgf = fm.getCommand('getfile');


            const regist = () => {
                fm.options.getFileCallback = cgf.callback = (file, fm) => {
                    var url, reg, info;

                    // URL normalization
                    url = (!opts.urlstatic) ? fm.convAbsUrl(file.url) : opts.urlstatic + file.url;

                    // Make file info
                    info = file.name + ' (' + fm.formatSize(file.size) + ')';

                    // Provide file and text for the link dialog
                    if (meta.filetype == 'file') {
                        callback(url, { text: info, title: info });
                    }

                    // Provide image and alt text for the image dialog
                    if (meta.filetype == 'image') {
                        callback(url, { alt: info });
                    }

                    // Provide alternative source and posted for the media dialog
                    if (meta.filetype == 'media') {
                        callback(url);
                    }
                };
                fm.getUI().dialogelfinder('open');
            };
            if (cgf) {
                // elFinder booted
                regist();
            } else {
                // elFinder booting now
                fm.bind('init', () => {
                    cgf = fm.getCommand('getfile');
                    regist();
                });
            }
        });

        return false;
    };

    this.uploadHandler = function (blobInfo, success, failure) {
        new Promise(function (resolve, reject) {
            getfm(uploadTargetHash).then(fm => {
                let fmNode = fm.getUI(),
                    file = blobInfo.blob(),
                    clipdata = true;
                const err = (e) => {
                    var dlg = e.data.dialog || {};
                    if (dlg.hasClass('elfinder-dialog-error') || dlg.hasClass('elfinder-confirm-upload')) {
                        fmNode.dialogelfinder('open');
                        fm.unbind('dialogopened', err);
                    }
                },
                    closeDlg = () => {
                        if (!fm.getUI().find('.elfinder-dialog-error:visible,.elfinder-confirm-upload:visible').length) {
                            fmNode.dialogelfinder('close');
                        }
                    };

                // check file object
                if (file.name) {
                    // file blob of client side file object
                    clipdata = void (0);
                }
                // Bind err function and exec upload
                fm.bind('dialogopened', err).exec('upload', {
                    files: [file],
                    target: uploadTargetHash,
                    clipdata: clipdata, // to get unique name on connector
                    dropEvt: { altKey: true, ctrlKey: true } // diable watermark on demo site
                }, void (0), uploadTargetHash)
                    .done(data => {
                        if (data.added && data.added.length) {
                            fm.url(data.added[0].hash, { async: true }).done(function (url) {
                                // prevent to use browser cache
                                url += (url.match(/\?/) ? '&' : '?') + '_t=' + data.added[0].ts;
                                resolve(fm.convAbsUrl(url));
                            }).fail(function () {
                                reject(fm.i18n('errFileNotFound'));
                            });
                        } else {
                            reject(fm.i18n(data.error ? data.error : 'errUpload'));
                        }
                    })
                    .fail(err => {
                        const error = fm.parseError(err);
                        reject(fm.i18n(error ? (error === 'userabort' ? 'errAbort' : error) : 'errUploadNoFiles'));
                    })
                    .always(() => {
                        fm.unbind('dialogopened', err);
                        closeDlg();
                    });
            }).catch((fm, err) => {
                const error = fm.parseError(err);
                reject(fm.i18n(error ? (error === 'userabort' ? 'errAbort' : error) : 'errUploadNoFiles'));
            });
        }).then((url) => {
            success(url);
        }).catch((err) => {
            failure(err);
        });
    };
};

(function ($) {
    var FMGSIDModal = function ($el, options) { //CustomPlugin
        this._defaults = {
            url: '/connector', // connector URL (REQUIRED)
            elModal: '#gsidelmodal',
            classModal: '',
            elFinderId: 'elfinderchoose',
            multiple: false,
            folders: false,
            elpreview: '',//img preview
            titleModal: 'File manager',
            footercontent: '', //content footer modal
            urlstatic: '',
            elbinding: '',// return full url
            getCallBack: null,
            keydata: 'fmGSIDModal'
        };

        var plugin = this;
        plugin.$element = $($el);

        plugin._options = $.extend(true, {}, this._defaults, options);

        plugin.options = function (options) {
            return (options) ?
                $.extend(true, plugin._options, options) :
                plugin._options;
        };

        plugin.init = function () {
            if (plugin.$element.attr('data-note') != '')
                plugin._options.footercontent = plugin.$element.attr('data-note');
            if (plugin.$element.attr('data-preview-el') != '')
                plugin._options.elpreview = plugin.$element.attr('data-preview-el');
            if (plugin.$element.attr('data-elbinding') != '')
                plugin._options.elbinding = plugin.$element.attr('data-elbinding');

            plugin.$element.click(function (e) {
                plugin.bindingModal();
                var _fmGSIDModal = plugin.$element.data(plugin._options.keydata);

                _fmGSIDModal.createFM();

                e.preventDefault();
            });
        }

        plugin.createFM = function () {
            var _fmGSIDModal = plugin.$element.data(plugin._options.keydata);
            var myCommands = elFinder.prototype._options.commands;

            var disabled = ['extract', 'archive', 'home', 'quicklook', 'rm', 'duplicate', 'rename', 'mkdir', 'mkfile', 'copy', 'cut', 'paste', 'edit', 'archive', 'search', 'resize'];
            $.each(disabled, function (i, cmd) {
                (idx = $.inArray(cmd, myCommands)) !== -1 && myCommands.splice(idx, 1);
            });

            // do something, in this case open elfinder.
            var elfinder = $('#' + _fmGSIDModal._options.elFinderId).elfinder({
                url: _fmGSIDModal._options.url, // connector URL (REQUIRED)
                resizable: false,
                width: '100%',
                height: 768,
                /*       commands: myCommands,*/
                commands: [
                    'custom', 'open', 'reload', 'home', 'up', 'back', 'forward', 'getfile', 'quicklook',
                    'download', 'rm', 'duplicate', 'rename', 'mkdir', 'mkfile', 'upload', 'copy',
                    'cut', 'paste', 'edit', 'extract', 'archive', 'search', 'info', 'view', 'resize', 'sort', 'netmount'
                ],
                contextmenu: {
                    // navbarfolder menu
                    navbar: ['open', '|', 'copy', 'cut', 'paste', 'duplicate', '|', 'rm', '|', 'info'],
                    // current directory menu
                    cwd: ['reload', 'back', '|', 'upload', 'mkdir', 'mkfile', 'paste', '|', 'sort', '|', 'info'],
                    // current directory file menu
                    files: ['getfile', '|', 'custom', 'quicklook', '|', 'download', '|', 'copy', 'cut', 'paste', 'duplicate', '|', 'rm', '|', 'edit', 'rename', 'resize', '|', 'archive', 'extract', '|', 'info']
                },
                uiOptions: {
                    toolbar: [
                        // toolbar configuration
                        ['open'],
                        ['back', 'forward'],
                        ['reload'],
                        ['home', 'up'],
                        ['mkfile', 'upload'],
                        ['info'],
                        ['quicklook'],
                        ['copy', 'cut', 'paste'],
                        ['rm'],
                        ['duplicate', 'rename', 'edit', 'resize'],
                        ['extract', 'archive'],
                        ['search'],
                        ['view'],
                    ]
                },
                commandsOptions: {
                    getfile: {
                        onlyPath: true,
                        folders: _fmGSIDModal._options.folders,
                        multiple: _fmGSIDModal._options.multiple,
                        oncomplete: 'close'
                    }
                },
                mimeDetect: 'internal',
                onlyMimes: [
                    'image/jpeg',
                    'image/jpg',
                    'image/png',
                    'image/gif'
                ],
                /*                        soundPath: '{{ asset('packages/ barryvdh / elfinder / sounds') }}',*/
                handlers: {
                    dblclick: function (event, elfinderInstance) {
                        var fileInfo = elfinderInstance.file(event.data.file);
                        if (fileInfo.mime != 'directory') {
                            var url = elfinderInstance.url(event.data.file);

                            if (_fmGSIDModal._options.elpreview != '')
                                $(_fmGSIDModal._options.elpreview).attr('src', _fmGSIDModal._options.urlstatic + url);

                            if (_fmGSIDModal._options.elbinding != '')
                                $(_fmGSIDModal._options.elbinding).val(url);

                            elfinderInstance.destroy();
                            return false; // stop elfinder
                        }
                    },
                    destroy: function (event, elfinderInstance) {
                        //elfinder.remove();
                        //elfinder.dialog('close');
                        $(_fmGSIDModal._options.elModal).modal('hide');
                    }
                },
                getFileCallback: function (file, fm) {
                    if (typeof _fmGSIDModal._options.getCallBack === "function") {
                        _fmGSIDModal._options.getCallBack(file, fm, _fmGSIDModal.$element, _fmGSIDModal._options);
                    }
                    else {
                        if (file.url != '') {
                            if (_fmGSIDModal._options.elpreview != '')
                                $(_fmGSIDModal._options.elpreview).attr('src', _fmGSIDModal._options.urlstatic + file.url);

                            if (_fmGSIDModal._options.elbinding != '')
                                $(_fmGSIDModal._options.elbinding).val(file.url);
                        }
                    }
                    elfinder.destroy();
                    return false;
                },

            }).elfinder('instance');
        }

        plugin.bindingModal = function () {
            var _fmGSIDModal = plugin.$element.data(plugin.options.keydata).fmGSIDModal;

            var $el = $('.gsidfmmodal');
            if ($el.length <= 0) {
                $el = $("<div/>");
                $el.addClass('modal draggable fade config gsidfmmodal' + _fmGSIDModal._options.classModal)
                    .attr('tabindex', '-1')
                    .attr('role', 'dialog')
                    .attr('aria-hidden', 'true')
                    .attr('data-backdrop', 'static')
                    .attr('data-keyboard', 'false')
                    .appendTo('body');

                if (_fmGSIDModal._options.elModal != '' && _fmGSIDModal._options.elModal.charAt(0) == '#')
                    $el.attr('id', _fmGSIDModal._options.elModal.substring(1, _fmGSIDModal._options.elModal.length));
                else
                    $el.addClass(_fmGSIDModal._options.elModal.substring(1, _fmGSIDModal._options.elModal.length));
            }
            $el.html('');

            var $mdialog = $('<div/>').addClass('modal-dialog').attr('role', 'document').appendTo($el);
            var $mcontent = $('<div/>').addClass('modal-dialog').appendTo($mdialog);
            var $mheader = $('<div/>').addClass('modal-header').appendTo($mcontent);
            var $mtitle = $('<h6/>').addClass('modal-title').text(_fmGSIDModal._options.titleModal).appendTo($mheader);
            var $mbtnClose = $('<button/>')
                .addClass('close')
                .attr('type', 'button')
                .attr('data-dismiss', 'modal')
                .attr('aria-hidden', 'true')
                .text('x')
                .appendTo($mheader);

            var $mbody = $('<div/>').addClass('modal-body').appendTo($mcontent);
            var $mfinderId = $('<div/>').attr('id', _fmGSIDModal._options.elFinderId).appendTo($mbody);
            if (_fmGSIDModal._options.footercontent != '') {
                var $mfooter = $('<div/>').addClass('modal-footer').appendTo($mcontent);
                var $mfooterContent = $('<div/>').text(_fmGSIDModal._options.footercontent).appendTo($mfooter);
            }

            $(_fmGSIDModal._options.elModal).on('show.bs.modal', function (event) {
                var idx = $('.modal:visible').length;
                $(this).css('z-index', 1040 + (10 * idx));
                $(_fmGSIDModal._options.elModal + ' .modal-backdrop').not('.stacked').css('z-index', 1039 + (10 * idx));
                $(_fmGSIDModal._options.elModal + ' .modal-backdrop').not('.stacked').addClass('stacked');
            });
            $(_fmGSIDModal._options.elModal + '.draggable>.modal-dialog').draggable({
                cursor: 'move',
                handle: '.modal-header'
            });
            $(_fmGSIDModal._options.elModal + '.draggable>.modal-dialog>.modal-content>.modal-header').css('cursor', 'move');

            $(_fmGSIDModal._options.elModal).modal('show');
        }

        plugin.init();
    };

    $.fn.fmGSIDModal = function (methodOrOptions) {
        var method = (typeof methodOrOptions === 'string') ? methodOrOptions : undefined;
        if (method) {
            var fmGSIDModals = [];
            function getFMGSIDModal() {
                var $el = $(this);
                var fmGSIDModal = $el.data('fmGSIDModal');
                fmGSIDModals.push(fmGSIDModal);
            }

            this.each(getFMGSIDModal);

            var args = (arguments.length > 1) ? Array.prototype.slice.call(arguments, 1) : undefined;
            var results = [];

            function applyMethod(index) {
                var fmGSIDModal = fmGSIDModals[index];

                if (!fmGSIDModal) {
                    console.warn('$.fmGSIDModal not instantiated yet');
                    console.info(this);
                    results.push(undefined);
                    return;
                }

                if (typeof fmGSIDModal[method] === 'function') {
                    var result = fmGSIDModal[method].apply(fmGSIDModal, args);
                    results.push(result);
                } else {
                    console.warn('Method \'' + method + '\' not defined in $.fmGSIDModal');
                }
            }

            this.each(applyMethod);

            return (results.length > 1) ? results : results[0];
        } else {
            var options = (typeof methodOrOptions === 'object') ? methodOrOptions : undefined;

            function init() {
                var $el = $(this);
                var fmGSIDModal = new FMGSIDModal($el, options);
                $el.data(fmGSIDModal._options.keydata, fmGSIDModal);
            }

            return this.each(init);
        }
    };
})(jQuery);


//- Dung cong cu https://javascriptobfuscator.com/Javascript-Obfuscator.aspx de ma hoa file Scripts\tinymce\tinymceElfinder.js va nho doi ten file
