using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class SaveFilePathConfig
    {
        public SaveFilePathConfig()
        {
            Code = "PARAMETER_PATH_SAVE_FILE_CONFIG";
        }
        public string Code { get; set; }
        public string ProductPath { get; set; }
        public string ProductBranchPath { get; set; }
        public string SitePath { get; set; }
        public string UserPath { get; set; }
        public string ConfigPath { get; set; }
        public string NewsPath { get; set; }
        public string ProductCategoryPath { get; set; }
        public string PartnerPath { get; set; }
        public string RecruitmentPath { get; set; }
        public string GalleryPath { get; set; }
    }
}
