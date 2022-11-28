using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Province:GSIDMongoEntity
    {
        public Province()
        {
            this.Districts = new List<District>();
            this.Users = new List<User>();
        }
        
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }

        #region not map

        private Country _country;
        [BsonIgnore]
        public Country Country
        {
            get
            {
                if (_country == null)
                    _country = DbContext.Current.GetOne<Country>(u => u.Id.Equals(CountryId));

                return _country;
            }
            set
            {
                _country = value;
            }
        }

        //
        private List<District> _districts;
        [BsonIgnore]
        public List<District> Districts
        {
            get
            {
                if (_districts == null)
                    _districts = DbContext.Current.GetMany<District>(u => u.ProvinceId == Id);
                return _districts;
            }
            set
            {
                _districts = value;
            }
        }

        //
        private List<User> _users;
        [BsonIgnore]
        public List<User> Users
        {
            get
            {
                if (_users == null)
                    _users = DbContext.Current.GetMany<User>(u => u.ProvinceStayingId == Id);
                return _users;
            }
            set
            {
                _users = value;
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
