using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IRecruitmentTagService
    {
        RecruitmentTag GetBy(string id);
        RecruitmentTag IsNameVnAvailable(string name);
        RecruitmentTag IsNameVnAvailable(string name, string id);
        RecruitmentTag IsNameEnAvailable(string name);
        RecruitmentTag IsNameEnAvailable(string name, string id);
        List<RecruitmentTag> GetAll();
        List<RecruitmentTag> GetAll(bool isDeleted);
        List<RecruitmentTag> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(RecruitmentTag obj);
        void Update(RecruitmentTag obj);
        bool? Delete(string id);
        bool? DeleteAll();
    }

    public class RecruitmentTagService : IRecruitmentTagService
    {
        private readonly IGSIDMongoRepository repository;

        public RecruitmentTagService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public RecruitmentTag GetBy(string id)
        {
            return repository.GetOne<RecruitmentTag>(id);
        }

        public RecruitmentTag IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<RecruitmentTag>(w => w.NameVn.ToLower() == name);
        }

        public RecruitmentTag IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<RecruitmentTag>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public RecruitmentTag IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<RecruitmentTag>(w => w.NameEn.ToLower() == name);
        }

        public RecruitmentTag IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<RecruitmentTag>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<RecruitmentTag> GetAll()
        {
            return repository.All<RecruitmentTag>()
                                .OrderBy(c => c.NameEn).ToList();
        }

        public List<RecruitmentTag> GetAll(bool isDeleted)
        {
            return repository.GetMany<RecruitmentTag>(c => c.IsDeleted == isDeleted).OrderBy(c => c.NameEn).ToList();
        }

        public List<RecruitmentTag> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<RecruitmentTag>();

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

        public string Create(RecruitmentTag obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<RecruitmentTag>(obj);
        }

        public void Update(RecruitmentTag obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<RecruitmentTag>(obj);
        }

        public bool? Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<RecruitmentTag>(id);
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
                repository.Delete<RecruitmentTag>();
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
