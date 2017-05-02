$(document).ready(function () {
    var newdate = new Date();
    $('.footer>div>span').text('© ' + newdate.getFullYear());
    if ($('div.start').length) {
        $('div.start').removeClass('start')
    }
    var paralexHead = function (item) {
        if (item.length && Modernizr.mq('(min-width: 769px)')) {
            item.attr('style', 'background-position:center -' + $(window).scrollTop() / 4 + 'px')
        } else {
            item.attr('style', 'background-position:center center')
        }
    }
    var paralexBlock = function (item) {
        if (item.length && Modernizr.mq('(min-width: 769px)')) {
            item.attr('style', 'background-position:center ' + (item.offset().top - $(window).scrollTop()) / 4 + 'px')
        } else {
            item.attr('style', 'background-position:center center')
        }
    }
    paralexHead($('.header-box'))
    paralexBlock($('.video-block'))

    $(window).on('scroll', function () {
        paralexHead($('.header-box'))
        paralexBlock($('.video-block'))
    })
    if ($('.ideas-block').length) {
        var waypoints = $('.ideas-block').waypoint(function (direction) {
            $('.ideas-block').removeClass('inactive')
        }, {
            offset: '55%'
        })
    }
    if ($('.ideas-block').length) {
        var waypoints2 = $('.share-idea-block').waypoint(function (direction) {
            $('.share-idea-block').removeClass('inactive')
        }, {
            offset: '55%'
        })
    }
    $('.menu-bar').on('click', function () {
        var _this = $(this);
        if (_this.siblings('ul.on').length) {
            _this.siblings('ul').removeClass('on')
        } else {
            _this.siblings('ul').addClass('on')
        }
        return false
    })
    $('.fancybox-media').fancybox({
        padding: 0,
        width: 854,
        height: 480,
        openEffect: 'none',
        closeEffect: 'none',
        helpers: {
            media: {}
        }
    })

    //Magic Line Start
    if ($('.main-menu').length) {
        $("ul.main-menu").append("<li id='magic-line'></li>");
        /* Cache it */
        var $magicLine = $("#magic-line");
        var $el, leftPos, newWidth, currentwidth = $(".current").width();
        if ($('ul.main-menu>li:first-child>a').hasClass("current"))
            currentwidth = 0;
        $magicLine
            .width(currentwidth)
            .css("left", $(".current").position().left)
            .data("origLeft", $magicLine.position().left)
            .data("origWidth", $magicLine.width());
        $("ul.main-menu>li").find("a").hover(function () {
            $el = $(this);
            leftPos = $el.position().left - 15;
            newWidth = $el.parent().width();
            if ($el.hasClass('bulb-logo')) {
                $magicLine.stop().animate({
                    width: 0
                });
            } else {
                $magicLine.stop().animate({
                    left: leftPos,
                    width: newWidth
                });
            }
        }, function () {
            $magicLine.stop().animate({
                left: $magicLine.data("origLeft"),
                width: $magicLine.data("origWidth")
            });
        });
    }
    //Magic Line End

    //Table Responsive Start

    var theadData = [];

    function responsiveTable() {
        if ($('.tableFormatInside,.tableFormatFirst').length) {
            $('.tableFormatInside,.tableFormatFirst').each(function () {
                var thisgrid = $(this);
                var trs = thisgrid.find('th').each(function () {
                    theadData.push($(this).text());
                });
                thisgrid.find('tr').each(function () {
                    var totaltd = $(this).find('td');
                    for (var i = 0; i < totaltd.length; i++) {
                        $(this).children('td:eq(' + i + ')').prepend('<span class="thead">' + theadData[i] + '</span>')
                    }
                })
            })
        }

    }

    function unResponsiveTable() {
        if ($('span.thead').length) {
            $('span.thead').remove()
        }
    }
    //Table Responsive End



    //Resize Function Start
    var checkTable = function () {
        if (Modernizr.mq('(min-width: 769px)')) {
            unResponsiveTable()
        } else if (Modernizr.mq('(max-width: 769px)')) {
            if ($('span.thead').length == 0) {
                responsiveTable()
            }
        }
    };
    var resizeFunction = function () {
        checkTable()
        if (Modernizr.mq('(max-width: 600px)')) {
            var windowWidth = $(window).width(),
                scale;
            scale = windowWidth / 600;
            if ($('.middle-section')) {
                $('.middle-section').css('transform', 'scale(' + scale + ')')
            }
        } else {
            if ($('.middle-section')) {
                $('.middle-section').removeAttr('style')
            }
        }
    };
    resizeFunction()
    $(window).on('resize', resizeFunction)
})
