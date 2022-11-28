using System;
using System.Collections.Generic;
using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IQuickLinkService
    {
        QuickLink GetBy(string id);
        QuickLink IsNameVnAvailable(string name);
        QuickLink IsNameVnAvailable(string name, string id);
        QuickLink IsNameEnAvailable(string name);
        QuickLink IsNameEnAvailable(string name, string id);
        List<QuickLink> GetAll();
        List<QuickLink> GetAll(bool isDeleted);
        List<QuickLink> GetAllByParent(string parentId);
        List<QuickLink> GetAllByParent(string parentId, bool isDeleted);
        string Create(QuickLink obj);
        void Update(QuickLink obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class QuickLinkService : IQuickLinkService
    {
        IGSIDMongoRepository repository;

        public QuickLinkService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public QuickLink GetBy(string id)
        {
            return repository.GetOne<QuickLink>(id);
        }

        public QuickLink IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<QuickLink>(w => w.NameVn.ToLower() == name);
        }

        public QuickLink IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<QuickLink>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public QuickLink IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<QuickLink>(w => w.NameEn.ToLower() == name);
        }

        public QuickLink IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<QuickLink>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<QuickLink> GetAll()
        {
            return repository.All<QuickLink>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<QuickLink> GetAll(bool isDeleted)
        {
            return repository.GetMany<QuickLink>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }
        
        public List<QuickLink> GetAllByParent(string parentId)
        {
            return repository.GetMany<QuickLink>(c => c.ParentId == parentId)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }

        public List<QuickLink> GetAllByParent(string parentId, bool isDeleted)
        {
            return repository.GetMany<QuickLink>(c => c.ParentId == parentId && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }

        public string Create(QuickLink obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<QuickLink>(obj);
        }

        public void Update(QuickLink obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<QuickLink>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<QuickLink>(id);
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
                repository.Delete<QuickLink>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

