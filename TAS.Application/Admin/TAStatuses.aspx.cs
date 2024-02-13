
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Admin_TAStatuses : AuthenticatedPageClass
{

    Business.TAWorkFlow W = new Business.TAWorkFlow();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVTAStatuses.PreRender += new EventHandler(GVTAStatuses_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    void GVTAStatuses_PreRender(object sender, EventArgs e)
    {
        if (GVTAStatuses.Rows.Count > 0)
        {
            GVTAStatuses.UseAccessibleHeader = true;
            GVTAStatuses.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            W.GetTAStatusCodes(ref dt);
            GVTAStatuses.DataSource = dt;
            GVTAStatuses.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVTAStatuses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string ID = e.CommandArgument.ToString();
            if (e.CommandName == "MarkAsActioned")
            {
                W.ActionedTAStatusCodeToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsEditable")
            {
                W.EditableTAStatusCodeToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsReturnable")
            {
                W.ReturnableTAStatusCodeToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsCancellable")
            {
                W.CancellableTAStatusCodeToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsDelegatable")
            {
                W.DelegatableStatusCodeToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsByTravellor")
            {
                W.ByTravellorToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsBySupervisor")
            {
                W.BySupervisorToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsByRMO")
            {
                W.ByRMOToggle(ID);
                FillGrid();
            }

            if (e.CommandName == "MarkAsByApprover")
            {
                W.ByApproverToggle(ID);
                FillGrid();
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVTAStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow & GVTAStatuses.EditIndex == -1)
            {
                LinkButton ibA = new LinkButton();
                LinkButton ibE = new LinkButton();
                LinkButton ibR = new LinkButton();
                LinkButton ibC = new LinkButton();
                LinkButton ibD = new LinkButton();
                LinkButton ibTra = new LinkButton();
                LinkButton ibSup = new LinkButton();
                LinkButton ibRMO = new LinkButton();
                LinkButton ibApp = new LinkButton();

                ibA = (LinkButton)e.Row.FindControl("ibIsActioned");
                ibA.Enabled = this.CanEdit;

                ibE = (LinkButton)e.Row.FindControl("ibIsEditable");
                ibE.Enabled = this.CanEdit;

                ibR = (LinkButton)e.Row.FindControl("ibIsReturnable");
                ibR.Enabled = this.CanEdit;

                ibC = (LinkButton)e.Row.FindControl("ibIsCancellable");
                ibC.Enabled = this.CanEdit;

                ibD = (LinkButton)e.Row.FindControl("ibIsDelegatable");
                ibD.Enabled = this.CanEdit;

                ibTra = (LinkButton)e.Row.FindControl("ByTravellor");
                ibTra.Enabled = this.CanEdit;

                ibSup = (LinkButton)e.Row.FindControl("BySupervisor");
                ibSup.Enabled = this.CanEdit;

                ibRMO = (LinkButton)e.Row.FindControl("ByRMO");
                ibRMO.Enabled = this.CanEdit;

                ibApp = (LinkButton)e.Row.FindControl("ByApprover");
                ibApp.Enabled = this.CanEdit;


            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibIsActioned = (LinkButton)e.Row.FindControl("ibIsActioned");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("ibIsEditable");
                LinkButton ibReturn = (LinkButton)e.Row.FindControl("ibIsReturnable");
                LinkButton ibCancel = (LinkButton)e.Row.FindControl("ibIsCancellable");
                LinkButton ibDelegate = (LinkButton)e.Row.FindControl("ibIsDelegatable");
                LinkButton ibTra = (LinkButton)e.Row.FindControl("ByTravellor");
                LinkButton ibSup = (LinkButton)e.Row.FindControl("BySupervisor");
                LinkButton ibRMO = (LinkButton)e.Row.FindControl("ByRMO");
                LinkButton ibApp = (LinkButton)e.Row.FindControl("ByApprover");

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsActioned"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsActioned")) == 0)
                {
                    ibIsActioned.CssClass = "fa fa-times";
                    ibIsActioned.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibIsActioned.CssClass = "fa fa-check";
                }

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsEditable"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsEditable")) == 0)
                {
                    ibEdit.CssClass = "fa fa-times";
                    ibEdit.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibEdit.CssClass = "fa fa-check";
                }

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsReturnable"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsReturnable")) == 0)
                {
                    ibReturn.CssClass = "fa fa-times";
                    ibReturn.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibReturn.CssClass = "fa fa-check";
                }

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsCancellable"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsCancellable")) == 0)
                {
                    ibCancel.CssClass = "fa fa-times";
                    ibCancel.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibCancel.CssClass = "fa fa-check";
                }

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "IsDelegatable"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IsDelegatable")) == 0)
                {
                    ibDelegate.CssClass = "fa fa-times";
                    ibDelegate.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibDelegate.CssClass = "fa fa-check";
                }

                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "ByTravellor"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ByTravellor")) == 0)
                {
                    ibTra.CssClass = "fa fa-times";
                    ibTra.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibTra.CssClass = "fa fa-check";
                }
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "BySupervisor"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BySupervisor")) == 0)
                {
                    ibSup.CssClass = "fa fa-times";
                    ibSup.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibSup.CssClass = "fa fa-check";
                }
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "ByRMO"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ByRMO")) == 0)
                {
                    ibRMO.CssClass = "fa fa-times";
                    ibRMO.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibRMO.CssClass = "fa fa-check";
                }
                if (object.ReferenceEquals(DataBinder.Eval(e.Row.DataItem, "ByApprover"), DBNull.Value) || Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ByApprover")) == 0)
                {
                    ibApp.CssClass = "fa fa-times";
                    ibApp.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ibApp.CssClass = "fa fa-check";
                }
                ibIsActioned.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibEdit.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibReturn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibCancel.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibDelegate.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibTra.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibSup.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibRMO.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
                ibApp.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LookupsID").ToString();
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


}





