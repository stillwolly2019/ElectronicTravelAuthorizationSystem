  
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
using System.Data;

public partial class Reports_RCWizard_PreviewDispatchReport : AuthenticatedPageClass
{
    byte[] Lbytes = new byte[0];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (String.IsNullOrEmpty(txtTravelDate.Text))
                {
                    ViewReport(DateTime.Now.Date);
                }
                else
                {
                    ViewReport(Convert.ToDateTime(txtTravelDate.Text));
                }
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }
    
    protected void ViewReport(DateTime TravelDate)
    {
        Microsoft.Reporting.WinForms.ServerReport sr = new Microsoft.Reporting.WinForms.ServerReport();
        string ReportMainPath = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
        sr.ReportServerUrl = new System.Uri(ReportMainPath);
        sr.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/Dispatch";
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
        Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("DateOfTravel", TravelDate.Year+"-"+ TravelDate.Month+"-" + TravelDate.Day);
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });
        Export(sr, "PDF", "Dispatch", gname.ToString(), TravelDate.ToString());

        if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }
        else
        {
            System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }

        File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\Dispatch.PDF"), Lbytes);
        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\Dispatch.PDF";
        string id = "Lokiri";
    }

    public bool Export(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string DateOfTravel)
    {
        DateTime TravelDate = Convert.ToDateTime(DateOfTravel);
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
            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\Dispatch.pdf"), FileMode.Create, FileAccess.Write))
            fs.Write(bytes, 0, bytes.Length);
            //using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + TravelDate.Month + "_"+ TravelDate.Day + "_" + TravelDate.Year + ".pdf"), FileMode.Create, FileAccess.Write))
            //    fs.Write(bytes, 0, bytes.Length);

            Lbytes = bytes;
        }
        catch (Exception ex)
        {

        }
        return true;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        PanelMessage.Visible = false;
        try
        {
            if (!String.IsNullOrEmpty(txtTravelDate.Text))
            {
                ViewReport(Convert.ToDateTime(txtTravelDate.Text));
            }
            else
            {
                ViewReport(DateTime.Now.Date);
            }
        }
        catch (Exception ex)
        {
            PanelMessage.Visible = true;
            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
            lblmsg.Text = "Something went wrong " + ex.Message + " , Please contact  your system administrator";

        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtTravelDate.Text = "";
        ViewReport(DateTime.Now.Date);
    }


}

