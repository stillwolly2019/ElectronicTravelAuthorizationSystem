  
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

public partial class Admin_UsersPossibleDelegation : AuthenticatedPageClass
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
            u.GetAllPossibleDelegations("", ref dt);
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
            if (e.CommandName == "DeleteDelegation")
            {
                string Userid = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DelegatorID"].ToString();
                string DelegatedTo = GVUsersDelegation.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DelegateeID"].ToString();
                u.DeletePossibleDelegation(Userid, DelegatedTo);
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.Text = "Record has been deleted successfully";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                FillGrid();
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

                string UserId = hfUserId.Value;
                string DelegatedTo = hfDelegatedTo.Value;
                u.InsertPossibleDelegation(UserId, DelegatedTo);
                Clear();
                FillGrid();
                PanelMessageAddNewDelegation.Visible = true;
                PanelMessageAddNewDelegation.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewDelegation.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewDelegation.Text = "User has been added successfully";
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
                    txtDelegatorName.Text = dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString();
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
        u.GetAllPossibleDelegations(txtSearchText.Text.Trim(), ref dt);
        GVUsersDelegation.DataSource = dt;
        GVUsersDelegation.DataBind();
        lblGVUsersDelegationCount.Text = dt.Rows.Count.ToString();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearchText.Text = "";
        FillGrid();
    }
}
