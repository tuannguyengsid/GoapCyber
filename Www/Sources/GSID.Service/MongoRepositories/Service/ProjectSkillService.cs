using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IProjectSkillService
    {
        ProjectSkill GetBy(string id);
        ProjectSkill IsNameVnAvailable(string name);
        ProjectSkill IsNameVnAvailable(string name, string id);
        ProjectSkill IsNameEnAvailable(string name);
        ProjectSkill IsNameEnAvailable(string name, string id);
        List<ProjectSkill> GetAll();
        List<ProjectSkill> GetAll(bool isDeleted);
        List<ProjectSkill> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(ProjectSkill obj);
        void Update(ProjectSkill obj);
        bool Delete(string id); 
        bool Delete(string[] ids);
        bool DeleteAll();
    }

    public class ProjectSkillService : IProjectSkillService
    {
        private readonly IGSIDMongoRepository repository;

        public ProjectSkillService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public ProjectSkill GetBy(string id)
        {
            return repository.GetOne<ProjectSkill>(id);
        }

        public ProjectSkill IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectSkill>(w => w.NameVn.ToLower() == name);
        }

        public ProjectSkill IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectSkill>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public ProjectSkill IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectSkill>(w => w.NameEn.ToLower() == name);
        }

        public ProjectSkill IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProjectSkill>(w => w.NameEn.ToLower() == name && w.Id != id);
        }
        public List<ProjectSkill> GetAll()
        {
            return repository.All<ProjectSkill>()
                                .OrderBy(c => c.NameVn).ToList();
        }

        public List<ProjectSkill> GetAll(bool isDeleted)
        {
            return repository.GetMany<ProjectSkill>(c => c.IsDeleted == isDeleted).OrderBy(c => c.NameVn).ToList();
        }

        public List<ProjectSkill> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<ProjectSkill>();

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

        public string Create(ProjectSkill obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<ProjectSkill>(obj);
        }

        public void Update(ProjectSkill obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<ProjectSkill>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<ProjectSkill>(id);
                result = true;
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
                repository.Delete<ProjectSkill>(c => ids.Contains(c.Id));
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
                repository.Delete<ProjectSkill>();
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
