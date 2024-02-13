      
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_TravelLocations : AuthenticatedPageClass
{
    //RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    Business.Locations L = new Business.Locations();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVCountrys.PreRender += new EventHandler(GVCountrys_PreRender);
        GVCity.PreRender += new EventHandler(GVCity_PreRender);
        PanelMessage.Visible = false;

        if (!IsPostBack)
        {
            //LnkAddNewItem.Enabled = this.CanAdd;
            FillGrid();
        }
    }
    void GVCountrys_PreRender(object sender, EventArgs e)
    {
        if (GVCountrys.Rows.Count > 0)
        {
            GVCountrys.UseAccessibleHeader = true;
            GVCountrys.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void GVCity_PreRender(object sender, EventArgs e)
    {
        if (GVCity.Rows.Count > 0)
        {
            GVCity.UseAccessibleHeader = true;
            GVCity.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    //void Clear()
    //{
    //    txtName.Text = "";
    //    PanelMessagePopUp.Visible = false;
    //    lblmsgPopUp.Text = "";
    //    GVCountrys.SelectedIndex = -1;
    //}
    void FillGrid()
    {
        DataTable dt = new DataTable();
        L.GetAllCountries(ref dt);
        lblGVCount.Text = dt.Rows.Count.ToString();
        GVCountrys.DataSource = dt;
        GVCountrys.DataBind();
    }
    protected void GVCountrys_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ManageCity")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openCityModal('" + GVCountrys.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["CountryCode"].ToString() + "');", true);
            ClearCityPopUp();
            GVCountrys.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            GVCountrys.SelectedIndex = -1;
            hdnCountryCode.Value = "";
            hdnCountryCode.Value = GVCountrys.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["CountryCode"].ToString();
            FillCityGrid();
        }
    }
    protected void GVCountrys_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibAdd = (LinkButton)e.Row.FindControl("ibAdd");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liCountryName");
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
    //protected void LnkAddNewItem_Click(object sender, EventArgs e)
    //{
    //    Clear();
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    //}

    #region Cities
    private void ClearCityPopUp()
    {
        lblAmsg.Text = "";
        PanelAmsg.Visible = false;
        txtCityCode.Text = "";
        txtCityName.Text = "";
        GVCity.SelectedIndex = -1;
    }
    void FillCityGrid()
    {
        DataTable dt = new DataTable();
        L.GetAllCitiesByCountryCode(hdnCountryCode.Value, ref dt);
        lblGVCityCount.Text = dt.Rows.Count.ToString();
        GVCity.DataSource = dt;
        GVCity.DataBind();
    }
    protected void GVCity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditItem")
            {
                ClearCityPopUp();
                GVCity.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                txtCityCode.Text = GVCity.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["CityCode"].ToString();
                txtCityName.Text = GVCity.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["CityName"].ToString();
                hdnCountryCode.Value = GVCity.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["CountryCode"].ToString();
            }
            else if (e.CommandName == "DeleteItem")
            {
                DataTable dtCount = new DataTable();
                L.GetTravelItineraryByLocationID(GVCity.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["CityID"].ToString(), ref dtCount);
                if (dtCount.Rows.Count > 0)
                {
                    PanelAmsg.Visible = true;
                    PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                    lblAmsg.Text = "Item can't be deleted, there are Travel Itineraries with this Location";
                    lblAmsg.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    L.DeleteCity(GVCity.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["CityID"].ToString());
                    PanelAmsg.Visible = true;
                    PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                    lblAmsg.Text = "Location has been deleted successfully";
                    lblAmsg.ForeColor = System.Drawing.Color.Green;
                    FillCityGrid();
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVCity_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVCity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibAdd = (LinkButton)e.Row.FindControl("ibAdd");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liCityName");
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
    protected void LnkCityave_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;
            if (GVCity.SelectedIndex == -1)
            {
                result = L.InsertUpdateCity("",txtCityCode.Text.Trim(),txtCityName.Text.Trim(), hdnCountryCode.Value);
            }
            else
            {
                string CityID = GVCity.DataKeys[GVCity.SelectedIndex].Values["CityID"].ToString();
                result = L.InsertUpdateCity(CityID,txtCityCode.Text.Trim(),txtCityName.Text.Trim(),hdnCountryCode.Value);
            }

            ClearCityPopUp();
            FillCityGrid();
            PanelAmsg.Visible = true;

            if (result == -1)
            {
                PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Red;
                lblAmsg.Text = "A City with same name under this Country already exists";
            }
            else
            {
                PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Green;
                lblAmsg.Text = "City has been added successfully";
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
