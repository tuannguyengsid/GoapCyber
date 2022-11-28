using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class District: GSIDMongoEntity
    {
        public District()
        {
            this.Wards = new List<Ward>();
        }
        
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string CountryId { get; set; }
        public string ProvinceId { get; set; }
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
        private Province _province;
        [BsonIgnore]
        public Province Province
        {
            get
            {
                if (_province == null)
                    _province = DbContext.Current.GetOne<Province>(u => u.Id.Equals(ProvinceId));

                return _province;
            }
            set
            {
                _province = value;
            }
        }

        //
        private List<Ward> _wards;
        [BsonIgnore]
        public List<Ward> Wards
        {
            get
            {
                if (_wards == null)
                    _wards = DbContext.Current.GetMany<Ward>(u => u.DistrictId == Id);
                return _wards;
            }
            set
            {
                _wards = value;
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
