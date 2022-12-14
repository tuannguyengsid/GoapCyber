using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Contact : GSIDMongoEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string Address { get; set; }
        public bool IsSubscribe { get; set; }
        public bool IsContact { get; set; }
        public string Noted { get; set; }

        [BsonIgnore]
        public string FullAddress { get; set; }
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
