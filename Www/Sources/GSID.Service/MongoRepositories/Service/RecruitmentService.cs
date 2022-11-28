using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IRecruitmentService
    {
        Recruitment GetBy(string id);
        Recruitment GetRouterVn(string routerid);
        Recruitment GetRouterEn(string routerid);
        Recruitment IsNameVnAvailable(string name);
        Recruitment IsNameVnAvailable(string name, string id);
        Recruitment IsNameEnAvailable(string name);
        Recruitment IsNameEnAvailable(string name, string id);
        List<Recruitment> GetAll();
        List<Recruitment> GetAllByCareer(string careerId, bool isDeleted);
        List<Recruitment> GetAllByCareer(string careerId);
        List<Recruitment> GetAllByDepartment(string departmentId);
        List<Recruitment> GetAll(bool isDeleted);
        List<Recruitment> GetAllByRelated(Recruitment recruitment, int take, bool isDeleted);
        List<Recruitment> GetAllBySearch(string keyword, bool isDeleted);
        List<Recruitment> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, DateTime? BeginExpirationDate, DateTime? EndExpirationDate, string[] SiteId, string[] PositionId, string[] DepartmentId, string[] CareerId, string[] RecruitmentTagId);
        List<Recruitment> GetAllByExpirationDate(bool isDeleted);
        List<Recruitment> GetAllByExpirationDate(string careerId, bool isDeleted);
        List<Recruitment> GetAllByRecruitmentTag(string recruitmentTagId);
        List<Recruitment> GetAllByRecruitmentSite(string siteId);
        string Create(Recruitment obj);
        void Update(Recruitment obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class RecruitmentService : IRecruitmentService
    {
        private readonly IGSIDMongoRepository repository;

        public RecruitmentService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Recruitment GetBy(string id)
        {
            return repository.GetOne<Recruitment>(id);
        }

        public Recruitment GetRouterVn(string routerid)
        {
            return repository.GetOne<Recruitment>(w => w.RouteDataUrlVnId == routerid);
        }

        public Recruitment GetRouterEn(string routerid)
        {
            return repository.GetOne<Recruitment>(w => w.RouteDataUrlEnId == routerid);
        }

        public Recruitment IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Recruitment>(w => w.NameVn.ToLower() == name);
        }

        public Recruitment IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Recruitment>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public Recruitment IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Recruitment>(w => w.NameEn.ToLower() == name);
        }

        public Recruitment IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Recruitment>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<Recruitment> GetAll()
        {
            return repository.All<Recruitment>().OrderByDescending(c => c.AddedByDate ?? DateTime.Now).ToList();
        }

        public List<Recruitment> GetAllByCareer(string careerId, bool isDeleted)
        {
            return repository.GetMany<Recruitment>(c => c.CareerId == careerId && c.IsDeleted == isDeleted).OrderByDescending(c => c.AddedByDate ?? DateTime.Now).ToList();
        }

        public List<Recruitment> GetAllByCareer(string careerId)
        {
            return repository.GetMany<Recruitment>(c => c.CareerId == careerId).ToList();
        }

        public List<Recruitment> GetAllByDepartment(string departmentId)
        {
            return repository.GetMany<Recruitment>(c => c.DepartmentId == departmentId).ToList();
        }

        public List<Recruitment> GetAllByRelated(Recruitment recruitment, int take, bool isDeleted)
        {
            if (recruitment == null)
                return null;

            return repository.GetMany<Recruitment>(c => c.CareerId == recruitment.CareerId && c.RecruitmentTagId == recruitment.RecruitmentTagId && c.IsDeleted == isDeleted).OrderByDescending(c => c.AddedByDate ?? DateTime.Now).Take(take).ToList();
        }

        public List<Recruitment> GetAll(bool isDeleted)
        {
            return repository.GetMany<Recruitment>(c => c.IsDeleted == isDeleted).OrderByDescending(c => c.AddedByDate ?? DateTime.Now).ToList();
        }

        public List<Recruitment> GetAllByExpirationDate(bool isDeleted)
        {
            return repository.GetMany<Recruitment>(c => c.ExpirationDate.HasValue && c.ExpirationDate.Value >= DateTime.Now && c.IsDeleted == isDeleted).OrderByDescending(c => c.AddedByDate ?? DateTime.Now).ToList();
        }

        public List<Recruitment> GetAllByExpirationDate(string careerId, bool isDeleted) 
        {

            return repository.GetMany<Recruitment>(c => c.CareerId == careerId && c.ExpirationDate.HasValue && c.ExpirationDate.Value >= DateTime.Now && c.IsDeleted == isDeleted).OrderByDescending(c => c.AddedByDate ?? DateTime.Now).ToList();
        }

        public List<Recruitment> GetAllByRecruitmentTag(string recruitmentTagId)
        {
            return repository.GetMany<Recruitment>(c => c.RecruitmentTagId == recruitmentTagId).ToList();
        }

        public List<Recruitment> GetAllByRecruitmentSite(string siteId)
        {
            return repository.GetMany<Recruitment>(c => c.SiteId == siteId).ToList();
        }

        public List<Recruitment> GetAllBySearch(string keyword, bool isDeleted)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            keyword = keyword.ToLower();
            return repository.GetMany<Recruitment>(c => (!string.IsNullOrEmpty(c.NameVn) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.DescriptionVn) && c.DescriptionVn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.DescriptionEn) && c.DescriptionEn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.ExperienceVn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.ExperienceVn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.ExperienceEn) && c.ExperienceEn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.SalaryVn) && c.SalaryVn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(c.SalaryEn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                                        && c.IsDeleted == isDeleted)
                                    .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                            .ToList();
        }

        public List<Recruitment> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, DateTime? BeginExpirationDate, DateTime? EndExpirationDate, string[] SiteId, string[] PositionId, string[] DepartmentId, string[] CareerId, string[] RecruitmentTagId)
        {
            SiteId = SiteId != null ? Array.FindAll(SiteId, s => !string.IsNullOrEmpty(s)) : SiteId;
            PositionId = PositionId != null ? Array.FindAll(PositionId, s => !string.IsNullOrEmpty(s)) : PositionId;
            DepartmentId = DepartmentId != null ? Array.FindAll(DepartmentId, s => !string.IsNullOrEmpty(s)) : DepartmentId;
            CareerId = CareerId != null ? Array.FindAll(CareerId, s => !string.IsNullOrEmpty(s)) : CareerId;
            RecruitmentTagId = RecruitmentTagId != null ? Array.FindAll(RecruitmentTagId, s => !string.IsNullOrEmpty(s)) : RecruitmentTagId;
            var _all = repository.All<Recruitment>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(c => (!string.IsNullOrEmpty(c.NameVn) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.DescriptionVn) && c.DescriptionVn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.DescriptionEn) && c.DescriptionEn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.ExperienceVn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.ExperienceVn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.ExperienceEn) && c.ExperienceEn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.SalaryVn) && c.SalaryVn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(c.SalaryEn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                    ).ToList();
            }

            if (SiteId != null && SiteId.Count() > 0)
                _all = _all.Where(c => SiteId.Contains(c.SiteId)).ToList();
            if (PositionId != null && PositionId.Count() > 0)
                _all = _all.Where(c => PositionId.Contains(c.PositionId)).ToList();
            if (DepartmentId != null && DepartmentId.Count() > 0)
                _all = _all.Where(c => DepartmentId.Contains(c.DepartmentId)).ToList();
            if (CareerId != null && CareerId.Count() > 0)
                _all = _all.Where(c => CareerId.Contains(c.CareerId)).ToList();
            if (RecruitmentTagId != null && RecruitmentTagId.Count() > 0)
                _all = _all.Where(c => RecruitmentTagId.Contains(c.RecruitmentTagId)).ToList();
            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();
            if (BeginExpirationDate.HasValue)
                _all = _all.Where(w => w.ExpirationDate.HasValue && w.ExpirationDate.Value >= BeginExpirationDate.Value).ToList();
            if (EndExpirationDate.HasValue)
                _all = _all.Where(w => w.ExpirationDate.HasValue && w.ExpirationDate.Value <= EndExpirationDate.Value).ToList();

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public string Create(Recruitment obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Recruitment>(obj);
        }

        public void Update(Recruitment obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Recruitment>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<News>(id);
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                    repository.Delete<News>(id);
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
                repository.Delete<Recruitment>();
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
