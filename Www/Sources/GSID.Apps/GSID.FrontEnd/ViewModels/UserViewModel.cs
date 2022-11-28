using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class UserCreateViewModel
    {
        [Display(Name = "Họ"), Required(ErrorMessage = "Họ buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string LastName { get; set; }
        [Display(Name = "Tên đêm")]
        public string MiddleName { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string FirstName { get; set; }

        [Display(Name = "Email"), Required(ErrorMessage = "Email buộc phải nhập."), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        //[System.Web.Mvc.Remote("CheckExistingEmail", "User", HttpMethod = "POST", ErrorMessage = "Địa chỉ email này đã tồn tại")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string Password { get; set; }
        [Display(Name = "Kích hoạt tài khoản"), Required(ErrorMessage = "Kích hoạt tài khoản buộc phải chọn")]
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Quản trị viên"), Required(ErrorMessage = "Định hình tài khoản buộc phải chọn")]
        //public bool? IsAdministrator { get; set; }

        //[Display(Name = "Phân quyền"), Required(ErrorMessage = "Phân quyền là bắt buộc phải chọn.")]
        //public string RoleId { get; set; }
        [Display(Name = "Ghi chú")]
        public string Noted { get; set; }
    }

    public class UserEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Họ"), Required(ErrorMessage = "Họ buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string LastName { get; set; }
        [Display(Name = "Tên đêm")]
        public string MiddleName { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string FirstName { get; set; }

        [Display(Name = "Email"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Kích hoạt tài khoản"), Required(ErrorMessage = "Kích hoạt tài khoản buộc phải chọn")]
        public bool? IsDeleted { get; set; }
        //[Display(Name = "Quản trị viên"), Required(ErrorMessage = "Định hình tài khoản buộc phải chọn")]
        //public bool? IsAdministrator { get; set; }
        //[Display(Name = "Phân quyền"), Required(ErrorMessage = "Phân quyền là bắt buộc phải chọn.")]
        //public string RoleId { get; set; }
        [Display(Name = "Ghi chú")]
        public string Noted { get; set; }
        public string ImageUrl { get; set; }
        public string ImageChange { get; set; }
    }


    public class MemberCreateViewModel
    {
        [Display(Name = "Họ"), Required(ErrorMessage = "Họ buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string LastName { get; set; }
        [Display(Name = "Tên đêm")]
        public string MiddleName { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string FirstName { get; set; }

        [Display(Name = "Email"), Required(ErrorMessage = "Email buộc phải nhập."), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        //[System.Web.Mvc.Remote("CheckExistingEmail", "User", HttpMethod = "POST", ErrorMessage = "Địa chỉ email này đã tồn tại")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string Password { get; set; }
        [Display(Name = "Kích hoạt tài khoản"), Required(ErrorMessage = "Kích hoạt tài khoản buộc phải chọn")]
        public bool? IsDeleted { get; set; }
        public string Href { get; set; }
        public string CompanyId { get; set; }
        public string ActionLoad { get; set; }
    }

    public class MemberEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Họ"), Required(ErrorMessage = "Họ buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string LastName { get; set; }
        [Display(Name = "Tên đêm")]
        public string MiddleName { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        public string FirstName { get; set; }

        [Display(Name = "Email"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Kích hoạt tài khoản"), Required(ErrorMessage = "Kích hoạt tài khoản buộc phải chọn")]
        public bool? IsDeleted { get; set; }
        public string Href { get; set; }
        public string CompanyId { get; set; }
        public string ActionLoad { get; set; }
    }
    
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Mật khẩu buộc phải nhập.")]
        public string Password { get; set; }
        public string returnUrl { get; set; }

        [Display(Name = "Ghi nhớ?")]
        public bool IsRememberMe { get; set; }
        public string Language { get; set; }
    }

    public class ForgotViewModel
    {

        [Display(Name = "Email"), Required(ErrorMessage = "Email buộc phải nhập."), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        //[System.Web.Mvc.Remote("CheckExistingEmail", "User", HttpMethod = "POST", ErrorMessage = "Địa chỉ email này đã tồn tại")]
        public string Email { get; set; }
        public string Language { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không chính xác.")]
        [Display(Name = "Xác nhận mật khẩu"), Required(ErrorMessage = "Xác nhận mật khẩu buộc phải nhập")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Language { get; set; }
    }


    public class RegisterViewModel
    {
        [Display(Name = "Họ và tên"), Required(ErrorMessage = "Họ và tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string FullName { get; set; }

        [Display(Name = "Email"), Required(ErrorMessage = "Email buộc phải nhập."), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        //[System.Web.Mvc.Remote("CheckExistingEmail", "User", HttpMethod = "POST", ErrorMessage = "Địa chỉ email này đã tồn tại")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không chính xác.")]
        [Display(Name = "Xác nhận mật khẩu"), Required(ErrorMessage = "Xác nhận mật khẩu buộc phải nhập")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsAgree { get; set; }
        public string Language { get; set; }
    }



    public class ChangePasswordViewModel
    {
        [Display(Name = "Mật khẩu cũ"), Required(ErrorMessage = "Mật khẩu cũ buộc phải nhập")]
        public string PasswordOld { get; set; }

        [Display(Name = "Mật khẩu mới"), Required(ErrorMessage = "Mật khẩu mới buộc phải nhập")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu mới không chính xác.")]
        [Display(Name = "Xác nhận mật khẩu mới"), Required(ErrorMessage = "Xác nhận mật khẩu mới buộc phải nhập")]
        public string ConfirmNewPassword { get; set; }
    }
}