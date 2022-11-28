using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.Models
{
    public partial class Topic
    {
        [NotMapped]
        public List<User> Users { get; set; }
        [NotMapped]
        public List<CategoryTopicToTopic> CategoryTopics { get; set; }
    }
}
