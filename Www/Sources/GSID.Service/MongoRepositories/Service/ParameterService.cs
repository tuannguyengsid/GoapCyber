using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static GSID.Model.MongodbModels.Parameter;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IParameterService
    {
        Parameter GetBy(string id);
        Parameter GetByCode(string code);
        List<Parameter> GetAll();
        string Create(Parameter obj);
        bool Update(Parameter obj);
        bool Delete(string id);
        bool DeleteAll();
        bool Delete(string[] ids);
    }

    public class ParameterService : IParameterService
    {
        IGSIDMongoRepository repository;
        public ParameterService(IGSIDMongoRepository _repository)
        {
            repository = _repository;
        }

        public Parameter GetBy(string id)
        {
            return repository.GetOne<Parameter>(id);
        }

        public Parameter GetByCode(string code)
        {
            return repository.GetOne<Parameter>(p => p.Code == code);
        }

        public List<Parameter> GetAll()
        {
            return repository.All<Parameter>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public string Create(Parameter obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Parameter>(obj);
        }

        public bool Update(Parameter obj)
        {
            bool result = false;
            try
            {
                obj.EditedByDate = DateTime.Now;
                repository.Update<Parameter>(obj);
                return result;
            }
            catch
            {
            }
            return result;
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Parameter>(id);
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
                repository.DropCollection<Parameter>();
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
                repository.Delete<Parameter>(c => ids.Contains(c.Id));
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
