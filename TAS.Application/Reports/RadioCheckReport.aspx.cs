  
      
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

public partial class Reports_RadioCheckReport : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    HttpContext context = HttpContext.Current;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["DF"] as string) && !string.IsNullOrEmpty(Request["DT"] as string))
            {
                IFrameStep1.Src = "RCWizard/PreviewRadioCheckReport.aspx?DF=" + Request["DF"].ToString() + "&&DT=" + Request["DT"].ToString();
            }
            else
            {
                IFrameStep1.Src = "RCWizard/PreviewRadioCheckReport.aspx?First=1";
            }

        }
    }
}


