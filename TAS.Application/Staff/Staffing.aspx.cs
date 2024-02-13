using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Staff_Staffing : AuthenticatedPageClass
{
    Business.Security s = new Business.Security();
    Business.Employees em = new Business.Employees();
    Business.Units u = new Business.Units();
    Business.SubUnits su = new Business.SubUnits();
    Business.Departments de = new Business.Departments();
    Business.Missions m = new Business.Missions();
    Business.AMS AMS = new Business.AMS();
    protected void Page_Load(object sender, EventArgs e)
    {
        GVStaffManagement.PreRender += new EventHandler(GVStaffManagement_PreRender);
        pnlMessageEditStaff.Visible = false;
        PanelMessageStaff.Visible = false;
        if (!IsPostBack)
        {
            LnkAddNewItem.Enabled = this.CanAdd;
            lnkbtnEditStaffSave.Enabled = this.CanAdd;
            FillDDl();
            DDLDepartment_SelectedIndexChanged(this, EventArgs.Empty);
        }
    }
    void GVStaffManagement_PreRender(object sender, EventArgs e)
    {
        if (GVStaffManagement.Rows.Count > 0)
        {
            GVStaffManagement.UseAccessibleHeader = true;
            GVStaffManagement.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    protected void GVStaffManagement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton lbMove = (LinkButton)e.Row.FindControl("lbMove");
                LinkButton lbUserName = (LinkButton)e.Row.FindControl("lbUserName");
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");

                LinkButton ibIsManager = (LinkButton)e.Row.FindControl("ibIsManager");
                LinkButton ibIsInternational = (LinkButton)e.Row.FindControl("ibIsInternational");



                if (!Convert.ToBoolean(GVStaffManagement.DataKeys[e.Row.RowIndex].Values["IsManager"]))
                {
                    ibIsManager.CssClass = "fa fa-times";
                    ibIsManager.ForeColor = System.Drawing.Color.Red;
                    ibIsManager.ToolTip = "Make Manager";
                }
                else
                {
                    ibIsManager.CssClass = "fa fa-check";
                    ibIsManager.ToolTip = "Remove Manager";
                }
                if (!Convert.ToBoolean(GVStaffManagement.DataKeys[e.Row.RowIndex].Values["IsInternationalStaff"]))
                {
                    ibIsInternational.CssClass = "fa fa-times";
                    ibIsInternational.ForeColor = System.Drawing.Color.Red;
                    ibIsInternational.ToolTip = "Make International";
                }
                else
                {
                    ibIsInternational.CssClass = "fa fa-check";
                    ibIsInternational.ToolTip = "Remove International";
                }

                ibIsInternational.Enabled = this.CanEdit;
                ibIsManager.Enabled = this.CanEdit;
                lbUserName.Enabled = this.CanEdit;
                lbMove.Visible = this.CanEdit;
                ibDelete.Visible = this.CanDelete;


            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    private void FillDDl()
    {
        try
        {
            DataTable dtl = new DataTable();
            m.GetAllMissions(ref dtl);
            ddlMissionsName.DataSource = dtl;
            ddlMissionsName.DataBind();

            DataTable dtLoc = new DataTable();
            m.GetAllLocationsByMissionID(ddlMissionsName.SelectedValue, ref dtLoc);
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataBind();
            ddlLocationNew.DataSource = dtLoc;
            ddlLocationNew.DataBind();
            DDLLocationMove.DataSource = dtLoc;
            DDLLocationMove.DataBind();

            DDLDepartment.Items.Add(new ListItem("UNASSIGNED", "00000000-0000-0000-0000-000000000001"));

            DataTable dt = new DataTable();
            de.GetAllDepartmentByMissionID(ddlMissionsName.SelectedValue, ref dt);
            DDLDepartment.DataSource = dt;
            DDLDepartment.DataTextField = "DeptName";
            DDLDepartment.DataValueField = "DepartmentID";
            DDLDepartment.DataBind();
            DDLDepartment.SelectedIndex = 1;


            ddlNationality.Items.Add(new ListItem("Please Select", "00000000-0000-0000-0000-000000000001"));
            DataTable dlt = new DataTable();
            de.GetAllCountries(ref dlt);
            ddlNationality.DataSource = dlt;
            ddlNationality.DataTextField = "CountryDescription";
            ddlNationality.DataValueField = "CountryDescription";
            ddlNationality.DataBind();
            ddlNationality.SelectedIndex = -1;

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void DDLDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStaffList();
    }
    protected void FillStaffList()
    {
        try
        {
            DataTable dt = new DataTable();
            em.GetStaffsByDepartmentID(DDLDepartment.SelectedValue, ddlLocation.SelectedValue, ref dt);
            GVStaffManagement.DataSource = dt;
            GVStaffManagement.DataBind();
            lblGVStaffManagementCount.Text = dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVStaffManagement_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            PanelMessage.Visible = false;
            lblmsg.Text = "";

            if (e.CommandName == "Move")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openStaffModal('" + GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DisplayName"].ToString() + "');", true);
                hdnEmployeeID.Value = "";
                hdnPRIMSNumber.Value = "";
                hdnEmployeeID.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserID"].ToString();
                hdnPRIMSNumber.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["PRISM_Number"].ToString();

                lblEmployeeNameMove.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DisplayName"].ToString();
                FillStaffMovementDDl();
            }
            else if (e.CommandName == "EditItem")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEditStaffModal();", true);
                hdnEmployeeID.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserID"].ToString();
                hdnDepartmentID.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DepartmentID"].ToString();
                hdnUnitID.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UnitID"].ToString();
                hdnNationality.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["Country"].ToString();
                hdnSubUnitID.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["SubUnitID"].ToString();
                ddlLocationNew.SelectedValue = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["LocationID"].ToString();
                FillStaffEdittDDl();
                if (hdnDepartmentID.Value == "")
                {
                    ddlStaffDepartment.SelectedValue = "00000000-0000-0000-0000-000000000000";
                    ddlStaffDepartment.Enabled = true;
                }
                else
                {
                    ddlStaffDepartment.SelectedValue = hdnDepartmentID.Value;
                    ddlStaffDepartment.Enabled = false;
                }
                ddlLocationNew.Enabled = false;
                ddlStaffDepartment_SelectedIndexChanged(this, EventArgs.Empty);
                ddlStaffUnit.SelectedValue = hdnUnitID.Value == "" ? "00000000-0000-0000-0000-000000000000" : hdnUnitID.Value;
                ddlStaffUnit_SelectedIndexChanged(this, EventArgs.Empty);
                ddlStaffSubUnit.SelectedValue = hdnSubUnitID.Value == "" ? "00000000-0000-0000-0000-000000000000" : hdnSubUnitID.Value;
                ddlStaffUnit.Enabled = ddlStaffSubUnit.Enabled = false;
                lnkbtnEditStaffClear.Visible = false;
                txtFirstName.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["FirstName"].ToString();
                txtLastName.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["LastName"].ToString();
                txtDisplayName.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DisplayName"].ToString();
                txtTitle.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["Title"].ToString();
                txtUNIDNo.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UNID"].ToString();

                txtUserName.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserName"].ToString();
                txtEmail.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["Email"].ToString();
                txtPRISMNumber.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["PRISM_Number"].ToString();
                lblEditStaff.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DisplayName"].ToString();
                rdMale.Checked = Convert.ToBoolean(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["Gender"].ToString() == "Male");
                rdFemale.Checked = Convert.ToBoolean(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["Gender"].ToString() == "Female");

                ddlNationality.SelectedValue = hdnNationality.Value == "" ? "00000000-0000-0000-0000-000000000000" : hdnNationality.Value;

                //rdNational.Checked = !Convert.ToBoolean(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsInternationalStaff"].ToString());
                //rdInternational.Checked = Convert.ToBoolean(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsInternationalStaff"].ToString());
                //txtUNIDNo.Text = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UNID"].ToString();


                lnkbtnEditStaffSave.Enabled = this.CanEdit;
                lblMoidficationType.Text = "Edit ";
                //FillStaffMovementDDl();
            }
            else if (e.CommandName == "Manager")
            {
                LinkButton ibIsManager = (LinkButton)GVStaffManagement.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("ibIsManager");
                string EmployeeID = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserID"].ToString();
                string BadgeNumber = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["PRISM_Number"].ToString();
                string DepID = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["DepartmentID"].ToString();
                string UnitID = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UnitID"].ToString();
                string SubUnitID = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["SubUnitID"].ToString();
                if (!Convert.ToBoolean(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsManager"]))
                {
                    //update AD database
                    em.UpdateUserIsManagerByUserID(EmployeeID, true);

                    if (CheckAMS.Checked)
                    {
                        AMS.InsertManagerAssignment("", BadgeNumber, DepID, UnitID, SubUnitID);
                    }
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-success alert-dismissable";
                    lblmsg.ForeColor = System.Drawing.Color.Green;
                    lblmsg.Text = "Is Manager has been updated successfully";

                }
                else
                {
                    //update AD database
                    em.UpdateUserIsManagerByUserID(EmployeeID, false);

                    if (CheckAMS.Checked)
                    {
                        AMS.DeleteManagerAssignmentByPRISMNumber(BadgeNumber, DepID, UnitID, SubUnitID);
                    }
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-success alert-dismissable";
                    lblmsg.ForeColor = System.Drawing.Color.Green;
                    lblmsg.Text = "Is Manager has been updated successfully";

                }
                DDLDepartment_SelectedIndexChanged(DDLDepartment, EventArgs.Empty);
            }
            else if (e.CommandName == "International")
            {
                lblmsg.Text = "";
                PanelMessage.Visible = false;

                //update AD database
                string EmployeeID = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserID"].ToString();
                em.UpdateUserIsInternationalByUserID(EmployeeID, !Convert.ToBoolean(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsInternationalStaff"]));

                if (CheckAMS.Checked)
                {
                    AMS.UpdateEmployeeInfoIsInternationalStaffByPRISMNumber(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["PRISM_Number"].ToString(), !Convert.ToBoolean(GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["IsInternationalStaff"]));
                }

                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                lblmsg.Text = "Is International has been updated successfully";
                DDLDepartment_SelectedIndexChanged(DDLDepartment, EventArgs.Empty);
            }
            else if (e.CommandName == "deleteItem")
            {
                em.DeleteStaff(hdnEmployeeID.Value = GVStaffManagement.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["UserID"].ToString());
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = System.Drawing.Color.Green;
                lblmsg.Text = "Staff has been delete successfully";
                FillStaffList();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    #region StaffMovement
    private void FillStaffMovementDDl()
    {
        try
        {

            DataTable dt = new DataTable();
            de.GetAllDepartmentByMissionID(ddlMissionsName.SelectedValue, ref dt);
            DDLLocationMove.SelectedIndex = -1;
            //Move Department DDL
            ddlMoveDepartment.Items.Clear();
            ddlMoveUnit.Items.Clear();
            ddlMoveSubUnit.Items.Clear();

            ddlMoveDepartment.Items.Add(new ListItem("-- Select Department --", "00000000-0000-0000-0000-000000000000"));
            ddlMoveDepartment.DataSource = dt;
            ddlMoveDepartment.DataTextField = "DeptName";
            ddlMoveDepartment.DataValueField = "DepartmentID";
            ddlMoveDepartment.DataBind();
            ddlMoveDepartment.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void lnkbtnMoveSave_Click(object sender, EventArgs e)
    {
        em.MoveStaffMember(ddlMoveDepartment.SelectedValue, ddlMoveUnit.SelectedValue, ddlMoveSubUnit.SelectedValue, hdnEmployeeID.Value, DDLLocationMove.SelectedValue);

        if (CheckAMS.Checked)
        {
            AMS.MoveStaffMemberByPRISMNumber(ddlMoveDepartment.SelectedValue, ddlMoveUnit.SelectedValue, ddlMoveSubUnit.SelectedValue, hdnPRIMSNumber.Value);
        }

        PanelMessageStaff.Visible = true;
        PanelMessageStaff.CssClass = "alert alert-success alert-dismissable";
        lblmsgStaff.ForeColor = System.Drawing.Color.Green;
        lblmsgStaff.Text = "User has been updated successfully";

        ddlMoveDepartment.SelectedIndex = -1;
        ddlMoveUnit.Items.Clear();
        ddlMoveSubUnit.Items.Clear();
        DDLDepartment_SelectedIndexChanged(this, EventArgs.Empty);
    }
    protected void lnkbtnMoveClear_Click(object sender, EventArgs e)
    {
        PanelMessageStaff.Visible = false;
        lblmsgStaff.Text = "";
        ddlMoveDepartment.SelectedIndex = -1;
        ddlMoveUnit.Items.Clear();
        ddlMoveSubUnit.Items.Clear();
    }
    protected void ddlMoveDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlMoveUnit.Items.Clear();
            ddlMoveSubUnit.Items.Clear();
            //Units DDL
            ddlMoveUnit.Items.Add(new ListItem("-- Select Unit --", "00000000-0000-0000-0000-000000000000"));
            DataTable dt = new DataTable();
            u.GetAllUnitsbyDepID(ddlMoveDepartment.SelectedValue, ref dt);
            ddlMoveUnit.DataSource = dt;
            ddlMoveUnit.DataTextField = "UnitName";
            ddlMoveUnit.DataValueField = "UnitID";
            ddlMoveUnit.DataBind();
            ddlMoveUnit.SelectedIndex = 0;
            if (ddlMoveUnit.SelectedIndex != 0)
            {
                ddlMoveSubUnit.Items.Add(new ListItem("-- Select Sub Unit --", "00000000-0000-0000-0000-000000000000"));
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ddlMoveUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Sub Units DDL
            ddlMoveSubUnit.Items.Clear();
            //SubUnit DDL
            ddlMoveSubUnit.Items.Add(new ListItem("-- Select Sub Unit --", "00000000-0000-0000-0000-000000000000"));
            DataTable dtm = new DataTable();
            su.GetAllSubUnitByUnitID(ddlMoveUnit.SelectedValue, ref dtm);
            ddlMoveSubUnit.DataSource = dtm;
            ddlMoveSubUnit.DataTextField = "SubUnitName";
            ddlMoveSubUnit.DataValueField = "SubUnitID";
            ddlMoveSubUnit.DataBind();
            ddlMoveSubUnit.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    #endregion

    #region StaffEdit
    protected void lnkbtnEditStaffSave_Click(object sender, EventArgs e)
    {
        try
        {
            pnlMessageEditStaff.Visible = true;
            string Gender = string.Empty;
            bool IsInternationstaff = false;

            if (ddlNationality.Text != "South sudan")
            {
                IsInternationstaff = true;
            }

            if (rdMale.Checked)
            {
                Gender = "Male";
            }
            else if (rdFemale.Checked)
            {
                Gender = "Female";
            }
            else
            {
                Gender = "";
            }

            if (String.IsNullOrEmpty(Gender))
            {
                rdMale.CssClass = "alert alert-danger alert-dismissable";
                lblMessageEditStaff.ForeColor = System.Drawing.Color.Red;
                lblMessageEditStaff.Text = "Please Select Gender";
            }

            else
            {
                int status = em.UpdateStaffInfo(hdnEmployeeID.Value,
                    txtUserName.Text,
                    txtDisplayName.Text,
                    txtPRISMNumber.Text,
                    txtEmail.Text,
                    ddlStaffDepartment.SelectedValue,
                    ddlStaffUnit.SelectedValue,
                    ddlStaffSubUnit.SelectedValue,
                    ddlMissionsName.SelectedValue,
                    ddlLocationNew.SelectedValue,
                    Gender,
                    txtUNIDNo.Text, txtFirstName.Text,
                    txtLastName.Text,
                    ddlNationality.Text, txtTitle.Text,
                    IsInternationstaff);
                if (status == -1)
                {
                    pnlMessageEditStaff.CssClass = "alert alert-danger alert-dismissable";
                    lblMessageEditStaff.ForeColor = System.Drawing.Color.Red;
                    lblMessageEditStaff.Text = "Staff with the same user name, email or PRISM Number already exists";
                }
                else if (status == 1)
                {
                    pnlMessageEditStaff.CssClass = "alert alert-success alert-dismissable";
                    lblMessageEditStaff.ForeColor = System.Drawing.Color.Green;
                    lblMessageEditStaff.Text = "Staff info has been updated successfully!";
                    ClearStaffInfo();
                    FillStaffList();
                }
                else
                {
                    pnlMessageEditStaff.CssClass = "alert alert-danger alert-dismissable";
                    lblMessageEditStaff.ForeColor = System.Drawing.Color.Red;
                    lblMessageEditStaff.Text = "Sorry, Staff info update failed, please try again";
                }

            }


        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());

        }

    }
    protected void LnkAddNewItem_Click(object sender, EventArgs e)
    {
        ClearStaffInfo();
        FillStaffEdittDDl();
        ddlLocationNew.Enabled = ddlStaffDepartment.Enabled = ddlStaffUnit.Enabled = ddlStaffSubUnit.Enabled = true;
        lnkbtnEditStaffClear.Visible = true;
        lblMoidficationType.Text = "Add ";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openEditStaffModal('');", true);

    }
    protected void lnkbtnEditStaffClear_Click(object sender, EventArgs e)
    {
        ClearStaffInfo();
        FillStaffEdittDDl();

    }
    private void ClearStaffInfo()
    {
        txtUserName.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtDisplayName.Text = "";
        txtPRISMNumber.Text = "";
        txtUNIDNo.Text = "";
        txtEmail.Text = "";
        txtTitle.Text = "";
        ddlStaffDepartment.Items.Clear();
        ddlStaffUnit.Items.Clear();
        ddlStaffSubUnit.Items.Clear();
        ddlLocationNew.SelectedIndex = -1;
        ddlNationality.SelectedIndex = -1;
        hdnEmployeeID.Value = "";
        lblEditStaff.Text = "";
        rdMale.Checked = false;
        rdFemale.Checked = false;
    }
    private void FillStaffEdittDDl()
    {
        try
        {

            DataTable dt = new DataTable();
            de.GetAllDepartmentByMissionID(ddlMissionsName.SelectedValue, ref dt);

            //Move Department DDL
            ddlStaffDepartment.Items.Clear();
            ddlMoveUnit.Items.Clear();
            ddlMoveSubUnit.Items.Clear();

            ddlStaffDepartment.Items.Add(new ListItem("-- Select Department --", "00000000-0000-0000-0000-000000000000"));
            ddlStaffDepartment.DataSource = dt;
            ddlStaffDepartment.DataTextField = "DeptName";
            ddlStaffDepartment.DataValueField = "DepartmentID";
            ddlStaffDepartment.DataBind();
            ddlStaffDepartment.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ddlStaffDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlStaffUnit.Items.Clear();
            ddlStaffSubUnit.Items.Clear();
            //Units DDL
            ddlStaffUnit.Items.Add(new ListItem("-- Select Unit --", "00000000-0000-0000-0000-000000000000"));
            DataTable dt = new DataTable();
            u.GetAllUnitsbyDepID(ddlStaffDepartment.SelectedValue, ref dt);
            ddlStaffUnit.DataSource = dt;
            ddlStaffUnit.DataTextField = "UnitName";
            ddlStaffUnit.DataValueField = "UnitID";
            ddlStaffUnit.DataBind();
            ddlStaffUnit.SelectedIndex = 0;
            if (ddlStaffUnit.SelectedIndex != 0)
            {
                ddlStaffSubUnit.Items.Add(new ListItem("-- Select Sub Unit --", "00000000-0000-0000-0000-000000000000"));
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ddlStaffUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Sub Units DDL
            ddlStaffSubUnit.Items.Clear();
            //SubUnit DDL
            ddlStaffSubUnit.Items.Add(new ListItem("-- Select Sub Unit --", "00000000-0000-0000-0000-000000000000"));
            DataTable dtm = new DataTable();
            su.GetAllSubUnitByUnitID(ddlStaffUnit.SelectedValue, ref dtm);
            ddlStaffSubUnit.DataSource = dtm;
            ddlStaffSubUnit.DataTextField = "SubUnitName";
            ddlStaffSubUnit.DataValueField = "SubUnitID";
            ddlStaffSubUnit.DataBind();
            ddlStaffSubUnit.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Text != "")
            {
                DataTable dt = new DataTable();
                m.SearchUsersForIMTools(txtUserName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    txtUserName.Text = dt.Rows[0]["UserName"].ToString();
                    txtFirstName.Text = dt.Rows[0]["FirstName"].ToString();
                    txtLastName.Text = dt.Rows[0]["LastName"].ToString();
                    txtDisplayName.Text = dt.Rows[0]["DisplayName"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();
                    txtTitle.Text = dt.Rows[0]["Title"].ToString();
                    ddlNationality.SelectedValue = dt.Rows[0]["Country"].ToString();
                    txtPRISMNumber.Text = dt.Rows[0]["PRISM_Number"].ToString();
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    txtEmail.Enabled = false;
                }
                else
                {
                    pnlMessageEditStaff.CssClass = "alert alert-danger alert-dismissable";
                    lblMessageEditStaff.ForeColor = System.Drawing.Color.Red;
                    lblMessageEditStaff.Text = "Un registered staff with specified User name not found";
                    pnlMessageEditStaff.Visible = true;
                    lblMessageEditStaff.Visible = true;
                    ClearStaffInfo();
                    return;
                }
            }
            else
            {
                pnlMessageEditStaff.CssClass = "alert alert-danger alert-dismissable";
                lblMessageEditStaff.ForeColor = System.Drawing.Color.Red;
                lblMessageEditStaff.Text = "Please enter UserName";
                pnlMessageEditStaff.Visible = true;
                lblMessageEditStaff.Visible = true;
                ClearStaffInfo();
                return;
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    #endregion
}