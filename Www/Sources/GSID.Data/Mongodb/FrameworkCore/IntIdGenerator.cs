using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GSID.Data.Mongodb.MongoEntities
{
//    public class UniqueIntGenerator<T> : IIdGenerator
//    {
//        public object GenerateId(object container, object document)
//        {
//            var col = (IMongoCollection<T>)container;
//            var sortBy = SortKey.Descending("_id");
//            var last = col.FindAll().SetSortOrder(sortBy).FirstOrDefault();
//            var id = (last == null) ? 1 : (int)last.ToBsonDocument()["_id"] + 1;
//            return id;
//        }

//        public bool IsEmpty(object id)
//        {
//            return ((id is int) && (id as int? == 0));
//        }
//    }
}
