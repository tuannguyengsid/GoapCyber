using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class ImageFilterViewModel
    {
        public List<Image> List { get; set; }
        public string Keyword { get; set; }

        [Display(Name = "Thời gian tạo")]
        public string BeginAddDateString { get; set; }
        public string EndAddDateString { get; set; }
    }

    public class ImageCreateViewModel
    {
        [Display(Name = "Thư viện hình ảnh"), Required(ErrorMessage = "Thư viện hình ảnh buộc phải chọn.")]
        public string[] ImageLibraryIds { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public List<ImageLibrary> ImageLibraries { get; set; }
        public string ImagePaths { get; set; }
    }

    public class ImageEditViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Thư viện hình ảnh"), Required(ErrorMessage = "Thư viện hình ảnh buộc phải chọn.")]
        public string[] ImageLibraryIds { get; set; }
        [Display(Name = "Kích hoạt"), Required(ErrorMessage = "Kích hoạt thông tin buộc phải chọn")]
        public bool IsDeleted { get; set; }
        public string ImageSrc { get; set; }
        public List<ImageLibrary> ImageLibraries { get; set; }
    }
}