using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;
using static GSID.Model.MongodbModels.Product;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class ProductFilterViewModel
    {
        public List<Product> List { get; set; }
        public string Keyword { get; set; }
        [Display(Name = "Nhóm sản phẩm")]
        public string[] ProductCategoryId { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class ProductCreateViewModel : SEOEntityViewModel
    {
        [Display(Name = "Tên sản phẩm"), Required(ErrorMessage = "Tên sản phẩm buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnAvailable", "Product", "", HttpMethod = "POST", ErrorMessage = "Tên sản phẩm này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên sản phẩm"), Required(ErrorMessage = "Tên sản phẩm(En) buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnAvailable", "Product", "", HttpMethod = "POST", ErrorMessage = "Tên sản phẩm này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugEn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        [AllowHtml]
        public string ShortDescriptionVn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        [AllowHtml]
        public string ShortDescriptionEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Loại nhập liệu tổng quan")]
        public IsOverviewViewType OverviewViewTypeVn { get; set; }
        [Display(Name = "Loại nhập liệu tổng quan")]
        public IsOverviewViewType OverviewViewTypeEn { get; set; }

        [Display(Name = "Tổng quan")]
        [AllowHtml]
        public string OverviewVn { get; set; }
        [Display(Name = "Tổng quan")]
        [AllowHtml]
        public string OverviewEn { get; set; }

        [Display(Name = "Nhóm sản phẩm"), Required(ErrorMessage = "Nhóm sản phẩm buộc phải chọn.")]
        public string ProductCategoryId { get; set; }
        public string ProductCategorySlugVn { get; set; }
        public string ProductCategorySlugEn { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public string ImageSrc { get; set; }
        public string ImageEnSrc { get; set; }

        public string ImageMaterialVnPaths { get; set; }
        public string ImageMaterialEnPaths { get; set; }
    }

    public class ProductEditViewModel : SEOEntityViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên sản phẩm"), Required(ErrorMessage = "Tên sản phẩm buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameVnIdAvailable", "Product", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên sản phẩm này đã tồn tại")]
        public string NameVn { get; set; }
        [Display(Name = "Tên sản phẩm"), Required(ErrorMessage = "Tên sản phẩm(En) buộc phải nhập.")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
        [MyRemoteAttribute("IsNameEnIdAvailable", "Product", "", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên sản phẩm này đã tồn tại")]
        public string NameEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugVn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string SlugEn { get; set; }
        [Display(Name = "Đường dẫn"), Required(ErrorMessage = "Đường dẫn buộc phải nhập.")]
        public string FullSlugEn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        [AllowHtml]
        public string ShortDescriptionVn { get; set; }
        [Display(Name = "Mô tả ngắn")]
        [AllowHtml]
        public string ShortDescriptionEn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionVn { get; set; }
        [Display(Name = "Mô tả")]
        [AllowHtml]
        public string DescriptionEn { get; set; }
        [Display(Name = "Loại hiển thị")]
        public IsOverviewViewType OverviewViewTypeVn { get; set; }
        [Display(Name = "Loại hiển thị")]
        public IsOverviewViewType OverviewViewTypeEn { get; set; }
        public List<ProductOverviewBlock> ProductOverviewBlockVn { get; set; }
        public List<ProductOverviewBlock> ProductOverviewBlockEn { get; set; }
        [Display(Name = "Tổng quan")]
        [AllowHtml]
        public string OverviewVn { get; set; }
        [Display(Name = "Tổng quan")]
        [AllowHtml]
        public string OverviewEn { get; set; }
        [Display(Name = "Nhóm sản phẩm"), Required(ErrorMessage = "Nhóm sản phẩm buộc phải chọn.")]
        public string ProductCategoryId { get; set; }
        public string ProductCategorySlugVn { get; set; }
        public string ProductCategorySlugEn { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductImage> Images { get; set; }
        public string ImageMaterialVnPaths { get; set; }
        public List<ProductImage> ImageEns { get; set; }
        public string ImageMaterialEnPaths { get; set; }

        public string ImageSrc { get; set; }
        public string ImageEnSrc { get; set; }
        
    }

    public class ImageNodeSort
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
    }
}