using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IWardService
    {
        Ward GetBy(string id);
        List<Ward> GetAll();
        List<Ward> GetAll(bool isDeleted);
        List<Ward> GetAllByDistrictIsDeleted(string districtId,Nullable<bool> isDeleted); 
        List<Ward> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] CountryId, string[] ProvinceId, string[] DistrictId);
        string Create(Ward obj);
        void Update(Ward obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class WardService : IWardService
    {
        private readonly IGSIDMongoRepository repository;

        public WardService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Ward GetBy(string id)
        {
            return repository.GetOne<Ward>(id);
        }

        public List<Ward> GetAll()
        {
            return repository.All<Ward>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Ward> GetAll(bool isDeleted)
        {
            return repository.GetMany<Ward>(c => c.IsDeleted == isDeleted)
                                .OrderBy(c => c.NameVn)
                                    .ToList();
        }

        public List<Ward> GetAllByDistrictIsDeleted(string districtId, Nullable<bool> isDeleted)
        {
            return repository.GetMany<Ward>(c => c.DistrictId == districtId 
                                            && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.NameVn)
                                    .ToList();
        }

        public List<Ward> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] CountryId, string[] ProvinceId, string[] DistrictId)
        {
            CountryId = CountryId != null ? Array.FindAll(CountryId, s => !string.IsNullOrEmpty(s)) : CountryId;
            ProvinceId = ProvinceId != null ? Array.FindAll(ProvinceId, s => !string.IsNullOrEmpty(s)) : ProvinceId;
            DistrictId = DistrictId != null ? Array.FindAll(DistrictId, s => !string.IsNullOrEmpty(s)) : DistrictId;

            var _all = repository.All<Ward>();

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
            if (DistrictId != null && DistrictId.Count() > 0)
                _all = _all.Where(c => DistrictId.Contains(c.DistrictId)).ToList();

            return _all.OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public string Create(Ward obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Ward>(obj);
        }

        public void Update(Ward obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Ward>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Ward>(id);
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
                repository.Delete<Ward>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
