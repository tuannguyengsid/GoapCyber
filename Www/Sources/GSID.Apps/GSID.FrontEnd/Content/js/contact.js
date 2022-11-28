
    
;(function($) {
    "use strict";

    
    jQuery.validator.addMethod('answercheck', function (value, element) {
        return this.optional(element) || /^\bcat\b$/.test(value)
    }, "type the correct answer -_-");

    // validate contactForm form
    $(function() {
        $('#contactForm').validate({
            rules: {
                name: {
                    required: true,
                    minlength: 2
                },
                name2: {
                    required: true,
                    minlength: 2
                },
                email: {
                    required: true,
                    email: true
                },
                subject: {
                    required: true,
                    minlength: 12
                },
                message: {
                    required: true,
                    minlength: 20
                }
            },
            messages: {
                name: {
                    required: "Vui lòng nhập tên?",
                    minlength: "Bạn cần nhập tối tiểu 2 ký tự"
                },
                name2: {
                    required: "vui lòng nhập tên",
                    minlength: "Bạn cần nhập tối tiểu 2 ký tự"
                },
                email: {
                    required: "Vui lòng nhập email"
                },
                subject: {
                    required: "Vui lòng nhập tiêu đề"
                },
                message: {
                    required: "Bạn cần viết nội dung",
                    minlength: "Xin vui lòng nhập ký tự"
                }
            },
            submitHandler: function(form) {
                $(form).ajaxSubmit({
                    type:"POST",
                    data: $(form).serialize(),
                    url:"",
                    success: function() {
                        $('#contactForm :input').attr('disabled', 'disabled');
                        $('#contactForm').fadeTo( "slow", 1, function() {
                            $(this).find(':input').attr('disabled', 'disabled');
                            $(this).find('label').css('cursor','default');
                            $('#success').fadeIn()
                        })
                    },
                    error: function() {
                        $('#contactForm').fadeTo( "slow", 1, function() {
                            $('#error').fadeIn()
                        })
                    }
                })
            }
        })
    })
        
})(jQuery)
