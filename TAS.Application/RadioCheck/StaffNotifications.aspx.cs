using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RadioCheck_StaffNotifications : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    //Globals g = new Globals();
    //Business.Users u = new Business.Users();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVMRs.PreRender += new EventHandler(GVMRs_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
            FillDDLs();
        }
    }

    void FillDDLs(string Loc = "")
    {
        try
        {
            DataSet ds = new DataSet();
            R.GetAllLookupsList(ref ds);
            ddlStatusCode.DataSource = ds.Tables[8];
            ddlStatusCode.DataBind();
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void Clear()
    {
        //txtLocation.Text = "";
        txtMRNo.Text = "";
        ddlStatusCode.SelectedIndex = -1;
        FillGrid();
    }

    #region MYMR
    void GVMRs_PreRender(object sender, EventArgs e)
    {
        if (GVMRs.Rows.Count > 0)
        {
            GVMRs.UseAccessibleHeader = true;
            GVMRs.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid(bool SearchMode=false)
    {
        try
        {
            DataTable dt = new DataTable();
            string Status = "";
            if (ddlStatusCode.SelectedValue.ToString() == "")
            {
                Status = "00000000-0000-0000-0000-000000000000";
            }
            else
            {
                Status = ddlStatusCode.SelectedValue.ToString();
            }
            if (SearchMode)
            {
                R.SearchStaffMovementRequest(ref dt, txtMRNo.Text.Trim(), Status);
            }
            else
            {
                R.GetStaffMovementRequest(ref dt, txtMRNo.Text.Trim(), Status);
            }

            lblGVMRsCount.Text = dt.Rows.Count.ToString();
            GVMRs.DataSource = dt;
            GVMRs.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
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
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "NotificationOfAbsenceForm.pdf");
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

    protected void GVMRs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVMRs.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GVMRs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VMR")
        {
            Response.Redirect("~/RadioCheck/NotificationFormWizard.aspx?MRNO=" + Encrypt(GVMRs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["MovementRequestNumber"].ToString()) + "&&MRID=" + Encrypt(GVMRs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["MovementRequestID"].ToString()), false);
        }
        if (e.CommandName == "PrintMR")
        {
            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/NotificationOfAbsenceForm";
            string MovementRequestNumber = GVMRs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["MovementRequestNumber"].ToString();
            ReportParameter p = new ReportParameter("MovementRequestNumber", MovementRequestNumber);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.Refresh();
            Export(viewer, "PDF", "Notification Of Absence");
        }

        if (e.CommandName == "Delete")
        {
            GVMRs.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            R.DeleteMovementRequest(GVMRs.DataKeys[GVMRs.SelectedIndex].Values["MovementRequestID"].ToString());
            PanelMessage.Visible = true;
            PanelMessage.CssClass = "alert alert-success alert-dismissable";
            lblmsg.Text = "MR has been deleted successfully";
            lblmsg.ForeColor = System.Drawing.Color.Green;
            FillGrid();
        }

    }
    protected void GVMRs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        FillGrid(true);
    }
    protected void BtnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RadioCheck/NotificationFormWizard.aspx?First=1", false);
    }
    #endregion

}

