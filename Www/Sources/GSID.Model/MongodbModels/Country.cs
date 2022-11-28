using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Country:GSIDMongoEntity
    {
        public Country()
        {
            this.Provinces = new List<Province>();
        }
        
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        #region not map
        //
        //
        private List<Province> _provinces;
        [BsonIgnore]
        public List<Province> Provinces
        {
            get
            {
                if (_provinces == null)
                    _provinces = DbContext.Current.GetMany<Province>(u => u.CountryId == Id);
                return _provinces;
            }
            set
            {
                _provinces = value;
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
        #endregion
    }
}
