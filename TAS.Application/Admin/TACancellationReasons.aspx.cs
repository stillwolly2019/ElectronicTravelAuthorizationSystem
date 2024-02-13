  
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_TACancellationReasons : AuthenticatedPageClass
{
    Business.Roles R = new Business.Roles();
    Business.TAWorkFlow W = new Business.TAWorkFlow();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVStatusCodeCancellationReasons.PreRender += new EventHandler(GVStatusCodeCancellationReasons_PreRender);
        if (!IsPostBack)
        {
            FillLookups();
        }
    }
    void GVStatusCodeCancellationReasons_PreRender(object sender, EventArgs e)
    {
        if (GVStatusCodeCancellationReasons.Rows.Count > 0)
        {
            GVStatusCodeCancellationReasons.UseAccessibleHeader = true;
            GVStatusCodeCancellationReasons.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillLookups()
    {
        try
        {
            DataTable dt = new DataTable();
            R.GetReturnableCancellableStatusCodes(ref dt,"C");
            DDLStatusCodes.DataSource = dt;
            DDLStatusCodes.DataTextField = "StatusDescription";
            DDLStatusCodes.DataValueField = "LookupsID";
            DDLStatusCodes.DataBind();
            DDLStatusCodes.SelectedIndex = 0;
            FillGrid();
            //dt = new DataTable();
            //W.GetAllTripSchemas(ref dt);
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
            W.GetCancellationReasonsByStatusCode(DDLStatusCodes.SelectedValue, ref dt);
            GVStatusCodeCancellationReasons.DataSource = dt;
            GVStatusCodeCancellationReasons.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void DDLStatusCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void GVStatusCodeCancellationReasons_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName == "GrantPermission")
            {
                string LookupsID = e.CommandArgument.ToString();
                W.StatusCodeCancellationReasonsToggle(DDLStatusCodes.SelectedValue, LookupsID);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVStatusCodeCancellationReasons_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVStatusCodeCancellationReasons_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVStatusCodeCancellationReasons.EditIndex == -1)
            {
                LinkButton ibE = new LinkButton();
                ibE = (LinkButton)e.Row.FindControl("ibApply");
                ibE.Enabled = this.CanEdit;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibConfirm = (LinkButton)e.Row.FindControl("ibApply");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "ApplyReason"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ApplyReason")) == 0)
                {
                    ibConfirm.CssClass = "fa fa-times";
                    ibConfirm.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibConfirm.CssClass = "fa fa-check";
                }
                ibConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}



