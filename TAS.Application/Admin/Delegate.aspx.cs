
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.Mail;
using System.IO;

public partial class Admin_Delegate : AuthenticatedPageClass
{
    AuthenticatedPageClass A = new AuthenticatedPageClass();
    Business.Users u = new Business.Users();
    Business.Roles r = new Business.Roles();
    Business.Lookups l = new Business.Lookups();
    RadioCheckBusiness.RadioCheck Noti = new RadioCheckBusiness.RadioCheck();
    Business.MailModel MM = new Business.MailModel();
    DataView dv = new DataView();
    Globals g = new Globals();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVUsersDelegation.PreRender += new EventHandler(GVUsersDelegation_PreRender);
        if (!IsPostBack)
        {
            CalendarExtender1.StartDate = DateTime.Now;
            CalendarExtender2.StartDate = DateTime.Now;
            try
            {
                LnkSave.Enabled = this.CanAdd;
                FillGrid();
                FillDDL();
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }

    void GVUsersDelegation_PreRender(object sender, EventArgs e)
    {
        if (GVUsersDelegation.Rows.Count > 0)
        {
            GVUsersDelegation.UseAccessibleHeader = true;
            GVUsersDelegation.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    void FillGrid()
    {
        try
        {
            Objects.User ui = (Objects.User)Session["userinfo"];
            DataTable dt = new DataTable();
            u.GetUserDelegations(ui.User_Id, ref dt);
            dv = dt.DefaultView;
            GVUsersDelegation.DataSource = dv;
            GVUsersDelegation.DataBind();
            lblGVUsersDelegationCount.Text = dt.Rows.Count.ToString();
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
            u.GetPossibleDelegatees(ui.User_Id, ref dt);
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

    protected void GVUsersDelegation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void GVUsersDelegation_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void GVUsersDelegation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVUsersDelegation.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "RevertDelegation")
            {
                int ID = Convert.ToInt32(GVUsersDelegation.DataKeys[GVUsersDelegation.SelectedIndex].Values["ID"].ToString());
                u.RevertDeligation(ID);
                Response.Redirect("Delegate.aspx", false);
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


    void Clear()
    {
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        DDLUsers.SelectedIndex = -1;
        PanelMessageAddNewDelegation.Visible = false;
        lblmsgAddNewDelegation.Text = "";
    }

    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (GVUsersDelegation.SelectedIndex == -1)
            {
                if (DDLUsers.SelectedIndex ==-1)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Please select a user to delegate to";
                    return;
                }

                if (String.IsNullOrEmpty(txtRemark.Text))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Please enter delegation remarks";
                    return;
                }

                Objects.User ui = (Objects.User)Session["userinfo"];
                DateTime? DateFrom = null;
                DateTime? DateTo = null;
                if (!String.IsNullOrEmpty(txtDateFrom.Text)) { DateFrom = Convert.ToDateTime(txtDateFrom.Text); }
                if (!String.IsNullOrEmpty(txtDateTo.Text)) { DateTo = Convert.ToDateTime(txtDateTo.Text); }


                if (String.IsNullOrEmpty(txtDateFrom.Text) || String.IsNullOrEmpty(txtDateTo.Text))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Please select valid date range";
                    return;
                }

                if (Convert.ToDateTime(txtDateFrom.Text).Date < DateTime.Now.Date)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation for past dates not supported";
                    return;
                }

                if (Convert.ToDateTime(txtDateFrom.Text).Date > Convert.ToDateTime(txtDateTo.Text).Date)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation end date must be greated than delegation start date";
                    return;
                }

                if (u.AlreadyDelegated(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo) || u.PersonalMultipleDelegation(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation Overlap not supported. Please contact your administrator for details";
                    return;
                }


                if (u.UserHasActiveDeligation(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "User has an active delegation for specified period";
                    return;
                }
                if (u.IsMultipleDelegation(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo))
                {
                    if (!u.AllowMultipleDelegation(DDLUsers.SelectedValue))
                    {
                        PanelMessageAddNewDelegation.Visible = true;
                        PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                        lblmsgAddNewDelegation.ForeColor = Color.Red;
                        lblmsgAddNewDelegation.Text = "User has an active delegation for specified period";
                        return;
                    }
                    else
                    {
                        if (u.MaximumDelegationLimitReached(ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo))
                        {
                            PanelMessageAddNewDelegation.Visible = true;
                            PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                            lblmsgAddNewDelegation.ForeColor = Color.Red;
                            lblmsgAddNewDelegation.Text = "Intended delegate has reached his maximum limit";
                            return;
                        }
                    }
                }
                u.InsertUpdateDelegations(0, ui.User_Id, DDLUsers.SelectedValue, DateFrom, DateTo, txtRemark.Text.Trim());
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                int ID = Convert.ToInt32(GVUsersDelegation.DataKeys[GVUsersDelegation.SelectedIndex].Values["ID"].ToString());
                u.RevertDeligation(ID);
                Response.Redirect("~/Default.aspx", false);
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void LnkAddNewDelegation_Click(object sender, EventArgs e)
    {
        Clear();
        GVUsersDelegation.SelectedIndex = -1;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }

    protected void btnAdvSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        u.SearchUsersForDelegation(txtSearchText.Text.Trim(), ref dt);

        GVUsersDelegation.DataSource = dt;
        GVUsersDelegation.DataBind();
        lblGVUsersDelegationCount.Text = dt.Rows.Count.ToString();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearchText.Text = "";
    }
}