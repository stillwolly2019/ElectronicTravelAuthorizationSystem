
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_DelegatableStatusCodes : AuthenticatedPageClass
{

    Business.TAWorkFlow W = new Business.TAWorkFlow();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVDelegatableStatusCodes.PreRender += new EventHandler(GVDelegatableStatusCodes_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    void GVDelegatableStatusCodes_PreRender(object sender, EventArgs e)
    {
        if (GVDelegatableStatusCodes.Rows.Count > 0)
        {
            GVDelegatableStatusCodes.UseAccessibleHeader = true;
            GVDelegatableStatusCodes.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            W.GetDelegatableStatusCodes(ref dt);
            GVDelegatableStatusCodes.DataSource = dt;
            GVDelegatableStatusCodes.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVDelegatableStatusCodes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "MarkAsEmergency")
            {
                W.DelegatableStatusCodeToggle(ID);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    
    protected void GVDelegatableStatusCodes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVDelegatableStatusCodes.EditIndex == -1)
            {
                LinkButton ibE = new LinkButton();
                ibE = (LinkButton)e.Row.FindControl("ibIsDelegatable");
                ibE.Enabled = this.CanEdit;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibConfirm = (LinkButton)e.Row.FindControl("ibIsDelegatable");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsDelegatable"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsDelegatable")) == 0)
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





