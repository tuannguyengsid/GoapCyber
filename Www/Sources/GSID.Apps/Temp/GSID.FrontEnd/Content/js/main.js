

(function ($) {
    "use strict";

    imageFunction();

    function imageFunction() {
        $("[data-bg-image]").each(function () {
            const img = $(this).data("bg-image");
            $(this).css({
                backgroundImage: "url(" + img + ")",
            });
        });
    }

    /*----------------------------------
     //TweenMax Mouse Effect
     -----------------------------------*/
    $(".motion-effects-wrap").mousemove(function (e) {
        parallaxIt(e, ".motion-effects1", -100);
        parallaxIt(e, ".motion-effects2", -200);
        parallaxIt(e, ".motion-effects3", 100);
        parallaxIt(e, ".motion-effects4", 200);
        parallaxIt(e, ".motion-effects5", -50);
        parallaxIt(e, ".motion-effects6", 50);
    });
    function parallaxIt(e, target_class, movement) {
        const $wrap = $(e.target).parents(".motion-effects-wrap");
        if (!$wrap.length) return;
        const $target = $wrap.find(target_class);
        const relX = e.pageX - $wrap.offset().left;
        const relY = e.pageY - $wrap.offset().top;
        TweenMax.to($target, 1, {
            x: ((relX - $wrap.width() / 2) / $wrap.width()) * movement,
            y: ((relY - $wrap.height() / 2) / $wrap.height()) * movement,
        });
    }

    /*-------------------------------------------------------------------------------
	  Navbar 
	-------------------------------------------------------------------------------*/

    function backToTop(button) {
        var $button = $(button),
            $body = $('body'),
            windowH = $(window).height();
        if ($(window).scrollTop() > windowH / 2) {
            $button.addClass('is-visible');
        }
        $(window).scroll(function () {
            if ($(this).scrollTop() > windowH / 2) {
                $button.addClass('is-visible');
            } else {
                $button.removeClass('is-visible');
            }
        });

        function scrollToTop() {
            $body.addClass('blockSticky');
            var speed = $(window).scrollTop() / 4 > 500 ? $(window).scrollTop() / 4 : 500;
          
            $("html, body").animate({
                scrollTop: 0
            }, speed, function () {
                $body.removeClass('blockSticky');
            });
        }

        $button.on('click', function (e) {
            scrollToTop();
            e.preventDefault();
        });
    }
    backToTop('.js-back-to-top');

    function navbarFixed() {
        if ($(".header_area").length) {
            $(window).scroll(function () {
                var scroll = $(window).scrollTop();
                if (scroll) {
                    $(".header_area").addClass("navbar_fixed");
                } else {
                    $(".header_area").removeClass("navbar_fixed");
                }
            });
        }
    }
    navbarFixed();

    function offcanvasActivator() {
        if ($(".bar_menu").length) {
            $(".bar_menu").on("click", function () {
                $("#menu").toggleClass("show-menu");
            });
            $(".close_icon").on("click", function () {
                $("#menu").removeClass("show-menu");
            });
        }
    }
    offcanvasActivator();

    $(".offcanfas_menu .dropdown").on("show.bs.dropdown", function (e) {
        $(this).find(".dropdown-menu").first().stop(true, true).slideDown(400);
    });
    $(".offcanfas_menu .dropdown").on("hide.bs.dropdown", function (e) {
        $(this).find(".dropdown-menu").first().stop(true, true).slideUp(500);
    });

    /*-------------------------------------------------------------------------------
	  mCustomScrollbar js
	-------------------------------------------------------------------------------*/
    $(window).on("load", function () {
        if ($(".mega_menu_two .scroll").length) {
            $(".mega_menu_two .scroll").mCustomScrollbar({
                mouseWheelPixels: 50,
                scrollInertia: 0,
            });
        }
    });

    /*-------------------------------------------------------------------------------
	  WOW js
	-------------------------------------------------------------------------------*/
    function wowAnimation() {
        new WOW({
            offset: 100,
            animateClass: "animated",
            mobile: true,
        }).init();
    }
    wowAnimation();

    /*-------------------------------------------------------------------------------
	  service_carousel js
	-------------------------------------------------------------------------------*/
    function serviceSlider() {
        var service_slider = $(".service_carousel");
        if (service_slider.length) {
            service_slider.owlCarousel({
                loop: true,
                margin: 15,
                items: 4,
                autoplay: true,
                smartSpeed: 2000,
                responsiveClass: true,
                nav: true,
                dots: false,
                stagePadding: 100,
                navText: [, '<i class="ti-arrow-left"></i>'],
                responsive: {
                    0: {
                        items: 1,
                        stagePadding: 0,
                    },
                    578: {
                        items: 2,
                        stagePadding: 0,
                    },
                    992: {
                        items: 3,
                        stagePadding: 0,
                    },
                    1200: {
                        items: 3,
                    },
                },
            });
        }
    }
    serviceSlider();
    

    /*-------------------------------------------------------------------------------
	  service_carousel js
	-------------------------------------------------------------------------------*/
    function recruitmentSlider() {
        var recruitment_slider = $(".recruitment_carousel");
        if (recruitment_slider.length) {
            recruitment_slider.owlCarousel({
                loop: true,
                margin: 15,
                items: 4,
                autoplay: true,
                smartSpeed: 2000,
                responsiveClass: true,
                nav: true,
                dots: false,
                stagePadding: 100,
                navText: [ '<i class="icon-angle-right"></i>', '<i class="icon-angle-left"></i>'],
                responsive: {
                    0: {
                        items: 1,
                        stagePadding: 0,
                    },
                    578: {
                        items: 2,
                        stagePadding: 0,
                    },
                    992: {
                        items: 3,
                        stagePadding: 0,
                    },
                    1200: {
                        items: 3,
                    },
                },
            });
        }
    }
    recruitmentSlider();
    
    /*-------------------------------------------------------------------------------
	  app_screenshot_slider js
	-------------------------------------------------------------------------------*/
    function appScreenshot() {
        var app_screenshotSlider = $(".app_screenshot_slider");
        if (app_screenshotSlider.length) {
            app_screenshotSlider.owlCarousel({
                loop: true,
                margin: 10,
                items: 5,
                autoplay: false,
                smartSpeed: 2000,
                responsiveClass: true,
                nav: false,
                dots: true,
                responsive: {
                    0: {
                        items: 1,
                    },
                    650: {
                        items: 2,
                    },
                    776: {
                        items: 4,
                    },
                    1199: {
                        items: 5,
                    },
                },
            });
        }
    }
    appScreenshot();

    function breakscrumslider() {
        var app_screenshotSlider = $(".breadcrumb_area_slider");
        if (app_screenshotSlider.length) {
            app_screenshotSlider.owlCarousel({
                loop: true,
                margin: 10,
                items: 1,
                autoplay: false,
                smartSpeed: 2000,
                responsiveClass: true,
                nav: false,
                dots: true,
                responsive: {
                    0: {
                        items: 1,
                    },
                    650: {
                        items: 1,
                    },
                    776: {
                        items: 1,
                    },
                    1199: {
                        items: 1,
                    },
                },
            });
        }
    }
    breakscrumslider();
    /*-------------------------------------------------------------------------------
	  Portfolio isotope js
	-------------------------------------------------------------------------------*/
    function portfolioMasonry() {
        var portfolio = $("#work-portfolio");
        if (portfolio.length) {
            portfolio.imagesLoaded(function () {
                // images have loaded
                // Activate isotope in container
                portfolio.isotope({
                    // itemSelector: ".portfolio_item",
                    layoutMode: "masonry",
                    filter: "*",
                    animationOptions: {
                        duration: 1000,
                    },
                    transitionDuration: "0.5s",
                    masonry: {},
                });

                // Add isotope click function
                $("#portfolio_filter div").on("click", function () {
                    $("#portfolio_filter div").removeClass("active");
                    $(this).addClass("active");

                    var selector = $(this).attr("data-filter");
                    portfolio.isotope({
                        filter: selector,
                        animationOptions: {
                            animationDuration: 750,
                            easing: "linear",
                            queue: false,
                        },
                    });
                    return false;
                });
            });
        }
    }
    portfolioMasonry();

    /*-------------------------------------------------------------------------------
	  jobFilter js
	-------------------------------------------------------------------------------*/
    function jobFilter() {
        var jobsfilter = $("#tab_filter");
        if (jobsfilter.length) {
            jobsfilter.imagesLoaded(function () {
                // images have loaded
                // Activate isotope in container
                jobsfilter.isotope({
                    itemSelector: ".item",
                });

                // Add isotope click function
                $("#job_filter div").on("click", function () {
                    $("#job_filter div").removeClass("active");
                    $(this).addClass("active");

                    var selector = $(this).attr("data-filter");
                    jobsfilter.isotope({
                        filter: selector,
                        animationOptions: {
                            animationDuration: 750,
                            easing: "linear",
                            queue: false,
                        },
                    });
                    return false;
                });
            });
        }
    }
    jobFilter();

    /*-------------------------------------------------------------------------------
	  blogMasonry js
	-------------------------------------------------------------------------------*/
    function blogMasonry() {
        var blog = $("#blog_masonry");
        if (blog.length) {
            blog.imagesLoaded(function () {
                blog.isotope({
                    layoutMode: "masonry",
                });
            });
        }
    }
    blogMasonry();

    if ($(".height-emulator").length) {
        $(".height-emulator").css({
            height: $("footer").outerHeight(true)
        });
    }

    /*-------------------------------------------------------------------------------
	  popup js
	-------------------------------------------------------------------------------*/
    function popupGallery() {
        if ($(".img_popup,.image-link").length) {
            $(".img_popup,.image-link").each(function () {
                $(".img_popup,.image-link").magnificPopup({
                    type: "image",
                    tLoading: "Loading image #%curr%...",
                    removalDelay: 300,
                    mainClass: "mfp-with-zoom mfp-img-mobile",
                    gallery: {
                        enabled: true,
                        navigateByImgClick: true,
                        preload: [0, 1], // Will preload 0 - before current, and 1 after the current image,
                    },
                });
            });
        }
    }
    popupGallery();

    
 
    /*-------------------------------------------------------------------------------
	  Parallax Effect js
	-------------------------------------------------------------------------------*/
    function parallaxEffect() {
        if ($(".parallax-effect,#apps_craft_animation").length) {
            $(".parallax-effect,#apps_craft_animation").parallax();
        }
    }
    parallaxEffect();

    /*-------------------------------------------------------------------------------
	  preloader js
	-------------------------------------------------------------------------------*/
    function loader() {
        $(window).on("load", function () {
            $("#ctn-preloader").addClass("loaded");
            // Una vez haya terminado el preloader aparezca el scroll

            if ($("#ctn-preloader").hasClass("loaded")) {
                // Es para que una vez que se haya ido el preloader se elimine toda la seccion preloader
                $("#preloader")
                    .delay(900)
                    .queue(function () {
                        $(this).remove();
                    });
            }
        });
    }
    loader();

    /*-------------------------------------------------------------------------------
	  tooltip js
	-------------------------------------------------------------------------------*/
    if ($('[data-toggle="tooltip"]').length) {
        $('[data-toggle="tooltip"]').tooltip();
    }
    /*-------------------------------------------------------------------------------
	  Pricing Filter js
	-------------------------------------------------------------------------------*/
    if ($("#slider-range").length) {
        $("#slider-range").slider({
            range: true,
            min: 5,
            max: 150,
            values: [5, 100],
            slide: function (event, ui) {
                $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
            },
        });
        $("#amount").val(
            "$" +
            $("#slider-range").slider("values", 0) +
            " - $" +
            $("#slider-range").slider("values", 1)
        );
    }

    /*-------------------------------------------------------------------------------
	  search js
	-------------------------------------------------------------------------------*/
    $(".search-btn").on("click", function () {
        $("body").addClass("open");
        setTimeout(function () {
            $(".search-input").focus();
        }, 500);
        return false;
    });
    $(".close_icon").on("click", function () {
        $("body").removeClass("open");
        return false;
    });

    /*-------------------------------------------------------------------------------
	  active dropdown
	-------------------------------------------------------------------------------*/
    function active_dropdown() {
        if ($(window).width() < 992) {
            $(".menu li.submenu > a").on("click", function (event) {
                event.preventDefault();
                $(this).parent().find("ul").first().toggle(700);
                $(this).parent().siblings().find("ul").hide(700);
            });
        }
    }
    active_dropdown();

    /*-------------------------------------------------------------------------------
	  hamberger menu
	-------------------------------------------------------------------------------*/
    function hamberger_menu() {
        if ($(".burger_menu").length) {
            $(".burger_menu").on("click", function () {
                $(this).toggleClass("open");
                $("body").removeClass("menu-is-closed").addClass("menu-is-opened");
            });
            $(".close, .click-capture").on("click", function () {
                $("body").removeClass("menu-is-opened").addClass("menu-is-closed");
            });
        }
    }
    hamberger_menu();

    /*-------------------------------------------------------------------------------
	  Full screen sections 
	-------------------------------------------------------------------------------*/
    function doAnimations(elements) {
        var animationEndEvents =
            "webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend";
        elements.each(function () {
            var $this = $(this);
            var $animationDelay = $this.data("delay");
            var $animationType = "animated " + $this.data("animation");
            $this.css({
                "animation-delay": $animationDelay,
                "-webkit-animation-delay": $animationDelay,
            });
            $this.addClass($animationType).one(animationEndEvents, function () {
                $this.removeClass($animationType);
            });
        });
    }

    /*---------  data background -----------*/
    if ($('[data-background]').length > 0) {
        $('[data-background]').each(function () {
            $(this).css('background-image', 'url(' + $(this).attr('data-background') + ')');
        });
    }



    /*---------  grid -----------*/
    if ($('.grid').length > 0) {

        var $grid = $('.grid').imagesLoaded(function () {
            $grid.masonry({
                itemSelector: '.grid-item',
                percentPosition: true,
                columnWidth: '.grid-sizer'
            });
        });
    }


    if ($(".showcase_slider").length) {
        const sliders = $('.showcase_slider');
        sliders.on("init", function (e, slick) {
            var $firstAnimatingElements = $("div.slider:first-child").find(
                "[data-animation]"
            );
            doAnimations($firstAnimatingElements);
        });
        sliders.on("beforeChange", function (
            e,
            slick,
            currentSlide,
            nextSlide
        ) {
            var $animatingElements = $(
                'div.slider[data-slick-index="' + nextSlide + '"]'
            ).find("[data-animation]");
            doAnimations($animatingElements);
        });

        function onSliderAfterChange(event, slick, currentSlide) {
            $(event.target).data('current-slide', currentSlide);
        }

        function onSliderWheel(e) {
            var deltaY = e.originalEvent.deltaY,
                $currentSlider = $(e.currentTarget),
                currentSlickIndex = $currentSlider.data('current-slide') || 0;

            if (
                // check when you scroll up
                (deltaY < 0 && currentSlickIndex == 0) ||
                // check when you scroll down
                (deltaY > 0 && currentSlickIndex == $currentSlider.data('slider-length') - 1)
            ) {
                return;
            }

            e.preventDefault();

            if (e.originalEvent.deltaY < 0) {
                $currentSlider.slick('slickPrev');
            } else {
                $currentSlider.slick('slickNext');
            }
        }

        sliders.each(function (index, element) {
                var $element = $(element);
                $element.data('slider-length', $element.children().length);
            })
            .slick({
                infinite: false,
                slidesToShow: 1,
                vertical: true,
                verticalSwiping: true,
                slidesToScroll: 1,
                dots: false,
                arrows: false,
                responsive: [
                {
                    breakpoint: 991,
                    settings: {
                        verticalSwiping: false,
                        vertical: false,
                    },
                }
            ],
            
            })
            .on('afterChange', onSliderAfterChange)
            .on('wheel', onSliderWheel);

        function doAnimations(elements) {
            var animationEndEvents =
                "webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend";
            elements.each(function () {
                var $this = $(this);
                var $animationDelay = $this.data("delay");
                var $animationType = "animated " + $this.data("animation");
                $this.css({
                    "animation-delay": $animationDelay,
                    "-webkit-animation-delay": $animationDelay,
                });
                $this.addClass($animationType).one(animationEndEvents, function () {
                    $this.removeClass($animationType);
                });
            });
        }
    }
})(jQuery);