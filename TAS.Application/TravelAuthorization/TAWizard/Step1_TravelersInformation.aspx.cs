using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TravelAuthorization_TAWizard_Step1_TravelersInformation : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.Users u = new Business.Users();
    Business.Security Sec = new Business.Security();
    Globals g = new Globals();

    protected void Page_Load(object sender, EventArgs e)
    {
        txtTravelersFirstName.Enabled = this.CanAmend;
        txtTravelerLastName.Enabled = this.CanAmend;
        txtPRISM.Enabled = this.CanAmend;
        CalendarExtender2.StartDate = DateTime.Now;
        CalendarExtender3.StartDate = DateTime.Now;

        if (!IsPostBack)
        {
            try
            {
                System.Web.UI.HtmlControls.HtmlGenericControl TAStatus = null;
                TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
                TAStatus.Visible = this.CanAmend;

                LinkButton lnk = new LinkButton();
                lnk = (LinkButton)WizardHeader.FindControl("lbStep1");
                lnk.CssClass = "btn btn-success btn-circle btn-lg";

                Objects.User ui = (Objects.User)Session["userinfo"];
                hdnstaffcategory.Value = ui.IsInternationalStaff == true ? "1" : "0";
                FillDDLs();

                if (ui.IsManager || ui.IsEmergencyTACreator)
                {
                    userLists.Visible = true;
                }
                if (ui.IsEmergencyTACreator)
                {
                    checkIsEmergency.Visible = true;
                }

                if (!string.IsNullOrEmpty(Request["TANO"] as string) && !string.IsNullOrEmpty(Request["TAID"] as string))
                {
                    FillTA(Decrypt(Request["TAID"]));
                    DDLUsers.Enabled = false;
                    if (rdYAnnual.Checked)
                    {
                        txtPrivateStayDateFrom.Enabled = true;
                        txtPrivateStayDateTo.Enabled = true;
                    }

                    //lock content
                    DataTable dt = new DataTable();
                    t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                    if (dt.Rows.Count > 0)
                    {
                        //if (dt.Rows[0]["StatusCode"].ToString() != "PEN" && dt.Rows[0]["StatusCode"].ToString() != "INC" && dt.Rows[0]["StatusCode"].ToString() != "RBS" && dt.Rows[0]["StatusCode"].ToString() != "RBHR" && dt.Rows[0]["StatusCode"].ToString() != "RBHOSO" && dt.Rows[0]["StatusCode"].ToString() != "RBRMO" && dt.Rows[0]["StatusCode"].ToString() != "RBHOO" && dt.Rows[0]["StatusCode"].ToString() != "RBCOM")
                        //{
                        //    pnlContent.Enabled = false;
                        //}

                        if (Convert.ToBoolean(dt.Rows[0]["IsEditable"].ToString()) == false)
                        {
                            pnlContent.Enabled = false;
                            //pnlContent2.Enabled = false;
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
    #region TravelAuthorization
    void FillDDLs(bool initial = true)
    {
        try
        {
            if (initial)
            {
                Objects.User ui = (Objects.User)Session["userinfo"];
                DataTable dt = new DataTable();
                u.GetStaffMembersByDepartmentID(ref dt);
                DDLUsers.DataSource = dt;
                DDLUsers.DataBind();
                DDLUsers.SelectedValue = ui.User_Id.ToString();
                DataSet ds = new DataSet();
                bool sc = hdnstaffcategory.Value == "1" ? true : false;
                l.GetAllLookupsList(ref ds, sc);
                ddlModeOfTravelCode.DataSource = ds.Tables[1];
                ddlModeOfTravelCode.DataBind();

                ddlCarrierCode.DataSource = ds.Tables[13];
                ddlCarrierCode.DataBind();

                ddlExtraCargoCode.DataSource = ds.Tables[16];
                ddlExtraCargoCode.DataBind();

                ddlETDETACode.DataSource = ds.Tables[14];
                ddlETDETACode.DataBind();

                ddlETDETAPickup.DataSource = ds.Tables[15];
                ddlETDETAPickup.DataBind();

                ddlPickupLocation.DataSource = ds.Tables[18];
                ddlPickupLocation.DataBind();

                ddlBookingStatus.DataSource = ds.Tables[17];
                ddlBookingStatus.DataBind();


                if (checkIsEmergency.Checked == true)
                {
                    ddlTripSchemaCode.DataSource = ds.Tables[11];
                }
                else
                {
                    ddlTripSchemaCode.DataSource = ds.Tables[10];
                }
                ddlTripSchemaCode.DataBind();
            }
            else
            {
                DataSet ds = new DataSet();
                bool sc = hdnstaffcategory.Value == "1" ? true : false;
                l.GetAllLookupsList(ref ds, sc);
                if (checkIsEmergency.Checked == true)
                {
                    ddlTripSchemaCode.DataSource = ds.Tables[11];
                }
                else
                {
                    ddlTripSchemaCode.DataSource = ds.Tables[10];
                }
                ddlTripSchemaCode.DataBind();
            }



        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll(string TravelAuthorizationID)
    {

        txtTravelersFirstName.Text = "";
        txtTravelerSecondName.Text = "";
        txtTravelerLastName.Text = "";
        txtPurposeOfTravel.Text = "";
        txtFlightRefNumber.Text = "";
        txtRemarksOfTravel.Text = "";
        txtCheckinDate.Text = "";
        txtCheckoutDate.Text = "";
        ddlModeOfTravelCode.SelectedIndex = -1;
        ddlTripSchemaCode.SelectedIndex = -1;
        ddlCarrierCode.SelectedIndex = -1;
        ddlExtraCargoCode.SelectedIndex = -1;
        //ddlETDETACode.SelectedIndex = -1;
        ddlETDETAPickup.SelectedIndex = -1;
        ddlPickupLocation.SelectedIndex = -1;
        ddlBookingStatus.SelectedIndex = -1;
    }
    void FillTA(string TravelAuthorizationID)
    {
        DataTable dt = new DataTable();
        t.GetTravelAuthorizationByTravelAuthorizationID(TravelAuthorizationID, ref dt);

        foreach (DataRow row in dt.Rows)
        {
            txtPRISM.Text = row["PRISMNumber"].ToString();
            txtTravelersFirstName.Text = row["FirstName"].ToString();
            txtTravelerSecondName.Text = row["MiddleName"].ToString();
            txtTravelerLastName.Text = row["LastName"].ToString();
            txtPurposeOfTravel.Text = row["PurposeOfTravel"].ToString();
            ddlModeOfTravelCode.SelectedValue = row["ModeOfTravelCode"].ToString();
            ddlTripSchemaCode.SelectedValue = row["TripSchemaCode"].ToString();
            hdnstaffcategory.Value = Convert.ToBoolean(row["IsInternationalStaff"].ToString()) == true ? "1" : "0";
            hdnisemergency.Value = Convert.ToBoolean(row["IsEmergency"].ToString()) == true ? "1" : "0";
            checkIsEmergency.Checked = Convert.ToBoolean(row["IsEmergency"].ToString());


            ddlExtraCargoCode.SelectedValue = row["ExtraCargoCode"].ToString();
            ddlCarrierCode.SelectedValue = row["FlightCarrierCode"].ToString();
            txtFlightRefNumber.Text = row["FlightRefNo"].ToString();
            // txtRemarksOfTravel.Text =  row["LastName"].ToString();


            ddlETDETACode.SelectedValue = row["ETDETACode"].ToString();
            ddlETDETAPickup.SelectedValue = row["ETDETAPCode"].ToString();
            ddlPickupLocation.SelectedValue = row["PickupDropCode"].ToString();
            ddlBookingStatus.SelectedValue = row["BookingCode"].ToString();
            txtRemarksOfTravel.Text = row["Remarks"].ToString();
            txtCheckinDate.Text = row["CheckinDate"].ToString();
            txtCheckoutDate.Text = row["CheckOutDate"].ToString();
            txtNoNights.Text = row["Nights"].ToString();

            // txtRemarksOfTravel.Enabled = false;



            // txtNoNights.Text = "8";

            // checkIsEmergency.Checked = Convert.ToBoolean(row["IsNotify "].ToString());


            bool inter = hdnstaffcategory.Value == "1" ? true : false;
            bool em = hdnisemergency.Value == "1" ? true : false;
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds, inter);
            if (em == true)
            {
                ddlTripSchemaCode.DataSource = ds.Tables[11];
            }
            else
            {
                ddlTripSchemaCode.DataSource = ds.Tables[10];
            }
            ddlTripSchemaCode.DataBind();



            if (ddlTripSchemaCode.SelectedItem.Text == "Home Leave")
            {
                trFamilyMembers.Visible = true;
                txtFamilyMembers.Text = row["FamilyMembers"].ToString();
            }
            else
            {
                trFamilyMembers.Visible = false;
            }





            if (ddlTripSchemaCode.SelectedItem.Text == "Rest & Recuperation")
            {
                trRandRDays.Visible = true;
                txtRRStartDate.Text = row["RRStartDate"].ToString();
                txtRREndDate.Text = row["RREndDate"].ToString();
            }
            else if (ddlTripSchemaCode.SelectedItem.Text == "Maternity Leave" || ddlTripSchemaCode.SelectedItem.Text == "Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "Home Leave" || ddlTripSchemaCode.SelectedItem.Text == "Paternity Leave" || ddlTripSchemaCode.SelectedItem.Text == "Personal Leave/Weekend Away")
            {
                trLeaveDays.Visible = true;
                txtLeaveStartDate.Text = row["LeaveStartDate"].ToString();
                txtLeaveEndDate.Text = row["LeaveEndDate"].ToString();
                txtLeaveStartDate.CssClass = txtLeaveStartDate.CssClass + " Req";
                txtLeaveEndDate.CssClass = txtLeaveEndDate.CssClass + " Req";

            }
            else if (ddlTripSchemaCode.SelectedItem.Text == "R & R Plus Paternity" || ddlTripSchemaCode.SelectedItem.Text == "Home Leave and R&R" || ddlTripSchemaCode.SelectedItem.Text == "R&R with Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "TDY with R&R and Annual Leave")
            {
                trRandRDays.Visible = true;
                trLeaveDays.Visible = true;
                txtRRStartDate.Text = row["RRStartDate"].ToString();
                txtRREndDate.Text = row["RREndDate"].ToString();
                txtLeaveStartDate.Text = row["LeaveStartDate"].ToString();
                txtLeaveEndDate.Text = row["LeaveEndDate"].ToString();
                txtLeaveStartDate.CssClass = txtLeaveStartDate.CssClass + " Req";
                txtLeaveEndDate.CssClass = txtLeaveEndDate.CssClass + " Req";
                txtRRStartDate.CssClass = txtRRStartDate.CssClass + " Req";
                txtRREndDate.CssClass = txtRREndDate.CssClass + " Req";
            }
            else
            {
                trRandRDays.Visible = false;
                trLeaveDays.Visible = false;
                txtRRStartDate.Text = "";
                txtRREndDate.Text = "";
                txtLeaveStartDate.Text = "";
                txtLeaveEndDate.Text = "";
            }

            if (ddlCarrierCode.SelectedItem.Text == "UNHAS")
            {
                trCargo.Visible = true;
                ddlExtraCargoCode.SelectedValue = row["FlightCarrierCode"].ToString();

                // txtLeaveStartDate.CssClass = txtLeaveStartDate.CssClass + " Req";
                //  txtLeaveEndDate.CssClass = txtLeaveEndDate.CssClass + " Req";

            }


            if (ddlModeOfTravelCode.SelectedItem.Text == "Air")
            {
                // trCargo.Visible = true;
                // ddlExtraCargoCode.SelectedValue = row["FlightCarrierCode"].ToString();
                ddlCarrierCode.Enabled = true;
                txtFlightRefNumber.Enabled = true;
                // txtLeaveStartDate.CssClass = txtLeaveStartDate.CssClass + " Req";
                //  txtLeaveEndDate.CssClass = txtLeaveEndDate.CssClass + " Req";

            }





            DDLUsers.SelectedValue = row["UserID"].ToString();
            txtCity.Text = row["CityOfAccommodation"].ToString();

            rdYAnnual.Checked = Convert.ToBoolean(row["IsPrivateStay"]);
            rdNoAnnual.Checked = !Convert.ToBoolean(row["IsPrivateStay"]);
            txtPrivateStayDateFrom.Text = row["PrivateStayDateFrom"].ToString();
            txtPrivateStayDateTo.Text = row["PrivateStayDateTo"].ToString();
            if (Convert.ToBoolean(row["IsPrivateStay"]))
            {
                txtPrivateStayDateFrom.Enabled = true;
                txtPrivateStayDateTo.Enabled = true;
                txtPrivateStayDateFrom.CssClass = txtPrivateStayDateFrom.CssClass + " Req";
                txtPrivateStayDateTo.CssClass = txtPrivateStayDateTo.CssClass + " Req";
            }

            rdYesDiv.Checked = Convert.ToBoolean(row["IsPrivateDeviation"]);
            rdNoDiv.Checked = !Convert.ToBoolean(row["IsPrivateDeviation"]);
            txtDiv.Text = row["PrivateDeviationLegs"].ToString();
            if (Convert.ToBoolean(row["IsPrivateDeviation"]))
            {
                txtDiv.Enabled = true;
                txtDiv.CssClass = txtDiv.CssClass + " Req";
            }



            rdYesAcc.Checked = Convert.ToBoolean(row["IsAccommodationProvided"]);
            rdNoAcc.Checked = !Convert.ToBoolean(row["IsAccommodationProvided"]);
            txtAcc.Text = row["AccommodationDetails"].ToString();
            if (Convert.ToBoolean(row["IsAccommodationProvided"]))
            {
                txtAcc.Enabled = true;
                txtAcc.CssClass = txtAcc.CssClass + " Req";

                txtCheckinDate.Enabled = true;
                txtCheckinDate.CssClass = txtAcc.CssClass + " Req";
                txtCheckoutDate.Enabled = true;
                txtCheckoutDate.CssClass = txtAcc.CssClass + " Req";
                txtNoNights.Enabled = true;
                txtNoNights.CssClass = txtAcc.CssClass + " Req";


            }


        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            string TravelAuthorizationID = "";
            DataTable dt = new DataTable();

            DateTime? PrivateStayDateFrom;
            DateTime? PrivateStayDateTo;

            DateTime? CheckinDate;
            DateTime? CheckOutDate;

            DateTime? LeaveStartDate;
            DateTime? LeaveEndDate;
            DateTime? RRStartDate;
            DateTime? RREndDate;
            bool IsEmergency = checkIsEmergency.Checked;
            bool IsNotify = checkIsNotify.Checked;
            try
            {
                if (txtPrivateStayDateFrom.Text != "")
                {
                    PrivateStayDateFrom = Convert.ToDateTime(txtPrivateStayDateFrom.Text);
                }
                else
                {
                    PrivateStayDateFrom = null;
                }

                if (txtPrivateStayDateTo.Text != "")
                {
                    PrivateStayDateTo = Convert.ToDateTime(txtPrivateStayDateTo.Text);
                }
                else
                {
                    PrivateStayDateTo = null;
                }

                if (txtCheckinDate.Text != "")
                {
                    CheckinDate = Convert.ToDateTime(txtCheckinDate.Text);
                }
                else
                {
                    CheckinDate = null;
                }

                if (txtCheckoutDate.Text != "")
                {
                    CheckOutDate = Convert.ToDateTime(txtCheckoutDate.Text);
                }
                else
                {
                    CheckOutDate = null;
                }





                //Added by Lokiri James

                if (txtLeaveStartDate.Text != "")
                {
                    LeaveStartDate = Convert.ToDateTime(txtLeaveStartDate.Text);
                }
                else
                {
                    LeaveStartDate = null;
                }

                if (txtLeaveEndDate.Text != "")
                {
                    LeaveEndDate = Convert.ToDateTime(txtLeaveEndDate.Text);
                }
                else
                {
                    LeaveEndDate = null;
                }


                if (txtRRStartDate.Text != "")
                {
                    RRStartDate = Convert.ToDateTime(txtRRStartDate.Text);
                }
                else
                {
                    RRStartDate = null;
                }

                if (txtRREndDate.Text != "")
                {
                    RREndDate = Convert.ToDateTime(txtRREndDate.Text);
                }
                else
                {
                    RREndDate = null;
                }

                bool RandRRelationship = t.CheckRandRRelationShip(ddlTripSchemaCode.SelectedValue);
                //if (ddlTripSchemaCode.SelectedItem.Text == "R & R Plus Paternity" || ddlTripSchemaCode.SelectedItem.Text == "TDY with R&R" || ddlTripSchemaCode.SelectedItem.Text == "Home Leave and R&R" || ddlTripSchemaCode.SelectedItem.Text == "R&R with Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "TDY with R&R and Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "Rest & Recuperation")
                if (RandRRelationship)
                {
                    if (RRStartDate != null && RREndDate != null)
                    {
                        if (RREndDate < RRStartDate)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Error in R&R Days - End date cannot be before Start date";
                            PanelMessage.Focus();
                            return;
                        }
                    }
                    else
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Please Specify R&R Days";
                        PanelMessage.Focus();
                        return;
                    }

                }

                bool IsLeaveRelated = t.CheckLeaveRelationShip(ddlTripSchemaCode.SelectedValue);
                //if (ddlTripSchemaCode.SelectedItem.Text == "TDY with Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "Personal Leave/Weekend Away" || ddlTripSchemaCode.SelectedItem.Text == "R&R with Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "Paternity Leave" || ddlTripSchemaCode.SelectedItem.Text == "Home Leave" || ddlTripSchemaCode.SelectedItem.Text == "Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "TDY with R&R and Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "Maternity Leave")
                if (IsLeaveRelated)
                {
                    if (LeaveStartDate != null && LeaveEndDate != null)
                    {
                        if (LeaveEndDate < LeaveStartDate)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Error in Leave Days - End date cannot be before Start date";
                            PanelMessage.Focus();
                            return;
                        }
                    }
                    else
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Please Specify Leave Days";
                        PanelMessage.Focus();
                        return;
                    }

                }


                //Added by Lokiri James


                if (PrivateStayDateFrom != null && PrivateStayDateTo != null)
                {
                    if (PrivateStayDateTo < PrivateStayDateFrom)
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Error in Private Stay - To date cannot be before from date";
                        PanelMessage.Focus();
                        return;
                    }
                }

                if (g.CheckPastDate(PrivateStayDateFrom))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Error in Private Stay - From date cannot be in the past";
                    PanelMessage.Focus();
                    return;
                }

                if (g.CheckPastDate(PrivateStayDateTo))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Error in Private Stay - To date cannot be in the past";
                    PanelMessage.Focus();
                    return;
                }

                //added to check the checkin checkout date




                if (CheckinDate != null && CheckOutDate != null)
                {
                    if (CheckOutDate < CheckinDate)
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Error in Checkout Stay - To date cannot be before from date";
                        PanelMessage.Focus();
                        return;
                    }
                }

                if (g.CheckPastDate(CheckinDate))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Error in CheckIn - From date cannot be in the past";
                    PanelMessage.Focus();
                    return;
                }

                if (g.CheckPastDate(CheckOutDate))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Error in Check out - To date cannot be in the past";
                    PanelMessage.Focus();
                    return;
                }






            }
            catch (FormatException)
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Error in Private Stay - Wrong date format";
                PanelMessage.Focus();
                return;
            }

            if (rdYesAcc.Checked == true)
            {
                if (String.IsNullOrEmpty(txtAcc.Text))
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please Specify accomodation Location";
                    PanelMessage.Focus();
                    return;
                }
                else
                {
                    DataTable accLoc = new DataTable();
                    l.GetAccomodationByDescription(txtAcc.Text.Trim(), ref accLoc);
                    if (accLoc.Rows.Count == 0)
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "You must select the Location from the auto complete List";
                        PanelMessage.Focus();
                        return;
                    }

                }
            }

            //added the insert accomodation

            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                TravelAuthorizationID = t.InsertUpdateTravelersInformation(Decrypt(Request["TAID"].ToString()),
                    txtTravelersFirstName.Text.Trim(), txtTravelerSecondName.Text.Trim(),
                    txtTravelerLastName.Text.Trim(), txtPurposeOfTravel.Text.Trim(), ddlTripSchemaCode.SelectedValue,
                    ddlModeOfTravelCode.SelectedValue, "PEN", DDLUsers.SelectedValue.ToString(),
                    txtCity.Text, rdYAnnual.Checked, null, rdYesDiv.Checked, txtDiv.Text,
                    rdYesAcc.Checked, txtAcc.Text, PrivateStayDateFrom,
                    PrivateStayDateTo, txtFamilyMembers.Text.Trim(),
                    txtPRISM.Text, LeaveStartDate, LeaveEndDate, RRStartDate, RREndDate, IsEmergency,
                   ddlExtraCargoCode.SelectedValue,
            ddlCarrierCode.SelectedValue,
            txtFlightRefNumber.Text,
            ddlETDETACode.SelectedValue,
            ddlETDETAPickup.SelectedValue,
            ddlPickupLocation.SelectedValue,
            ddlBookingStatus.SelectedValue,
            txtRemarksOfTravel.Text,
            CheckinDate,
            CheckOutDate,
            txtNoNights.Text, IsNotify
);
            }
            else
            {
                TravelAuthorizationID = t.InsertUpdateTravelersInformation("", txtTravelersFirstName.Text.Trim(),
                    txtTravelerSecondName.Text.Trim(), txtTravelerLastName.Text.Trim(),
                    txtPurposeOfTravel.Text.Trim(), ddlTripSchemaCode.SelectedValue,
                    ddlModeOfTravelCode.SelectedValue, "PEN", DDLUsers.SelectedValue.ToString(),
                    txtCity.Text, rdYAnnual.Checked, null, rdYesDiv.Checked, txtDiv.Text,
                    rdYesAcc.Checked, txtAcc.Text, PrivateStayDateFrom,
                    PrivateStayDateTo, txtFamilyMembers.Text.Trim(),
                    txtPRISM.Text, LeaveStartDate, LeaveEndDate, RRStartDate, RREndDate, IsEmergency,
                     ddlExtraCargoCode.SelectedValue,
            ddlCarrierCode.SelectedValue,
            txtFlightRefNumber.Text,
            ddlETDETACode.SelectedValue,
            ddlETDETAPickup.SelectedValue,
            ddlPickupLocation.SelectedValue,
            ddlBookingStatus.SelectedValue,
            txtRemarksOfTravel.Text,
            CheckinDate,
            CheckOutDate,
            txtNoNights.Text, IsNotify


                    );
            }
            string TANO = "";
            DataTable dtTANO = new DataTable();
            t.GetTravelAuthorizationByTravelAuthorizationID(TravelAuthorizationID, ref dtTANO);
            TANO = dtTANO.Rows[0]["TravelAuthorizationNumber"].ToString();
            Response.Redirect("~/TravelAuthorization/TAWizard/Step2_AdvanceAndSecurity.aspx?TAID=" + Encrypt(TravelAuthorizationID) + "&&TANO=" + Encrypt(TANO) + "&First=1", false);
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void rdYesDiv_CheckedChanged(object sender, EventArgs e)
    {

        txtPrivateStayDateFrom.CssClass = txtPrivateStayDateFrom.CssClass + " Req";
        txtPrivateStayDateTo.CssClass = txtPrivateStayDateTo.CssClass + " Req";
        txtDiv.CssClass = txtDiv.CssClass + " Req";

        txtPrivateStayDateFrom.Enabled = true;
        txtPrivateStayDateTo.Enabled = true;
        txtDiv.Enabled = true;

        rdYAnnual.Checked = true;
        rdNoAnnual.Checked = false;

    }
    protected void rdNoDiv_CheckedChanged(object sender, EventArgs e)
    {

        //txtPrivateStayDateFrom.CssClass = txtPrivateStayDateFrom.CssClass.Replace("Req", "");
        //txtPrivateStayDateTo.CssClass = txtPrivateStayDateTo.CssClass.Replace("Req", "");
        txtDiv.CssClass = txtDiv.CssClass.Replace("Req", "");

        //txtPrivateStayDateFrom.Enabled = false;
        //txtPrivateStayDateTo.Enabled = false;
        txtDiv.Enabled = false;

        //txtPrivateStayDateFrom.Text = "";
        //txtPrivateStayDateTo.Text = "";
        txtDiv.Text = "";
        rdNoAnnual.Enabled = true;
        //rdYAnnual.Checked = false;
        //rdNoAnnual.Checked = true;
        //rdNoAnnual.Enabled = true;
    }
    protected void rdYAnnual_CheckedChanged(object sender, EventArgs e)
    {
        txtPrivateStayDateFrom.Enabled = true;
        txtPrivateStayDateTo.Enabled = true;
        txtPrivateStayDateFrom.CssClass = txtPrivateStayDateFrom.CssClass + " Req";
        txtPrivateStayDateTo.CssClass = txtPrivateStayDateTo.CssClass + " Req";

        txtDiv.CssClass = txtDiv.CssClass.Replace("Req", "");
    }
    protected void rdNoAnnual_CheckedChanged(object sender, EventArgs e)
    {
        txtPrivateStayDateFrom.Enabled = false;
        txtPrivateStayDateTo.Enabled = false;
        txtPrivateStayDateFrom.CssClass = txtPrivateStayDateFrom.CssClass.Replace("Req", "");
        txtPrivateStayDateTo.CssClass = txtPrivateStayDateTo.CssClass.Replace("Req", "");
        txtPrivateStayDateFrom.Text = "";
        txtPrivateStayDateTo.Text = "";

        rdYesDiv.Checked = false;
        rdNoDiv.Checked = true;
        txtDiv.Text = "";
        txtDiv.Enabled = false;

    }
    protected void rdYesAcc_CheckedChanged(object sender, EventArgs e)
    {
        txtAcc.CssClass = txtAcc.CssClass + " Req";
        txtAcc.Enabled = true;
    }
    protected void rdNoAcc_CheckedChanged(object sender, EventArgs e)
    {
        txtAcc.CssClass = txtAcc.CssClass.Replace("Req", "");
        txtAcc.Text = "";
        txtAcc.Enabled = false;
    }
    protected void DDLUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string UserID = DDLUsers.SelectedValue;
            DataTable dtu = new DataTable();
            Sec.GetUserInfoByUserID(UserID, ref dtu);
            hdnstaffcategory.Value = Convert.ToBoolean(dtu.Rows[0]["IsInternationalStaff"].ToString()) == true ? "1" : "0";
            bool sc = Convert.ToBoolean(dtu.Rows[0]["IsInternationalStaff"].ToString());
            txtTravelersFirstName.Text = dtu.Rows[0]["FirstName"].ToString();
            txtTravelerLastName.Text = dtu.Rows[0]["LastName"].ToString();
            txtPRISM.Text = dtu.Rows[0]["PRISM_Number"].ToString();
            FillDDLs(false);

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ddlCarrierCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFlightRefNumber.Enabled = true;
        if (ddlCarrierCode.SelectedItem.Text == "UNHAS")
        {
            trCargo.Visible = true;
        }
        else
        {
            trCargo.Visible = false;
        }
    }
    protected void ddlModeOfTravelCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlModeOfTravelCode.SelectedItem.Text == "Air")
        {
            ddlCarrierCode.Enabled = true;

        }
        else
        {
            ddlCarrierCode.Enabled = false;

        }
    }
    protected void ddlTripSchemaCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool LeaveRelationship = false;
        bool RandRRelationship = false;

        LeaveRelationship = t.CheckLeaveRelationShip(ddlTripSchemaCode.SelectedValue);
        RandRRelationship = t.CheckRandRRelationShip(ddlTripSchemaCode.SelectedValue);

        //Clear All
        txtLeaveStartDate.Text = "";
        txtLeaveEndDate.Text = "";
        txtRRStartDate.Text = "";
        txtRREndDate.Text = "";
        //Clear All


        if (LeaveRelationship && RandRRelationship)
        {
            trRandRDays.Visible = true;
            trLeaveDays.Visible = true;
        }
        else if (LeaveRelationship)
        {
            trLeaveDays.Visible = true;
        }
        else if (RandRRelationship)
        {
            trRandRDays.Visible = true;
        }
        else
        {
            trRandRDays.Visible = false;
            trLeaveDays.Visible = false;
        }

        if (ddlTripSchemaCode.SelectedItem.Text == "Home Leave")
        {
            trFamilyMembers.Visible = true;
        }
        else
        {
            trFamilyMembers.Visible = false;
        }

        //if (LeaveRelationship)
        //{
        //    trRandRDays.Visible = true;
        //    txtRRStartDate.Text = "";
        //    txtRREndDate.Text = "";

        //    trLeaveDays.Visible = false;
        //    txtLeaveStartDate.Text = "";
        //    txtLeaveEndDate.Text = "";
        //}
        //if (ddlTripSchemaCode.SelectedItem.Text == "Rest & Recuperation")
        //{
        //    trRandRDays.Visible = true;
        //    txtRRStartDate.Text = "";
        //    txtRREndDate.Text = "";

        //    trLeaveDays.Visible = false;
        //    txtLeaveStartDate.Text = "";
        //    txtLeaveEndDate.Text = "";
        //}
        //else if (ddlTripSchemaCode.SelectedItem.Text == "Maternity Leave" || ddlTripSchemaCode.SelectedItem.Text == "Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "Home Leave" || ddlTripSchemaCode.SelectedItem.Text == "Paternity Leave" || ddlTripSchemaCode.SelectedItem.Text == "Personal Leave/Weekend Away")
        //{
        //    trLeaveDays.Visible = true;
        //    txtLeaveStartDate.Text = "";
        //    txtLeaveEndDate.Text = "";
        //    trRandRDays.Visible = false;
        //    txtRRStartDate.Text = "";
        //    txtRREndDate.Text = "";
        //}
        //else if (ddlTripSchemaCode.SelectedItem.Text == "R & R Plus Paternity" || ddlTripSchemaCode.SelectedItem.Text == "Home Leave and R&R" || ddlTripSchemaCode.SelectedItem.Text == "R&R with Annual Leave" || ddlTripSchemaCode.SelectedItem.Text == "TDY with R&R and Annual Leave")
        //{
        //    trRandRDays.Visible = true;
        //    trLeaveDays.Visible = true;
        //    txtRRStartDate.Text = "";
        //    txtRREndDate.Text = "";
        //    txtLeaveStartDate.Text = "";
        //    txtLeaveEndDate.Text = "";
        //}
        //else
        //{
        //    trRandRDays.Visible = false;
        //    trLeaveDays.Visible = false;
        //    txtRRStartDate.Text = "";
        //    txtRREndDate.Text = "";
        //    txtLeaveStartDate.Text = "";
        //    txtLeaveEndDate.Text = "";
        //}

    }
    protected void checkIsEmergency_CheckedChanged(object sender, EventArgs e)
    {
        if (checkIsEmergency.Checked == true)
        {
            Objects.User ui = (Objects.User)Session["userinfo"];

            DataTable dt = new DataTable();
            u.GetMissionStaffMembers(ref dt);
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds);
            ddlTripSchemaCode.DataSource = ds.Tables[11];
            ddlTripSchemaCode.DataBind();

            DDLUsers.DataSource = dt;
            DDLUsers.DataBind();
            DDLUsers.SelectedValue = ui.User_Id.ToString();
            hdnisemergency.Value = "1";
        }
        else
        {
            Objects.User ui = (Objects.User)Session["userinfo"];
            DataTable dt = new DataTable();
            u.GetStaffMembersByDepartmentID(ref dt);
            DDLUsers.DataSource = dt;
            DDLUsers.DataBind();
            DDLUsers.SelectedValue = ui.User_Id.ToString();
            hdnisemergency.Value = "0";

            DataSet ds = new DataSet();
            bool staffcategory = hdnstaffcategory.Value == "1" ? true : false;
            l.GetAllLookupsList(ref ds, staffcategory);
            ddlTripSchemaCode.DataSource = ds.Tables[10];
            ddlTripSchemaCode.DataBind();
        }
    }
    #endregion

}