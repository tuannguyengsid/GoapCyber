using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb.MongoCore;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.ConfigurationsCore
{    
    [BsonIgnoreExtraElements]
    public class FlagSetting : GSIDMongoEntity
    {
        [Required]
        [BsonElement("fV")]
        public string FlagValue { get; set; }

        [BsonElement("dc")]
        public string Description { get; set; }

        [BsonElement("iP")]
        public bool IsPermanent { get; set; }
    }
}
