using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace EntityClasses
{

public class MailModel
{
    public string From { get; set; }
    public string To { get; set; }
    public string CC { get; set; }
    public string BCC { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string PWD { get; set; }

    public void SendMail()
    {
        ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
        MailMessage mail = new MailMessage();
        mail.To.Add(this.To);
        mail.From = new MailAddress("PALMS@iom.int");
        mail.Subject = this.Subject;
        mail.Body = this.Body;
        mail.IsBodyHtml = true;
        SmtpClient smtp = new SmtpClient();
        smtp.Host = "172.25.68.9";
        smtp.Port = 25;
        System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential("palms", "iom@2017");
        networkCredential.Domain = "eu";
        smtp.UseDefaultCredentials = true;
        smtp.Credentials = networkCredential;
        smtp.EnableSsl = false;
        smtp.Send(mail);
    }

}

public class TravelauthorizationSystemUser
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }


    }

}