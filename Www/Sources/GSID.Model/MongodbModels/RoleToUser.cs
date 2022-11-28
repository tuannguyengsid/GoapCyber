using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class RoleToUser: GSIDMongoEntity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public Nullable<bool> IsAuthorize { get; set; }
        public Nullable<System.DateTime> AuthorizeTime { get; set; }
        public Nullable<System.Guid> AuthorizeFromUser { get; set; }
        public string Noted { get; set; }

        #region not map

        private Role _role;
        [BsonIgnore]
        public Role Role
        {
            get
            {
                if (_role == null)
                    _role = DbContext.Current.GetOne<Role>(u => u.Id.Equals(RoleId));

                return _role;
            }
            set
            {
                _role = value;
            }
        }
        private User _user;
        [BsonIgnore]
        public User User
        {
            get
            {
                if (_user == null)
                    _user = DbContext.Current.GetOne<User>(u => u.Id.Equals(UserId));

                return _user;
            }
            set
            {
                _user = value;
            }
        }
        #endregion
    }
}
