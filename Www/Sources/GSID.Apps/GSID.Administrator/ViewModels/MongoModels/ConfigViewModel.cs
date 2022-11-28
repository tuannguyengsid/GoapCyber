using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;
using static GSID.Model.ExtraEntities.SmsBranchCmcConfig;
using GSID.Model.MongodbModels;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class LogoViewModel
    {
        public string ImageSrc { get; set; }
        public string ImageChange { get; set; }
    }

    public class SiteInformationViewModel
    {
        public string WhiteLogoSrc { get; set; }
        public string BlackLogoSrc { get; set; }
        public string IconSrc { get; set; }

        [Display(Name = "Địa chỉ Site lưu trữ")]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Vui lòng nhập đúng định dạng: http(s)://")]
        public string UrlAddressSiteMultimedia { get; set; }
        [Display(Name = "Đường dẫn Site lưu trữ")]
        public string PathAddressSiteMultimedia { get; set; }
        [Display(Name = "Địa chỉ website chính")]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Vui lòng nhập đúng định dạng: http(s)://")]
        public string UrlAddressSite { get; set; }

        [Display(Name = "Địa chỉ website quản trị")]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Vui lòng nhập đúng định dạng: http(s)://")]
        public string UrlBackEndSite { get; set; }
        [Display(Name = "Tiền tố")]
        public string Prefix { get; set; }

        [Display(Name = "Tiêu đề trang web")]
        public string SiteTitleVn { get; set; }
        [Display(Name = "Tiêu đề trang web")]
        public string SiteTitleEn { get; set; }
        [Display(Name = "Hotline")]
        public string HotlineVn { get; set; }
        [Display(Name = "Hiển thị Hotline")]
        public string DisplayHotlineVn { get; set; }
        [Display(Name = "Hotline")]
        public string HotlineEn { get; set; }
        [Display(Name = "Hiển thị Hotline")]
        public string DisplayHotlineEn { get; set; }
        [Display(Name = "Thời gian làm việc")]
        [AllowHtml]
        public string WorkingTimeVn { get; set; }
        [Display(Name = "Thời gian làm việc")]
        [AllowHtml]
        public string WorkingTimeEn { get; set; }

        [Display(Name = "Tên công ty")]
        public string CompanyNameVn { get; set; }
        [Display(Name = "Tên công ty")]
        public string CompanyNameEn { get; set; }
        [Display(Name = "Mã số thuế")]
        public string TaxCode { get; set; }
        [Display(Name = "Giấy phép kinh doanh")]
        public string BusinessLicenseVn { get; set; }
        [Display(Name = "Giấy phép kinh doanh")]
        public string BusinessLicenseEn { get; set; }
        [Display(Name = "Địa chỉ")]
        [AllowHtml]
        public string AddressVn { get; set; }
        [Display(Name = "Địa chỉ")]
        [AllowHtml]
        public string AddressEn { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng địa chỉ email")]
        public string EmailVn { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng địa chỉ email")]
        public string EmailEn { get; set; }
        [Display(Name = "Keyword(Vn)")]
        public string KeywordSEOVn { get; set; }
        [Display(Name = "Keyword(En)")]
        public string KeywordSEOEn { get; set; }
        [Display(Name = "Description(Vn)")]
        public string DescriptionSEOVn { get; set; }
        [Display(Name = "Description(En)")]
        public string DescriptionSEOEn { get; set; }
        [AllowHtml]
        [Display(Name = "Thẻ html header")]
        public string TagHeader { get; set; }
        [Display(Name = "Css header")]
        public string CssHeader { get; set; }
        [Display(Name = "Script header")]
        public string ScriptHeader { get; set; }
        [AllowHtml]
        [Display(Name = "Thẻ html footer")]
        public string TagFooter { get; set; }
        [Display(Name = "Script footer")]
        public string ScriptFooter { get; set; }

        [Display(Name = "Ẩn/Hiện trang tài khoản")]
        public bool IsShowAuthenticationPage { get; set; }
    }

    public class SaveFilePathConfigViewModel
    {
        [Display(Name = "Đường dẫn sản phẩm")]
        public string ProductPath { get; set; }
        [Display(Name = "Đường dẫn thương hiệu")]
        public string ProductBranchPath { get; set; }
        [Display(Name = "Đường dẫn nhân sự")]
        public string UserPath { get; set; }
        [Display(Name = "Đường dẫn khu cấu hình")]
        public string ConfigPath { get; set; }
        [Display(Name = "Đường dẫn tin tức")]
        public string NewsPath { get; set; }
        public string ProductCategoryPath { get; set; }
        [Display(Name = "Đường dẫn văn phòng")]
        public string SitePath { get; set; }
        [Display(Name = "Đường dẫn đối tác")]
        public string PartnerPath { get; set; }
        [Display(Name = "Đường dẫn tuyển dụng")]
        public string RecruitmentPath { get; set; }
        [Display(Name = "Đường dẫn thư viện hình ảnh")]
        public string GalleryPath { get; set; }
        //public FileManager FileManage { get; set; }
    }

    public class EmailIMapConfigViewModel
    {
        [Display(Name = "Email"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        [Required(ErrorMessage = "Email buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Mật khẩu buộc phải nhập.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "In comming server buộc phải nhập.")]
        public string InCommingServer { get; set; }
        [Display(Name = "Cổng truy cập"), Required(ErrorMessage = "Cổng truy cập buộc phải nhập.")]
        public int Port { get; set; }
    }

    public class EmailSTMPConfigViewModel
    {
        [Display(Name = "Tên hiển thị"), Required(ErrorMessage = "Tên hiển thị buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string FullName { get; set; }
        [Display(Name = "Email"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        [Required(ErrorMessage = "Email buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Mật khẩu buộc phải nhập.")]
        public string Password { get; set; }
        [Display(Name = "STMP Server"), Required(ErrorMessage = "STMP buộc phải nhập.")]
        public string STMPSever { get; set; }
        [Display(Name = "Cổng truy cập"), Required(ErrorMessage = "Cổng truy cập buộc phải nhập.")]
        public int STMPPort { get; set; }
        [Display(Name = "Imap Server")]
        public string IMAPSever { get; set; }
        [Display(Name = "Imap cổng truy cập")]
        public int IMAPPort { get; set; }

        [Display(Name = "Email nhận thông báo"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string EmailNotification { get; set; }
    }

    public class BFOAPIConfigViewModel
    {
        [Display(Name = "Địa chỉ truy cập")]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Vui lòng nhập đúng định dạng: http(s)://")]
        public string Url { get; set; }
        [Required(ErrorMessage = "AppId buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string AppId { get; set; }
        [Display(Name = "SecretKey"), Required(ErrorMessage = "Secret key buộc phải nhập.")]
        public string SecretKey  { get; set; }
    }

    public class EmailFollowConfigViewModel
    {
        [Display(Name = "Email"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Xin vui lòng nhập đúng định dạng email.")]
        [Required(ErrorMessage = "Email buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Email { get; set; }
    }

    public class SmsBranchCmcConfigViewModel
    {
        [Display(Name = "Gửi tin nhắn không Unicode"), Required(ErrorMessage = "Nội dung buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string SendUrl { get; set; }
        [Display(Name = "Ký tự giới hạn"), Required(ErrorMessage = "Ký tự giới hạn buộc phải nhập.")]
        public int MessageLimitSendUrl { get; set; }
        [Display(Name = "Gửi tin nhắn chứa Unicode"), Required(ErrorMessage = "Nội dung buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string SendUTFUrl { get; set; }
        [Display(Name = "Ký tự giới hạn"), Required(ErrorMessage = "Ký tự giới hạn buộc phải nhập.")]
        public int MessageLimitSendUTFUrl { get; set; }
        [Display(Name = "Branch name"), Required(ErrorMessage = "Branch name buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Branchname { get; set; }
        [Display(Name = "Tài khoản"), Required(ErrorMessage = "Tài khoản buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Mật khẩu buộc phải nhập.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        public string Password { get; set; }
        [Display(Name = "Loại gửi"), Required(ErrorMessage = "Loại gửi buộc phải nhập.")]
        public SentType Type { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt buộc phải nhập.")]
        public bool IsActive { get; set; }
    }
}