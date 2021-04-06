var chart;
$(document).ready(function() {
    cals.init({
        slick: $("[data-slick]"),
        selectpicker: $(".selectpicker"),
        innerscroll: $(".inner-scroll"),

        datepicker_inline: $("#datepicker_inline"),
        timepicker: $("#timepicker, #timepicker2, #timepicker3, #timepicker4"),
        datepicker: $("#datepicker"),
        datepicker2: $("#datepicker2"),
        load_more_btn: ".load-more a",
        load_more_container: ".load-items-container",

    });
});

var self;
var cals = {
    init: function(options) {
        this.settings = options;
        self = this;
        this.utilities();
        this.configureModal();
        this.datepickers();
        this.selectpicker();
        this.mainSlider();
        this.stickyHeader();
        this.loadmoreitems();

    },

    // selectpicker
    selectpicker: function() {
        $.fn.selectpicker.Constructor.BootstrapVersion = '4';
        if (/Android|webOS|iPhone|BlackBerry/i.test(navigator.userAgent)) {
            $.fn.selectpicker.Constructor.DEFAULTS.mobile = true;
        }
        cals.settings.selectpicker.selectpicker({
            container: 'body',
            size: 8,
            liveSearchPlaceholder: 'Search'
        });
    },
    mainSlider: function() {

        var $slider = $('.main-banner .slick-instance');

        if ($slider.length) {
            var slidesCount;
            var currentSlide;
            var sliderCounter = $(".slider-counter .total");
            var activeSlideNum = $(".slider-counter .current");
            var updateSliderCounter = function(slick, currentIndex) {
                currentSlide = slick.slickCurrentSlide() + 1;
                slidesCount = slick.slideCount;
                $(sliderCounter).text("0" + slidesCount);
                $(activeSlideNum).text("0" + currentSlide);

            };
            $slider.on('init', function(event, slick) {
                // $slider.append(sliderCounter);
                updateSliderCounter(slick);
            });
            $slider.on('afterChange', function(event, slick, currentSlide) {
                updateSliderCounter(slick, currentSlide);
            });

        }
    },
    loadmoreitems: function() {
        $(this.settings.load_more_btn).on("click", function(e) {
            e.preventDefault();
            var prnt = $(this);
            var dataurl = $(this).attr("data-path");
            if (dataurl) {
                $(".load-more").addClass("loading");
                setTimeout(function() {
                    $.ajax({
                        url: dataurl,
                        dataType: "html",
                        success: function(responseText) {
                            if (responseText != "") {
                                var dataToInsert = $.parseHTML(responseText);
                                $(self.settings.load_more_container).append(dataToInsert);
                                //contentWayPoint();
                            }
                            $(".load-more").removeClass("loading");
                        }
                    })
                }, 1000);
            }
        });
    },
    stickyHeader: function() {
        $(window).scroll(function() {

            if ($(this).scrollTop() > 110) {
                $("header").addClass("sticky");

            } else {
                $("header").removeClass("sticky");

            }
        });
    },
    utilities: function() {
        AOS.init({
            delay: 100, // values from 0 to 3000, with step 50ms
            duration: 900, // values from 0 to 3000, with step 50ms
        });
        //slick  slider 
        cals.settings.slick.slick();
        // mCustomScrollbar
        //cals.settings.innerscroll.mCustomScrollbar(); 
        $(".menu-icon").click(function(e) {
            e.preventDefault();
            $("body").toggleClass("menu-active");
        });


        $(".remove-bar").on("click", function(e) {
            e.preventDefault();

            $(".page-name").hide();
            // $(this).hide();
        });

        $(".wishlist").click(function(e) {
            e.preventDefault();
            $(this).addClass("liked");
        });

        $(".remove-dropdown").click(function(e) {
            e.preventDefault();
            $(".categories .dropdown-menu").removeClass("show");
        });


    },
    // datepickers
    datepickers: function() {
        this.settings.timepicker.datetimepicker({
            format: 'LT',
            ignoreReadonly: true,
            keepOpen: false,
        });
        this.settings.datepicker_inline.datetimepicker({
            inline: true,
            format: 'DD/MM/YYYY',
        });
        this.settings.datepicker.datetimepicker({
            format: 'L',
            keepOpen: false,
            ignoreReadonly: true,
        });
        this.settings.datepicker2.datetimepicker({
            format: 'L',
            keepOpen: false,
            ignoreReadonly: true,
        });
    },
    // modal
    configureModal: function() {
        $("body").on("click", "*[data-toggle='custom-modal']", function(e) {
            e.preventDefault();
            $(".custom-modal").removeClass("large");
            var url = $(this).attr("data-path");
            var size = $(this).attr("data-size");
            var class_name = $(this).attr("data-class");
            $(".custom-modal").removeClass("large");
            $(".custom-modal").removeClass("medium");
            $(".custom-modal").removeClass("small");
            $.get(url, function(data) {
                $(".custom-modal").modal("show");
                $(".custom-modal .modal-body").html(data);

                if (size) {
                    $(".custom-modal").addClass(size);
                }
                if (class_name) {
                    $(".custom-modal").addClass(class_name);
                }
                setTimeout(function() {
                    $(".custom-modal .modal-body").addClass("show");
                }, 200);
                $("body").addClass("remove-scroll");
            });
        });
        $(".modal").on("hidden.bs.modal", function() {
            $(".custom-modal .modal-body").removeClass("show");
            $(".custom-modal .modal-body").empty();
            $(".custom-modal").removeClass("account-modal");
            $("body").removeClass("remove-scroll");
            $(".custom-modal").removeClass("large");
            $(".custom-modal").removeClass("medium");
            $(".custom-modal").removeClass("small");
        });
    },


};