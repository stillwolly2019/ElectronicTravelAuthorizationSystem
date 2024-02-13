  
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

public partial class Reports_RCWizard_PreviewRadioCheckReport : AuthenticatedPageClass
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
            DataTable dt = new DataTable();
            R.GetRadioOperatorLocations(ref dt,ui.User_Id.ToString());
            ddlLocationsName.DataSource = dt;
            ddlLocationsName.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void PreviewSingleReport(string RadioCheckID,string LocationID)
    {
        string LocationName = R.GetStaffLocationName(LocationID);
        DateTime RadioCheckStartDate = Convert.ToDateTime(R.GetRadioCheckDateByRadioCheckID(RadioCheckID)).Date;
        Microsoft.Reporting.WinForms.ServerReport sr = new Microsoft.Reporting.WinForms.ServerReport();
        string ReportMainPath = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
        sr.ReportServerUrl = new System.Uri(ReportMainPath);
        sr.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/DailyRadioCheckDefaulters";
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

        if (String.IsNullOrEmpty(RadioCheckID))
        {
            PanelMessage.Visible = true;
            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
            lblmsg.Text = "Sorry, there is NO report available.";
        }
        else
        {
            Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("RadioCheckID", RadioCheckID);
            sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });
            Microsoft.Reporting.WinForms.ReportParameter l = new Microsoft.Reporting.WinForms.ReportParameter("LocationID", LocationID);
            sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { l });
            ExportSingle(sr, "PDF", "DailyRadioCheck", gname.ToString(), RadioCheckID, LocationName);
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

        File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\DailyRadioCheckDefaulters.PDF"), Lbytes);
        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\DailyRadioCheckDefaulters.PDF";
    }
    protected void PreviewMultipleReport(String RadioCheckStartDateID, String RadioCheckEndDateID,string LocationID, int TargetCount)
    {
        string LocationName = R.GetStaffLocationName(LocationID);
        Microsoft.Reporting.WinForms.ServerReport sr = new Microsoft.Reporting.WinForms.ServerReport();
        string ReportMainPath = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
        sr.ReportServerUrl = new System.Uri(ReportMainPath);

        sr.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/RadioCheckResults";
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

        Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("RadioCheckStartDateID", RadioCheckStartDateID);
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });

        Microsoft.Reporting.WinForms.ReportParameter l = new Microsoft.Reporting.WinForms.ReportParameter("LocationID", LocationID);
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { l });

        Microsoft.Reporting.WinForms.ReportParameter r = new Microsoft.Reporting.WinForms.ReportParameter("RadioCheckEndDateID", RadioCheckEndDateID);
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { r });

        Microsoft.Reporting.WinForms.ReportParameter q = new Microsoft.Reporting.WinForms.ReportParameter("TargetCount", TargetCount.ToString());
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { q });
        ExportMultiple(sr, "PDF", "RadioCheckResults", gname.ToString(), RadioCheckStartDateID, RadioCheckEndDateID, LocationName);

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
    public bool ExportSingle(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string RCheckID,string LocationName)
    {
        DateTime RadioCheckDate = Convert.ToDateTime(R.GetRadioCheckStartDateByRadioCheckID(RCheckID)).Date;
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
            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + RadioCheckDate.Day+"_"+ LocationName + "_" + RadioCheckDate.Month + "_" + RadioCheckDate.Year + ".pdf"), FileMode.Create, FileAccess.Write))
                fs.Write(bytes, 0, bytes.Length);

            Lbytes = bytes;
        }
        catch (Exception ex)
        {

        }
        return true;
    }
    public bool ExportMultiple(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string RCheckStartID, string RCheckEndID,string LocationName)
    {
        DateTime StartDate = Convert.ToDateTime(R.GetRadioCheckDateByRadioCheckID(RCheckStartID)).Date;
        DateTime EndDate = Convert.ToDateTime(R.GetRadioCheckDateByRadioCheckID(RCheckEndID)).Date;
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

            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + LocationName + "-" + StartDate.Day + "-" + StartDate.Month + "-" + StartDate.Year + "_to_" + EndDate.Day + "-" + EndDate.Month + "-" + EndDate.Year + ".pdf"), FileMode.Create, FileAccess.Write))
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
            if (!String.IsNullOrEmpty(txtStartDate.Text) && !String.IsNullOrEmpty(txtEndDate.Text))
            {
                if (Convert.ToDateTime(txtStartDate.Text.Trim()).Date == Convert.ToDateTime(txtEndDate.Text.Trim()).Date)
                {
                    string RadioCheckStartID = R.GetRadioCheckIDByRadioCheckDate(Convert.ToDateTime(txtStartDate.Text.Trim()));
                    if (String.IsNullOrEmpty(RadioCheckStartID))
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.Text = "No report for selected period";
                    }
                    else
                    {
                        PreviewSingleReport(RadioCheckStartID,ddlLocationsName.SelectedValue);
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(txtTargetCount.Text))
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.Text = "Please enter target number of unaccounted";
                    }
                    else
                    {
                        string RadioCheckStartID = R.RadioCheckStartIDByRadioCheckDate(Convert.ToDateTime(txtStartDate.Text.Trim()));
                        string RadioCheckEndID = R.RadioCheckEndIDByRadioCheckDate(Convert.ToDateTime(txtEndDate.Text.Trim()));
                        if (String.IsNullOrEmpty(RadioCheckStartID) || String.IsNullOrEmpty(RadioCheckEndID))
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.Text = "No report for selected period";
                        }
                        else
                        {
                            PreviewMultipleReport(RadioCheckStartID, RadioCheckEndID,ddlLocationsName.SelectedValue, Convert.ToInt32(txtTargetCount.Text));
                        }
                    }
                }


            }
            else if (!String.IsNullOrEmpty(txtStartDate.Text))
            {
                string RadioCheckID = R.GetRadioCheckIDByRadioCheckDate(Convert.ToDateTime(txtStartDate.Text.Trim()));
                if (String.IsNullOrEmpty(RadioCheckID))
                {
                    if (Convert.ToDateTime(txtStartDate.Text.Trim()).Date == DateTime.Now.Date)
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.Text = "Sorry, no report for today";
                    }
                    else
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.Text = "Sorry, no report for selected day";
                    }
                }
                else
                {
                    PreviewSingleReport(RadioCheckID, ddlLocationsName.SelectedValue);
                }
            }
            else
            {
                string RadioCheckID = R.GetRadioCheckIDByRadioCheckDate(DateTime.Now.Date);
                if (String.IsNullOrEmpty(RadioCheckID))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.Text = "Sorry, no report for today";
                }
                else
                {
                    PreviewSingleReport(RadioCheckID,ddlLocationsName.SelectedValue);
                }
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
        txtStartDate.Text = DateTime.Now.ToString();
        txtEndDate.Text = "";
        string RadioCheckID = R.GetRadioCheckIDByRadioCheckDate(DateTime.Now.Date);
        if (String.IsNullOrEmpty(RadioCheckID))
        {
            PanelMessage.Visible = true;
            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
            lblmsg.Text = "Sorry, no report for today";
        }
        else
        {
            PreviewSingleReport(RadioCheckID,ddlLocationsName.SelectedValue);
        }
    }
    protected void ddlLocationsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        IndicateReport();
    }

    void IndicateReport()
    {
            try
            {
                DateTime? StartDate;
                DateTime? EndDate;
                int TargetCount = 0;
                string LocationID = ddlLocationsName.SelectedValue.ToString();
                if (!String.IsNullOrEmpty(txtTargetCount.Text))
                {
                    TargetCount = Convert.ToInt32(txtTargetCount.Text);
                }
                if (!String.IsNullOrEmpty(txtStartDate.Text) && !String.IsNullOrEmpty(txtEndDate.Text))
                {
                    StartDate = Convert.ToDateTime(txtStartDate.Text.Trim()).Date;
                    EndDate = Convert.ToDateTime(txtEndDate.Text.Trim()).Date;
                    if (StartDate == EndDate)
                    {
                        string RadioCheckID = R.GetRadioCheckIDByRadioCheckDate(StartDate);
                        if (String.IsNullOrEmpty(RadioCheckID))
                        {
                            if (StartDate == DateTime.Now.Date)
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.Text = "Sorry, no report for today";
                            }
                            else
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.Text = "Sorry, no report for selected period";
                            }
                        }
                        else
                        {
                            PreviewSingleReport(RadioCheckID,LocationID);
                        }
                    }
                    else
                    {
                        string StartRadioCheckID = R.GetRadioCheckIDByRadioCheckDate(StartDate);
                        string EndRadioCheckID = R.GetRadioCheckIDByRadioCheckDate(EndDate);
                        if (!String.IsNullOrEmpty(StartRadioCheckID) && !String.IsNullOrEmpty(EndRadioCheckID))
                        {
                            PreviewMultipleReport(StartRadioCheckID, EndRadioCheckID, LocationID, TargetCount);
                        }
                        else
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.Text = "Sorry, no report for selected period";
                        }

                    }
                }
                else if (!String.IsNullOrEmpty(txtStartDate.Text))
                {
                    string RadioCheckID = R.GetRadioCheckIDByRadioCheckDate(Convert.ToDateTime(txtStartDate.Text));
                    if (Convert.ToDateTime(txtStartDate.Text).Date == DateTime.Now.Date)
                    {
                        if (String.IsNullOrEmpty(RadioCheckID))
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.Text = "Sorry, no report for Today";
                        }
                        else
                        {
                            PreviewSingleReport(RadioCheckID,LocationID);
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(RadioCheckID))
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.Text = "Sorry, no report for selected day";
                        }
                        else
                        {
                            PreviewSingleReport(RadioCheckID,LocationID);
                        }
                    }
                }
                else
                {
                    string RadioCheckID = R.GetLatestRadioCheckIDByRadioCheckDate();
                    if (String.IsNullOrEmpty(RadioCheckID))
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.Text = "Sorry, no report for today";
                    }
                    else
                    {
                        PreviewSingleReport(RadioCheckID,LocationID);
                    }
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee.Message);
            }
    }


}

