using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface INewsCategoryService
    {
        NewsCategory GetBy(string id);
        NewsCategory GetBySlugVn(string slugVn);
        NewsCategory GetBySlugEn(string slugEn);
        List<NewsCategory> GetAll();
        List<NewsCategory> GetAll(bool isDeleted);
        List<NewsCategory> GetAllParent(string parentId);
        List<NewsCategory> GetAllParent(string parentId, bool isDeleted);
        List<NewsCategory> GetAllParent(string parentId, string subtractId);
        List<NewsCategory> GetAllParent(string parentId, string subtractId, bool isDeleted);
        List<NewsCategory> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(NewsCategory obj);
        void Update(NewsCategory obj);
        bool Delete(string id);
        bool Delete(string[] ids);
        bool DeleteAll();
    }

    public class NewsCategoryService : INewsCategoryService
    {
        IGSIDMongoRepository repository;

        public NewsCategoryService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public NewsCategory GetBy(string id)
        {
            return repository.GetOne<NewsCategory>(id);
        }

        public NewsCategory GetBySlugVn(string slugVn)
        {
            return repository.GetOne<NewsCategory>(c=> c.SlugVn == slugVn);
        }

        public NewsCategory GetBySlugEn(string slugEn)
        {
            return repository.GetOne<NewsCategory>(c => c.SlugEn == slugEn);
        }

        public List<NewsCategory> GetAll()
        {
            return repository.All<NewsCategory>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<NewsCategory> GetAll(bool isDeleted)
        {
            return repository.GetMany<NewsCategory>(c => c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<NewsCategory> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<NewsCategory>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(w => (!string.IsNullOrEmpty(w.NameVn) && w.NameVn.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.NameEn) && w.NameEn.ToLower().Contains(keyword))
                                    ).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public List<NewsCategory> GetAllParent(string parentId)
        {
            return repository.GetMany<NewsCategory>(c => c.ParentId == parentId)
                                .OrderBy(c => c.Sort).ToList();
        }

        public List<NewsCategory> GetAllParent(string parentId, bool isDeleted)
        {
            return repository.GetMany<NewsCategory>(c => c.ParentId == parentId && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort).ToList();
        }

        public List<NewsCategory> GetAllParent(string parentId, string subtractId)
        {
            return repository.GetMany<NewsCategory>(c => c.ParentId == parentId && c.Id != subtractId)
                                .OrderBy(c => c.Sort).ToList();
        }

        public List<NewsCategory> GetAllParent(string parentId, string subtractId, bool isDeleted)
        {
            return repository.GetMany<NewsCategory>(c => c.ParentId == parentId && c.Id != subtractId && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort).ToList();
        }

        public string Create(NewsCategory obj)
        {
            obj.AddedByDate = DateTime.Now;
            return repository.Insert<NewsCategory>(obj);
        }

        public void Update(NewsCategory obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<NewsCategory>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var _hasNew = repository.GetMany<News>(c => c.NewsCategoryId == id).Count;
                var _hasNewsCategory = repository.GetMany<NewsCategory>(c => c.ParentId == id).Count;
                if (_hasNew <=0 && _hasNewsCategory <= 0)
                {
                    var obj = repository.GetOne<NewsCategory>(id);
                    if (obj != null)
                    {
                        repository.Delete<NewsCategory>(id);

                        if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                            repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                        if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                            repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);

                        result = true;
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        public bool Delete(string[] ids)
        {
            bool result = false;
            try
            {
                foreach (var id in ids)
                {
                    var _hasNew = repository.GetMany<News>(c => c.NewsCategoryId == id).Count;
                    var _hasNewsCategory = repository.GetMany<NewsCategory>(c => c.ParentId == id).Count;
                    if (_hasNew > 0 || _hasNewsCategory > 0)
                        ids = Array.FindAll(ids, i => i != id).ToArray();
                }

                var xxx = repository.GetMany<NewsCategory>(c => ids.Contains(c.Id));
                foreach (var obj in xxx)
                {
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                }

                repository.Delete<NewsCategory>(c => ids.Contains(c.Id));
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
                repository.Delete<NewsCategory>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}