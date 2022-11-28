using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface ITagPostService
    {
        TagPost GetBy(string id);
        TagPost GetRouterVn(string routerid);
        TagPost GetRouterEn(string routerid);
        TagPost IsNameVnAvailable(string name);
        TagPost IsNameVnAvailable(string name, string id);
        TagPost IsNameEnAvailable(string name);
        TagPost IsNameEnAvailable(string name, string id);
        List<TagPost> GetAll();
        List<TagPost> GetAllByIds(string[] ids, bool isDeleted);
        List<TagPost> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        List<TagPost> GetAll(bool isDeleted);
        string Create(TagPost obj);
        void Update(TagPost obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class TagPostService : ITagPostService
    {
        private readonly IGSIDMongoRepository repository;

        public TagPostService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public TagPost GetBy(string id)
        {
            return repository.GetOne<TagPost>(id);
        }
        public TagPost GetRouterVn(string routerid)
        {
            return repository.GetOne<TagPost>(w => w.RouteDataUrlVnId == routerid);
        }

        public TagPost GetRouterEn(string routerid)
        {
            return repository.GetOne<TagPost>(w => w.RouteDataUrlEnId == routerid);
        }

        public TagPost IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<TagPost>(w => w.NameVn.ToLower() == name);
        }

        public TagPost IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<TagPost>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public TagPost IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<TagPost>(w => w.NameEn.ToLower() == name);
        }

        public TagPost IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<TagPost>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<TagPost> GetAll()
        {
            return repository.All<TagPost>()
                                .OrderBy(c => c.NameEn).ToList();
        }

        public List<TagPost> GetAllByIds(string[] ids, bool isDeleted)
        {
            return repository.GetMany<TagPost>(c => ids.Contains(c.Id) && c.IsDeleted == isDeleted).OrderBy(c => c.NameEn).ToList();
        }

        public List<TagPost> GetAll(bool isDeleted)
        {
            return repository.GetMany<TagPost>(c => c.IsDeleted == isDeleted).OrderBy(c => c.NameEn).ToList();
        }

        public List<TagPost> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<TagPost>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(c => (!string.IsNullOrEmpty(keyword) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(keyword) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                    ).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public string Create(TagPost obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<TagPost>(obj);
        }

        public void Update(TagPost obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<TagPost>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<TagPost>(id);
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                    repository.Delete<TagPost>(id);
                    result = true;
                }
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
                repository.Delete<TagPost>();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
            return result;
        }
    }
}
