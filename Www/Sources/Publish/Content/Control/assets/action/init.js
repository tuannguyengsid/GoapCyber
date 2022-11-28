
$(function () {
	
	//FSEditor
	//$(".fullscreen").fseditor({maxHeight: 500});

	 // iPhone like button Toggle (uncommented because already activated in demo.js)
	 // $('.toggle').toggles({on:true});

	// Autogrow Textarea
	$('textarea.autosize').autosize({append: "\n"});

	
	$('#showref').hide();
	$('.summernote').summernote({
		height: 100,
		placeholder: 'start typing',
		toolbar: [
			// [groupName, [list of button]]
			['style', ['bold', 'italic', 'underline', 'clear']],
			['fontsize', ['fontsize']],
			['color', ['color']],
			['para', ['ul', 'ol']]
		  ]
	});
	
    //placeholder
    $(".select-placeholder").select2({
        placeholder: $(this).attr('data-placeholder'),
        allowClear: true
    });
	
	$(".refer").change(function(){
		var value = $(this).val();
		try {
			var sp = value.split('_');
			if(sp[1] == 'showref')
				$('#showref').slideDown({duration:200});
			else
				$('#showref').slideUp({duration:200});
		}
		catch(err) {
			$('#showref').slideUp({duration:200});
		}
	});
	
	$('.mask').inputmask();
	
	$(".datepicker").datepicker({
		format: 'dd/mm/yyyy',
		todayHighlight: true,
		allowClear: true
	});
	
	$('#multi-select-carrer').multiSelect({
		selectableHeader: "<input type='text' class='form-control' style='margin-bottom: 10px;'  autocomplete='off' placeholder='Lọc ngành nghề...'>",
		selectionHeader: "<input type='text' class='form-control' style='margin-bottom: 10px;' autocomplete='off' placeholder='Lọc ngành nghề...'>",
		afterInit: function(ms){
			var that = this,
			$selectableSearch = that.$selectableUl.prev(),
			$selectionSearch = that.$selectionUl.prev(),
			selectableSearchString = '#'+that.$container.attr('id')+' .ms-elem-selectable:not(.ms-selected)',
			selectionSearchString = '#'+that.$container.attr('id')+' .ms-elem-selection.ms-selected';

			that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
			.on('keydown', function(e){
				if (e.which === 40){
					that.$selectableUl.focus();
					return false;
				}
			});

			that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
			.on('keydown', function(e){
				if (e.which == 40){
					that.$selectionUl.focus();
					return false;
				}
			});
		},
		afterSelect: function(){
			this.qs1.cache();
			this.qs2.cache();
		},
		afterDeselect: function(){
			this.qs1.cache();
			this.qs2.cache();
		}
	});

	$('[data-toggler]').click(function () {
		if (($(this).parents('.toggler').children('.toggler-content').css('display'))=="none") {
			$(this).parents('.toggler').children('.toggler-content').slideDown({duration:200});
			$(this).parents('.toggler').children('.toggler-header').children('.fa').toggleClass('fa-angle-down fa-angle-left');
		} else {
			$(this).parents('.toggler').children('.toggler-content').slideUp({duration:200});
			$(this).parents('.toggler').children('.toggler-header').children('.fa').toggleClass('fa-angle-down fa-angle-left');
		}
	});
});