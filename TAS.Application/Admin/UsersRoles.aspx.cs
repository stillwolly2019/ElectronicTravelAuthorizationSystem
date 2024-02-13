using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_UsersRoles : AuthenticatedPageClass
{
    Business.Roles r = new Business.Roles();
    Business.Users u = new Business.Users();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                btnSaveRole.Enabled = this.CanAdd;
                btnSaveUser.Enabled = this.CanAdd;
                DataTable dt = new DataTable();
                u.GetAllUsers("00000000-0000-0000-0000-000000000000", ref dt);
                DDLUsers.DataSource = dt;
                DDLUsers.DataTextField = "FullName";
                DDLUsers.DataValueField = "UserID";
                DDLUsers.DataBind();
                DDLUsers.Items.Insert(0, "");
                lstUsers.DataSource = dt;
                lstUsers.DataTextField = "FullName";
                lstUsers.DataValueField = "UserID";
                lstUsers.DataBind();
                dt = new DataTable();
                r.GetAllRoles(ref dt);
                DDLRoles.DataSource = dt;
                DDLRoles.DataTextField = "RoleName";
                DDLRoles.DataValueField = "RoleID";
                DDLRoles.DataBind();
                DDLRoles.Items.Insert(0, "");
                lstRoles.DataSource = dt;
                lstRoles.DataTextField = "RoleName";
                lstRoles.DataValueField = "RoleID";
                lstRoles.DataBind();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }
    void ClearUser()
    {
        DDLUsers.SelectedIndex = -1;
        lstRoles.SelectedIndex = -1;
        lblmsgUser.Text = "&nbsp;";
    }
    void ClearRole()
    {
        DDLRoles.SelectedIndex = -1;
        lstUsers.SelectedIndex = -1;
        lblmsgRole.Text = "&nbsp;";
    }
    void ReloadBoth()
    {
        try
        {
            if (DDLUsers.SelectedIndex < 1)
            { ClearUser(); }
            else
            {
                DataTable dt = new DataTable();
                u.GetUserRoles(DDLUsers.SelectedValue, ref dt);
                if (dt.Rows.Count == 0)
                {
                    lstRoles.SelectedIndex = -1;
                    return;
                }
                for (int x = 0; x <= lstRoles.Items.Count - 1; x++)
                    lstRoles.Items[x].Selected = false;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    for (int x = 0; x <= lstRoles.Items.Count - 1; x++)
                    {
                        if (dt.Rows[i]["RoleID"].ToString().Replace("{", "").Replace("}", "") == lstRoles.Items[x].Value)
                        {
                            lstRoles.Items[x].Selected = true;
                        }
                    }
                }
            }
            if (DDLRoles.SelectedIndex < 1)
            { ClearRole(); }
            else
            {
                DataTable dt = new DataTable();
                u.GetRoleUsers(DDLRoles.SelectedValue, ref dt);
                if (dt.Rows.Count == 0)
                {
                    lstUsers.SelectedIndex = -1;
                    return;
                }
                for (int x = 0; x <= lstUsers.Items.Count - 1; x++)
                    lstUsers.Items[x].Selected = false;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    for (int x = 0; x <= lstUsers.Items.Count - 1; x++)
                    {
                        if (dt.Rows[i]["UserID"].ToString().Replace("{", "").Replace("}", "") == lstUsers.Items[x].Value)
                        {
                            lstUsers.Items[x].Selected = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void DDLUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        ReloadBoth();
    }
    protected void DDLRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        ReloadBoth();
    }
    protected void btnClearUser_Click(object sender, EventArgs e)
    {
        ClearUser();
    }
    protected void btnClearRole_Click(object sender, EventArgs e)
    {
        ClearRole();
    }
    protected void btnSaveUser_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsgUser.Text = "&nbsp;";
            if (DDLUsers.SelectedIndex > 0)
            {
                u.DeleteUserRoles(DDLUsers.SelectedValue);
                for (int i = 0; i <= lstRoles.Items.Count - 1; i++)
                {
                    if (lstRoles.Items[i].Selected)
                    { u.InsertUsersRoles(DDLUsers.SelectedValue, lstRoles.Items[i].Value); }
                }
                lblmsgUser.ForeColor = Color.Green;
                lblmsgUser.Text = "Roles have been assigned successfully";
                ReloadBoth();
            }
            else
            {
                lblmsgUser.ForeColor = Color.Red;
                lblmsgUser.Text = "Please select a user";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnSaveRole_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsgRole.Text = "&nbsp;";
            if (DDLRoles.SelectedIndex > 0)
            {
                u.DeleteRoleUsers(DDLRoles.SelectedValue);
                for (int i = 0; i <= lstUsers.Items.Count - 1; i++)
                {
                    if (lstUsers.Items[i].Selected)
                    { u.InsertUsersRoles(lstUsers.Items[i].Value, DDLRoles.SelectedValue); }
                }
                lblmsgRole.ForeColor = Color.Green;
                lblmsgRole.Text = "Users have been assigned successfully";
                ReloadBoth();
            }
            else
            {
                lblmsgRole.ForeColor = Color.Red;
                lblmsgRole.Text = "Please select a role";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}