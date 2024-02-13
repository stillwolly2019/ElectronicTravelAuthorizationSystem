  
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_RoleTripSchemas : AuthenticatedPageClass
{
    Business.Roles R = new Business.Roles();
    Business.TAWorkFlow W = new Business.TAWorkFlow();

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
            R.GetStaffCategories(ref dt);
            DDLRoles.DataSource = dt;
            DDLRoles.DataTextField = "RoleName";
            DDLRoles.DataValueField = "RoleID";
            DDLRoles.DataBind();
            DDLRoles.SelectedIndex = 0;
            FillGrid();
            dt = new DataTable();
            W.GetAllTripSchemas(ref dt);
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
            W.GetRoleTripSchemas(DDLRoles.SelectedValue, ref dt);
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
    protected void GVRolesPermissions_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName == "GrantPermission")
            {
                string LookupsID = e.CommandArgument.ToString();
                W.RoleAccessToTripSchemaToggle(DDLRoles.SelectedValue, LookupsID);
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
                ibE = (LinkButton)e.Row.FindControl("ibAllow");
                ibE.Enabled = this.CanEdit;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibConfirm = (LinkButton)e.Row.FindControl("ibAllow");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "Allow"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Allow")) == 0)
                {
                    ibConfirm.CssClass = "fa fa-times";
                    ibConfirm.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibConfirm.CssClass = "fa fa-check";
                }
                ibConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}



