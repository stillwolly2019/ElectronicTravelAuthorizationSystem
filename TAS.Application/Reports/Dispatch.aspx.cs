  
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

public partial class Reports_Dispatch : AuthenticatedPageClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["TD"] as string))
            {
                IFrameReport.Src = "RCWizard/PreviewDispatchReport.aspx?TD=" + Request["TD"].ToString();
            }
            else
            {
                IFrameReport.Src = "RCWizard/PreviewDispatchReport.aspx?First=1";
            }
        }
    }
}


