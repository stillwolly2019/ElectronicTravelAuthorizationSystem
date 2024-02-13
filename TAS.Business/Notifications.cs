using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Business
{
    public class MailModel
    {
        HttpContext context = HttpContext.Current;
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string PWD { get; set; }

        public void SendMail(bool priority=false)
        {
            string SMTP_Domain = ConfigurationManager.AppSettings["SMTP_Domain"];
            string SMTP_Host = ConfigurationManager.AppSettings["SMTP_Host"];
            string SMTP_Account = ConfigurationManager.AppSettings["SMTP_Account"];
            string SMTP_User = ConfigurationManager.AppSettings["SMTP_User"];
            string SMTP_Password = ConfigurationManager.AppSettings["SMTP_Password"];

            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
            MailMessage mail = new MailMessage();

            if (priority)
            {
                mail.Priority = MailPriority.High;
            }
            mail.To.Add(this.To); 
            mail.From = new MailAddress(SMTP_Account);
            mail.Subject = this.Subject;
            mail.Body = this.Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = SMTP_Host;
            smtp.Port = 25;
            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(SMTP_User, SMTP_Password);
            networkCredential.Domain = SMTP_Domain;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

    }


}
