using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mail;

namespace GSID.Admin.Helpers
{
    public class MailHelper
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string STMP { get; set; }

        public bool SendEmail(string emailTo, string subject, string message)
        {
            bool _flag = false;
            try
            {
                System.Net.Mail.MailMessage objeto_mail = new System.Net.Mail.MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = Port;
                client.Host = STMP;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(UserName, Password);
                objeto_mail.From = new MailAddress(UserName);
                objeto_mail.To.Add(new MailAddress(emailTo));
                objeto_mail.Subject = subject;
                objeto_mail.Body = message;
                objeto_mail.BodyEncoding = UTF8Encoding.UTF8;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(objeto_mail);
                _flag = true;
            }
            catch (Exception)
            {
            }

            return _flag;
        }
    }
}