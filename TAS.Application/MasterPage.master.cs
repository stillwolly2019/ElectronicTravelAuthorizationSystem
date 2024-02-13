using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Web.Profile;
public partial class MasterPage : System.Web.UI.MasterPage
{
    Business.Security s = new Business.Security();
    Business.Users u = new Business.Users();
    public string sTargetURLForSessionTimeout;
    public int iWarningTimeoutInMilliseconds;
    public int iSessionTimeoutInMilliseconds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userinfo"] == null)
        {
            try
            {
                Response.Redirect("~/Login.aspx?RetURL=" + Request.ServerVariables["Url"], false);
            }
            catch (Exception ex)
            {
                Response.Redirect("Login.aspx?RetURL=" + Request.ServerVariables["Url"], false);
            }
        }
        else
        {
            if (!IsPostBack)
            {
                try
                {
                    //ltrBootstrap1.Text = "<div class=" + "'btn-group open'" + ">";
                    //ltrBootstrap2.Text = "</div>";
                    //GetNewUsers
                    Objects.User ui = (Objects.User)Session["userinfo"];
                    lblUserName.Text = ui.FirstName + " " + ui.LastName.ToUpper();

                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();

                    s.GetUserMenu(ref dt);
                    s.GetUserMenu(ref dt1);
                    DataView dvMain = new DataView();
                    DataView dv = new DataView();
                    dv = dt1.DefaultView;
                    dvMain = dt.DefaultView;
                    dvMain.RowFilter = "ParentID = '00000000-0000-0000-0000-000000000000'";
                    string TheMenuHTML = "<ul class='nav' id='side-menu'>";
                    for (int i = 0; i <= dvMain.Count - 1; i++)
                    {
                        TheMenuHTML += "<li><a href='" + (HttpContext.Current.Handler as Page).ResolveUrl("~/" + dvMain[i]["PageURL"]) + "'>" + dvMain[i]["PageName"];
                        dv.RowFilter = "ParentID = '" + dvMain[i]["PageID"] + "'";
                        if (dv.Count > 0)
                        {
                            TheMenuHTML += "<span class='fa arrow'></span></a><ul class='nav nav-second-level'>";
                            for (int x = 0; x <= dv.Count - 1; x++)
                            {
                                TheMenuHTML += "<li><a href='" + (HttpContext.Current.Handler as Page).ResolveUrl("~/" + dv[x]["PageURL"]) + "'>" + dv[x]["PageName"] + "</a></li>";
                            }
                            TheMenuHTML += "</ul></li>";
                        }
                        else
                        { TheMenuHTML += "</a></li>"; }

                    }
                    string approot = ConfigurationManager.AppSettings["application.rootOut"];
                    TheMenuHTML += "<li><a href='" + approot + "/UserManual/TASS User Manual.docx' target='_blank'>User Manual</a></li>";
                    TheMenuHTML += "</ul>";
                    TheMenu.InnerHtml = TheMenuHTML;
                }
                catch (Exception ex)
                {
                    IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
                }

            }
        }
    }


    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        s.ADLogOut();
        Response.Redirect("~/Login.aspx");
    }
}
