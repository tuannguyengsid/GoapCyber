using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Linq;
using System.Web;
using static GSID.Model.MongodbModels.Position;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class ContactFilterViewModel
    {
        public List<Contact> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class ContactCreateViewModel
    {
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Display(Name = "Email"), Required(ErrorMessage = "Email buộc phải nhập."), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Đăng ký nhận thư"), Required(ErrorMessage = "Đăng ký nhận thư buộc phải chọn.")]
        public bool IsSubscribe { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
    }

    public class ContactEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Display(Name = "Email"), Required(ErrorMessage = "Email buộc phải nhập."), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Đăng ký nhận thư"), Required(ErrorMessage = "Đăng ký nhận thư buộc phải chọn.")]
        public bool IsSubscribe { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Department> Departments { get; set; }
    }
}