using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_RolesPermissions : AuthenticatedPageClass
{
    Business.Roles r = new Business.Roles();
    Business.Pages p = new Business.Pages();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVRolesPermissions.PreRender += new EventHandler(GVRolesPermissions_PreRender);
        if (!IsPostBack)
        {
            FillLookups();
        }
    }
    void GVRolesPermissions_PreRender(object sender, EventArgs e)
    {
        if (GVRolesPermissions.Rows.Count > 0)
        {
            GVRolesPermissions.UseAccessibleHeader = true;
            GVRolesPermissions.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillLookups()
    {
        try
        {
            DataTable dt = new DataTable();
            r.GetAllRoles(ref dt);
            DDLRoles.DataSource = dt;
            DDLRoles.DataTextField = "RoleName";
            DDLRoles.DataValueField = "RoleID";
            DDLRoles.DataBind();
            DDLRoles.SelectedIndex = 0;
            FillGrid();
            dt = new DataTable();
            p.GetAllPages(ref dt);
            DataView dv = dt.DefaultView;
            dv.RowFilter = "ParentID = '00000000-0000-0000-0000-000000000000'";
            DDLParents.DataSource = dv;
            DDLParents.DataTextField = "PageName";
            DDLParents.DataValueField = "PageID";
            DDLParents.DataBind();
            DDLParents.Items.Insert(0, "-- All --");
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            string ThePage;
            if (DDLParents.SelectedIndex < 1)
                ThePage = Guid.Empty.ToString();
            else
                ThePage = DDLParents.SelectedValue;
            r.GetRolePages(DDLRoles.SelectedValue, ThePage, ref dt);
            GVRolesPermissions.DataSource = dt;
            GVRolesPermissions.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void DDLRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void DDLParents_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void GVRolesPermissions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //GVRolesPermissions.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            string ID = e.CommandArgument.ToString();

            if (e.CommandName == "PermissionRead")
            {
                r.PermissionsToggle(DDLRoles.SelectedValue, ID, true, false, false, false, false);
                FillGrid();
            }
            else if (e.CommandName == "PermissionEdit")
            {
                r.PermissionsToggle(DDLRoles.SelectedValue, ID, false, true, false, false, false);
                FillGrid();
            }
            else if (e.CommandName == "PermissionAdd")
            {
                r.PermissionsToggle(DDLRoles.SelectedValue, ID, false, false, true, false, false);
                FillGrid();
            }
            else if (e.CommandName == "PermissionDelete")
            {
                r.PermissionsToggle(DDLRoles.SelectedValue, ID, false, false, false, true, false);
                FillGrid();
            }
            else if (e.CommandName == "PermissionAmend")
            {
                r.PermissionsToggle(DDLRoles.SelectedValue, ID, false, false, false, false, true);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVRolesPermissions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVRolesPermissions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVRolesPermissions.EditIndex == -1)
            {
                LinkButton ibE = new LinkButton();
                ibE = (LinkButton)e.Row.FindControl("ibEdit");
                ibE.Enabled = this.CanEdit;
                LinkButton ibD = new LinkButton();
                ibD = (LinkButton)e.Row.FindControl("ibDelete");
                ibD.Enabled = this.CanDelete;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibR = (LinkButton)e.Row.FindControl("ibRead");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "Read"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Read")) == 0)
                {
                    ibR.CssClass = "fa fa-times";
                    ibR.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibR.CssClass = "fa fa-check";
                }
                ibR.CommandArgument = DataBinder.Eval(e.Row.DataItem, "PageID").ToString();

                LinkButton ibE = (LinkButton)e.Row.FindControl("ibEdit");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "Edit"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Edit")) == 0)
                {
                    ibE.CssClass = "fa fa-times";
                    ibE.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibE.CssClass = "fa fa-check";
                }
                ibE.CommandArgument = DataBinder.Eval(e.Row.DataItem, "PageID").ToString();

                LinkButton ibA = (LinkButton)e.Row.FindControl("ibAdd");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "Add"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Add")) == 0)
                {
                    ibA.CssClass = "fa fa-times";
                    ibA.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibA.CssClass = "fa fa-check";
                }
                ibA.CommandArgument = DataBinder.Eval(e.Row.DataItem, "PageID").ToString();

                LinkButton ibD = (LinkButton)e.Row.FindControl("ibDelete");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "Delete"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Delete")) == 0)
                {
                    ibD.CssClass = "fa fa-times";
                    ibD.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibD.CssClass = "fa fa-check";
                }
                ibD.CommandArgument = DataBinder.Eval(e.Row.DataItem, "PageID").ToString();
                ibD.Attributes.Clear();

                LinkButton ibConfirm = (LinkButton)e.Row.FindControl("ibAmend");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "Amend"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Amend")) == 0)
                {
                    ibConfirm.CssClass = "fa fa-times";
                    ibConfirm.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibConfirm.CssClass = "fa fa-check";
                }
                ibConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "PageID").ToString();
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}