using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Setting.FormKeys
{

    public enum StatusModify
    {
        Error = 1,
        Modified = 2
    }

    public enum OrderStatus : int
    {
        WaitApproval = 1,
        AreDelivered = 2,
        Delivered = 3,
        Cancel = 4
    }

    public enum AjaxStatus
    {
        Error = 1,
        Sucessfull = 2
    }

    public enum StatusDelete 
    {
        Error = 1,
        Deleted = 2
    }

    public enum PermissionState: int
    {
        Selected        = 1,
        Undetermined    = 2
    }
    public enum Manufacturer : int
    {
        Manufacturer = 1,
        Partner = 2
    }

    public class TypeSearch
    {
        public const string RedirectAll             = "Search/All";
        public const string RedirectProduct         = "Search/Product";
        public const string RedirectCategory        = "Search/Category";
        public const string RedirectManufactuner    = "Search/Manufactuner";
    }
}
