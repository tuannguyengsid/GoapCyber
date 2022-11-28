using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IImageService
    {
        Image GetBy(string id);
        List<Image> GetAll();
        List<Image> GetAll(bool isDeleted);
        List<Image> GetAllByLatest(int take, bool isDeleted);
        List<Image> GetAllByImageLibrary(string imgLibraryId);
        List<Image> GetAllByImageLibrary(string imgLibraryId, bool isDeleted);
        string Create(Image obj);
        void Update(Image obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class ImageService : IImageService
    {
        private readonly IGSIDMongoRepository repository;

        public ImageService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Image GetBy(string id)
        {
            return repository.GetOne<Image>(id);
        }

        public List<Image> GetAll()
        {
            return repository.All<Image>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<Image> GetAll(bool isDeleted)
        {
            return repository.GetMany<Image>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<Image> GetAllByLatest(int take, bool isDeleted)
        {
            return repository.GetMany<Image>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .Take(take)
                                        .ToList();
        }

        public List<Image> GetAllByImageLibrary(string imgLibraryId)
        {
            return repository.GetMany<Image>(c => (c.ImageLibraryIds != null && c.ImageLibraryIds.Any(a => a == imgLibraryId)))
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<Image> GetAllByImageLibrary(string imgLibraryId, bool isDeleted)
        {
            return repository.GetMany<Image>(c => (c.ImageLibraryIds != null && c.ImageLibraryIds.Any(a => a == imgLibraryId)) && c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public string Create(Image obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Image>(obj);
        }

        public void Update(Image obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Image>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Image>(id);
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
                repository.Delete<Image>();
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
