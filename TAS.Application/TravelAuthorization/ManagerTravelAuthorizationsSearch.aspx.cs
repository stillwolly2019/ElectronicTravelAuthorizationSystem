using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WebForms;
public partial class TravelAuthorization_ManagerTravelAuthorizationsSearch : AuthenticatedPageClass
{
    Business.TravelAuthorization TA = new Business.TravelAuthorization();
    Business.Lookups l = new Business.Lookups();
    Business.Users u = new Business.Users();
    Globals g = new Globals();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVTAs.PreRender += new EventHandler(GVTAs_PreRender);
        if (!IsPostBack)
        {
            FillDDLs();
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

            DataTable dt = new DataTable();
            u.GetStaffMembersByDepartmentID(ref dt);

            ddlStaffMembers.DataSource = dt;
            ddlStaffMembers.DataBind();

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    #region GVTA
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
            string Status = "";
            if (ddlStatusCode.SelectedValue.ToString() == "")
            {
                Status = "";
            }
            else
            {
                Status = ddlStatusCode.SelectedValue.ToString();
            }
            string location = "";
            if (txtLocation.Text != "")
            {
                location = txtLocation.Text.ToString();
            }
            TA.SearchTravelAuthorizationMangers(txtTANo.Text.Trim(), Status, location, ref dt);
            lblGVTAsCount.Text = dt.Rows.Count.ToString();

            GVTAs.DataSource = dt;
            GVTAs.DataBind();
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
        if (e.CommandName == "PrintTE")
        {
            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/Travel Expense Claim";
            string TravelAuthorizationNumber = GVTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString();
            ReportParameter p = new ReportParameter("TravelAuthorizationNumber", TravelAuthorizationNumber);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.Refresh();
            ExportTE(viewer, "PDF", "Travel Expense Claim");
        }
        if (e.CommandName == "DuplicateTA")
        {
            ClearAll();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            ViewState["TAID"] = GVTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString();
            ViewState["TANO"] = GVTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString();

        }
    }
    protected void GVTAs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Objects.User ui = (Objects.User)Session["userinfo"];

            HiddenField hdnIsTecComplete = new HiddenField();
            hdnIsTecComplete = (HiddenField)e.Row.FindControl("hdnIsTecComplete");

            LinkButton lnkPrintTE;
            lnkPrintTE = (LinkButton)e.Row.FindControl("lnkPrintTE");

            LinkButton lnkDuplicateTA;
            lnkDuplicateTA = (LinkButton)e.Row.FindControl("lnkDuplicateTA");

            if (hdnIsTecComplete.Value.Trim() == "True")
            {
                lnkPrintTE.Visible = true;
            }
            else
            {
                lnkPrintTE.Visible = false;
            }

            if (ui.IsManager == false)
            {
                lnkDuplicateTA.Enabled = false;
            }
        }
    }
    protected void GVTAs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        GVTAs.SelectedIndex = -1;
        txtTANo.Text = "";
        txtLocation.Text = "";
        ddlStatusCode.SelectedIndex = -1;
        lblGVTAsCount.Text = "0";
        GVTAs.DataSource = null;
        GVTAs.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    #endregion

    #region StaffMembers
    protected void ClearAll()
    {
        ViewState["Users"] = null;
        ViewState["TAID"] = null;
        ViewState["TANO"] = null;

        ddlStaffMembers.SelectedIndex = -1;
        PanelMessageDuplicateTA.Visible = false;
        lblmsgDuplicateTA.Text = "";
        PanelMessageDuplicateTARejection.Visible = false;
        lblmsgDuplicateTARejection.Text = "";


        gvStaffMembers.DataSource = null;
        gvStaffMembers.DataBind();
    }
    protected void gvStaffMembers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton ibDeleteStaff;
            ibDeleteStaff = (LinkButton)e.Row.FindControl("ibDeleteStaff");
        }
    }
    protected void gvStaffMembers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            PanelMessageDuplicateTA.Visible = false;
            lblmsgDuplicateTA.Text = "";
            PanelMessageDuplicateTARejection.Visible = false;
            lblmsgDuplicateTARejection.Text = "";

            List<User> user = (List<User>)ViewState["Users"];

            var itemToRemove = user.Single(r => r.UserID == gvStaffMembers.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["UserID"].ToString());
            itemToRemove.isDeleted = true;
            ViewState["Users"] = user;

            user = user.Where(item => item.isDeleted == false).ToList();
            gvStaffMembers.DataSource = user.ToList();
            gvStaffMembers.DataBind();

            ddlStaffMembers.SelectedIndex = -1;

            //PanelMessageDuplicateTA.Visible = true;
            //PanelMessageDuplicateTA.CssClass = "alert alert-success alert-dismissable";
            //lblmsgDuplicateTA.Text = "Staff member has been deleted successfully";
            //lblmsgDuplicateTA.ForeColor = System.Drawing.Color.Green;
        }
    }
    protected void gvStaffMembers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void ibAddStaff_Click(object sender, EventArgs e)
    {
        PanelMessageDuplicateTA.Visible = false;
        lblmsgDuplicateTA.Text = "";
        PanelMessageDuplicateTARejection.Visible = false;
        lblmsgDuplicateTARejection.Text = "";

        List<User> User;
        if (ViewState["Users"] == null)
        {
            User = new List<User>();
        }
        else
        {
            User = (List<User>)ViewState["Users"];
            User = User.Where(item => item.isDeleted == false).ToList();
        }


        User singleUser = new User("", "", false, false);
        bool Exists = false;

        if (User.Count != 0)
        {
            foreach (var item in User)
            {
                if (item.UserID != ddlStaffMembers.SelectedValue)
                {
                    singleUser = new User(ddlStaffMembers.SelectedValue.ToString(), ddlStaffMembers.SelectedItem.Text, false, false);
                }
                else
                {
                    Exists = true;
                    break;
                }
            }
        }
        else
        {
            singleUser = new User(ddlStaffMembers.SelectedValue.ToString(), ddlStaffMembers.SelectedItem.Text, false, false);
        }

        if (singleUser.UserID != "" && Exists != true)
        {
            User.Add(singleUser);
            ddlStaffMembers.SelectedIndex = -1;

            //PanelMessageDuplicateTA.Visible = true;
            //PanelMessageDuplicateTA.CssClass = "alert alert-success alert-dismissable";
            //lblmsgDuplicateTA.Text = "Staff member has been added successfully";
            //lblmsgDuplicateTA.ForeColor = System.Drawing.Color.Green;

        }
        else
        {
            PanelMessageDuplicateTARejection.Visible = true;
            PanelMessageDuplicateTARejection.CssClass = "alert alert-danger alert-dismissable";
            lblmsgDuplicateTARejection.ForeColor = Color.Red;
            lblmsgDuplicateTARejection.Text = "Staff member is already added";
            PanelMessageDuplicateTARejection.Focus();
            return;
        }

        ViewState["Users"] = User;
        User = User.Where(item => item.isDeleted == false).ToList();
        gvStaffMembers.DataSource = User.ToList();
        gvStaffMembers.DataBind();
    }
    protected void btnDuplicate_Click(object sender, EventArgs e)
    {
        if (gvStaffMembers.Rows.Count > 0)
        {


            //Clear msgs and panels (Can't user clear function because we dont want to cleare the viewstates)
            PanelMessageDuplicateTA.Visible = false;
            lblmsgDuplicateTA.Text = "";
            PanelMessageDuplicateTARejection.Visible = false;
            lblmsgDuplicateTARejection.Text = "";

            List<User> user;

            if (ViewState["Users"] != null)
            {
                user = (List<User>)ViewState["Users"];
                user = user.Where(item => item.isDeleted == false).ToList();
            }
            else
            {
                user = new List<User>();
            }

            DataTable dtDup = new DataTable();
            string NamesOfUsersWithDuplicate = "";
            string NamesOfUsersWithCreated = "";

            foreach (var item in user)
            {
                if (item.isDeleted == false)
                {
                    TA.DuplicateTA(ViewState["TAID"].ToString(), item.UserID);
                }
            }

            foreach (var item in user)
            {
                if (item.isDuplicate && item.isDeleted == false)
                {
                    NamesOfUsersWithDuplicate = NamesOfUsersWithDuplicate + "<br>" + item.FullName;
                }
                else
                {
                    NamesOfUsersWithCreated = NamesOfUsersWithCreated + "<br>" + item.FullName;
                }
            }

            if (NamesOfUsersWithDuplicate != "")
            {
                PanelMessageDuplicateTARejection.Visible = true;
                PanelMessageDuplicateTARejection.CssClass = "alert alert-danger alert-dismissable";
                lblmsgDuplicateTARejection.ForeColor = Color.Red;
                lblmsgDuplicateTARejection.Text = "TA is already created for the following staff members" + NamesOfUsersWithDuplicate;
                PanelMessageDuplicateTARejection.Focus();
            }

            if (NamesOfUsersWithCreated != "")
            {
                PanelMessageDuplicateTA.Visible = true;
                PanelMessageDuplicateTA.CssClass = "alert alert-success alert-dismissable";
                lblmsgDuplicateTA.Text = "TA has been created successfully for the following staff members" + NamesOfUsersWithCreated;
                lblmsgDuplicateTA.ForeColor = System.Drawing.Color.Green;
            }
        }
        else
        {
            PanelMessageDuplicateTARejection.Visible = true;
            PanelMessageDuplicateTARejection.CssClass = "alert alert-danger alert-dismissable";
            lblmsgDuplicateTARejection.ForeColor = Color.Red;
            lblmsgDuplicateTARejection.Text = "Press \"+ click to save\" button to add staff member to the list.";
            PanelMessageDuplicateTARejection.Focus();
        }
    }
    #endregion
}