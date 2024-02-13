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

public partial class Staff_SMWizard_WizardHeader : System.Web.UI.UserControl
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    //Business.Users u = new Business.Users();
    Business.Security Sec = new Business.Security();
    //Business.MailModel MM = new Business.MailModel();
    //TravelAuthorizationDataModel db = new TravelAuthorizationDataModel();
    //Globals g = new Globals();
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
    protected void CheckStatusPermission()
    {
        try
        {
            Objects.User UserInfo = (Objects.User)Session["userinfo"];
            System.Data.DataTable dt = new System.Data.DataTable();
            Sec.getPagePermissions("/WizardHeader.ascx", ref dt);
        }

        catch (Exception ex)
        {

        }
    }

    #endregion

    #region steps

    void IndicateStep()
    {
        double Progress = 0.0;

        if (!string.IsNullOrEmpty(Request["PERNO"] as string))
        {
            string StaffName = R.GetStaffName(a.Decrypt(Request["PERNO"].ToString()));
            lblTitle.Text = "Staff Information - " + StaffName;
            string Step;
            Step = R.GetStaffInformationStep(a.Decrypt(Request["PERNO"].ToString()));

            if (Step == "Step 1")
            {
                Progress = 20;
                tdStep1.Attributes["style"] = "width: 20%; text-align: center";

                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("1_PersonalInformation.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
                }
                lbStep2.Visible = false;
                lbStep3.Visible = false;
                lbStep4.Visible = false;
                lbStep5.Visible = false;
                //lbStep6.Visible = false;
            }
           
            else if (Step == "Step 2")
            {
                Progress = 40;
                tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("2_Contacts.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
                }

                lbStep3.Visible = false;
                lbStep4.Visible = false;
                lbStep5.Visible = false;
                //lbStep6.Visible = false;

            }
            else if (Step == "Step 3")
            {
                Progress = 60;
                tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                tdStep3.Attributes["style"] = "width: 20%; text-align: center";
                //tdStep4.Attributes["style"] = "width: 20%; text-align: center";
                // Hiding other steps
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("3_Location.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
                }

                lbStep4.Visible = false;
                lbStep5.Visible = false;
                //lbStep6.Visible = false;

            }
            else if (Step == "Step 4")
            {
                Progress = 80;

                tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                tdStep3.Attributes["style"] = "width: 20%; text-align: center";
                tdStep4.Attributes["style"] = "width: 20%; text-align: center";
                //tdStep5.Attributes["style"] = "width: 20%; text-align: center";
                // Hiding other steps
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("4_EmergencyContacts.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
                }
                lbStep5.Visible = false;
            }
            else if (Step == "Step 5")
            {
                Progress = 100;

                tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                tdStep2.Attributes["style"] = "width: 20%; text-align: center";
                tdStep3.Attributes["style"] = "width: 20%; text-align: center";
                tdStep4.Attributes["style"] = "width: 20%; text-align: center";
                tdStep5.Attributes["style"] = "width: 20%; text-align: center";
                //tdStep6.Attributes["style"] = "width: 18%; text-align: center";
                // Hiding other steps

                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("5_DownloadStaffInformation.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
                }
            }

            else
            {
                lblTitle.Text = "Staff Information Management";
                Progress = 15;
                tdStep1.Attributes["style"] = "width: 20%; text-align: center";
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("1_PersonalInformation.aspx?First=1", false);
                }

                lbStep2.Visible = false;
                lbStep3.Visible = false;
                lbStep4.Visible = false;
                lbStep5.Visible = false;
                //lbStep6.Visible = false;
            }


        }
        else
        {
            lblTitle.Text = "Staff Information Management";

            Progress = 20;
            tdStep1.Attributes["style"] = "width: 20%; text-align: center";
            if (string.IsNullOrEmpty(Request["First"] as string))
            {
                Response.Redirect("1_PersonalInformation.aspx?First=1", false);
            }
            lbStep1.Visible = false;
            lbStep2.Visible = false;
            lbStep3.Visible = false;
            lbStep4.Visible = false;
            lbStep5.Visible = false;
           // lbStep6.Visible = false;
        }
       
        ltrProgress.Text = "<div class='progress progress-striped active'><div class='progress-bar progress-bar-primary' role='progressbar' aria-valuenow='20' aria-valuemin='0' aria-valuemax='100' style='width: " + Progress + "%'></div></div>";

    }
    protected void lbStep1_Click(object sender, EventArgs e)
    {
        Response.Redirect("1_PersonalInformation.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 1", false);
    }
    protected void lbStep2_Click(object sender, EventArgs e)
    {
        Response.Redirect("2_Contacts.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 2", false);
        //Response.Redirect("2_StaffCategory.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 2", false);
    }
    protected void lbStep3_Click(object sender, EventArgs e)
    {
        Response.Redirect("3_Location.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 3", false);
        //Response.Redirect("3_Contacts.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 3", false);
    }
    protected void lbStep4_Click(object sender, EventArgs e)
    {
        Response.Redirect("4_EmergencyContacts.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 4", false);
        //Response.Redirect("4_Location.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 4", false);
    }
    protected void lbStep5_Click(object sender, EventArgs e)
    {
        Response.Redirect("5_DownloadStaffInformation.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 5", false);
        //Response.Redirect("5_EmergencyContacts.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 5", false);
    }
    //protected void lbStep6_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("6_DownloadStaffInformation.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1" + "&Step=Step 6", false);
    //}

    #endregion

}
