using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public class TagPost : GSIDMongoEntity
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }
        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }

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
