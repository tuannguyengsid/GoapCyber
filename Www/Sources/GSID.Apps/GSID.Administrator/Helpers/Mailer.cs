using GSID.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GSID.WebApp.Helpers
{
    public class Mailer
    {
        private const int Timeout = 180000;
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _pass;
        private readonly bool _ssl;

   //     public string Sender { get; set; }
        public string Recipient { get; set; }
        public string RecipientCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentFile { get; set; }

        public Mailer()
        {
            //IParameterService paraService = DependencyResolver.Current.GetService<IParameterService>();
            //var objconfig       = paraService.GetByCode(GSID.Setting.ParameterCodeKey.CONFIG_EMAIL);
            //string configInfor  = objconfig != null ? objconfig.Description : "";
            //var data            = configInfor.Split('|');
            
            ////MailServer - Represents the SMTP Server
            //_host       = data.Count() >= 3 ? data[0].ToString() : "";
            ////Port- Represents the port number
            //_port = int.Parse(data.Count() >= 3 ? data[1].ToString() : "0");
            ////MailAuthUser and MailAuthPass - Used for Authentication for sending email
            //_user = data.Count() >= 3 ? data[2].ToString() : "";
            //_pass = data.Count() >= 3 ? data[3].ToString() : "";
            _ssl = true; //Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
        }

        public void Send()
        {
            try
            {
                // We do not catch the error here... let it pass direct to the caller
                Attachment att = null;
          //      var message = new MailMessage(Sender, Recipient, Subject, Body) { IsBodyHtml = true };
                var message = new MailMessage(_user, Recipient, Subject, Body) { IsBodyHtml = true };
                if (RecipientCC != null)
                {
                    message.Bcc.Add(RecipientCC);
                }
                var smtp = new SmtpClient(_host, _port);

                if (!String.IsNullOrEmpty(AttachmentFile))
                {
                    if (File.Exists(AttachmentFile))
                    {
                        att = new Attachment(AttachmentFile);
                        message.Attachments.Add(att);
                    }
                }

                if (_user.Length > 0 && _pass.Length > 0)
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_user, _pass);
                    smtp.EnableSsl = _ssl;
                }

                smtp.Send(message);

                if (att != null)
                    att.Dispose();
                message.Dispose();
                smtp.Dispose();
            }

            catch (Exception ex)
            {

            }
        }
    }
}