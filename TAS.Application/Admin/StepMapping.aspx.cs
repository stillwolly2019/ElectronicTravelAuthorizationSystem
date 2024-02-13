  
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_StepMapping : AuthenticatedPageClass
{
    Business.TravelAuthorization p = new Business.TravelAuthorization();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVStepMaps.PreRender += new EventHandler(GVStepMaps_PreRender);
        if (!IsPostBack)
        {
            LnkAddNewStepMap.Enabled = this.CanAdd;
            FillGrid();
            FillDDL();
        }
    }
    void GVStepMaps_PreRender(object sender, EventArgs e)
    {
        if (GVStepMaps.Rows.Count > 0)
        {
            GVStepMaps.UseAccessibleHeader = true;
            GVStepMaps.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillDDL()
    {
        try
        {
            DataTable dts = new DataTable();
            DataTable dtstp = new DataTable();

            p.GetAllStatuses(ref dts);
            DDLStatusCodes.DataSource = dts;
            DDLStatusCodes.DataTextField = "Description";
            DDLStatusCodes.DataValueField = "LookupsID";
            DDLStatusCodes.DataBind();
            DDLStatusCodes.SelectedIndex = 0;

            p.GetAllWorkFlowSteps(ref dtstp);
            DDLWorkFlow.DataSource = dtstp;
            DDLWorkFlow.DataTextField = "StepName";
            DDLWorkFlow.DataValueField = "StepID";
            DDLWorkFlow.DataBind();
            DDLWorkFlow.SelectedIndex = 0;

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
            p.GetAllStepMaps(ref dt);
            GVStepMaps.DataSource = dt;
            GVStepMaps.DataBind();

            lblCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        GVStepMaps.SelectedIndex = -1;
        lblmsg.Text = "";
        PanelMessage.Visible = false;
        PanelMessageAddNewStepMap.Visible = false;
        lblmsgAddNewStepMap.Text = "";
        DDLWorkFlow.SelectedIndex = -1;
        DDLStatusCodes.SelectedIndex = -1;
    }
    protected void GVStepMaps_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVStepMaps.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditItem")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewStepMap.Visible = false;
                lblmsgAddNewStepMap.Text = "";

                DDLStatusCodes.SelectedValue = GVStepMaps.DataKeys[Convert.ToInt16(e.CommandArgument.ToString())].Values["StatusID"].ToString();
                DDLWorkFlow.SelectedValue = GVStepMaps.DataKeys[Convert.ToInt16(e.CommandArgument.ToString())].Values["StepID"].ToString();
                txtActionNeeded.Text = GVStepMaps.DataKeys[Convert.ToInt16(e.CommandArgument.ToString())].Values["ActionNeeded"].ToString();
                DDLStatusCodes.Enabled = false;
            }
            else if (e.CommandName == "deleteItem")
            {
                DDLStatusCodes.Enabled = false;
                DDLWorkFlow.Enabled = false;
                p.DeleteStepMap(GVStepMaps.DataKeys[GVStepMaps.SelectedIndex].Values["StatusID"].ToString());
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
    protected void GVStepMaps_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVStepMaps_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void LnkAddNewStepMap_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (GVStepMaps.SelectedIndex == -1)
            {
                p.InsertUpdateStepMap(DDLStatusCodes.SelectedValue,Convert.ToInt32(DDLWorkFlow.SelectedValue),txtActionNeeded.Text);
                FillGrid();
                ClearAll();
                PanelMessageAddNewStepMap.Visible = true;
                PanelMessageAddNewStepMap.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewStepMap.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewStepMap.Text = "Step Mapping has been added successfully";
            }
            else
            {
                p.InsertUpdateStepMap(DDLStatusCodes.SelectedValue,Convert.ToInt32(DDLWorkFlow.SelectedValue), txtActionNeeded.Text);
                FillGrid();
                ClearAll();
                PanelMessageAddNewStepMap.Visible = true;
                PanelMessageAddNewStepMap.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewStepMap.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewStepMap.Text = "Step Mapping has been updated successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}