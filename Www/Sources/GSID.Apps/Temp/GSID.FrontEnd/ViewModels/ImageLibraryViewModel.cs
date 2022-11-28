using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class ImageLibraryViewModel
    {
        public List<Image> Images { get; set; }
        public List<ImageLibrary> ImageLibraries { get; set; }
    }
}