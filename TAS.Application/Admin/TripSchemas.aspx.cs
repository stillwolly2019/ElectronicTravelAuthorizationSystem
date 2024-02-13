
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_TripSchemas : AuthenticatedPageClass
{

    Business.TAWorkFlow W = new Business.TAWorkFlow();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVTripSchemas.PreRender += new EventHandler(GVTripSchemas_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    void GVTripSchemas_PreRender(object sender, EventArgs e)
    {
        if (GVTripSchemas.Rows.Count > 0)
        {
            GVTripSchemas.UseAccessibleHeader = true;
            GVTripSchemas.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            W.GetTripSchemas(ref dt);
            GVTripSchemas.DataSource = dt;
            GVTripSchemas.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVTripSchemas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "MarkAsEmergency")
            {
                W.EmergencyTripSchemaToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsLeave")
            {
                W.LeaveTripSchemaToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsRandR")
            {
                W.RandRTripSchemaToggle(ID);
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    
    protected void GVTripSchemas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVTripSchemas.EditIndex == -1)
            {
                LinkButton ibE = new LinkButton();
                ibE = (LinkButton)e.Row.FindControl("ibIs");
                ibE.Enabled = this.CanEdit;

                LinkButton ibR = new LinkButton();
                ibR = (LinkButton)e.Row.FindControl("ibIsR");
                ibR.Enabled = this.CanEdit;

                LinkButton ibL = new LinkButton();
                ibL = (LinkButton)e.Row.FindControl("ibIsL");
                ibL.Enabled = this.CanEdit;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibConfirm = (LinkButton)e.Row.FindControl("ibIs");
                LinkButton iLConfirm = (LinkButton)e.Row.FindControl("ibIsL");
                LinkButton iRConfirm = (LinkButton)e.Row.FindControl("ibIsR");
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsEmergency"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsEmergency")) == 0)
                {
                    ibConfirm.CssClass = "fa fa-times";
                    ibConfirm.ForeColor = System.Drawing.Color.Red;
                    ibConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
                else
                {
                    ibConfirm.CssClass = "fa fa-check";
                    ibConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsRandR"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsRandR")) == 0)
                {
                    iRConfirm.CssClass = "fa fa-times";
                    iRConfirm.ForeColor = System.Drawing.Color.Red;
                    iRConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
                else
                {
                    iRConfirm.CssClass = "fa fa-check";
                    iRConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsLeave"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsLeave")) == 0)
                {
                    iLConfirm.CssClass = "fa fa-times";
                    iLConfirm.ForeColor = System.Drawing.Color.Red;
                    iLConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
                else
                {
                    iLConfirm.CssClass = "fa fa-check";
                    iLConfirm.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                }
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


}





