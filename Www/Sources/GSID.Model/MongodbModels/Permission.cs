using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Permission: GSIDMongoEntity
    {
        public Permission()
        {
            this.RoleToPermisions = new List<RoleToPermision>();
        }

        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsMenu { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public Nullable<int> Sort { get; set; }
        #region not map
        //
        private List<RoleToPermision> _roleToPermisions;
        [BsonIgnore]
        public List<RoleToPermision> RoleToPermisions
        {
            get
            {
                if (_roleToPermisions == null)
                    _roleToPermisions = DbContext.Current.GetMany<RoleToPermision>(u => u.PermissionId == Id);
                return _roleToPermisions;
            }
            set
            {
                _roleToPermisions = value;
            }
        }
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
