using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Recruitment : GSIDMongoEntity
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public string SiteId { get; set; }
        public string PositionId { get; set; }
        public string DepartmentId { get; set; }
        public string CareerId { get; set; }
        public string ExperienceVn { get; set; }
        public string ExperienceEn { get; set; }
        public string SalaryVn { get; set; }
        public string SalaryEn { get; set; }
        public Nullable<DateTime> ExpirationDate { get; set; }
        public string RecruitmentTagId { get; set; }
        public string ImageSrc { get; set; }
        #region not map

        //
        private RouteDataUrl _routeDataUrlVn;
        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn
        {
            get
            {
                if (_routeDataUrlVn == null && !string.IsNullOrEmpty(RouteDataUrlVnId))
                    _routeDataUrlVn = DbContext.Current.GetOne<RouteDataUrl>(u => u.Id.Equals(RouteDataUrlVnId));
                return _routeDataUrlVn;
            }
            set
            {
                _routeDataUrlVn = value;
            }
        }

        //
        private RouteDataUrl _routeDataUrlEn;
        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn
        {
            get
            {
                if (_routeDataUrlEn == null && !string.IsNullOrEmpty(RouteDataUrlEnId))
                    _routeDataUrlEn = DbContext.Current.GetOne<RouteDataUrl>(u => u.Id.Equals(RouteDataUrlEnId));
                return _routeDataUrlEn;
            }
            set
            {
                _routeDataUrlEn = value;
            }
        }

        private Site _site;
        [BsonIgnore]
        public Site Site
        {
            get
            {
                if (_site == null)
                    _site = DbContext.Current.GetOne<Site>(u => u.Id.Equals(SiteId));

                return _site;
            }
            set
            {
                _site = value;
            }
        }


        private Position _position;
        [BsonIgnore]
        public Position Position
        {
            get
            {
                if (_position == null)
                    _position = DbContext.Current.GetOne<Position>(u => u.Id.Equals(PositionId));

                return _position;
            }
            set
            {
                _position = value;
            }
        }


        private Department _department;
        [BsonIgnore]
        public Department Department
        {
            get
            {
                if (_department == null)
                    _department = DbContext.Current.GetOne<Department>(u => u.Id.Equals(DepartmentId));

                return _department;
            }
            set
            {
                _department = value;
            }
        }



        private Career _career;
        [BsonIgnore]
        public Career Career
        {
            get
            {
                if (_career == null)
                    _career = DbContext.Current.GetOne<Career>(u => u.Id.Equals(CareerId));

                return _career;
            }
            set
            {
                _career = value;
            }
        }


        private RecruitmentTag _recruitmentTag;
        [BsonIgnore]
        public RecruitmentTag RecruitmentTag
        {
            get
            {
                if (_recruitmentTag == null)
                    _recruitmentTag = DbContext.Current.GetOne<RecruitmentTag>(u => u.Id.Equals(RecruitmentTagId));

                return _recruitmentTag;
            }
            set
            {
                _recruitmentTag = value;
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
