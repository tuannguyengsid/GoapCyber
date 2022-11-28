using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSID.Data;
using GSID.Data.Mongodb;
using GSID.Service;
using GSID.Setting;
using GSID.Service.MongoRepositories.Service;
using System.Web.Mvc;

namespace GSID.Admin.Helpers
{
    public class RBACUser
    {
        public bool IsSysAdmin { get; set; }

        private List<Role> Roles = new List<Role>();

        public RBACUser()
        {
            Roles = GSIDSessionFacade.GSIDSessionUserLogon != null ? GSIDSessionFacade.GSIDSessionUserLogon.Roles : new List<Role>();
            IsSysAdmin = (Roles != null ? (Roles.Where(r => r.IsSysAdmin == true).Count() > 0) : false);
        }

        private static string RequiredPermission(string action, string controller)
        {
            return string.Format("{0}-{1}", controller, action);
        }

        public static bool HasPermission(string action, string controller)
        {
            var Roles = (GSIDSessionFacade.GSIDSessionUserLogon.Roles ?? new List<Role>());
            var IsSysAdmin = (Roles != null ? (Roles.Where(r => r.IsSysAdmin == true).Count() > 0) : false);
            if ((!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action)) || IsSysAdmin)
            {
                bool _bFound = false;
                string _requiredPermission = RequiredPermission(action, controller);

                var _roles = (GSIDSessionFacade.GSIDSessionUserLogon.Roles ?? new List<Role>());
                var _isSysAdmin = (_roles != null ? (_roles.Where(r => r.IsSysAdmin == true).Count() > 0) : false);

                if (_isSysAdmin)
                    return true;
                foreach (Role role in _roles)
                {
                    _bFound = (role.RoleToPermisions.Where(p => p.Permission != null && !string.IsNullOrEmpty(p.Permission.Description) && p.Permission.Description.ToLower() == _requiredPermission.ToLower()).ToList().Count > 0);
                    if (_bFound)
                        break;
                }
                return _bFound;
            }
            else
                return false;
        }

        public bool HasPermission(string requiredPermission)
        {
            bool bFound = false;
            if (IsSysAdmin)
                return true;
            foreach (Role role in this.Roles)
            {
                bFound = (role.RoleToPermisions.Where(p => p.Permission != null && !string.IsNullOrEmpty(p.Permission.Description) && p.Permission.Description.ToLower() == requiredPermission.ToLower()).ToList().Count > 0);
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

            if (lmenu == null)
            {
                if (IsSysAdmin)
                {
                    IPermissionService perService = DependencyResolver.Current.GetService<IPermissionService>();
                    lmenu = perService.GetAllByMenu(true, false);
                }
                else
                {
                    lmenu = new List<Permission>();

                    IPermissionService perService = DependencyResolver.Current.GetService<IPermissionService>();
                    lmenu = perService.GetAllByRoleIds(Roles, true);
                }

                HttpContext.Current.Session[SestionName.MainMenu] = lmenu;
            }
            return lmenu;
        }

        public static List<Permission> GetStaticListMenu()
        {
            List<Permission> lmenu = (List<Permission>)HttpContext.Current.Session[SestionName.MainMenu];
            User _user = (User)HttpContext.Current.Session[SestionName.gsidSessionUserLogon];
            if (_user == null && HttpContext.Current.Request.IsAuthenticated)
            {
                string[] userData = HttpContext.Current.User.Identity.Name.Split('|');
                if (userData[5] == "user")
                {
                    IUserService userService = DependencyResolver.Current.GetService<IUserService>();
                    _user = userService.VerifiedAccount(userData[3]);
                    HttpContext.Current.Session[SestionName.gsidSessionUserLogon] = _user;
                }
            }

            if (_user != null)
            {
                var _roles = (_user.Roles ?? new List<Role>());
                var _isSysAdmin = (_roles != null ? (_roles.Where(r => r.IsSysAdmin == true).Count() > 0) : false);
                if (lmenu == null)
                {
                    if (_isSysAdmin)
                    {
                        IPermissionService perService = DependencyResolver.Current.GetService<IPermissionService>();
                        lmenu = perService.GetAllByMenu(true, false);
                    }
                    else
                    {
                        lmenu = new List<Permission>();

                        IPermissionService perService = DependencyResolver.Current.GetService<IPermissionService>();
                        lmenu = perService.GetAllByRoleIds(_roles, true);
                    }

                    HttpContext.Current.Session[SestionName.MainMenu] = lmenu;
                }
            }

            return lmenu;
        }

        public static string RootPermissionId(HttpRequestBase request)
        {
            IPermissionService _perService = DependencyResolver.Current.GetService<IPermissionService>();

            var _controller         = request.RequestContext.RouteData.Values["controller"].ToString();
            var _action             = request.RequestContext.RouteData.Values["action"].ToString();
            var _requiredPermission = RequiredPermission(_action, _controller);

            var _perObj = _perService.GetByDescription(_requiredPermission);
            if (_perObj != null)
                return ParrentPermissionId(_perObj.ParentId);

            return "";
        }

        public static string ParrentPermissionId(string parentId)
        {
            IPermissionService _perService = DependencyResolver.Current.GetService<IPermissionService>();
            var _perObj = _perService.GetByParent(parentId);
            if (_perObj != null)
            {
                if (string.IsNullOrEmpty(_perObj.ParentId))
                    return _perObj.Id;
                else
                    return ParrentPermissionId(_perObj.ParentId);
            }
            else
            {
                return _perObj.Id;
            }
        }
    }
}