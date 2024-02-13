using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WinForms;
using System.Net;

public partial class RadioCheck_NOAWizard_4_DownloadNOA : AuthenticatedPageClass
{
    byte[] Lbytes = new byte[0];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LinkButton lnk = new LinkButton();
                lnk = (LinkButton)WizardHeader.FindControl("lbStepDownload");
                lnk.CssClass = "btn btn-success btn-circle btn-lg";

                if (!string.IsNullOrEmpty(Request["MRNO"] as string))
                {
                    ViewMRForm();
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee.Message);
            }
        }

    }
    public bool Export(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string uname)
    {
        try
        {
            Microsoft.Reporting.WinForms.Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;

            Guid gname = default(Guid);
            gname = Guid.NewGuid();

            filetype = "Pdf";
            byte[] bytes = viewer.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + uname + ".pdf"), FileMode.Create, FileAccess.Write))
                fs.Write(bytes, 0, bytes.Length);

            Lbytes = bytes;
        }
        catch (Exception ex)
        {

        }
        return true;
    }
    protected void ViewMRForm()
    {

        Microsoft.Reporting.WinForms.ServerReport sr = new Microsoft.Reporting.WinForms.ServerReport();
        string ReportMainPath = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
        sr.ReportServerUrl = new System.Uri(ReportMainPath);
        sr.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/NotificationOfAbsenceForm";

        Guid gname = default(Guid);
        gname = Guid.NewGuid();

        if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }
        else
        {
            System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }

        Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("MovementRequestNumber", Decrypt(Request["MRNO"]));
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });
        Export(sr, "PDF", "Notification of absence Form", gname.ToString(), Decrypt(Request["MRID"]));

        if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }
        else
        {
            System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }

        File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\NotificationOfAbsence.PDF"), Lbytes);

        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\NotificationOfAbsence.PDF";


    }

}

