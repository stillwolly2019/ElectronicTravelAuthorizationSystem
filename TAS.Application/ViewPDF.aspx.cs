using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WebForms;

public partial class TravelAuthorization_ViewPDF : AuthenticatedPageClass
{
    Business.TravelAuthorization TA = new Business.TravelAuthorization();
    Business.Media Media = new Business.Media();
    HttpContext context = HttpContext.Current;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewSecurityClearence();
        }
    }

    void ViewSecurityClearence()
    {
        try
        {
            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = Request["TAID"];
                byte[] bytes = { 0 };
                string fileName = "", contentType = "";

                DataTable dt = new DataTable();
                Media.GetSecurityTrainingFilesByTAID(TravelAuthorizationID, ref dt);
                foreach (DataRow row in dt.Rows)
                {
                    bytes = (byte[])row["FileData"];
                    contentType = row["FileExtension"].ToString();
                    fileName = row["FileName"].ToString();
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName + contentType);

                string sid = bytes.ToString();
                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }



}