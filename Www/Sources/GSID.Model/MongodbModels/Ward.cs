using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Ward: GSIDMongoEntity
    {
        public Ward()
        {
            this.Boroughs = new List<Borough>();
        }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string CountryId { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        #region not map

        private District _district;
        [BsonIgnore]
        public District District
        {
            get
            {
                if (_district == null)
                    _district = DbContext.Current.GetOne<District>(u => u.Id.Equals(DistrictId));

                return _district;
            }
            set
            {
                _district = value;
            }
        }
        

        //
        private List<Borough> _boroughs;
        [BsonIgnore]
        public List<Borough> Boroughs
        {
            get
            {
                if (_boroughs == null)
                    _boroughs = DbContext.Current.GetMany<Borough>(u => u.WardId == Id);
                return _boroughs;
            }
            set
            {
                _boroughs = value;
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
