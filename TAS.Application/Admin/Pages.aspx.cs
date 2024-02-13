using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
public partial class Admin_Pages : AuthenticatedPageClass
{
    Business.Pages baPages = new Business.Pages();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVPages.PreRender += new EventHandler(GVPages_PreRender);
        if (!IsPostBack)
        {
            LnkAddNewPage.Enabled = this.CanAdd;
            FillGrid();
        }
    }
    void GVPages_PreRender(object sender, EventArgs e)
    {
        if (GVPages.Rows.Count > 0)
        {
            GVPages.UseAccessibleHeader = true;
            GVPages.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            baPages.GetAllPages(ref dt);
            GVPages.DataSource = dt;
            GVPages.DataBind();

            lblGVPagesCount.Text = dt.Rows.Count.ToString();

            DataView dv = dt.DefaultView;
            dv.RowFilter = "ParentID = '00000000-0000-0000-0000-000000000000'";
            DDLParents.DataSource = dv;
            DDLParents.DataTextField = "PageName";
            DDLParents.DataValueField = "PageID";
            DDLParents.DataBind();
            DDLParents.Items.Insert(0, "-- Please Select --");
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        GVPages.SelectedIndex = -1;
        PanelMessage.Visible = false;
        lblmsg.Text = "";
        PanelMessageAddNewPage.Visible = false;
        lblmsgAddNewPage.Text = "";
        txtPageName.Text = "";
        txtPageURL.Text = "";
        DDLParents.SelectedIndex = -1;
        txtOrder.Text = "";
        rdNo.Checked = false;
        rdYes.Checked = true;
    }
    protected void GVPages_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GVPages.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditItem")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                PanelMessageAddNewPage.Visible = false;
                lblmsgAddNewPage.Text = "";

                // Prepare for update
                if (GVPages.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["ParentID"].ToString() != "00000000-0000-0000-0000-000000000000")
                { DDLParents.SelectedValue = GVPages.DataKeys[Convert.ToInt16(e.CommandArgument.ToString())].Values["ParentID"].ToString(); }
                else { DDLParents.SelectedIndex = -1; }
                txtPageName.Text = GVPages.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                txtPageURL.Text = GVPages.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[1].Text;
                txtOrder.Text = GVPages.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[3].Text;
                CheckBox chk;
                chk = (CheckBox)GVPages.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("chkMenu");
                if (chk.Checked)
                {
                    rdYes.Checked = true;
                    rdNo.Checked = false;
                }
                else
                {
                    rdNo.Checked = true;
                    rdYes.Checked = false;
                }
                //End

            }
            if (e.CommandName == "deleteItem")
            {
                baPages.DeletePages(GVPages.DataKeys[GVPages.SelectedIndex].Values["PageID"].ToString());
                ClearAll();
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                lblmsg.Text = "Page has been deleted successfully";
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVPages_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVPages_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void LnkAddNewPage_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            int TheOrder;
            if (!int.TryParse(txtOrder.Text, out TheOrder))
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Please entere a number in the page order field";
                return;
            }
            string TheID;
            if (DDLParents.SelectedIndex != 0)
                TheID = new Guid(DDLParents.SelectedValue).ToString();
            else
                TheID = Guid.Empty.ToString();
            if (GVPages.SelectedIndex == -1)
            {
                baPages.InsertUpdatePages("", txtPageName.Text, txtPageURL.Text, TheID, Convert.ToInt16(txtOrder.Text), rdYes.Checked);
                FillGrid();
                ClearAll();
                PanelMessageAddNewPage.Visible = true;
                PanelMessageAddNewPage.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewPage.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewPage.Text = "Page has been added successfully";
            }
            else
            {
                baPages.InsertUpdatePages(GVPages.DataKeys[GVPages.SelectedIndex].Values["PageID"].ToString(), txtPageName.Text, txtPageURL.Text, TheID, Convert.ToInt32(txtOrder.Text), rdYes.Checked);
                FillGrid();
                ClearAll();
                PanelMessageAddNewPage.Visible = true;
                PanelMessageAddNewPage.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewPage.ForeColor = System.Drawing.Color.Green;
                lblmsgAddNewPage.Text = "Page has been updated successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
}