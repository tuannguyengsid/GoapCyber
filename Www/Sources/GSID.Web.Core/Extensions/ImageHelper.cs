using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Extensions
{
    public static class ImageHelper
    {
        public static bool IsSmallerThan(this Size size1, Size size2)
        {
            return size2.Width > size1.Width && size2.Height > size1.Height;
        }
    }
}
