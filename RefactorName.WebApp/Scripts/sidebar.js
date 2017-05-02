(function ($) {
    "use strict";

    var App = function () {
        var o = this; // Create reference to this instance
        $(document).ready(function () {
            o.initialize();
        }); // Initialize app when document is ready

    };
    var p = App.prototype;

    // =========================================================================
    // MEMBERS
    // =========================================================================

    // Constant
    App.MENU_MAXIMIZED = 1;
    App.MENU_COLLAPSED = 2;
    App.MENU_MINIMIZED = 3;

    // Private
    p._responsiveSidebar = true;
    p._callFunctions = null;
    p._resizeTimer = null;

    // =========================================================================
    // INIT
    // =========================================================================

    p.initialize = function () {
        // Init events
        this._enableEvents();

        // Init base
        this._invalidateMenu();
        this._evalMenuScrollbar();
        this._initMenuExpandedItem();
        // Init components
        //this._initValidation();
        //this._initSlimscroll();
        //this._initTasks();
        //this._initTabs();
        //this._initForms();
        //this._initTooltips();
        //this._initPopover();
    };

    // =========================================================================
    // EVENTS
    // =========================================================================

    // events
    p._enableEvents = function () {
        var o = this;

        $('.btn-menu').on('click', function (e) {
            o._handleMenuState(e);
        });
        $('.main-menu').on('click', 'li', function (e) {
            o._handleMenuItemClick(e);
        });
        $('.sidebar-search > a').on('click', function (e) {
            o._toggleMenuSearchState(e);
        });
        $(window).on('resize', function (e) {
            o._handleScreenSize(e);
            o._evalMenuScrollbar(e);

            clearTimeout(o._resizeTimer);
            o._resizeTimer = setTimeout(function () {
                o._handleFunctionCalls(e);
            }, 300);
        });
    };

    // handlers
    p._handleScreenSize = function () {
        this._invalidateMenu();
    };

    // =========================================================================
    // MAIN MENU
    // =========================================================================

    p._handleMenuState = function (e) {
        if (this.getMenuState() === App.MENU_MINIMIZED) {
            $('body').toggleClass('open-menu');
        }
        else if (this.getMenuState() === App.MENU_COLLAPSED) {
            $('body').addClass('sidebar-expanded');
            $('body').removeClass('sidebar-collapsed');
        }
        else {
            $('body').removeClass('sidebar-expanded');
            $('body').addClass('sidebar-collapsed');
        }
    };

    p._handleMenuItemClick = function (e) {
        e.stopPropagation();

        var item = $(e.currentTarget);
        var link = item.find('> a');
        var submenu = item.find('> ul');
        var parentmenu = item.closest('ul');

        if (submenu.length !== 0) {
            this._closeMenus(parentmenu);
            if (!submenu.is(":visible")) {
                this._openSubMenu(item);
            }
        }
    };

    p._closeMenus = function (menu) {
        menu.find('> li > ul').slideUp(220, function () {
            $(this).closest('li').removeClass('expanded');
            localStorage.removeItem('expandedMenueItem');
        });
    };

    p._openSubMenu = function (item) {
        // If the menu is collapsed, the submenu should have no expand animation (BUG: android)
        if (this.getMenuState() === App.MENU_COLLAPSED) {
            item.addClass('expanded');
            item.find('> ul').show();
            var $ddd = item.find('span.title');
            localStorage.setItem('expandedMenueItem', $ddd.text());
        }
        else {
            item.addClass('expanding');
            item.find('> ul').slideDown(220, function () {
                item.addClass('expanded');
                item.removeClass('expanding');
                var $ddd = item.find('span.title');
                localStorage.setItem('expandedMenueItem', $ddd.text());
            });
        }
    };

    p._invalidateMenu = function () {
        var o = this;
        // Retrieve active link
        var selectedLink = $('.main-menu a.active');

        // When the main menu is minimized, the collapsed state should be removed
        if (o.getMenuState() === App.MENU_MINIMIZED) {
            $('body').removeClass('sidebar-collapsed');
        }

        // Expand all parent submenu's of the active link so it will be visible on startup
        selectedLink.closest('ul').parentsUntil($('.main-menu')).each(function () {
            if ($(this).is('li')) {
                $(this).addClass('active');
                $(this).addClass('expanded');
            }
        });

        // When invalidating, dont expand the first submenu when the menu is collapsed
        if (this.getMenuState() === App.MENU_COLLAPSED) {
            $('.main-menu').find('> li').removeClass('expanded');
        }

        // Trigger event
        $('.main-menu').triggerHandler('ready');
    };

    p.getMenuState = function () {
        var width = $('.main-menu').width();
        var overflowHidden = ($('html').css('overflow-x') === 'hidden');

        var menuState = App.MENU_MAXIMIZED;
        if (overflowHidden) {
            menuState = App.MENU_MINIMIZED;
        }
        else if (width > 10 && width <= 100) {
            menuState = App.MENU_COLLAPSED;
        }
        return menuState;
    };

    p._toggleMenuSearchState = function (e) {
        var btn = $(e.currentTarget);
        var menu = $(e.currentTarget).parent();
        menu.toggleClass('open');
        if (menu.hasClass('open')) {
            menu.find('input').focus();
        }
    };

    p._evalMenuScrollbar = function () {
        if (!$.isFunction($.fn.slimScroll)) {
            return;
        }

        // Add a scrollbar if needed to a .sidebar-fixed
        if ($('#sidebar').hasClass('sidebar-fixed')) {
            var menu = $('.main-menu');
            var parent = menu.parent();

            if (parent.hasClass('slimScrollDiv')) {
                // If the scroller exists, resize it.
                var height = $(window).height() - parent.position().top;
                menu.css({ height: height });
                parent.css({ height: height });
            }
            else {
                // If the scroller doesnt exists, create it.
                menu.slimScroll({
                    height: $(window).height() - menu.position().top
                });
            }
        }
    };

    p._initMenuExpandedItem = function () {
        var expandedTitle = localStorage.getItem('expandedMenueItem');
        if (expandedTitle) {
            $('ul.main-menu span.title').filter(function () { return $(this).text() == expandedTitle; }).closest('li').addClass('expanded')
        }
    };

    // =========================================================================
    // BOX LOADER
    // =========================================================================

    p.addBoxLoader = function (box) {
        var container = $('<div class="box-loader"></div>').appendTo(box);
        container.hide().fadeIn();
        var opts = {
            lines: 17, // The number of lines to draw
            length: 0, // The length of each line
            width: 3, // The line thickness
            radius: 6, // The radius of the inner circle
            corners: 1, // Corner roundness (0..1)
            rotate: 13, // The rotation offset
            direction: 1, // 1: clockwise, -1: counterclockwise
            color: '#000', // #rgb or #rrggbb or array of colors
            speed: 2, // Rounds per second
            trail: 76, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            top: 'auto', // Top position relative to parent in px
            left: 'auto' // Left position relative to parent in px
        };
        var spinner = new Spinner(opts).spin(container.get(0));
        box.data('box-spinner', spinner);
    };

    p.removeBoxLoader = function (box) {
        var spinner = box.data('box-spinner');
        var loader = box.find('.box-loader');
        loader.fadeOut(function () {
            spinner.stop();
            loader.remove();
        });
    };

    // =========================================================================
    // BOX COLLAPSE
    // =========================================================================

    p.toggleBoxCollapse = function (box, duration) {
        duration = typeof duration !== 'undefined' ? duration : 400;
        var dispatched = false;
        box.find('.slimScrollDiv').slideToggle(duration);
        box.find('.box-body').slideToggle(duration, function () {
            if (dispatched === false) {
                $('#COLLAPSER').triggerHandler('box.bb.collapse', [!$(this).is(":visible")]);
                dispatched = true;
            }
        });
        box.toggleClass('box-collapsed');
    };

    // =========================================================================
    // BOX REMOVE
    // =========================================================================

    p.removeBox = function (box) {
        box.fadeOut(function () {
            box.remove();
        });
    };

    // =========================================================================
    // VALIDATION
    // =========================================================================

    p._initValidation = function () {
        if (!$.isFunction($.fn.validate)) {
            return;
        }
        $.validator.setDefaults({
            highlight: function (element) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorElement: 'span',
            errorClass: 'help-block',
            errorPlacement: function (error, element) {
                if (element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                }
                else if (element.parent('label').length) {
                    error.insertAfter(element.parent());
                }
                else {
                    error.insertAfter(element);
                }
            }
        });

        $('.form-validate').each(function () {
            var validator = $(this).validate();
            $(this).data('validator', validator);
        });
    };

    // =========================================================================
    // SLIMSCROLL
    // =========================================================================

    p._initSlimscroll = function () {
        if (!$.isFunction($.fn.slimScroll)) {
            return;
        }

        $.each($('.scroll'), function (e) {
            var holder = $(this);
            holder.slimScroll({
                alwaysVisible: true,
                height: holder.height()
            });
        });
    };

    // =========================================================================
    // JQUERY-KNOB
    // =========================================================================

    p.getKnobStyle = function (knob) {
        var holder = knob.closest('.knob');
        var options = {
            width: Math.floor(holder.outerWidth()),
            height: Math.floor(holder.outerHeight()),
            fgColor: holder.css('color'),
            bgColor: holder.css('border-top-color'),
            draw: function () {
                if (knob.data('percentage')) {
                    $(this.i).val(this.cv + '%');
                }
            }
        };
        return options;
    };

    // =========================================================================
    // TASK LIST
    // =========================================================================

    p._initTasks = function () {
        $('.list-tasks .task-checkbox [data-toggle="buttons"].active').each(function (e) {
            var task = $(this).closest('li');
            var input = task.find('[data-toggle="buttons"] input');
            if (input.prop('checked') === true) {
                task.addClass('done');
            }
        });

        // Add events
        var o = this;
        $('.list-tasks').on('click', 'li', function (e) {
            o._handleTaskClick(e);
        });

        // Add sorting
        this._initSortableTasklist();
    };

    p._initSortableTasklist = function () {
        if (!$.isFunction($.fn.sortable)) {
            return;
        }

        $(".list-tasks").sortable({
            placeholder: "ui-state-highlight",
            delay: 100,
            start: function (e, ui) {
                ui.placeholder.height(ui.item.outerHeight() - 1);
            }
        });

    };
    p._handleTaskClick = function (e) {
        e.stopPropagation();

        var task = $(e.currentTarget);
        var list = task.closest('.list-tasks');
        var cb = task.find('[data-toggle="buttons"]');
        var input = cb.find('input');
        cb.button('toggle');
        task.toggleClass('done');
        list.triggerHandler('task.bb.completed', [task, task.hasClass('done')]);
    };

    // =========================================================================
    // TOOLTIPS
    // =========================================================================

    p._initTooltips = function () {
        if (!$.isFunction($.fn.tooltip)) {
            return;
        }
        $('[data-toggle="tooltip"]').tooltip();
    };

    // =========================================================================
    // POPOVER
    // =========================================================================

    p._initPopover = function () {
        if (!$.isFunction($.fn.popover)) {
            return;
        }
        $('[data-toggle="popover"]').popover();
    };

    // =========================================================================
    // TABS
    // =========================================================================

    p._initTabs = function () {
        if (!$.isFunction($.fn.tab)) {
            return;
        }
        $('[data-toggle="tabs"] a').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
    };

    // =========================================================================
    // FORMS
    // =========================================================================

    p._initForms = function () {
        // WebKit bug 53166 fix ('display' styles in media queries donâ€™t get re-applied correctly after resizing)
        var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
        var isSafari = /Safari/.test(navigator.userAgent) && /Apple Computer/.test(navigator.vendor);
        if (isChrome || isSafari) {
            $('.form-banded .form-group').each(function (e) {
                $(this).has('>[class*="col-sm"]').addClass('form-group-sm');
                $(this).has('>[class*="col-md"]').addClass('form-group-md');
                $(this).has('>[class*="col-lg"]').addClass('form-group-lg');
            });
        }
    };

    // =========================================================================
    // WYSIHTML
    // =========================================================================

    // To integrate Wysihtml5 into boostbox, boostbox has to bind to Wysihtml5 
    // events to detect state changes
    p.monitorWysihtml5 = function (editor) {
        if (typeof wysihtml5 === 'undefined') {
            return;
        }

        editor.on("show:dialog", function () {
            $(editor.toolbar.container).find('.panel-bar > div:visible').addClass('panel-visible');
        });
        editor.on("cancel:dialog", function () {
            setTimeout(function () {
                $(editor.toolbar.container).find('.panel-bar > div:hidden').removeClass('panel-visible');
            }, 1);
        });
        editor.on("save:dialog", function () {
            setTimeout(function () {
                $(editor.toolbar.container).find('.panel-bar > div:hidden').removeClass('panel-visible');
            }, 1);
        });

        editor.on("aftercommand:composer", function () {
            if (editor.toolbar !== undefined) {
                setTimeout(function () {
                    $(editor.toolbar.container).find('.btn').removeClass('active');
                    $(editor.toolbar.container).find('.btn.wysihtml5-command-active').addClass('active');
                }, 1);
            }
        });

        editor.on("load", function () {
            $(editor.composer.iframe).contents().on('keyup mouseup', function (e) {
                var loopNr = 0;
                var loop = setInterval(function () {
                    $(editor.toolbar.container).find('.btn').removeClass('active');
                    $(editor.toolbar.container).find('.btn.wysihtml5-command-active').addClass('active');

                    if (++loopNr === 2) {
                        clearInterval(loop);
                    }
                }, 251);
            });
        });
    };

    // =========================================================================
    // UTILS
    // =========================================================================

    p.callOnResize = function (func) {
        if (this._callFunctions === null) {
            this._callFunctions = [];
        }
        this._callFunctions.push(func);
        func.call();
    };

    p._handleFunctionCalls = function (e) {
        if (this._callFunctions === null) {
            return;
        }
        for (var i = 0; i < this._callFunctions.length; i++) {
            this._callFunctions[i].call();
        }
    };

    // =========================================================================
    // DEFINE NAMESPACE
    // =========================================================================

    window.boostbox = window.boostbox || {};
    window.boostbox.App = new App;
}(jQuery)); // pass in (jQuery):


//prettyPrint();

//$(function () {
//    $('input, select').on('change', function (event) {
//        var $element = $(event.target),
//          $container = $element.closest('.example');

//        if (!$element.data('tagsinput'))
//            return;

//        var val = $element.val();
//        if (val === null)
//            val = "null";
//        $('pre.val', $container).html(($.isArray(val) ? JSON.stringify(val) : "\"" + val.replace('"', '\\"') + "\""));
//        $('pre.items', $container).html(JSON.stringify($element.tagsinput('items')));
//    }).trigger('change');
//});