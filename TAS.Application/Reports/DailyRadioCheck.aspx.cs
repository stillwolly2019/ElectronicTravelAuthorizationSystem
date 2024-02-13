using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class Reports_DailyRadioCheck : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    HttpContext context = HttpContext.Current;

    protected void Page_Load(object sender, EventArgs e)
    {
        GVMyRCs.PreRender += new EventHandler(GVMyRCs_PreRender);
        if (!IsPostBack)
        {
            try
            {
                FillHeader();
                GetRadioCheckSummary();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }
    void GVMyRCs_PreRender(object sender, EventArgs e)
    {
        if (GVMyRCs.Rows.Count > 0)
        {
            GVMyRCs.UseAccessibleHeader = true;
            GVMyRCs.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    protected void ddlLocationsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetRadioCheckSummary();
    }
    void GetRadioCheckSummary()
    {
        try
        {
            DataTable dt = new DataTable();
            DateTime? RCheckDate;

            if (txtRadioCheckDate.Text == "")
            {
                RCheckDate = null;
            }
            else
            {
                RCheckDate = Convert.ToDateTime(txtRadioCheckDate.Text);
            }
            string LocationID = ddlLocationsName.SelectedValue;
            R.GetRadioCheckSummary(ref dt, RCheckDate, LocationID);
            lblGVRCsCount.Text = dt.Rows.Count.ToString();
            GVMyRCs.DataSource = dt;
            GVMyRCs.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    public bool Export(ReportViewer viewer, string exportType, string reportsTitle,string LocationName, DateTime RadioChechDate)
    {
        try
        {
            string RcDate = RadioChechDate.Day + "_" + RadioChechDate.Month + "_" + RadioChechDate.Year;
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;

            filetype = "Pdf";
            byte[] bytes = viewer.ServerReport.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "DailyRadioCheckReport_"+ LocationName+"_" + RcDate + ".pdf");
            Response.Flush();
            Response.BinaryWrite(bytes);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
        return true;
    }
    public bool ExportExcel(ReportViewer viewer, string exportType, string reportsTitle, DateTime RadioChechDate)
    {
        try
        {
            string RcDate = RadioChechDate.Day + "-" + RadioChechDate.Month + "-" + RadioChechDate.Year;
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;

            filetype = "Excel";
            byte[] bytes = viewer.ServerReport.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "DailyRadioCheckReport_" + RcDate + ".xls");
            Response.Flush();
            Response.BinaryWrite(bytes);

            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
        return true;
    }
    protected void GVMyRCs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVMyRCs.PageIndex = e.NewPageIndex;
        GetRadioCheckSummary();
    }
    protected void GVMyRCs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VRC")
        {
            Response.Redirect("~/Reports/RadioCheckFormWizard.aspx?RCD=" + Encrypt(GVMyRCs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["RadioCheckID"].ToString())+"&loc="+Encrypt(ddlLocationsName.SelectedValue.ToString()), false);
        }
        if (e.CommandName == "PrintRC")
        {
            string LocationID = ddlLocationsName.SelectedValue.Trim();
            string LocLocationNameationName = ddlLocationsName.Text.Trim();
            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/DailyRadioCheck";
            string RadioCheckID = GVMyRCs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["RadioCheckID"].ToString();
            string LocationName = GVMyRCs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["LocationName"].ToString();
            DateTime RadioChechDate = Convert.ToDateTime(R.GetRadioCheckStartDateByRadioCheckID(RadioCheckID));
            ReportParameter p = new ReportParameter("RadioCheckID", RadioCheckID);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            ReportParameter x = new ReportParameter("LocationID", LocationID);
            viewer.ServerReport.SetParameters(new ReportParameter[] { x });
            viewer.ServerReport.Refresh();
            Export(viewer, "PDF", "RadioCheckResults", LocationName, RadioChechDate);
        }

        if (e.CommandName == "PrintExcel")
        {
            string LocationID = ddlLocationsName.SelectedValue.ToString();
            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/DailyRadioCheck";
            string RadioCheckID = GVMyRCs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["RadioCheckID"].ToString();
            DateTime RadioChechDate = Convert.ToDateTime(R.GetRadioCheckStartDateByRadioCheckID(RadioCheckID));
            ReportParameter p = new ReportParameter("RadioCheckID", RadioCheckID);
            ReportParameter x = new ReportParameter("LocationID", LocationID);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.SetParameters(new ReportParameter[] { x });
            viewer.ServerReport.Refresh();
            ExportExcel(viewer, "EXCEL", "RadioCheckResults", RadioChechDate);
        }

    }
    protected void GVMyRCs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        GetRadioCheckSummary();
    }
    protected void BtnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Reports/DailyRadioCheck.aspx", false);
    }

}

