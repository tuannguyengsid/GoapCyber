using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GSID.Model.MongodbModels.User;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IUserService
    {
        User GetBy(string id);
        User GetByEmail(string email);
        User VerifiedAccount(string email);
        List<User> GetAll();
        List<User> GetAllSmsByIds(List<string> ids);
        List<User> GetAllEmailByIds(List<string> ids);
        List<User> GetAll(bool isDeleted);
        List<User> GetAllByType(UserIsType? type);
        string Create(User obj);
        void Update(User obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class UserService : IUserService
    {
        private readonly IGSIDMongoRepository repository;

        public UserService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public User GetBy(string id)
        {
            return repository.GetOne<User>(id);
        }

        public User VerifiedAccount(string email)
        {
            var user = repository.GetOne<User>(c => c.Email == email);
            //if (user != null)
            //{
            //    user.Roles = repository.GetMany<RoleToUser>(c=> c.UserId == user.Id).Select(u => u.Role).ToList();
            //}
            return user;
        }
       
        public User GetByEmail(string email)
        {
            return repository.GetOne<User>(c => c.Email == email.Trim().ToLower());
        }

        public List<User> GetAll()
        {
            return repository.All<User>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<User> GetAllSmsByIds(List<string> ids)
        {
            return repository.GetMany<User>(c => ids.Contains(c.Id) 
                                                && c.IsDeleted == false
                                                && !string.IsNullOrEmpty(c.PhoneNumber)).ToList();
        }

        public List<User> GetAllEmailByIds(List<string> ids)
        {
            return repository.GetMany<User>(c => ids.Contains(c.Id)
                                                && c.IsDeleted == false
                                                && !string.IsNullOrEmpty(c.Email)).ToList();
        }

        public List<User> GetAll(bool isDeleted)
        {
            return repository.GetMany<User>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.MaxValue)
                                    .ToList();
        }

        public List<User> GetAllByType(UserIsType? type)
        {
            var list = repository.GetMany<User>(c => c.IsType == type).OrderByDescending(c => c.AddedByDate ?? DateTime.MaxValue).ToList();
            list.ForEach(mapping => {
                mapping.FullName = string.Format("{0} {1}", mapping.FirstName, mapping.LastName);
            });
            return list;
        }
        public string Create(User obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<User>(obj);
        }

        public void Update(User obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<User>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<User>(id);
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
                repository.Delete<User>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
