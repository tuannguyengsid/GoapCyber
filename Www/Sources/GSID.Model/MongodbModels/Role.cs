using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;
using System.Linq.Expressions;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Role: GSIDMongoEntity
    {
        public Role()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsSysAdmin { get; set; }
        #region not map

        private List<Permission> _permissions;
        [BsonIgnore]
        public List<Permission> Permissions
        {
            get
            {
                if (_permissions == null)
                {
                    var r2p = DbContext.Current.GetMany<RoleToPermision>(u => u.RoleId == Id);
                    _permissions = new List<Permission>();
                    r2p.ForEach(mapping => {
                        _permissions.Add(mapping.Permission);
                    });
                }
                return _permissions;
            }
            set
            {
                _permissions = value;
            }
        }


        //
        private List<RoleToPermision> _roleToPermisions;
        [BsonIgnore]
        public List<RoleToPermision> RoleToPermisions
        {
            get
            {
                if (_roleToPermisions == null)
                    _roleToPermisions = DbContext.Current.GetMany<RoleToPermision>(u => u.RoleId == Id);
                return _roleToPermisions;
            }
            set
            {
                _roleToPermisions = value;
            }
        }

        private List<RoleToUser> _roleToUsers;
        [BsonIgnore]
        public List<RoleToUser> RoleToUsers
        {
            get
            {
                if (_roleToUsers == null)
                    _roleToUsers = DbContext.Current.GetMany<RoleToUser>(u => u.RoleId == Id);
                return _roleToUsers;
            }
            set
            {
                _roleToUsers = value;
            }
        }

        #endregion
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
