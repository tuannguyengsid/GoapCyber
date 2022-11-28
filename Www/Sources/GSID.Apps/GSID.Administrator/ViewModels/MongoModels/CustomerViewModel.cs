using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class CustomerVitaeFilterViewModel
    {
        public List<Customer> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class CustomerCreateViewModel
    {
        //[Display(Name = "Họ và tên"), Required(ErrorMessage = "Họ và tên thông tin buộc phải nhập")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Display(Name = "Email"), Required(ErrorMessage = "Email buộc phải nhập."), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        //[Display(Name = "Phường/xã"), Required(ErrorMessage = "Phường/xã thông tin buộc phải chọn")]
        [Display(Name = "Phường/xã")]
        public string ProvinceId { get; set; }
        //[Display(Name = "Quận/huyện"), Required(ErrorMessage = "Quận/huyện thông tin buộc phải chọn")]
        [Display(Name = "Quận/huyện")]
        public string DistrictId { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Nhận thông báo email"), Required(ErrorMessage = "Nhận thông báo email thông tin buộc phải chọn")]
        public bool IsSubscribe { get; set; }
        [Display(Name = "Liên hệ"), Required(ErrorMessage = "Liên hệ thông tin buộc phải chọn")]
        public bool IsContact { get; set; }
        [Display(Name = "Ghi chú")]
        public string Noted { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
    }

    public class CustomerEditViewModel
    {
        public string Id { get; set; }
        //[Display(Name = "Họ và tên"), Required(ErrorMessage = "Họ và tên thông tin buộc phải nhập")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        //[Display(Name = "Phường/xã"), Required(ErrorMessage = "Phường/xã thông tin buộc phải chọn")]
        [Display(Name = "Phường/xã")]
        public string ProvinceId { get; set; }
        //[Display(Name = "Quận/huyện"), Required(ErrorMessage = "Quận/huyện thông tin buộc phải chọn")]
        [Display(Name = "Quận/huyện")]
        public string DistrictId { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Nhận thông báo email"), Required(ErrorMessage = "Nhận thông báo email thông tin buộc phải chọn")]
        public bool IsSubscribe { get; set; }
        [Display(Name = "Liên hệ"), Required(ErrorMessage = "Liên hệ thông tin buộc phải chọn")]
        public bool IsContact { get; set; }
        [Display(Name = "Ghi chú")]
        public string Noted { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
    }

    public class CustomerDistrictFilterViewModel
    {
        [Display(Name = "Quận/huyện"), Required(ErrorMessage = "Quận/huyện thông tin buộc phải chọn")]
        public string DistrictId { get; set; }
        public List<District> Districts { get; set; }
    }

    
}