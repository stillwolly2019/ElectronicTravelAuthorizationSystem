using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WebForms;

public partial class TravelAuthorization_MapAcomodationLocations : AuthenticatedPageClass
{
    Business.TravelAuthorization TA = new Business.TravelAuthorization();
    Globals g = new Globals();
    Business.Lookups l = new Business.Lookups();
    HttpContext context = HttpContext.Current;
    Business.Users u = new Business.Users();

    protected void Page_Load(object sender, EventArgs e)
    {
        Objects.User ui = (Objects.User)Session["userinfo"];
        GVMyTAs.PreRender += new EventHandler(GVMyTAs_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
            Clear();
        }
    }
    void Clear()
    {
        txtTANo.Text = "";
    }
    void GVMyTAs_PreRender(object sender, EventArgs e)
    {
        if (GVMyTAs.Rows.Count > 0)
        {
            GVMyTAs.UseAccessibleHeader = true;
            GVMyTAs.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            string TANo = "";
            if (txtTANo.Text == "") { TANo = ""; } else { TANo = txtTANo.Text.Trim(); }
            TA.GetTravelAuthorizationsForMapping(ref dt, TANo);
            lblGVTAsCount.Text = dt.Rows.Count.ToString();
            GVMyTAs.DataSource = dt;
            GVMyTAs.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillDDLs()
    {
        try
        {
            DataTable dt = new DataTable();
            u.GetActiveLocations(ref dt);
            DDLLocations.DataSource = dt;
            DDLLocations.DataBind();
            DDLLocations.SelectedValue = hdnAccommodationLocationID.Value;

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVMyTAs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVMyTAs.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GVMyTAs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "MapTA")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openRoleAccessModal('" + GVMyTAs.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["TravelersName"].ToString() + "');", true);
            ClearUserAccessesPopup();
            GVMyTAs.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            GVMyTAs.SelectedIndex = -1;
            hdnTANo.Value = GVMyTAs.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["TravelAuthorizationNumber"].ToString();
            hdnAccommodationLocationID.Value = GVMyTAs.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["AccommodationLocationID"].ToString();
            FillDDLs();
            FillTAInfoGrid();
        }
    }

    protected void GVMyTAs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void GVMyTAs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
        FillGrid();
    }

    protected void GVTravelAuthorization_RowDataBound(object sender, GridViewRowEventArgs e)
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

    void FillTAInfoGrid()
    {
        DataTable dt = new DataTable();
        TA.GetTAInfo(hdnTANo.Value, ref dt);
        lblGVTravelAuthorizationCount.Text = dt.Rows.Count.ToString();
        GVTravelAuthorization.DataSource = dt;
        GVTravelAuthorization.DataBind();
    }

    protected void GVTravelAuthorization_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void LnkTravelAuthorizationave_Click(object sender, EventArgs e)
    {
        try
        {
            string result = "";
            result =TA.MapTA(hdnTANo.Value, DDLLocations.SelectedValue);
            ClearUserAccessesPopup();
            FillTAInfoGrid();
            PanelAmsg.Visible = true;
            if (result=="1")
            {
                PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Green;
                lblAmsg.Text = "Accomodation Location mapped successfully";
            }
            else
            {
                PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Red;
                lblAmsg.Text = "OOps, Something went wrong. Failed to Transfer TA";
            }
           

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectHome()", true);
    }

    private void ClearUserAccessesPopup()
    {
        lblAmsg.Text = "";
        PanelAmsg.Visible = false;
        DDLLocations.SelectedIndex = -1;
        GVTravelAuthorization.SelectedIndex = -1;
    }



}