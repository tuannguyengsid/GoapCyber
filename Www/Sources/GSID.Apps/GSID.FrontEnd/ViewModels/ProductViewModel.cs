using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class ProductViewModel
    {
        public ProductCategory ProductCategory { get; set; }
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }

    }

}