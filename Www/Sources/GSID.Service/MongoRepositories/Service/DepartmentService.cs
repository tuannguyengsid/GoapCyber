using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IDepartmentService
    {
        Department GetBy(string id);
        Department IsNameVnAvailable(string name);
        Department IsNameVnAvailable(string name, string id);
        Department IsNameEnAvailable(string name);
        Department IsNameEnAvailable(string name, string id);
        List<Department> GetAll();
        List<Department> GetAll(bool isDeleted);
        List<Department> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(Department obj);
        void Update(Department obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class DepartmentService : IDepartmentService
    {
        IGSIDMongoRepository repository;

        public DepartmentService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Department GetBy(string id)
        {
            return repository.GetOne<Department>(id);
        }

        public Department IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Department>(w => w.NameVn.ToLower() == name);
        }

        public Department IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Department>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public Department IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Department>(w => w.NameEn.ToLower() == name);
        }

        public Department IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Department>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<Department> GetAll()
        {
            return repository.All<Department>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Department> GetAll(bool isDeleted)
        {
            return repository.GetMany<Department>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Department> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<Department>();

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

        public string Create(Department obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Department>(obj);
        }

        public void Update(Department obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Department>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Department>(id);
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
                repository.Delete<Department>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

