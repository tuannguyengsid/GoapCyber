using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IProjectCategoryService
    {
        ProjectCategory GetBy(string id);
        ProjectCategory GetBySlugVn(string slug);
        ProjectCategory GetBySlugEn(string slug);
        ProjectCategory GetRouterVn(string routerid);
        ProjectCategory GetRouterEn(string routerid);
        ProjectCategory IsNameVnAvailable(string name);
        ProjectCategory IsNameVnAvailable(string name, string id);
        ProjectCategory IsNameEnAvailable(string name);
        ProjectCategory IsNameEnAvailable(string name, string id);
        ProjectCategory GetBy(string id, bool isDeleted);
        List<ProjectCategory> GetAll();
        List<ProjectCategory> GetAll(bool isDeleted);
        List<ProjectCategory> GetAllBySearch(string keyword, bool isDeleted);
        List<ProjectCategory> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(ProjectCategory obj);
        void Update(ProjectCategory obj);
        bool Delete(string id);
        bool Delete(string[] ids);
        bool DeleteAll();
    }

    public class ProjectCategoryService : IProjectCategoryService
    {
        IGSIDMongoRepository repository;

        public ProjectCategoryService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public ProjectCategory GetBy(string id)
        {
            return repository.GetOne<ProjectCategory>(id);
        }

        public ProjectCategory GetRouterVn(string routerid)
        {
            return repository.GetOne<ProjectCategory>(w => w.RouteDataUrlVnId == routerid);
        }

        public ProjectCategory GetRouterEn(string routerid)
        {
            return repository.GetOne<ProjectCategory>(w => w.RouteDataUrlEnId == routerid);
        }

        public ProjectCategory IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectCategory>(w => w.NameVn.ToLower() == name);
        }

        public ProjectCategory IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectCategory>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public ProjectCategory IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectCategory>(w => w.NameEn.ToLower() == name);
        }

        public ProjectCategory IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectCategory>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public ProjectCategory GetBy(string id, bool isDeleted)
        {
            return repository.GetOne<ProjectCategory>(c => c.Id == id && c.IsDeleted == isDeleted);
        }

        public ProjectCategory GetBySlugEn(string slug)
        {
            return repository.GetOne<ProjectCategory>(c => c.SlugEn == slug);
        }

        public ProjectCategory GetBySlugVn(string slug)
        {
            return repository.GetOne<ProjectCategory>(c => c.SlugVn == slug);
        }

        public List<ProjectCategory> GetAll()
        {
            return repository.All<ProjectCategory>().OrderBy(c => c.Sort).ToList();
        }

        public List<ProjectCategory> GetAllBySearch(string keyword, bool isDeleted)
        {
            return repository.GetMany<ProjectCategory>(c => ((!string.IsNullOrEmpty(keyword) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(keyword) && c.NameEn.ToLower().Contains(keyword.ToLower())))
                                                        && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort).ToList();
        }

        public List<ProjectCategory> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<ProjectCategory>();

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

            return _all.OrderBy(c => c.Sort).ToList();
        }

        public List<ProjectCategory> GetAll(bool isDeleted)
        {
            return repository.GetMany<ProjectCategory>(c => c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }


        public string Create(ProjectCategory obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<ProjectCategory>(obj);
        }

        public void Update(ProjectCategory obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<ProjectCategory>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<ProjectCategory>(id);
                if (obj != null)
                {
                    repository.Delete<ProjectCategory>(id);

                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                    result = true;
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
                    var _hasProj = repository.GetMany<Project>(c => c.ProjectCategoryIds.Contains(id)).Count;
                    if (_hasProj > 0)
                        ids = Array.FindAll(ids, i => i != id).ToArray();
                }

                var xxx = repository.GetMany<ProjectCategory>(c => ids.Contains(c.Id));
                foreach (var obj in xxx)
                {
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                }

                repository.Delete<ProjectCategory>(c => ids.Contains(c.Id));
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
                repository.Delete<ProjectCategory>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
