
using Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Staff_SMWizard_1_PersonalInformation : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    HttpContext context = HttpContext.Current;

    protected void Page_Load(object sender, EventArgs e)
    {
        txtUserName.Enabled = this.CanAmend;
        txtPRISMNumber.Enabled = this.CanAmend;

        if (!IsPostBack)
        {
            try
            {
                LinkButton lnk = new LinkButton();
                lnk = (LinkButton)WizardHeader.FindControl("lbStep1");
                lnk.CssClass = "btn btn-success btn-circle btn-lg fa fa-book";

                if (!string.IsNullOrEmpty(Request["PERNO"] as string))
                {
                    GetStaffInformationByPersonalNumber(Decrypt(Request["PERNO"]));
                    FillDDLs();

                }
                else
                {
                    ClearAll();
                    FillDDLs();

                }


            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }

    }

    #region Staff
    void FillDDLs()
    {
        try
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            DataTable dt = new DataTable();
            DataSet dg = new DataSet();
            DataSet ds = new DataSet();
            DataSet dsU = new DataSet();
            DataSet dsSu = new DataSet();
            R.GetAllLookupsList(ref ds);
            ddlCountry.DataSource = ds.Tables[0];
            ddlCountry.DataBind();

            R.GetAllLookupsList(ref ds);
            ddlGender.DataSource = ds.Tables[10];
            ddlGender.DataBind();

            //Load Locations
            R.GetRadioOperatorLocations(ref dt,ui.User_Id.ToString());
            ddlLocation.DataSource = ds.Tables[1];
            ddlLocation.DataBind();

            //Load departmnets
            ddlDepartment.DataSource = ds.Tables[2];
            ddlDepartment.DataBind();

            //Load Units
            R.GetUnitsByDepartmentID(ddlDepartment.SelectedValue, ref dsU);
            ddlUnit.DataSource = dsU.Tables[0];
            ddlUnit.DataBind();
            ddlUnit.Enabled = dsU.Tables[0].Rows.Count > 0;

            //Load SubUnits
            R.GetSubUnitsByUnitID(ddlUnit.SelectedValue, ref dsSu);
            ddlSubUnit.DataSource = dsSu.Tables[0];
            ddlSubUnit.DataBind();
            ddlSubUnit.Enabled = dsSu.Tables[0].Rows.Count > 0;

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillUnitDDLs()
    {
        try
        {
            //Load Units
            DataSet dsU = new DataSet();
            R.GetUnitsByDepartmentID(ddlDepartment.SelectedValue, ref dsU);
            ddlUnit.DataSource = dsU.Tables[0];
            ddlUnit.DataBind();
            ddlUnit.Enabled = dsU.Tables[0].Rows.Count > 0;

            //Load SubUnits
            DataSet dsSu = new DataSet();
            R.GetSubUnitsByUnitID(ddlUnit.SelectedValue, ref dsSu);
            ddlSubUnit.DataSource = dsSu.Tables[0];
            ddlSubUnit.DataBind();
            ddlSubUnit.Enabled = dsSu.Tables[0].Rows.Count > 0;

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillSubUnitDDLs()
    {
        try
        {
            DataSet dsSu = new DataSet();
            R.GetSubUnitsByUnitID(ddlUnit.SelectedValue, ref dsSu);
            ddlSubUnit.DataSource = dsSu.Tables[0];
            ddlSubUnit.DataBind();
            ddlSubUnit.Enabled = dsSu.Tables[0].Rows.Count > 0;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


    void ClearAll()
    {
        //txtFirstName.Text = "";
        txtDisplayName.Text = "";
        txtPRISMNumber.Text = "";
        txtUserName.Text = "";
        txtUserName.Text = "";
        ddlCountry.SelectedIndex = -1;
        ddlLocation.SelectedIndex = -1;
        ddlUnit.SelectedIndex = -1;
        ddlSubUnit.SelectedIndex = -1;
    }
    void GetStaffInformationByPersonalNumber(string PERNO)
    {
        DataTable dt = new DataTable();
        R.GetStaffInformationByPersonalNumber(PERNO, ref dt);
        foreach (DataRow row in dt.Rows)
        {
            txtUserName.Text= row["UserName"].ToString();
            txtPRISMNumber.Text = row["PRISMNumber"].ToString();
            txtDisplayName.Text = row["DisplayName"].ToString();
            ddlLocation.SelectedValue = row["LocationID"].ToString();
            ddlDepartment.SelectedValue = row["DepartmentID"].ToString();
            ddlGender.SelectedValue = row["GenderID"].ToString();
            ddlUnit.SelectedValue = row["UnitID"].ToString();
            ddlSubUnit.SelectedValue = row["SubUnitID"].ToString();
            txtCallSign.Text = row["CallSign"].ToString();
            ddlCountry.SelectedValue = row["CountryID"].ToString();
            txtPRISMNumber.Enabled = false;
            //txtFirstName.Enabled = false;
            txtDisplayName.Enabled = false;
            ddlLocation.Enabled = false;
            ddlUnit.Enabled = false;
            ddlSubUnit.Enabled = false;
            //ddlCountry.Enabled = false;
        }
    }
    protected void BtnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Staff/StaffManagement.aspx");
    }
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtUserName.Text))
            {
                DataTable dt = new DataTable();
                R.GetRegisteredStaffInformationByUserID(txtUserName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    txtUserName.Text = dt.Rows[0]["UserName"].ToString();
                    txtPRISMNumber.Text = dt.Rows[0]["PRISMNumber"].ToString();
                    txtDisplayName.Text = dt.Rows[0]["DisplayName"].ToString();
                    txtCallSign.Text = dt.Rows[0]["CallSign"].ToString();
                    ddlLocation.SelectedValue= dt.Rows[0]["LocationID"].ToString();
                    ddlCountry.SelectedValue= dt.Rows[0]["CountryID"].ToString();
                    ddlGender.SelectedValue = dt.Rows[0]["GenderID"].ToString();
                    ddlDepartment.SelectedValue = dt.Rows[0]["DepartmentID"].ToString();
                    ddlUnit.SelectedValue = dt.Rows[0]["UnitID"].ToString();
                    ddlSubUnit.SelectedValue = dt.Rows[0]["SubUnitID"].ToString();
                    txtPRISMNumber.Enabled = false;
                    txtDisplayName.Enabled = false;
                    FillDDLs();
                }
                else
                {
                    DataTable dtt = new DataTable();
                    R.GetStaffInformationByUserIDForRegistration(txtUserName.Text.Trim(), ref dtt);
                    if (dtt.Rows.Count > 0)
                    {
                        txtUserName.Text = dtt.Rows[0]["UserName"].ToString();
                        txtPRISMNumber.Text = dtt.Rows[0]["PRISMNumber"].ToString();
                        txtDisplayName.Text = dtt.Rows[0]["DisplayName"].ToString();
                        ddlCountry.SelectedValue = dtt.Rows[0]["CountryID"].ToString();
                        ddlLocation.SelectedValue = dtt.Rows[0]["LocationID"].ToString();
                        ddlDepartment.SelectedValue = dtt.Rows[0]["DepartmentID"].ToString();
                        ddlUnit.SelectedValue = dtt.Rows[0]["UnitID"].ToString();
                        ddlSubUnit.SelectedValue = dtt.Rows[0]["SubUnitID"].ToString();
                        ddlGender.SelectedValue = dtt.Rows[0]["GenderID"].ToString();
                        txtPRISMNumber.Enabled = false;
                        txtDisplayName.Enabled = false;
                        //txtLastName.Enabled = false;
                        //ddlCountry.Enabled = false;
                        //FillDDLs();

                    }
                    else
                    {
                        txtUserName.Text = "";
                    }
                }
            }
            else
            {
                ClearAll();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUnitDDLs();
    }

    protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSubUnitDDLs();
    }

  
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            string PERNO = string.Empty;
            
            if (!string.IsNullOrEmpty(Request["PERNO"] as string))
            {
                PERNO = R.InsertUpdateStaffInformation(Decrypt(Request["PERNO"].ToString()), txtUserName.Text, txtDisplayName.Text, ddlDepartment.SelectedValue, ddlUnit.SelectedValue, ddlSubUnit.SelectedValue, ddlLocation.SelectedValue, ddlCountry.SelectedValue, txtCallSign.Text,ddlGender.SelectedValue, true);
            }
            else
            {
                PERNO = R.InsertUpdateStaffInformation(txtPRISMNumber.Text, txtUserName.Text, txtDisplayName.Text, ddlDepartment.SelectedValue, ddlUnit.SelectedValue, ddlSubUnit.SelectedValue, ddlLocation.SelectedValue, ddlCountry.SelectedValue, txtCallSign.Text,ddlGender.SelectedValue, true);
            }

            if (String.IsNullOrEmpty(PERNO))
            {
                //Error Message
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Failed to Save Information. Please contact your administrator";
            }
            else
            {
                //Redirect
                Response.Redirect("~/Staff/SMWizard/2_Contacts.aspx?PERNO=" +Encrypt(PERNO) + "&First=1", false);
            }
            
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    #endregion

}

