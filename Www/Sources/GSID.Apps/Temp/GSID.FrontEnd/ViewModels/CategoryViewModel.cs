using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
        }
        public List<Product> Products { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }

}