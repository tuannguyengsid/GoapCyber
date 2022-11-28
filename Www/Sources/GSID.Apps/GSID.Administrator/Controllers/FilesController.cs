using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSID.FileManagement;
using System.IO;

namespace GSID.Admin.Controllers
{
    public class FilesController : BaseAuthenticationController
    {
        public FilesController()
        {

        }
        private Connector _connector;

        public Connector Connector
        {
            get
            {
                if (_connector == null)
                {
                    FileSystemDriver driver = new FileSystemDriver();
                    string mapPath = string.Format(@"{0}", GSIDSessionSiteInformation.PathAddressSiteMultimedia);
                    DirectoryInfo thumbsStorage = new DirectoryInfo(mapPath);
                    driver.AddRoot(new Root(new DirectoryInfo(mapPath), "/")
                    {
                        Alias = GSIDSessionSiteInformation.UrlAddressSiteMultimedia,
                        IsLocked = false,
                        IsReadOnly = false,
                        IsShowOnly = false,
                        ThumbnailsStorage = thumbsStorage,
                        MaxUploadSizeInMb = 100,
                        ThumbnailsUrl = "/Thumbnails/"
                    });
                    _connector = new Connector(driver);
                }
                return _connector;
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Browser()
        {
            return Connector.Process(this.HttpContext.Request);
        }

        public ActionResult SelectFile(string target)
        {
            return Json(Connector.GetFileByHash(target).FullName);
        }

        public ActionResult Thumbs(string tmb)
        {
            return Connector.GetThumbnail(Request, Response, tmb);
        }
    }
}