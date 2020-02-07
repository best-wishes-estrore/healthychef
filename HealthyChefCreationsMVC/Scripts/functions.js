(function ($, window) {
    var $wrapper = null, $doc = $(document), $win = $(window);

    $doc.on('ready', function () {
        $wrapper = $('#wrapper');

        // fullscreen images function
        var fullScreen = function (img) {
            // vars
            var win_w = $wrapper.width() + 100,
				win_h = $wrapper.height(),
				img_w = parseInt($(img).attr('width')),
				img_h = parseInt($(img).attr('height')),
				win_ratio = win_h / win_w,
				img_ratio = img_h / img_w;

            if (img_ratio < win_ratio) {
                $(img).css({
                    height: win_h,
                    width: win_h / img_ratio
                });
            } else {
                $(img).css({
                    height: win_w * img_ratio,
                    width: win_w
                });
            }
        }

        // set fullscreen pictures
        $('img.fullscreen').each(function () {
            fullScreen(this);
        });

        $win.resize(function () {
            $('img.fullscreen').each(function () {
                fullScreen(this);
            });
        });

        //if ($('#slider').length) {
        //    // init home slideshow
        //    $('#slider').carouFredSel({
        //        width: '100%',
        //        height: '385px',
        //        items: {
        //            visible: 1
        //        },
        //        scroll: {
        //            pauseDuration: 5000,
        //            fx: 'crossfade'
        //        }
        //    });

        //}

    });

})(jQuery, window)

$(document).ready(function () {
    $('input.page-button').each(function (index, element) {
        //alert('foo');
        $(element).removeClass('page-button');//.css('width', '107px');
        $(element).addClass('page-button-submit');
        //alert('bar');
    });
    //
    //if ($.browser.msie) // this was dep in 1.3 - rc {
    //    $('.page-button').css('font-weight', 'bold');
    //}
    //$(document)
		//.on('mouseenter', '#navigation li', function () {
		//	var self = $(this);
		//	if (self.find('ul').length) {
		//	    self.addClass('expanded').find('ul').stop(true, true).slideDown('fast');
		//	}
		//}).on('mouseleave', '#navigation li', function () {
		//	var self = $(this);
		//	self.find('ul').stop(true, true).slideUp('fast', function () {
		//	    self.removeClass('expanded');
		//	});
		//});
});
