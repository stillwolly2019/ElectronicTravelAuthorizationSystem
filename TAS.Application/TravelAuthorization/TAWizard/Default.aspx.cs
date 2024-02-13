
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TravelAuthorization_TAWizard_Default : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization TA = new Business.TravelAuthorization();
    Business.Users u = new Business.Users();
    Business.Security Sec = new Business.Security();
    Globals g = new Globals();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GVMyTAs.PreRender += new EventHandler(GVMyTAs_PreRender);
            if (!IsPostBack)
            {
                FillGrid();
            }
        }
    }
    void GVMyTAs_PreRender(object sender, EventArgs e)
    {
        if (GVMyTAs.Rows.Count > 0)
        {
            GVMyTAs.UseAccessibleHeader = true;
            GVMyTAs.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid(string TANo = "")
    {
        try
        {
            DataTable dt = new DataTable();
            TA.GetDashboardItems(ref dt, TANo);
            lblGVTAsCount.Text = dt.Rows.Count.ToString();
            GVMyTAs.DataSource = dt;
            GVMyTAs.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVMyTAs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VTA")
        {
            Response.Redirect("~/Dashboard/Step1_TravelersInformation.aspx?TANO=" + Encrypt(GVMyTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString()) + "&&TAID=" + Encrypt(GVMyTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString()), false);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string TANo = string.Empty;
        if (!String.IsNullOrEmpty(txtTANo.Text))
        {
            TANo = txtTANo.Text.Trim();
        }
        FillGrid(TANo);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Step1_TravelersInformation.aspx?First=1", false);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

}