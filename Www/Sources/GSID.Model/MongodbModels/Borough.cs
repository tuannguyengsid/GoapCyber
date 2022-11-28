using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Borough : GSIDMongoEntity
    {
        public string Name { get; set; }
        public string WardId { get; set; }
        public string KeywordSync { get; set; }
        public Nullable<bool> IsDefault { get; set; }

        #region not map
        private Ward _ward;
        [BsonIgnore]
        public Ward Ward
        {
            get
            {
                if (_ward == null)
                    _ward = DbContext.Current.GetOne<Ward>(u => u.Id.Equals(WardId));

                return _ward;
            }
            set
            {
                _ward = value;
            }
        }


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
        //
        #endregion
    }
}
