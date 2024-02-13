using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;


public partial class Admin_DocumentTypes : AuthenticatedPageClass
{
    Business.DocumentTypes l = new Business.DocumentTypes();
    protected void Page_Load(object sender, EventArgs e)
    {
        gvDocumentType.PreRender += new EventHandler(gvDocumentType_PreRender);
        if (!IsPostBack)
        {
            FillGrid();
        }

        LnkSave.Visible = this.CanAdd;
    }
    void gvDocumentType_PreRender(object sender, EventArgs e)
    {
        if (gvDocumentType.Rows.Count > 0)
        {
            gvDocumentType.UseAccessibleHeader = true;
            gvDocumentType.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            l.GetAllDocumentTypes(ref dt);
            gvDocumentType.DataSource = dt;
            gvDocumentType.DataBind();

            lblCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    void ClearAll()
    {
        lblmsg.Text = "";
        lblmsgAddNewDocumentType.Text = "";
        PanelMessage.Visible = false;
        PanelMessageAddNewDocumentType.Visible = false;
        txtName.Text = "";


        gvDocumentType.SelectedIndex = -1;
        for (int i = 0; i <= gvDocumentType.Rows.Count - 1; i++)
        {
            gvDocumentType.Rows[i].BackColor = default(System.Drawing.Color);
            gvDocumentType.Rows[i].ForeColor = default(System.Drawing.Color);
        }
    }

    protected void gvDocumentType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditDocumentType")
            {
                ClearAll();
                gvDocumentType.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvDocumentType.SelectedRow.BackColor = Color.Green;
                gvDocumentType.SelectedRow.ForeColor = Color.White;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                txtName.Text = gvDocumentType.SelectedRow.Cells[0].Text;
            }
            if (e.CommandName == "deleteItem")
            {
                l.DeleteDocumentType(gvDocumentType.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["DocumentTypeID"].ToString());
                FillGrid();
                ClearAll();
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Item has been deleted successfully";
            }
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvDocumentType_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvDocumentType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (gvDocumentType.EditIndex == -1))
        {
            LinkButton ibE = new LinkButton();
            ibE = (LinkButton)e.Row.FindControl("ibEdit");
            ibE.Visible = this.CanEdit;
            LinkButton ibD = new LinkButton();
            ibD = (LinkButton)e.Row.FindControl("ibDelete");
            ibD.Visible = this.CanDelete;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvDocumentType.SelectedIndex == -1)
            {
                DataTable dt = new DataTable();
                l.CheckDuplicateDocumentType(Guid.Empty.ToString(), txtName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    PanelMessageAddNewDocumentType.Visible = true;
                    PanelMessageAddNewDocumentType.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDocumentType.ForeColor = Color.Red;
                    lblmsgAddNewDocumentType.Text = "This item is already added, please choose different name";
                    return;
                }

                l.InsertUpdateDocumentType("", txtName.Text.Trim());
                FillGrid();
                ClearAll();
                PanelMessageAddNewDocumentType.Visible = true;
                PanelMessageAddNewDocumentType.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewDocumentType.ForeColor = Color.Green;
                lblmsgAddNewDocumentType.Text = "Item has been added successfully";
            }
            else
            {
                DataTable dt = new DataTable();
                l.CheckDuplicateDocumentType(gvDocumentType.DataKeys[gvDocumentType.SelectedIndex].Values["DocumentTypeID"].ToString(), txtName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    PanelMessageAddNewDocumentType.Visible = true;
                    PanelMessageAddNewDocumentType.CssClass = "alert alert-danger alert-dismissable";
                    lblmsgAddNewDocumentType.ForeColor = Color.Red;
                    lblmsgAddNewDocumentType.Text = "This item is already added, please choose different name";
                    return;
                }

                l.InsertUpdateDocumentType(gvDocumentType.DataKeys[gvDocumentType.SelectedIndex].Values["DocumentTypeID"].ToString(), txtName.Text.Trim());
                FillGrid();
                ClearAll();
                PanelMessageAddNewDocumentType.Visible = true;
                PanelMessageAddNewDocumentType.CssClass = "alert alert-success alert-dismissable";
                lblmsgAddNewDocumentType.ForeColor = Color.Green;
                lblmsgAddNewDocumentType.Text = "Item has been updated successfully";
            }
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void LnkAddNewDocumentType_Click(object sender, EventArgs e)
    {
        ClearAll();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
}