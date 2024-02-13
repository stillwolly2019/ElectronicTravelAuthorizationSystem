  
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_ReturnCancelationReasons : AuthenticatedPageClass
{
    Business.Roles R = new Business.Roles();
    Business.TAWorkFlow W = new Business.TAWorkFlow();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVStatusCodeRejectionReasons.PreRender += new EventHandler(GVStatusCodeRejectionReasons_PreRender);
        if (!IsPostBack)
        {
            FillLookups();
        }
    }
    void GVStatusCodeRejectionReasons_PreRender(object sender, EventArgs e)
    {
        if (GVStatusCodeRejectionReasons.Rows.Count > 0)
        {
            GVStatusCodeRejectionReasons.UseAccessibleHeader = true;
            GVStatusCodeRejectionReasons.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillLookups()
    {
        try
        {
            DataTable dt = new DataTable();
            R.GetReturnableCancellableStatusCodes(ref dt);
            DDLStatusCodes.DataSource = dt;
            DDLStatusCodes.DataTextField = "StatusDescription";
            DDLStatusCodes.DataValueField = "LookupsID";
            DDLStatusCodes.DataBind();
            DDLStatusCodes.SelectedIndex = 0;
            FillGrid();
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
            W.GetRejectionReasonsByStatusCode(DDLStatusCodes.SelectedValue, ref dt);
            GVStatusCodeRejectionReasons.DataSource = dt;
            GVStatusCodeRejectionReasons.DataBind();
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

    protected void GVStatusCodeRejectionReasons_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName == "GrantRPermission")
            {
                string LookupsID = e.CommandArgument.ToString();
                W.StatusCodeRejectionReasonsToggle(DDLStatusCodes.SelectedValue, LookupsID);
                FillGrid();
            }
            if (e.CommandName == "GrantCPermission")
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

    protected void GVStatusCodeRejectionReasons_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void GVStatusCodeRejectionReasons_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVStatusCodeRejectionReasons.EditIndex == -1)
            {
                LinkButton ibR = new LinkButton();
                ibR = (LinkButton)e.Row.FindControl("ibRApply");
                ibR.Enabled = this.CanEdit;

                LinkButton ibC = new LinkButton();
                ibC = (LinkButton)e.Row.FindControl("ibCApply");
                ibC.Enabled = this.CanEdit;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibConfirmR = (LinkButton)e.Row.FindControl("ibRApply");
                LinkButton ibConfirmC = (LinkButton)e.Row.FindControl("ibCApply");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "ApplyReturnReason"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ApplyReturnReason")) == 0)
                {
                    ibConfirmR.CssClass = "fa fa-times";
                    ibConfirmR.ForeColor = System.Drawing.Color.Red;
                    ibConfirmR.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
                else
                {
                    ibConfirmR.CssClass = "fa fa-check";
                    ibConfirmR.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "ApplyCancelationReason"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ApplyCancelationReason")) == 0)
                {
                    ibConfirmC.CssClass = "fa fa-times";
                    ibConfirmC.ForeColor = System.Drawing.Color.Red;
                    ibConfirmC.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
                else
                {
                    ibConfirmC.CssClass = "fa fa-check";
                    ibConfirmC.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}



