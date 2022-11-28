using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class ImageLibrary : GSIDMongoEntity
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }
        public int Sort { get; set; }
        #region not map

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
        #endregion
    }
}
