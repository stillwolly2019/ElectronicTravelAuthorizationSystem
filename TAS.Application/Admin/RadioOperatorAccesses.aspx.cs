using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.Mail;
using System.IO;

public partial class Admin_RadioOperatorAccesses : AuthenticatedPageClass
{
    AuthenticatedPageClass A = new AuthenticatedPageClass();
    Business.UserLocations UL = new Business.UserLocations();
    Business.Users u = new Business.Users();
    Business.Roles r = new Business.Roles();
    Business.Lookups l = new Business.Lookups();
    RadioCheckBusiness.RadioCheck Noti = new RadioCheckBusiness.RadioCheck();
    Business.MailModel MM = new Business.MailModel();
    DataView dv = new DataView();
    Globals g = new Globals();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVUsers.PreRender += new EventHandler(GVUsers_PreRender);
        if (!IsPostBack)
        {
            try
            {

                FillGrid();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }
    void GVUsers_PreRender(object sender, EventArgs e)
    {
        if (GVUsers.Rows.Count > 0)
        {
            GVUsers.UseAccessibleHeader = true;
            GVUsers.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            u.GetAllRadioOperators("", ref dt);
            dv = dt.DefaultView;

            GVUsers.DataSource = dv;
            GVUsers.DataBind();
            lblGVUsersCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVUserRoleAccess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("ibEdit");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibEdit.Visible = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void LnkUserRoleAccessave_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;
            result = UL.InsertRadioOperatorLocationsAccess(hdnUserID.Value,DDLLocation.SelectedValue);
            ClearUserAccessesPopup();
            FillRoleAccessGrid();
            PanelAmsg.Visible = true;

            if (result == -1)
            {
                PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Red;
                lblAmsg.Text = "User already has selected access information";
            }
            else
            {
                PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Green;
                lblAmsg.Text = "Access information has been added successfully";
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ManageRolesAccesses")
            {
                string FullName = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["FullName"].ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openRoleAccessModal('" + FullName + "');", true);
                ClearUserAccessesPopup();
                GVUsers.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                GVUsers.SelectedIndex = -1;
                hdnUserID.Value = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserID"].ToString();
                FillDDLs();
                FillRoleAccessGrid();
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVUserRoleAccess_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVUserRoleAccess.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "DeleteUserRoleAccess")
            {
                UL.DeleteRadioOperatorAccess(Convert.ToInt32(GVUserRoleAccess.DataKeys[GVUserRoleAccess.SelectedIndex].Values["ID"].ToString()));
                PanelAmsg.Visible = true;
                PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                lblAmsg.Text = "Access details deleted successfully";
                lblAmsg.ForeColor = System.Drawing.Color.Red;
                FillRoleAccessGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVUserRoleAccess_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    private void ClearUserAccessesPopup()
    {
        lblAmsg.Text = "";
        PanelAmsg.Visible = false;
        DDLLocation.SelectedIndex = -1;
        GVUserRoleAccess.SelectedIndex = -1;
    }
    void FillRoleAccessGrid()
    {
        DataTable dt = new DataTable();
        UL.GetRadioOperatorLocationsAcceses(hdnUserID.Value,ref dt);
        lblGVUserRoleAccessCount.Text = dt.Rows.Count.ToString();
        GVUserRoleAccess.DataSource = dt;
        GVUserRoleAccess.DataBind();
    }
    void FillDDLs()
    {
        DDLLocation.SelectedIndex = -1;
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Noti.GetAllLookupsList(ref ds);
            DDLLocation.DataSource = ds.Tables[1];
            DDLLocation.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        DDLLocation.SelectedIndex = -1;
    }
    protected void DDLLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillRoleAccessGrid();
    }

    protected void btnAdvSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        u.SearchRadioOperators(txtSearchUsername.Text.Trim(), txtSearchFirstName.Text.Trim(), txtSearchLastName.Text.Trim(), ref dt);

        GVUsers.DataSource = dt;
        GVUsers.DataBind();
        lblGVUsersCount.Text = dt.Rows.Count.ToString();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearchFirstName.Text = "";
        txtSearchLastName.Text = "";
        txtSearchUsername.Text = "";
        FillGrid();
    }
}