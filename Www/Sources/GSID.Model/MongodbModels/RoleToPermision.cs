using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class RoleToPermision: GSIDMongoEntity
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
        public Nullable<int> State { get; set; }
        #region not map

        private Permission _permission;
        [BsonIgnore]
        public Permission Permission
        {
            get
            {
                if (_permission == null)
                    _permission = DbContext.Current.GetOne<Permission>(u => u.Id.Equals(PermissionId));

                return _permission;
            }
            set
            {
                _permission = value;
            }
        }

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

        #endregion
    }
}
