using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IRouteDataUrlService
    {
        RouteDataUrl GetBy(string id);
        RouteDataUrl GetByUrl(string url, string id);
        RouteDataUrl GetByUrl(string url);
        RouteDataUrl GetByUrl(string url, bool isUrlRequired);
        RouteDataUrl GetByPage(string pageId);
        RouteDataUrl IsTitleAvailable(string title, string id);
        RouteDataUrl IsTitleAvailable(string title, string id, string idExtra);
        RouteDataUrl IsKeywordAvailable(string keyword, string id);
        RouteDataUrl IsKeywordAvailable(string keyword, string id, string idExtra);
        RouteDataUrl IsDescriptionAvailable(string description, string id);
        RouteDataUrl IsDescriptionAvailable(string description, string id, string idExtra);
        RouteDataUrl IsUrlAvailable(string url, string id);
        List<RouteDataUrl> GetAll();
        List<RouteDataUrl> GetAll(bool isDeleted);
        string Create(RouteDataUrl obj);
        void Update(RouteDataUrl obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class RouteDataUrlService : IRouteDataUrlService
    {
        IGSIDMongoRepository repository;

        public RouteDataUrlService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public RouteDataUrl GetBy(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return repository.GetOne<RouteDataUrl>(id);
        }

        public RouteDataUrl GetByUrl(string url, string id)
        {
            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(c => c.Url == url);
            else
                return repository.GetOne<RouteDataUrl>(c => c.Id != id && c.Url == url);
        }

        public RouteDataUrl GetByUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            url = url.Trim().ToLower();

            return repository.GetOne<RouteDataUrl>(w => !string.IsNullOrEmpty(w.Url) && w.Url == url);
        }

        public RouteDataUrl GetByUrl(string url, bool isUrlRequired)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            url = url.Trim().ToLower();
            if (isUrlRequired)
                return repository.GetOne<RouteDataUrl>(w => w.Url == url && w.IsUrlRequired == true);
            else if (!isUrlRequired && !string.IsNullOrEmpty(url))
                return repository.GetOne<RouteDataUrl>(w => !string.IsNullOrEmpty(w.Url) && w.Url == url);
            else
                return null;
        }

        public RouteDataUrl GetByPage(string pageId)
        {
            if (string.IsNullOrEmpty(pageId))
                return null;
            return repository.GetOne<RouteDataUrl>(w => w.EditablePageId == pageId);
        }


        public RouteDataUrl IsUrlAvailable(string url, string id)
        {
            url = !string.IsNullOrEmpty(url) ? url.Trim().ToLower() : "";

            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(w => w.Url.ToLower() == url);
            else
                return repository.GetOne<RouteDataUrl>(w => w.Url.ToLower() == url && w.Id != id);
        }

        public RouteDataUrl IsTitleAvailable(string title, string id)
        {
            title = !string.IsNullOrEmpty(title) ? title.Trim().ToLower() : "";

            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(w => w.Title.ToLower() == title);
            else
                return repository.GetOne<RouteDataUrl>(w => w.Title.ToLower() == title && w.Id != id);
        }

        public RouteDataUrl IsTitleAvailable(string title, string id, string idExtra)
        {

            title = !string.IsNullOrEmpty(title) ? title.Trim().ToLower() : "";

            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(w => w.Title.ToLower() == title);
            else
                return repository.GetOne<RouteDataUrl>(w => w.Title.ToLower() == title && w.Id != id && w.Id != idExtra);
        }

        public RouteDataUrl IsKeywordAvailable(string keyword, string id)
        {
            keyword = !string.IsNullOrEmpty(keyword) ? keyword.Trim().ToLower() : "";

            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(w => w.Keyword.ToLower() == keyword);
            else
                return repository.GetOne<RouteDataUrl>(w => w.Keyword.ToLower() == keyword && w.Id != id);
        }

        public RouteDataUrl IsKeywordAvailable(string keyword, string id, string idExtra)
        {
            keyword = !string.IsNullOrEmpty(keyword) ? keyword.Trim().ToLower() : "";

            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(w => w.Keyword.ToLower() == keyword);
            else
                return repository.GetOne<RouteDataUrl>(w => w.Keyword.ToLower() == keyword && w.Id != id && w.Id != idExtra);
        }

        public RouteDataUrl IsDescriptionAvailable(string description, string id)
        {
            description = !string.IsNullOrEmpty(description) ? description.Trim().ToLower() : "";

            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(w => w.Description.ToLower() == description);
            else
                return repository.GetOne<RouteDataUrl>(w => w.Description.ToLower() == description && w.Id != id);
        }

        public RouteDataUrl IsDescriptionAvailable(string description, string id, string idExtra)
        {
            description = !string.IsNullOrEmpty(description) ? description.Trim().ToLower() : "";

            if (string.IsNullOrEmpty(id))
                return repository.GetOne<RouteDataUrl>(w => w.Description.ToLower() == description);
            else
                return repository.GetOne<RouteDataUrl>(w => w.Description.ToLower() == description && w.Id != id && w.Id != idExtra);
        }


        public List<RouteDataUrl> GetAll()
        {
            return repository.All<RouteDataUrl>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<RouteDataUrl> GetAll(bool isDeleted)
        {
            return repository.GetMany<RouteDataUrl>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public string Create(RouteDataUrl obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<RouteDataUrl>(obj);
        }

        public void Update(RouteDataUrl obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<RouteDataUrl>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<RouteDataUrl>(id);
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
                repository.Delete<RouteDataUrl>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}
