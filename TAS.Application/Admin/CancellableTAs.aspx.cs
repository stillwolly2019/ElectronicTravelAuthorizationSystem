
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_CancellableTAs : AuthenticatedPageClass
{

    Business.TAWorkFlow W = new Business.TAWorkFlow();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVCancellableStatuses.PreRender += new EventHandler(GVCancellableStatuses_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    void GVCancellableStatuses_PreRender(object sender, EventArgs e)
    {
        if (GVCancellableStatuses.Rows.Count > 0)
        {
            GVCancellableStatuses.UseAccessibleHeader = true;
            GVCancellableStatuses.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            W.GetCancellableTAStatusCodes(ref dt);
            GVCancellableStatuses.DataSource = dt;
            GVCancellableStatuses.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVCancellableStatuses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "MarkAsCancellable")
            {
                W.CancellableTAStatusCodeToggle(ID);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVCancellableStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVCancellableStatuses.EditIndex == -1)
            {
                LinkButton ibE = new LinkButton();
                ibE = (LinkButton)e.Row.FindControl("ibIsCancellable");
                ibE.Enabled = this.CanEdit;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibConfirm = (LinkButton)e.Row.FindControl("ibIsCancellable");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsCancellable"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsCancellable")) == 0)
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





