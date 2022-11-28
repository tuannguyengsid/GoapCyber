using GSID.Model.ExtraEntities;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSID.Admin.Attributes;
using System.Web.Mvc;

namespace GSID.Admin.Areas.PageManagement.ViewModels
{
    public class RecruitmentDetailPageViewModel
    {
        [Display(Name = "Hình nền sitemap")]
        public string BreakScrumBackgroundSrc { get; set; }
        [Display(Name = "Số tin mới hiển thị")]
        public int RelastItem { get; set; }
        public string MenuActiveId { get; set; }
        public List<MenuNode> MenuNodes { get; set; }
    }
}