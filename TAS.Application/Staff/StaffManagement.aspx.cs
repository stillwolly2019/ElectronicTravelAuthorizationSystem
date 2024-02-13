
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

public partial class Staff_StaffManagement : AuthenticatedPageClass
{
    HttpContext context = HttpContext.Current;
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVStaff.PreRender += new EventHandler(GVStaff_PreRender);
        if (!IsPostBack)
        {
            try
            {
                FillHeader();
                FillGrid();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }
    void GVStaff_PreRender(object sender, EventArgs e)
    {
        if (GVStaff.Rows.Count > 0)
        {
            GVStaff.UseAccessibleHeader = true;
            GVStaff.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    void Clear()
    {
        txtSearchText.Text = "";
        GVStaff.SelectedIndex = -1;
    }
    void FillGrid()
    {
        string LocId = ddlLocationsName.SelectedValue.Trim();
        DataTable dt = new DataTable();
        R.GetAllStaffByLocationID(LocId, txtSearchText.Text, ref dt);
        lblGVStaffCount.Text = dt.Rows.Count.ToString();
        GVStaff.DataSource = dt;
        GVStaff.DataBind();
    }
    protected void ddlLocationsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void GVStaff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibIsActive = (LinkButton)e.Row.FindControl("ibIsActive");
                if (!Convert.ToBoolean(GVStaff.DataKeys[e.Row.RowIndex].Values["Active"]))
                {
                    ibIsActive.CssClass = "fa fa-close";
                    ibIsActive.ToolTip = "Activate staff";
                }
                else
                {
                    ibIsActive.CssClass = "fa fa-check";
                    ibIsActive.ToolTip = "Deactivate Staff";
                }

                LinkButton ibEdit = (LinkButton)e.Row.FindControl("ibEdit");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibEdit.Visible = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
                ibIsActive.Enabled = this.CanEdit;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }

    }
    protected void GVStaff_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "VSI")
        {
            Response.Redirect("~/Staff/StaffManagementFormWizard.aspx?PERNO=" + Encrypt(GVStaff.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["PRISMNumber"].ToString()), false);
        }

        if (e.CommandName == "PrintSI")
        {
            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/StaffInformationForm";
            string PERNO = GVStaff.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["PRISMNumber"].ToString();
            ReportParameter p = new ReportParameter("PERNO", PERNO);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.Refresh();
            Export(viewer, "PDF", "Staff Information Form");
        }

        if (e.CommandName == "IsActive")
        {
            string Result = R.DeactivateStaff(GVStaff.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["PRISMNumber"].ToString());
            if (Result == "AOK")
            {
                lblmsg.Text = "Staff activated successfully";
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Green;
            }
            else if (Result == "DOK")
            {
                lblmsg.Text = "Staff deactivated successfully";
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblmsg.Text = "Failed to Change  status";
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Red;
            }
            FillGrid();
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
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "StaffInformationForm.pdf");
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
    protected void LnkAddNewUser_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Staff/StaffManagementFormWizard.aspx?First=1", false);
    }
    protected void BtnAdvSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        R.GetAllStaffByLocationID(ddlLocationsName.SelectedValue, txtSearchText.Text.Trim(), ref dt);

        GVStaff.DataSource = dt;
        GVStaff.DataBind();
        lblGVStaffCount.Text = dt.Rows.Count.ToString();
    }
    protected void BtnClear_Click(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        lblmsg.Visible = false;
        txtSearchText.Text = "";
        FillGrid();
    }
    protected void LnkAddNewItem_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Staff/StaffManagementFormWizard.aspx?First=1", false);
    }


}

