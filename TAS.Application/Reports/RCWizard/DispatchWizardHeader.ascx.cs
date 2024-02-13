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

public partial class Reports_RCWizard_DispatchWizardHeader : System.Web.UI.UserControl
{
    Business.Security Sec = new Business.Security();
    HttpContext context = HttpContext.Current;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["TD"]))
            {
                DateTime TravelDate = Convert.ToDateTime(Request["TD"]);
                Response.Redirect("PreviewDispatchReport.aspx?TD=" + TravelDate + "&First=1", false);
            }
            else
            {
                DateTime TravelDate = DateTime.Now.Date;
                if (string.IsNullOrEmpty(Request["First"] as string))
                {
                    Response.Redirect("PreviewDispatchReport.aspx?TD=" + TravelDate + "&First=1", false);
                }
            }
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
                StatusDiv.Visible = Convert.ToBoolean(dt.Rows[0]["Amend"]);
            }
            else
            {
                StatusDiv.Visible = false;
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
        if (!string.IsNullOrEmpty(Request["TD"]))
        {
            DateTime TravelDate = Convert.ToDateTime(Request["TD"]);
            Response.Redirect("PreviewDispatchReport.aspx?TD=" + TravelDate + "&First=1", false);
        }
        else
        {
            DateTime TravelDate = DateTime.Now.Date;
            if (string.IsNullOrEmpty(Request["First"] as string))
            {
                Response.Redirect("PreviewDispatchReport.aspx?TD=" + TravelDate + "&First=1", false);
            }
        }

    }

}

