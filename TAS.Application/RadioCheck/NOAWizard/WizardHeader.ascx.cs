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





public partial class RadioCheck_NOAWizard_WizardHeader : System.Web.UI.UserControl
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    AuthenticatedPageClass a = new AuthenticatedPageClass();
    Business.Security Sec = new Business.Security();
    HttpContext context = HttpContext.Current;
    Business.MailModel MM = new Business.MailModel();
    //TravelAuthorizationDataModel db = new TravelAuthorizationDataModel();
    Globals g = new Globals();

    public object MR { get; private set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //CheckStatusPermission();
            IndicateStep();
        }
    }
    void FillDDLs()
    {
        try
        {
            DataSet ds = new DataSet();
            R.GetAllLookupsList(ref ds);
            ddlStatusCode.DataSource = ds.Tables[8];
            ddlStatusCode.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void FillStatus()
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string MovementRequestID = a.Decrypt(Request["MRID"]);
            R.GetMovementRequestByMovementRequestID(MovementRequestID, ref dt);
            FillStatusHistory(MovementRequestID);

            //fill ddlstatus
            R.GetAllLookupsList(ref ds);
            DataView dv = new DataView();
            dv = ds.Tables[8].DefaultView;
            //dv.RowFilter = "Code IN (" + dt.Rows[0]["LongDescription"].ToString() + ") OR Code = '" + dt.Rows[0]["Code"].ToString() + "'";
            dv.RowFilter = "Code IN (" + dt.Rows[0]["LongDescription"].ToString() + ") OR Code = ''";
            ddlStatusCode.DataSource = dv;
            ddlStatusCode.DataBind();
            //ddlStatusCode.SelectedValue = dt.Rows[0]["Code"].ToString();
            ddlStatusCode.SelectedValue = "";
            Objects.User ui = (Objects.User)context.Session["userinfo"];

            hdnCurrentMRStatus.Value = dt.Rows[0]["StatusCode"].ToString();
            hdnCurrentStatusDescription.Value = dt.Rows[0]["MRStatus"].ToString();
            hdnUserID.Value = dt.Rows[0]["UserID"].ToString();
            CurrentMRStatus.Text = dt.Rows[0]["MRStatus"].ToString(); ;
            CurrentMRStatus.Enabled = false;


            //For sending emails
            string UserID = dt.Rows[0]["UserID"].ToString();
            hdnUserID.Value = dt.Rows[0]["UserID"].ToString();

            if (dt.Rows[0]["Code"].ToString() == "NC")
            {
                btnSaveStatus.Enabled = false;
                ddlStatusCode.Enabled = false;
                txtRejectComment.Enabled = false;
                CurrentMRStatus.Visible = true;
                ddlStatusCode.Visible = false;
                btnSaveStatus.Visible = false;
                Condit.Visible = false;

                DataTable dtReasons = new DataTable();
                R.GetCancellationReasons(ref dtReasons);
                lstRejectionReason.DataSource = dtReasons;
                lstRejectionReason.DataTextField = "Description";
                lstRejectionReason.DataValueField = "LookupsID";
                lstRejectionReason.DataBind();

                FillCancellationReason(a.Decrypt(Request["MRID"]), "NC");
                txtRejectComment.Visible = true;
                RejectionReasonDiv.Visible = true;
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Cancelled  Notification of absence cannot be modified.  Please email IOM South Sudan IMU Support ssudanimu@iom.int for the change request.";
            }
            else if (dt.Rows[0]["Code"].ToString() == "NP")
            {
                if (ui.User_Id == hdnUserID.Value )
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    txtRejectComment.Enabled = true;
                    CurrentMRStatus.Visible = false;
                    ddlStatusCode.Visible = true;
                    btnSaveStatus.Visible = true;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    ddlStatusCode.Enabled = false;
                    txtRejectComment.Enabled = false;
                    CurrentMRStatus.Visible = true;
                    ddlStatusCode.Visible = false;
                    btnSaveStatus.Visible = false;
                    Condit.Visible = false;
                }

            }
            else if (dt.Rows[0]["Code"].ToString() == "NR")
            {

                DataTable dtReasons = new DataTable();
                R.GetMRReturnReasons(ref dtReasons);
                lstRejectionReason.DataSource = dtReasons;
                lstRejectionReason.DataTextField = "Description";
                lstRejectionReason.DataValueField = "LookupsID";
                lstRejectionReason.DataBind();

                if (ui.User_Id == hdnUserID.Value)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    CurrentMRStatus.Visible = false;
                    ddlStatusCode.Visible = true;
                    btnSaveStatus.Visible = true;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    ddlStatusCode.Enabled = false;
                    CurrentMRStatus.Visible = true;
                    ddlStatusCode.Visible = false;
                    btnSaveStatus.Visible = false;
                    Condit.Visible = false;
                }

                FillReturnReason(a.Decrypt(Request["MRID"]), "NR");
                txtRejectComment.Enabled = false;

            }
            else if (dt.Rows[0]["Code"].ToString() == "NS")
            {
                if (ui.IsSupervisor)
                {
                    btnSaveStatus.Enabled = true;
                    ddlStatusCode.Enabled = true;
                    ddlStatusCode.Visible = true;
                    CurrentMRStatus.Visible = false;
                    Condit.Visible = true;
                }
                else
                {
                    btnSaveStatus.Enabled = false;
                    btnSaveStatus.Visible = false;
                    ddlStatusCode.Enabled = false;
                    ddlStatusCode.Visible = false;
                    CurrentMRStatus.Visible = true;
                    Condit.Visible = false;
                }
            }
            else if (dt.Rows[0]["Code"].ToString() == "NA")
            {

                btnSaveStatus.Enabled = false;
                ddlStatusCode.Enabled = false;
                txtRejectComment.Enabled = false;
                CurrentMRStatus.Visible = true;
                ddlStatusCode.Visible = false;
                btnSaveStatus.Visible = false;
                Condit.Visible = false;
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Approved notifications cannot be modified.  Please email IOM South Sudan IMU Support ssudanimu@iom.int for the change request.";
            }
            else
            {
                btnSaveStatus.Enabled = false;
                ddlStatusCode.Enabled = false;
            }

            if (ddlStatusCode.SelectedValue == "" || ddlStatusCode.SelectedValue == dt.Rows[0]["Code"].ToString().ToUpper())
            {
                btnSaveStatus.Enabled = false;
                txtRejectComment.Enabled = false;
            }


        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }

    }
    protected void FillStatusHistory(string MovementRequestID)
    {
        try
        {
            DataTable dtHistory = new DataTable();
            R.GetMRStatusHistory(MovementRequestID, ref dtHistory);
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
    private void FillReturnReason(string MovementRequestID, string RejectionReasonType)
    {
        RejectionReasonDiv.Style.Add("visibility", "visible");
        lstRejectionReason.Visible = true;
        lblRejectionReason.Visible = true;
        trRejectComment.Visible = true;
        txtRejectComment.Enabled = true;

        DataTable dtREjectionReasons = new DataTable();
        R.GetMRReturnReason(MovementRequestID, RejectionReasonType, ref dtREjectionReasons);
        if (dtREjectionReasons.Rows.Count > 0)
            txtRejectComment.Text = dtREjectionReasons.Rows[0]["RejectionComment"].ToString();
        for (int i = 0; i < dtREjectionReasons.Rows.Count; i++)
        {
            txtRejectComment.Text = dtREjectionReasons.Rows[i]["RejectionComment"].ToString();
            for (int j = 0; j < lstRejectionReason.Items.Count; j++)
            {

                if (dtREjectionReasons.Rows[i]["RejectionReasonID"].ToString() == lstRejectionReason.Items[j].Value.ToString())
                {
                    lstRejectionReason.Items[j].Selected = true;
                }
            }
        }
    }
    private void FillCancellationReason(string MovementRequestID, string RejectionReasonType)
    {
        RejectionReasonDiv.Style.Add("visibility", "visible");
        lstRejectionReason.Visible = true;
        lblRejectionReason.Visible = true;
        trRejectComment.Visible = true;
        txtRejectComment.Enabled = true;

        DataTable dtREjectionReasons = new DataTable();
        R.GetMRCancellationReason(MovementRequestID, RejectionReasonType, ref dtREjectionReasons);
        if (dtREjectionReasons.Rows.Count > 0)
            txtRejectComment.Text = dtREjectionReasons.Rows[0]["RejectionComment"].ToString();
        for (int i = 0; i < dtREjectionReasons.Rows.Count; i++)
        {
            txtRejectComment.Text = dtREjectionReasons.Rows[i]["RejectionComment"].ToString();
            for (int j = 0; j < lstRejectionReason.Items.Count; j++)
            {

                if (dtREjectionReasons.Rows[i]["RejectionReasonID"].ToString() == lstRejectionReason.Items[j].Value.ToString())
                {
                    lstRejectionReason.Items[j].Selected = true;
                }
            }
        }
    }

    //protected void btnSaveStatus_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string MovementRequestNumber = "";
    //        string MovementRequestID = "";

    //        if (hdnUserID.Value.ToString() == "")
    //        {
    //            return;
    //        }
    //        Objects.User ui = (Objects.User)context.Session["userinfo"];

    //        MovementRequestNumber = a.Decrypt(Request["MRNO"]);
    //        MovementRequestID = a.Decrypt(Request["MRID"]);
    //        string result;
    //        bool IsReason = false;
    //        if (ddlStatusCode.SelectedValue == "SUBA" || ddlStatusCode.SelectedValue == "CRC")
    //        {
    //            result = R.InsertMRStatus(MovementRequestID, "NS", "", "").ToString();
    //        }
    //        else if (ddlStatusCode.SelectedValue == "NAPP")
    //        {
    //            result = R.InsertMRStatus(MovementRequestID, "NA", "", "").ToString();
    //        }
    //        else if (ddlStatusCode.SelectedValue == "CNOA")
    //        {
    //            string Reasons = "";
    //            for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
    //            {
    //                if (lstRejectionReason.Items[i].Selected)
    //                {
    //                    IsReason = true;
    //                    Reasons += lstRejectionReason.Items[i].Text + " - ";
    //                }
    //            }

    //            if (IsReason)
    //            {
    //                Reasons = Reasons.Substring(0, Reasons.Length - 3);
    //                result = R.InsertMRStatus(MovementRequestID, "NC", Reasons, "");
    //            }
    //            else
    //            {
    //                PanelMessage.Visible = true;
    //                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
    //                lblmsg.ForeColor = Color.Red;
    //                lblmsg.Text = "Please select the cancellation reasons";
    //                return;
    //            }
    //        }
    //        else if (ddlStatusCode.SelectedValue == "RNOA")
    //        {
    //            string Reasons = "";
    //            for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
    //            {
    //                if (lstRejectionReason.Items[i].Selected)
    //                {
    //                    IsReason = true;
    //                    Reasons += lstRejectionReason.Items[i].Text + " - ";
    //                }
    //            }

    //            if (IsReason)
    //            {
    //                Reasons = Reasons.Substring(0, Reasons.Length - 3);
    //                result = R.InsertMRStatus(MovementRequestID, "NR", Reasons, "");
    //            }
    //            else
    //            {
    //                PanelMessage.Visible = true;
    //                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
    //                lblmsg.ForeColor = Color.Red;
    //                lblmsg.Text = "Please select the return reasons";
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            result = R.InsertMRStatus(MovementRequestID, ddlStatusCode.SelectedValue.ToString(), "", "").ToString();
    //        }

    //        DataTable rdt = new DataTable();
    //        R.GetMRByMRNo(ref rdt, MovementRequestNumber);

    //        //var rdt = (from d in db.V_D_MovementRequest
    //        //           where d.MovementRequestNumber == MovementRequestNumber
    //        //           select d).FirstOrDefault();

    //        if (result == "1")
    //        {
    //            DataTable EmailType = new DataTable();
    //            DataTable Receipient = new DataTable();
    //            hfLatestStatusID.Value = ddlStatusCode.SelectedValue;
    //            btnSaveStatus.Enabled = false;
    //            PanelMessage.Visible = true;
    //            PanelMessage.CssClass = "alert alert-success alert-dismissable";
    //            lblmsg.ForeColor = Color.Green;
    //            lblmsg.Text = "Status has been updated successfully";

    //            if (ddlStatusCode.SelectedValue == "NAPP")
    //            {
    //                //Approval Notification
    //                DataTable Receipients = new DataTable();
    //                R.GetEmailContent(ref EmailType, 4);
    //                if (EmailType.Rows.Count > 0)
    //                {
    //                    MM.To =  rdt.TravellorEmail==null ? rdt.CreatorEmail : rdt.TravellorEmail;
    //                    MM.Subject = EmailType.Rows[0]["Subject"].ToString();
    //                    MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", rdt.TravellorFullName);
    //                    MM.SendMail();

    //                }
    //                //Approval Notification

    //            }
    //            else if (ddlStatusCode.SelectedValue == "CRC" || ddlStatusCode.SelectedValue == "SFA")
    //            {
    //                //Start the Emailing
    //                DataTable Receipients = new DataTable();
    //                R.GetEmailContent(ref EmailType, 1);
    //                R.GetMREmailReceipients(ref Receipients, MovementRequestNumber);
    //                if (EmailType.Rows.Count > 0 && Receipients.Rows.Count > 0)
    //                {
    //                    for (var i = 0; i < Receipients.Rows.Count; i += 1)
    //                    {
    //                        MM.To = Receipients.Rows[i]["Email"].ToString();
    //                        MM.Subject = EmailType.Rows[0]["Subject"].ToString();
    //                        MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", Receipients.Rows[i]["DisplayName"].ToString());
    //                        MM.SendMail();
    //                    }

    //                }
    //                //End the Emailing
    //            }
    //            else if (ddlStatusCode.SelectedValue == "CNOA")
    //            {
    //                string RejectionReasons = "";
    //                for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
    //                {
    //                    if (lstRejectionReason.Items[i].Selected)
    //                    {
    //                        RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
    //                    }
    //                }

    //                if (RejectionReasons == "")
    //                {
    //                    PanelMessage.Visible = true;
    //                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
    //                    lblmsg.ForeColor = Color.Red;
    //                    lblmsg.Text = "Please select the rejection reasons";
    //                    return;
    //                }

    //                for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
    //                {
    //                    if (lstRejectionReason.Items[i].Selected)
    //                    {
    //                        R.InsertRejectionReasons(MovementRequestID, lstRejectionReason.Items[i].Value, "NC", txtRejectComment.Text);
    //                    }
    //                }

    //                //Start the Emailing
    //                R.GetMovementRequestByMovementRequestID(MovementRequestID, ref Receipient);
    //                if (Receipient.Rows.Count > 0)
    //                {
    //                    //Travellor is the Creator
    //                    R.GetEmailContent(ref EmailType, 3);
    //                    if (EmailType.Rows.Count > 0)
    //                    {
    //                        MM.To = Receipient.Rows[0]["TravellorEmail"].ToString();
    //                        MM.Subject = EmailType.Rows[0]["Subject"].ToString();
    //                        MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", Receipient.Rows[0]["TravelersName"].ToString());
    //                        MM.SendMail();
    //                    }
    //                    //Travellor is the Creator

    //                }
    //                //End the Emailing
    //            }
    //            else if (ddlStatusCode.SelectedValue == "RNOA")
    //            {
    //                string RejectionReasons = "";
    //                for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
    //                {
    //                    if (lstRejectionReason.Items[i].Selected)
    //                    {
    //                        RejectionReasons = RejectionReasons + "," + lstRejectionReason.Items[i].Text;
    //                    }
    //                }

    //                if (RejectionReasons == "")
    //                {
    //                    PanelMessage.Visible = true;
    //                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
    //                    lblmsg.ForeColor = Color.Red;
    //                    lblmsg.Text = "Please select the rejection reasons";
    //                    return;
    //                }

    //                for (int i = 0; i <= lstRejectionReason.Items.Count - 1; i++)
    //                {
    //                    if (lstRejectionReason.Items[i].Selected)
    //                    {
    //                        R.InsertRejectionReasons(MovementRequestID, lstRejectionReason.Items[i].Value, "NR", txtRejectComment.Text);
    //                    }
    //                }

    //                //Start the Emailing
    //                DataTable Receipients = new DataTable();
    //                R.GetEmailContent(ref EmailType, 2);
    //                if (EmailType.Rows.Count > 0)
    //                {
    //                    MM.To = rdt.TravellorEmail;
    //                    MM.Subject = EmailType.Rows[0]["Subject"].ToString();
    //                    MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", rdt.TravellorFullName);
    //                    MM.SendMail();

    //                }
    //                //End the Emailing

    //            }
    //            if (ui.IsSupervisor)
    //            {
    //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectToSupervisorHome()", true);
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectToStaffHome()", true);
    //            }

    //            //Response.Redirect("1_NotifiersInformation.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
    //        }
    //        else
    //        {
    //            PanelMessage.Visible = true;
    //            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
    //            lblmsg.ForeColor = Color.Red;
    //            lblmsg.Text = "The status for this Notification is already " + ddlStatusCode.SelectedItem.Text;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        PanelMessage.Visible = true;
    //        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
    //        lblmsg.ForeColor = Color.Red;
    //        lblmsg.Text = "Something went wrong " + ex.Message + " , Please contact  your system administrator";

    //    }
    //}

    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        try
        {
            string MovementRequestNumber = "";
            string MovementRequestID = "";

            if (hdnUserID.Value.ToString() == "")
            {
                return;
            }
            Objects.User ui = (Objects.User)context.Session["userinfo"];

            MovementRequestNumber = a.Decrypt(Request["MRNO"]);
            MovementRequestID = a.Decrypt(Request["MRID"]);
            string result;
            bool IsReason = false;
            if (ddlStatusCode.SelectedValue == "SUBA" || ddlStatusCode.SelectedValue == "CRC")
            {
                result = R.InsertMRStatus(MovementRequestID, "NS", "", "").ToString();
            }
            else if (ddlStatusCode.SelectedValue == "NAPP")
            {
                result = R.InsertMRStatus(MovementRequestID, "NA", "", "").ToString();
            }
            else if (ddlStatusCode.SelectedValue == "CNOA")
            {
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
                    result = R.InsertMRStatus(MovementRequestID, "NC", Reasons, "");
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please select the cancellation reasons";
                    return;
                }
            }
            else if (ddlStatusCode.SelectedValue == "RNOA")
            {
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
                    result = R.InsertMRStatus(MovementRequestID, "NR", Reasons, "");
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please select the return reasons";
                    return;
                }
            }
            else
            {
                result = R.InsertMRStatus(MovementRequestID, ddlStatusCode.SelectedValue.ToString(), "", "").ToString();
            }

            DataTable rdt = new DataTable();
            R.GetMRByMRNo(ref rdt, MovementRequestNumber);

            //var rdt = (from d in db.V_D_MovementRequest
            //           where d.MovementRequestNumber == MovementRequestNumber
            //           select d).FirstOrDefault();

            if (result == "1")
            {
                DataTable EmailType = new DataTable();
                DataTable Receipient = new DataTable();
                hfLatestStatusID.Value = ddlStatusCode.SelectedValue;
                btnSaveStatus.Enabled = false;
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Status has been updated successfully";

                if (ddlStatusCode.SelectedValue == "NAPP")
                {
                    //Approval Notification
                    DataTable Receipients = new DataTable();
                    R.GetEmailContent(ref EmailType, 4);
                    if (EmailType.Rows.Count > 0)
                    {
                        MM.To = rdt.Rows[0]["TravellorEmail"].ToString() == null ? rdt.Rows[0][".CreatorEmail"].ToString() : rdt.Rows[0][".TravellorEmail"].ToString();
                        MM.Subject = EmailType.Rows[0]["Subject"].ToString();
                        MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", rdt.Rows[0][".TravellorFullName"].ToString());
                        MM.SendMail();

                    }
                    //Approval Notification

                }
                else if (ddlStatusCode.SelectedValue == "CRC" || ddlStatusCode.SelectedValue == "SFA")
                {
                    //Start the Emailing
                    DataTable Receipients = new DataTable();
                    R.GetEmailContent(ref EmailType, 1);
                    R.GetMREmailReceipients(ref Receipients, MovementRequestNumber);
                    if (EmailType.Rows.Count > 0 && Receipients.Rows.Count > 0)
                    {
                        for (var i = 0; i < Receipients.Rows.Count; i += 1)
                        {
                            MM.To = Receipients.Rows[i]["Email"].ToString();
                            MM.Subject = EmailType.Rows[0]["Subject"].ToString();
                            MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", Receipients.Rows[i]["DisplayName"].ToString());
                            MM.SendMail();
                        }

                    }
                    //End the Emailing
                }
                else if (ddlStatusCode.SelectedValue == "CNOA")
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
                            R.InsertRejectionReasons(MovementRequestID, lstRejectionReason.Items[i].Value, "NC", txtRejectComment.Text);
                        }
                    }

                    //Start the Emailing
                    R.GetMovementRequestByMovementRequestID(MovementRequestID, ref Receipient);
                    if (Receipient.Rows.Count > 0)
                    {
                        //Travellor is the Creator
                        R.GetEmailContent(ref EmailType, 3);
                        if (EmailType.Rows.Count > 0)
                        {
                            MM.To = Receipient.Rows[0]["TravellorEmail"].ToString();
                            MM.Subject = EmailType.Rows[0]["Subject"].ToString();
                            MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", Receipient.Rows[0]["TravelersName"].ToString());
                            MM.SendMail();
                        }
                        //Travellor is the Creator

                    }
                    //End the Emailing
                }
                else if (ddlStatusCode.SelectedValue == "RNOA")
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
                            R.InsertRejectionReasons(MovementRequestID, lstRejectionReason.Items[i].Value, "NR", txtRejectComment.Text);
                        }
                    }

                    //Start the Emailing
                    DataTable Receipients = new DataTable();
                    R.GetEmailContent(ref EmailType, 2);
                    if (EmailType.Rows.Count > 0)
                    {
                        MM.To = rdt.Rows[0][".TravellorEmail"].ToString();
                        MM.Subject = EmailType.Rows[0]["Subject"].ToString();
                        MM.Body = EmailType.Rows[0]["EmailBody"].ToString().Replace("<<UserName>>", rdt.Rows[0][".TravellorFullName"].ToString());
                        MM.SendMail();

                    }
                    //End the Emailing

                }
                if (ui.IsSupervisor)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectToSupervisorHome()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RedirectToStaffHome()", true);
                }

                //Response.Redirect("1_NotifiersInformation.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
            }
            else
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "The status for this Notification is already " + ddlStatusCode.SelectedItem.Text;
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
        if (ddlStatusCode.SelectedValue == hfLatestStatusID.Value)
        {
            btnSaveStatus.Enabled = false;
        }
        else
        {
            btnSaveStatus.Enabled = true;
        }
        txtDocNo.CssClass = "form-control";
        if (ddlStatusCode.SelectedValue == "RNOA")
        {
            RejectionReasonDiv.Style.Add("visibility", "visible");
            lstRejectionReason.Visible = true;
            lblRejectionReason.Visible = true;
            trRejectComment.Visible = true;
            trMRDocument.Style.Add("visibility", "hidden");
            txtRejectComment.Enabled = true;

            DataTable dtReturnReasons = new DataTable();
            R.GetReturnReasons(ref dtReturnReasons);
            lstRejectionReason.DataSource = dtReturnReasons;
            lstRejectionReason.DataTextField = "Description";
            lstRejectionReason.DataValueField = "LookupsID";
            lstRejectionReason.DataBind();

            if (!string.IsNullOrEmpty(Request["MRID"] as string))
            {
                string MovementRequestID = a.Decrypt(Request["MRID"]);
                FillReturnReason(MovementRequestID, "NR");
            }

        }
        else if (ddlStatusCode.SelectedValue == "CNOA")
        {
            RejectionReasonDiv.Style.Add("visibility", "visible");
            lstRejectionReason.Visible = true;
            lblRejectionReason.Visible = true;
            trRejectComment.Visible = true;
            trMRDocument.Style.Add("visibility", "hidden");
            txtRejectComment.Enabled = true;

            DataTable dtCanReasons = new DataTable();
            R.GetCancellationReasons(ref dtCanReasons);
            lstRejectionReason.DataSource = dtCanReasons;
            lstRejectionReason.DataTextField = "Description";
            lstRejectionReason.DataValueField = "LookupsID";
            lstRejectionReason.DataBind();

            if (!string.IsNullOrEmpty(Request["MRID"] as string))
            {
                string MovementRequestID = a.Decrypt(Request["MRID"]);
                FillCancellationReason(MovementRequestID, "NC");
            }
        }

        else
        {
            RejectionReasonDiv.Style.Add("visibility", "hidden");
            lstRejectionReason.Visible = false;
            lblRejectionReason.Visible = false;
            trRejectComment.Visible = false;
            //trMRDocument.Style.Add("visibility", "hidden");
        }
    }
    #region steps
    void IndicateStep()
    {
        double Progress = 0.0;

        if (!string.IsNullOrEmpty(Request["MRID"] as string) && !string.IsNullOrEmpty(Request["MRNO"] as string))
        {
            lblTitle.Text = "Notification of absence - " + a.Decrypt(Request["MRNO"].ToString());
            string Step;

            Step = R.GetMRStepByMovementRequestID(a.Decrypt(Request["MRID"].ToString()));
            FillStatus();

            if (Step == "Step 1")
            {
                Progress = 25;
                tdStep1.Attributes["style"] = "width: 25%; text-align: center";
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("1_NotifiersInformation.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
                }
            }
            else if (Step == "Step 2")
            {
                Progress = 50;
                tdStep1.Attributes["style"] = "width: 25%; text-align: center";
                tdStep2.Attributes["style"] = "width: 25%; text-align: center";

                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("2_AbsenceDates.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
                }

                NOAStatusDiv.Visible = false;
                lbStep3.Visible = false;
                lbStepDownload.Visible = false;
            }
            else if (Step == "Step 3")
            {
                Progress = 75;
                tdStep1.Attributes["style"] = "width: 25%; text-align: center";
                tdStep2.Attributes["style"] = "width: 25%; text-align: center";
                tdStep3.Attributes["style"] = "width: 25%; text-align: center";
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("3_AbsenceDetails.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
                }

                NOAStatusDiv.Visible = false;
                lbStepDownload.Visible = false;
            }

            else if (Step == "Step 4")
            {
                Progress = 100;
                tdStep1.Attributes["style"] = "width: 25%; text-align: center";
                tdStep2.Attributes["style"] = "width: 25%; text-align: center";
                tdStep3.Attributes["style"] = "width: 25%; text-align: center";
                tdStep4.Attributes["style"] = "width: 25%; text-align: center";

                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("4_DownloadNOA.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
                }
            }
            FillStatus();
        }
        else
        {
            lblTitle.Text = "Notification of absence";
            Progress = 20;
            tdStep1.Attributes["style"] = "width: 20%; text-align: center";
            if (string.IsNullOrEmpty(Request["First"] as string))
            {
                Response.Redirect("1_NotifiersInformation.aspx?First=1", false);
            }
            NOAStatusDiv.Visible = false;
            lbStep2.Visible = false;
            lbStep3.Visible = false;
            lbStepDownload.Visible = false;
        }
        ltrProgress.Text = "<div class='progress progress-striped active'><div class='progress-bar progress-bar-primary' role='progressbar' aria-valuenow='20' aria-valuemin='0' aria-valuemax='100' style='width: " + Progress + "%'></div></div>";

    }
    protected void lbStep1_Click(object sender, EventArgs e)
    {
        Response.Redirect("1_NotifiersInformation.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1" + "&Step=Step 1", false);
    }
    protected void lbStep2_Click(object sender, EventArgs e)
    {
        Response.Redirect("2_AbsenceDates.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1" + "&Step=Step 2", false);
    }
    protected void lbStep3_Click(object sender, EventArgs e)
    {
        Response.Redirect("3_AbsenceDetails.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1" + "&Step=Step 3", false);
    }
    protected void lbStepDownload_Click(object sender, EventArgs e)
    {
        Response.Redirect("4_DownloadNOA.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1" + "&Step=Step 4", false);
    }
    #endregion

}
