using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IMenuNodeService
    {
        MenuNode GetBy(string id);
        MenuNode IsNameVnAvailable(string name);
        MenuNode IsNameVnAvailable(string name, string id);
        MenuNode IsNameEnAvailable(string name);
        MenuNode IsNameEnAvailable(string name, string id);
        List<MenuNode> GetAll();
        List<MenuNode> GetAllParent(string id);
        List<MenuNode> GetAllParent(string id, string subtractId);
        List<MenuNode> GetAllParent(string id, Nullable<bool> isDeleted);
        List<MenuNode> GetAll(bool isDeleted);
        string Create(MenuNode obj);
        void Update(MenuNode obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class MenuNodeService : IMenuNodeService
    {
        IGSIDMongoRepository repository;

        public MenuNodeService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public MenuNode GetBy(string id)
        {
            return repository.GetOne<MenuNode>(id);
        }

        public MenuNode IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<MenuNode>(w => w.NameVn.ToLower() == name);
        }

        public MenuNode IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<MenuNode>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public MenuNode IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<MenuNode>(w => w.NameEn.ToLower() == name);
        }

        public MenuNode IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<MenuNode>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<MenuNode> GetAll()
        {
            return repository.All<MenuNode>().ToList();
        }

        public List<MenuNode> GetAll(bool isDeleted)
        {
            return repository.GetMany<MenuNode>(c => c.IsDeleted == isDeleted)
                                            .OrderBy(c => c.Sort)
                                                .ToList();
        }

        public List<MenuNode> GetAllParent(string id)
        {
            return repository.GetMany<MenuNode>(c => c.ParentId == id)
                                            .OrderBy(c => c.Sort)
                                                .ToList();
        }

        public List<MenuNode> GetAllParent(string id, Nullable<bool> isDeleted)
        {
            return repository.GetMany<MenuNode>(c => c.ParentId == id && c.IsDeleted == isDeleted)
                                            .OrderBy(c => c.Sort)
                                                .ToList();
        }

        public List<MenuNode> GetAllParent(string id, string subtractId)
        {
            return repository.GetMany<MenuNode>(c => c.ParentId == id && c.Id != subtractId)
                                            .OrderBy(c => c.Sort)
                                                .ToList();
        }

        public string Create(MenuNode obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<MenuNode>(obj);
        }

        public void Update(MenuNode obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<MenuNode>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<MenuNode>(id);
                var xxx = GetAllParent(id);
                foreach (var x in xxx)
                    Delete(x.Id);
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
                repository.Delete<MenuNode>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

