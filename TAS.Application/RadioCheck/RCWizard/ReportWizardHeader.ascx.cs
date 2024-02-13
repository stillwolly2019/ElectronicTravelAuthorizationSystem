
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

public partial class RadioCheck_RCWizard_ReportWizardHeader : System.Web.UI.UserControl
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    Business.Security Sec = new Business.Security();
    HttpContext context = HttpContext.Current;
    Business.MailModel MM = new Business.MailModel();
    //AuthenticatedPageClass a = new AuthenticatedPageClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IndicateStep();
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
                NOAStatusDiv.Visible = Convert.ToBoolean(dt.Rows[0]["Amend"]);
            }
            else
            {
                NOAStatusDiv.Visible = false;
            }
        }

        catch (Exception ex)
        {

        }
    }
    protected void gvHistoryStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvHistoryStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    void IndicateStep()
    {
        if (!string.IsNullOrEmpty(Request["DF"]) && !string.IsNullOrEmpty(Request["DF"]))
        {
            DateTime StartDate = Convert.ToDateTime(Request["DF"]);
            DateTime EndDate = Convert.ToDateTime(Request["DT"]);
            if (StartDate == EndDate)
            {
                Response.Redirect("PreviewRadioCheckReport.aspx?DF=" + StartDate + "&First=1", false);
                //lblTitle.Text = "Radio Check Results - " + Convert.ToDateTime(StartDate).ToString("dddd, dd MMMM yyyy");
            }
            else
            {
                Response.Redirect("PreviewRadioCheckReport.aspx?DF=" + StartDate + "&&DT=" + EndDate + "&First=1", false);
                //lblTitle.Text = "Radio Check Results - " + Convert.ToDateTime(StartDate).ToString("dddd, dd MMMM yyyy") + " "+ Convert.ToDateTime(EndDate).ToString("dddd, dd MMMM yyyy");
            }
        }
        else
        {
            DateTime StartDate = DateTime.Now.Date;
            //lblTitle.Text = "Radio Check Results - "+ StartDate.ToString("dddd, dd MMMM yyyy");
            if (string.IsNullOrEmpty(Request["First"] as string))
            {
                Response.Redirect("PreviewRadioCheckReport.aspx?DF=" + StartDate + "&First=1", false);
            }
        }

    }
    
}

