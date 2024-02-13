  
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

public partial class Reports_RadioCheckFormWizard : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["RCD"] as string) && !string.IsNullOrEmpty(Request["loc"] as string))
            {
                IFrameStep1.Src = "RCWizard/Step1_MasterStaffList.aspx?RCD=" + Request["RCD"].ToString()+"&loc=" + Request["loc"].ToString();
            }
            else
            {
                IFrameStep1.Src = "RCWizard/Step1_MasterStaffList.aspx?First=1";
            }

        }
    }
}
