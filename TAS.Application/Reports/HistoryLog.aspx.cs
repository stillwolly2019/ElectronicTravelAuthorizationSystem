using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security;
using Microsoft.Reporting.WebForms;
using System.Reflection;
public partial class Reports_HistoryLog : AuthenticatedPageClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RV.ZoomMode = ZoomMode.Percent;
            RV.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();

            RV.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            RV.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/HistorySearchLog";
            RV.ShowPageNavigationControls = true;
            RV.ShowPrintButton = true;
            RV.ShowExportControls = true;
            RV.ServerReport.Refresh();
        }
    }
}