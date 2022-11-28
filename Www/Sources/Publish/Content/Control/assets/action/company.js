jQuery(document).ready(function() {
    $('.dropdown-menu').click(function(e) {
        e.stopPropagation();
    });

	$(".datepicker").datepicker({todayHighlight: true});
	$('.mask').inputmask();

    $('[data-toggle="popover"]').popover();

    // Enabling Popover Example 2 - JS (hidden content and title capturing)
    $("#popoverExampleOne").popover({
        html : true, 
        content: function() {
            return $('#popoverExampleTwoHiddenContent2').html();
        }//,
        // title: function() {
        //     return $('#popoverExampleTwoHiddenTitle').html();
        // }
    });

    $("#popoverExampleTwo").popover({
        html : true, 
        content: function() {
            return $('#popoverExampleTwoHiddenContent').html();
        }//,
        // title: function() {
        //     return $('#popoverExampleTwoHiddenTitle').html();
        // }
    });
});

