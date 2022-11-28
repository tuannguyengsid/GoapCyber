using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class CustomerViewModel
    {
        [Required(ErrorMessageResourceName = "NameRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "EmailRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "EmailRegularExpressionCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "PhoneNumberRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessageResourceName = "ProvinceIdRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string ProvinceId { get; set; }
        [Required(ErrorMessageResourceName = "DistrictIdRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string DistrictId { get; set; }
        [Required(ErrorMessageResourceName = "AddressRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string Address { get; set; }
        [Required(ErrorMessageResourceName = "QualityRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public int Quality { get; set; }
        public string Message { get; set; }
        public string ProductId { get; set; }

        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
    }

    public class ContactUsViewModel
    {
        [Required(ErrorMessageResourceName = "NameRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "EmailRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "EmailRegularExpressionCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "PhoneNumberRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessageResourceName = "ProvinceIdRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string ProvinceId { get; set; }
        [Required(ErrorMessageResourceName = "DistrictIdRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string DistrictId { get; set; }
        [Required(ErrorMessageResourceName = "AddressRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string Address { get; set; }
        public string Noted { get; set; }

        public List<Province> Provinces { get; set; }
        public List<District> Districts { get; set; }
    }

    public class CustomerDistrictFilterViewModel
    {
        [Required(ErrorMessageResourceName = "DistrictIdRequiredCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string DistrictId { get; set; }
        public List<District> Districts { get; set; }
    }

    public class NewsletterSubscriptionViewModel
    {
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName = "EmailRegularExpressionCustomerViewModel", ErrorMessageResourceType = typeof(GSID.Resources.Resources))]
        public string Email { get; set; }
    }
}