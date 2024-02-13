
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_EmergencyTripSchemas : AuthenticatedPageClass
{

    Business.TAWorkFlow W = new Business.TAWorkFlow();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVEmergencyTripSchemas.PreRender += new EventHandler(GVEmergencyTripSchemas_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    void GVEmergencyTripSchemas_PreRender(object sender, EventArgs e)
    {
        if (GVEmergencyTripSchemas.Rows.Count > 0)
        {
            GVEmergencyTripSchemas.UseAccessibleHeader = true;
            GVEmergencyTripSchemas.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            W.GetEmergencyTripSchemas(ref dt);
            GVEmergencyTripSchemas.DataSource = dt;
            GVEmergencyTripSchemas.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVEmergencyTripSchemas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "MarkAsEmergency")
            {
                W.EmergencyTripSchemaToggle(ID);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    
    protected void GVEmergencyTripSchemas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVEmergencyTripSchemas.EditIndex == -1)
            {
                LinkButton ibE = new LinkButton();
                ibE = (LinkButton)e.Row.FindControl("ibIsEmergency");
                ibE.Enabled = this.CanEdit;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibConfirm = (LinkButton)e.Row.FindControl("ibIsEmergency");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsEmergency"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsEmergency")) == 0)
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





