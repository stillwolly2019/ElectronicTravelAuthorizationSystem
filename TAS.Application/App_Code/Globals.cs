using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Configuration;
using System.Text;
using System.IO;
/// <summary>
/// Summary description for Globals
/// </summary>
public class Globals
{
    public bool CheckDate(String date)
    {
        try
        {
            DateTime dt = DateTime.Parse(date);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool CheckPastDate(DateTime? date)
    {
        try
        {
            if (Convert.ToDateTime(date.Value.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }
        catch
        {
            return false;
        }
    }
    public bool CheckPastDate(DateTime date)
    {
        try
        {
            if (Convert.ToDateTime(date.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch
        {
            return false;
        }
    }
    public enum DateInterval
    {
        Year,
        Month,
        Weekday,
        Day,
        Hour,
        Minute,
        Second
    }
    public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
    {
        TimeSpan ts = date2 - date1;
        switch (interval)
        {
            case DateInterval.Year:
                return date2.Year - date1.Year;
            case DateInterval.Month:
                return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
            case DateInterval.Weekday:
                return Fix(ts.TotalDays) / 7;
            case DateInterval.Day:
                return Fix(ts.TotalDays);
            case DateInterval.Hour:
                return Fix(ts.TotalHours);
            case DateInterval.Minute:
                return Fix(ts.TotalMinutes);
            default:
                return Fix(ts.TotalSeconds);
        }
    }
    private static long Fix(double Number)
    {
        if (Number >= 0)
        {
            return (long)Math.Floor(Number);
        }
        return (long)Math.Ceiling(Number);
    }
    public string GetMimeTypeByFileName(string sFileName)
    {
        string sMime = "application/octet-stream";

        string sExtension = System.IO.Path.GetExtension(sFileName);
        if (!string.IsNullOrEmpty(sExtension))
        {
            sExtension = sExtension.Replace(".", "");
            sExtension = sExtension.ToLower();
            if (sExtension == "pdf")
            {
                sMime = "application/pdf";
            }
            if (sExtension == "xls" || sExtension == "xlsx")
            {
                sMime = "application/ms-excel";
            }
            else if (sExtension == "doc" || sExtension == "docx")
            {
                sMime = "application/msword";
            }
            else if (sExtension == "ppt" || sExtension == "pptx")
            {
                sMime = "application/ms-powerpoint";
            }
            else if (sExtension == "rtf")
            {
                sMime = "application/rtf";
            }
            else if (sExtension == "zip")
            {
                sMime = "application/zip";
            }
            else if (sExtension == "mp3")
            {
                sMime = "audio/mpeg";
            }
            else if (sExtension == "bmp")
            {
                sMime = "image/bmp";
            }
            else if (sExtension == "gif")
            {
                sMime = "image/gif";
            }
            else if (sExtension == "jpg" || sExtension == "jpeg")
            {
                sMime = "image/jpeg";
            }
            else if (sExtension == "png")
            {
                sMime = "image/png";
            }
            else if (sExtension == "tiff" || sExtension == "tif")
            {
                sMime = "image/tiff";
            }
            else if (sExtension == "txt")
            {
                sMime = "text/plain";
            }
        }

        return sMime;
    }
    public void StreamFileToBrowser(string sFileName, byte[] fileBytes)
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        context.Response.Clear();
        context.Response.ClearHeaders();
        context.Response.ClearContent();
        context.Response.AppendHeader("content-length", fileBytes.Length.ToString());
        context.Response.ContentType = GetMimeTypeByFileName(sFileName);
        context.Response.AppendHeader("content-disposition", "attachment; filename=" + sFileName);
        context.Response.BinaryWrite(fileBytes);
        context.ApplicationInstance.CompleteRequest();
    }
    public void SendMessage(string subject, string messageBody, string toAddress, string Attachment)
    {
        try
        {
            SmtpClient SmtpServer = new SmtpClient();
            MailMessage mail = new MailMessage();
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUser"], ConfigurationManager.AppSettings["SMTPPassword"], "AS");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = credential;
            SmtpServer.Host = ConfigurationManager.AppSettings["SMTPServer"];
            SmtpServer.EnableSsl = true;
            SmtpServer.Port = 25;
            mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPEmail"]);
            mail.To.Add(toAddress);
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = messageBody;
            if (Attachment != "")
            {
                Attachment att = new Attachment(Attachment);
                mail.Attachments.Add(att);
            }
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.Send(mail);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }
    public void SendMessageWithCC(string subject, string messageBody, string toAddress, string CCAddress, string Attachment)
    {
        try
        {
            SmtpClient SmtpServer = new SmtpClient();
            MailMessage mail = new MailMessage();
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUser"], ConfigurationManager.AppSettings["SMTPPassword"], "AS");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = credential;
            SmtpServer.Host = ConfigurationManager.AppSettings["SMTPServer"];
            SmtpServer.EnableSsl = true;
            SmtpServer.Port = 25;
            mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPEmail"]);
            mail.To.Add(toAddress);
            mail.CC.Add(CCAddress);
            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = messageBody;
            if (Attachment != "")
            {
                string[] Atts;
                Atts = Attachment.Split(';');
                for (int i = 0; i <= Atts.Length - 2; i++)
                {
                    Attachment att = new Attachment(Atts[i]);
                    mail.Attachments.Add(att);
                }
            }
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.Send(mail);

        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }
    public string DayOfWeekToArabic(string DayName)
    {
        string ArabicDayName;
        switch (DayName)
        {
            case "Sunday":
                ArabicDayName = "الأحد";
                break;
            case "Monday":
                ArabicDayName = "الإثنين";
                break;
            case "Tuesday":
                ArabicDayName = "الثلاثاء";
                break;
            case "Wednesday":
                ArabicDayName = "الأربعاء";
                break;
            case "Thursday":
                ArabicDayName = "الخميس";
                break;
            case "Friday":
                ArabicDayName = "الجمعة";
                break;
            default:
                ArabicDayName = "السبت";
                break;
        }
        return ArabicDayName;
    }
    public byte[] GetPhoto(string filePath)
    {
        FileStream stream = new FileStream(
            filePath, FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(stream);

        byte[] photo = reader.ReadBytes((int)stream.Length);

        reader.Close();
        stream.Close();

        return photo;
    }
}