
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

public partial class TravelAuthorization_TAWizard_Step2_AdvanceAndSecurity : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.Security s = new Business.Security();
    Business.Media Media = new Business.Media();
    Globals g = new Globals();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl TAStatus = null;
            TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
            TAStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep2");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            DataTable dtRoleName = new DataTable();
            s.GetRoleNameByUserID(ref dtRoleName);

            if (dtRoleName.Rows.Count > 0)
            {
                if (dtRoleName.Rows[0]["RoleName"].ToString() == "HR/Admin")
                {
                    rowDSANotApp.Visible = true;
                }
                else
                {
                    rowDSANotApp.Visible = false;
                }
            }

            FillDDLs();

            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());
                FillTA(TravelAuthorizationID);
                hftaid.Value = TravelAuthorizationID;

                checkStatus();


                //lock content
                DataTable dt = new DataTable();
                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsEditable"].ToString()) == false)
                    {
                        pnlContent1.Enabled = false;
                        pnlContent2.Enabled = false;
                        fuAttachment.Style.Clear();
                        fuAttachment.Style.Add("display", "none");
                        lblattch.Visible = true;
                        ibDelete.Enabled = false;
                        fuMedicalAttachement.Style.Clear();
                        fuMedicalAttachement.Style.Add("display", "none");
                        lblMedicalClearaneAttach.Visible = true;
                        ibDeleteMedical.Enabled = false;
                        fuOtherDocumentsAttachement.Style.Clear();
                        fuOtherDocumentsAttachement.Style.Add("display", "none");
                        lblOtherDocumentsClearaneAttach.Visible = true;
                        ibDeleteOtherDocuments.Enabled = false;
                    }

                    //if (dt.Rows[0]["StatusCode"].ToString() != "PEN" && dt.Rows[0]["StatusCode"].ToString() != "INC")
                    //{
                    //    pnlContent1.Enabled = false;
                    //    pnlContent2.Enabled = false;
                    //    fuAttachment.Style.Clear();
                    //    fuAttachment.Style.Add("display", "none");
                    //    lblattch.Visible = true;
                    //    ibDelete.Enabled = false;
                    //    fuMedicalAttachement.Style.Clear();
                    //    fuMedicalAttachement.Style.Add("display", "none");
                    //    lblMedicalClearaneAttach.Visible = true;
                    //    ibDeleteMedical.Enabled = false;
                    //    fuOtherDocumentsAttachement.Style.Clear();
                    //    fuOtherDocumentsAttachement.Style.Add("display", "none");
                    //    lblOtherDocumentsClearaneAttach.Visible = true;
                    //    ibDeleteOtherDocuments.Enabled = false;
                    //}
                }
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();

            string AdvMethod = "";
            int VisaObt = 0;
            int Health = 0;

            // added for Training


            int OtherDocuments = 0;



            float Advance = 0;

            if (rdViaBank.Checked)
                AdvMethod = "Bank";
            else if (rdViaCash.Checked)
                AdvMethod = "Cash";
            else if (rdViaCheck.Checked)
                AdvMethod = "Check";
            if (rdVisaNo.Checked)
                VisaObt = 1;
            else if (rdVisYes.Checked)
                VisaObt = 2;
            if (rdHealthNo.Checked)
                Health = 1;
            else if (rdHealthYes.Checked)
                Health = 2;

            //*******************************************8
            if (rdOtherDocumentsNo.Checked)
                OtherDocuments = 1;
            else if (rdOtherDocumentsYes.Checked)
                OtherDocuments = 2;



            if (rdYesAdv.Checked)
            {
                if (float.TryParse(txtAdvAmnt.Text, out Advance) == false)
                {
                    Advance = 0;
                    txtAdvAmnt.CssClass += " invalid";
                    lblAmountMsg.Visible = true;
                    return;
                }
                else
                {
                    lblAmountMsg.Visible = false;
                }
            }

            if (!g.CheckDate(txtVisa.Text.Trim()) & txtVisa.Text.Trim() != "")
            {
                txtVisa.CssClass += " invalid";
                return;
            }


            DataTable dts = new DataTable();
            Media.GetSecurityTrainingFilesByTAID(Decrypt(Request["TAID"]), ref dts);
            if (dts.Rows.Count == 0)
            {
                fuAttachment.Style.Add("border-style", "solid !important");
                fuAttachment.Style.Add("border-color", "#FF0000 !important");

                // added below
                fuMedicalAttachement.Style.Add("border-style", "solid !important");
                fuMedicalAttachement.Style.Add("border-color", "#FF0000 !important");


                fuOtherDocumentsAttachement.Style.Add("border-style", "solid !important");
                fuOtherDocumentsAttachement.Style.Add("border-color", "#FF0000 !important");





                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(Request["TAID"] as string))
                {
                    checkStatus();


                }
                fuAttachment.Style.Add("border-style", "");
                fuAttachment.Style.Add("border-color", "");

                //added


                fuMedicalAttachement.Style.Add("border-style", "");
                fuMedicalAttachement.Style.Add("border-color", "");



                fuOtherDocumentsAttachement.Style.Add("border-style", "");
                fuOtherDocumentsAttachement.Style.Add("border-color", "");


            }

            if (checkConfirm.Checked)
            {
                checkConfirm.CssClass = "checkbox-inline";
            }
            else
            {
                checkConfirm.Style.Add("border-style", "solid !important");
                checkConfirm.Style.Add("border-color", "#FF0000 !important");
                if (!string.IsNullOrEmpty(Request["TAID"] as string))
                {
                    checkStatus();
                }
                return;
            }

            int SecClearanceReqBy = 2;
            if (rdMissionYes.Checked)
            {
                SecClearanceReqBy = 1;
                // call to check status
                checkStatus();
            }
            else if (rdMissionNo.Checked)
            {
                SecClearanceReqBy = 0;
                // call to check status
                checkStatus();
            }

            int x = 0;

            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                t.UpdateAdvanceAndSecurity(Decrypt(Request["TAID"]),
                    checkSecurityClearance.Checked,
                    checkSecurityTraining.Checked,
                    rdYesAdv.Checked,
                    DDLAdvCurr.SelectedValue.ToString(),
                    Convert.ToDecimal(Advance),
                    AdvMethod,
                    VisaObt,
                    txtVisa.Text,
                    Health,

                    OtherDocuments,

                    SecClearanceReqBy,
                    checkConfirm.Checked,
                    checkNotForDSA.Checked);

                Response.Redirect("~/TravelAuthorization/TAWizard/Step3_WBS.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);

            }

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillDDLs()
    {
        try
        {
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds);

            DDLAdvCurr.DataSource = ds.Tables[7];
            DDLAdvCurr.DataBind();


        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll(string TravelAuthorizationID)
    {
        checkSecurityClearance.Checked = false;
        checkSecurityTraining.Checked = false;
        checkStatus();
    }
    #region Advances

    void FillTA(string TravelAuthorizationID)
    {
        DataTable dt = new DataTable();
        t.GetTravelAuthorizationByTravelAuthorizationID(TravelAuthorizationID, ref dt);

        foreach (DataRow row in dt.Rows)
        {
            if (Convert.ToBoolean(row["TAConfirm"]))
            {
                checkSecurityClearance.Checked = Convert.ToBoolean(row["SecurityClearance"]);
                checkSecurityTraining.Checked = Convert.ToBoolean(row["SecurityTraining"]);
                rdYesAdv.Checked = Convert.ToBoolean(row["IsTravelAdvanceRequested"]);
                rdNoAdv.Checked = !Convert.ToBoolean(row["IsTravelAdvanceRequested"]);
                DDLAdvCurr.SelectedValue = row["TravelAdvanceCurrency"].ToString();
                txtAdvAmnt.Text = row["TravelAdvanceAmount"].ToString();

                if (row["TravelAdvanceMethod"].ToString() == "Bank")
                    rdViaBank.Checked = true;
                //Added to check status not to hide the panel
                checkStatus();

                if (row["TravelAdvanceMethod"].ToString() == "Check")
                    rdViaCheck.Checked = true;
                //Added to check status not to hide the panel
                checkStatus();
                if (row["TravelAdvanceMethod"].ToString() == "Cash")
                    rdViaCash.Checked = true;
                //Added to check status not to hide the panel
                checkStatus();

                if (Convert.ToBoolean(row["IsTravelAdvanceRequested"]))
                {
                    DDLAdvCurr.Enabled = true;
                    txtAdvAmnt.Enabled = true;
                    rdViaBank.Enabled = true;
                    rdViaCheck.Enabled = true;
                    rdViaCash.Enabled = true;

                    if (!rdViaBank.Checked & !rdViaCheck.Checked & !rdViaCash.Checked)
                    {
                        DDLAdvCurr.CssClass = DDLAdvCurr.CssClass + " Req";
                        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass + " Req";
                        rdViaBank.CssClass = rdViaBank.CssClass + " Req";
                        rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
                        rdViaCash.CssClass = rdViaCash.CssClass + " Req";

                        //Added to check status not to hide the panel
                        checkStatus();
                    }

                }
                if (row["IsVisaObtained"].ToString() == "0")
                    rdVisaNA.Checked = true;
                if (row["IsVisaObtained"].ToString() == "1")
                    rdVisaNo.Checked = true;
                if (row["IsVisaObtained"].ToString() == "2")
                {
                    rdVisYes.Checked = true;
                    txtVisa.Enabled = true;
                    txtVisa.CssClass = "form-control Req";
                }

                txtVisa.Text = row["VisaIssued"].ToString();

                if (row["IsVaccinationObtained"].ToString() == "0")
                    rdHealthNA.Checked = true;
                checkStatus();
                if (row["IsVaccinationObtained"].ToString() == "1")
                    rdHealthNo.Checked = true;
                checkStatus();
                if (row["IsVaccinationObtained"].ToString() == "2")
                    rdHealthYes.Checked = true;
                checkStatus();



                checkStatus();

                //adde for OtherDocuments********************************************88
                if (row["IsDocumentsObtained"].ToString() == "0")
                    rdOtherDocumentsNA.Checked = true;
                checkStatus();
                if (row["IsDocumentsObtained"].ToString() == "1")
                    rdOtherDocumentsNo.Checked = true;
                checkStatus();
                if (row["IsDocumentsObtained"].ToString() == "2")
                    rdOtherDocumentsYes.Checked = true;
                checkStatus();








                checkNotForDSA.Checked = Convert.ToBoolean(row["IsNotForDSA"]);

                if (row["IsSecurityClearanceRequestedByMission"] != DBNull.Value)
                {
                    rdMissionYes.Checked = Convert.ToBoolean(row["IsSecurityClearanceRequestedByMission"]);
                    rdMissionNo.Checked = !Convert.ToBoolean(row["IsSecurityClearanceRequestedByMission"]);

                    rdMissionYes.Enabled = true;
                    rdMissionNo.Enabled = true;

                    if (!rdMissionYes.Checked && !rdMissionNo.Checked)
                    {
                        rdMissionYes.CssClass = "radio-inline Req";
                        rdMissionNo.CssClass = "radio-inline Req";
                    }
                    else
                    {
                        rdMissionYes.CssClass = "radio-inline";
                        rdMissionNo.CssClass = "radio-inline";
                    }

                }

                checkConfirm.Checked = Convert.ToBoolean(row["TAConfirm"]);
            }
        }
    }

    void checkStatus()

    {
        CheckMedicalFile();
        CheckSecurityClearanceFile();
        CheckOtherDocumentsFile();
    }




    protected void checkSecurityClearance_CheckedChanged(object sender, EventArgs e)
    {
        if (checkSecurityClearance.Checked)
        {
            fuAttachment.Style.Clear();
            fuAttachment.Style.Add("display", "block");
            pnlFile.Style.Clear();
            pnlFile.Style.Add("display", "none");

            lblattch.Visible = true;

            checkSecurityClearance.CssClass = "checkbox-inline";
            checkSecurityClearance.Style.Add("border-style", "");
            checkSecurityClearance.Style.Add("border-color", "");

            // check status

            checkStatus();
        }
        else
        {
            fuAttachment.Style.Clear();
            fuAttachment.Style.Add("display", "none");
            pnlFile.Style.Clear();
            pnlFile.Style.Add("display", "none");

            lblattch.Visible = false;
            checkStatus();
        }
    }
    protected void rdYesAdv_CheckedChanged(object sender, EventArgs e)
    {
        DDLAdvCurr.Enabled = true;
        txtAdvAmnt.Enabled = true;
        rdViaBank.Enabled = true;
        rdViaCheck.Enabled = true;
        rdViaCash.Enabled = true;

        DDLAdvCurr.CssClass = DDLAdvCurr.CssClass + " Req";
        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass + " Req";

        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
        {
            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
            rdViaCash.CssClass = rdViaCash.CssClass + " Req";
        }
        else
        {
            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
        }



    }
    protected void rdNoAdv_CheckedChanged(object sender, EventArgs e)
    {
        DDLAdvCurr.Enabled = false;
        txtAdvAmnt.Enabled = false;
        rdViaBank.Enabled = false;
        rdViaCheck.Enabled = false;
        rdViaCash.Enabled = false;
        DDLAdvCurr.SelectedIndex = -1;
        txtAdvAmnt.Text = "0";
        lblAmountMsg.Visible = false;

        rdViaBank.Checked = false;
        rdViaCheck.Checked = false;
        rdViaCash.Checked = false;

        DDLAdvCurr.CssClass = DDLAdvCurr.CssClass.Replace("Req", "");
        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass.Replace("Req", "");
        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass.Replace("invalid", "");

        rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
        rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
        rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
    }

    protected void rdVisYes_CheckedChanged(object sender, EventArgs e)
    {
        txtVisa.Enabled = true;
        txtVisa.CssClass = "form-control Req";
    }
    protected void rdVisaNo_CheckedChanged(object sender, EventArgs e)
    {
        txtVisa.Enabled = false;
        txtVisa.Text = "";
        txtVisa.CssClass = "form-control";

    }
    protected void rdVisaNA_CheckedChanged(object sender, EventArgs e)
    {
        txtVisa.Enabled = false;
        txtVisa.Text = "";
        txtVisa.CssClass = "form-control";
    }

    protected void rdMissionYes_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdMissionYes.Checked && !rdMissionNo.Checked)
        {
            rdMissionYes.CssClass = "radio-inline Req";
            rdMissionNo.CssClass = "radio-inline Req";
        }
        else
        {
            rdMissionYes.CssClass = "radio-inline";
            rdMissionNo.CssClass = "radio-inline";
        }
    }
    protected void rdMissionNo_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdMissionYes.Checked && !rdMissionNo.Checked)
        {
            rdMissionYes.CssClass = "radio-inline Req";
            rdMissionNo.CssClass = "radio-inline Req";
        }
        else
        {
            rdMissionYes.CssClass = "radio-inline";
            rdMissionNo.CssClass = "radio-inline";
        }
    }
    protected void checkSecurityTraining_CheckedChanged(object sender, EventArgs e)
    {
        if (checkSecurityTraining.Checked)
        {
            checkStatus();

            rdMissionYes.Enabled = true;
            rdMissionNo.Enabled = true;

            if (!rdMissionYes.Checked && !rdMissionNo.Checked)
            {
                rdMissionYes.CssClass = "radio-inline Req";
                rdMissionNo.CssClass = "radio-inline Req";
            }
            else
            {
                rdMissionYes.CssClass = "radio-inline";
                rdMissionNo.CssClass = "radio-inline";
            }

        }
        else
        {
            checkStatus();

            rdMissionYes.Checked = false;
            rdMissionNo.Checked = false;
            rdMissionYes.Enabled = false;
            rdMissionNo.Enabled = false;
            rdMissionYes.CssClass = "radio-inline";
            rdMissionNo.CssClass = "radio-inline";

        }
    }

    protected void rdViaBank_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
        {
            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
            rdViaCash.CssClass = rdViaCash.CssClass + " Req";


        }
        else
        {
            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
        }
    }
    protected void rdViaCheck_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
        {
            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
            rdViaCash.CssClass = rdViaCash.CssClass + " Req";
        }
        else
        {
            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
        }
    }
    protected void rdViaCash_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
        {
            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
            rdViaCash.CssClass = rdViaCash.CssClass + " Req";
        }
        else
        {
            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
        }
    }

    protected void rdHealthYes_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdHealthYes.Checked && !rdHealthNo.Checked && !rdHealthNA.Checked)
        {
            rdHealthYes.CssClass = rdHealthYes.CssClass + " Req";
            rdHealthNo.CssClass = rdHealthNo.CssClass + " Req";
            rdHealthNA.CssClass = rdHealthNA.CssClass + " Req";
        }


        fuMedicalAttachement.Style.Clear();
        fuMedicalAttachement.Style.Add("display", "block");
        pnlMedicalFiles.Style.Clear();
        pnlMedicalFiles.Style.Add("display", "none");
        lblMedicalClearaneAttach.Visible = true;
        checkStatus();
    }
    protected void rdHealthNo_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdHealthYes.Checked && !rdHealthNo.Checked && !rdHealthNA.Checked)
        {
            rdHealthYes.CssClass = rdHealthYes.CssClass + " Req";
            rdHealthNo.CssClass = rdHealthNo.CssClass + " Req";
            rdHealthNA.CssClass = rdHealthNA.CssClass + " Req";
        }


        fuMedicalAttachement.Style.Clear();
        fuMedicalAttachement.Style.Add("display", "none");
        pnlMedicalFiles.Style.Clear();
        pnlMedicalFiles.Style.Add("display", "none");
        lblMedicalClearaneAttach.Visible = false;
        checkStatus();
    }
    protected void rdHealthNA_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdHealthYes.Checked && !rdHealthNo.Checked && !rdHealthNA.Checked)
        {
            rdHealthYes.CssClass = rdHealthYes.CssClass + " Req";
            rdHealthNo.CssClass = rdHealthNo.CssClass + " Req";
            rdHealthNA.CssClass = rdHealthNA.CssClass + " Req";
        }

        fuMedicalAttachement.Style.Clear();
        fuMedicalAttachement.Style.Add("display", "none");
        pnlMedicalFiles.Style.Clear();
        pnlMedicalFiles.Style.Add("display", "none");
        lblMedicalClearaneAttach.Visible = false;
        checkStatus();
    }
    protected void rdOtherDocumentsYes_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdOtherDocumentsYes.Checked && !rdOtherDocumentsNo.Checked && !rdOtherDocumentsNA.Checked)
        {
            rdOtherDocumentsYes.CssClass = rdOtherDocumentsYes.CssClass + " Req";
            rdOtherDocumentsNo.CssClass = rdOtherDocumentsNo.CssClass + " Req";
            rdOtherDocumentsNA.CssClass = rdOtherDocumentsNA.CssClass + " Req";
        }


        fuOtherDocumentsAttachement.Style.Clear();
        fuOtherDocumentsAttachement.Style.Add("display", "block");
        pnlOtherDocumentsFiles.Style.Clear();
        pnlOtherDocumentsFiles.Style.Add("display", "none");

        lblOtherDocumentsClearaneAttach.Visible = true;
        checkStatus();
    }
    protected void rdOtherDocumentsNo_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdOtherDocumentsYes.Checked && !rdOtherDocumentsNo.Checked && !rdOtherDocumentsNA.Checked)
        {
            rdOtherDocumentsYes.CssClass = rdOtherDocumentsYes.CssClass + " Req";
            rdOtherDocumentsNo.CssClass = rdOtherDocumentsNo.CssClass + " Req";
            rdOtherDocumentsNA.CssClass = rdOtherDocumentsNA.CssClass + " Req";
        }


        fuOtherDocumentsAttachement.Style.Clear();
        fuOtherDocumentsAttachement.Style.Add("display", "none");
        pnlOtherDocumentsFiles.Style.Clear();
        pnlOtherDocumentsFiles.Style.Add("display", "none");

        lblOtherDocumentsClearaneAttach.Visible = false;
        checkStatus();

    }
    protected void rdOtherDocumentsNA_CheckedChanged(object sender, EventArgs e)
    {
        if (!rdOtherDocumentsYes.Checked && !rdOtherDocumentsNo.Checked && !rdOtherDocumentsNA.Checked)
        {
            rdOtherDocumentsYes.CssClass = rdOtherDocumentsYes.CssClass + " Req";
            rdOtherDocumentsNo.CssClass = rdOtherDocumentsNo.CssClass + " Req";
            rdOtherDocumentsNA.CssClass = rdOtherDocumentsNA.CssClass + " Req";
        }


        fuOtherDocumentsAttachement.Style.Clear();
        fuOtherDocumentsAttachement.Style.Add("display", "none");
        pnlOtherDocumentsFiles.Style.Clear();
        pnlOtherDocumentsFiles.Style.Add("display", "none");

        lblOtherDocumentsClearaneAttach.Visible = false;
        checkStatus();
    }


    #endregion

    #region Security Upload
    protected void CheckSecurityClearanceFile()
    {
        string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

        byte[] bytes = { 0 };
        string fileName = "", contentType = "";

        DataTable dt = new DataTable();

        Media.GetSecurityTrainingFilesByTAID(TravelAuthorizationID, ref dt);

        if (dt.Rows.Count > 0)
        {
            fuAttachment.Style.Clear();
            fuAttachment.Style.Add("display", "none");
            pnlFile.Style.Clear();
            pnlFile.Style.Add("display", "block");

            lblattch.Visible = true;
        }
        else
        {
            if (checkSecurityClearance.Checked)
            {
                fuAttachment.Style.Clear();
                fuAttachment.Style.Add("display", "block");
                pnlFile.Style.Clear();
                pnlFile.Style.Add("display", "none");

                lblattch.Visible = true;

                checkSecurityClearance.CssClass = "checkbox-inline";
                checkSecurityClearance.Style.Add("border-style", "");
                checkSecurityClearance.Style.Add("border-color", "");
            }
            else
            {
                fuAttachment.Style.Clear();
                fuAttachment.Style.Add("display", "none");
                pnlFile.Style.Clear();
                pnlFile.Style.Add("display", "none");

                lblattch.Visible = false;
            }
        }
    }
    void ClearAll()
    {
        checkSecurityClearance.Checked = false;
        checkSecurityTraining.Checked = false;

    }
    protected void ProcessUpload(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        try
        {
            string FileName = Path.GetFileName(e.FileName);
            string FileNameOnly = Path.GetFileNameWithoutExtension(e.FileName);
            string FileExtension = Path.GetExtension(e.FileName);

            if (FileName != string.Empty)
            {
                Regex FilenameRegex = new Regex("(.*?)\\.(pdf)$");
                //Regex FilenameRegex = new Regex("(.*?)\\.(doc|docx|pdf|xls|xlsx)$");
                if ((FilenameRegex.IsMatch(FileName, Convert.ToInt16(RegexOptions.IgnoreCase))))
                {

                    Guid gname = default(Guid);
                    gname = Guid.NewGuid();
                    string nameAndType = "SecurityClearance - " + FileNameOnly + FileExtension;
                    string UploadFileName = "SecurityClearance - " + FileNameOnly + " - " + gname.ToString() + FileExtension;
                    if (!Directory.Exists(Server.MapPath("~\\UploadedFiles\\")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~\\UploadedFiles\\"));
                    }
                    if (int.Parse(e.FileSize) < 4194304)
                    {
                        fuAttachment.SaveAs(Server.MapPath("~\\UploadedFiles\\" + UploadFileName));
                        if (e.State == AjaxControlToolkit.AsyncFileUploadState.Success)
                        {
                            Media.InsertSecurityTrainingFiles(Decrypt(Request["TAID"]), Decrypt(Request["TANO"]), FileNameOnly, FileExtension, g.GetPhoto(Server.MapPath("~\\UploadedFiles\\" + UploadFileName)));
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibView_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["TAID"] as string))
        {
            string TravelAuthorizationID = Decrypt(Request["TAID"]);
            Response.Redirect("~/ViewPDF.aspx?TAID="+ TravelAuthorizationID, false);
        }
    }
    //protected void ibView_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(Request["TAID"] as string))
    //        {
    //            string TravelAuthorizationID = Decrypt(Request["TAID"]);
    //            byte[] bytes = { 0 };
    //            string fileName = "", contentType = "";
    //            DataTable dt = new DataTable();
    //            Media.GetSecurityTrainingFilesByTAID(TravelAuthorizationID, ref dt);
    //            foreach (DataRow row in dt.Rows)
    //            {
    //                bytes = (byte[])row["FileData"];
    //                contentType = row["FileExtension"].ToString();
    //                fileName = row["FileName"].ToString();
    //            }
    //            Response.Clear();
    //            Response.Buffer = true;
    //            Response.Charset = "";
    //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //            Response.ContentType = contentType;
    //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + contentType);
    //            string sid = bytes.ToString();
    //            Response.BinaryWrite(bytes);
    //            Response.Flush();
    //            HttpContext.Current.ApplicationInstance.CompleteRequest();
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
    //        }

    //    }
    //    catch (Exception ex)
    //    {

    //        IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
    //    }
    //}
    protected void ibDelete_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["TAID"] as string))
        {
            string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

            Media.DeleteSecurityTrainingFiles(TravelAuthorizationID);
            lblUploadMsg.ForeColor = Color.Green;
            fuAttachment.Style.Clear();
            fuAttachment.Style.Add("display", "block");
            pnlFile.Style.Clear();
            pnlFile.Style.Add("display", "none");
            lblUploadMsg.Text = "File has been deleted successfully";
        }
        else
        {
            lblUploadMsg.ForeColor = Color.Green;
            lblUploadMsg.Text = "File was deleted successfully";
            fuAttachment.Style.Clear();
            fuAttachment.Style.Add("display", "block");
            pnlFile.Style.Clear();
            pnlFile.Style.Add("display", "none");
            lblUploadMsg.Text = "";
        }
        checkStatus();
    }

    #endregion

    #region UploadMedical
    protected void CheckMedicalFile()
    {
        string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

        byte[] bytes = { 0 };
        string fileName = "", contentType = "";

        DataTable dt = new DataTable();

        Media.GetMedicalFilesByTAID(TravelAuthorizationID, ref dt);

        if (dt.Rows.Count > 0)
        {
            fuMedicalAttachement.Style.Clear();
            fuMedicalAttachement.Style.Add("display", "none");
            pnlMedicalFiles.Style.Clear();
            pnlMedicalFiles.Style.Add("display", "block");
            lblMedicalClearaneAttach.Visible = true;
        }
        else
        {
            if (rdHealthYes.Checked)
            {
                fuMedicalAttachement.Style.Clear();
                fuMedicalAttachement.Style.Add("display", "block");
                pnlMedicalFiles.Style.Clear();
                pnlMedicalFiles.Style.Add("display", "none");
                lblMedicalClearaneAttach.Visible = true;
            }
            else
            {
                fuMedicalAttachement.Style.Clear();
                fuMedicalAttachement.Style.Add("display", "none");
                pnlMedicalFiles.Style.Clear();
                pnlMedicalFiles.Style.Add("display", "none");
                lblMedicalClearaneAttach.Visible = false;




            }

        }
    }
    protected void ProcessUploadMedical(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        try
        {
            string FileName = Path.GetFileName(e.FileName);
            string FileNameOnly = Path.GetFileNameWithoutExtension(e.FileName);
            string FileExtension = Path.GetExtension(e.FileName);

            if (FileName != string.Empty)
            {
                Regex FilenameRegex = new Regex("(.*?)\\.(pdf)$");
                //Regex FilenameRegex = new Regex("(.*?)\\.(doc|docx|pdf|xls|xlsx)$");
                if ((FilenameRegex.IsMatch(FileName, Convert.ToInt16(RegexOptions.IgnoreCase))))
                {

                    Guid gname = default(Guid);
                    gname = Guid.NewGuid();
                    string nameAndType = "MedicalClearance - " + FileNameOnly + FileExtension;
                    string UploadFileName = "MedicalClearance - " + FileNameOnly + " - " + gname.ToString() + FileExtension;
                    if (!Directory.Exists(Server.MapPath("~\\UploadedFiles\\")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~\\UploadedFiles\\"));
                    }
                    if (int.Parse(e.FileSize) < 4194304)
                    {
                        fuMedicalAttachement.SaveAs(Server.MapPath("~\\UploadedFiles\\" + UploadFileName));
                        if (e.State == AjaxControlToolkit.AsyncFileUploadState.Success)
                        {
                            //Session["UploadFileNameMedical"] = UploadFileName;
                            Media.InsertMedicalFiles(Decrypt(Request["TAID"]), Decrypt(Request["TANO"]), FileNameOnly, FileExtension, g.GetPhoto(Server.MapPath("~\\UploadedFiles\\" + UploadFileName)));
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibViewMedical_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

                byte[] bytes = { 0 };
                string fileName = "", contentType = "";

                DataTable dt = new DataTable();

                Media.GetMedicalFilesByTAID(TravelAuthorizationID, ref dt);

                foreach (DataRow row in dt.Rows)
                {
                    bytes = (byte[])row["FileData"];
                    contentType = row["FileExtension"].ToString();
                    fileName = row["FileName"].ToString();
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + contentType);
                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibDeleteMedical_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["TAID"] as string))
        {
            string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

            Media.DeleteMedicalFiles(TravelAuthorizationID);
            lblUploadMessageMedical.ForeColor = Color.Green;
            fuMedicalAttachement.Style.Clear();
            fuMedicalAttachement.Style.Add("display", "block");
            pnlMedicalFiles.Style.Clear();
            pnlMedicalFiles.Style.Add("display", "none");
            lblUploadMessageMedical.Text = "File has been deleted successfully";
        }
        else
        {
            lblUploadMessageMedical.ForeColor = Color.Green;
            lblUploadMessageMedical.Text = "File was deleted successfully";
            fuMedicalAttachement.Style.Clear();
            fuMedicalAttachement.Style.Add("display", "block");
            pnlMedicalFiles.Style.Clear();
            pnlMedicalFiles.Style.Add("display", "none");
            lblUploadMessageMedical.Text = "";
        }

        //*******************************************888
        checkStatus();
    }
    #endregion

    #region UploadOtherDocuments
    protected void CheckOtherDocumentsFile()
    {
        string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

        byte[] bytes = { 0 };
        string fileName = "", contentType = "";

        DataTable dt = new DataTable();

        Media.GetOtherDocumentsFilesByTAID(TravelAuthorizationID, ref dt);

        if (dt.Rows.Count > 0)
        {
            fuOtherDocumentsAttachement.Style.Clear();
            fuOtherDocumentsAttachement.Style.Add("display", "none");
            pnlOtherDocumentsFiles.Style.Clear();
            pnlOtherDocumentsFiles.Style.Add("display", "block");
            lblOtherDocumentsClearaneAttach.Visible = true;
        }
        else
        {
            if (rdOtherDocumentsYes.Checked)
            {
                fuOtherDocumentsAttachement.Style.Clear();
                fuOtherDocumentsAttachement.Style.Add("display", "block");
                pnlOtherDocumentsFiles.Style.Clear();
                pnlOtherDocumentsFiles.Style.Add("display", "none");
                lblOtherDocumentsClearaneAttach.Visible = true;
            }
            else
            {
                fuOtherDocumentsAttachement.Style.Clear();
                fuOtherDocumentsAttachement.Style.Add("display", "none");
                pnlOtherDocumentsFiles.Style.Clear();
                pnlOtherDocumentsFiles.Style.Add("display", "none");
                lblOtherDocumentsClearaneAttach.Visible = false;
            }

        }
    }
    protected void ProcessUploadOtherDocuments(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        try
        {
            string FileName = Path.GetFileName(e.FileName);
            string FileNameOnly = Path.GetFileNameWithoutExtension(e.FileName);
            string FileExtension = Path.GetExtension(e.FileName);

            if (FileName != string.Empty)
            {
                Regex FilenameRegex = new Regex("(.*?)\\.(pdf)$");
                //Regex FilenameRegex = new Regex("(.*?)\\.(doc|docx|pdf|xls|xlsx)$");
                if ((FilenameRegex.IsMatch(FileName, Convert.ToInt16(RegexOptions.IgnoreCase))))
                {

                    Guid gname = default(Guid);
                    gname = Guid.NewGuid();
                    string nameAndType = "OtherDocumentsClearance - " + FileNameOnly + FileExtension;
                    string UploadFileName = "OtherDocumentsClearance - " + FileNameOnly + " - " + gname.ToString() + FileExtension;
                    if (!Directory.Exists(Server.MapPath("~\\UploadedFiles\\")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~\\UploadedFiles\\"));
                    }
                    if (int.Parse(e.FileSize) < 4194304)
                    {
                        fuOtherDocumentsAttachement.SaveAs(Server.MapPath("~\\UploadedFiles\\" + UploadFileName));
                        if (e.State == AjaxControlToolkit.AsyncFileUploadState.Success)
                        {
                            //Session["UploadFileNameOtherDocuments"] = UploadFileName;
                            Media.InsertOtherDocumentsFiles(Decrypt(Request["TAID"]), Decrypt(Request["TANO"]), FileNameOnly, FileExtension, g.GetPhoto(Server.MapPath("~\\UploadedFiles\\" + UploadFileName)));
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibViewOtherDocuments_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

                byte[] bytes = { 0 };
                string fileName = "", contentType = "";

                DataTable dt = new DataTable();

                Media.GetOtherDocumentsFilesByTAID(TravelAuthorizationID, ref dt);

                foreach (DataRow row in dt.Rows)
                {
                    bytes = (byte[])row["FileData"];
                    contentType = row["FileExtension"].ToString();
                    fileName = row["FileName"].ToString();
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + contentType);
                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibDeleteOtherDocuments_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["TAID"] as string))
        {
            string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

            Media.DeleteOtherDocumentsFiles(TravelAuthorizationID);
            lblUploadMessageOtherDocuments.ForeColor = Color.Green;
            fuOtherDocumentsAttachement.Style.Clear();
            fuOtherDocumentsAttachement.Style.Add("display", "block");
            pnlOtherDocumentsFiles.Style.Clear();
            pnlOtherDocumentsFiles.Style.Add("display", "none");
            lblUploadMessageOtherDocuments.Text = "File has been deleted successfully";
        }
        else
        {
            lblUploadMessageOtherDocuments.ForeColor = Color.Green;
            lblUploadMessageOtherDocuments.Text = "File was deleted successfully";
            fuOtherDocumentsAttachement.Style.Clear();
            fuOtherDocumentsAttachement.Style.Add("display", "block");
            pnlOtherDocumentsFiles.Style.Clear();
            pnlOtherDocumentsFiles.Style.Add("display", "none");
            lblUploadMessageOtherDocuments.Text = "";
        }
        checkStatus();
    }
    #endregion

}

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//public partial class TravelAuthorization_TAWizard_Step2_AdvanceAndSecurity : AuthenticatedPageClass
//{
//    Business.Lookups l = new Business.Lookups();
//    Business.TravelAuthorization t = new Business.TravelAuthorization();
//    Business.Security s = new Business.Security();
//    Business.Media Media = new Business.Media();
//    Globals g = new Globals();

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!IsPostBack)
//        {
//            System.Web.UI.HtmlControls.HtmlGenericControl TAStatus = null;
//            TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
//            TAStatus.Visible = this.CanAmend;

//            LinkButton lnk = new LinkButton();
//            lnk = (LinkButton)WizardHeader.FindControl("lbStep2");
//            lnk.CssClass = "btn btn-success btn-circle btn-lg";

//            DataTable dtRoleName = new DataTable();
//            s.GetRoleNameByUserID(ref dtRoleName);

//            if (dtRoleName.Rows.Count > 0)
//            {
//                if (dtRoleName.Rows[0]["RoleName"].ToString() == "HR/Admin")
//                {
//                    rowDSANotApp.Visible = true;
//                }
//                else
//                {
//                    rowDSANotApp.Visible = false;
//                }
//            }

//            FillDDLs();

//            if (!string.IsNullOrEmpty(Request["TAID"] as string))
//            {
//                string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());
//                FillTA(TravelAuthorizationID);
//                CheckMedicalFile();
//                CheckSecurityClearanceFile();
//                //lock content
//                DataTable dt = new DataTable();
//                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
//                if (dt.Rows.Count > 0)
//                {
//                    if (dt.Rows[0]["StatusCode"].ToString() != "PEN" && dt.Rows[0]["StatusCode"].ToString() != "INC" && dt.Rows[0]["StatusCode"].ToString() != "RBS" && dt.Rows[0]["StatusCode"].ToString() != "RBHR" && dt.Rows[0]["StatusCode"].ToString() != "RBHOSO" && dt.Rows[0]["StatusCode"].ToString() != "RBRMO" && dt.Rows[0]["StatusCode"].ToString() != "RBHOO" && dt.Rows[0]["StatusCode"].ToString() != "RBCOM")
//                    {
//                        pnlContent1.Enabled = false;
//                        pnlContent2.Enabled = false;

//                        fuAttachment.Style.Clear();
//                        fuAttachment.Style.Add("display", "none");
//                        lblattch.Visible = true;
//                        ibDelete.Enabled = false;

//                        fuMedicalAttachement.Style.Clear();
//                        fuMedicalAttachement.Style.Add("display", "none");
//                        lblMedicalClearaneAttach.Visible = true;
//                        ibDeleteMedical.Enabled = false;
//                    }
//                }
//            }
//        }
//    }

//    void FillDDLs()
//    {
//        try
//        {
//            DataSet ds = new DataSet();
//            l.GetAllLookupsList(ref ds);

//            DDLAdvCurr.DataSource = ds.Tables[7];
//            DDLAdvCurr.DataBind();


//        }
//        catch (Exception ex)
//        {

//            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
//        }
//    }
//    void ClearAll(string TravelAuthorizationID)
//    {
//        checkSecurityClearance.Checked = false;
//        checkSecurityTraining.Checked = false;

//    }
//    #region Advances

//    void FillTA(string TravelAuthorizationID)
//    {
//        DataTable dt = new DataTable();
//        t.GetTravelAuthorizationByTravelAuthorizationID(TravelAuthorizationID, ref dt);

//        foreach (DataRow row in dt.Rows)
//        {
//            if (Convert.ToBoolean(row["TAConfirm"]))
//            {
//                checkSecurityClearance.Checked = Convert.ToBoolean(row["SecurityClearance"]);
//                checkSecurityTraining.Checked = Convert.ToBoolean(row["SecurityTraining"]);
//                rdYesAdv.Checked = Convert.ToBoolean(row["IsTravelAdvanceRequested"]);
//                rdNoAdv.Checked = !Convert.ToBoolean(row["IsTravelAdvanceRequested"]);
//                DDLAdvCurr.SelectedValue = row["TravelAdvanceCurrency"].ToString();
//                txtAdvAmnt.Text = row["TravelAdvanceAmount"].ToString();

//                if (row["TravelAdvanceMethod"].ToString() == "Bank")
//                    rdViaBank.Checked = true;
//                if (row["TravelAdvanceMethod"].ToString() == "Check")
//                    rdViaCheck.Checked = true;
//                if (row["TravelAdvanceMethod"].ToString() == "Cash")
//                    rdViaCash.Checked = true;

//                if (Convert.ToBoolean(row["IsTravelAdvanceRequested"]))
//                {
//                    DDLAdvCurr.Enabled = true;
//                    txtAdvAmnt.Enabled = true;
//                    rdViaBank.Enabled = true;
//                    rdViaCheck.Enabled = true;
//                    rdViaCash.Enabled = true;

//                    if (!rdViaBank.Checked & !rdViaCheck.Checked & !rdViaCash.Checked)
//                    {
//                        DDLAdvCurr.CssClass = DDLAdvCurr.CssClass + " Req";
//                        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass + " Req";
//                        rdViaBank.CssClass = rdViaBank.CssClass + " Req";
//                        rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
//                        rdViaCash.CssClass = rdViaCash.CssClass + " Req";
//                    }

//                }
//                if (row["IsVisaObtained"].ToString() == "0")
//                    rdVisaNA.Checked = true;
//                if (row["IsVisaObtained"].ToString() == "1")
//                    rdVisaNo.Checked = true;
//                if (row["IsVisaObtained"].ToString() == "2")
//                {
//                    rdVisYes.Checked = true;
//                    txtVisa.Enabled = true;
//                    txtVisa.CssClass = "form-control Req";
//                }

//                txtVisa.Text = row["VisaIssued"].ToString();
//                if (row["IsVaccinationObtained"].ToString() == "0")
//                    rdHealthNA.Checked = true;
//                if (row["IsVaccinationObtained"].ToString() == "1")
//                    rdHealthNo.Checked = true;
//                if (row["IsVaccinationObtained"].ToString() == "2")
//                    rdHealthYes.Checked = true;

//                checkNotForDSA.Checked = Convert.ToBoolean(row["IsNotForDSA"]);

//                if (row["IsSecurityClearanceRequestedByMission"] != DBNull.Value)
//                {
//                    rdMissionYes.Checked = Convert.ToBoolean(row["IsSecurityClearanceRequestedByMission"]);
//                    rdMissionNo.Checked = !Convert.ToBoolean(row["IsSecurityClearanceRequestedByMission"]);

//                    rdMissionYes.Enabled = true;
//                    rdMissionNo.Enabled = true;

//                    if (!rdMissionYes.Checked && !rdMissionNo.Checked)
//                    {
//                        rdMissionYes.CssClass = "radio-inline Req";
//                        rdMissionNo.CssClass = "radio-inline Req";
//                    }
//                    else
//                    {
//                        rdMissionYes.CssClass = "radio-inline";
//                        rdMissionNo.CssClass = "radio-inline";
//                    }

//                }

//                checkConfirm.Checked = Convert.ToBoolean(row["TAConfirm"]);
//            }
//        }
//    }

//    protected void btnSave_Click(object sender, EventArgs e)
//    {
//        try
//        {
//            DataTable dt = new DataTable();

//            string AdvMethod = "";
//            int VisaObt = 0;
//            int Health = 0;
//            float Advance = 0;

//            if (rdViaBank.Checked)
//                AdvMethod = "Bank";
//            else if (rdViaCash.Checked)
//                AdvMethod = "Cash";
//            else if (rdViaCheck.Checked)
//                AdvMethod = "Check";
//            if (rdVisaNo.Checked)
//                VisaObt = 1;
//            else if (rdVisYes.Checked)
//                VisaObt = 2;
//            if (rdHealthNo.Checked)
//                Health = 1;
//            else if (rdHealthYes.Checked)
//                Health = 2;

//            if (rdYesAdv.Checked)
//            {
//                if (float.TryParse(txtAdvAmnt.Text, out Advance) == false)
//                {
//                    Advance = 0;
//                    txtAdvAmnt.CssClass += " invalid";
//                    lblAmountMsg.Visible = true;
//                    return;
//                }
//                else
//                {
//                    lblAmountMsg.Visible = false;
//                }
//            }

//            if (!g.CheckDate(txtVisa.Text.Trim()) & txtVisa.Text.Trim() != "")
//            {
//                txtVisa.CssClass += " invalid";
//                return;
//            }


//            DataTable dts = new DataTable();
//            Media.GetSecurityTrainingFilesByTAID(Decrypt(Request["TAID"]), ref dts);
//            if (dts.Rows.Count == 0)
//            {
//                fuAttachment.Style.Add("border-style", "solid !important");
//                fuAttachment.Style.Add("border-color", "#FF0000 !important");
//                return;
//            }
//            else
//            {
//                if (!string.IsNullOrEmpty(Request["TAID"] as string))
//                {
//                    CheckMedicalFile();
//                    CheckSecurityClearanceFile();
//                }
//                fuAttachment.Style.Add("border-style", "");
//                fuAttachment.Style.Add("border-color", "");
//            }
//            //if (checkSecurityClearance.Checked)
//            //{
//            //    checkSecurityClearance.CssClass = "checkbox-inline";
//            //}
//            //else
//            //{
//            //    checkSecurityClearance.Style.Add("border-style", "solid !important");
//            //    checkSecurityClearance.Style.Add("border-color", "#FF0000 !important");
//            //    return;
//            //}

//            if (checkConfirm.Checked)
//            {
//                checkConfirm.CssClass = "checkbox-inline";
//            }
//            else
//            {
//                checkConfirm.Style.Add("border-style", "solid !important");
//                checkConfirm.Style.Add("border-color", "#FF0000 !important");
//                if (!string.IsNullOrEmpty(Request["TAID"] as string))
//                {
//                    CheckMedicalFile();
//                    CheckSecurityClearanceFile();
//                }
//                return;
//            }

//            int SecClearanceReqBy = 2;
//            if (rdMissionYes.Checked)
//            {
//                SecClearanceReqBy = 1;
//            }
//            else if (rdMissionNo.Checked)
//            {
//                SecClearanceReqBy = 0;
//            }

//            if (!string.IsNullOrEmpty(Request["TAID"] as string))
//            {
//                t.UpdateAdvanceAndSecurity(Decrypt(Request["TAID"]),
//                    checkSecurityClearance.Checked,
//                    checkSecurityTraining.Checked,
//                    rdYesAdv.Checked,
//                    DDLAdvCurr.SelectedValue.ToString(),
//                    Convert.ToDecimal(Advance),
//                    AdvMethod,
//                    VisaObt,
//                    txtVisa.Text,
//                    Health,
//                    SecClearanceReqBy,
//                    checkConfirm.Checked,
//                    checkNotForDSA.Checked);

//                Response.Redirect("~/TravelAuthorization/TAWizard/Step3_WBS.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);

//            }

//        }
//        catch (Exception ex)
//        {

//            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
//        }
//    }
//    protected void checkSecurityClearance_CheckedChanged(object sender, EventArgs e)
//    {
//        if (checkSecurityClearance.Checked)
//        {
//            fuAttachment.Style.Clear();
//            fuAttachment.Style.Add("display", "block");
//            pnlFile.Style.Clear();
//            pnlFile.Style.Add("display", "none");

//            lblattch.Visible = true;

//            checkSecurityClearance.CssClass = "checkbox-inline";
//            checkSecurityClearance.Style.Add("border-style", "");
//            checkSecurityClearance.Style.Add("border-color", "");
//        }
//        else
//        {
//            fuAttachment.Style.Clear();
//            fuAttachment.Style.Add("display", "none");
//            pnlFile.Style.Clear();
//            pnlFile.Style.Add("display", "none");

//            lblattch.Visible = false;
//        }
//    }
//    protected void rdYesAdv_CheckedChanged(object sender, EventArgs e)
//    {
//        DDLAdvCurr.Enabled = true;
//        txtAdvAmnt.Enabled = true;
//        rdViaBank.Enabled = true;
//        rdViaCheck.Enabled = true;
//        rdViaCash.Enabled = true;

//        DDLAdvCurr.CssClass = DDLAdvCurr.CssClass + " Req";
//        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass + " Req";

//        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
//            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
//            rdViaCash.CssClass = rdViaCash.CssClass + " Req";
//        }
//        else
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
//            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
//            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
//        }



//    }
//    protected void rdNoAdv_CheckedChanged(object sender, EventArgs e)
//    {
//        DDLAdvCurr.Enabled = false;
//        txtAdvAmnt.Enabled = false;
//        rdViaBank.Enabled = false;
//        rdViaCheck.Enabled = false;
//        rdViaCash.Enabled = false;
//        DDLAdvCurr.SelectedIndex = -1;
//        txtAdvAmnt.Text = "0";
//        lblAmountMsg.Visible = false;

//        rdViaBank.Checked = false;
//        rdViaCheck.Checked = false;
//        rdViaCash.Checked = false;

//        DDLAdvCurr.CssClass = DDLAdvCurr.CssClass.Replace("Req", "");
//        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass.Replace("Req", "");
//        txtAdvAmnt.CssClass = txtAdvAmnt.CssClass.Replace("invalid", "");

//        rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
//        rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
//        rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
//    }

//    protected void rdVisYes_CheckedChanged(object sender, EventArgs e)
//    {
//        txtVisa.Enabled = true;
//        txtVisa.CssClass = "form-control Req";
//    }
//    protected void rdVisaNo_CheckedChanged(object sender, EventArgs e)
//    {
//        txtVisa.Enabled = false;
//        txtVisa.Text = "";
//        txtVisa.CssClass = "form-control";

//    }
//    protected void rdVisaNA_CheckedChanged(object sender, EventArgs e)
//    {
//        txtVisa.Enabled = false;
//        txtVisa.Text = "";
//        txtVisa.CssClass = "form-control";
//    }

//    protected void rdMissionYes_CheckedChanged(object sender, EventArgs e)
//    {
//        if (!rdMissionYes.Checked && !rdMissionNo.Checked)
//        {
//            rdMissionYes.CssClass = "radio-inline Req";
//            rdMissionNo.CssClass = "radio-inline Req";
//        }
//        else
//        {
//            rdMissionYes.CssClass = "radio-inline";
//            rdMissionNo.CssClass = "radio-inline";
//        }
//    }
//    protected void rdMissionNo_CheckedChanged(object sender, EventArgs e)
//    {
//        if (!rdMissionYes.Checked && !rdMissionNo.Checked)
//        {
//            rdMissionYes.CssClass = "radio-inline Req";
//            rdMissionNo.CssClass = "radio-inline Req";
//        }
//        else
//        {
//            rdMissionYes.CssClass = "radio-inline";
//            rdMissionNo.CssClass = "radio-inline";
//        }
//    }
//    protected void checkSecurityTraining_CheckedChanged(object sender, EventArgs e)
//    {
//        if (checkSecurityTraining.Checked)
//        {
//            rdMissionYes.Enabled = true;
//            rdMissionNo.Enabled = true;

//            if (!rdMissionYes.Checked && !rdMissionNo.Checked)
//            {
//                rdMissionYes.CssClass = "radio-inline Req";
//                rdMissionNo.CssClass = "radio-inline Req";
//            }
//            else
//            {
//                rdMissionYes.CssClass = "radio-inline";
//                rdMissionNo.CssClass = "radio-inline";
//            }

//        }
//        else
//        {
//            rdMissionYes.Checked = false;
//            rdMissionNo.Checked = false;
//            rdMissionYes.Enabled = false;
//            rdMissionNo.Enabled = false;
//            rdMissionYes.CssClass = "radio-inline";
//            rdMissionNo.CssClass = "radio-inline";

//        }
//    }

//    protected void rdViaBank_CheckedChanged(object sender, EventArgs e)
//    {
//        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
//            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
//            rdViaCash.CssClass = rdViaCash.CssClass + " Req";
//        }
//        else
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
//            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
//            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
//        }
//    }
//    protected void rdViaCheck_CheckedChanged(object sender, EventArgs e)
//    {
//        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
//            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
//            rdViaCash.CssClass = rdViaCash.CssClass + " Req";
//        }
//        else
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
//            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
//            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
//        }
//    }
//    protected void rdViaCash_CheckedChanged(object sender, EventArgs e)
//    {
//        if (!rdViaBank.Checked && !rdViaCheck.Checked && !rdViaCash.Checked)
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass + " Req";
//            rdViaCheck.CssClass = rdViaCheck.CssClass + " Req";
//            rdViaCash.CssClass = rdViaCash.CssClass + " Req";
//        }
//        else
//        {
//            rdViaBank.CssClass = rdViaBank.CssClass.Replace("Req", "");
//            rdViaCheck.CssClass = rdViaCheck.CssClass.Replace("Req", "");
//            rdViaCash.CssClass = rdViaCash.CssClass.Replace("Req", "");
//        }
//    }

//    protected void rdHealthYes_CheckedChanged(object sender, EventArgs e)
//    {
//        fuMedicalAttachement.Style.Clear();
//        fuMedicalAttachement.Style.Add("display", "block");
//        pnlMedicalFiles.Style.Clear();
//        pnlMedicalFiles.Style.Add("display", "none");

//        lblMedicalClearaneAttach.Visible = true;
//    }
//    protected void rdHealthNo_CheckedChanged(object sender, EventArgs e)
//    {
//        fuMedicalAttachement.Style.Clear();
//        fuMedicalAttachement.Style.Add("display", "none");
//        pnlMedicalFiles.Style.Clear();
//        pnlMedicalFiles.Style.Add("display", "none");

//        lblMedicalClearaneAttach.Visible = false;

//    }
//    protected void rdHealthNA_CheckedChanged(object sender, EventArgs e)
//    {
//        fuMedicalAttachement.Style.Clear();
//        fuMedicalAttachement.Style.Add("display", "none");
//        pnlMedicalFiles.Style.Clear();
//        pnlMedicalFiles.Style.Add("display", "none");

//        lblMedicalClearaneAttach.Visible = false;
//    }
//    #endregion

//    #region Upload

//    protected void CheckSecurityClearanceFile()
//    {
//        string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

//        byte[] bytes = { 0 };
//        string fileName = "", contentType = "";

//        DataTable dt = new DataTable();

//        Media.GetSecurityTrainingFilesByTAID(TravelAuthorizationID, ref dt);

//        if (dt.Rows.Count > 0)
//        {
//            fuAttachment.Style.Clear();
//            fuAttachment.Style.Add("display", "none");
//            pnlFile.Style.Clear();
//            pnlFile.Style.Add("display", "block");

//            lblattch.Visible = true;
//        }
//        else
//        {
//            if (checkSecurityClearance.Checked)
//            {
//                fuAttachment.Style.Clear();
//                fuAttachment.Style.Add("display", "block");
//                pnlFile.Style.Clear();
//                pnlFile.Style.Add("display", "none");

//                lblattch.Visible = true;

//                checkSecurityClearance.CssClass = "checkbox-inline";
//                checkSecurityClearance.Style.Add("border-style", "");
//                checkSecurityClearance.Style.Add("border-color", "");
//            }
//            else
//            {
//                fuAttachment.Style.Clear();
//                fuAttachment.Style.Add("display", "none");
//                pnlFile.Style.Clear();
//                pnlFile.Style.Add("display", "none");

//                lblattch.Visible = false;
//            }
//        }
//    }
//    void ClearAll()
//    {
//        checkSecurityClearance.Checked = false;
//        checkSecurityTraining.Checked = false;
//    }
//    protected void ProcessUpload(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
//    {
//        try
//        {
//            string FileName = Path.GetFileName(e.FileName);
//            string FileNameOnly = Path.GetFileNameWithoutExtension(e.FileName);
//            string FileExtension = Path.GetExtension(e.FileName);

//            if (FileName != string.Empty)
//            {
//                Regex FilenameRegex = new Regex("(.*?)\\.(doc|docx|pdf|xls|xlsx)$");
//                if ((FilenameRegex.IsMatch(FileName, Convert.ToInt16(RegexOptions.IgnoreCase))))
//                {

//                    Guid gname = default(Guid);
//                    gname = Guid.NewGuid();
//                    string nameAndType = "SecurityClearance - " + FileNameOnly + FileExtension;
//                    string UploadFileName = "SecurityClearance - " + FileNameOnly + " - " + gname.ToString() + FileExtension;
//                    if (!Directory.Exists(Server.MapPath("~\\UploadedFiles\\")))
//                    {
//                        System.IO.Directory.CreateDirectory(Server.MapPath("~\\UploadedFiles\\"));
//                    }
//                    if (int.Parse(e.FileSize) < 4194304)
//                    {
//                        fuAttachment.SaveAs(Server.MapPath("~\\UploadedFiles\\" + UploadFileName));
//                        if (e.State == AjaxControlToolkit.AsyncFileUploadState.Success)
//                        {
//                            Media.InsertSecurityTrainingFiles(Decrypt(Request["TAID"]), Decrypt(Request["TANO"]), FileNameOnly, FileExtension, g.GetPhoto(Server.MapPath("~\\UploadedFiles\\" + UploadFileName)));
//                        }
//                        return;
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
//        }
//    }
//    protected void ibView_Click(object sender, EventArgs e)
//    {
//        try
//        {
//            if (!string.IsNullOrEmpty(Request["TAID"] as string))
//            {
//                string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
//                string TravelAuthorizationID = Decrypt(Request["TAID"]);

//                byte[] bytes = { 0 };
//                string fileName = "", contentType = "";

//                DataTable dt = new DataTable();

//                Media.GetSecurityTrainingFilesByTAID(TravelAuthorizationID, ref dt);

//                foreach (DataRow row in dt.Rows)
//                {
//                    bytes = (byte[])row["FileData"];
//                    contentType = row["FileExtension"].ToString();
//                    fileName = row["FileName"].ToString();
//                }

//                Response.Clear();
//                Response.Buffer = true;
//                Response.Charset = "";
//                Response.Cache.SetCacheability(HttpCacheability.NoCache);
//                Response.ContentType = contentType;
//                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + contentType);
//                Response.BinaryWrite(bytes);
//                Response.Flush();
//                HttpContext.Current.ApplicationInstance.CompleteRequest();
//            }

//        }
//        catch (Exception ex)
//        {

//            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
//        }
//    }
//    protected void ibDelete_Click(object sender, EventArgs e)
//    {
//        if (!string.IsNullOrEmpty(Request["TAID"] as string))
//        {
//            string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

//            Media.DeleteSecurityTrainingFiles(TravelAuthorizationID);
//            lblUploadMsg.ForeColor = Color.Green;
//            fuAttachment.Style.Clear();
//            fuAttachment.Style.Add("display", "block");
//            pnlFile.Style.Clear();
//            pnlFile.Style.Add("display", "none");
//            lblUploadMsg.Text = "File has been deleted successfully";
//        }
//        else
//        {
//            lblUploadMsg.ForeColor = Color.Green;
//            lblUploadMsg.Text = "File was deleted successfully";
//            fuAttachment.Style.Clear();
//            fuAttachment.Style.Add("display", "block");
//            pnlFile.Style.Clear();
//            pnlFile.Style.Add("display", "none");
//            lblUploadMsg.Text = "";
//        }
//        CheckMedicalFile();
//        CheckSecurityClearanceFile();
//    }

//    #endregion

//    #region UploadMedical
//    protected void CheckMedicalFile()
//    {
//        string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

//        byte[] bytes = { 0 };
//        string fileName = "", contentType = "";

//        DataTable dt = new DataTable();

//        Media.GetMedicalFilesByTAID(TravelAuthorizationID, ref dt);

//        if (dt.Rows.Count > 0)
//        {
//            fuMedicalAttachement.Style.Clear();
//            fuMedicalAttachement.Style.Add("display", "none");
//            pnlMedicalFiles.Style.Clear();
//            pnlMedicalFiles.Style.Add("display", "block");
//            lblMedicalClearaneAttach.Visible = true;
//        }
//        else
//        {
//            if (rdHealthYes.Checked)
//            {
//                fuMedicalAttachement.Style.Clear();
//                fuMedicalAttachement.Style.Add("display", "block");
//                pnlMedicalFiles.Style.Clear();
//                pnlMedicalFiles.Style.Add("display", "none");
//                lblMedicalClearaneAttach.Visible = true;
//            }
//            else
//            {
//                fuMedicalAttachement.Style.Clear();
//                fuMedicalAttachement.Style.Add("display", "none");
//                pnlMedicalFiles.Style.Clear();
//                pnlMedicalFiles.Style.Add("display", "none");
//                lblMedicalClearaneAttach.Visible = false;
//            }

//        }
//    }
//    protected void ProcessUploadMedical(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
//    {
//        try
//        {
//            string FileName = Path.GetFileName(e.FileName);
//            string FileNameOnly = Path.GetFileNameWithoutExtension(e.FileName);
//            string FileExtension = Path.GetExtension(e.FileName);

//            if (FileName != string.Empty)
//            {
//                Regex FilenameRegex = new Regex("(.*?)\\.(doc|docx|pdf|xls|xlsx)$");
//                if ((FilenameRegex.IsMatch(FileName, Convert.ToInt16(RegexOptions.IgnoreCase))))
//                {

//                    Guid gname = default(Guid);
//                    gname = Guid.NewGuid();
//                    string nameAndType = "MedicalClearance - " + FileNameOnly + FileExtension;
//                    string UploadFileName = "MedicalClearance - " + FileNameOnly + " - " + gname.ToString() + FileExtension;
//                    if (!Directory.Exists(Server.MapPath("~\\UploadedFiles\\")))
//                    {
//                        System.IO.Directory.CreateDirectory(Server.MapPath("~\\UploadedFiles\\"));
//                    }
//                    if (int.Parse(e.FileSize) < 4194304)
//                    {
//                        fuMedicalAttachement.SaveAs(Server.MapPath("~\\UploadedFiles\\" + UploadFileName));
//                        if (e.State == AjaxControlToolkit.AsyncFileUploadState.Success)
//                        {
//                            //Session["UploadFileNameMedical"] = UploadFileName;
//                            Media.InsertMedicalFiles(Decrypt(Request["TAID"]), Decrypt(Request["TANO"]), FileNameOnly, FileExtension, g.GetPhoto(Server.MapPath("~\\UploadedFiles\\" + UploadFileName)));
//                        }
//                        return;
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
//        }
//    }
//    protected void ibViewMedical_Click(object sender, EventArgs e)
//    {
//        try
//        {
//            if (!string.IsNullOrEmpty(Request["TAID"] as string))
//            {
//                string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

//                byte[] bytes = { 0 };
//                string fileName = "", contentType = "";

//                DataTable dt = new DataTable();

//                Media.GetMedicalFilesByTAID(TravelAuthorizationID, ref dt);

//                foreach (DataRow row in dt.Rows)
//                {
//                    bytes = (byte[])row["FileData"];
//                    contentType = row["FileExtension"].ToString();
//                    fileName = row["FileName"].ToString();
//                }

//                Response.Clear();
//                Response.Buffer = true;
//                Response.Charset = "";
//                Response.Cache.SetCacheability(HttpCacheability.NoCache);
//                Response.ContentType = contentType;
//                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + contentType);
//                Response.BinaryWrite(bytes);
//                Response.Flush();
//                HttpContext.Current.ApplicationInstance.CompleteRequest();

//            }

//        }
//        catch (Exception ex)
//        {

//            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
//        }
//    }
//    protected void ibDeleteMedical_Click(object sender, EventArgs e)
//    {
//        if (!string.IsNullOrEmpty(Request["TAID"] as string))
//        {
//            string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

//            Media.DeleteMedicalFiles(TravelAuthorizationID);
//            lblUploadMessageMedical.ForeColor = Color.Green;
//            fuMedicalAttachement.Style.Clear();
//            fuMedicalAttachement.Style.Add("display", "block");
//            pnlMedicalFiles.Style.Clear();
//            pnlMedicalFiles.Style.Add("display", "none");
//            lblUploadMessageMedical.Text = "File has been deleted successfully";
//        }
//        else
//        {
//            lblUploadMessageMedical.ForeColor = Color.Green;
//            lblUploadMessageMedical.Text = "File was deleted successfully";
//            fuMedicalAttachement.Style.Clear();
//            fuMedicalAttachement.Style.Add("display", "block");
//            pnlMedicalFiles.Style.Clear();
//            pnlMedicalFiles.Style.Add("display", "none");
//            lblUploadMessageMedical.Text = "";
//        }
//        CheckMedicalFile();
//        CheckSecurityClearanceFile();
//    }
//    #endregion

//}
