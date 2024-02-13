using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

public partial class TravelAuthorization_TAWizard_WizardHeader : System.Web.UI.UserControl
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.Users u = new Business.Users();
    HttpContext context = HttpContext.Current;
    Business.Security Sec = new Business.Security();
    RadioCheckBusiness.RadioCheck Noti = new RadioCheckBusiness.RadioCheck();
    Business.MailModel MM = new Business.MailModel();
    //TravelAuthorizationDataModel db = new TravelAuthorizationDataModel();
    Globals g = new Globals();
    AuthenticatedPageClass a = new AuthenticatedPageClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckStatusPermission();
            IndicateStep();
        }
    }

    #region status
    void FillDDLs()
    {
        try
        {
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds);
            ddlStatusCode.DataSource = ds.Tables[5];
            ddlStatusCode.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void CheckStatusPermission()
    {
        try
        {
            Objects.User UserInfo = (Objects.User)Session["userinfo"];
            System.Data.DataTable dt = new System.Data.DataTable();
            Sec.getPagePermissions("/WizardHeader.ascx", ref dt);

            if (dt.Rows.Count > 0)
            {
                TAStatusDiv.Visible = Convert.ToBoolean(dt.Rows[0]["Amend"]);
            }
            else
            {
                TAStatusDiv.Visible = false;
            }
        }

        catch (Exception ex)
        {

        }
    }
    protected bool FillStatus()
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            bool ShowTEC = false;
            string TravelAuthorizationID = a.Decrypt(Request["TAID"]);
            t.GetTravelAuthorizationByTravelAuthorizationID(TravelAuthorizationID, ref dt);
            FillStatusHistory(TravelAuthorizationID);

            //fill ddlstatus
            l.GetAllLookupsList(ref ds);
            DataView dv = new DataView();
            dv = ds.Tables[5].DefaultView;
            //dv.RowFilter = "Code IN (" + dt.Rows[0]["LongDescription"].ToString() + ")";
            dv.RowFilter = "Code IN (" + dt.Rows[0]["LongDescription"].ToString() + ") OR Code = ''";
            ddlStatusCode.DataSource = dv;
            ddlStatusCode.DataBind();
            ddlStatusCode.SelectedValue = "00000000-0000-0000-0000-000000000000";
            //ddlStatusCode.Text = "";
            //ddlStatusCode.SelectedValue = dt.Rows[0]["StatusCode"].ToString();
            //For sending emails

            hdnCurrentTAStatus.Value = dt.Rows[0]["StatusCode"].ToString();
            hdnCurrentStatusDescription.Value = dt.Rows[0]["TAStatus"].ToString();
            hdnUserID.Value = dt.Rows[0]["UserID"].ToString();
            hdnMissionID.Value = dt.Rows[0]["MissionID"].ToString();
            hdnLocationID.Value = dt.Rows[0]["LocationID"].ToString();
            hdnDepartmentID.Value = dt.Rows[0]["DepartmentID"].ToString();
            hdnUnitID.Value = dt.Rows[0]["UnitID"].ToString();
            hdnSubUnitID.Value = dt.Rows[0]["SubUnitID"].ToString();
            CurrentTAStatus.Text = dt.Rows[0]["TAStatus"].ToString(); 
            CurrentTAStatus.Enabled = false;
            bool IsEmergencyTA = Convert.ToBoolean(dt.Rows[0]["IsEmergency"].ToString());
            Objects.User ui = (Objects.User)context.Session["userinfo"];

            //Conditional Lock/Unlocking of Status Div
            if (dt.Rows[0]["StatusCode"].ToString() == "CAN")
            {
                btnSaveStatus.Enabled = false;
                ddlStatusCode.Enabled = false;
                txtRejectComment.Enabled = false;
                CurrentTAStatus.Visible = true;
                ddlStatusCode.Visible = false;
                btnSaveStatus.Visible = false;
                Condit.Visible = false;

                DataTable dtReasons = new DataTable();
                l.GetCancellationReason(ref dtReasons, a.Decrypt(Request["TAID"]));
                lstRejectionReason.DataSource = dtReasons;
                lstRejectionReason.DataTextField = "Description";
                lstRejectionReason.DataValueField = "LookupsID";
                lstRejectionReason.DataBind();


                FillCancellationReason(a.Decrypt(Request["TAID"]), "CAN");
                btnSaveStatus.Enabled = false;
                ddlStatusCode.Enabled = false;
                txtRejectComment.Enabled = false;
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "PEN")
            {
                if ((ui.IsEmergencyTACreator && IsEmergencyTA) || (ui.User_Id == hdnUserID.Value) || (ui.IsManager && ui.LocationID == hdnLocationID.Value && ui.DepartmentID == hdnDepartmentID.Value && ui.UnitID == hdnUnitID.Value && ui.SubUnitID == hdnSubUnitID.Value))
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    txtRejectComment.Enabled = true;
                    CurrentTAStatus.Visible = false;
                    ddlStatusCode.Visible = true;
                    btnSaveStatus.Visible = true;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    ddlStatusCode.Enabled = false;
                    txtRejectComment.Enabled = false;
                    CurrentTAStatus.Visible = true;
                    ddlStatusCode.Visible = false;
                    btnSaveStatus.Visible = false;
                    Condit.Visible = false;
                }

            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "RB_SUB_SUP" || dt.Rows[0]["StatusCode"].ToString() == "RB_SUP" || dt.Rows[0]["StatusCode"].ToString() == "RB_OSS" || dt.Rows[0]["StatusCode"].ToString() == "RB_HR" || dt.Rows[0]["StatusCode"].ToString() == "RB_HOSO" || dt.Rows[0]["StatusCode"].ToString() == "RB_RMO" || dt.Rows[0]["StatusCode"].ToString() == "RB_DCOM" || dt.Rows[0]["StatusCode"].ToString() == "RB_COM" || dt.Rows[0]["StatusCode"].ToString() == "RB_RGD" || dt.Rows[0]["StatusCode"].ToString() == "APHB_DCOM" || dt.Rows[0]["StatusCode"].ToString() == "APHB_COM" || dt.Rows[0]["StatusCode"].ToString() == "APHB_RGD" || dt.Rows[0]["StatusCode"].ToString() == "TECDI")
            {
                DataTable dtReasons = new DataTable();
                l.GetRejectionReason(ref dtReasons,TravelAuthorizationID);
                lstRejectionReason.DataSource = dtReasons;
                lstRejectionReason.DataTextField = "Description";
                lstRejectionReason.DataValueField = "LookupsID";
                lstRejectionReason.DataBind();

                if (ui.User_Id == hdnUserID.Value || (ui.IsManager && ui.LocationID == hdnLocationID.Value && ui.DepartmentID == hdnDepartmentID.Value && ui.UnitID == hdnUnitID.Value && ui.SubUnitID == hdnSubUnitID.Value))
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    CurrentTAStatus.Visible = false;
                    ddlStatusCode.Visible = true;
                    btnSaveStatus.Visible = true;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    ddlStatusCode.Enabled = false;
                    CurrentTAStatus.Visible = true;
                    ddlStatusCode.Visible = false;
                    btnSaveStatus.Visible = false;
                    Condit.Visible = false;
                }
                if (dt.Rows[0]["StatusCode"].ToString() == "RB_SUB_SUP")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "SUB_SUP");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_SUP")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "SUP");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_OSS")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "SEC");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_HOSO")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "HOSO");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_HR")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "HR");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_RMO")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "RMO");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_DCOM")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "DCOM");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_COM")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "COM");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "RB_RGD")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "RGD");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "APHB_RGD")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "APPRGD");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "APHB_DCOM")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "APPDCOM");
                }
                else if (dt.Rows[0]["StatusCode"].ToString() == "APHB_COM")
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "APPCOM");
                }
                else if (dt.Rows[0]["StatusCode"].ToString().ToUpper() == "TECDI".ToUpper())
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "TEC");
                }
                else
                {
                    FillRejectionReason(a.Decrypt(Request["TAID"]), "TA");
                }
                txtRejectComment.Enabled = false;

            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TADR_SUB_SUP")
            {
                if (ui.IsSubSupervisor == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TADR_SUP")
            {
                //if (ui.IsSupervisor == true && ui.DepartmentID == hdnDepartmentID.Value && ui.UnitID == hdnUnitID.Value && ui.SubUnitID == hdnSubUnitID.Value)
                if ((ui.IsSupervisor == true && ui.DepartmentID == hdnDepartmentID.Value && ui.UnitID == hdnUnitID.Value && ui.SubUnitID == hdnSubUnitID.Value) || (ui.IsSupervisor == true))
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TADR_MAN")
            {
                if (ui.IsSupervisorManager && Convert.ToBoolean(dt.Rows[0]["TravellorIsSupervisor"].ToString()) == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TADR_DCOM")
            {
                if (ui.IsHOO)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TADR_COM")
            {
                if (ui.IsCOM == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TADR_RGD")
            {
                if (ui.IsRegionalDirector == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "SCV")
            {
                if (ui.IsSecReqVerifier == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "AAC")
            {
                if (ui.IsHOSO == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "SAR")
            {
                if (ui.IsHRAttendancePersonnel == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "WBSV")
            {
                if ((ui.IsRMO == true && ui.LocationID == hdnLocationID.Value) || (ui.IsRMO))
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "APP_DCOM")
            {
                if (ui.IsHOO == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "APP_COM")
            {
                if (ui.IsCOM == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "APP_RGD")
            {
                if (ui.IsRegionalDirector == true)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TADC")
            {
                if (ui.User_Id == hdnUserID.Value || (ui.IsManager && ui.LocationID == hdnLocationID.Value && ui.DepartmentID == hdnDepartmentID.Value && ui.UnitID == hdnUnitID.Value && ui.SubUnitID == hdnSubUnitID.Value))
                {
                    int NullTime = Convert.ToInt32(dt.Rows[0]["NullTime"]);
                    bool IsTecComplete = Convert.ToBoolean(dt.Rows[0]["IsTecComplete"]);
                    if (NullTime == 0 && IsTecComplete == true)
                    {
                        btnSaveStatus.Enabled = true;
                        ddlStatusCode.Enabled = true;
                        ddlStatusCode.Visible = true;
                        CurrentTAStatus.Visible = false;
                        Condit.Visible = true;
                    }
                    else
                    {
                        btnSaveStatus.Enabled = false;
                        ddlStatusCode.Enabled = false;
                        btnSaveStatus.Visible = false;
                        ddlStatusCode.Visible = false;
                        CurrentTAStatus.Visible = true;
                        Condit.Visible = false;
                    }
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    ddlStatusCode.Enabled = false;
                    txtRejectComment.Enabled = false;

                    btnSaveStatus.Enabled = false;
                    ddlStatusCode.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    Condit.Visible = false;

                }


            }
            else if (dt.Rows[0]["StatusCode"].ToString() == "TECDC")
            {
                if (ui.IsAdmin == false)
                {
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    btnSaveStatus.Enabled = false;
                    txtRejectComment.Enabled = false;
                    btnSaveStatus.Visible = false;
                    Condit.Visible = false;
                }
                else
                {
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    btnSaveStatus.Enabled = true;
                    txtRejectComment.Enabled = true;
                    btnSaveStatus.Visible = true;
                    Condit.Visible = true;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString().ToUpper() == "TECCOM")
            {
                if (ui.IsAdmin == false)
                {
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    btnSaveStatus.Enabled = false;
                    txtRejectComment.Enabled = false;
                    btnSaveStatus.Visible = false;
                    Condit.Visible = false;
                }
                else
                {
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    btnSaveStatus.Enabled = true;
                    txtRejectComment.Enabled = true;
                    btnSaveStatus.Visible = true;
                    Condit.Visible = true;
                }
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "You are not allowed to make changes to TAs/TECs already validated by Admin staff. For any changes, please email IOM South Sudan ICT Support issicts@iom.int for the change request.";
            }
            else if (dt.Rows[0]["StatusCode"].ToString().ToUpper() == "TECRTA")
            {
                if (ui.IsAdmin == false)
                {
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentTAStatus.Visible = true;
                    btnSaveStatus.Enabled = false;
                    //txtRejectComment.Enabled = false;
                    btnSaveStatus.Visible = false;
                    Condit.Visible = false;
                }
                else
                {
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentTAStatus.Visible = false;
                    btnSaveStatus.Enabled = true;
                    //txtRejectComment.Enabled = true;
                    btnSaveStatus.Visible = true;
                    Condit.Visible = true;
                }
            }
            else if (dt.Rows[0]["StatusCode"].ToString().ToUpper() == "SET")
            {
                txtDocNo.Enabled = false;
                trTADocument.Style.Add("visibility", "visible");
                txtDocNo.Text = dt.Rows[0]["DocumentNumber"].ToString();
                btnSaveStatus.Enabled = false;
                ddlStatusCode.Enabled = false;
                CurrentTAStatus.Visible = true;
                ddlStatusCode.Visible = false;
                btnSaveStatus.Visible = false;
                Condit.Visible = false;

            }
            else if (dt.Rows[0]["StatusCode"].ToString().ToUpper() == "NDSA")
            {
                ddlStatusCode.Enabled = false;
                btnSaveStatus.Enabled = false;
            }

            //Lock/Unlock Status Div

            if (ddlStatusCode.SelectedValue == "" || ddlStatusCode.SelectedValue == dt.Rows[0]["StatusCode"].ToString().ToUpper())
            {
                btnSaveStatus.Enabled = false;
                txtRejectComment.Enabled = false;
            }

            if ((ddlStatusCode.SelectedValue == "PEN" || dt.Rows[0]["StatusCode"].ToString().ToUpper() == "PEN") || ddlStatusCode.SelectedValue == "TADS" || dt.Rows[0]["StatusCode"].ToString().ToUpper() == "TADS" || ddlStatusCode.SelectedValue == "CAN" || dt.Rows[0]["StatusCode"].ToString().ToUpper() == "CAN" || ddlStatusCode.SelectedValue == "NDSA" || dt.Rows[0]["StatusCode"].ToString().ToUpper() == "NDSA")
            {
                ShowTEC = false;
            }
            else
            {
                if (Convert.ToDateTime(dt.Rows[0]["ToLocationDate"]) <= DateTime.Now)
                {
                    ShowTEC = true;
                }
                else
                {
                    ShowTEC = false;
                }
            }
            if (Convert.ToBoolean(dt.Rows[0]["IsNotForDSA"]))
            {
                ShowTEC = false;
            }
            return ShowTEC;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
            return false;
        }

    }
    protected void FillStatusHistory(string TravelAuthorizationID)
    {
        try
        {
            DataTable dtHistory = new DataTable();
            t.GetTAStatusHistory(TravelAuthorizationID, ref dtHistory);
            if (dtHistory.Rows.Count != 0)
            {
                gvHistoryStatus.DataSource = dtHistory;
                gvHistoryStatus.DataBind();
            }

        }
        catch (Exception ex)
        {

        }
    }
    private void FillRejectionReason(string TravelAuthorizationID, string RejectionReasonType)
    {
        RejectionReasonDiv.Style.Add("visibility", "visible");
        lstRejectionReason.Visible = true;
        lblRejectionReason.Visible = true;
        trRejectComment.Visible = true;
        txtRejectComment.Enabled = true;
        trTADocument.Style.Add("visibility", "hidden");

        DataTable dtRejectionReasons = new DataTable();
        t.GetRejectionReason(TravelAuthorizationID, RejectionReasonType, ref dtRejectionReasons);
        if (dtRejectionReasons.Rows.Count > 0)
            txtRejectComment.Text = dtRejectionReasons.Rows[0]["RejectionComment"].ToString();
        for (int i = 0; i < dtRejectionReasons.Rows.Count; i++)
        {
            txtRejectComment.Text = dtRejectionReasons.Rows[i]["RejectionComment"].ToString();
            for (int j = 0; j < lstRejectionReason.Items.Count; j++)
            {

                if (dtRejectionReasons.Rows[i]["RejectionReasonID"].ToString() == lstRejectionReason.Items[j].Value.ToString())
                {
                    lstRejectionReason.Items[j].Selected = true;
                }
            }
        }
    }
    private void FillReturnReason(string TravelAuthorizationID, string ReasonType)
    {
        RejectionReasonDiv.Style.Add("visibility", "visible");
        lstRejectionReason.Visible = true;
        lblRejectionReason.Visible = true;
        trRejectComment.Visible = true;
        txtRejectComment.Enabled = true;
        trTADocument.Style.Add("visibility", "hidden");

        DataTable dtRejectionReasons = new DataTable();
        t.GetReturnReason(TravelAuthorizationID, ReasonType, ref dtRejectionReasons);
        if (dtRejectionReasons.Rows.Count > 0)
            txtRejectComment.Text = dtRejectionReasons.Rows[0]["RejectionComment"].ToString();
        for (int i = 0; i < dtRejectionReasons.Rows.Count; i++)
        {
            txtRejectComment.Text = dtRejectionReasons.Rows[i]["RejectionComment"].ToString();
            for (int j = 0; j < lstRejectionReason.Items.Count; j++)
            {

                if (dtRejectionReasons.Rows[i]["RejectionReasonID"].ToString() == lstRejectionReason.Items[j].Value.ToString())
                {
                    lstRejectionReason.Items[j].Selected = true;
                }
            }
        }
    }
    private void FillCancellationReason(string TravelAuthorizationID, string RejectionReasonType)
    {
        RejectionReasonDiv.Style.Add("visibility", "visible");
        lstRejectionReason.Visible = true;
        lblRejectionReason.Visible = true;
        trRejectComment.Visible = true;
        txtRejectComment.Enabled = true;
        trTADocument.Style.Add("visibility", "hidden");

        DataTable dtRejectionReasons = new DataTable();
        t.GetCancellationReason(TravelAuthorizationID, RejectionReasonType, ref dtRejectionReasons);
        if (dtRejectionReasons.Rows.Count > 0)
            txtRejectComment.Text = dtRejectionReasons.Rows[0]["RejectionComment"].ToString();
        for (int i = 0; i < dtRejectionReasons.Rows.Count; i++)
        {
            txtRejectComment.Text = dtRejectionReasons.Rows[i]["RejectionComment"].ToString();
            for (int j = 0; j < lstRejectionReason.Items.Count; j++)
            {

                if (dtRejectionReasons.Rows[i]["RejectionReasonID"].ToString() == lstRejectionReason.Items[j].Value.ToString())
                {
                    lstRejectionReason.Items[j].Selected = true;
                }
            }
        }
    }
    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        btnSaveStatus.Enabled = false;
        ddlStatusCode.Enabled = false;
        try
        {
            string TravelAuthorizationNumber = "";
            string TravelAuthorizationID = "";
            if (hdnUserID.Value.ToString() == "")
            {
                return;
            }
            Objects.User ui = (Objects.User)Session["userinfo"];
            TravelAuthorizationNumber = a.Decrypt(Request["TANO"]);
            TravelAuthorizationID = a.Decrypt(Request["TAID"]);
            string result = "0";
            bool IsReason = false;
            bool priority = false;
            DataTable rdt = new DataTable();

            if (ddlStatusCode.SelectedValue == "SFA")
            {
                t.GetTAByTANo(TravelAuthorizationNumber, hdnUserID.Value.ToString(),6, ref rdt);

                //string Name = rdt.Rows[0]["FirstName"].ToString() + " " + rdt.Rows[0]["LastName"].ToString();
                //string Email = rdt.Rows[0]["Email"].ToString();

                if (Convert.ToBoolean(rdt.Rows[0]["IsEmergency"].ToString()) == true)
                {
                    priority = true;
                    result = t.InsertTAStatus(TravelAuthorizationID, "APP_COM", "", "");
                }
                else if (Convert.ToBoolean(rdt.Rows[0]["TravellorIsCOM"].ToString()) == true)
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_RGD", "", "");
                }
                else if (Convert.ToBoolean(rdt.Rows[0]["TravellorIsHOO"].ToString()) == true)
                {
                    if (Convert.ToBoolean(rdt.Rows[0]["IsInternationalFlight"].ToString()) == true)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "TADR_DCOM", "", "");
                    }
                    else
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "TADR_COM", "", "");
                    }
                }
                else if (Convert.ToBoolean(rdt.Rows[0]["TravellorIsSubStaffMember"].ToString()) == true)
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_SUB_SUP", "", "");
                }
                else if (Convert.ToBoolean(rdt.Rows[0]["TravellorIsSupervisorManager"].ToString()) == true)
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_COM", "", "");
                }
                else if (Convert.ToBoolean(rdt.Rows[0]["TravellorIsSupervisor"].ToString()) == true)
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_MAN", "", "");
                }
                else
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_SUP", "", "");
                }

                if (result == "1")
                {
                    //Pending Task  Notification to all Concerned Staff
                    Noti.SendTAPendingNotificationEmail(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(),6);
                    //Pending Task  Notification to all Concerned Staff
                }
            }
            else if (ddlStatusCode.SelectedValue == "RTA")
            {
                t.GetTAByTANo(TravelAuthorizationNumber, hdnUserID.Value.ToString(),7, ref rdt);
                //Return TA
                string Reasons = "";
                for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                {
                    if (lstRejectionReason.Items[i].Selected)
                    {
                        IsReason = true;
                        Reasons += lstRejectionReason.Items[i].Text + " - ";
                    }
                }

                if (IsReason)
                {
                    Reasons = Reasons.Substring(0, Reasons.Length - 3);
                    if (hdnCurrentTAStatus.Value == "TADR_SUB_SUP")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_SUB_SUP", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "SUBSUP", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "TADR_SUP")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_SUP", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "SUP", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "TADR_MAN")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_MAN", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "MAN", txtRejectComment.Text);
                            }
                        }

                    }
                    else if (hdnCurrentTAStatus.Value == "TADR_DCOM")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_DCOM", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "DCOM", txtRejectComment.Text);
                            }
                        }

                    }
                    else if (hdnCurrentTAStatus.Value == "TADR_COM")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_COM", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "COM", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "TADR_RGD")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_RGD", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "RGD", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "APP_RGD")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "APHB_RGD", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "APPRGD", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "APP_COM")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "APHB_COM", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "APPCOM", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "APP_DCOM")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "APHB_DCOM", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "APPDCOM", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "SCV")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_OSS", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "SEC", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "SAR")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_HR", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "HR", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "AAC")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_HOSO", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "HOSO", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "WBSV")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "RB_RMO", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "RMO", txtRejectComment.Text);
                            }
                        }
                    }
                    else if (hdnCurrentTAStatus.Value == "TECDC")
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "TEC", Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "TEC", txtRejectComment.Text);
                            }
                        }
                    }
                    else
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, ddlStatusCode.SelectedValue.ToString(), Reasons, "");
                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "TA", txtRejectComment.Text);
                            }
                        }
                    }
                    if (result == "1")
                    {
                        //Pending Task  Notification to all Concerned Staff
                        Noti.SendTANotificationToOwner(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(), 7);
                        //Pending Task  Notification to all Concerned Staff
                    }
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please select the rejection reasons";
                    return;
                }
                //Return TA
            }
            else if (ddlStatusCode.SelectedValue == "CAN")
            {
                t.GetTAByTANo(TravelAuthorizationNumber, hdnUserID.Value.ToString(),8, ref rdt);
                //TA Cancelled
                string Reasons = "";
                for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                {
                    if (lstRejectionReason.Items[i].Selected)
                    {
                        IsReason = true;
                        Reasons += lstRejectionReason.Items[i].Text + " - ";
                    }
                }

                if (IsReason)
                {
                    Reasons = Reasons.Substring(0, Reasons.Length - 3);
                    result = t.InsertTAStatus(TravelAuthorizationID, ddlStatusCode.SelectedValue.ToString(), Reasons, "");
                    if (result == "1")

                    {

                        string RejectionReasons = "";
                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
                            }
                        }

                        if (RejectionReasons == "")
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Please select the rejection reasons";
                            return;
                        }

                        for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
                        {
                            if (lstRejectionReason.Items[i].Selected)
                            {
                                t.InsertRejectionReasons(TravelAuthorizationID, lstRejectionReason.Items[i].Value, "CAN", txtRejectComment.Text);
                            }
                        }

                        Noti.SendTANotificationToOwner(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(),8);
                        //Mailing
                    }
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please select the cancellation reasons";
                    return;
                }
                //TA Cancelled
            }
            else if (ddlStatusCode.SelectedValue == "CCC")
            {
                t.GetTAByTANo(TravelAuthorizationNumber, hdnUserID.Value.ToString(),6, ref rdt);
                //Corrected
                if (rdt.Rows[0]["StatusCode"].ToString() == "RB_RGD")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_RGD", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_COM")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_COM", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_DCOM")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_DCOM", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_SUB_SUP")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_SUB_SUP", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_SUP")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_SUP", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_MAN")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "TADR_MAN", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_OSS")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "SCV", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_HR")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "SAR", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_HOSO")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "AAC", "", "");
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "RB_RMO")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "WBSV", "", "");

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "APHB_DCOM")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "APP_DCOM", "", "");

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "APHB_COM")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "APP_COM", "", "");

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "APHB_RGD")
                {
                    result = t.InsertTAStatus(TravelAuthorizationID, "APP_RGD", "", "");
                }
                if (result == "1")
                {
                    //Pending task
                    Noti.SendTAPendingNotificationEmail(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(), 6);
                    //Pending task
                }
                //Corrected
            }
            else if (ddlStatusCode.SelectedValue == "APP")
            {
                t.GetTAByTANo(TravelAuthorizationNumber, hdnUserID.Value.ToString(),9, ref rdt);
                if (rdt.Rows[0]["StatusCode"].ToString() == "TADR_SUB_SUP")
                {
                    if (ui.IsSubSupervisor)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "SCV", "", "");
                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "TADR_SUP")
                {
                    if (ui.IsSupervisor)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "SCV", "", "");
                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "TADR_MAN")
                {
                    if (ui.IsSupervisorManager)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "SCV", "", "");
                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "TADR_DCOM")
                {
                    if (ui.IsHOO)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "SCV", "", "");
                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "TADR_COM")
                {
                    if (ui.IsCOM)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "SCV", "", "");
                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "TADR_RGD")
                {
                    if (ui.IsRegionalDirector)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "SCV", "", "");
                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "SCV")
                {
                    if (ui.IsSecReqVerifier)
                    {
                        if (Convert.ToBoolean(rdt.Rows[0]["IsInternationalFlight"].ToString()) == true)
                        {
                            result = t.InsertTAStatus(TravelAuthorizationID, "SAR", "", "");
                        }
                        else
                        {
                            if (Convert.ToBoolean(rdt.Rows[0]["IsAccommodationProvided"].ToString()) == true)
                            {
                                result = t.InsertTAStatus(TravelAuthorizationID, "AAC", "", "");
                            }
                            else
                            {
                                result = t.InsertTAStatus(TravelAuthorizationID, "WBSV", "", "");
                            }
                        }

                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "SAR")
                {
                    if (ui.IsHRAttendancePersonnel)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "WBSV", "", "");
                    }
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "AAC")
                {
                    if (ui.IsHOSO)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "WBSV", "", "");
                    }
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "WBSV")
                {
                    if (ui.IsRMO == true)
                    {
                        if (Convert.ToBoolean(rdt.Rows[0]["TravellorIsCOM"].ToString()) == true)
                        {
                            result = t.InsertTAStatus(TravelAuthorizationID, "APP_RGD", "", "");
                        }
                        else if (Convert.ToBoolean(rdt.Rows[0]["TravellorIsHOO"].ToString()) == true)
                        {
                            if (Convert.ToBoolean(rdt.Rows[0]["IsInternationalFlight"].ToString()) == true)
                            {
                                result = t.InsertTAStatus(TravelAuthorizationID, "APP_DCOM", "", "");
                            }
                            else
                            {
                                result = t.InsertTAStatus(TravelAuthorizationID, "APP_COM", "", "");
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(rdt.Rows[0]["IsInternationalFlight"].ToString()) == true)
                            {
                                result = t.InsertTAStatus(TravelAuthorizationID, "APP_COM", "", "");
                            }
                            else
                            {
                                result = t.InsertTAStatus(TravelAuthorizationID, "APP_DCOM", "", "");
                            }
                        }
                    }

                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "APP_DCOM")
                {
                    if (ui.IsHOO)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "TADC", "", "");
                    }
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "APP_COM")
                {
                    if (ui.IsCOM)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "TADC", "", "");
                    }
                }
                else if (rdt.Rows[0]["StatusCode"].ToString() == "APP_RGD")
                {
                    if (ui.IsRegionalDirector)
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "TADC", "", "");
                    }
                }
                if (result == "1")
                {
                    if (rdt.Rows[0]["StatusCode"].ToString() == "APP_DCOM" || rdt.Rows[0]["StatusCode"].ToString() == "APP_COM" || rdt.Rows[0]["StatusCode"].ToString() == "APP_RGD")
                    {
                        Noti.SendTANotificationToOwner(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(),9);
                    }
                    else
                    {
                        Noti.SendTAPendingNotificationEmail(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(), 6);
                    }

                }

            }
            else if (ddlStatusCode.SelectedValue == "SUB_FIN")
            {
                t.GetTAByTANo(TravelAuthorizationNumber, hdnUserID.Value.ToString(),6, ref rdt);
                if (rdt.Rows[0]["StatusCode"].ToString() == "TADC")
                {
                    if (ui.User_Id == hdnUserID.Value || (ui.IsManager && ui.LocationID == hdnLocationID.Value && ui.DepartmentID == hdnDepartmentID.Value && ui.UnitID == hdnUnitID.Value && ui.SubUnitID == hdnSubUnitID.Value))
                    {
                        result = t.InsertTAStatus(TravelAuthorizationID, "TECDC", "", "");
                    }
                }
                if (result == "1")
                {
                    //Pending task notification
                    Noti.SendTAPendingNotificationEmail(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(),6);
                    //Pending task notification
                }

            }
            else if (ddlStatusCode.SelectedValue == "SET")
            {
                t.GetTAByTANo(TravelAuthorizationNumber, hdnUserID.Value.ToString(),12, ref rdt);
                result = t.InsertTAStatus(TravelAuthorizationID, ddlStatusCode.SelectedValue.ToString(), "", "");
                if (result == "1")
                {
                    Noti.SendTANotificationToOwner(rdt.Rows[0]["TravelAuthorizationNumber"].ToString(),12);
                }
            }
            else
            {
                result = t.InsertTAStatus(TravelAuthorizationID, ddlStatusCode.SelectedValue.ToString(), "", "");
            }

            DataTable dtr = new DataTable();
            Sec.GetRoleNameByUserID(ref dtr);
            if (dtr.Rows[0]["RoleName"].ToString() == "Finance")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectFinance()", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectHome()", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectHome()", true);
        }
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx", false);
    }
    protected void ddlStatusCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStatusCode.SelectedValue == hfLatestStatusID.Value)
        {
            btnSaveStatus.Enabled = false;
        }
        else
        {
            btnSaveStatus.Enabled = true;
            txtDocNo.CssClass = "form-control";
        }

        if (ddlStatusCode.SelectedValue.Trim() == "CAN")
        {
            RejectionReasonDiv.Style.Add("visibility", "visible");
            lstRejectionReason.Visible = true;
            lblRejectionReason.Visible = true;
            trRejectComment.Visible = true;
            txtRejectComment.Enabled = true;
            trTADocument.Style.Add("visibility", "hidden");
            DataTable dtReasons = new DataTable();
            l.GetCancellationReason(ref dtReasons, a.Decrypt(Request["TAID"]));
            lstRejectionReason.DataSource = dtReasons;
            lstRejectionReason.DataTextField = "Description";
            lstRejectionReason.DataValueField = "LookupsID";
            lstRejectionReason.DataBind();

            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = a.Decrypt(Request["TAID"]);
                FillCancellationReason(TravelAuthorizationID, "CAN");
            }

        }
        else if (ddlStatusCode.SelectedValue.Trim() == "RTA")
        {
            RejectionReasonDiv.Style.Add("visibility", "visible");
            lstRejectionReason.Visible = true;
            lblRejectionReason.Visible = true;
            trRejectComment.Visible = true;
            txtRejectComment.Enabled = true;
            trTADocument.Style.Add("visibility", "hidden");


            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = a.Decrypt(Request["TAID"]);
                if (hdnCurrentTAStatus.Value == "TADR_SUB_SUP")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons, TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "SUBSUP");
                }
                else if (hdnCurrentTAStatus.Value == "TADR_SUP")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons, TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "SUP");
                }
                else if (hdnCurrentTAStatus.Value == "TADR_DCOM")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons,TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "DCOM");
                }
                else if (hdnCurrentTAStatus.Value == "TADR_COM")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons,TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "COM");
                }
                else if (hdnCurrentTAStatus.Value == "TADR_RGD")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons,TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "RGD");
                }
                else if (hdnCurrentTAStatus.Value == "SCV")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons,TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "SEC");
                }
                else if (hdnCurrentTAStatus.Value == "SAR")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons,TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "HR");
                }
                else if (hdnCurrentTAStatus.Value == "AAC")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons,TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "HOSO");
                }
                else if (hdnCurrentTAStatus.Value == "WBSV")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons,TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "RMO");
                }
                else if (hdnCurrentTAStatus.Value == "APP_DCOM")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons, TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "APPDCOM");
                }
                else if (hdnCurrentTAStatus.Value == "APP_COM")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons, TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "APPCOM");
                }
                else if (hdnCurrentTAStatus.Value == "APP_RGD")
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons, TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "APPRGD");
                }
                else if (hdnCurrentTAStatus.Value == "TECDC")
                {
                    DataTable dtReasonsTEC = new DataTable();
                    l.GetTECRejectionReason(ref dtReasonsTEC);
                    lstRejectionReason.DataSource = dtReasonsTEC;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "TEC");
                }
                else if (hdnCurrentTAStatus.Value == "TECCom")
                {
                    DataTable dtReasonsTEC = new DataTable();
                    l.GetTECReturnedRejectionReason(ref dtReasonsTEC);
                    lstRejectionReason.DataSource = dtReasonsTEC;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "TECRTA");
                }
                else
                {
                    DataTable dtReasons = new DataTable();
                    l.GetReturnReasons(ref dtReasons, TravelAuthorizationID);
                    lstRejectionReason.DataSource = dtReasons;
                    lstRejectionReason.DataTextField = "Description";
                    lstRejectionReason.DataValueField = "LookupsID";
                    lstRejectionReason.DataBind();
                    FillReturnReason(TravelAuthorizationID, "TA");
                }

            }
        }
        else if (ddlStatusCode.SelectedValue.Trim() == "SET")
        {
            RejectionReasonDiv.Style.Add("visibility", "hidden");
            txtDocNo.CssClass = "form-control Req";
            lstRejectionReason.Visible = false;
            lblRejectionReason.Visible = false;
            trRejectComment.Visible = false;
            trTADocument.Style.Add("visibility", "visible");
        }
        else
        {
            RejectionReasonDiv.Style.Add("visibility", "hidden");
            lstRejectionReason.Visible = false;
            lblRejectionReason.Visible = false;
            trRejectComment.Visible = false;
            trTADocument.Style.Add("visibility", "hidden");
        }
    }
    protected void gvHistoryStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvHistoryStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void txtDocNo_TextChanged(object sender, EventArgs e)
    {
        t.InsertTADocumentNumber(a.Decrypt(Request["TAID"]), txtDocNo.Text.Trim());

        PanelMessage.Visible = true;
        PanelMessage.CssClass = "alert alert-success alert-dismissable";
        lblmsg.ForeColor = Color.Green;
        lblmsg.Text = "Document number has been saved successfully";
    }
    #endregion

    #region steps
    void IndicateStep()
    {
        double Progress = 0.0;
        bool ShowTEC = false;

        if (!string.IsNullOrEmpty(Request["TAID"] as string) && !string.IsNullOrEmpty(Request["TANO"] as string))
        {
            lblTitle.Text = "Travel Authorization - " + a.Decrypt(Request["TANO"].ToString());
            string Step;

            Step = t.GetTAStepByTravelAuthorizationID(a.Decrypt(Request["TAID"].ToString()));
            ShowTEC = FillStatus();

            if (Step == "Step 1")
            {
                if (ShowTEC)
                {
                    Progress = 5;
                }
                else
                {
                    Progress = 20;
                    tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                }

                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("Step1_TravelersInformation.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }

            }
            else if (Step == "Step 2")
            {
                if (ShowTEC)
                {
                    Progress = 15;
                }
                else
                {
                    Progress = 40;
                    tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                    tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                }

                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("Step2_AdvanceAndSecurity.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }

                TAStatusDiv.Visible = false;
                lbStep3.Visible = false;
                lbStep4.Visible = false;
                lbStep5.Visible = false;
                lbStepDownload.Visible = false;
                lbStep6.Visible = false;
                lbStep7.Visible = false;
                lbStep8.Visible = false;
                lbStep9.Visible = false;


            }
            else if (Step == "Step 3")
            {
                if (ShowTEC)
                {
                    Progress = 25;
                }
                else
                {
                    Progress = 60;
                    tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                    tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                    tdStep3.Attributes["style"] = "width: 20%; text-align: center";
                }
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("Step3_WBS.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }
                //if (!string.IsNullOrEmpty(Request["Step"] as string))
                //{
                //    if ((Request["Step"].ToString() != "Step 3"))
                //        Response.Redirect("Step3_WBS.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                //}

                TAStatusDiv.Visible = false;
                lbStep4.Visible = false;
                lbStepDownload.Visible = false;
                lbStep5.Visible = false;
                lbStep6.Visible = false;
                lbStep7.Visible = false;
                lbStep8.Visible = false;
                lbStep9.Visible = false;

            }
            else if (Step == "Step 4")
            {
                if (ShowTEC)
                {
                    Progress = 35;
                }
                else
                {
                    Progress = 80;
                    tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                    tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                    tdStep3.Attributes["style"] = "width: 20%; text-align: center";
                    tdStep4.Attributes["style"] = "width: 20%; text-align: center";
                }
                // Hiding other steps
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }
                //if (!string.IsNullOrEmpty(Request["Step"] as string))
                //{
                //    if ((Request["Step"].ToString() != "Step 4"))
                //        Response.Redirect("Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                //}
                TAStatusDiv.Visible = false;
                lbStepDownload.Visible = false;
                lbStep5.Visible = false;
                lbStep6.Visible = false;
                lbStep7.Visible = false;
                lbStep8.Visible = false;
                lbStep9.Visible = false;

            }
            else if (Step == "Step 5" && !ShowTEC)
            {
                Progress = 100;

                tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                tdStep3.Attributes["style"] = "width: 20%; text-align: center";
                tdStep4.Attributes["style"] = "width: 20%; text-align: center";
                tdStep5.Attributes["style"] = "width: 20%; text-align: center";
                // Hiding other steps
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("DownloadTA.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }
                //if (!string.IsNullOrEmpty(Request["Step"] as string))
                //{
                //    if ((Request["Step"].ToString() != "Step 4"))
                //        Response.Redirect("Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                //}

                lbStep5.Visible = false;
                lbStep6.Visible = false;
                lbStep7.Visible = false;
                lbStep8.Visible = false;
                lbStep9.Visible = false;
            }

            if (ShowTEC)
            {
                if (Step == "Step 5")
                {
                    Progress = 55;
                    // Hiding other steps
                    if (string.IsNullOrEmpty(Request["First"] as string))
                    {
                        Response.Redirect("Step5_TECItinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                    }

                    lbStep6.Visible = false;
                    lbStep7.Visible = false;
                    lbStep8.Visible = false;
                    lbStep9.Visible = false;
                }
                else if (Step == "Step 6")
                {
                    Progress = 65;
                    // Hiding other steps
                    if (string.IsNullOrEmpty(Request["First"] as string))
                    {
                        Response.Redirect("Step6_TECExpenses.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                    }

                    lbStep7.Visible = false;
                    lbStep8.Visible = false;
                    lbStep9.Visible = false;
                }
                else if (Step == "Step 7")
                {
                    Progress = 75;
                    // Hiding other steps
                    if (string.IsNullOrEmpty(Request["First"] as string))
                    {
                        Response.Redirect("Step7_TECAdvances.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                    }

                    lbStep8.Visible = false;
                    lbStep9.Visible = false;
                }
                else if (Step == "Step 8")
                {
                    Progress = 85;
                    // Hiding other steps
                    if (string.IsNullOrEmpty(Request["First"] as string))
                    {
                        Response.Redirect("Step8_CheckList.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                    }
                    lbStep9.Visible = false;
                }
                else if (Step == "Step 9")
                {
                    Progress = 100;
                    // Hiding other steps
                    if (string.IsNullOrEmpty(Request["First"] as string))
                    {
                        Response.Redirect("DownloadTEC.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                    }
                }

            }
            else if (Step == "Step 6" | Step == "Step 7" | Step == "Step 8" | Step == "Step 9" | Step == "Step 10")
            {
                Progress = 45;
                // Hiding other steps
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("DownloadTA.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }

                lbStep5.Visible = false;
                lbStep6.Visible = false;
                lbStep7.Visible = false;
                lbStep8.Visible = false;
                lbStep9.Visible = false;
            }
            //FillStatus();
        }
        else
        {
            lblTitle.Text = "Travel Authorization";
            if (ShowTEC)
            {
                Progress = 5;
            }
            else
            {
                Progress = 20;
                tdStep1.Attributes["style"] = "width: 20%; text-align: center";
            }
            if (string.IsNullOrEmpty(Request["First"] as string))
            {
                Response.Redirect("Step1_TravelersInformation.aspx?First=1", false);
            }

            TAStatusDiv.Visible = false;
            lbStep2.Visible = false;
            lbStep3.Visible = false;
            lbStep4.Visible = false;
            lbStepDownload.Visible = false;
            lbStep5.Visible = false;
            lbStep6.Visible = false;
            lbStep7.Visible = false;
            lbStep8.Visible = false;
            lbStep9.Visible = false;
        }
        ltrProgress.Text = "<div class='progress progress-striped active'><div class='progress-bar progress-bar-primary' role='progressbar' aria-valuenow='20' aria-valuemin='0' aria-valuemax='100' style='width: " + Progress + "%'></div></div>";

    }
    protected void lbStep1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step1_TravelersInformation.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 1", false);
    }
    protected void lbStep2_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step2_AdvanceAndSecurity.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 2", false);
    }
    protected void lbStep3_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step3_WBS.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 3", false);
    }
    protected void lbStep4_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 4", false);
    }
    protected void lbStepDownload_Click(object sender, EventArgs e)
    {
        Response.Redirect("DownloadTA.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 5", false);
    }
    protected void lbStep5_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step5_TECItinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 6", false);
    }
    protected void lbStep6_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step6_TECExpenses.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 7", false);
    }
    protected void lbStep7_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step7_TECAdvances.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 8", false);
    }
    protected void lbStep8_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step8_CheckList.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 9", false);
    }
    protected void lbStep9_Click(object sender, EventArgs e)
    {
        Response.Redirect("DownloadTEC.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1" + "&Step=Step 10", false);
    }

    #endregion

}