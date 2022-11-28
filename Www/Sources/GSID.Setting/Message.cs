using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Setting
{
    public class Message
    {
        #region Title Box
        public const string TITLE_ERROR = "Lỗi";
        public const string TITLE_REPORT = "Thông báo";
        #endregion

        #region Content Box
        public const string CONTENT_POSTDATA_DEFAUT_ERROR = "Xin vui lòng kiểm tra website hoặc bấm F5 để cập nhật.";
        public const string CONTENT_POSTDATA_CREATE_SUCCESSFULL = "Tạo mới thông tin thành công!";
        public const string CONTENT_POSTDATA_CREATE_EXIST = "Dữ liệu đã tồn tại trong hệ thống.";
        public const string CONTENT_POSTDATA_UPDATE_SUCCESSFULL = "Cập nhật thông tin thành công!";
        public const string CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY = "Dữ liệu không tồn tại hoặc đã bị xóa trước đó.";
        public const string CONTENT_POSTDATA_DELETE_ERROR_OR_EMPTY_CUSTOMER = "Dữ liệu không tồn tại hoặc đã được xóa trước đó hoặc khách hàng này đang tồn tại nhiều đơn hàng. Xin vui lòng kiểm tra và xóa danh sách đơn hàng của khách hàng này.";
        public const string CONTENT_POSTDATA_DELETE_ERROR_OR_ERROR_EXIST_DATA = "{0} có chứa {1}. Vui lòng xóa các {1} trước khi xóa {0}.";

        public const string CONTENT_EMAIL_CONNECTION_FAILT = "Tài khoản email không đăng nhập được. Xin vui lòng kiểm tra!";

        public const string CONTENT_POSTDATA_RESEND_ERROR_OR_EMPTY = "Can't resend email!";
        public const string CONTENT_CATCH_RetryLimitExceededException = "Không thể lưu thông tin. Xin vui lòng thử lại hoặc liên hệ đến nhân viên IT/quản trị viên để được hỗ trợ.";
        public const string LOGIN_SUCCESSFULL = "Đăng nhập thành công!";
        public const string LOGIN_FAIL = "Lỗi đăng nhập. Xin vui lòng kiểm tra lại thông tin email/mật khẩu đăng nhập";
        public const string LOGIN_LOCKED = "Tài khoản của bạn đã bị khóa";
        public const string LOGIN_LOGON = "Tài khoản hiện đang đăng nhập vào hệ thống. ";
        #endregion
    }
    public class Default
    {
        public const string Status_Error = "1";
        public const string Status_Sucessfull = "2";

        public const string Image_Default = "/Content/Control/assets/img/default/default_user.png";
        public const string ImageNullDefault = "/Content/Control/assets/img/default/default_gallery_photo.jpg";

        public const string FileManagementConectDefault = "/connector";



    }

    public class Parameters
    {
        public const string EMAILSYSTEM_AUTOREPLY_NEWTOPIC = "EMAILSYSTEM_AUTOREPLY_NEWTOPIC";

    }
}
