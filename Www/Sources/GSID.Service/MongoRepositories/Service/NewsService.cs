using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface INewsService
    {
        News GetBy(string id);
        News GetRouterVn(string routerid);
        News GetRouterEn(string routerid);
        News IsNameVnAvailable(string name);
        News IsNameVnAvailable(string name, string id);
        News IsNameEnAvailable(string name);
        News IsNameEnAvailable(string name, string id);
        List<News> GetAll();
        List<News> GetAll(bool isDeleted);
        List<News> GetAllByTag(string tagId, bool isDeleted);
        List<News> GetAllByTag(string tagId);
        //List<News> GetAllByRelated(string cateId, string absentId, int take, bool isDeleted);
        //List<News> GetAllByRelated(string cateId, List<string> absentIds, int take, bool isDeleted);
        List<News> GetAllByRelated(string[] tagIds, int take, bool isDeleted);
        List<News> GetAllLatest(int take, bool isDeleted);
        List<News> GetAllLatest(int take, List<string> subIds, bool isDeleted);
        List<News> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] TagPostId, string[] NewsCategoryId);
        //List<News> GetAllLatest(string cateid, int take, List<string> subIds, bool isDeleted);
        //List<News> GetAllByCategory(string cateId, bool isDeleted);
        //List<News> GetAllByCategory(string cateId, List<string> lakeIds, bool isDeleted);
        List<News> GetAllBySearch(string keyword, bool isDeleted);
        string Create(News obj);
        void Update(News obj);
        bool Delete(string id);
        bool Delete(string[] ids);
        bool DeleteAll();
    }

    public class NewsService : INewsService
    {
        IGSIDMongoRepository repository;

        public NewsService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public News GetBy(string id)
        {
            return repository.GetOne<News>(id);
        }
        public News GetRouterVn(string routerid)
        {
            return repository.GetOne<News>(w => w.RouteDataUrlVnId == routerid);
        }

        public News GetRouterEn(string routerid)
        {
            return repository.GetOne<News>(w => w.RouteDataUrlEnId == routerid);
        }

        public News IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<News>(w => w.NameVn.ToLower() == name);
        }

        public News IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<News>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public News IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<News>(w => w.NameEn.ToLower() == name);
        }

        public News IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<News>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        //public News GetBySlugVnCate(string cateid, string slugVn)
        //{
        //    return repository.GetOne<News>(c => c.NewsCategoryId == cateid && c.SlugVn == slugVn);
        //}
        
        //public News GetBySlugEnCate(string cateid, string slugEn)
        //{
        //    return repository.GetOne<News>(c => c.NewsCategoryId == cateid && c.SlugEn == slugEn);
        //}

        public List<News> GetAll()
        {
            return repository.All<News>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<News> GetAll(bool isDeleted)
        {
            return repository.GetMany<News>(c => c.IsDeleted == isDeleted)
                                 .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<News> GetAllByTag(string tagId, bool isDeleted)
        {
            return repository.GetMany<News>(c => (c.TagPostIds != null && c.TagPostIds.Any(a => a == tagId)) && c.IsDeleted == isDeleted)
                                 .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<News> GetAllByTag(string tagId)
        {
            return repository.GetMany<News>(c => (c.TagPostIds != null && c.TagPostIds.Any(a => a == tagId))).ToList();
        }

        public List<News> GetAllByRelated(string[] tagIds, int take, bool isDeleted)
        {
            if (tagIds == null)
                return null;

            return repository.GetMany<News>(c => (c.TagPostIds != null && c.TagPostIds.Any(a => tagIds.Contains(a))) && c.IsDeleted == isDeleted)
                                    .Take(take)
                                        .ToList();
        }

        public List<News> GetAllBySearch(string keyword, bool isDeleted)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            keyword = keyword.ToLower();
            return repository.GetMany<News>(c => ((!string.IsNullOrEmpty(c.NameVn) && c.NameVn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.ShortDescriptionVn) && c.ShortDescriptionVn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.ShortDescriptionEn) && c.ShortDescriptionEn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.DescriptionVn) && c.DescriptionVn.ToLower().Contains(keyword))
                                                    || (!string.IsNullOrEmpty(c.DescriptionEn) && c.DescriptionEn.ToLower().Contains(keyword)))
                                                        && c.IsDeleted == isDeleted)
                                 .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                            .ToList();
        }

        public List<News> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] TagPostId, string[] NewsCategoryId)
        {
            TagPostId = TagPostId != null ? Array.FindAll(TagPostId, s => !string.IsNullOrEmpty(s)) : TagPostId;
            NewsCategoryId = NewsCategoryId != null ? Array.FindAll(NewsCategoryId, s => !string.IsNullOrEmpty(s)) : NewsCategoryId;
            var _all = repository.All<News>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(c => (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.NameVn) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.NameEn) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.ShortDescriptionVn) && c.ShortDescriptionVn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.ShortDescriptionEn) && c.ShortDescriptionEn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.DescriptionVn) && c.DescriptionVn.ToLower().Contains(keyword.ToLower()))
                                                    || (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(c.DescriptionEn) && c.DescriptionEn.ToLower().Contains(keyword.ToLower()))
                                    ).ToList();
            }

            if (TagPostId != null && TagPostId.Count() > 0)
                _all = _all.Where(c => c.TagPostIds != null && c.TagPostIds.Any(a => TagPostId.Contains(a))).ToList();
            if (NewsCategoryId != null && NewsCategoryId.Count() > 0)
                _all = _all.Where(c => NewsCategoryId.Contains(c.NewsCategoryId)).ToList();
            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public List<News> GetAllLatest(int take, bool isDeleted)
        {
            return repository.GetMany<News>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .Take(take)
                                        .ToList();
        }

        public List<News> GetAllLatest(int take, List<string> subIds, bool isDeleted)
        {
            return repository.GetMany<News>(c => !subIds.Contains(c.Id) && c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .Take(take)
                                        .ToList();
        }
        //public List<News> GetAllByRelated(string cateId, string absentId, int take, bool isDeleted) 
        //{
        //    return repository.GetMany<News>(c => c.NewsCategoryId == cateId 
        //                                            && c.Id != absentId
        //                                            && c.IsDeleted == isDeleted)
        //                        .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
        //                            .Take(take)
        //                                .ToList();
        //}

        //public List<News> GetAllByRelated(string cateId, List<string> absentIds, int take, bool isDeleted)
        //{
        //    return repository.GetMany<News>(c => c.NewsCategoryId == cateId
        //                                            && !absentIds.Contains(c.Id)
        //                                            && c.IsDeleted == isDeleted)
        //                        .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
        //                            .Take(take)
        //                                .ToList();
        //}

        //public List<News> GetAllLatest(string cateid, int take, List<string> subIds, bool isDeleted)
        //{

        //    return repository.GetMany<News>(c => !subIds.Contains(c.Id) && c.NewsCategoryId == cateid && c.IsDeleted == isDeleted)
        //                        .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
        //                            .Take(take)
        //                                .ToList();
        //}
        //public List<News> GetAllByCategory(string cateId, bool isDeleted)
        //{
        //    return repository.GetMany<News>(c => c.NewsCategoryId == cateId
        //                                            && c.IsDeleted == isDeleted)
        //                                        .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
        //                                                .ToList();
        //}
        //public List<News> GetAllByCategory(string cateId, List<string> lakeIds, bool isDeleted)
        //{
        //    return repository.GetMany<News>(c => c.NewsCategoryId == cateId 
        //                                            && !lakeIds.Contains(c.Id) 
        //                                            && c.IsDeleted == isDeleted)
        //                                        .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
        //                                                .ToList();
        //}

        public string Create(News obj)
        {
            obj.AddedByDate = DateTime.Now;
            return repository.Insert<News>(obj);
        }

        public void Update(News obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<News>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<News>(id);
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);
                    repository.Delete<News>(id);
                    result = true;
                }
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
                repository.Delete<News>(c => ids.Contains(c.Id));
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
                repository.Delete<News>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

