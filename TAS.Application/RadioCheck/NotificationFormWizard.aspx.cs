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

public partial class Notification_NotificationFormWizard : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    HttpContext context = HttpContext.Current;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
                if (!string.IsNullOrEmpty(Request["MRID"] as string) && !string.IsNullOrEmpty(Request["MRNO"] as string))
                {
                    IFrameStep1.Src = "NOAWizard/1_NotifiersInformation.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString();
                }
                else
                {
                    IFrameStep1.Src = "NOAWizard/1_NotifiersInformation.aspx?First=1";
                }
        }
    }
}

