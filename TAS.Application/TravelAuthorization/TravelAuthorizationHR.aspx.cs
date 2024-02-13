using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class TravelAuthorization_TravelAuthorizationHR : AuthenticatedPageClass
{
    Business.TravelAuthorization TA = new Business.TravelAuthorization();
    Business.Lookups l = new Business.Lookups();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillDDLs();

            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
            string TravelAuthorizationID = Decrypt(Request["TAID"]);
            FillTA(TravelAuthorizationID);
            FillTravelItinerary(TravelAuthorizationNumber);
            FillWBS(TravelAuthorizationID);



            txtCity.Enabled = false;
            rdYAnnual.Enabled = false;
            rdNoAnnual.Enabled = false;
            txtAnnual.Enabled = false;
            rdYesDiv.Enabled = false;
            rdNoDiv.Enabled = false;
            txtDiv.Enabled = false;
            rdYesAcc.Enabled = false;
            rdNoAcc.Enabled = false;
            txtAcc.Enabled = false;
            rdYesAdv.Enabled = false;
            rdNoAdv.Enabled = false;
            DDLAdvCurr.Enabled = false;
            txtAdvAmnt.Enabled = false;
            rdViaBank.Enabled = false;
            rdViaCheck.Enabled = false;
            rdViaCash.Enabled = false;
            rdVisaNA.Enabled = false;
            rdVisaNo.Enabled = false;
            rdVisYes.Enabled = false;
            txtVisa.Enabled = false;
            rdHealthNA.Enabled = false;
            rdHealthNo.Enabled = false;
            rdHealthYes.Enabled = false;
            rdMissionYes.Enabled = false;
            rdMissionNo.Enabled = false;
            chkConfirm.Enabled = false;
        }
        if (!string.IsNullOrEmpty(ViewState["StatusCode"] as string))
        {
            if (ViewState["StatusCode"].ToString() == "COM")
            {
                btnSave.Visible = false;
                ddlStatusCode.Enabled = false;
                //txtComments.Enabled = false;
            }
            else
            {
                btnSave.Visible = this.CanAdd;
                ddlStatusCode.Enabled = true;
                //  txtComments.Enabled = true;
            }
        }
    }
    void FillDDLs()
    {
        try
        {
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds);

            ddlModeOfTravelCode.DataSource = ds.Tables[1];
            ddlModeOfTravelCode.DataBind();

            ddlTripSchemaCode.DataSource = ds.Tables[2];
            ddlTripSchemaCode.DataBind();

            ddlStatusCode.DataSource = ds.Tables[5];
            ddlStatusCode.DataBind();
            DDLAdvCurr.DataSource = ds.Tables[7];
            DDLAdvCurr.DataBind();


            DataTable dtReasons = new DataTable();
            l.GetRejectionReason(ref dtReasons);
            lstRejectionReason.DataSource = dtReasons;
            lstRejectionReason.DataTextField = "Description";
            lstRejectionReason.DataValueField = "LookupsID";

            lstRejectionReason.DataBind();
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillTA(string TravelAuthorizationID)
    {
        DataTable dt = new DataTable();
        TA.GetTravelAuthorizationByTravelAuthorizationID(TravelAuthorizationID, ref dt);

        foreach (DataRow row in dt.Rows)
        {
            txtTravelersName.Text = row["TravelersName"].ToString();
            lblTANo.Text = row["TravelAuthorizationNumber"].ToString();
            txtPurposeOfTravel.Text = row["PurposeOfTravel"].ToString();
            ddlModeOfTravelCode.SelectedValue = row["ModeOfTravelCode"].ToString();
            ddlTripSchemaCode.SelectedValue = row["TripSchemaCode"].ToString();
            checkSecurityClearance.Checked = Convert.ToBoolean(row["SecurityClearance"]);
            checkSecurityTraining.Checked = Convert.ToBoolean(row["SecurityTraining"]);
            ddlStatusCode.SelectedValue = row["StatusCode"].ToString();

            ViewState["StatusCode"] = row["StatusCode"].ToString();
            if (row["StatusCode"].ToString().ToUpper() == "INC".ToUpper())
            {
                lstRejectionReason.Visible = true;
                RejectionReasonDiv.Style.Add("display", "block");
                FIllRejectionReason(TravelAuthorizationID);
            }

            txtCity.Text = row["CityOfAccommodation"].ToString();
            rdYAnnual.Checked = Convert.ToBoolean(row["IsPrivateStay"]);
            rdNoAnnual.Checked = !Convert.ToBoolean(row["IsPrivateStay"]);
            txtAnnual.Text = row["PrivateStayDates"].ToString();
            rdYesDiv.Checked = Convert.ToBoolean(row["IsPrivateDeviation"]);
            rdNoDiv.Checked = !Convert.ToBoolean(row["IsPrivateDeviation"]);
            txtDiv.Text = row["PrivateDeviationLegs"].ToString();
            rdYesAcc.Checked = Convert.ToBoolean(row["IsAccommodationProvided"]);
            rdNoAcc.Checked = !Convert.ToBoolean(row["IsAccommodationProvided"]);
            txtAcc.Text = row["AccommodationDetails"].ToString();
            rdYesAdv.Checked = Convert.ToBoolean(row["IsTravelAdvanceRequested"]);
            rdNoAdv.Checked = !Convert.ToBoolean(row["IsTravelAdvanceRequested"]);
            DDLAdvCurr.SelectedValue = row["TravelAdvanceCurrency"].ToString();
            txtAdvAmnt.Text = row["TravelAdvanceAmount"].ToString();
            if (row["TravelAdvanceMethod"].ToString() == "Bank")
                rdViaBank.Checked = true;
            if (row["TravelAdvanceMethod"].ToString() == "Check")
                rdViaCheck.Checked = true;
            if (row["TravelAdvanceMethod"].ToString() == "Cash")
                rdViaCash.Checked = true;
            if (row["IsVisaObtained"].ToString() == "0")
                rdVisaNA.Checked = true;
            if (row["IsVisaObtained"].ToString() == "1")
                rdVisaNo.Checked = true;
            if (row["IsVisaObtained"].ToString() == "2")
                rdVisYes.Checked = true;
            txtVisa.Text = row["VisaIssued"].ToString();
            if (row["IsVaccinationObtained"].ToString() == "0")
                rdHealthNA.Checked = true;
            if (row["IsVaccinationObtained"].ToString() == "1")
                rdHealthNo.Checked = true;
            if (row["IsVaccinationObtained"].ToString() == "2")
                rdHealthYes.Checked = true;
            rdMissionYes.Checked = Convert.ToBoolean(row["IsSecurityClearanceRequestedByMission"]);
            rdMissionNo.Checked = !Convert.ToBoolean(row["IsSecurityClearanceRequestedByMission"]);
            chkConfirm.Checked = Convert.ToBoolean(row["TAConfirm"]);
            // txtComments.Text = row["Comments"].ToString();
        }
    }
    void FillTravelItinerary(string TravelAuthorizationNumber)
    {
        DataTable dt = new DataTable();
        TA.GetTravelItineraryByTravelAuthorizationNumber(TravelAuthorizationNumber, ref dt);
        gvTravelItinerary.DataSource = dt;
        gvTravelItinerary.DataBind();
    }
    void FillWBS(string TravelAuthorizationID)
    {
        DataTable dt = new DataTable();
        TA.GetWBSByTravelAuthorizationID(TravelAuthorizationID, ref dt);
        gvWBS.DataSource = dt;
        gvWBS.DataBind();

        foreach (DataRow row in dt.Rows)
        {
            if (Convert.ToBoolean(row["IsPercentage"]))
            {
                rdPercentage.Checked = true;
            }
            else
            {
                rdAmount.Checked = true;
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        PanelMessage.Visible = false;
        lblmsg.Text = "";

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string TravelAuthorizationID = Decrypt(Request["TAID"]);
        TA.UpdateTravelAuthorizationStatus(TravelAuthorizationID, ddlStatusCode.SelectedValue, "");

        PanelMessage.Visible = true;
        lblmsg.ForeColor = Color.Green;
        lblmsg.Text = "Items have been updated successfully";
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer viewer = new ReportViewer();
            //viewer.ZoomMode = ZoomMode.Percent;
            //viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);

            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/Travel Authorization Form";
            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
            ReportParameter p = new ReportParameter("TravelAuthorizationNumber", TravelAuthorizationNumber);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.Refresh();

            Export(viewer, "PDF", "Travel Authorizatoin Form");
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    public bool Export(ReportViewer viewer, string exportType, string reportsTitle)
    {
        try
        {
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;

            filetype = "Pdf";
            byte[] bytes = viewer.ServerReport.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "MyTA.pdf");
            Response.Flush();
            Response.BinaryWrite(bytes);

            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
        return true;
    }



    #region status

    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        try
        {
            string TravelAuthorizationNumber = "";
            string TravelAuthorizationID = "";
            if (ddlStatusCode.SelectedItem.Text == "TA Incomplete")
            {
                // t.InsertTAComments()
                TravelAuthorizationNumber = Decrypt(Request["TANO"]);
                TravelAuthorizationID = Decrypt(Request["TAID"]);
                string RejectionReasons = "";

                for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                {
                    if (lstRejectionReason.Items[i].Selected)
                    {
                        TA.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "", "");
                        RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                    }
                }
                TA.InsertTAStatus(TravelAuthorizationID, ddlStatusCode.SelectedValue.ToString(), RejectionReasons, "");

                PanelMessage.Visible = true;
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "TA Status has been updated successfully";

            }
            else
            {
                if (!string.IsNullOrEmpty(Request["TANO"] as string) && !string.IsNullOrEmpty(Request["TAID"] as string))
                {
                    TravelAuthorizationNumber = Decrypt(Request["TANO"]);
                    TravelAuthorizationID = Decrypt(Request["TAID"]);
                    TA.InsertTAStatus(TravelAuthorizationID, ddlStatusCode.SelectedValue.ToString(), "", "");
                    PanelMessage.Visible = true;
                    lblmsg.ForeColor = Color.Green;
                    lblmsg.Text = "TA Status has been updated successfully";


                }
            }

        }
        catch (Exception ex)
        {
            PanelMessage.Visible = true;
            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
            lblmsg.ForeColor = Color.Red;
            lblmsg.Text = "Something went wrong " + ex.Message + " , Please contact  your system administrator";

        }


    }
    protected void ddlStatusCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStatusCode.SelectedItem.Text == "TA Incomplete")
        {
            RejectionReasonDiv.Style.Add("display", "block");
            lstRejectionReason.Visible = true;

        }
        else
        {
            RejectionReasonDiv.Style.Add("display", "none");
            lstRejectionReason.Visible = false;
        }


    }

    private void FIllRejectionReason(string TravelAuthorizationID)
    {

        DataTable dtREjectionReasons = new DataTable();
        TA.GetRejectionReason(TravelAuthorizationID, "", ref dtREjectionReasons);

        for (int i = 0; i < dtREjectionReasons.Rows.Count; i++)
        {
            if (dtREjectionReasons.Rows[i]["RejectionReasonID"].ToString() == lstRejectionReason.Items[i].Value.ToString())
            {
                lstRejectionReason.Items[i].Selected = true;
            }
        }





    }
    #endregion
}