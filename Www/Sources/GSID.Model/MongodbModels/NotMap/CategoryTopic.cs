using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.Models
{
    public partial class CategoryTopic
    {
        [NotMapped]
        public List<string> ListTagString { get; set; }
        [NotMapped]
        public List<string> ListEmailString { get; set; }
    }
}
