using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IPermissionService
    {
        Permission GetBy(string id);
        Permission GetByParent(string _parentId);
        Permission GetByDescription(string _description);
        List<Permission> GetAllByDescription(string _description);
        List<Permission> GetAll();
        List<Permission> GetAllByRoleIds(List<Role> roles);
        List<Permission> GetAllByRoleIds(List<Role> roles, bool isMenu);
        List<Permission> GetAllByChild(string _parentId);
        List<Permission> GetAllByChild(string _parentId, bool isdeleted);
        List<Permission> GetAllByName(string name);
        List<Permission> GetAllByMenu(bool? isMenu);
        List<Permission> GetAllByMenu(bool? isMenu, bool isDeleted);
        string Create(Permission obj);
        void Update(Permission obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class PermissionService : IPermissionService
    {
        private readonly IGSIDMongoRepository repository;

        public PermissionService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Permission GetBy(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            return repository.GetOne<Permission>(id);
        }

        public Permission GetByParent(string _parentId)
        {
            if (string.IsNullOrEmpty(_parentId))
                return null;
            return repository.GetOne<Permission>(c=> c.Id == _parentId);
        }

        public Permission GetByDescription(string _description)
        {
            if (string.IsNullOrEmpty(_description))
                return null;
            _description = _description.Trim().ToLower();

            return repository.GetOne<Permission>(c => c.Description == _description);
        }

        public List<Permission> GetAll()
        {
            return repository.All<Permission>()
                                .OrderBy(c => c.Sort)
                                    .ToList();
        }

        public List<Permission> GetAllByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return repository.GetMany<Permission>(c => c.Name.Contains(name))
                                .OrderBy(c => c.Sort)
                                    .ToList();
        }

        public List<Permission> GetAllByDescription(string _description)
        {
            if (string.IsNullOrEmpty(_description))
                return null;
            _description = _description.Trim().ToLower();

            return repository.GetMany<Permission>(c => c.Description == _description)
                                .OrderBy(c => c.Sort)
                                    .ToList();
        }

        public List<Permission> GetAllByChild(string _parentId)
        {
            return repository.GetMany<Permission>(p => p.ParentId == _parentId)
                                .OrderBy(c => c.Sort)
                                    .ToList();
        }
        public List<Permission> GetAllByChild(string _parentId, bool isdeleted)
        {
            return repository.GetMany<Permission>(p => p.ParentId == _parentId && p.IsDeleted == isdeleted)
                                .OrderBy(c => c.Sort)
                                    .ToList();
        }

        public List<Permission> GetAllByMenu(bool? isMenu)
        {
            if (!isMenu.HasValue)
                return null;
            return repository.GetMany<Permission>(c => c.IsMenu == isMenu)
                                .OrderBy(c => c.Sort)
                                    .ToList();
        }

        public List<Permission> GetAllByRoleIds(List<Role> roles)
        {
            if (roles.Count >= 0)
            {
                var _roleIds = roles.Select(s => s.Id).ToList();
                var perIds = _roleIds.Count() >= 0 ? repository.GetMany<RoleToPermision>(w => _roleIds.Contains(w.RoleId)).Select(s => s.PermissionId).ToList()
                                                            : new List<string>();
                return perIds.Count() > 0 ? repository.GetMany<Permission>(w => perIds.Contains(w.Id))
                                                    : new List<Permission>();
            }
            else
                return repository.All<Permission>().OrderBy(c => c.Sort).ToList();
        }

        public List<Permission> GetAllByRoleIds(List<Role> roles, bool isMenu)
        {
            if (roles.Count >= 0)
            {
                var _roleIds = roles.Select(s => s.Id).ToList();
                var perIds = _roleIds.Count() >= 0 ? repository.GetMany<RoleToPermision>(w => _roleIds.Contains(w.RoleId)).Select(s => s.PermissionId).ToList()
                                                            : new List<string>();
                return perIds.Count() > 0 ? repository.GetMany<Permission>(w => perIds.Contains(w.Id) && w.IsMenu == isMenu)
                                                    : new List<Permission>();
            }
            else
                return repository.All<Permission>().OrderBy(c => c.Sort).ToList();
        }


        public List<Permission> GetAllByMenu(bool? isMenu, bool isDeleted)
        {
            return repository.GetMany<Permission>(c => c.IsMenu == isMenu && c.IsDeleted == isDeleted).OrderBy(c => c.Sort ?? int.MaxValue)
                                .OrderBy(c => c.Sort)
                                    .ToList();
        }

        public string Create(Permission obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Permission>(obj);
        }

        public void Update(Permission obj)
        {
            repository.Update<Permission>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Permission>(id);
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
                repository.Delete<Permission>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
