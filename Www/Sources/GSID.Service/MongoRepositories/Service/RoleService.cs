using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IRoleService
    {
        Role GetBy(string id);
        List<Role> GetAll();
        List<Role> GetAllByUser(string id);
        string Create(Role obj);
        void Update(Role obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class RoleService : IRoleService
    {
        private readonly IGSIDMongoRepository repository;

        public RoleService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Role GetBy(string id)
        {
            return repository.GetOne<Role>(id);
        }

        public List<Role> GetAll()
        {
            return repository.All<Role>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }
        public List<Role> GetAllByUser(string id)
        {
            List<Role> roles = new List<Role>();
            var usertoRoles = repository.GetMany<RoleToUser>(c => c.UserId == id).ToList();
            usertoRoles.ForEach(mapping => {
                Role role = new Role();
                role = mapping.Role;
                role.Permissions = new List<Permission>();

                foreach (var item in mapping.Role.RoleToPermisions)
                    role.Permissions.Add(item.Permission);

                roles.Add(role);
            });

            return roles;
        }


        public string Create(Role obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Role>(obj);
        }

        public void Update(Role obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Role>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<RoleToPermision>(w => w.RoleId == id);
                repository.Delete<Role>(id);
                result = true;
            }
            catch
            {
            }
            return result;
        }
        public bool DeleteAll()
        {
            bool result = false;
            try
            {
                repository.Delete<Role>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
