using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class RecruitmentViewModel
    {
        public RecruitmentViewModel()
        {
            PageSize = 12;
            PageVisit = 8;
            PageIndex = 1;
        }
        public List<Recruitment> Recruitments { get; set; }
        public List<Career> Careers { get; set; }
        public Career Career { get; set; }

        public int PageSize { get; set; }
        public int PageVisit { get; set; }
        public int PageIndex { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int CountTotal { get; set; }
    }

    public class RecruitmentDetailViewModel
    {
        public Recruitment Recruitment { get; set; }
        public Career Career { get; set; }
        public Site Site { get; set; }
        public Position Position { get; set; }
        public Department Department { get; set; }
        public RecruitmentTag RecruitmentTag { get; set; }
        public List<Recruitment> Related { get; set; }
        public string returnUrl { get; set; }
    }

    public class RecruitmentSubmitViewModel
    {
        public string Language { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string CareerId { get; set; }
        public List<Career> Careers { get; set; }
        public HttpPostedFileBase FileUpload { get; set; }
        public string returnUrl { get; set; }

    }

    public class RecruitmentDetailSubmitViewModel
    {
        public string Language { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string CareerId { get; set; }
        public string RecruitmentId { get; set; }
        public List<Career> Careers { get; set; }
        public HttpPostedFileBase FileUpload { get; set; }
        public string returnUrl { get; set; }
    }
}