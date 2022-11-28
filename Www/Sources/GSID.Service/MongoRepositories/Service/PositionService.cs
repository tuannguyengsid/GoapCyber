using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IPositionService
    {
        Position GetBy(string id);
        Position IsNameVnAvailable(string name);
        Position IsNameVnAvailable(string name, string id);
        Position IsNameEnAvailable(string name);
        Position IsNameEnAvailable(string name, string id);
        List<Position> GetAll();
        List<Position> GetAll(bool isDeleted);
        List<Position> GetAllByDepartmentIsDeleted(string departmentId, bool isDeleted);
        List<Position> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(Position obj);
        void Update(Position obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class PositionService : IPositionService
    {
        IGSIDMongoRepository repository;

        public PositionService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Position GetBy(string id)
        {
            return repository.GetOne<Position>(id);
        }

        public Position IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Position>(w => w.NameVn.ToLower() == name);
        }

        public Position IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Position>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public Position IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Position>(w => w.NameEn.ToLower() == name);
        }

        public Position IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Position>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<Position> GetAll()
        {
            return repository.All<Position>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Position> GetAll(bool isDeleted)
        {
            return repository.GetMany<Position>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Position> GetAllByDepartmentIsDeleted(string departmentId, bool isDeleted)
        {
            return repository.GetMany<Position>(c => c.DepartmentId == departmentId 
                                                        && c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Position> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<Position>();

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

        public string Create(Position obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Position>(obj);
        }

        public void Update(Position obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Position>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Position>(id);
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
                repository.Delete<Position>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

