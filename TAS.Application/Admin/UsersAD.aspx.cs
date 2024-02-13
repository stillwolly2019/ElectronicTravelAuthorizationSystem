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

public partial class Admin_UsersAD : AuthenticatedPageClass
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

                LnkSave.Enabled = this.CanAdd;
                FillLookups();
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
            u.GetAllUsers("", ref dt);
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
    void FillLookups()
    {
        try
        {
            DataTable dt = new DataTable();
            r.GetAllRoles(ref dt);
            chkRoles.DataSource = dt;
            chkRoles.DataTextField = "RoleName";
            chkRoles.DataValueField = "RoleID";
            chkRoles.DataBind();

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("ibEdit");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                LinkButton ibDeactivate = (LinkButton)e.Row.FindControl("ibDeactivate");
                LinkButton ibUserRoles = (LinkButton)e.Row.FindControl("ibUserRoles");
                bool Active = Convert.ToBoolean(GVUsers.DataKeys[e.Row.RowIndex].Values["Active"]);
                if (Active)
                {
                    ibDeactivate.CssClass = "fa fa-check";
                    ibDeactivate.ToolTip = "Click to Deactivate";
                }
                else
                {
                    ibDeactivate.ForeColor = System.Drawing.Color.Red;
                    ibDeactivate.CssClass = "fa fa-times";
                    ibDeactivate.ToolTip = "Activate";
                    //e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;
                    e.Row.ForeColor= System.Drawing.Color.Red;
                    ibEdit.ForeColor= System.Drawing.Color.Red;
                    ibDelete.ForeColor= System.Drawing.Color.Red;
                    ibDeactivate.ForeColor = System.Drawing.Color.Red;
                    ibUserRoles.ForeColor = System.Drawing.Color.Red;
                }





                ibEdit.Visible = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
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
            result = UL.InsertUserLocations(hdnUserID.Value,DDLUserRoles.SelectedValue,DDLLocation.SelectedValue,DDLDepartment.SelectedValue,DDLUnit.SelectedValue,DDLSubUnit.SelectedValue);
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
                lblAmsg.Text = "Access informaiton has been added successfully";
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
            GVUsers.SelectedIndex = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditUser")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewUser.Visible = false;
                lblmsgAddNewUser.Text = "";

                // Prepare for update
                string UserID = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["UserID"].ToString();
                txtUserName.Text = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["UserName"].ToString();
                txtPersNo.Text = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["PRISM_Number"].ToString();
                txtUserName.Enabled = false;
                txtFirstName.Text = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["FirstName"].ToString();
                txtLastName.Text = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["LastName"].ToString();
                txtEmail.Text = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["Email"].ToString();
                chkIsManager.Checked = Convert.ToBoolean(GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["IsManager"]);
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtEmail.Enabled = false;
                DataTable dt = new DataTable();
                u.GetUserRoles(UserID, ref dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (ListItem ListItem in chkRoles.Items)
                        {
                            if (ListItem.Value == row["RoleID"].ToString())
                            {
                                ListItem.Selected = true;
                            }
                        }
                    }
                }
                // End
            }
            if (e.CommandName == "ActivateDeactivateUser")
            {
                u.ActivateDeactivateUsers(GVUsers.DataKeys[GVUsers.SelectedIndex].Values["UserID"].ToString());
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.Text = "Account operation successfull";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                FillGrid();
            }

            if (e.CommandName == "deleteItem")
            {
                u.DeleteUsers(GVUsers.DataKeys[GVUsers.SelectedIndex].Values["UserID"].ToString());
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.Text = "User has been deleted successfully";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                FillGrid();
            }
            

            if (e.CommandName == "ManageRolesAccesses")
            {
                string FullName = GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["LastName"].ToString() + " " + GVUsers.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["FirstName"].ToString();
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
                UL.DeleteUserRoleAccess(Convert.ToInt32(GVUserRoleAccess.DataKeys[GVUserRoleAccess.SelectedIndex].Values["ID"].ToString()));
                PanelAmsg.Visible = true;
                PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                lblAmsg.Text = "Access details deleted successfully";
                lblAmsg.ForeColor = System.Drawing.Color.Green;
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
        DDLUserRoles.SelectedIndex = -1;
        DDLLocation.SelectedIndex = -1;
        DDLDepartment.SelectedIndex = -1;
        DDLUnit.SelectedIndex = -1;
        DDLSubUnit.SelectedIndex = -1;
        GVUserRoleAccess.SelectedIndex = -1;
    }

    void FillRoleAccessGrid()
    {
        DataTable dt = new DataTable();
        UL.GetUserRolesAcceses(hdnUserID.Value,DDLUserRoles.SelectedValue,DDLLocation.SelectedValue,ref dt);
        lblGVUserRoleAccessCount.Text = dt.Rows.Count.ToString();
        GVUserRoleAccess.DataSource = dt;
        GVUserRoleAccess.DataBind();
    }

    void FillDDLs()
    {
        DDLUserRoles.SelectedIndex = -1;
        DDLDepartment.SelectedIndex = -1;
        DDLUnit.SelectedIndex = -1;
        DDLSubUnit.SelectedIndex = -1;

        try
        {
            DataSet dg = new DataSet();
            DataSet ds = new DataSet();
            DataSet dsU = new DataSet();
            DataSet dsSu = new DataSet();
            UL.GetUserAccessLookupsList(hdnUserID.Value, ref ds);
            DDLUserRoles.DataSource = ds.Tables[0];
            DDLUserRoles.DataBind();

            DDLLocation.DataSource = ds.Tables[1];
            DDLLocation.DataBind();

            //Load departmnets
            DDLDepartment.DataSource = ds.Tables[2];
            DDLDepartment.DataBind();

            //Load Units
            UL.GetUnitsByDepartmentID(DDLDepartment.SelectedValue, ref dsU);
            DDLUnit.DataSource = dsU.Tables[0];
            DDLUnit.DataBind();
            DDLUnit.Enabled = dsU.Tables[0].Rows.Count > 0;

            //Load SubUnits
            UL.GetSubUnitsByUnitID(DDLUnit.SelectedValue, ref dsSu);
            DDLSubUnit.DataSource = dsSu.Tables[0];
            DDLSubUnit.DataBind();
            DDLSubUnit.Enabled = dsSu.Tables[0].Rows.Count > 0;

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillUnitDDLs()
    {
        DDLUnit.SelectedIndex = -1;
        DDLSubUnit.SelectedIndex = -1;
        try
        {
            //Load Units
            DataSet dsU = new DataSet();
            UL.GetUnitsByDepartmentID(DDLDepartment.SelectedValue, ref dsU);
            DDLUnit.DataSource = dsU.Tables[0];
            DDLUnit.DataBind();
            DDLUnit.Enabled = dsU.Tables[0].Rows.Count > 0;

            //Load SubUnits
            DataSet dsSu = new DataSet();
            UL.GetSubUnitsByUnitID(DDLUnit.SelectedValue, ref dsSu);
            DDLSubUnit.DataSource = dsSu.Tables[0];
            DDLSubUnit.DataBind();
            DDLSubUnit.Enabled = dsSu.Tables[0].Rows.Count > 0;

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillSubUnitDDLs()
    {
        DDLSubUnit.SelectedIndex = -1;
        try
        {
            DataSet dsSu = new DataSet();
            UL.GetSubUnitsByUnitID(DDLUnit.SelectedValue, ref dsSu);
            DDLSubUnit.DataSource = dsSu.Tables[0];
            DDLSubUnit.DataBind();
            DDLSubUnit.Enabled = dsSu.Tables[0].Rows.Count > 0;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


    void ClearAll()
    {
        DDLUserRoles.SelectedIndex = -1;
        DDLLocation.SelectedIndex = -1;
        DDLUnit.SelectedIndex = -1;
        DDLSubUnit.SelectedIndex = -1;
    }

    void Clear()
    {
        txtUserName.Text = "";
        txtUserName.Enabled = true;
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtPersNo.Text = "";
        txtFirstName.Enabled = true;
        txtLastName.Enabled = true;
        txtEmail.Enabled = true;
        txtEmail.Text = "";
        chkRoles.SelectedIndex = -1;
        PanelMessageAddNewUser.Visible = false;
        lblmsgAddNewUser.Text = "";
        chkIsManager.Checked = false;
        hfUserName.Value = "";

    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool Roles = false;
            foreach (ListItem ListItem in chkRoles.Items)
            {
                if (ListItem.Selected)
                {
                    Roles = true;
                }
            }
            if (Roles == false)
            {
                PanelMessageAddNewUser.Visible = true;
                PanelMessageAddNewUser.CssClass = "alert alert-danger alert-dismissable";
                lblmsgAddNewUser.ForeColor = Color.Red;
                lblmsgAddNewUser.Text = "Please select a role";
                return;
            }

            if (GVUsers.SelectedIndex == -1)
            {
                if (hfUserName.Value == "")
                {
                    PanelMessageAddNewUser.Visible = true;
                    PanelMessageAddNewUser.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewUser.ForeColor = Color.Red;
                    lblmsgAddNewUser.Text = "This user name does not exist in active directory database.Please select the user name from the auto complete list";
                    return;
                }

                string InsertedID;
                string UserName = txtUserName.Text;
                string FullName = txtFirstName.Text+" "+txtLastName.Text;
                string Email = txtEmail.Text;

                InsertedID = u.InsertUpdateUsers("", txtUserName.Text, txtFirstName.Text, txtLastName.Text, txtEmail.Text, chkIsManager.Checked, txtPersNo.Text.Trim());
                u.DeleteUserRoles(InsertedID);
                for (int i = 0; i <= chkRoles.Items.Count - 1; i++)
                {
                    if (chkRoles.Items[i].Selected)
                    { u.InsertUsersRoles(InsertedID, chkRoles.Items[i].Value); }
                }
                Clear();
                FillGrid();
                PanelMessageAddNewUser.Visible = true;
                PanelMessageAddNewUser.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewUser.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewUser.Text = "User has been added successfully";
                //Send Welcome Email
                if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Email))
                {
                    //Travellor is the Creator
                    DataTable EmailType = new DataTable();
                    Noti.GetEmailContent(ref EmailType, 5);
                    if (EmailType.Rows.Count > 0)
                    {
                        MM.To = Email.Trim();
                        MM.Subject = EmailType.Rows[0]["Subject"].ToString();
                        MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", FullName).Replace("<<UserID>>", UserName);
                        MM.SendMail();
                    }
                }
                //Send Welcome Email

            }
            else
            {
                string UserID = GVUsers.DataKeys[GVUsers.SelectedIndex].Values["UserID"].ToString();
                u.InsertUpdateUsers(UserID, txtUserName.Text, txtFirstName.Text, txtLastName.Text, txtEmail.Text, chkIsManager.Checked, txtPersNo.Text.Trim());
                u.DeleteUserRoles(UserID);
                for (int i = 0; i <= chkRoles.Items.Count - 1; i++)
                {
                    if (chkRoles.Items[i].Selected)
                    { u.InsertUsersRoles(UserID, chkRoles.Items[i].Value); }
                }
                Clear();
                FillGrid();
                PanelMessageAddNewUser.Visible = true;
                PanelMessageAddNewUser.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewUser.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewUser.Text = "User has been updated successfully";
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Text != "")
            {
                DataTable dt = new DataTable();
                u.SearchUsers(txtUserName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    hfUserName.Value = dt.Rows[0]["UserName"].ToString();
                    txtFirstName.Text = dt.Rows[0]["FirstName"].ToString();
                    txtLastName.Text = dt.Rows[0]["LastName"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();
                    txtPersNo.Text = dt.Rows[0]["PRISM_Number"].ToString();
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    txtEmail.Enabled = false;
                }
                else
                {
                    hfUserName.Value = "";
                }
            }
            else
            {
                Clear();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ddlLocationsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void DDLLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDLDepartment.SelectedIndex = -1;
        DDLUnit.SelectedIndex = -1;
        DDLSubUnit.SelectedIndex = -1;
        FillRoleAccessGrid();
    }
    protected void DDLDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        FillUnitDDLs();
        FillRoleAccessGrid();
    }
    protected void DDLUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSubUnitDDLs();
        FillRoleAccessGrid();
    }
    protected void DDLSubUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillRoleAccessGrid();
    }
    protected void LnkAddNewUser_Click(object sender, EventArgs e)
    {
        Clear();
        GVUsers.SelectedIndex = -1;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void btnAdvSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        u.SearchUsers(txtSearchUsername.Text.Trim(), txtSearchFirstName.Text.Trim(), txtSearchLastName.Text.Trim(), ref dt);

        GVUsers.DataSource = dt;
        GVUsers.DataBind();
        lblGVUsersCount.Text = dt.Rows.Count.ToString();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearchFirstName.Text = "";
        txtSearchLastName.Text = "";
        txtSearchUsername.Text = "";
    }
}