using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.Admin.ViewModels.MongoModels
{
    public class DesktopViewModel
    {
        public DesktopViewModel()
        {
            ItemTake = 20;
        }

        public int ItemTake { get; set; }

        public List<CurriculumVitae> CurriculumVitaes { get; set; }
        public List<ContactMessage> ContactMessages { get; set; }

        public int TotalContact { get; set; }
        public int TotalICurriculumVitae { get; set; }
        public int TotalRecruitment { get; set; }
        public int TotalPost { get; set; }
    }
}