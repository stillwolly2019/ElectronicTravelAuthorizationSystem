  
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_ManageDepartments : AuthenticatedPageClass
{
    Business.Employees em = new Business.Employees();
    Business.Departments de = new Business.Departments();
    Business.Missions m = new Business.Missions();
    Business.Security suh = new Business.Security();
    Business.Units u = new Business.Units();
    Business.SubUnits su = new Business.SubUnits();

    protected void Page_Load(object sender, EventArgs e)
    {
        GVDepartments.PreRender += new EventHandler(GVDepartments_PreRender);
        GVUnits.PreRender += new EventHandler(GVUnits_PreRender);
        GVSubUnits.PreRender += new EventHandler(GVSubUnits_PreRender);
        PanelMessage.Visible = false;

        if (!IsPostBack)
        {
            LnkAddNewItem.Enabled = this.CanAdd;
            FillHeader();
            FillGrid();
        }
    }
    void GVDepartments_PreRender(object sender, EventArgs e)
    {
        if (GVDepartments.Rows.Count > 0)
        {
            GVDepartments.UseAccessibleHeader = true;
            GVDepartments.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void GVUnits_PreRender(object sender, EventArgs e)
    {
        if (GVUnits.Rows.Count > 0)
        {
            GVUnits.UseAccessibleHeader = true;
            GVUnits.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void GVSubUnits_PreRender(object sender, EventArgs e)
    {
        if (GVSubUnits.Rows.Count > 0)
        {
            GVSubUnits.UseAccessibleHeader = true;
            GVSubUnits.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    private void FillHeader()
    {
        try
        {
            DataTable dt = new DataTable();
            m.GetAllMissions(ref dt);
            ddlMissionsName.DataSource = dt;
            ddlMissionsName.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void Clear()
    {
        txtName.Text = "";
        PanelMessagePopUp.Visible = false;
        lblmsgPopUp.Text = "";
        GVDepartments.SelectedIndex = -1;
    }
    void FillGrid()
    {

        DataTable dt = new DataTable();
        de.GetAllDepartmentByMissionID(ddlMissionsName.SelectedValue, ref dt);
        lblGVCount.Text = dt.Rows.Count.ToString();
        GVDepartments.DataSource = dt;
        GVDepartments.DataBind();
    }
    protected void GVDepartments_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "ManageUnit")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openUnitModal('" + GVDepartments.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DeptName"].ToString() + "');", true);
            ClearUnitPopUp();
            GVDepartments.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            GVDepartments.SelectedIndex = -1;
            hdnDepartmentID.Value = "";
            hdnDepartmentID.Value = GVDepartments.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DepartmentID"].ToString();
            hdnMissionID.Value = GVDepartments.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["MissionID"].ToString();
            FillUnitGrid();
        }
        if (e.CommandName == "EditItem")
        {
            Clear();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            GVDepartments.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            txtName.Text = GVDepartments.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DeptName"].ToString();
        }
        else if (e.CommandName == "DeleteItem")
        {
            DataTable dtCount = new DataTable();
            em.GetStaffNumberUnderDepUnitSubUnit(GVDepartments.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DepartmentID"].ToString(), "00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", ref dtCount);
            if (dtCount.Rows.Count > 0)
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.Text = "Item can't be deleted, there are staff members under this department";
                lblmsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                de.DeleteDepartment(GVDepartments.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["DepartmentID"].ToString());
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.Text = "Department has been deleted successfully";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                FillGrid();
            }
        }
    }
    protected void GVDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibAdd = (LinkButton)e.Row.FindControl("ibAdd");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liDeptName");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibAdd.Visible = this.CanAdd;
                ibEdit.Enabled = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void LnkAddNewItem_Click(object sender, EventArgs e)
    {
        Clear();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    }
    protected void LnkSave_Click(object sender, EventArgs e)
    {
        int result = 0;
        if (GVDepartments.SelectedIndex == -1)
        {
            result = de.InsertDepartments("", txtName.Text.Trim(), ddlMissionsName.SelectedValue);
        }
        else
        {
            string DepartmentID = GVDepartments.DataKeys[GVDepartments.SelectedIndex].Values["DepartmentID"].ToString();
            result = de.InsertDepartments(DepartmentID, txtName.Text.Trim(), ddlMissionsName.SelectedValue);
        }

        Clear();
        FillGrid();
        PanelMessagePopUp.Visible = true;

        if (result == -1)
        {
            PanelMessagePopUp.CssClass = "alert alert-danger alert-dismissable";
            lblmsgPopUp.ForeColor = System.Drawing.Color.Red;
            lblmsgPopUp.Text = "Department with the same name already exists";
        }
        else
        {
            PanelMessagePopUp.CssClass = "alert alert-success alert-dismissable";
            lblmsgPopUp.ForeColor = System.Drawing.Color.Green;
            lblmsgPopUp.Text = "Department has been added successfully";
        }

    }
    #region Unit
    private void ClearUnitPopUp()
    {
        lblAmsg.Text = "";
        PanelAmsg.Visible = false;
        txtUnitName.Text = "";
        GVUnits.SelectedIndex = -1;
    }
    void FillUnitGrid()
    {
        DataTable dt = new DataTable();
        u.GetAllUnitsbyDepID(hdnDepartmentID.Value, ref dt);
        lblGVUnitCount.Text = dt.Rows.Count.ToString();
        GVUnits.DataSource = dt;
        GVUnits.DataBind();
    }
    protected void GVUnits_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "ManageSubUnit")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSubUnitModal('" + GVUnits.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UnitName"].ToString() + "');", true);
                ClearSubUnitPopUp();
                GVUnits.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                GVUnits.SelectedIndex = -1;
                hdnUnitID.Value = "";
                hdnUnitID.Value = GVUnits.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UnitID"].ToString();
                //lblUnitName.Text = GVUnits.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UnitName"].ToString();
                FillSubUnitGrid();
            }
            if (e.CommandName == "EditItem")
            {
                ClearUnitPopUp();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openUnitModal();", true);
                GVUnits.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                txtUnitName.Text = GVUnits.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["UnitName"].ToString();
            }
            else if (e.CommandName == "DeleteItem")
            {
                DataTable dtCount = new DataTable();
                em.GetStaffNumberUnderDepUnitSubUnit("00000000-0000-0000-0000-000000000000", GVUnits.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["UnitID"].ToString(), "00000000-0000-0000-0000-000000000000", ref dtCount);
                if (dtCount.Rows.Count > 0)
                {
                    PanelAmsg.Visible = true;
                    PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                    lblAmsg.Text = "Item can't be deleted, there are staff members under this unit";
                    lblAmsg.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    u.DeleteUnit(GVUnits.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["UnitID"].ToString());
                    PanelAmsg.Visible = true;
                    PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                    lblAmsg.Text = "Unit has been deleted successfully";
                    lblAmsg.ForeColor = System.Drawing.Color.Green;
                    FillUnitGrid();
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVUnits_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVUnits_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibAdd = (LinkButton)e.Row.FindControl("ibAdd");
                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liUnitName");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibAdd.Visible = this.CanAdd;
                ibEdit.Enabled = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void LnkUnitSave_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;
            if (GVUnits.SelectedIndex == -1)
            {
                result = u.InsertUnits("", hdnDepartmentID.Value, hdnMissionID.Value, txtUnitName.Text.Trim());
            }
            else
            {
                string UnitID = GVUnits.DataKeys[GVUnits.SelectedIndex].Values["UnitID"].ToString();
                result = u.InsertUnits(UnitID, hdnDepartmentID.Value, hdnMissionID.Value, txtUnitName.Text.Trim());
            }

            ClearUnitPopUp();
            FillUnitGrid();
            PanelAmsg.Visible = true;

            if (result == -1)
            {
                PanelAmsg.CssClass = "alert alert-danger alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Red;
                lblAmsg.Text = "Unit under this department with the same name already exists";
            }
            else
            {
                PanelAmsg.CssClass = "alert alert-success alert-dismissable";
                lblAmsg.ForeColor = System.Drawing.Color.Green;
                lblAmsg.Text = "Unit has been added successfully";
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    #endregion
    #region Sub Unit
    private void ClearSubUnitPopUp()
    {
        lblSubUnitmsg.Text = "";
        PanelSubUnitmsg.Visible = false;
        txtSubUnitName.Text = "";
        GVSubUnits.SelectedIndex = -1;
    }
    void FillSubUnitGrid()
    {
        DataTable dt = new DataTable();
        su.GetAllSubUnitByUnitID(hdnUnitID.Value, ref dt);
        lblGVSubUnitCount.Text = dt.Rows.Count.ToString();
        GVSubUnits.DataSource = dt;
        GVSubUnits.DataBind();
    }
    protected void GVSubUnits_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "EditItem")
            {
                ClearSubUnitPopUp();
                GVSubUnits.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                txtSubUnitName.Text = GVSubUnits.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["SubUnitName"].ToString();
            }
            else if (e.CommandName == "DeleteItem")
            {
                DataTable dtCount = new DataTable();
                em.GetStaffNumberUnderDepUnitSubUnit("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", GVSubUnits.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["SubUnitID"].ToString(), ref dtCount);
                if (dtCount.Rows.Count > 0)
                {
                    PanelSubUnitmsg.Visible = true;
                    PanelSubUnitmsg.CssClass = "alert alert-danger alert-dismissable";
                    lblSubUnitmsg.Text = "Sub unit can't be deleted, there are staff members under this sub unit";
                    lblSubUnitmsg.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    su.DeleteSubUnit(GVSubUnits.DataKeys[Convert.ToInt32(e.CommandArgument)].Values["SubUnitID"].ToString());
                    PanelSubUnitmsg.Visible = true;
                    PanelSubUnitmsg.CssClass = "alert alert-success alert-dismissable";
                    lblSubUnitmsg.Text = "Sub unit has been deleted successfully";
                    lblSubUnitmsg.ForeColor = System.Drawing.Color.Green;
                    FillSubUnitGrid();
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVSubUnits_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVSubUnits_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton ibEdit = (LinkButton)e.Row.FindControl("liSubUnitName");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                ibEdit.Enabled = this.CanEdit;
                ibDelete.Visible = this.CanDelete;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void LnkSubUnitSave_Click(object sender, EventArgs e)
    {
        int result = 0;
        if (GVSubUnits.SelectedIndex == -1)
        {
            result = su.InsertSubUnits("", hdnUnitID.Value, hdnDepartmentID.Value, hdnMissionID.Value, txtSubUnitName.Text.Trim());

        }
        else
        {
            string SubUnitID = GVSubUnits.DataKeys[GVSubUnits.SelectedIndex].Values["SubUnitID"].ToString();
            result = su.InsertSubUnits(SubUnitID, hdnUnitID.Value, hdnDepartmentID.Value, hdnMissionID.Value, txtSubUnitName.Text.Trim());

        }

        ClearSubUnitPopUp();
        FillSubUnitGrid();
        PanelSubUnitmsg.Visible = true;

        if (result == -1)
        {
            PanelSubUnitmsg.CssClass = "alert alert-danger alert-dismissable";
            lblSubUnitmsg.ForeColor = System.Drawing.Color.Red;
            lblSubUnitmsg.Text = "Sub Unit under this Unit with the same name already exists";
        }
        else
        {
            PanelSubUnitmsg.CssClass = "alert alert-success alert-dismissable";
            lblSubUnitmsg.ForeColor = System.Drawing.Color.Green;
            lblSubUnitmsg.Text = "Sub unit has been added successfully";
        }



    }
    #endregion

    protected void ddlMissionsName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}