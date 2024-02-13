
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_AccomodationLocations : AuthenticatedPageClass
{
    //RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    Business.TravelAuthorization T = new Business.TravelAuthorization();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVAccomodations.PreRender += new EventHandler(GVAccomodations_PreRender);
        PanelMessage.Visible = false;

        if (!IsPostBack)
        {
            LnkAddNewItem.Enabled = this.CanAdd;
            FillHeader();
            FillGrid();
        }
    }
    void GVAccomodations_PreRender(object sender, EventArgs e)
    {
        if (GVAccomodations.Rows.Count > 0)
        {
            GVAccomodations.UseAccessibleHeader = true;
            GVAccomodations.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    
    private void FillHeader()
    {
        try
        {
            DataTable dt = new DataTable();
            T.GetAllLocations(ref dt);
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
        GVAccomodations.SelectedIndex = -1;
    }
    void FillGrid()
    {
        DataTable dt = new DataTable();
        T.GetAllAccomodationsByLocationID(ddlLocationsName.SelectedValue, ref dt);
        lblGVCount.Text = dt.Rows.Count.ToString();
        GVAccomodations.DataSource = dt;
        GVAccomodations.DataBind();
    }
    protected void GVAccomodations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditItem")
        {
            Clear();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            GVAccomodations.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            txtName.Text = GVAccomodations.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["AccomodationName"].ToString();
        }
        else if (e.CommandName == "DeleteItem")
        {
            DataTable dtCount = new DataTable();
            T.GetTravelAuthorizationsByAccomodation(GVAccomodations.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["AccomodationID"].ToString(), ref dtCount);
            if (dtCount.Rows.Count > 0)
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.Text = "Item can't be deleted, there are TAs with this Accomodation";
                lblmsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                T.DeleteAccomodation(GVAccomodations.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["AccomodationID"].ToString());
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.Text = "Accomodation has been deleted successfully";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                FillGrid();
            }
        }
    }
    protected void GVAccomodations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibAdd = (LinkButton)e.Row.FindControl("ibAdd");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liAccomodationName");
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
    protected void GVAccomodations_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
        if (GVAccomodations.SelectedIndex == -1)
        {
            result = T.InsertAccomodation("", txtName.Text.Trim(), ddlLocationsName.SelectedValue);
        }
        else
        {
            string AccomodationID = GVAccomodations.DataKeys[GVAccomodations.SelectedIndex].Values["AccomodationID"].ToString();
            result = T.InsertAccomodation(AccomodationID, txtName.Text.Trim(), ddlLocationsName.SelectedValue);
        }

        Clear();
        FillGrid();
        PanelMessagePopUp.Visible = true;

        if (result == -1)
        {
            PanelMessagePopUp.CssClass = "alert alert-danger alert-dismissable";
            lblmsgPopUp.ForeColor = System.Drawing.Color.Red;
            lblmsgPopUp.Text = "Accomodation with the same name already exists";
        }
        else
        {
            PanelMessagePopUp.CssClass = "alert alert-success alert-dismissable";
            lblmsgPopUp.ForeColor = System.Drawing.Color.Green;
            lblmsgPopUp.Text = "Accomodation has been added successfully";
        }

    }

    protected void ddlLocationsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}