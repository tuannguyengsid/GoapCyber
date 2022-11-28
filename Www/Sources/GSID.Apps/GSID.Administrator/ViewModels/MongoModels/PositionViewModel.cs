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
    public class PositionFilterViewModel
    {
        public List<Position> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class PositionCreateViewModel
    {
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnAvailable", "Position", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnAvailable", "Position", "", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Phòng ban"), Required(ErrorMessage = "Phòng ban buộc phải chọn.")]
        public string DepartmentId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Department> Departments { get; set; }
    }

    public class PositionEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnIdAvailable", "Position", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên"), Required(ErrorMessage = "Tên buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnIdAvailable", "Position", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Phòng ban"), Required(ErrorMessage = "Phòng ban buộc phải chọn.")]
        public string DepartmentId { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<Department> Departments { get; set; }
    }
}