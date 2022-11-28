using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Site : GSIDMongoEntity
    {
        public Site()
        {
            //this.Provinces = new List<Province>();
        }

        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public string AddressVn { get; set; }
        public string AddressEn { get; set; }
        public string BoroughId { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public string CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageSrc { get; set; }


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
        #region not map
        ////
        ////
        //private List<Province> _provinces;
        //[BsonIgnore]
        //public List<Province> Provinces
        //{
        //    get
        //    {
        //        if (_provinces == null)
        //            _provinces = DbContext.Current.GetMany<Province>(u => ProvinceIds.Contains(u.Id));
        //        return _provinces;
        //    }
        //    set
        //    {
        //        _provinces = value;
        //    }
        //}
        #endregion
    }
}
