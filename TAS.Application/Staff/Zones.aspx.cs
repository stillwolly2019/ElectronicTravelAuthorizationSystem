  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Staff_Zones : AuthenticatedPageClass
{
    HttpContext context = HttpContext.Current;
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVZones.PreRender += new EventHandler(GVZones_PreRender);
        GVResidence.PreRender += new EventHandler(GVResidence_PreRender);
        PanelMessage.Visible = false;

        if (!IsPostBack)
        {
            LnkAddNewItem.Enabled = this.CanAdd;
            FillHeader();
            FillGrid();
        }
    }
    void GVZones_PreRender(object sender, EventArgs e)
    {
        if (GVZones.Rows.Count > 0)
        {
            GVZones.UseAccessibleHeader = true;
            GVZones.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void GVResidence_PreRender(object sender, EventArgs e)
    {
        if (GVResidence.Rows.Count > 0)
        {
            GVResidence.UseAccessibleHeader = true;
            GVResidence.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    private void FillHeader()
    {
        try
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            DataTable dt = new DataTable();
            R.GetAllLocations(ref dt);
            //R.GetAllLocations(ref dt,ui.User_Id.ToString());
            ddlLocationsName.DataSource = dt;
            ddlLocationsName.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    void Clear()
    {
        txtName.Text = "";
        PanelMessagePopUp.Visible = false;
        lblmsgPopUp.Text = "";
        GVZones.SelectedIndex = -1;
    }
    void FillGrid()
    {
        DataTable dt = new DataTable();
        R.GetAllZonesByLocationID(ddlLocationsName.SelectedValue, ref dt);
        lblGVCount.Text = dt.Rows.Count.ToString();
        GVZones.DataSource = dt;
        GVZones.DataBind();
    }
    protected void GVZones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ManageResidence")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openResidenceModal('" + GVZones.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["ZoneName"].ToString() + "');", true);
            ClearResidencePopUp();
            GVZones.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            GVZones.SelectedIndex = -1;
            hdnZoneID.Value = "";
            hdnZoneID.Value = GVZones.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["ZoneID"].ToString();
            hdnLocationID.Value = GVZones.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["LocationID"].ToString();
            FillResidenceGrid();
        }
        if (e.CommandName == "EditItem")
        {
            Clear();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            GVZones.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            txtName.Text = GVZones.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["ZoneName"].ToString();
        }
        else if (e.CommandName == "DeleteItem")
        {
            DataTable dtCount = new DataTable();
            R.GetStaffNumberUnderZoneResidence(GVZones.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["ZoneID"].ToString(), "00000000-0000-0000-0000-000000000000", ref dtCount);
            if (dtCount.Rows.Count > 0)
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.Text = "Item can't be deleted, there are residencial areas under this zone";
                lblmsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                R.DeleteZone(GVZones.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["ZoneID"].ToString());
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.Text = "Zone has been deleted successfully";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                FillGrid();
            }
        }
    }
    protected void GVZones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibAdd = (LinkButton)e.Row.FindControl("ibAdd");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liZoneName");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibAdd.Visible = this.CanAdd;
                ibEdit.Enabled = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVZones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void LnkAddNewItem_Click(object sender, EventArgs e)
    {
        Clear();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        int result = 0;
        if (GVZones.SelectedIndex == -1)
        {
            result = R.InsertZones("", txtName.Text.Trim(), ddlLocationsName.SelectedValue);
        }
        else
        {
            string ZoneID = GVZones.DataKeys[GVZones.SelectedIndex].Values["ZoneID"].ToString();
            result = R.InsertZones(ZoneID, txtName.Text.Trim(), ddlLocationsName.SelectedValue);
        }

        Clear();
        FillGrid();
        PanelMessagePopUp.Visible = true;

        if (result == -1)
        {
            PanelMessagePopUp.CssClass = "alert alert-danger alert-dismissable";
            lblmsgPopUp.ForeColor = System.Drawing.Color.Red;
            lblmsgPopUp.Text = "Zone with the same name already exists";
        }
        else
        {
            PanelMessagePopUp.CssClass = "alert alert-success alert-dismissable";
            lblmsgPopUp.ForeColor = System.Drawing.Color.Green;
            lblmsgPopUp.Text = "Zone has been added successfully";
        }

    }
    #region Residence
    private void ClearResidencePopUp()
    {
        lblAmsg.Text = "";
        PanelAmsg.Visible = false;
        txtResidenceName.Text = "";
        GVResidence.SelectedIndex = -1;
    }
    void FillResidenceGrid()
    {
        DataTable dt = new DataTable();
        R.GetAllResidencesbyZoneID(hdnZoneID.Value, ref dt);
        lblGVResidenceCount.Text = dt.Rows.Count.ToString();
        GVResidence.DataSource = dt;
        GVResidence.DataBind();
    }
    protected void GVResidence_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditItem")
            {
                ClearResidencePopUp();
                GVResidence.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                txtResidenceName.Text = GVResidence.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["ResidenceName"].ToString();
            }
            else if (e.CommandName == "DeleteItem")
            {
                //DataTable dtCount = new DataTable();
                //R.GetStaffNumberInResidence(GVDepartments.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DepartmentID"].ToString(), "00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", ref dtCount);
                //R.GetStaffNumberInResidence(GVResidence.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["ResideUnitID"]."00000000-0000-0000-0000-000000000000", ref dtCount);
                //if (dtCount.Rows.Count > 0)
                //{
                //    PanelAmsg.Visible = true;
                //    PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                //    lblAmsg.Text = "Item can't be deleted, there are staff members under this unit";
                //    lblAmsg.ForeColor = System.Drawing.Color.Red;
                //}
                //else
                //{
                //    u.DeleteResidence(GVResidence.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["ResidenceID"].ToString());
                //    PanelAmsg.Visible = true;
                //    PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                //    lblAmsg.Text = "Unit has been deleted successfully";
                //    lblAmsg.ForeColor = System.Drawing.Color.Green;
                //    FillResidenceGrid();
                //}
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVResidence_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVResidence_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibAdd = (LinkButton)e.Row.FindControl("ibAdd");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liResidenceName");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibAdd.Visible = this.CanAdd;
                ibEdit.Enabled = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void LnkResidenceave_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;
            if (GVResidence.SelectedIndex == -1)
            {
                result = R.InsertResidences("", hdnZoneID.Value, hdnLocationID.Value, txtResidenceName.Text.Trim());
            }
            else
            {
                string ResidenceID = GVResidence.DataKeys[GVResidence.SelectedIndex].Values["ResidenceID"].ToString();
                result = R.InsertResidences(ResidenceID, hdnZoneID.Value, hdnLocationID.Value, txtResidenceName.Text.Trim());
            }

            ClearResidencePopUp();
            FillResidenceGrid();
            PanelAmsg.Visible = true;

            if (result == -1)
            {
                PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Red;
                lblAmsg.Text = "A Residence with same name under this Zone already exists";
            }
            else
            {
                PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Green;
                lblAmsg.Text = "Residence has been added successfully";
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    #endregion

    protected void ddlLocationsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}