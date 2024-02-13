  

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

public partial class Staff_StaffManagementFormWizard : AuthenticatedPageClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["PERNO"] as string))
            {
                IFrameStep1.Src = "SMWizard/1_PersonalInformation.aspx?PERNO=" + Request["PERNO"].ToString();
            }
            else
            {

                IFrameStep1.Src = "SMWizard/1_PersonalInformation.aspx?First=1";
            }

        }
    }
}