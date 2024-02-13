using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

public partial class RadioCheck_ConductRadioCheck : AuthenticatedPageClass
{
    HttpContext context = HttpContext.Current;
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVStaffRadioCheck.PreRender += new EventHandler(GVStaffRadioCheck_PreRender);
        if (!IsPostBack)
        {
            try
            {
                FillHeader();
                GetRadioCheckRollCall();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }
    void GVStaffRadioCheck_PreRender(object sender, EventArgs e)
    {
        if (GVStaffRadioCheck.Rows.Count > 0)
        {
            GVStaffRadioCheck.UseAccessibleHeader = true;
            GVStaffRadioCheck.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    private void FillHeader()
    {
        try
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            DataTable dt = new DataTable();
            R.GetRadioOperatorLocations(ref dt,ui.User_Id.ToString());
            ddlLocationsName.DataSource = dt;
            ddlLocationsName.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void GetRadioCheckRollCall()
    {
        try
        {
            string LocId = ddlLocationsName.SelectedValue.Trim();
            Objects.User ui = (Objects.User)Session["userinfo"];
            DateTime CheckDate = DateTime.Now.Date;
            string PERNO = ui.PRISMNumber;
            DataTable dt = new DataTable();
            R.GetRadioCheckRollCall(ref dt, CheckDate, LocId);
            GVStaffRadioCheck.DataSource = dt;
            GVStaffRadioCheck.DataBind();
            lblGVStaffRadioCheckCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ddlLocationsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetRadioCheckRollCall();
    }
    protected void GVStaffRadioCheck_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibIsAccountedFor = (LinkButton)e.Row.FindControl("ibIsAccountedFor");

                if (!Convert.ToBoolean(GVStaffRadioCheck.DataKeys[e.Row.RowIndex].Values["IsAccountedFor"]))
                {
                    ibIsAccountedFor.CssClass = "fa fa-square";
                    ibIsAccountedFor.ForeColor = System.Drawing.Color.DarkGreen;
                    ibIsAccountedFor.ToolTip = "Mark as accounted";
                }
                else
                {
                    ibIsAccountedFor.CssClass = "fa fa-check";
                    ibIsAccountedFor.ToolTip = "Mark as unaccounted";
                }
                ibIsAccountedFor.Enabled = this.CanEdit;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVStaffRadioCheck_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AccountedFor")
            {
                LinkButton ibIsAccountedFor = (LinkButton)GVStaffRadioCheck.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("ibIsAccountedFor");
                string StaffCallSign = GVStaffRadioCheck.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["CallSign"].ToString();
                string StaffPERNO = GVStaffRadioCheck.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["PRISMNumber"].ToString();
                string LocationID = ddlLocationsName.SelectedValue;

                if (!Convert.ToBoolean(GVStaffRadioCheck.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsAccountedFor"]))
                {
                    //update AD database
                    R.InsertDeleteRadioCheckInformation(StaffCallSign, StaffPERNO, LocationID, true);
                }
                else
                {
                    //update AD database
                    R.InsertDeleteRadioCheckInformation(StaffCallSign, StaffPERNO, LocationID, false);

                }
                GetRadioCheckRollCall();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

}





