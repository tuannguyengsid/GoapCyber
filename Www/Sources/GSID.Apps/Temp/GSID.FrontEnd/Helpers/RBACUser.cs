using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSID.Data;
using GSID.Data.Mongodb;
using GSID.Service;
using GSID.Setting;

namespace GSID.FrontEnd.Helpers
{
    public class RBACUser
    {
        public bool IsSysAdmin { get; set; }

        private List<Role> Roles = new List<Role>();
        
        public RBACUser()
        {
            //Roles = (GSIDSessionFacade.GSIDSessionUserLogon.Roles ?? new List<Role>());
            //IsSysAdmin = (Roles != null ? ( Roles.Where(r => r.IsSysAdmin == true).Count() > 0 ) : false);
        }

        public bool HasPermission(string requiredPermission)
        {
            bool bFound = false;
            //if (IsSysAdmin)
            //    return true;
            foreach (Role role in this.Roles) {
                bFound = (role.RoleToPermisions.Where(p => p.Permission != null && p.Permission.Description.ToLower() == requiredPermission.ToLower()).ToList().Count > 0);
                if (bFound)
                    break;
            }
            return bFound;
        }

        public bool HasRole(string role)
        {
            return (Roles.Where(p => p.Name == role).ToList().Count > 0);
        }

        public bool HasRoles(string roles)
        {
            bool bFound = false;
            string[] _roles = roles.ToLower().Split(';');
            foreach (Role role in this.Roles)
            {
                try
                {
                    bFound = _roles.Contains(role.Name.ToLower());
                    if (bFound)
                        return bFound;
                }
                catch (Exception)
                {
                }
            }
            return bFound;
        }

        public List<Permission> GetListMenu()
        {
            List<Permission> lmenu = (List<Permission>)HttpContext.Current.Session[SestionName.MainMenu];

            //if (lmenu == null) {
            //    if (IsSysAdmin) {
            //        using (GSIDEcommerceEntities db = new GSIDEcommerceEntities())
            //        {
            //            lmenu = db.Permissions.Where(c => c.IsMenu == true).ToList();
            //        }
            //    }
            //    else {
            //        lmenu = new List<Permission>();

            //        Roles.ForEach(mapping => {
            //            lmenu.AddRange(mapping.Permissions.Where(p2 => lmenu.All(p1 => p1.Id != p2.Id) 
            //                                                            && p2.IsMenu == true));
            //        });
            //    }

            //    HttpContext.Current.Session[SestionName.MainMenu] = lmenu;
            //}
            return lmenu;
        }
    }
}