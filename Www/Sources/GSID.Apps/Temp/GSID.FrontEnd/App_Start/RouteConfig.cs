using GSID.FrontEnd.Handlers;
using GSID.FrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GSID.FrontEnd
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("css/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            #region Account
            #region Login
            routes.MapRoute(
                name: "Account_Login_en",
                url: "login",
                defaults: new { controller = "Account", action = "Login", language = "en" }
            );

            routes.MapRoute(
                name: "Account_Login_vn",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login", language = "vn" }
            );
            #endregion

            #region Regisgter
            routes.MapRoute(
                name: "Account_Register_en",
                url: "register",
                defaults: new { controller = "Account", action = "Register", language = "en" }
            );

            routes.MapRoute(
                name: "Account_Register_vn",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register", language = "vn" }
            );
            #endregion

            #region Forgot
            routes.MapRoute(
                name: "Account_Forgot_en",
                url: "Forgot",
                defaults: new { controller = "Account", action = "Forgot", language = "en" }
            );

            routes.MapRoute(
                name: "Account_Forgot_vn",
                url: "quen-mat-khau",
                defaults: new { controller = "Account", action = "Forgot", language = "vn" }
            );
            #endregion

            #region Reset Password
            routes.MapRoute(
                name: "Account_ResetPassword_en",
                url: "resetpassword",
                defaults: new { controller = "Account", action = "ResetPassword", language = "en" }
            );

            routes.MapRoute(
                name: "Account_ResetPassword_vn",
                url: "cap-nhat-mat-khau",
                defaults: new { controller = "Account", action = "ResetPassword", language = "vn" }
            );
            #endregion

            #region Verify Account Register
            routes.MapRoute(
                name: "Account_VerifyRegister_en",
                url: "en/verify-account-registration",
                defaults: new { controller = "Account", action = "VerifyRegistration", language = "en" }
            );

            routes.MapRoute(
                name: "Account_VerifyRegister_vn",
                url: "vn/xac-nhan-dang-ky",
                defaults: new { controller = "Account", action = "VerifyRegistration", language = "vn" }
            );
            #endregion

            #region Log off
            routes.MapRoute(
                name: "Account_LogOff_en",
                url: "en/LogOff",
                defaults: new { controller = "Account", action = "LogOff", language = "en" }
            );

            routes.MapRoute(
                name: "Account_LogOff_vn",
                url: "vn/dang-xuat",
                defaults: new { controller = "Account", action = "LogOff", language = "vn" }
            );
            #endregion
            #endregion

            #region Recruitment

            #region Recruitment Detail Submit
            routes.MapRoute(
                    name: "Recruitment_Detail_Submit_en",
                    url: "en/recruitment-cv-detail",
                    defaults: new { controller = "Recruitment", action = "RecruitmentDetailSubmit", language = "en" }
                );
            routes.MapRoute(
                name: "Recruitment_Detail_Submit_vn",
                url: "vn/tuyen-dung-cv-detail",
                defaults: new { controller = "Recruitment", action = "RecruitmentDetailSubmit", language = "vn" }
            );
            #endregion

            #region Recruitment Submit
            routes.MapRoute(
                name: "Recruitment_Submit_en",
                url: "en/recruitment-cv",
                defaults: new { controller = "Recruitment", action = "RecruitmentSubmit", language = "en" }
            );
            routes.MapRoute(
                name: "Recruitment_submit_vn",
                url: "vn/tuyen-dung-cv/",
                defaults: new { controller = "Recruitment", action = "RecruitmentSubmit", language = "vn" }
            );
            #endregion

            #endregion

            #region Post
            routes.MapRoute(
                name: "Post_en",
                url: "en/news",
                defaults: new { controller = "Post", action = "Index", language = "en" }
            );
            routes.MapRoute(
                name: "Post_vn",
                url: "vn/tin-tuc",
                defaults: new { controller = "Post", action = "Index", language = "vn" }
            );
            #endregion


            routes.MapRoute(
                name: "notfound",
                url: "{language}/notfound",
                defaults: new { controller = "Error", action = "NotFound" }
            );


            routes.MapRoute(
                name: "Search_en",
                url: "search",
                defaults: new { controller = "Search", action = "Index", language = "en" }
            );
            routes.MapRoute(
                name: "Search_vn",
                url: "tim-kiem",
                defaults: new { controller = "Search", action = "Index", language = "vn" }
            );

            routes.MapRoute(
                name: "About_vn",
                url: "ajax/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );


            routes.MapRoute(
                "IUrlRouteHandler",
                "{*urlRouteHandler}").RouteHandler = new UrlRouteHandler();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
