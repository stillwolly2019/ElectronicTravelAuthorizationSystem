
    using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RadioCheck_NOAWizard_3_AbsenceDetails : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    protected void Page_Load(object sender, EventArgs e)
    {
        Objects.User ui = (Objects.User)Session["userinfo"];
        if (!IsPostBack)
        {
            Calendar1.StartDate = DateTime.Now;
            Calendar2.StartDate = DateTime.Now;
            System.Web.UI.HtmlControls.HtmlGenericControl MRStatus = null;
            MRStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("NOAStatusDiv");
            MRStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep3");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            FillDDLs();
            if (!string.IsNullOrEmpty(Request["MRID"] as string))
            {
                string MovementRequestID = Decrypt(Request["MRID"].ToString());
                FillLeaveItineraryItems(MovementRequestID);

                // lock the form only if the status is in the TEC
                DataTable dt = new DataTable();
                R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Code"].ToString() == "NP" || dt.Rows[0]["Code"].ToString() == "NR" && ui.User_Id == dt.Rows[0]["RequesterID"].ToString())
                    {
                        pnlContent.Enabled = true;
                    }
                    else
                    {
                        pnlContent.Enabled = false;
                    }

                    //if (dt.Rows[0]["Code"].ToString() == "NA" || dt.Rows[0]["Code"].ToString() == "NC")
                    //{
                    //}
                }

            }
        }
    }

    #region LeaveItinerary

    void FillDDLs()
    {
        try
        {
            DataSet ds = new DataSet();
            R.GetAllLookupsList(ref ds);
            ddlLeaveCategory.DataSource = ds.Tables[9];
            ddlLeaveCategory.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillLeaveItineraryItems(string MovementRequestID)
    {
        try
        {
            DataTable dt = new DataTable();
            R.GetLeaveItineraryByLeaveRequestID(MovementRequestID, ref dt);
            gvLeaveItinerary.DataSource = dt;
            gvLeaveItinerary.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        ddlLeaveCategory.SelectedIndex = -1;

        gvLeaveItinerary.SelectedIndex = -1;
        for (int i = 0; i <= gvLeaveItinerary.Rows.Count - 1; i++)
        {
            gvLeaveItinerary.Rows[i].BackColor = default(System.Drawing.Color);
            gvLeaveItinerary.Rows[i].ForeColor = default(System.Drawing.Color);
        }
    }
    protected void gvLeaveItinerary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            LinkButton ibA = new LinkButton();
            LinkButton ibD = new LinkButton();
            LinkButton ibe = new LinkButton();
            ibA = (LinkButton)e.Row.FindControl("ibAdd");
            ibe = (LinkButton)e.Row.FindControl("ibEdit");
            ibD = (LinkButton)e.Row.FindControl("ibDelete");
        }
    }
    protected void gvLeaveItinerary_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditItinerary")
            {
                ClearAll();
                gvLeaveItinerary.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvLeaveItinerary.SelectedRow.BackColor = Color.LightGray;
                gvLeaveItinerary.SelectedRow.ForeColor = Color.Black;

                txtDateFrom.Text = gvLeaveItinerary.Rows[Convert.ToInt16(e.CommandArgument)].Cells[1].Text.ToString();
                txtDateTo.Text = gvLeaveItinerary.Rows[Convert.ToInt16(e.CommandArgument)].Cells[2].Text.ToString();
                ddlLeaveCategory.SelectedValue = gvLeaveItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["LeaveCategoryCode"].ToString();
            }
            if (e.CommandName == "DeleteItem")
            {
                R.DeleteLeaveItinery(gvLeaveItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["MovementItineraryID"].ToString());
                ClearAll();

                // update the status back to pending if the user update/Add 
                string result = "";
                DataTable dtStatus = new DataTable();
                R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dtStatus);
                if (dtStatus.Rows.Count > 0)
                {
                    if (dtStatus.Rows[0]["Code"].ToString() == "NS")
                    {
                        result = R.InsertMRStatus(Decrypt(Request["MRID"]), "NS", "", "");
                    }
                }

                if (!string.IsNullOrEmpty(Request["MRID"] as string))
                {
                    string MovementRequestID = Decrypt(Request["MRID"].ToString());
                    DataTable dt = new DataTable();
                    R.GetLeaveItineraryByLeaveRequestID(MovementRequestID, ref dt);
                    gvLeaveItinerary.DataSource = dt;
                    gvLeaveItinerary.DataBind();

                    if (dt.Rows.Count == 0)
                    {
                        Response.Redirect("~/RadioCheck/NOAWizard/4_DownloadNOA.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"] + "&First=1", false);
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
    protected void gvLeaveItinerary_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void ibAdd_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["MRID"] as string))
        {
            if (Convert.ToDateTime(txtDateTo.Text.Trim() + " 2:00:00") > Convert.ToDateTime(txtDateFrom.Text.Trim() + " 1:00:00"))
            {
                DataTable dtLeaveCategory = new DataTable();
                R.GetLeaveCategoryDescription(ddlLeaveCategory.Text.Trim(), ref dtLeaveCategory);
                string LeaveCategoryCode = "";
                foreach (DataRow row in dtLeaveCategory.Rows)
                {
                    LeaveCategoryCode = row["LookupsID"].ToString();
                }

                if (dtLeaveCategory.Rows.Count > 0)
                {
                    string result = "";
                    if (gvLeaveItinerary.SelectedIndex == -1)
                    {
                        DataTable dtTa = new DataTable();
                        DataTable dtDup = new DataTable();
                        R.GetMovementRequestByMovementRequestID(Decrypt(Request["MRID"].ToString()), ref dtTa);
                        R.CheckDuplicatedMR(Decrypt(Request["MRID"].ToString()),
                            dtTa.Rows[0]["TravelersName"].ToString(),
                            Convert.ToDateTime(txtDateFrom.Text.Trim()),
                            Convert.ToDateTime(txtDateTo.Text.Trim()),
                            LeaveCategoryCode,
                            "",
                            ref dtDup);

                        if (dtDup.Rows.Count > 0)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "You already have a Notification with same itinery";
                        }
                        else
                        {
                            if (R.IsValidDateRange(Decrypt(Request["MRID"].ToString()), Convert.ToDateTime(txtDateFrom.Text.Trim()), Convert.ToDateTime(txtDateTo.Text.Trim())) == "No")
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "Invalid date range. Specified dates must be within those specified in Step 1";
                            }
                            else
                            { 

                            result = R.InsertUpdateLeaveItinery("", Decrypt(Request["MRID"].ToString()), ddlLeaveCategory.SelectedValue, Convert.ToDateTime(txtDateFrom.Text.Trim()), Convert.ToDateTime(txtDateTo.Text.Trim()), gvLeaveItinerary.Rows.Count + 1);
                            if (result == "1")
                            {
                                ClearAll();
                                FillLeaveItineraryItems(Decrypt(Request["MRID"].ToString()));

                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                lblmsg.ForeColor = Color.Green;
                                lblmsg.Text = "Item has been added successfully";

                                // update the status back to pending if the user update/Add 
                                DataTable dt = new DataTable();
                                R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dt);
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["Code"].ToString() == "NS" || dt.Rows[0]["Code"].ToString() == "NA")
                                    {
                                        result = R.InsertMRStatus(Decrypt(Request["MRID"]), "NS", "", "");
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
                        DataTable dtTa = new DataTable();
                        DataTable dtDup = new DataTable();

                        R.GetMovementRequestByMovementRequestID(Decrypt(Request["MRID"].ToString()), ref dtTa);
                        R.CheckDuplicatedMR(Decrypt(Request["MRID"].ToString()),
                            dtTa.Rows[0]["TravelersName"].ToString(),
                            Convert.ToDateTime(txtDateFrom.Text.Trim()),
                            Convert.ToDateTime(txtDateTo.Text.Trim()),
                            ddlLeaveCategory.SelectedValue,
                            gvLeaveItinerary.DataKeys[gvLeaveItinerary.SelectedIndex].Values["MovementItineraryID"].ToString(),
                            ref dtDup);

                        if (dtDup.Rows.Count > 0)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "You already have the same MR form with the same itinerary";
                        }
                        else
                        {
                            if (R.IsValidDateRange(Decrypt(Request["MRID"].ToString()), Convert.ToDateTime(txtDateFrom.Text.Trim()), Convert.ToDateTime(txtDateTo.Text.Trim())) == "No")
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "Invalid date range. Specified dates must be within those specified in Step 1";
                            }
                            else
                            { 
                                result = R.InsertUpdateLeaveItinery(gvLeaveItinerary.DataKeys[gvLeaveItinerary.SelectedIndex].Values["MovementItineraryID"].ToString(), Decrypt(Request["MRID"].ToString()), ddlLeaveCategory.SelectedValue, Convert.ToDateTime(txtDateFrom.Text.Trim()), Convert.ToDateTime(txtDateTo.Text.Trim()), gvLeaveItinerary.SelectedIndex + 1);
                            if (result == "1")
                            {
                                ClearAll();
                                FillLeaveItineraryItems(Decrypt(Request["MRID"].ToString()));

                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                lblmsg.ForeColor = Color.Green;
                                lblmsg.Text = "Item has been added successfully";

                                // update the status back to pending if the user update/Add 
                                DataTable dt = new DataTable();
                                R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dt);
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["Code"].ToString() == "NS" || dt.Rows[0]["Code"].ToString() == "NA")
                                    {
                                        result = R.InsertMRStatus(Decrypt(Request["MRID"]), "NS", "", "");
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
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "You have to select a Leave Category from the list";
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
        if (!string.IsNullOrEmpty(Request["MRID"] as string) && !string.IsNullOrEmpty(Request["MRNO"] as string))
        {
            if (ddlLeaveCategory.SelectedIndex != 0 || txtDateFrom.Text != "" || txtDateTo.Text != "")
            {
                bool invalid = false;
                if (ddlLeaveCategory.SelectedIndex == 0)
                {
                    ddlLeaveCategory.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    ddlLeaveCategory.CssClass = "form-control";
                }

                if (txtDateFrom.Text == "")
                {
                    txtDateFrom.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtDateFrom.CssClass = "form-control";
                }

                if (txtDateTo.Text == "")
                {
                    txtDateTo.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtDateTo.CssClass = "form-control";
                }

                if (invalid)
                    return;


                if (!string.IsNullOrEmpty(Request["MRID"] as string))
                {
                    if (Convert.ToDateTime(txtDateTo.Text.Trim() + " 2:00:00") > Convert.ToDateTime(txtDateFrom.Text.Trim() + " 1:00:00"))
                    {
                        DataTable dtLm = new DataTable();
                        R.GetLeaveCategoryDescription(ddlLeaveCategory.Text.Trim(), ref dtLm);

                        string LeaveCategoryCode = "";

                        foreach (DataRow row in dtLm.Rows)
                        {
                            LeaveCategoryCode = row["LeaveCategoryCode"].ToString();
                        }



                        if (dtLm.Rows.Count > 0)
                        {
                            //    lblmsg.Text = "From date can't be smaller than to date from the previos record";
                            string result = "";
                            if (gvLeaveItinerary.SelectedIndex == -1)
                            {
                                DataTable dtTa = new DataTable();
                                DataTable dtDup = new DataTable();

                                R.GetMovementRequestByMovementRequestID(Decrypt(Request["MRID"].ToString()), ref dtTa);
                                R.CheckDuplicatedMR(Decrypt(Request["MRID"].ToString()),
                                    dtTa.Rows[0]["TravelersName"].ToString(),
                                    Convert.ToDateTime(txtDateFrom.Text.Trim()),
                                    Convert.ToDateTime(txtDateTo.Text.Trim()),
                                    ddlLeaveCategory.SelectedValue,
                                    "",
                                    ref dtDup);

                                if (dtDup.Rows.Count > 0)
                                {
                                    PanelMessage.Visible = true;
                                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                    lblmsg.ForeColor = Color.Red;
                                    lblmsg.Text = "You already have the same MR form with the same itinery";
                                }
                                else
                                {
                                    result = R.InsertUpdateLeaveItinery("", Decrypt(Request["MRID"].ToString()), ddlLeaveCategory.SelectedValue, Convert.ToDateTime(txtDateFrom.Text.Trim()), Convert.ToDateTime(txtDateTo.Text.Trim()), gvLeaveItinerary.Rows.Count + 1);
                                    if (result == "1")
                                    {
                                        ClearAll();
                                        FillLeaveItineraryItems(Decrypt(Request["MRID"].ToString()));

                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                        lblmsg.ForeColor = Color.Green;
                                        lblmsg.Text = "Item has been added successfully";

                                        // update the status back to pending if the user update/Add 
                                        DataTable dt = new DataTable();
                                        R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["StatusCode"].ToString() == "NS" || dt.Rows[0]["Code"].ToString() == "NA")
                                            {
                                                result = R.InsertMRStatus(Decrypt(Request["MRID"]), "NS", "", "");
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

                                R.GetMovementRequestByMovementRequestID(Decrypt(Request["MRID"].ToString()), ref dtTa);
                                R.CheckDuplicatedMR(Decrypt(Request["MRID"].ToString()),
                                dtTa.Rows[0]["TravelersName"].ToString(),
                                Convert.ToDateTime(txtDateFrom.Text.Trim()),
                                Convert.ToDateTime(txtDateTo.Text.Trim()),
                                ddlLeaveCategory.SelectedValue,
                                gvLeaveItinerary.DataKeys[gvLeaveItinerary.SelectedIndex].Values["LeaveItineraryID"].ToString(),
                                ref dtDup);

                                if (dtDup.Rows.Count > 0)
                                {
                                    PanelMessage.Visible = true;
                                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                    lblmsg.ForeColor = Color.Red;
                                    lblmsg.Text = "You already have the same MR form with the same itinerary";
                                }
                                else
                                {
                                    result = R.InsertUpdateLeaveItinery(gvLeaveItinerary.DataKeys[gvLeaveItinerary.SelectedIndex].Values["LeaveItineraryID"].ToString(), Decrypt(Request["MRID"].ToString()), ddlLeaveCategory.SelectedValue, Convert.ToDateTime(txtDateFrom.Text.Trim()), Convert.ToDateTime(txtDateTo.Text.Trim()), gvLeaveItinerary.Rows.Count + 1);
                                    if (result == "1")
                                    {
                                        ClearAll();
                                        FillLeaveItineraryItems(Decrypt(Request["MRID"].ToString()));

                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                        lblmsg.ForeColor = Color.Green;
                                        lblmsg.Text = "Item has been added successfully";

                                        // update the status back to pending if the user update/Add 
                                        DataTable dt = new DataTable();
                                        R.GetMRStatusByMRID(Decrypt(Request["MRID"]), ref dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["Code"].ToString() == "NS" || dt.Rows[0]["Code"].ToString() == "NA")
                                            {
                                                result = R.InsertMRStatus(Decrypt(Request["MRID"]), "NS", "", "");
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
                if (gvLeaveItinerary.Rows.Count > 0)
                {
                    Response.Redirect("~/RadioCheck/NOAWizard/4_DownloadNOA.aspx?MRID=" + Request["MRID"].ToString() + "&&MRNO=" + Request["MRNO"].ToString() + "&First=1", false);
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