using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WebForms;

public partial class TravelAuthorization_Default2 : AuthenticatedPageClass
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
            if(ui!=null)
            {
            DataTable dt = new DataTable();
            TA.GetDelegationStatus(ref dt,ui.User_Id);
            if (dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool HasDelegated = row["HasDelegated"].ToString()=="1"?true:false;
                    bool HasBeenDelegated = row["HasBeenDelegated"].ToString()=="1"?true: false;
                    if (HasDelegated && !HasBeenDelegated)
                    {
                        btnAdd.Visible = false;
                    }
                    else
                    {
                        FillGrid();
                        Clear();
                    }

                }
            }
            }

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
            //Objects.User ui = (Objects.User)Session["userinfo"];
            DataTable dt = new DataTable();
            string TANo = "";
            if (txtTANo.Text == "") { TANo = ""; } else { TANo = txtTANo.Text.Trim(); }
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
    protected void GVMyTAs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVMyTAs.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void GVMyTAs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VTA")
        {
            Response.Redirect("~/TravelAuthorization/TravelAuthorizationFormWizard.aspx?TANO=" + Encrypt(GVMyTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString()) + "&&TAID=" + Encrypt(GVMyTAs.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString()), false);
        }
    }
    protected void GVMyTAs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVMyTAs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Objects.User ui = (Objects.User)Session["userinfo"];

            //HiddenField hdnIsTecComplete = new HiddenField();
            //hdnIsTecComplete = (HiddenField)e.Row.FindControl("hdnIsTecComplete");

            //LinkButton lnkPrintTE;
            //lnkPrintTE = (LinkButton)e.Row.FindControl("lnkPrintTE");

            //LinkButton ibDeleteTA;
            //ibDeleteTA = (LinkButton)e.Row.FindControl("ibDeleteTA");
            //string StatusCode = GVMyTAs.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values["StatusCode"].ToString();

            //if (StatusCode == "TA Pending")
            //{
            //    ibDeleteTA.Visible = true;
            //}
            //else
            //{
            //    ibDeleteTA.Visible = false;
            //}

            //if (hdnIsTecComplete.Value.Trim() == "True")
            //{
            //    lnkPrintTE.Visible = true;
            //    ibDeleteTA.Visible = false;
            //}
            //else
            //{
            //    lnkPrintTE.Visible = false;
            //}

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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TravelAuthorization/TravelAuthorizationFormWizard.aspx?First=1", false);
    }

}