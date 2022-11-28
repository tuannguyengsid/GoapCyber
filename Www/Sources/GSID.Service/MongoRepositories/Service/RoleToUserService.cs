using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GSID.Model.MongodbModels.User;

namespace GSID.Service
{
    public interface IUserToRoleService
    {
        RoleToUser GetBy(string id);
        RoleToUser GetByUserAuthorize(string userId);
        List<RoleToUser> GetAll();
        List<RoleToUser> GetAllByUser(string userId);
        List<RoleToUser> GetAllByRole(string roleId);
        void Create(RoleToUser obj);
        void Update(RoleToUser obj);
        bool? Delete(string id);
        bool? DeleteByUser(string userid);
        bool? DeleteAll();
    }

    public class UserToRoleService : IUserToRoleService
    {
        private readonly IGSIDMongoRepository repository;

        public UserToRoleService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public RoleToUser GetBy(string id)
        {
            return repository.GetOne<RoleToUser>(id);
        }

        public RoleToUser GetByUserAuthorize(string userId)
        {
            return repository.GetOne<RoleToUser>(c => c.UserId == userId);
        }

        public List<RoleToUser> GetAllByRole(string roleId)
        {
            return repository.GetMany<RoleToUser>(c => c.RoleId == roleId);
        }

        public List<RoleToUser> GetAllByUser(string userId)
        {
            return repository.GetMany<RoleToUser>(c => c.UserId == userId);
        }

        public List<RoleToUser> GetAll()
        {
            return repository.All<RoleToUser>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public void Create(RoleToUser obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            repository.Insert<RoleToUser>(obj);
        }

        public void Update(RoleToUser obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<RoleToUser>(obj);
        }

        public bool? Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<RoleToUser>(id);
                result = true;
            }
            catch
            {
            }
            return result;
        }
        public bool? DeleteByUser(string userid)
        {
            bool result = false;
            try
            {
                repository.Delete<RoleToUser>(c => c.UserId == userid);
                result = true;
            }
            catch
            {
            }
            return result;
        }

        public bool? DeleteAll()
        {
            bool result = false;
            try
            {
                repository.Delete<RoleToUser>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
