using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class FilterMenuConfig
    {
        public FilterMenuConfig()
        {
            Code = "PARAMETER_FILTER_MENU_CONFIG";
        }

        public string Code { get; set; }
        public string TitleVn { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }

        public int CollectionLimitItem { get; set; }
        public string CollectionProductId { get; set; }

        public int ProductCategoryLimitItem { get; set; }
        public string ProductCategoryProductId { get; set; }

        public int ProductSizeLimitItem { get; set; }
        public string ProductSizeProductId { get; set; }

        public int ProductColorLimitItem { get; set; }
        public string ProductColorProductId { get; set; }

        public int ProductSurfaceLimitItem { get; set; }
        public string ProductSurfaceProductId { get; set; }

        public int ProductApplicationLimitItem { get; set; }
        public string ProductApplicationProductId { get; set; }
    }
}
