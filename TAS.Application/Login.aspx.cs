using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;
using System.Text;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using FormsAuthAd;
public partial class Login : PageClass
{
    AuthenticatedPageClass A = new AuthenticatedPageClass();
    AuthenticatedPageClass B = new AuthenticatedPageClass();
    Business.Security p = new Business.Security();
    Business.Lookups Lookup = new Business.Lookups();
    private string AppName = ConfigurationManager.AppSettings["ApplicationName"];

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblError.Visible = false;
                int retvalAD = -1;
                string[] UserName;
                UserName = HttpContext.Current.User.Identity.Name.Split('\\');
                retvalAD = p.ADLogin(UserName[0], false);
                if (retvalAD == 1)
                {
                    if (Request["RetURL"] != null)
                        Response.Redirect(Request["RetURL"], false);
                    else
                        Response.Redirect("Default.aspx", false);
                }
                else
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();
                    DataTable dtL = new DataTable();
                    Lookup.GetDisclaimer(ref dtL);
                    lblDisclaimer.Text = dtL.Rows[0]["DisclaimerText"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {

        lblError.Visible = false;
        int retvalAD = -1;

        string ADPath = ConfigurationManager.AppSettings["ADPath"], Domain = ConfigurationManager.AppSettings["Domain"];
        FormsAuthAd.LDAPAuthentication adAuth = new FormsAuthAd.LDAPAuthentication(ADPath);
        string LoginName = txtUName.Text;
        LoginName = LoginName.TrimStart();
        LoginName = LoginName.TrimEnd();
        if (LoginName.Contains("@"))
        {
            LoginName = LoginName.Substring(0, LoginName.IndexOf("@"));
        }
        try
        {
            //if(p.IsValidUser(LoginName, txtPassword.Text))
            if (true == adAuth.IsAuthenticated(Domain, LoginName, txtPassword.Text, ADPath))
            {
                retvalAD = p.ADLogin(LoginName, true);
                if (retvalAD == 1)
                {
                    p.ADSingleSignOn();
                    if (Request["RetURL"] != null)
                        Response.Redirect(Request["RetURL"], false);
                    else
                        Response.Redirect("Default.aspx", false);
                }
                else
                {
                    Label1.Visible = true;
                    Label1.ForeColor = Color.Red;
                    Label1.Text = "Invalid user name and/or password";
                }
            }
            else
            {
                Label1.Visible = true;
                Label1.ForeColor = Color.Red;
                Label1.Text = "Invalid user name and/or password";
            }
        }
        catch (Exception exIOMINT)
        {
            Label1.Visible = true;
            Label1.ForeColor = Color.Red;
            Label1.Text = "Invalid user name and/or password";
        }
    }
}

