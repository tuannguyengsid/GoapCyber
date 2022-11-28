using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class User:GSIDMongoEntity
    {
        public enum UserIsType : int
        {
            None = -1,
            UserinSystem = 1,
            UserinContacts = 2
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> DatedByIdentityCard { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string AddressNumberResident { get; set; }
        public string AddressLine1Resident { get; set; }
        public string AddressLine2Resident { get; set; }
        public string BoroughResidentId { get; set; }
        public string WardResidentId { get; set; }
        public string DistrictResidentId { get; set; }
        public string ProvinceResidentId { get; set; }
        public string CountryResidentId { get; set; }
        public string AddressNumberStaying { get; set; }
        public string AddressLine1Staying { get; set; }
        public string AddressLine2Staying { get; set; }
        public string BoroughStayingId { get; set; }
        public string WardStayingId { get; set; }
        public string DistrictStayingId { get; set; }
        public string ProvinceStayingId { get; set; }
        public string CountryStayingId { get; set; }
        public string SiteId { get; set; }
        public string DepartmentId { get; set; }
        public string PositionId { get; set; }
        public Nullable<DateTime> TimeStartWorking { get; set; }
        public Nullable<bool> IsAdministrator { get; set; }
        public Nullable<UserIsType> IsType { get; set; }
        public Nullable<System.DateTime> LastLogon { get; set; }
        public string Noted { get; set; }
        public string ImageSrc { get; set; }

        #region not map

        //private Ward _ward;
        //[BsonIgnore]
        //public Ward Ward
        //{
        //    get
        //    {
        //        if (_ward == null)
        //            _ward = DbContext.Current.GetOne<Ward>(u => u.Id.Equals(WardId));

        //        return _ward;
        //    }
        //    set
        //    {
        //        _ward = value;
        //    }
        //}


        //private Borough _borough;
        //[BsonIgnore]
        //public Borough Borough
        //{
        //    get
        //    {
        //        if (_borough == null)
        //            _borough = DbContext.Current.GetOne<Borough>(u => u.Id.Equals(BoroughId));

        //        return _borough;
        //    }
        //    set
        //    {
        //        _borough = value;
        //    }
        //}

        //private District _district;
        //[BsonIgnore]
        //public District District
        //{
        //    get
        //    {
        //        if (_district == null)
        //            _district = DbContext.Current.GetOne<District>(u => u.Id.Equals(DistrictId));

        //        return _district;
        //    }
        //    set
        //    {
        //        _district = value;
        //    }
        //}

        //private Province _province;
        //[BsonIgnore]
        //public Province Province
        //{
        //    get
        //    {
        //        if (_province == null)
        //            _province = DbContext.Current.GetOne<Province>(u => u.Id.Equals(ProvinceId));

        //        return _province;
        //    }
        //    set
        //    {
        //        _province = value;
        //    }
        //}

        //private Country _country;
        //[BsonIgnore]
        //public Country Country
        //{
        //    get
        //    {
        //        if (_country == null)
        //            _country = DbContext.Current.GetOne<Country>(u => u.Id.Equals(CountryId));

        //        return _country;
        //    }
        //    set
        //    {
        //        _country = value;
        //    }
        //}


        private List<RoleToUser> _roleToUsers;
        [BsonIgnore]
        public List<RoleToUser> RoleToUsers
        {
            get
            {
                if (_roleToUsers == null)
                    _roleToUsers = DbContext.Current.GetMany<RoleToUser>(u => u.UserId == Id);
                return _roleToUsers;
            }
            set
            {
                _roleToUsers = value;
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
