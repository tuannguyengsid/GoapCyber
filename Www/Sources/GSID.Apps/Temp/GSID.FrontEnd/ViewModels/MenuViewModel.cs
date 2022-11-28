using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{


    public class FilterMenuConfigViewModel
    {
        public List<MenuNode> MenuNodes { get; set; }
        public string TitleVn { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }

    }
}