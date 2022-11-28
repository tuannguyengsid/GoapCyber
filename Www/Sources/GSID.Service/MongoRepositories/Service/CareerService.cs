using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface ICareerService
    {
        Career GetBy(string id);
        Career GetRouterVn(string routerid);
        Career GetRouterEn(string routerid);
        Career IsNameVnAvailable(string name);
        Career IsNameVnAvailable(string name, string id);
        Career IsNameEnAvailable(string name);
        Career IsNameEnAvailable(string name, string id);
        List<Career> GetAll();
        List<Career> GetAll(bool isDeleted);
        List<Career> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        List<Career> GetAllShowRecruitmentPage(bool IsShow);
        List<Career> GetAllShowRecruitmentPage(bool IsShow, bool isDeleted);
        string Create(Career obj);
        void Update(Career obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class CareerService : ICareerService
    {
        private readonly IGSIDMongoRepository repository;

        public CareerService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Career GetBy(string id)
        {
            return repository.GetOne<Career>(id);
        }

        public Career GetRouterVn(string routerid)
        {
            return repository.GetOne<Career>(w => w.RouteDataUrlVnId == routerid);
        }

        public Career GetRouterEn(string routerid)
        {
            return repository.GetOne<Career>(w => w.RouteDataUrlEnId == routerid);
        }

        public Career IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Career>(w => w.NameVn.ToLower() == name);
        }

        public Career IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Career>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public Career IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Career>(w => w.NameEn.ToLower() == name);
        }

        public Career IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Career>(w => w.NameEn.ToLower() == name && w.Id != id);
        }
        public List<Career> GetAll()
        {
            return repository.All<Career>()
                                .OrderBy(c => c.Sort).ToList();
        }

        public List<Career> GetAll(bool isDeleted)
        {
            return repository.GetMany<Career>(c => c.IsDeleted == isDeleted).OrderBy(c => c.Sort).ToList();
        }

        public List<Career> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<Career>();

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

            return _all.OrderBy(c => c.Sort).ToList();
        }

        public List<Career> GetAllShowRecruitmentPage(bool IsShow)
        {
            return repository.GetMany<Career>(c => c.IsShowRecruitmentPage == IsShow).OrderBy(c => c.Sort).ToList();
        }

        public List<Career> GetAllShowRecruitmentPage(bool IsShow, bool isDeleted)
        {
            return repository.GetMany<Career>(c => c.IsShowRecruitmentPage == IsShow && c.IsDeleted == isDeleted).OrderBy(c => c.Sort).ToList();
        }

        public string Create(Career obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Career>(obj);
        }

        public void Update(Career obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Career>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<Career>(id);
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                    repository.Delete<Career>(id);
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
                repository.Delete<Career>();
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
