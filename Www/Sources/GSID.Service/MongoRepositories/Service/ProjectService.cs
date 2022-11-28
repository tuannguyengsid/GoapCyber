using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IProjectService
    {
        Project GetBy(string id);
        Project GetRouterVn(string routerid);
        Project GetRouterEn(string routerid);
        Project GetBySlugEn(string slug);
        Project GetBySlugVn(string slug);
        Project IsNameVnAvailable(string name);
        Project IsNameVnAvailable(string name, string id);
        Project IsNameEnAvailable(string name);
        Project IsNameEnAvailable(string name, string id);
        List<Project> GetAll();
        List<Project> GetAll(bool isDeleted);
        List<Project> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] ProjectCategoryId, string[] ProjectSkillId, string[] PartnerId, string[] ProductId);
        List<Project> GetAllBySearch(string keyword, bool isDeleted);
        string Create(Project obj);
        void Update(Project obj);
        bool Delete(string id);
        bool Delete(string[] ids);
        bool DeleteAll();
    }

    public class ProjectService : IProjectService
    {
        IGSIDMongoRepository repository;

        public ProjectService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Project GetBy(string id)
        {
            return repository.GetOne<Project>(id);
        }

        public Project GetBySlugEn(string slug)
        {
            return repository.GetOne<Project>(c => c.SlugEn == slug);
        }

        public Project GetBySlugVn(string slug)
        {
            return repository.GetOne<Project>(c => c.SlugVn == slug);
        }

        public Project GetRouterVn(string routerid)
        {
            return repository.GetOne<Project>(w => w.RouteDataUrlVnId == routerid);
        }

        public Project GetRouterEn(string routerid)
        {
            return repository.GetOne<Project>(w => w.RouteDataUrlEnId == routerid);
        }

        public Project IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Project>(w => w.NameVn.ToLower() == name);
        }

        public Project IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Project>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public Project IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Project>(w => w.NameEn.ToLower() == name);
        }

        public Project IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Project>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<Project> GetAll()
        {
            return repository.All<Project>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Project> GetAll(bool isDeleted)
        {
            return repository.GetMany<Project>(c => c.IsDeleted == isDeleted)
                                 .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Project> GetAllBySearch(string keyword, bool isDeleted)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            keyword = keyword.ToLower();
            return repository.GetMany<Project>(c => ((!string.IsNullOrEmpty(c.NameVn) && c.NameVn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.DescriptionVn) && c.DescriptionVn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.DescriptionEn) && c.DescriptionEn.ToLower().Contains(keyword)))
                                                        && c.IsDeleted == isDeleted)
                                 .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                            .ToList();
        }

        public List<Project> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] ProjectCategoryId, string[] ProjectSkillId, string[] PartnerId, string[] ProductId)
        {
            ProjectCategoryId = ProjectCategoryId != null ? Array.FindAll(ProjectCategoryId, s => !string.IsNullOrEmpty(s)) : ProjectCategoryId;
            ProjectSkillId = ProjectSkillId != null ? Array.FindAll(ProjectSkillId, s => !string.IsNullOrEmpty(s)) : ProjectSkillId;
            PartnerId = PartnerId != null ? Array.FindAll(PartnerId, s => !string.IsNullOrEmpty(s)) : PartnerId;
            ProductId = ProductId != null ? Array.FindAll(ProductId, s => !string.IsNullOrEmpty(s)) : ProductId;
            var _all = repository.All<Project>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(c => (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.NameVn) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.NameEn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.DescriptionVn) && c.DescriptionVn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.DescriptionEn) && c.DescriptionEn.ToLower().Contains(keyword.ToLower()))
                                    ).ToList();
            }

            if (ProjectCategoryId != null && ProjectCategoryId.Count() > 0)
                _all = _all.Where(c => c.ProjectCategoryIds != null && c.ProjectCategoryIds.Any(a => ProjectCategoryId.Contains(a))).ToList();
            if (ProjectSkillId != null && ProjectSkillId.Count() > 0)
                _all = _all.Where(c => c.ProjectSkillIds != null && c.ProjectSkillIds.Any(a => ProjectSkillId.Contains(a))).ToList();
            if (PartnerId != null && PartnerId.Count() > 0)
                _all = _all.Where(c => c.PartnerIds != null && c.PartnerIds.Any(a => PartnerId.Contains(a))).ToList();
            if (ProductId != null && ProductId.Count() > 0)
                _all = _all.Where(c => c.ProductIds != null && c.ProductIds.Any(a => ProductId.Contains(a))).ToList();
            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public List<Project> GetAllLatest(int take, bool isDeleted)
        {
            return repository.GetMany<Project>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .Take(take)
                                        .ToList();
        }

        public string Create(Project obj)
        {
            obj.AddedByDate = DateTime.Now;
            return repository.Insert<Project>(obj);
        }

        public void Update(Project obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Project>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<Project>(id);
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                    repository.Delete<Project>(id);
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
                repository.Delete<Project>(c => ids.Contains(c.Id));
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
                repository.Delete<Project>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}