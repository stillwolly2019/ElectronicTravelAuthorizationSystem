using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
public partial class Admin_Locations : AuthenticatedPageClass
{
    Business.Locations baLocations = new Business.Locations();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVLocations.PreRender += new EventHandler(GVLocations_PreRender);
        if (!IsPostBack)
        {
            LnkAddNewLocation.Enabled = this.CanAdd;
            FillDDL();
            FillGrid();
        }
    }
    void GVLocations_PreRender(object sender, EventArgs e)
    {
        if (GVLocations.Rows.Count > 0)
        {
            GVLocations.UseAccessibleHeader = true;
            GVLocations.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            baLocations.GetAllLocations(ref dt);
            GVLocations.DataSource = dt;
            GVLocations.DataBind();
            lblGVLocationsCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillDDL()
    {
        try
        {
            DataTable dt = new DataTable();
            baLocations.GetAllMissions(ref dt);
            DDLMissions.DataSource = dt;
            DDLMissions.DataTextField = "Name";
            DDLMissions.DataValueField = "MissionID";
            DDLMissions.DataBind();

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    void ClearAll()
    {
        GVLocations.SelectedIndex = -1;
        PanelMessage.Visible = false;
        lblmsg.Text = "";
        PanelMessageAddNewLocation.Visible = false;
        lblmsgAddNewLocation.Text = "";
        txtLocationName.Text = "";
        txtLocationCode.Text = "";
        txtBusinessArea.Text = "";
        DDLMissions.SelectedIndex = -1;
        rdYesAC.Checked = false;
        rdNoAC.Checked = true;
        rdYesRC.Checked = false;
        rdNoRC.Checked = true;
    }
    protected void GVLocations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVLocations.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditItem")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewLocation.Visible = false;
                lblmsgAddNewLocation.Text = "";

                // Prepare for update
                if (GVLocations.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["MissionID"].ToString() != "00000000-0000-0000-0000-000000000000")
                { DDLMissions.SelectedValue = GVLocations.DataKeys[Convert.ToInt16(e.CommandArgument.ToString())].Values["MissionID"].ToString(); }
                else { DDLMissions.SelectedIndex = -1; }
                txtLocationCode.Text = GVLocations.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                txtLocationName.Text = GVLocations.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                txtBusinessArea.Text = GVLocations.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[2].Text;
                CheckBox chkRC;
                CheckBox chkAC;
                chkRC = (CheckBox)GVLocations.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("chkRadioCheck");
                chkAC = (CheckBox)GVLocations.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("chkAllowAccess");
                if (chkRC.Checked) { rdYesRC.Checked = true; rdNoRC.Checked = false; } else { rdNoRC.Checked = true; rdYesRC.Checked = false; }
                if (chkAC.Checked) { rdYesAC.Checked = true; rdNoAC.Checked = false; } else { rdNoAC.Checked = true; rdYesAC.Checked = false; }
                //End

            }
            if (e.CommandName == "deleteItem")
            {
                baLocations.DeleteLocations(GVLocations.DataKeys[GVLocations.SelectedIndex].Values["LocationID"].ToString());
                ClearAll();
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                lblmsg.Text = "Location has been deleted successfully";
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVLocations_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVLocations_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void LnkAddNewLocation_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            string TheID;
            if (DDLMissions.SelectedIndex != 0)
                TheID = new Guid(DDLMissions.SelectedValue).ToString();
            else
                TheID = Guid.Empty.ToString();
            if (GVLocations.SelectedIndex == -1)
            {
                baLocations.InsertUpdateLocations("", txtLocationCode.Text, txtLocationName.Text,txtBusinessArea.Text,DDLMissions.SelectedValue,rdYesRC.Checked,rdYesAC.Checked);
                FillGrid();
                ClearAll();
                PanelMessageAddNewLocation.Visible = true;
                PanelMessageAddNewLocation.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewLocation.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewLocation.Text = "Location has been added successfully";
            }
            else
            {
                baLocations.InsertUpdateLocations(GVLocations.DataKeys[GVLocations.SelectedIndex].Values["LocationID"].ToString(), txtLocationCode.Text, txtLocationName.Text, txtBusinessArea.Text, DDLMissions.SelectedValue, rdYesRC.Checked, rdYesAC.Checked);
                FillGrid();
                ClearAll();
                PanelMessageAddNewLocation.Visible = true;
                PanelMessageAddNewLocation.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewLocation.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewLocation.Text = "Location has been updated successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}