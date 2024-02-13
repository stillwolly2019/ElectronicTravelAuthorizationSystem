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

public partial class Reports_RCWizard_WizardHeader : System.Web.UI.UserControl
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    //Business.Staff S = new Business.Staff();
    AuthenticatedPageClass a = new AuthenticatedPageClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IndicateStep();
        }
    }

    #region steps
    void IndicateStep()
    {
        double Progress = 0.0;
        if (!string.IsNullOrEmpty(Request["RCD"] as string) && !string.IsNullOrEmpty(Request["loc"] as string))
        {
            string Step;
            string LocationName = R.GetStaffLocationName(a.Decrypt(Request["loc"].ToString()));
            DateTime Datt = Convert.ToDateTime(R.GetRadioCheckStartDateByRadioCheckID(a.Decrypt(Request["RCD"].ToString())));
            Step = R.GetRadioCheckStepByRadioCheckDate(Datt);
            lblTitle.Text = "Radio Check Results - " + Datt.ToString("dddd, dd MMMM yyyy")+"("+LocationName+")";
            Progress = 100;
            tdStep1.Attributes["style"] = "width: 20%; text-align: center";
            if (string.IsNullOrEmpty(Request["First"] as string))
            {
                Response.Redirect("Step1_MasterStaffList.aspx?RCD=" + Request["RCD"].ToString() + "&loc="+ Request["loc"].ToString() + "&First=1", false);
            }
        }
        else
        {
            lblTitle.Text = "Radio Check Results";
            Progress = 20;
            tdStep1.Attributes["style"] = "width: 20%; text-align: center";

            if (string.IsNullOrEmpty(Request["First"] as string))
            {
                Response.Redirect("Step1_MasterStaffList.aspx?First=1", false);
            }

            lbStep2.Visible = false;
            lbStep3.Visible = false;
            lbStep4.Visible = false;
            lbStepDownload.Visible = false;
        }
        ltrProgress.Text = "<div class='progress progress-striped active'><div class='progress-bar progress-bar-primary' role='progressbar' aria-valuenow='20' aria-valuemin='0' aria-valuemax='100' style='width: " + Progress + "%'></div></div>";

    }
    protected void lbStep1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step1_MasterStaffList.aspx?RCD=" + Request["RCD"].ToString() + "&loc="+ Request["loc"].ToString() + "&First=1" + "&Step=Step 1", false);
    }
    protected void lbStep2_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step2_StaffAcountedFor.aspx?RCD=" + Request["RCD"].ToString() + "&loc=" + Request["loc"].ToString() + "&First=1" + "&Step=Step 2", false);
    }
    protected void lbStep3_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step3_StaffUnacountedFor.aspx?RCD=" + Request["RCD"].ToString() + "&loc=" + Request["loc"].ToString() + "&First=1" + "&Step=Step 3", false);
    }
    protected void lbStep4_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step4_Movements.aspx?RCD=" + Request["RCD"].ToString() + "&loc=" + Request["loc"].ToString() + "&First=1" + "&Step=Step 4", false);
    }
    protected void lbStepDownload_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step5_DownloadReport.aspx?RCD=" + Request["RCD"].ToString() + "&loc=" + Request["loc"].ToString() + "&First=1" + "&Step=Step 5", false);
    }

    #endregion

}
