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

public partial class TravelAuthorization_TravelAuthorizationFormWizard : AuthenticatedPageClass
{
    Business.TravelAuthorization t = new Business.TravelAuthorization();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["TAID"] as string) && !string.IsNullOrEmpty(Request["TANO"] as string))
            {
                IFrameStep1.Src = "TAWizard/Step1_TravelersInformation.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString();
            }
            else
            {
                IFrameStep1.Src = "TAWizard/Step1_TravelersInformation.aspx?First=1";
            }
            
        }
    }
}