using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.MongodbModels
{
    public partial class User
    {
        [BsonIgnore]
        public string FullName { get; set; }

        private List<Role> _roles;
        [BsonIgnore]
        public List<Role> Roles
        {
            get
            {
                if (_roles == null)
                {
                    var role2U = DbContext.Current.GetMany<RoleToUser>(w => w.UserId == Id).Select(s => s.RoleId).ToList();
                    _roles = role2U.Count() > 0 ? DbContext.Current.GetMany<Role>(w => role2U.Contains(w.Id)) : new List<Role>();
                }

                return _roles;
            }
            set
            {
                _roles = value;
            }
        }
    }
}
