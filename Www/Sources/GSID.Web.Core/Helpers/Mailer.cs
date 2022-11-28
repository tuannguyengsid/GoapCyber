using GSID.Model.ExtraEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit;

namespace GSID.Web.Core.Helpers
{
    //1	Gmail smtp.gmail.com  587
    //1 Imap google   imap.gmail.com 993
    //2	Outlook smtp.live.com   587
    //3	Yahoo Mail  smtp.mail.yahoo.com 465
    //4	Yahoo Mail Plus plus.smtp.mail.yahoo.com    465
    //5	Hotmail smtp.live.com   465
    //6	Office365.com smtp.office365.com  587
    //7	zoho Mail   smtp.zoho.com   465
    public class Mailer
    {
        private ManualResetEvent _doneEvent;

        public Mailer(ManualResetEvent doneEvent)
        {
            _doneEvent = doneEvent;
        }

        public static bool Connection(EmailSTMPConfig MailSender)
        {
            bool result = false;

            try
            {
                using (var client = new ImapClient())
                {
                    client.Connect(MailSender.IMAPSever, MailSender.IMAPPort, true); 
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(new NetworkCredential(MailSender.UserName, MailSender.Password));

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    client.Disconnect(true);
                    result = true;
                }


                //    var smtpClient = new SmtpClient(MailSender.STMPSever);
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = MailSender.Port;
                //smtpClient.EnableSsl = true;
                //smtpClient.Credentials = new NetworkCredential(MailSender.UserName, MailSender.Password);

                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            }
            catch (Exception)
            {

            }
            return result;
        }

        //public static bool SendMail(EmailSTMPConfig MailSender, MailTo mailTo)
        //{
        //    MailAddress MailAddress1 = new MailAddress(MailSender.UserName, MailSender.FullName);
        //    MailAddress MailAddress2 = new MailAddress(mailTo.To, mailTo.FullName);

        //    MailMessage message = new MailMessage(MailAddress1, MailAddress2);
        //    message.Subject = mailTo.Subject;
        //    message.SubjectEncoding = System.Text.Encoding.UTF8;
        //    message.IsBodyHtml = true;
        //    message.Body = mailTo.Body;
        //    message.BodyEncoding = System.Text.Encoding.UTF8;

        //    var smtpClient = new SmtpClient();
        //    smtpClient.Host = MailSender.STMPSever;
        //    smtpClient.Port = MailSender.STMPPort;
        //    smtpClient.TargetName = string.Format("STARTTLS/{0}", MailSender.STMPSever); // Set to avoid MustIssueStartTlsFirst exception
        //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //smtpClient.UseDefaultCredentials = false;
        //    smtpClient.EnableSsl = true;
        //    smtpClient.Credentials = new NetworkCredential(MailSender.UserName, MailSender.Password);
        //    smtpClient.Send(message);


        //    return true;
        //}

        public static bool SendMail(EmailSTMPConfig MailSender, MailTo mailTo)
        {
            // added after emails would not go out to service provider
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            MailMessage msg = new MailMessage("test@msm.com.vn", "hailnn@msm.com.vn");
            msg.Subject = "Your Subject Name";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name: ");
            sb.AppendLine("Mobile Number: ");
            msg.Body = sb.ToString();
            SmtpClient SmtpClient = new SmtpClient();
            SmtpClient.Host = "smtp.office365.com";
            SmtpClient.Port = 587;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.TargetName = "STARTTLS/smtp.office365.com";
            SmtpClient.EnableSsl = true;
            SmtpClient.Credentials = new System.Net.NetworkCredential("test@msm.com.vn", "Msm@2022");
            SmtpClient.UseDefaultCredentials = false; // Tried by commenting this too
            SmtpClient.Timeout = 5000;

            SmtpClient.Send(msg);







            return true;
        }

        public void ThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            _doneEvent.Set();
        }
    }
}
