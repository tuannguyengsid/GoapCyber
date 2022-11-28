using GSID.Data.Mongodb.ChangeLogs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.FrameworkCore
{
    public class GSIDMongoEntity : IGSIDEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [LoggingPrimaryKey]
        public string Id { get; set; }
        
        [BsonElement("v")]
        [IgnoreLogging]
        public virtual long Version { get; set; }

        [BsonElement("aB")]
        [IgnoreLogging]
        public virtual string AddedBy { get; set; }
        [BsonElement("aD")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [IgnoreLogging]
        public virtual Nullable<System.DateTime> AddedByDate { get; set; }

        [BsonElement("eB")]
        [IgnoreLogging]
        public virtual string EditedBy { get; set; }

        [BsonElement("eD")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [IgnoreLogging]
        public virtual Nullable<System.DateTime> EditedByDate { get; set; }

        [BsonElement("dB")]
        [IgnoreLogging]
        public virtual string DeletedBy { get; set; }
        [BsonElement("dD")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [IgnoreLogging]
        public virtual Nullable<System.DateTime> DeletedByDate { get; set; }
        [IgnoreLogging]
        public virtual bool IsDeleted { get; set; }
    }
}
