using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_Roles : AuthenticatedPageClass
{
    Business.Roles p = new Business.Roles();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVRoles.PreRender += new EventHandler(GVRoles_PreRender);
        if (!IsPostBack)
        {
            LnkAddNewRole.Enabled = this.CanAdd;
            FillGrid();
        }
    }
    void GVRoles_PreRender(object sender, EventArgs e)
    {
        if (GVRoles.Rows.Count > 0)
        {
            GVRoles.UseAccessibleHeader = true;
            GVRoles.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            p.GetAllRoles(ref dt);
            GVRoles.DataSource = dt;
            GVRoles.DataBind();
            lblCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        GVRoles.SelectedIndex = -1;
        lblmsg.Text = "";
        PanelMessage.Visible = false;
        PanelMessageAddNewRole.Visible = false;
        lblmsgAddNewRole.Text = "";
        txtRoleName.Text = "";
        txtUniqueFieldName.Text = "";
        CheckIsAdmin.Checked = false;
        CheckIsFinance.Checked = false;
    }
    protected void GVRoles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVRoles.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditItem")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewRole.Visible = false;
                lblmsgAddNewRole.Text = "";

                txtRoleName.Text = GVRoles.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                txtUniqueFieldName.Text = GVRoles.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                CheckIsAdmin.Checked = Convert.ToBoolean(GVRoles.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsAdmin"]);
                CheckIsFinance.Checked = Convert.ToBoolean(GVRoles.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsFinance"]);
                CheckIsApprover.Checked = Convert.ToBoolean(GVRoles.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsApprover"]);
                CheckIsRadioOperator.Checked = Convert.ToBoolean(GVRoles.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsRadioOperator"]);
            }
            else if (e.CommandName == "deleteItem")
            {
                p.DeleteRoles(GVRoles.DataKeys[GVRoles.SelectedIndex].Values["RoleID"].ToString());
                ClearAll();
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                lblmsg.Text = "Item has been deleted successfully";
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVRoles_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void LnkAddNewRole_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (GVRoles.SelectedIndex == -1)
            {
                p.InsertUpdateRoles("", txtRoleName.Text,txtUniqueFieldName.Text);
                FillGrid();
                ClearAll();
                PanelMessageAddNewRole.Visible = true;
                PanelMessageAddNewRole.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewRole.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewRole.Text = "Role has been added successfully";
            }
            else
            {
                p.InsertUpdateRoles(GVRoles.DataKeys[GVRoles.SelectedIndex].Values["RoleID"].ToString(), txtRoleName.Text, txtUniqueFieldName.Text);
                FillGrid();
                ClearAll();
                PanelMessageAddNewRole.Visible = true;
                PanelMessageAddNewRole.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewRole.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewRole.Text = "Role has been updated successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}