using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_TAWorkFlowSteps : AuthenticatedPageClass
{
    Business.TAWorkFlow p = new Business.TAWorkFlow();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVSteps.PreRender += new EventHandler(GVSteps_PreRender);
        if (!IsPostBack)
        {
            LnkAddNewStep.Enabled = this.CanAdd;
            //FillDLL();
            FillGrid();
        }
    }
    void GVSteps_PreRender(object sender, EventArgs e)
    {
        if (GVSteps.Rows.Count > 0)
        {
            GVSteps.UseAccessibleHeader = true;
            GVSteps.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            p.GetAllSteps(ref dt);
            GVSteps.DataSource = dt;
            GVSteps.DataBind();
            lblCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    //void FillDLL()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        p.GetAllStatuses(ref dt);
    //        DDLStatuses.DataSource = dt;
    //        DDLStatuses.DataTextField = "Description";
    //        DDLStatuses.DataValueField = "LookupsID";
    //        DDLStatuses.DataBind();
    //        DDLStatuses.SelectedIndex = 0;
    //    }
    //    catch (Exception ex)
    //    {
    //        IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
    //    }
    //}
    void ClearAll()
    {
        GVSteps.SelectedIndex = -1;
        lblmsg.Text = "";
        PanelMessage.Visible = false;
        PanelMessageAddNewStep.Visible = false;
        lblmsgAddNewStep.Text = "";
        txtStepID.Text = "";
        txtStepName.Text = "";
        //DDLStatuses.SelectedIndex=0;
        //txtStepName.Text = "";
        //txtStatusDescription.Text = "";
        //txtNextStep.Text = "";
        //txtActionNeeded.Text = "";
    }
    protected void GVSteps_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVSteps.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditItem")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewStep.Visible = false;
                lblmsgAddNewStep.Text = "";

                txtStepID.Text = GVSteps.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                txtStepName.Text = GVSteps.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                //DDLStatuses.SelectedValue = GVSteps.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[2].Text;
                //txtStatusDescription.Text = GVSteps.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[3].Text;
                //txtNextStep.Text = GVSteps.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[4].Text;
                //txtActionNeeded.Text = GVSteps.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[5].Text;
            }
            else if (e.CommandName == "deleteItem")
            {
                p.DeleteSteps(GVSteps.DataKeys[GVSteps.SelectedIndex].Values["StepID"].ToString());
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
    protected void GVSteps_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVSteps_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void LnkAddNewStep_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        string i = string.Empty;
        try
        {
            if (GVSteps.SelectedIndex == -1)
            {
                int StepID = txtStepID.Text == "" ? 0 : Convert.ToInt32(txtStepID.Text);
                if (StepID == 0)
                {
                    PanelMessageAddNewStep.Visible = true;
                    PanelMessageAddNewStep.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewStep.ForeColor = System.Drawing.Color.Red;
                    lblmsgAddNewStep.Text = "Invalid Step Number";
                    return;
                }
                else
                {
                    p.InsertUpdateSteps(StepID, txtStepName.Text);//,DDLStatuses.SelectedValue,txtStatusDescription.Text, NextStep, txtActionNeeded.Text);
                    FillGrid();
                    ClearAll();
                    PanelMessageAddNewStep.Visible = true;
                    PanelMessageAddNewStep.CssClass = "alert alert-success alert-dismissable";
                    lblmsgAddNewStep.ForeColor = System.Drawing.Color.Green;
                    lblmsgAddNewStep.Text = "Step has been added successfully";
                }
                
            }
            else
            {
                int StepID = txtStepID.Text == "" ? 0 : Convert.ToInt32(txtStepID.Text);
                p.InsertUpdateSteps(Convert.ToInt32(GVSteps.DataKeys[GVSteps.SelectedIndex].Values["StepID"].ToString()), txtStepName.Text);//, DDLStatuses.SelectedValue, txtStatusDescription.Text, NextStep, txtActionNeeded.Text);
                FillGrid();
                ClearAll();
                PanelMessageAddNewStep.Visible = true;
                PanelMessageAddNewStep.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewStep.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewStep.Text = "Step has been updated successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}