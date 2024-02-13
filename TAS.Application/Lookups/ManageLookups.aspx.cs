using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;

public partial class Lookups_ManageLookups : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVLookups.PreRender += new EventHandler(GVLookups_PreRender);
        if (!IsPostBack)
        {
            try
            {
                LnkSave.Enabled = this.CanAdd;
                FillDlls();
                FillGrid();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }

    void FillDlls()
    {
        try
        {
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds);
            ddlLookupsGroupName.DataSource = ds.Tables[0];
            ddlLookupsGroupName.DataBind();

            ddlLookupsSubGroup.DataSource = ds.Tables[0];
            ddlLookupsSubGroup.DataBind();
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void GVLookups_PreRender(object sender, EventArgs e)
    {
        if (GVLookups.Rows.Count > 0)
        {
            GVLookups.UseAccessibleHeader = true;
            GVLookups.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            l.GetAllLookups(ref dt);
            GVLookups.DataSource = dt;
            GVLookups.DataBind();

            lblCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        lblmsg.Text = "";
        PanelMessage.Visible = false;
        PanelMessageAddNewLookup.Visible = false;
        lblmsgAddNewLookup.Text = "";
        ddlLookupsGroupName.SelectedIndex = -1;
        ddlLookupsGroupName.SelectedIndex = -1;
        ddlLookupsSubGroup.SelectedIndex = -1;
        txtCode.Text = "";
        txtDescription.Text = "";
        txtLongDescription.Text = "";
        GVLookups.SelectedIndex = -1;
    }
    protected void GVLookups_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditLookup")
            {
                ClearAll();
                GVLookups.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                // Prepare for update
                ddlLookupsGroupName.SelectedValue = GVLookups.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["LookupGroupID"].ToString();

                if (GVLookups.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["SubGroupID"].ToString() != "")
                {
                    ddlLookupsSubGroup.SelectedValue = GVLookups.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["SubGroupID"].ToString();
                }

                txtCode.Text = GVLookups.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["Code"].ToString();
                txtDescription.Text = GVLookups.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["Description"].ToString();
                txtLongDescription.Text = GVLookups.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["LongDescription"].ToString();
                // END
            }
            if (e.CommandName == "deleteItem")
            {
                l.DeleteLookups(GVLookups.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["LookupsID"].ToString());
                FillGrid();
                ClearAll();
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Lookup has been deleted successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVLookups_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVLookups_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("ibEdit");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                Label lblDescription = (Label)e.Row.FindControl("lblDescription");

                ibEdit.Visible = this.CanEdit;
                ibDelete.Visible = this.CanDelete;

                if (lblDescription.Text.Length > 65)
                {
                    lblDescription.Text = lblDescription.Text.Substring(0, 65) + "...";
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void LnkAddNewLookup_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (GVLookups.SelectedIndex == -1)
            {
                DataTable dt = new DataTable();
                l.CheckDuplicateLookups(Guid.Empty.ToString(),ddlLookupsGroupName.SelectedValue,ddlLookupsSubGroup.SelectedValue, txtDescription.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    PanelMessageAddNewLookup.Visible = true;
                    PanelMessageAddNewLookup.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewLookup.ForeColor = Color.Red;
                    lblmsgAddNewLookup.Text = "This lookup is already added, please choose different name";
                    return;
                }

                l.InsertUpdateLookups("",ddlLookupsGroupName.SelectedValue,ddlLookupsSubGroup.SelectedValue, txtCode.Text.Trim(), txtDescription.Text.Trim(), txtLongDescription.Text.Trim());
                FillGrid();
                FillDlls();
                ClearAll();
                PanelMessageAddNewLookup.Visible = true;
                PanelMessageAddNewLookup.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewLookup.ForeColor = Color.Green;
                lblmsgAddNewLookup.Text = "Lookup has been added successfully";
            }
            else
            {
                l.InsertUpdateLookups(GVLookups.DataKeys[GVLookups.SelectedIndex].Values["LookupsID"].ToString(), ddlLookupsGroupName.SelectedValue, ddlLookupsSubGroup.SelectedValue, txtCode.Text.Trim(), txtDescription.Text.Trim(), txtLongDescription.Text.Trim());
                FillGrid();
                ClearAll();
                PanelMessageAddNewLookup.Visible = true;
                PanelMessageAddNewLookup.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewLookup.ForeColor = Color.Green;
                lblmsgAddNewLookup.Text = "Lookup has been updated successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}