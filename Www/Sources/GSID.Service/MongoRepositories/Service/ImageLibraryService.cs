using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IImageLibraryService
    {
        ImageLibrary GetBy(string id);
        ImageLibrary IsNameVnAvailable(string name);
        ImageLibrary IsNameVnAvailable(string name, string id);
        ImageLibrary IsNameEnAvailable(string name);
        ImageLibrary IsNameEnAvailable(string name, string id);
        ImageLibrary GetBySlugVn(string slugVn);
        ImageLibrary GetBySlugEn(string slugEn);
        List<ImageLibrary> GetAll();
        List<ImageLibrary> GetAll(bool isDeleted);
        List<ImageLibrary> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(ImageLibrary obj);
        void Update(ImageLibrary obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class ImageLibraryService : IImageLibraryService
    {
        private readonly IGSIDMongoRepository repository;

        public ImageLibraryService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public ImageLibrary GetBy(string id)
        {
            return repository.GetOne<ImageLibrary>(id);
        }

        public ImageLibrary IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ImageLibrary>(w => w.NameVn.ToLower() == name);
        }

        public ImageLibrary IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ImageLibrary>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public ImageLibrary IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ImageLibrary>(w => w.NameEn.ToLower() == name);
        }

        public ImageLibrary IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ImageLibrary>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public ImageLibrary GetBySlugVn(string slugVn)
        {
            return repository.GetOne<ImageLibrary>(c => c.SlugVn == slugVn);
        }

        public ImageLibrary GetBySlugEn(string slugEn)
        {
            return repository.GetOne<ImageLibrary>(c => c.SlugEn == slugEn);
        }

        public List<ImageLibrary> GetAll()
        {
            return repository.All<ImageLibrary>()
                                .OrderBy(c => c.Sort).ToList();
        }

        public List<ImageLibrary> GetAll(bool isDeleted)
        {
            return repository.GetMany<ImageLibrary>(c => c.IsDeleted == isDeleted).OrderBy(c => c.Sort).ToList();
        }

        public List<ImageLibrary> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<ImageLibrary>();

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

        public string Create(ImageLibrary obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<ImageLibrary>(obj);
        }

        public void Update(ImageLibrary obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<ImageLibrary>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<ImageLibrary>(id);
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
                repository.Delete<ImageLibrary>();
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
