using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WebForms;
public partial class TravelAuthorization_SearchTravelAuthorization : AuthenticatedPageClass
{
    Business.TravelAuthorization TA = new Business.TravelAuthorization();
    Business.Lookups l = new Business.Lookups();
    Business.Security Sec = new Business.Security();
    Globals g = new Globals();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVTAs.PreRender += new EventHandler(GVTAs_PreRender);
        try
        {
            if (!IsPostBack)
            {
                FillDDLs();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }   
    }
    void FillDDLs()
    {
        try
        {
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds);

            ddlStatusCode.DataSource = ds.Tables[5];
            ddlStatusCode.DataBind();
            //ddlStatusCode.SelectedValue = "PEN";
            DataTable dtr = new DataTable();
            Sec.GetRoleNameByUserID(ref dtr);
            if (dtr.Rows[0]["RoleName"].ToString() == "Finance")
            {
                ddlStatusCode.SelectedValue = "TECCom";
                ddlStatusCode.Enabled = false;
                FillGrid();
            }
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void GVTAs_PreRender(object sender, EventArgs e)
    {
        if (GVTAs.Rows.Count > 0)
        {
            GVTAs.UseAccessibleHeader = true;
            GVTAs.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            //DateTime FiledDateFrom = Convert.ToDateTime("1900-01-01");
            //if (g.CheckDate(txtFiledDateFrom.Text))
            //{
            //    FiledDateFrom = Convert.ToDateTime(txtFiledDateFrom.Text);
            //}
            string Status = "";
            if (ddlStatusCode.SelectedValue.ToString() == "" && txtWBSCode.Text.Trim()=="")
            {
                Status ="";
            }
            else
            {
                Status = ddlStatusCode.SelectedValue.ToString();
            }
            string location = "";
            if(txtLocation.Text!="")
            {
                location = txtLocation.Text.ToString();
            }
            TA.SearchTravelAuthorization(txtTANo.Text.Trim(), txtWBSCode.Text.Trim(), Status,location, ref dt);
            lblGVTAsCount.Text = dt.Rows.Count.ToString();
            GVTAs.DataSource = dt;
            GVTAs.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void GVTAs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VTA")
        {
            Response.Redirect("~/TravelAuthorization/TravelAuthorizationFormWizard.aspx?TANO=" + Encrypt(GVTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString()) + "&&TAID=" + Encrypt(GVTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString()), false);
        }

        if (e.CommandName == "PrintTA")
        {
            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);

            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/Travel Authorization Form";
            string TravelAuthorizationNumber = GVTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString();
            ReportParameter p = new ReportParameter("TravelAuthorizationNumber", TravelAuthorizationNumber);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.Refresh();

            Export(viewer, "PDF", "Travel Authorizatoin Form");
        }
    }
    public bool Export(ReportViewer viewer, string exportType, string reportsTitle)
    {
        try
        {
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
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "MyTA.pdf");
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
    public bool ExportTE(ReportViewer viewer, string exportType, string reportsTitle)
    {
        try
        {
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;

            filetype = "PDF";
            byte[] bytes = viewer.ServerReport.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "TEC.Pdf");
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
    protected void GVTAs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVTAs_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        GVTAs.SelectedIndex = -1;
        txtTANo.Text = "";
        txtWBSCode.Text = "";
        txtLocation.Text = "";
        ddlStatusCode.SelectedIndex = -1;
        lblGVTAsCount.Text = "0";
        GVTAs.DataSource = null;
        GVTAs.DataBind();
    }
}