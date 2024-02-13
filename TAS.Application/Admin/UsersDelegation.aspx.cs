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

public partial class Admin_UsersDelegation : AuthenticatedPageClass
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
            DataTable dt = new DataTable();
            u.GetAllDelegations("", ref dt);
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
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibEdit.Visible = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
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
            if (e.CommandName == "EditDelegation")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewDelegation.Visible = false;
                lblmsgAddNewDelegation.Text = "";

                // Prepare for update

                hfID.Value = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["ID"].ToString();
                hfDelegatedTo.Value = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DeligatedTo"].ToString();
                hfUserId.Value = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["UserId"].ToString();
                string UserID = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["UserId"].ToString();
                txtDelegatorName.Text = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["Delegator"].ToString();
                txtDelegateeName.Text = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["Delegatee"].ToString();
                txtDateFrom.Text = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DateFrom"].ToString();
                txtDateTo.Text = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DateTo"].ToString();
                txtRemark.Text = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["Remark"].ToString();
                // End
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void Clear()
    {
        txtDelegatorName.Text = "";
        txtDelegateeName.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";

        txtDelegatorName.Enabled = true;
        txtDelegateeName.Enabled = true;
        PanelMessageAddNewDelegation.Visible = false;
        lblmsgAddNewDelegation.Text = "";
        hfUserId.Value = "";

    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (GVUsersDelegation.SelectedIndex == -1)
            {
                if (hfUserId.Value == "")
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegator name not found";
                    return;
                }

                if (hfDelegatedTo.Value == "")
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegatee name not found";
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

              

                DateTime? DateFrom = null;
                DateTime? DateTo = null;
                if (!String.IsNullOrEmpty(txtDateFrom.Text)) { DateFrom = Convert.ToDateTime(txtDateFrom.Text); }
                if (!String.IsNullOrEmpty(txtDateTo.Text)) { DateTo = Convert.ToDateTime(txtDateTo.Text); }
                string UserId = hfUserId.Value;
                string DelegatedTo = hfDelegatedTo.Value;


                if (UserId.ToUpper() == DelegatedTo.ToUpper())
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "The delegator and delegated cannot be the same!";
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

                if (Convert.ToDateTime(txtDateFrom.Text).Date<DateTime.Now.Date)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation for past dates not supported";
                    return;
                }

                if (Convert.ToDateTime(txtDateFrom.Text).Date >Convert.ToDateTime(txtDateTo.Text).Date)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation end date must be greated than delegation start date";
                    return;
                }

                if (u.AlreadyDelegated(0,hfUserId.Value,hfDelegatedTo.Value,DateFrom,DateTo) || u.PersonalMultipleDelegation(0, hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation Overlap not supported. Please contact your administrator for details";
                    return;
                }


                if (u.UserHasActiveDeligation(0, hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "User has an active delegation for specified period";
                    return;
                }
                if (u.IsMultipleDelegation(0, hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo))
                {
                    if (!u.AllowMultipleDelegation(hfDelegatedTo.Value))
                    {
                        PanelMessageAddNewDelegation.Visible = true;
                        PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                        lblmsgAddNewDelegation.ForeColor = Color.Red;
                        lblmsgAddNewDelegation.Text = "User has an active delegation for specified period";
                        return;
                    }
                    else
                    {
                        if (u.MaximumDelegationLimitReached(hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo))
                        {
                            PanelMessageAddNewDelegation.Visible = true;
                            PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                            lblmsgAddNewDelegation.ForeColor = Color.Red;
                            lblmsgAddNewDelegation.Text = "Intended delegate has reached his maximum limit";
                            return;
                        }
                    }
                }

                u.InsertUpdateDelegations(0, UserId, DelegatedTo, DateFrom, DateTo, txtRemark.Text.Trim());
                Clear();
                FillGrid();
                PanelMessageAddNewDelegation.Visible = true;
                PanelMessageAddNewDelegation.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewDelegation.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewDelegation.Text = "User has been added successfully";
            }
            else
            {
                string UserId = hfUserId.Value;
                string DelegatedTo = hfDelegatedTo.Value;
                DateTime DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                DateTime DateTo = Convert.ToDateTime(txtDateTo.Text);
                int ID = Convert.ToInt32(GVUsersDelegation.DataKeys[GVUsersDelegation.SelectedIndex].Values["ID"].ToString());

                if (UserId== DelegatedTo)
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "The delegator and delegated cannot be the same!";
                    return;
                }
                if (String.IsNullOrEmpty(txtRemark.Text))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Please enter delegation remarks!";
                    return;
                }
                if (DateTo<DateFrom)
                {
                    u.RevertDeligation(ID);
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-success alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Green;
                    lblmsgAddNewDelegation.Text = "Delegation reverted successfully";
                    Response.Redirect("UsersDelegation.aspx", false);
                }

                if (u.AlreadyDelegated(ID, hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo,false) || u.PersonalMultipleDelegation(ID, hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo,false))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "Delegation Overlap not supported. Please contact your administrator for details";
                    return;
                }


                if (u.UserHasActiveDeligation(ID, hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo,false))
                {
                    PanelMessageAddNewDelegation.Visible = true;
                    PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDelegation.ForeColor = Color.Red;
                    lblmsgAddNewDelegation.Text = "User has an active delegation for specified period";
                    return;
                }

                if (u.IsMultipleDelegation(ID, hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo, false))
                {
                    if (!u.AllowMultipleDelegation(hfDelegatedTo.Value))
                    {
                        PanelMessageAddNewDelegation.Visible = true;
                        PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                        lblmsgAddNewDelegation.ForeColor = Color.Red;
                        lblmsgAddNewDelegation.Text = "User has an active delegation for specified period";
                        return;
                    }
                    else
                    {
                        if (u.MaximumDelegationLimitReached(hfUserId.Value, hfDelegatedTo.Value, DateFrom, DateTo, false))
                        {
                            PanelMessageAddNewDelegation.Visible = true;
                            PanelMessageAddNewDelegation.CssClass = "alert alert-danger alert-dismissable";
                            lblmsgAddNewDelegation.ForeColor = Color.Red;
                            lblmsgAddNewDelegation.Text = "Delegation to intended user has reached its maximum limit";
                            return;
                        }
                    }
                }

                u.InsertUpdateDelegations(ID, UserId, DelegatedTo, DateFrom, DateTo, txtRemark.Text, false);
                Clear();
                FillGrid();
                PanelMessageAddNewDelegation.Visible = true;
                PanelMessageAddNewDelegation.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewDelegation.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewDelegation.Text = "Delegation has been updated successfully";
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void txtDelegatorName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDelegatorName.Text != "")
            {
                DataTable dt = new DataTable();
                u.SearchUsersForDelegation(txtDelegatorName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    hfUserId.Value = dt.Rows[0]["UserId"].ToString();
                    txtDelegatorName.Text = dt.Rows[0]["FirstName"].ToString()+" "+ dt.Rows[0]["LastName"].ToString();
                }
                else
                {
                    hfUserId.Value = "";
                }
            }
            else
            {
                Clear();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void txtDelegateeName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDelegateeName.Text != "")
            {
                DataTable dt = new DataTable();
                u.SearchUsersForDelegation(txtDelegateeName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    hfDelegatedTo.Value = dt.Rows[0]["UserId"].ToString();
                    txtDelegateeName.Text = dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString();
                }
                else
                {
                    hfUserId.Value = "";
                }
            }
            else
            {
                Clear();
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