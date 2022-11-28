using GSID.Data.Mongodb.FrameworkCore;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public class Parameter : GSIDMongoEntity
    {
        public enum ParameterType
        {
            Const = 1,
            PageManagement = 2,
            Slider = 2,
        };
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Content { get; set; }
        public Nullable<ParameterType> Type { get; set; }

        //
        private User _addBy;
        [BsonIgnore]
        public User AddBy
        {
            get
            {
                if (_addBy == null && !string.IsNullOrEmpty(AddedBy))
                    _addBy = DbContext.Current.GetOne<User>(u => u.Id.Equals(AddedBy));
                return _addBy;
            }
            set
            {
                _addBy = value;
            }
        }

        //
        private User _editBy;
        [BsonIgnore]
        public User EditBy
        {
            get
            {
                if (_editBy == null && !string.IsNullOrEmpty(EditedBy))
                    _editBy = DbContext.Current.GetOne<User>(u => u.Id.Equals(EditedBy));
                return _editBy;
            }
            set
            {
                _editBy = value;
            }
        }
    }
}
