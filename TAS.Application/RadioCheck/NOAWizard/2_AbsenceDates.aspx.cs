using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RadioCheck_NOAWizard_2_AbsenceDates : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    //Business.Security s = new Business.Security();
    //Business.Media Media = new Business.Media();
    Globals g = new Globals();

    protected void Page_Load(object sender, EventArgs e)
    {
        Objects.User ui = (Objects.User)Session["userinfo"];
        if (!IsPostBack)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl MRStatus = null;
            MRStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("NOAStatusDiv");
            MRStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep2");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            if (!string.IsNullOrEmpty(Request["MRID"] as string))
            {
                string MovementRequestID = Decrypt(Request["MRID"].ToString());
                FillMR(MovementRequestID);
                DataTable dt = new DataTable();
                R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RequesterID"].ToString().ToUpper() == ui.User_Id.ToUpper() && (dt.Rows[0]["Code"].ToString().ToUpper() == "NP" || dt.Rows[0]["Code"].ToString().ToUpper() == "NR"))
                    {
                        pnlContent1.Enabled = true;
                        pnlContent2.Enabled = true;
                    }
                    else
                    {
                        pnlContent1.Enabled = false;
                        pnlContent2.Enabled = false;
                    }

                }
            }
        }
    }

    //void ClearAll(string MovementRequestID)
    //{
    //    CheckTravellingOut.Checked = false;
    //}

    void FillMR(string MovementRequestID)
    {
        DataTable dt = new DataTable();
        R.GetMovementRequestByMovementRequestID(MovementRequestID, ref dt);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                //CheckTravellingOut.Checked = Convert.ToBoolean(row["TravellingOut"]);
                txtStartDate.Text = row["StartDate"].ToString();
                txtEndDate.Text = row["EndDate"].ToString();
                txtAddresswhileAbsent.Text = row["AddressWhileAbsent"].ToString();
                checkConfirm.Checked = Convert.ToBoolean(row["ConfirmBySupervisor"]);
            }
        }
       
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime? StartDate;
            DateTime? EndDate;
            if (String.IsNullOrEmpty(txtStartDate.Text) || String.IsNullOrEmpty(txtEndDate.Text))
            {
                if (String.IsNullOrEmpty(txtStartDate.Text) && String.IsNullOrEmpty(txtEndDate.Text))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    txtStartDate.Style.Add("border-style", "solid !important");
                    txtStartDate.Style.Add("border-color", "#FF0000 !important");
                    txtEndDate.Style.Add("border-style", "solid !important");
                    txtEndDate.Style.Add("border-color", "#FF0000 !important");
                    lblmsg.Text = "Please select leave start and end dates";
                    PanelMessage.Focus();
                    return;
                }
                else if (String.IsNullOrEmpty(txtStartDate.Text))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    txtStartDate.Style.Add("border-style", "solid !important");
                    txtStartDate.Style.Add("border-color", "#FF0000 !important");
                    lblmsg.Text = "Please select leave start date";
                    PanelMessage.Focus();
                    return;
                }
                else if (String.IsNullOrEmpty(txtEndDate.Text))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    txtEndDate.Style.Add("border-style", "solid !important");
                    txtEndDate.Style.Add("border-color", "#FF0000 !important");
                    lblmsg.Text = "Please select leave end date";
                    PanelMessage.Focus();
                    return;
                }
                else
                {
                    //Check date Conditions
                    StartDate = Convert.ToDateTime(txtStartDate.Text);
                    EndDate = Convert.ToDateTime(txtEndDate.Text);
                    if (EndDate < StartDate)
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Error - Leave end date must be greater than start date";
                        PanelMessage.Focus();
                        return;
                    }

                    if (g.CheckPastDate(StartDate))
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Error -  Leave start date cannot be in the past";
                        PanelMessage.Focus();
                        return;
                    }

                    if (g.CheckPastDate(EndDate))
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Error -  Leave end date cannot be in the past";
                        PanelMessage.Focus();
                        return;
                    }
                    //Check date Conditions
                }

            }
            else
            {
                StartDate = Convert.ToDateTime(txtStartDate.Text);
                EndDate = Convert.ToDateTime(txtEndDate.Text);
                if (String.IsNullOrEmpty(txtAddresswhileAbsent.Text))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    txtAddresswhileAbsent.Style.Add("border-style", "solid !important");
                    txtAddresswhileAbsent.Style.Add("border-color", "#FF0000 !important");
                    lblmsg.Text = "Please enter Address while absent";
                    PanelMessage.Focus();
                    return;
                }

                if (checkConfirm.Checked)
                {
                    checkConfirm.CssClass = "checkbox-inline";
                }
                else
                {
                    checkConfirm.Style.Add("border-style", "solid !important");
                    checkConfirm.Style.Add("border-color", "#FF0000 !important");
                    return;
                }



                if (!string.IsNullOrEmpty(Request["MRID"] as string))
                {
                    string UserHasDuplicateNotification = R.UserHasDuplicateNotification(Decrypt(Request["MRID"]),StartDate, EndDate);
                    if (UserHasDuplicateNotification == "Yes")
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        txtStartDate.Style.Add("border-style", "solid !important");
                        txtStartDate.Style.Add("border-color", "#FF0000 !important");
                        txtEndDate.Style.Add("border-style", "solid !important");
                        txtEndDate.Style.Add("border-color", "#FF0000 !important");
                        lblmsg.Text = "Sorry, Notification already exists for Specified Period";
                        PanelMessage.Focus();
                        return;
                    }
                    else
                    {
                    R.UpdateMovementRequestDetails(
                        Decrypt(Request["MRID"]),
                        StartDate,
                        EndDate,
                        txtAddresswhileAbsent.Text,
                        false,
                        //CheckTravellingOut.Checked,
                        checkConfirm.Checked);
                    Response.Redirect("~/RadioCheck/NOAWizard/3_AbsenceDetails.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
                }
            }

            }

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

}