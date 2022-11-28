using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IRoleToPermisionService
    {
        RoleToPermision GetBy(string id);
        List<RoleToPermision> GetAll();
        List<RoleToPermision> GetAllBy(string roleId);
        List<RoleToPermision> GetAllBy(string roleId, int state);
        void Create(RoleToPermision obj);
        void Create(string _rolId, string[] _perIds, int? state);
        void Update(RoleToPermision obj);
        bool? Delete(string id);
        bool? DeleteByRole(string _rolid);
    }

    public class RoleToPermisionService : IRoleToPermisionService
    {
        private readonly IGSIDMongoRepository repository;

        public RoleToPermisionService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public RoleToPermision GetBy(string id)
        {
            return repository.GetOne<RoleToPermision>(id);
        }

        public List<RoleToPermision> GetAll()
        {
            return repository.All<RoleToPermision>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }
        public List<RoleToPermision> GetAllBy(string roleId)
        {
            return repository.GetMany<RoleToPermision>(c => c.RoleId == roleId)
                                .OrderByDescending(c => c.Id)
                                    .ToList();
        }

        public List<RoleToPermision> GetAllBy(string roleId, int state)
        {
            return repository.GetMany<RoleToPermision>(c => c.RoleId == roleId && c.State == state)
                                .OrderByDescending(c => c.Id)
                                    .ToList();
        }

        public void Create(RoleToPermision obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            repository.Insert<RoleToPermision>(obj);
        }

        public void Create(string _rolId, string[] _perIds, int? state)
        {
            foreach (var item in _perIds)
            {
                RoleToPermision obj = new RoleToPermision();
                obj.RoleId = _rolId;
                obj.PermissionId = item;
                obj.State = state;
                repository.Insert<RoleToPermision>(obj);
            }
        }

        public void Update(RoleToPermision obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<RoleToPermision>(obj);
        }

        public bool? Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<RoleToPermision>(id);
                result = true;
            }
            catch
            {
            }
            return result;
        }

        public bool? DeleteByRole(string _rolid)
        {
            bool result = false;
            try
            {
                repository.Delete<RoleToPermision>(c => c.RoleId == _rolid);
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
