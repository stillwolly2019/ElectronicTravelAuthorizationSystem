using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WinForms;
using System.Net;

public partial class Staff_SMWizard_5_DownloadStaffInformation : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    byte[] Lbytes = new byte[0];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LinkButton lnk = new LinkButton();
                lnk = (LinkButton)WizardHeader.FindControl("lbStep5");
                lnk.CssClass = "btn btn-info fa fa-download btn-circle btn-lg  fa fa-download";

                if (Request["PERNO"] != null)
                {
                    ViewSIForm();
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee.Message);
            }
        }

    }
    public bool Export(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string PERNO)
    {
        try
        {
            Microsoft.Reporting.WinForms.Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;

            Guid gname = default(Guid);
            gname = Guid.NewGuid();

            filetype = "Pdf";
            byte[] bytes = viewer.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + PERNO + ".pdf"), FileMode.Create, FileAccess.Write))
                fs.Write(bytes, 0, bytes.Length);

            Lbytes = bytes;
        }
        catch (Exception ex)
        {

        }
        return true;
    }
    protected void ViewSIForm()
    {
        Microsoft.Reporting.WinForms.ServerReport sr = new Microsoft.Reporting.WinForms.ServerReport();
        string ReportMainPath = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
        sr.ReportServerUrl = new System.Uri(ReportMainPath);
        sr.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/StaffInformationForm";
        Guid gname = default(Guid);
        gname = Guid.NewGuid();

        if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }
        else
        {
            System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }

        Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("PRISMNumber", Decrypt(Request["PERNO"]));
        sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });

        Export(sr, "PDF", "StaffInformationForm", gname.ToString(), Decrypt(Request["PERNO"]));

        if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }
        else
        {
            System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
            System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
        }

        File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\StaffInformationForm.PDF"), Lbytes);

        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\StaffInformationForm.PDF";


    }

}

