  
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
public partial class Admin_Delegate : AuthenticatedPageClass
{
    Business.Locations baLocations = new Business.Locations();
    Business.Users usr = new Business.Users();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVDelegations.PreRender += new EventHandler(GVDelegations_PreRender);
        if (!IsPostBack)
        {
            CalendarExtender1.StartDate = DateTime.Now;
            CalendarExtender2.StartDate = DateTime.Now;
            try
            {
                LnkSave.Enabled = this.CanAdd;
                FillDDL();
                FillGrid();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }

        }
    }
    void GVDelegations_PreRender(object sender, EventArgs e)
    {
        if (GVDelegations.Rows.Count > 0)
        {
            GVDelegations.UseAccessibleHeader = true;
            GVDelegations.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            Objects.User ui = (Objects.User)Session["userinfo"];
            DataTable dt = new DataTable();
            usr.GetUserDelegations(ui.User_Id,ref dt);
            GVDelegations.DataSource = dt;
            GVDelegations.DataBind();
            lblGVDelegationsCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillDDL()
    {
        try
        {
            Objects.User ui = (Objects.User)Session["userinfo"];
            DataTable dt = new DataTable();
            usr.GetPossibleDelegatees(ui.User_Id,ref dt);
            DDLUsers.DataSource = dt;
            DDLUsers.DataTextField = "DisplayName";
            DDLUsers.DataValueField = "UserId";
            DDLUsers.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        PanelMessageAddNewDelegation.Visible = false;
        lblmsgAddNewDelegation.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        txtRemark.Text = "";
        DDLUsers.SelectedIndex = -1;
    }
    protected void GVDelegations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVDelegations.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditDelegation")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewDelegation.Visible = false;
                lblmsgAddNewDelegation.Text = "";

                // Prepare for update
                if(GVDelegations.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserId"].ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    DDLUsers.SelectedValue = GVDelegations.DataKeys[Convert.ToInt16(e.CommandArgument.ToString())].Values["DeligatedTo"].ToString();
                }
                else
                {
                    DDLUsers.SelectedIndex = -1;
                }
                txtDateFrom.Text = GVDelegations.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DateFrom"].ToString();
                txtDateTo.Text = GVDelegations.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DateTo"].ToString();
                txtRemark.Text = GVDelegations.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["Remark"].ToString();
                //End
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVDelegations_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVDelegations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("ibEdit");
                ibEdit.Visible = this.CanEdit;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void LnkAddNewDelegation_Click(object sender, EventArgs e)
    {
        ClearAll();
        GVDelegations.SelectedIndex = -1;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (GVDelegations.SelectedIndex == -1)
            {
                if (DDLUsers.SelectedIndex == -1)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Please select a staff to delegate";
                    return;
                }

                if (String.IsNullOrEmpty(txtDateFrom.Text) || String.IsNullOrEmpty(txtDateTo.Text))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Please select valid date range";
                    return;
                }


                Objects.User ui = (Objects.User)Session["userinfo"];
                string UserId = ui.User_Id;
                string DelegatedTo = DDLUsers.SelectedValue;
                DateTime DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                DateTime DateTo = Convert.ToDateTime(txtDateTo.Text);

                if (DateFrom.Date < DateTime.Now.Date)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = System.Drawing.Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation from past dates NOT supported!";
                    return;
                }

                if (DateFrom > DateTo)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation end date should be greater than start date";
                    return;
                }

                if (usr.AlreadyDelegated(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo) || usr.PersonalMultipleDelegation(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation overlap noted for selected user";
                    return;
                }

                if (usr.UserHasActiveDeligation(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "You have an active delegation for specified period";
                    return;
                }

                if (usr.IsMultipleDelegation(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Intended user has an active delegation from another user. Please sontact your administrator";
                    return;
                }
                usr.InsertUpdateDelegations(0, UserId, DelegatedTo, DateFrom, DateTo, txtRemark.Text.Trim());
                ClearAll();
                FillGrid();
                PanelMessageAddNewDelegation.Visible = true;
                PanelMessageAddNewDelegation.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewDelegation.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewDelegation.Text = "User has been delegated successfully";
            }
            else
            {
                //Edit Mode
                Objects.User ui = (Objects.User)Session["userinfo"];
                string UserId = ui.User_Id;
                string DelegatedTo = DDLUsers.SelectedValue;
                DateTime DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                DateTime DateTo = Convert.ToDateTime(txtDateTo.Text);
                int ID = Convert.ToInt32(GVDelegations.DataKeys[GVDelegations.SelectedIndex].Values["ID"].ToString());
                usr.RevertDeligation(ID);
                ClearAll();
                FillGrid();
                PanelMessageAddNewDelegation.Visible = true;
                PanelMessageAddNewDelegation.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewDelegation.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewDelegation.Text = "Delegation has been updated successfully";
                //Edit Mode
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


}
