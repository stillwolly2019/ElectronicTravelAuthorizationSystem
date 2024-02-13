using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TravelAuthorization_TAWizard_Step4_Itinerary : AuthenticatedPageClass
{
    AuthenticatedPageClass a = new AuthenticatedPageClass();
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.Media Media = new Business.Media();
    Globals g = new Globals();
    HttpContext context = HttpContext.Current;
    byte[] Lbytes = new byte[0];

    protected void Page_Load(object sender, EventArgs e)
    {
            Objects.User ui = (Objects.User)Session["userinfo"];
        if (!IsPostBack)
        {
            Calendar1.StartDate = DateTime.Now;
            Calendar2.StartDate = DateTime.Now;

            System.Web.UI.HtmlControls.HtmlGenericControl TAStatus =
                null;
            TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
            TAStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep4");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            FillDDLs();
            //Initializecheck();
            if (!string.IsNullOrEmpty(Request["TANO"] as string))
            {
                string TravelAuthorizationNo = Decrypt(Request["TANO"].ToString());
                FillTravelItineraryItems(TravelAuthorizationNo);
                //FillDispatchItineraryItems(TravelAuthorizationNo);

                // lock the form only if the status is in the TEC
                DataTable dt = new DataTable();
                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0]["StatusCode"].ToString() == "TECCom" || dt.Rows[0]["StatusCode"].ToString() == "TECDI" || dt.Rows[0]["StatusCode"].ToString() == "SET" || dt.Rows[0]["StatusCode"].ToString() == "TECRTA" || dt.Rows[0]["StatusCode"].ToString() == "TECDC" || dt.Rows[0]["StatusCode"].ToString() == "NDSA")
                    if(Convert.ToBoolean(dt.Rows[0]["IsEditable"].ToString()) ==false)
                   {
                            pnlContent.Enabled = false;
                    }
                }

            }
            SecurityClearence.Visible = false;
            if (ui.IsSecReqVerifier == true)
            {
                DisplaySecurityClearence();
            }
            

        }
    }

    protected void DisplaySecurityClearence()
    {
        if (!String.IsNullOrEmpty(Request["TAID"]))
        {
            string TravelAuthorizationID = a.Decrypt(Request["TAID"]);
            byte[] bytes = { 0 };
            string fileName = "", contentType = "";
            DataTable dtt = new DataTable();
            Media.GetSecurityTrainingFilesByTAID(TravelAuthorizationID, ref dtt);
            if (dtt.Rows.Count > 0)
            {
                foreach (DataRow row in dtt.Rows)
                {
                    bytes = (byte[])row["FileData"];
                    contentType = row["FileExtension"].ToString();
                    fileName = row["FileName"].ToString();

                    //Export report
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

                    File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\"+ fileName+".PDF"), bytes);
                    IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\"+ fileName + ".PDF#toolbar=0";
                    SecurityClearence.Visible = true;
                }
            }
            else
            {
                SecurityClearence.Visible = false;
            }
            

        }



    }


    //protected void DisplaySecurityClearence()
    //{
    //    string TravelAuthorizationID = Request["TAID"].ToString();
    //    Guid gname = default(Guid);
    //    gname = Guid.NewGuid();

    //    if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
    //    {
    //        System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
    //    }
    //    else
    //    {
    //        System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
    //        System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
    //    }

    //    Microsoft.Reporting.WinForms.ReportParameter p = new Microsoft.Reporting.WinForms.ReportParameter("TravelAuthorizationNumber", Decrypt(Request["TANO"]).ToString());
    //    sr.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter[] { p });
    //    Export(sr, "PDF", "TAForm", gname.ToString(), Decrypt(Request["TAID"]));

    //    if (!Directory.Exists(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString())))
    //    {
    //        System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
    //    }
    //    else
    //    {
    //        System.IO.Directory.Delete(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()), true);
    //        System.IO.Directory.CreateDirectory(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString()));
    //    }

    //    File.WriteAllBytes(Server.MapPath("~\\DownloadedFiles\\" + gname.ToString() + "\\TAForm.PDF"), Lbytes);
    //    if (dt == null)
    //    {
    //        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\TAForm.PDF#toolbar=0";
    //    }
    //    else
    //    {
    //        IFramePDF.Src = "~\\DownloadedFiles\\" + gname.ToString() + "\\TAForm.PDF";
    //    }

    //}

    public bool Export(Microsoft.Reporting.WinForms.ServerReport viewer, string exportType, string reportsTitle, string DIR, string uname)
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

            using (var fs = new FileStream(Server.MapPath("~\\DownloadedFiles\\" + DIR + "\\" + uname + ".pdf"), FileMode.Create, FileAccess.Write))
                fs.Write(bytes, 0, bytes.Length);

            Lbytes = bytes;
        }
        catch (Exception ex)
        {

        }
        return true;
    }


    /*

    #region Dispatch Itinineray
    void Initializecheck()
    {
        txtFlightReference.Enabled = false;
        txtPickupDate.Enabled = false;
        //txtCarrier.Enabled = false;
        txtETAETD.Enabled = false;
        txtPickupLocation.Enabled = false;
        txtDropOffLocation.Enabled = false;
        txtPickupTime.Enabled = false;
        txtFlightReference.Text = "";
        txtPickupDate.Text = "";
        //txtCarrier.Text = "";
        txtETAETD.Text = "";
        txtPickupLocation.Text = "";
        txtDropOffLocation.Text = "";
        txtPickupTime.Text = "";
    }
    protected void checkApplicable_CheckedChanged(object sender, EventArgs e)
    {
        if (checkApplicable.Checked)
        {

            ibAddDispatch.Enabled = true;
            txtFlightReference.Enabled = true;
            txtPickupDate.Enabled = true;
            //txtCarrier.Enabled = true;
            txtETAETD.Enabled = true;
            txtPickupLocation.Enabled = true;
            txtDropOffLocation.Enabled = true;
            txtPickupTime.Enabled = true;

            txtFlightReference.Text = "";
            txtPickupDate.Text = "";
            //txtCarrier.Text = "";
            txtETAETD.Text = "";
            txtPickupLocation.Text = "";
            txtDropOffLocation.Text = "";
            txtPickupTime.Text = "";

        }
        else
        {
            ibAddDispatch.Enabled = false;
            txtFlightReference.Enabled = false;
            txtPickupDate.Enabled = false;
            //txtCarrier.Enabled = false;
            txtETAETD.Enabled = false;
            txtPickupLocation.Enabled = false;
            txtDropOffLocation.Enabled = false;
            txtPickupTime.Enabled = false;

            txtFlightReference.Text = "";
            txtPickupDate.Text = "";
            //txtCarrier.Text = "";
            txtETAETD.Text = "";
            txtPickupLocation.Text = "";
            txtDropOffLocation.Text = "";
            txtPickupTime.Text = "";
        }
    }
    protected void gvDispatch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditDispatch")
            {
                ClearAllDispatch();
                gvDispatch.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvDispatch.SelectedRow.BackColor = Color.LightGray;
                gvDispatch.SelectedRow.ForeColor = Color.Black;
                txtFlightReference.Text = gvDispatch.SelectedRow.Cells[0].Text;
                txtETAETD.Text = gvDispatch.SelectedRow.Cells[1].Text;
                txtPickupLocation.Text = gvDispatch.SelectedRow.Cells[2].Text;
                txtPickupDate.Text = gvDispatch.SelectedRow.Cells[3].Text;
                txtPickupTime.Text = gvDispatch.SelectedRow.Cells[4].Text;
                txtDropOffLocation.Text = gvDispatch.SelectedRow.Cells[5].Text;

            }

            if (e.CommandName == "DeleteDispatch")
            {
                t.DeleteDispatchItinerary(gvDispatch.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["DispatchItineraryID"].ToString());
                ClearAllDispatch();

                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Dispatch Booking has been deleted successfully";

                if (!string.IsNullOrEmpty(Request["TANO"] as string))
                {
                    string TravelAuthorizationNo = Decrypt(Request["TANO"].ToString());

                    FillDispatchItineraryItems(TravelAuthorizationNo);
                }


            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvDispatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {

                LinkButton ibEditDispatch = new LinkButton();
                ibEditDispatch = (LinkButton)e.Row.FindControl("ibEditDispatch");

                LinkButton ibDeleteDispatch = new LinkButton();
                ibDeleteDispatch = (LinkButton)e.Row.FindControl("ibDeleteDispatch");

            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvDispatch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void gvDispatch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDispatch.EditIndex = e.NewEditIndex;
        DataTable dt = new DataTable();
        if (!string.IsNullOrEmpty(Request["TANO"] as string))
        {
            string TravelAuthorizationNo = Decrypt(Request["TANO"].ToString());

            FillDispatchItineraryItems(TravelAuthorizationNo);

            t.GetDispatchItineraryByTravelAuthorizationNumber(TravelAuthorizationNo, ref dt);
            gvDispatch.DataSource = dt;
            gvDispatch.DataBind();
        }
    }
    void FillDispatchItineraryItems(string TravelAuthorizationNo)
    {
        try
        {
            DataTable dt = new DataTable();
            t.GetDispatchItineraryByTravelAuthorizationNumber(TravelAuthorizationNo, ref dt);
            gvDispatch.DataSource = dt;
            gvDispatch.DataBind();
            if (dt.Rows.Count > 0)
            {
                checkApplicable.Checked = true;

                txtFlightReference.Enabled = true;
                txtPickupDate.Enabled = true;
                //txtCarrier.Enabled = true;
                txtETAETD.Enabled = true;
                txtPickupLocation.Enabled = true;
                txtDropOffLocation.Enabled = true;
                txtPickupTime.Enabled = true;
                txtFlightReference.Text = "";
                txtPickupDate.Text = "";
                //txtCarrier.Text = "";
                txtETAETD.Text = "";
                txtPickupLocation.Text = "";
                txtDropOffLocation.Text = "";
                txtPickupTime.Text = "";
            }
            else
            {
                checkApplicable.Checked = false;
                txtFlightReference.Enabled = false;
                txtPickupDate.Enabled = false;
                //txtCarrier.Enabled = false;
                txtETAETD.Enabled = false;
                txtPickupLocation.Enabled = false;
                txtDropOffLocation.Enabled = false;
                txtPickupTime.Enabled = false;
                txtFlightReference.Text = "";
                txtPickupDate.Text = "";
                //txtCarrier.Text = "";
                txtETAETD.Text = "";
                txtPickupLocation.Text = "";
                txtDropOffLocation.Text = "";
                txtPickupTime.Text = "";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibAddDispatch_Click(object sender, EventArgs e)
    {
        //try
        //{
        if (!string.IsNullOrEmpty(Request["TANO"] as string))
        {
            string result = "";
            if (gvDispatch.SelectedIndex == -1)
            {
                TimeSpan interval;
                if (TimeSpan.TryParse(txtPickupTime.Text, out interval) == false)
                {
                    txtPickupTime.CssClass += " invalid";
                    return;
                }
                else
                {
                    if (TimeSpan.Parse(txtPickupTime.Text).TotalHours > 24)
                    {
                        txtPickupTime.CssClass += " invalid";
                        return;
                    }
                    else
                    {
                        txtPickupTime.CssClass.Replace("invalid", "");
                        result = t.InsertUpdateDispatchItinerary("", Decrypt(Request["TANO"].ToString()), txtFlightReference.Text, TimeSpan.Parse(txtETAETD.Text),
                             txtPickupLocation.Text, Convert.ToDateTime(txtPickupDate.Text.Trim()), TimeSpan.Parse(txtPickupTime.Text), txtDropOffLocation.Text, gvDispatch.Rows.Count + 1);

                        if (result == "1")
                        {
                            FillDispatchItineraryItems(Decrypt(Request["TANO"].ToString()));
                            ClearAllDispatch();
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-success alert-dismissable";
                            lblmsg.ForeColor = Color.Green;
                            lblmsg.Text = "Dispatch booking has been added successfully";

                            // update the status back to pending if the user update/Add 
                            DataTable dt = new DataTable();
                            t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["StatusCode"].ToString() == "TADS" || dt.Rows[0]["StatusCode"].ToString() == "TADC")
                                {
                                    result = t.InsertTAStatus(Decrypt(Request["TAID"]), "PEN", "", "");
                                }
                            }
                        }
                        else
                        {
                            FillDispatchItineraryItems(Decrypt(Request["TANO"].ToString()));
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "This item is already added";
                        }

                    }
                }
            }
            else
            {

                TimeSpan interval;

                if (TimeSpan.TryParse(txtPickupTime.Text, out interval) == false)
                {
                    txtPickupTime.CssClass += " invalid";
                    return;
                }
                else
                {
                    if (TimeSpan.Parse(txtPickupTime.Text).TotalHours > 24)
                    {
                        txtPickupTime.CssClass += " invalid";
                        return;
                    }
                    else
                    {
                        txtPickupTime.CssClass.Replace("invalid", "");

                        result = t.InsertUpdateDispatchItinerary(gvDispatch.DataKeys[gvDispatch.SelectedIndex].Values["DispatchItineraryID"].ToString(),
                        Decrypt(Request["TANO"].ToString()),
                        txtFlightReference.Text,
                        TimeSpan.Parse(txtETAETD.Text),
                        txtPickupLocation.Text, 
                        Convert.ToDateTime(txtPickupDate.Text.Trim()), 
                        TimeSpan.Parse(txtPickupTime.Text), 
                        txtDropOffLocation.Text, 
                        gvTravelItinerary.SelectedIndex + 1);
                        if (result == "1")
                        {
                            FillDispatchItineraryItems(Decrypt(Request["TANO"].ToString()));
                            ClearAllDispatch();
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-success alert-dismissable";
                            lblmsg.ForeColor = Color.Green;
                            lblmsg.Text = "Dispatch booking has been added successfully";

                            // update the status back to pending if the user update/Add 
                            DataTable dt = new DataTable();
                            t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["StatusCode"].ToString() == "TADS" || dt.Rows[0]["StatusCode"].ToString() == "TADC")
                                {
                                    result = t.InsertTAStatus(Decrypt(Request["TAID"]), "PEN", "", "");
                                }
                            }
                        }
                        else
                        {
                            FillDispatchItineraryItems(Decrypt(Request["TANO"].ToString()));
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "This item 2 is already added";
                        }

                    }

                }
            }
        }

        //}
        //catch (Exception ex)
        //{

        //    IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        //}

    }
    void ClearAllDispatch()
    {
        txtFlightReference.Text = "";
        txtPickupDate.Text = "";
        //txtCarrier.Text = "";
        txtETAETD.Text = "";
        txtPickupLocation.Text = "";
        txtDropOffLocation.Text = "";
        txtPickupTime.Text = "";

        gvDispatch.SelectedIndex = -1;
        for (int i = 0; i <= gvDispatch.Rows.Count - 1; i++)
        {
            gvDispatch.Rows[i].BackColor = default(System.Drawing.Color);
            gvDispatch.Rows[i].ForeColor = default(System.Drawing.Color);
        }
    }
    void ClearAllValidation()

    {
        txtFlightReference.CssClass = "";
        txtPickupDate.CssClass = "";
        //txtCarrier.CssClass = "";
        txtETAETD.CssClass = "";
        txtPickupLocation.CssClass = "";
        txtDropOffLocation.CssClass = "";
        txtPickupTime.CssClass = "";
    }
    #endregion
    */

    #region TravelItinerary
    void FillDDLs()
    {
        try
        {
            DataSet ds = new DataSet();
            l.GetAllLookupsList(ref ds);
            ddlModeOfTravel.DataSource = ds.Tables[1];
            ddlModeOfTravel.DataBind();

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillTravelItineraryItems(string TravelAuthorizationNo)
    {
        try
        {
            DataTable dt = new DataTable();
            t.GetTravelItineraryByTravelAuthorizationNumber(TravelAuthorizationNo, ref dt);
            gvTravelItinerary.DataSource = dt;
            gvTravelItinerary.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        txtFromLocationCode.Text = "";
        txtFromLocationDate.Text = "";
        txtToLocationCode.Text = "";
        txtToLocationDate.Text = "";
        ddlModeOfTravel.SelectedIndex = -1;

        gvTravelItinerary.SelectedIndex = -1;
        for (int i = 0; i <= gvTravelItinerary.Rows.Count - 1; i++)
        {
            gvTravelItinerary.Rows[i].BackColor = default(System.Drawing.Color);
            gvTravelItinerary.Rows[i].ForeColor = default(System.Drawing.Color);
        }
    }
    protected void gvTravelItinerary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            LinkButton ibD = new LinkButton();
            LinkButton ibe = new LinkButton();
            ibe = (LinkButton)e.Row.FindControl("ibEdit");
            ibD = (LinkButton)e.Row.FindControl("ibDelete");

        }
    }
    protected void gvTravelItinerary_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditItinerary")
            {
                ClearAll();
                gvTravelItinerary.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvTravelItinerary.SelectedRow.BackColor = Color.LightGray;
                gvTravelItinerary.SelectedRow.ForeColor = Color.Black;

                txtFromLocationCode.Text = gvTravelItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["FromLocationCodeName"].ToString();
                txtFromLocationDate.Text = gvTravelItinerary.Rows[Convert.ToInt16(e.CommandArgument)].Cells[2].Text.ToString();
                txtToLocationCode.Text = gvTravelItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["ToLocationCodeName"].ToString();
                txtToLocationDate.Text = gvTravelItinerary.Rows[Convert.ToInt16(e.CommandArgument)].Cells[4].Text.ToString();
                ddlModeOfTravel.SelectedValue = gvTravelItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["ModeOfTravelID"].ToString();
            }
            if (e.CommandName == "DeleteItem")
            {
                t.DeleteTravelItinerary(gvTravelItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelItineraryID"].ToString());
                ClearAll();

                // update the status back to pending if the user update/Add 
                string result = "";
                DataTable dtStatus = new DataTable();
                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dtStatus);
                if (dtStatus.Rows.Count > 0)
                {
                    if (dtStatus.Rows[0]["StatusCode"].ToString() == "TADS" || dtStatus.Rows[0]["StatusCode"].ToString() == "TADC")
                    {
                        result = t.InsertTAStatus(Decrypt(Request["TAID"]), "PEN", "", "");
                    }
                }

                if (!string.IsNullOrEmpty(Request["TANO"] as string))
                {
                    string TravelAuthorizationNo = Decrypt(Request["TANO"].ToString());
                    DataTable dt = new DataTable();
                    t.GetTravelItineraryByTravelAuthorizationNumber(TravelAuthorizationNo, ref dt);
                    gvTravelItinerary.DataSource = dt;
                    gvTravelItinerary.DataBind();

                    if (dt.Rows.Count == 0)
                    {
                        Response.Redirect("~/TravelAuthorization/TAWizard/Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"] + "&First=1", false);
                    }
                }

                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Item has been deleted successfully";

            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvTravelItinerary_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void ibAdd_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["TANO"] as string))
        {
            if (Convert.ToDateTime(txtToLocationDate.Text.Trim() + " 2:00:00") > Convert.ToDateTime(txtFromLocationDate.Text.Trim() + " 1:00:00"))
            {
                DataTable dtFrom = new DataTable();
                l.GetCityByDescription(txtFromLocationCode.Text.Trim(), ref dtFrom);

                DataTable dtTo = new DataTable();
                l.GetCityByDescription(txtToLocationCode.Text.Trim(), ref dtTo);

                string FromLocationCodeID = "";
                string ToLocationCodeID = "";

                foreach (DataRow row in dtFrom.Rows)
                {
                    FromLocationCodeID = row["CityID"].ToString();
                }

                foreach (DataRow row in dtTo.Rows)
                {
                    ToLocationCodeID = row["CityID"].ToString();
                }

                if (dtFrom.Rows.Count > 0 && dtTo.Rows.Count > 0)
                {
                    //    lblmsg.Text = "From date can't be smaller than to date from the previos record";
                    string result = "";
                    if (gvTravelItinerary.SelectedIndex == -1)
                    {
                        DataTable dtTa = new DataTable();
                        DataTable dtDup = new DataTable();

                        t.GetTravelAuthorizationByTravelAuthorizationID(Decrypt(Request["TAID"].ToString()), ref dtTa);

                        t.CheckDuplicatedTA(Decrypt(Request["TAID"].ToString()),
                            dtTa.Rows[0]["TravelersName"].ToString(),
                            Convert.ToDateTime(txtFromLocationDate.Text.Trim()),
                            Convert.ToDateTime(txtToLocationDate.Text.Trim()),
                            FromLocationCodeID,
                            ToLocationCodeID,
                            "",
                            ref dtDup);

                        if (dtDup.Rows.Count > 0)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "You already have the same TA form with the same itinerary";
                        }
                        else
                        {

                            result = t.InsertUpdateTravelItinerary("", Decrypt(Request["TANO"].ToString()), ddlModeOfTravel.SelectedValue, FromLocationCodeID, Convert.ToDateTime(txtFromLocationDate.Text.Trim()), ToLocationCodeID, Convert.ToDateTime(txtToLocationDate.Text.Trim()), gvTravelItinerary.Rows.Count + 1);
                            if (result == "1")
                            {
                                ClearAll();
                                FillTravelItineraryItems(Decrypt(Request["TANO"].ToString()));

                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                lblmsg.ForeColor = Color.Green;
                                lblmsg.Text = "Item has been added successfully";

                                // update the status back to pending if the user update/Add 
                                DataTable dt = new DataTable();
                                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["StatusCode"].ToString() == "TADS" || dt.Rows[0]["StatusCode"].ToString() == "TADC")
                                    {
                                        result = t.InsertTAStatus(Decrypt(Request["TAID"]), "PEN", "", "");
                                    }
                                }

                            }
                            else if (result == "2")
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "From date can't be smaller than previous to date";
                            }
                            else if (result == "3")
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "To date can't be greater than next from date";
                            }
                            else
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "This item is already added";
                            }
                        }
                    }
                    else
                    {
                        DataTable dtTa = new DataTable();
                        DataTable dtDup = new DataTable();

                        t.GetTravelAuthorizationByTravelAuthorizationID(Decrypt(Request["TAID"].ToString()), ref dtTa);
                        t.CheckDuplicatedTA(Decrypt(Request["TAID"].ToString()),
                            dtTa.Rows[0]["TravelersName"].ToString(),
                            Convert.ToDateTime(txtFromLocationDate.Text.Trim()),
                            Convert.ToDateTime(txtToLocationDate.Text.Trim()),
                            FromLocationCodeID,
                            ToLocationCodeID,
                            gvTravelItinerary.DataKeys[gvTravelItinerary.SelectedIndex].Values["TravelItineraryID"].ToString(),
                            ref dtDup);

                        if (dtDup.Rows.Count > 0)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "You already have the same TA form with the same itinerary";
                        }
                        else
                        {
                            result = t.InsertUpdateTravelItinerary(gvTravelItinerary.DataKeys[gvTravelItinerary.SelectedIndex].Values["TravelItineraryID"].ToString(), Decrypt(Request["TANO"].ToString()), ddlModeOfTravel.SelectedValue, FromLocationCodeID, Convert.ToDateTime(txtFromLocationDate.Text.Trim()), ToLocationCodeID, Convert.ToDateTime(txtToLocationDate.Text.Trim()), gvTravelItinerary.SelectedIndex + 1);
                            if (result == "1")
                            {
                                ClearAll();
                                FillTravelItineraryItems(Decrypt(Request["TANO"].ToString()));

                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                lblmsg.ForeColor = Color.Green;
                                lblmsg.Text = "Item has been added successfully";

                                // update the status back to pending if the user update/Add 
                                DataTable dt = new DataTable();
                                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["StatusCode"].ToString() == "TADS" || dt.Rows[0]["StatusCode"].ToString() == "TADC")
                                    {
                                        result = t.InsertTAStatus(Decrypt(Request["TAID"]), "PEN", "", "");
                                    }
                                }
                            }
                            else if (result == "2")
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "From date can't be smaller than previous to date";
                            }
                            else if (result == "3")
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "To date can't be greater than next from date";
                            }
                            else
                            {

                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "This item is already added";
                            }
                        }
                    }
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "You have to select a location from or location to , and make sure to select the value from the list";
                    return;
                }
            }
            else
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;

                lblmsg.Text = "To date can't be smaller than from date";
                return;
            }
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["TAID"] as string) && !string.IsNullOrEmpty(Request["TANO"] as string))
        {
            if (ddlModeOfTravel.SelectedIndex != 0 || txtFromLocationCode.Text != "" || txtFromLocationDate.Text != "" || txtToLocationCode.Text != "" || txtToLocationDate.Text != "")
            {
                bool invalid = false;

                if (ddlModeOfTravel.SelectedIndex == 0)
                {
                    ddlModeOfTravel.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    ddlModeOfTravel.CssClass = "form-control";
                }
                if (txtFromLocationCode.Text == "")
                {
                    txtFromLocationCode.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtFromLocationCode.CssClass = "form-control";
                }
                if (txtFromLocationDate.Text == "")
                {
                    txtFromLocationDate.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtFromLocationDate.CssClass = "form-control";
                }
                if (txtToLocationCode.Text == "")
                {
                    txtToLocationCode.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtToLocationCode.CssClass = "form-control";
                }

                if (txtToLocationDate.Text == "")
                {
                    txtToLocationDate.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtToLocationDate.CssClass = "form-control";
                }

                if (invalid)
                    return;


                if (!string.IsNullOrEmpty(Request["TANO"] as string))
                {
                    if (Convert.ToDateTime(txtToLocationDate.Text.Trim() + " 2:00:00") > Convert.ToDateTime(txtFromLocationDate.Text.Trim() + " 1:00:00"))
                    {
                        DataTable dtFrom = new DataTable();
                        l.GetCityByDescription(txtFromLocationCode.Text.Trim(), ref dtFrom);

                        DataTable dtTo = new DataTable();
                        l.GetCityByDescription(txtToLocationCode.Text.Trim(), ref dtTo);

                        string FromLocationCodeID = "";
                        string ToLocationCodeID = "";

                        foreach (DataRow row in dtFrom.Rows)
                        {
                            FromLocationCodeID = row["CityID"].ToString();
                        }

                        foreach (DataRow row in dtTo.Rows)
                        {
                            ToLocationCodeID = row["CityID"].ToString();
                        }

                        if (dtFrom.Rows.Count > 0 && dtTo.Rows.Count > 0)
                        {
                            //    lblmsg.Text = "From date can't be smaller than to date from the previos record";
                            string result = "";
                            if (gvTravelItinerary.SelectedIndex == -1)
                            {
                                DataTable dtTa = new DataTable();
                                DataTable dtDup = new DataTable();

                                t.GetTravelAuthorizationByTravelAuthorizationID(Decrypt(Request["TAID"].ToString()), ref dtTa);
                                t.CheckDuplicatedTA(Decrypt(Request["TAID"].ToString()),
                                    dtTa.Rows[0]["TravelersName"].ToString(),
                                    Convert.ToDateTime(txtFromLocationDate.Text.Trim()),
                                    Convert.ToDateTime(txtToLocationDate.Text.Trim()),
                                    FromLocationCodeID,
                                    ToLocationCodeID,
                                    "",
                                    ref dtDup);

                                if (dtDup.Rows.Count > 0)
                                {
                                    PanelMessage.Visible = true;
                                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                    lblmsg.ForeColor = Color.Red;
                                    lblmsg.Text = "You already have the same TA form with the same itinerary";
                                }
                                else
                                {
                                    result = t.InsertUpdateTravelItinerary("", Decrypt(Request["TANO"].ToString()), ddlModeOfTravel.SelectedValue, FromLocationCodeID, Convert.ToDateTime(txtFromLocationDate.Text.Trim()), ToLocationCodeID, Convert.ToDateTime(txtToLocationDate.Text.Trim()), gvTravelItinerary.Rows.Count + 1);
                                    if (result == "1")
                                    {
                                        ClearAll();
                                        FillTravelItineraryItems(Decrypt(Request["TANO"].ToString()));

                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                        lblmsg.ForeColor = Color.Green;
                                        lblmsg.Text = "Item has been added successfully";

                                        // update the status back to pending if the user update/Add 
                                        DataTable dt = new DataTable();
                                        t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["StatusCode"].ToString() == "TADS" || dt.Rows[0]["StatusCode"].ToString() == "TADC")
                                            {
                                                result = t.InsertTAStatus(Decrypt(Request["TAID"]), "PEN", "", "");
                                            }
                                        }
                                    }
                                    else if (result == "2")
                                    {
                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                        lblmsg.ForeColor = Color.Red;
                                        lblmsg.Text = "From date can't be smaller than previous to date";
                                    }
                                    else if (result == "3")
                                    {
                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                        lblmsg.ForeColor = Color.Red;
                                        lblmsg.Text = "To date can't be greater than next from date";
                                    }
                                    else
                                    {
                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                        lblmsg.ForeColor = Color.Red;
                                        lblmsg.Text = "This item is already added";
                                    }
                                }
                            }
                            else
                            {
                                DataTable dtTa = new DataTable();
                                DataTable dtDup = new DataTable();

                                t.GetTravelAuthorizationByTravelAuthorizationID(Decrypt(Request["TAID"].ToString()), ref dtTa);
                                t.CheckDuplicatedTA(Decrypt(Request["TAID"].ToString()),
                                    dtTa.Rows[0]["TravelersName"].ToString(),
                                    Convert.ToDateTime(txtFromLocationDate.Text.Trim()),
                                    Convert.ToDateTime(txtToLocationDate.Text.Trim()),
                                    FromLocationCodeID,
                                    ToLocationCodeID,
                                    gvTravelItinerary.DataKeys[gvTravelItinerary.SelectedIndex].Values["TravelItineraryID"].ToString(),
                                    ref dtDup);

                                if (dtDup.Rows.Count > 0)
                                {
                                    PanelMessage.Visible = true;
                                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                    lblmsg.ForeColor = Color.Red;
                                    lblmsg.Text = "You already have the same TA form with the same itinerary";
                                }
                                else
                                {
                                    result = t.InsertUpdateTravelItinerary(gvTravelItinerary.DataKeys[gvTravelItinerary.SelectedIndex].Values["TravelItineraryID"].ToString(), Decrypt(Request["TANO"].ToString()), ddlModeOfTravel.SelectedValue, FromLocationCodeID, Convert.ToDateTime(txtFromLocationDate.Text.Trim()), ToLocationCodeID, Convert.ToDateTime(txtToLocationDate.Text.Trim()), gvTravelItinerary.Rows.Count + 1);
                                    if (result == "1")
                                    {
                                        ClearAll();
                                        FillTravelItineraryItems(Decrypt(Request["TANO"].ToString()));

                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                        lblmsg.ForeColor = Color.Green;
                                        lblmsg.Text = "Item has been added successfully";

                                        // update the status back to pending if the user update/Add 
                                        DataTable dt = new DataTable();
                                        t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["StatusCode"].ToString() == "TADS" || dt.Rows[0]["StatusCode"].ToString() == "TADC")
                                            {
                                                result = t.InsertTAStatus(Decrypt(Request["TAID"]), "PEN", "", "");
                                            }
                                        }
                                    }
                                    else if (result == "2")
                                    {
                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                        lblmsg.ForeColor = Color.Red;
                                        lblmsg.Text = "From date can't be smaller than previous to date";
                                    }
                                    else if (result == "3")
                                    {
                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                        lblmsg.ForeColor = Color.Red;
                                        lblmsg.Text = "To date can't be greater than next from date";
                                    }
                                    else
                                    {

                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                        lblmsg.ForeColor = Color.Red;
                                        lblmsg.Text = "This item is already added";
                                    }
                                }
                            }
                        }
                        else
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "You have to select a location from or location to , and make sure to select the value from the list";
                        }
                    }
                    else
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "To date can't be smaller than from date";
                    }
                }
            }
            else
            {
                if (gvTravelItinerary.Rows.Count > 0)
                {
                    Response.Redirect("~/TravelAuthorization/TAWizard/DownloadTA.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please add itinerary before you continue to the next step.";
                }

            }

        }
    }
    #endregion

}