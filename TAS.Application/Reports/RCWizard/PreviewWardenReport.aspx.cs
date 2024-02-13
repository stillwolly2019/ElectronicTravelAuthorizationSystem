  
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

public partial class Reports_RCWizard_PreviewWardenReport : AuthenticatedPageClass
{
    HttpContext context = HttpContext.Current;
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    byte[] Lbytes = new byte[0];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                FillHeader();
                IndicateReport();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }
    private void FillHeader()
    {
        try
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            DataTable dtloc = new DataTable();
            DataTable dtzon = new DataTable();
            R.GetWardenLocations(ref dtloc,ui.User_Id.ToString());
            DDLLocation.DataSource = dtloc;
            DDLLocation.DataBind();

            R.GetAllZonesByLocationID(DDLLocation.SelectedValue, ref dtzon);
            DDLZone.DataSource = dtzon;
            DDLZone.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ViewReport(string LocationID, string ZoneID)
    {
        string LocationName = R.GetStaffLocationName(LocationID);
        string ZoneName = R.GetZoneName(ZoneID);
        Microsoft.Reporting.WinForms.ServerReport sr = new Microsoft.Reporting.WinForms.ServerReport();
        string ReportMainPath = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
        sr.ReportServerUrl = new System.Uri(ReportMainPath);
        sr.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/WardenReport";
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

        if (String.IsNullOrEmpty(LocationID) && String.IsNullOrEmpty(ZoneID))
        {
            PanelMessage.Visible = true;
            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
            lblmsg.Text = "Sorry, NO report available.";
        }
        else
        {
            Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("LocationID", LocationID);
            sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });
            Microsoft.Reporting.WinForms.ReportParameter l = new Microsoft.Reporting.WinForms.ReportParameter("ZoneID", ZoneID);
            sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { l });
            Export(sr, "PDF", "WardenReport", gname.ToString(),LocationID, ZoneID);
        }

        if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }
        else
        {
            System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }

        File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\WardenReport.PDF"), Lbytes);
        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\WardenReport.PDF";
    }
    public bool Export(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string LocationName, string ZoneName)
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
            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + ZoneName + "_" + LocationName + "_" + DateTime.Now.Month + "_"+ DateTime.Now.Day + "_" + DateTime.Now.Year + ".pdf"), FileMode.Create, FileAccess.Write))
                fs.Write(bytes, 0, bytes.Length);

            Lbytes = bytes;
        }
        catch (Exception ex)
        {

        }
        return true;
    }
    protected void btnSearchReport_Click(object sender, EventArgs e)
    {
        PanelMessage.Visible = false;
        try
        {
            if (DDLLocation.SelectedIndex!=-1 && DDLZone.SelectedIndex != -1)
            {
                string LocationID = DDLLocation.SelectedValue;
                string ZoneID = DDLZone.SelectedValue;
                ViewReport(LocationID,ZoneID);
            }
            else
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.Text = "Sorry, no report to display";
            }
            //PreviewReport();
        }
        catch (Exception ex)
        {
            PanelMessage.Visible = true;
            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
            lblmsg.Text = "Something went wrong " + ex.Message + " , Please contact  your system administrator";

        }
    }
    protected void btnClearReport_Click(object sender, EventArgs e)
    {
        DDLLocation.SelectedIndex = -1;
        DDLZone.SelectedIndex = -1;
        string LocationID = DDLLocation.SelectedValue;
        string ZoneID = DDLZone.SelectedValue;
        ViewReport(LocationID,ZoneID);
    }
    protected void DDLLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        IndicateReport();
    }
    void IndicateReport()
    {
            try
            {
                string LocationID = DDLLocation.SelectedValue.ToString();
                string ZoneID = DDLZone.SelectedValue.ToString();
            if (String.IsNullOrEmpty(LocationID) && String.IsNullOrEmpty(ZoneID))
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.Text = "Sorry, no report to display";
            }
            else
            {
                ViewReport(LocationID, ZoneID);
            }

            }
            catch (Exception ee)
            {
                Response.Write(ee.Message);
            }
    }


}

