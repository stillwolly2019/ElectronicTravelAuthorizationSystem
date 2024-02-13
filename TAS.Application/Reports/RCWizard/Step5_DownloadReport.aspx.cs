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

public partial class Reports_Step5_DownloadReport : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    byte[] Lbytes = new byte[0];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LinkButton lnk = new LinkButton();
                lnk = (LinkButton)WizardHeader.FindControl("lbStepDownload");
                lnk.CssClass = "btn btn-info fa fa-download btn-circle btn-lg";

                if (Request["RCD"]!=null && Request["loc"] != null)
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
    protected void ViewMRForm()
    {
        Microsoft.Reporting.WinForms.ServerReport sr = new Microsoft.Reporting.WinForms.ServerReport();
        string ReportMainPath = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
        sr.ReportServerUrl = new System.Uri(ReportMainPath);
        sr.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/DailyRadioCheck";
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

        Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("RadioCheckID", Decrypt(Request["RCD"]));
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });


        Microsoft.Reporting.WinForms.ReportParameter x = new Microsoft.Reporting.WinForms.ReportParameter("LocationID", Decrypt(Request["loc"]));
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { x });

        Export(sr, "PDF", "RadioCheckResults", gname.ToString(), Decrypt(Request["RCD"]), Decrypt(Request["loc"]));

        if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }
        else
        {
            System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }

        File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\RadioCheckResults.PDF"), Lbytes);

        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\RadioCheckResults.PDF";


    }
    public bool Export(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string RCheckID,string LocID)
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

            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + RCheckID+""+ LocID + ".pdf"), FileMode.Create, FileAccess.Write))
                fs.Write(bytes, 0, bytes.Length);

            Lbytes = bytes;
        }
        catch (Exception ex)
        {

        }
        return true;
    }

}

