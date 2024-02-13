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

public partial class RadioCheck_NOAWizard_1_NotifiersInformation : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LinkButton lnk = new LinkButton();
                lnk = (LinkButton)WizardHeader.FindControl("lbStep1");
                lnk.CssClass = "btn btn-success btn-circle btn-lg";
                FillDDLs();
                Objects.User ui = (Objects.User)Session["userinfo"];
                if (ui.IsManager)
                {
                    userLists.Visible = false;
                }
                else
                {
                    userLists.Visible = false;
                }
                if (!string.IsNullOrEmpty(Request["MRNO"] as string) && !string.IsNullOrEmpty(Request["MRID"] as string))
                {
                    FillMR(Decrypt(Request["MRID"]));
                    DDLUsers.Enabled = false;
                    DataTable dt = new DataTable();
                    R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dt);
                    if (dt.Rows.Count > 0)
                    {
                        if ((dt.Rows[0]["Code"].ToString() == "NP" || dt.Rows[0]["Code"].ToString() == "NR") && dt.Rows[0]["RequesterID"].ToString() == ui.User_Id)
                        {
                            pnlContent.Enabled = true;
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            pnlContent.Enabled = false;
                            btnSave.Enabled = false;
                        }
                    }
                }
                else
                {
                    txtTravelersFirstName.Text = ui.FirstName;
                    txtTravelerLastName.Text = ui.LastName;
                    txtPRISM.Text = ui.PRISMNumber;
                }
            }
            catch (Exception ex)
            {
                IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            }
        }
    }

    #region MovementRequest
    void FillDDLs()
    {
        try
        {
            Objects.User ui = (Objects.User)Session["userinfo"];
            DataTable dt = new DataTable();
            R.GetStaffMembersByDepartmentID(ref dt);
            DDLUsers.DataSource = dt;
            DDLUsers.DataBind();
            DDLUsers.SelectedValue = ui.User_Id.ToString();
            DataSet ds = new DataSet();
            R.GetAllLookupsList(ref ds);
            ddlLeaveCategory.DataSource = ds.Tables[9];
            ddlLeaveCategory.DataBind();
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll(string MovementRequestID)
    {
        txtTravelersFirstName.Text = "";
        txtTravelerSecondName.Text = "";
        txtTravelerLastName.Text = "";
        txtPurposeOfLeave.Text = "";
        ddlLeaveCategory.SelectedIndex = -1;
    }
    void FillMR(string MovementRequestID)
    {
        DataTable dt = new DataTable();
        R.GetMovementRequestByMovementRequestID(MovementRequestID, ref dt);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                txtPRISM.Text = row["PRISMNumber"].ToString();
                txtTravelersFirstName.Text = row["FirstName"].ToString();
                txtTravelerSecondName.Text = row["MiddleName"].ToString();
                txtTravelerLastName.Text = row["LastName"].ToString();
                txtPurposeOfLeave.Text = row["PurposeOfLeave"].ToString();
                ddlLeaveCategory.SelectedValue = row["LeaveCategoryCode"].ToString();
                DDLUsers.SelectedValue = row["UserID"].ToString();
            }
        }
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("/RadioCheck/StaffNotifications.aspx");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string MovementRequestID = "";
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Request["MRID"] as string))
            {
                MovementRequestID = R.InsertUpdateNotifierInformation(
                Decrypt(Request["MRID"].ToString()),
                DDLUsers.SelectedValue.ToString(),
                txtTravelersFirstName.Text,
                txtTravelerSecondName.Text,
                txtTravelerLastName.Text,
                ddlLeaveCategory.SelectedValue,
                txtPurposeOfLeave.Text,
                txtPRISM.Text
                );
            }
            else
            {
                MovementRequestID = R.InsertUpdateNotifierInformation(
                 "",
                 DDLUsers.SelectedValue.ToString(),
                 txtTravelersFirstName.Text,
                 txtTravelerSecondName.Text,
                 txtTravelerLastName.Text,
                ddlLeaveCategory.SelectedValue,
                txtPurposeOfLeave.Text,
                 txtPRISM.Text
                 );

            }
            string MRNO = "";
            DataTable dtMRNO = new DataTable();
            R.GetMovementRequestByMovementRequestID(MovementRequestID, ref dtMRNO);
            MRNO = dtMRNO.Rows[0]["MovementRequestNumber"].ToString();
            Response.Redirect("~/RadioCheck/NOAWizard/2_AbsenceDates.aspx?MRID=" + Encrypt(MovementRequestID) + "&&MRNO=" + Encrypt(MRNO) + "&First=1", false);
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void DDLUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string UserID = DDLUsers.SelectedValue;
            DataTable dtu = new DataTable();
            R.GetUserInfoByUserID(UserID, ref dtu);
            txtTravelersFirstName.Text = dtu.Rows[0]["FirstName"].ToString();
            txtTravelerLastName.Text = dtu.Rows[0]["LastName"].ToString();
            txtPRISM.Text = dtu.Rows[0]["PRISM_Number"].ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    #endregion

}
