using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public class ProductOverviewBlock : GSIDMongoEntity
    {
        public enum IsLanguage : int
        {
            None = -1,
            Vietnamese = 1,
            English = 2
        }

        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IsLanguage Language { get; set; }
        public string ImageSrc { get; set; }
        public int Sort { get; set; }
    }
}
