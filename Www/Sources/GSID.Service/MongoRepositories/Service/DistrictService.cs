using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IDistrictService
    {
        District GetBy(string id);
        List<District> GetAllVn();
        List<District> GetAllEn();
        List<District> GetAllVn(bool isDeleted);
        List<District> GetAllEn(bool isDeleted);
        List<District> GetAllByProvinceIsDeleted(string provinceId, Nullable<bool> isDeleted);
        List<District> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] CountryId, string[] ProvinceId);
        string Create(District obj);
        void Update(District obj);
        bool? Delete(string id);
        bool? DeleteAll();
    }

    public class DistrictService : IDistrictService
    {
        private readonly IGSIDMongoRepository repository;
        public DistrictService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public District GetBy(string id)
        {
            return repository.GetOne<District>(id);
        }

        public List<District> GetAllVn()
        {
            return repository.All<District>().OrderBy(c => c.NameVn).ToList();
        }

        public List<District> GetAllEn()
        {
            return repository.All<District>().OrderBy(c => c.NameEn).ToList();
        }

        public List<District> GetAllVn(bool isDeleted)
        {
            return repository.GetMany<District>(c => c.IsDeleted == isDeleted)
                                .OrderBy(c => c.NameVn)
                                    .ToList();
        }

        public List<District> GetAllEn(bool isDeleted)
        {
            return repository.GetMany<District>(c => c.IsDeleted == isDeleted)
                                .OrderBy(c => c.NameEn)
                                    .ToList();
        }

        public List<District> GetAllByProvinceIsDeleted(string provinceId, Nullable<bool> isDeleted)
        {
            return repository.GetMany<District>(c => c.ProvinceId == provinceId 
                                        && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.NameVn)
                                    .ToList();
        }

        public List<District> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] CountryId, string[] ProvinceId)
        {
            CountryId = CountryId != null ? Array.FindAll(CountryId, s => !string.IsNullOrEmpty(s)) : CountryId;
            ProvinceId = ProvinceId != null ? Array.FindAll(ProvinceId, s => !string.IsNullOrEmpty(s)) : ProvinceId;

            var _all = repository.All<District>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(w => (!string.IsNullOrEmpty(w.NameVn) && w.NameVn.ToLower().Contains(keyword))
                                        || (!string.IsNullOrEmpty(w.NameEn) && w.NameEn.ToLower().Contains(keyword))).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();
            if (CountryId != null && CountryId.Count() > 0)
                _all = _all.Where(c => CountryId.Contains(c.CountryId)).ToList();
            if (ProvinceId != null && ProvinceId.Count() > 0)
                _all = _all.Where(c => ProvinceId.Contains(c.ProvinceId)).ToList();

            return _all.OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public string Create(District obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<District>(obj);
        }

        public void Update(District obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<District>(obj);
        }

        public bool? Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<District>(id);
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
                repository.Delete<District>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
