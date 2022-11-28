using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface ICurriculumVitaeService
    {
        CurriculumVitae GetBy(string id);
        List<CurriculumVitae> GetAll();
        List<CurriculumVitae> GetAll(bool isDeleted);
        List<CurriculumVitae> GetAllByContact(string contactId);
        List<CurriculumVitae> GetAllByContact(string contactId, bool isDeleted);
        List<CurriculumVitae> GetAllByRecruitment(string recruitmentId);
        List<CurriculumVitae> GetAllByRecruitment(string recruitmentId, bool isDeleted);
        List<CurriculumVitae> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, DateTime? BeginExpirationDate, DateTime? EndExpirationDate, string[] SiteId, string[] PositionId, string[] DepartmentId, string[] CareerId, string[] RecruitmentTagId);
        string Create(CurriculumVitae obj);
        void Update(CurriculumVitae obj);
        bool Delete(string id);
        bool DeleteByContact(string contactId);
        bool? DeleteAll();
    }

    public class CurriculumVitaeService : ICurriculumVitaeService
    {
        private readonly IGSIDMongoRepository repository;

        public CurriculumVitaeService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public CurriculumVitae GetBy(string id)
        {
            return repository.GetOne<CurriculumVitae>(id);
        }

        public List<CurriculumVitae> GetAll()
        {
            return repository.All<CurriculumVitae>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<CurriculumVitae> GetAll(bool isDeleted)
        {
            return repository.GetMany<CurriculumVitae>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<CurriculumVitae> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, DateTime? BeginExpirationDate, DateTime? EndExpirationDate, string[] SiteId, string[] PositionId, string[] DepartmentId, string[] CareerId, string[] RecruitmentTagId)
        {
            SiteId = SiteId != null ? Array.FindAll(SiteId, s => !string.IsNullOrEmpty(s)) : SiteId;
            PositionId = PositionId != null ? Array.FindAll(PositionId, s => !string.IsNullOrEmpty(s)) : PositionId;
            DepartmentId = DepartmentId != null ? Array.FindAll(DepartmentId, s => !string.IsNullOrEmpty(s)) : DepartmentId;
            CareerId = CareerId != null ? Array.FindAll(CareerId, s => !string.IsNullOrEmpty(s)) : CareerId;
            RecruitmentTagId = RecruitmentTagId != null ? Array.FindAll(RecruitmentTagId, s => !string.IsNullOrEmpty(s)) : RecruitmentTagId;

            var _allContact = repository.All<Contact>();
            List<string> _contactIds = null;
            List<string> _recruitmentIds = null;

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();

                //Recruitment
                _allContact = _allContact.Where(w => (!string.IsNullOrEmpty(w.FullName) && w.FullName.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.Email) && w.Email.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.PhoneNumber) && w.PhoneNumber.ToLower().Contains(keyword))
                                    ).ToList();
                _contactIds = _allContact.Select(s => s.Id).ToList();

            }
            if (!string.IsNullOrEmpty(keyword)
                    ||(SiteId != null && SiteId.Count() > 0) 
                        || (PositionId != null && PositionId.Count() > 0)
                        || (DepartmentId != null && DepartmentId.Count() > 0)
                        || (CareerId != null && CareerId.Count() > 0)
                        || (RecruitmentTagId != null && RecruitmentTagId.Count() > 0) 
                        || (BeginExpirationDate.HasValue)
                        || (EndExpirationDate.HasValue))
            {
                var _allRecruitment = repository.All<Recruitment>();

                if (!string.IsNullOrEmpty(keyword))
                {
                    //Recruitment
                    _allRecruitment = _allRecruitment.Where(c => (!string.IsNullOrEmpty(c.NameVn) && c.NameVn.ToLower().Contains(keyword.ToLower()))
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
                    _allRecruitment = _allRecruitment.Where(c => SiteId.Contains(c.SiteId)).ToList();
                if (PositionId != null && PositionId.Count() > 0)
                    _allRecruitment = _allRecruitment.Where(c => PositionId.Contains(c.PositionId)).ToList();
                if (DepartmentId != null && DepartmentId.Count() > 0)
                    _allRecruitment = _allRecruitment.Where(c => DepartmentId.Contains(c.DepartmentId)).ToList();
                if (CareerId != null && CareerId.Count() > 0)
                    _allRecruitment = _allRecruitment.Where(c => CareerId.Contains(c.CareerId)).ToList();
                if (RecruitmentTagId != null && RecruitmentTagId.Count() > 0)
                    _allRecruitment = _allRecruitment.Where(c => RecruitmentTagId.Contains(c.RecruitmentTagId)).ToList();
                if (BeginExpirationDate.HasValue)
                    _allRecruitment = _allRecruitment.Where(w => w.ExpirationDate.HasValue && w.ExpirationDate.Value >= BeginExpirationDate.Value).ToList();
                if (EndExpirationDate.HasValue)
                    _allRecruitment = _allRecruitment.Where(w => w.ExpirationDate.HasValue && w.ExpirationDate.Value <= EndExpirationDate.Value).ToList();

                _recruitmentIds = _allRecruitment.Select(s => s.Id).ToList();
            }

            var _all = repository.All<CurriculumVitae>();
            if (_contactIds != null  && _contactIds.Count > 0 && _recruitmentIds != null && _recruitmentIds.Count > 0)
            {
                _all = _all.Where(w => (_contactIds != null && _contactIds.Contains(w.ContactId))
                             || (_recruitmentIds != null && _recruitmentIds.Contains(w.RecruitmentId))).ToList();
            }
            else if (_contactIds != null && _contactIds.Count > 0)
                _all = _all.Where(w => _contactIds.Contains(w.ContactId)).ToList();
            else if (_recruitmentIds != null && _recruitmentIds.Count > 0)
                _all = _all.Where(w => _recruitmentIds.Contains(w.RecruitmentId)).ToList();

            //_all = _all.Where(w => (_contactIds != null && _contactIds.Contains(w.ContactId))
            //             || (_recruitmentIds != null && _recruitmentIds.Contains(w.RecruitmentId))).ToList();

            if (CareerId != null && CareerId.Count() > 0)
                _all = _all.Where(c => CareerId.Contains(c.CareerId)).ToList();

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderByDescending(c => c.AddedByDate ?? DateTime.Now).ToList();
        }

        public List<CurriculumVitae> GetAllByContact(string contactId)
        {
            return repository.GetMany<CurriculumVitae>(c => c.ContactId == contactId)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<CurriculumVitae> GetAllByContact(string contactId, bool isDeleted)
        {
            return repository.GetMany<CurriculumVitae>(c => c.ContactId == contactId && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }
        
        public List<CurriculumVitae> GetAllByRecruitment(string recruitmentId)
        {
            return repository.GetMany<CurriculumVitae>(c => c.RecruitmentId == recruitmentId)
                                .OrderBy(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }
        
        public List<CurriculumVitae> GetAllByRecruitment(string recruitmentId, bool isDeleted)
        {

            return repository.GetMany<CurriculumVitae>(c => c.RecruitmentId == recruitmentId && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public string Create(CurriculumVitae obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<CurriculumVitae>(obj);
        }

        public void Update(CurriculumVitae obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<CurriculumVitae>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<CurriculumVitae>(id);
                result = true;
            }
            catch
            {
            }
            return result;
        }

        public bool DeleteByContact(string contactId)
        {
            bool result = false;
            try
            {
                repository.Delete<CurriculumVitae>(c=> c.ContactId == contactId);
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
                repository.Delete<CurriculumVitae>();
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
