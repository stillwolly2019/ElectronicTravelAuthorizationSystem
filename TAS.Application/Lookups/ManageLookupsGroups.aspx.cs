using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;


public partial class Lookups_ManageLookupsGroups : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    protected void Page_Load(object sender, EventArgs e)
    {
        gvLookupsGroup.PreRender += new EventHandler(gvLookupsGroup_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
        }

        LnkSave.Visible = this.CanAdd;
    }
    void gvLookupsGroup_PreRender(object sender, EventArgs e)
    {
        if (gvLookupsGroup.Rows.Count > 0)
        {
            gvLookupsGroup.UseAccessibleHeader = true;
            gvLookupsGroup.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            l.GetAllLookupsGroups(ref dt);
            gvLookupsGroup.DataSource = dt;
            gvLookupsGroup.DataBind();

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
        lblmsgAddNewLookup.Text = "";
        PanelMessage.Visible = false;
        PanelMessageAddNewLookup.Visible = false;
        txtName.Text = "";


        gvLookupsGroup.SelectedIndex = -1;
        for (int i = 0; i <= gvLookupsGroup.Rows.Count - 1; i++)
        {
            gvLookupsGroup.Rows[i].BackColor = default(System.Drawing.Color);
            gvLookupsGroup.Rows[i].ForeColor = default(System.Drawing.Color);
        }
    }

    protected void gvLookupsGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditLookup")
            {
                ClearAll();
                gvLookupsGroup.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvLookupsGroup.SelectedRow.BackColor = Color.Green;
                gvLookupsGroup.SelectedRow.ForeColor = Color.White;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                txtName.Text = gvLookupsGroup.SelectedRow.Cells[0].Text;
            }
            if (e.CommandName == "deleteItem")
            {
                l.DeleteLookupsGroups(gvLookupsGroup.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["LookupGroupID"].ToString());
                FillGrid();
                ClearAll();
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Item has been deleted successfully";
            }
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvLookupsGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvLookupsGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (gvLookupsGroup.EditIndex == -1))
        {
            LinkButton ibE = new LinkButton();
            ibE = (LinkButton)e.Row.FindControl("ibEdit");
            ibE.Visible = this.CanEdit;
            LinkButton ibD = new LinkButton();
            ibD = (LinkButton)e.Row.FindControl("ibDelete");
            ibD.Visible = this.CanDelete;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvLookupsGroup.SelectedIndex == -1)
            {
                DataTable dt = new DataTable();
                l.CheckDuplicateLookupGroup(Guid.Empty.ToString(), txtName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    PanelMessageAddNewLookup.Visible = true;
                    PanelMessageAddNewLookup.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewLookup.ForeColor = Color.Red;
                    lblmsgAddNewLookup.Text = "This item is already added, please choose different name";
                    return;
                }

                l.InsertUpdateLookupsGroups("", txtName.Text.Trim());
                FillGrid();
                ClearAll();
                PanelMessageAddNewLookup.Visible = true;
                PanelMessageAddNewLookup.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewLookup.ForeColor = Color.Green;
                lblmsgAddNewLookup.Text = "Item has been added successfully";
            }
            else
            {
                DataTable dt = new DataTable();
                l.CheckDuplicateLookupGroup(gvLookupsGroup.DataKeys[gvLookupsGroup.SelectedIndex].Values["LookupGroupID"].ToString(), txtName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    PanelMessageAddNewLookup.Visible = true;
                    PanelMessageAddNewLookup.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewLookup.ForeColor = Color.Red;
                    lblmsgAddNewLookup.Text = "This item is already added, please choose different name";
                    return;
                }

                l.InsertUpdateLookupsGroups(gvLookupsGroup.DataKeys[gvLookupsGroup.SelectedIndex].Values["LookupGroupID"].ToString(), txtName.Text.Trim());
                FillGrid();
                ClearAll();
                PanelMessageAddNewLookup.Visible = true;
                PanelMessageAddNewLookup.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewLookup.ForeColor = Color.Green;
                lblmsgAddNewLookup.Text = "Item has been updated successfully";
            }
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void LnkAddNewLookup_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
}