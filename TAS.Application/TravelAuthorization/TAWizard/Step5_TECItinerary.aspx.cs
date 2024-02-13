using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
public partial class TravelAuthorization_TAWizard_Step5_TECItinerary : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.Security s = new Business.Security();
    Business.Media Media = new Business.Media();
    Business.TravelExpenseClaim tec = new Business.TravelExpenseClaim();

    protected void Page_Load(object sender, EventArgs e)
    {
        txtExchangeRate.Enabled = this.CanAmend;

        if (!IsPostBack)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl TAStatus = null;
            TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
            TAStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep5");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            if (!string.IsNullOrEmpty(Request["TANO"] as string))
            {
                hfAmountTotal.Value = "0";
                hfDSARateTotal.Value = "0";
                hfNoOfDaysTotal.Value = "0";
                hfRateAmountTotal.Value = "0";
                txtExchangeRate.Text = ConfigurationManager.AppSettings["ExchangeRate"];
                FillGrids();

                //lock content
                Objects.User ui = (Objects.User)Session["userinfo"];
                DataTable dt = new DataTable();
                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                if (dt.Rows.Count > 0)
                {
                    //if (Convert.ToBoolean(dt.Rows[0]["IsEditable"].ToString()) == false)
                    //{
                    //    DataTable dtRoleName = new DataTable();
                    //    s.GetRoleNameByUserID(ref dtRoleName);
                    //    if (dtRoleName.Rows.Count > 0)
                    //    {
                    //        if (dtRoleName.Rows[0]["RoleName"].ToString() == "Finance")
                    //        {
                    //            pnlContent.Enabled = true;
                    //            btnSaveItinerary.Enabled = false;
                    //            btnSaveItineraryDSA.Enabled = false;
                    //        }
                    //        else
                    //        {
                    //            pnlContent.Enabled = false;
                    //        }
                    //    }

                    //    if (Convert.ToBoolean(dt.Rows[0]["InitializeTEC"].ToString()) == true)
                    //    {
                    //        pnlContent.Enabled = true;
                    //        btnSaveItinerary.Enabled = true;
                    //    }
                    //    else
                    //    {

                    //    }

                    //}

                    if (dt.Rows[0]["StatusCode"].ToString() == "SET" || dt.Rows[0]["StatusCode"].ToString() == "TECCom")
                    {
                        //open the DSA breakdown to financy (read only)
                        DataTable dtRoleName = new DataTable();
                        s.GetRoleNameByUserID(ref dtRoleName);
                        if (dtRoleName.Rows.Count > 0)
                        {
                            if (dtRoleName.Rows[0]["RoleName"].ToString() == "Finance")
                            {
                                pnlContent.Enabled = true;
                                btnSaveItinerary.Enabled = false;
                                btnSaveItineraryDSA.Enabled = false;
                            }
                            else
                            {
                                pnlContent.Enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }

    #region TEC

    public void FillGrids()
    {
        try
        {
            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
            DataTable dt = new DataTable();
            t.GetTravelAuthorizationByTravelAuthorizationNumber(TravelAuthorizationNumber, ref dt);

            string FirstName = "";
            string LastName = "";
            //string TAUserID = "";
            string Status = "";

            foreach (DataRow row in dt.Rows)
            {
                FirstName = row["TravelerFirstName"].ToString();
                LastName = row["TravelerLastName"].ToString();
                Status = row["StatusCode"].ToString();

                hdnUserID.Value = row["UserID"].ToString();
                hdnStatusCode.Value = row["StatusCode"].ToString();
                hdnTravelSchema.Value = row["TravelSchema"].ToString();
            }


            lblNameOfClaimant.Text = "&nbsp;" + FirstName + ", " + LastName + "&nbsp;";

            DataTable dtItinerary = new DataTable();
            tec.GetTECItineraryByTravelAuthorizationNumber(ref dtItinerary, TravelAuthorizationNumber);

            GVItinerary.DataSource = dtItinerary;
            GVItinerary.DataBind();

            GVItinerary.Columns[7].Visible = this.CanAmend;

            if (dtItinerary.Rows[0]["ExchangeRate"].ToString() != "0")
            {
                txtExchangeRate.Text = dtItinerary.Rows[0]["ExchangeRate"].ToString();
            }

            if (Status.ToUpper() == "COM")
            {
                btnSaveItineraryDSA.Visible = false;
                btnSaveItinerary.Visible = false;
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void GVItineraryCalculation()
    {
        try
        {
            decimal Rowtot = 0;
            decimal Rowtotam = 0;
            decimal Rowtotaml = 0;
            foreach (GridViewRow r in GVItinerary.Rows)
            {
                TextBox txtNoOfDays = (TextBox)r.FindControl("txtNoOfDays");
                TextBox txtAmount = (TextBox)r.FindControl("txtAmount");
                TextBox txtAmountLocal = (TextBox)r.FindControl("txtAmountLocal");

                if (txtNoOfDays.Text != "")
                {
                    Rowtot = Rowtot + Convert.ToDecimal(txtNoOfDays.Text);
                }

                if (txtAmount.Text != "")
                {
                    Rowtotam = Rowtotam + Convert.ToDecimal(txtAmount.Text);
                }

                if (txtAmountLocal.Text != "")
                {
                    Rowtotaml = Rowtotaml + Convert.ToDecimal(txtAmountLocal.Text);
                }
            }
            hfNoOfDaysTotal.Value = Rowtot.ToString();
            hfAmountTotal.Value = Rowtotam.ToString();
            hfRateAmountTotal.Value = Rowtotaml.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void GVItinerary_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "<b>Itinerary Details <i style='color:red'> (All times are mandatory)</i></b>";
                HeaderCell.ColumnSpan = 6;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderCell.ColumnSpan = 1;
                HeaderCell.BackColor = Color.FromArgb(238, 238, 238);
                HeaderCell.BorderColor = Color.FromArgb(238, 238, 238);
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "<b>Daily Subsistence Allowance (DSA) Breakdown <i style='color:red'> (For accounting purposes only)</i></b>";
                HeaderCell.ColumnSpan = 5;
                HeaderGridRow.Cells.Add(HeaderCell);


                GVItinerary.Controls[0].Controls.AddAt(0, HeaderGridRow);


                e.Row.Cells[0].ColumnSpan = 2;
                //now make up for the colspan from cell2
                e.Row.Cells.RemoveAt(1);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GVItineraryCalculation();
                e.Row.Cells[0].ColumnSpan = 5;
                //now make up for the colspan from cell2
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(1);
                Label lblAmount, lblNoOfDays, lblAmountLocalTotal;
                lblAmount = (Label)e.Row.FindControl("lblAmountTotal");
                lblNoOfDays = (Label)e.Row.FindControl("lblNoOfDaysTotal");
                lblAmountLocalTotal = (Label)e.Row.FindControl("lblAmountLocalTotal");
                lblAmount.Text = hfAmountTotal.Value;
                lblNoOfDays.Text = hfNoOfDaysTotal.Value;
                lblAmountLocalTotal.Text = hfRateAmountTotal.Value;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVItinerary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtAmount, txtNoOfDays, txtDSARate, txtAmountLocal, txtNoOfKms, txtFromLocationTime;
                LinkButton ibDSABreakdown;
                ibDSABreakdown = (LinkButton)e.Row.FindControl("ibDSABreakdown");

                txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                txtNoOfDays = (TextBox)e.Row.FindControl("txtNoOfDays");
                txtDSARate = (TextBox)e.Row.FindControl("txtDSARate");
                txtAmountLocal = (TextBox)e.Row.FindControl("txtAmountLocal");
                txtNoOfKms = (TextBox)e.Row.FindControl("txtNoOfKms");
                txtFromLocationTime = (TextBox)e.Row.FindControl("txtFromLocationTime");



                ibDSABreakdown.Visible = this.CanEdit;
                if (e.Row.Cells[0].Text.Trim() == "Dep.")
                {
                    txtAmount.Text = "";
                    txtNoOfDays.Text = "";
                    txtDSARate.Text = "";
                    txtAmountLocal.Text = "";
                    txtNoOfKms.Text = "";
                    txtAmount.Visible = false;
                    txtNoOfDays.Visible = false;
                    txtDSARate.Visible = false;
                    txtAmountLocal.Visible = false;
                    txtNoOfKms.Visible = false;
                    ibDSABreakdown.Visible = false;
                }
                else
                {
                    tec.InsertEmptyTECItineraryDSA(GVItinerary.DataKeys[Convert.ToInt16(e.Row.RowIndex)].Values["TravelItineraryID"].ToString());
                }
                txtAmount.Enabled = this.CanAmend;
                txtNoOfDays.Enabled = this.CanAmend;
                txtDSARate.Enabled = this.CanAmend;
                txtAmountLocal.Enabled = this.CanAmend;

                Objects.User ui = (Objects.User)Session["userinfo"];
                if (ViewState["TAUserID"].ToString() == ui.User_Id.ToString())
                {
                    txtFromLocationTime.Enabled = this.CanEdit;
                    txtNoOfKms.Enabled = this.CanEdit;
                }
                else
                {
                    txtFromLocationTime.Enabled = this.CanEdit;
                    txtNoOfKms.Enabled = this.CanEdit;
                }
                if (hdnStatusCode.Value.ToString().ToUpper() == "COM")
                {
                    txtAmount.Enabled = false;
                    txtNoOfDays.Enabled = false;
                    txtDSARate.Enabled = false;
                    txtAmountLocal.Enabled = false;
                    txtFromLocationTime.Enabled = false;
                    txtNoOfKms.Enabled = false;
                }
                else if (hdnStatusCode.Value.ToString().ToUpper() == "SET")
                {
                    txtFromLocationTime.Enabled = false;
                    txtNoOfKms.Enabled = false;
                    ibDSABreakdown.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVItinerary_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                lblArr.Text = GVItinerary.Rows[Convert.ToInt16(e.CommandArgument)].Cells[1].Text;
                lblDate.Text = GVItinerary.Rows[Convert.ToInt16(e.CommandArgument)].Cells[2].Text;
                DataTable dt = new DataTable();
                tec.InsertEmptyTECItineraryDSA(GVItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelItineraryID"].ToString());
                Divrate.Visible = this.CanAmend;
                DSARateid.Visible = this.CanAmend;
                hfDSAID.Value = GVItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TECItineraryID"].ToString();
                hfDSATAID.Value = GVItinerary.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelItineraryID"].ToString();
                FillDSAGrid();
                FillGrids();
                ClearDSA();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnSaveItinerary_Click(object sender, EventArgs e)
    {
        try
        {
            PanelItenMessage.Visible = false;
            lblItenmsg.Text = "";

            for (int i = 0; i <= GVItinerary.Rows.Count - 1; i++)
            {
                TextBox txtTime = new TextBox();
                TextBox txtNoOfKms = new TextBox();
                float NoOfKms;
                TimeSpan interval;
                txtTime = (TextBox)GVItinerary.Rows[i].FindControl("txtFromLocationTime");
                txtNoOfKms = (TextBox)GVItinerary.Rows[i].FindControl("txtNoOfKms");
                if (float.TryParse(txtNoOfKms.Text, out NoOfKms) == false)
                {
                    if (txtNoOfKms.Text != "")
                    {
                        NoOfKms = 0;
                        txtNoOfKms.CssClass += " invalid";
                        return;
                    }
                    else
                    {
                        NoOfKms = 0;
                    }
                }

                if (TimeSpan.TryParse(txtTime.Text, out interval) == false)
                {
                    txtTime.CssClass += " invalid";
                    return;
                }
                else
                {
                    if (TimeSpan.Parse(txtTime.Text).TotalHours > 24)
                    {
                        txtTime.CssClass += " invalid";
                        return;
                    }
                    else
                    {
                        txtTime.CssClass.Replace("invalid", "");
                        tec.UpdateTravelIteneraryTime(GVItinerary.DataKeys[i].Values["TravelItineraryID"].ToString(), TimeSpan.Parse(txtTime.Text), GVItinerary.Rows[i].Cells[0].Text);
                    }
                }
                if (txtNoOfKms.Visible)
                {
                    tec.UpdateTECItineraryNoOfKms(GVItinerary.DataKeys[i].Values["TravelItineraryID"].ToString(), NoOfKms);
                }
            }

            DataTable dtTecCheck = new DataTable();
            if (!string.IsNullOrEmpty(Request["TANO"] as string))
            {
                tec.CheckTravelIteneraryTime(ref dtTecCheck, Decrypt(Request["TANO"]));
            }

            Response.Redirect("~/TravelAuthorization/TAWizard/Step6_TECExpenses.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnSaveItineraryDSA_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i <= GVItinerary.Rows.Count - 1; i++)
            {
                TextBox txtNoOfDays, txtDSARate, txtAmount, txtAmountLocal;
                float NoOfDays, DSARate, Amount, AmounLocal;
                txtNoOfDays = (TextBox)GVItinerary.Rows[i].FindControl("txtNoOfDays");
                txtDSARate = (TextBox)GVItinerary.Rows[i].FindControl("txtDSARate");
                txtAmount = (TextBox)GVItinerary.Rows[i].FindControl("txtAmount");
                txtAmountLocal = (TextBox)GVItinerary.Rows[i].FindControl("txtAmountLocal");
                if (float.TryParse(txtNoOfDays.Text, out NoOfDays) == false)
                    NoOfDays = 0;
                if (float.TryParse(txtDSARate.Text, out DSARate) == false)
                    DSARate = 0;
                if (float.TryParse(txtAmount.Text, out Amount) == false)
                    Amount = 0;
                if (float.TryParse(txtAmountLocal.Text, out AmounLocal) == false)
                    AmounLocal = 0;
                if (txtNoOfDays.Visible)
                    tec.UpdateTECItineraryDSA(GVItinerary.DataKeys[i].Values["TravelItineraryID"].ToString(), NoOfDays, DSARate, Amount, AmounLocal);
            }
            FillGrids();
            PanelItenMessage.Visible = true;
            lblItenmsg.Text = "Travel DSA has been saved successfully";
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    #endregion

    #region DSABreakdown

    void FillDSAGrid()
    {
        try
        {
            DataTable dtDSA = new DataTable();
            tec.GetTECItineraryDSAByTECItineraryID(ref dtDSA, hfDSATAID.Value.ToString());
            gvDSABreakdown.DataSource = dtDSA;
            gvDSABreakdown.DataBind();
            gvDSABreakdown.Columns[5].Visible = this.CanAmend;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearDSA()
    {
        try
        {
            pnlDSAMsg.Visible = false;
            txtDSARateDSA.Text = "";
            txtNoOfDaysDSA.Text = "";
            txtPercentageDSA.Text = "";
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void gvDSABreakdown_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ibDelete = (LinkButton)e.Row.FindControl("ibDelete");
                e.Row.Cells[0].Text = decimal.Round(Convert.ToDecimal(e.Row.Cells[0].Text), 2, MidpointRounding.AwayFromZero).ToString();
                e.Row.Cells[1].Text = decimal.Round(Convert.ToDecimal(e.Row.Cells[1].Text), 2, MidpointRounding.AwayFromZero).ToString();
                e.Row.Cells[2].Text = decimal.Round(Convert.ToDecimal(e.Row.Cells[2].Text), 2, MidpointRounding.AwayFromZero).ToString();
                e.Row.Cells[3].Text = decimal.Round(Convert.ToDecimal(e.Row.Cells[3].Text), 2, MidpointRounding.AwayFromZero).ToString();
                e.Row.Cells[4].Text = decimal.Round(Convert.ToDecimal(e.Row.Cells[4].Text), 2, MidpointRounding.AwayFromZero).ToString();

                if (hdnStatusCode.Value.ToString().ToUpper() == "COM")
                {
                    ibDelete.Visible = false;
                }
                else
                {
                    ibDelete.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvDSABreakdown_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDSABreakdown.PageIndex = e.NewPageIndex;
            FillDSAGrid();
            FillGrids();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }

    }
    protected void gvDSABreakdown_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                tec.DeleteTECItineraryDSA(gvDSABreakdown.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TECItineraryDSAID"].ToString(), gvDSABreakdown.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TECItineraryID"].ToString());
                FillDSAGrid();
                FillGrids();
                ClearDSA();
                pnlDSAMsg.Visible = true;
                lblDSAMsg.Text = "item has been deleted successfully";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvDSABreakdown_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void ibDSABreakdown_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            FillGrids();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDSARateDSA.Text=="" || txtExchangeRate.Text=="" || txtNoOfDaysDSA.Text=="")
            {
                pnlDSAMsg.Visible = true;
                pnlDSAMsg.CssClass = "alert alert-danger alert-dismissable";
                lblDSAMsg.Text = "Please fill all required fields";
            }
            else if (Convert.ToInt32(txtPercentageDSA.Text) > 100)
            {
                pnlDSAMsg.Visible = true;
                pnlDSAMsg.CssClass = "alert alert-danger alert-dismissable";
                lblDSAMsg.Text = "Percentage should be equal or less than 100";
            }
            else
            {
                float RateAmount, LocalAmount;
                RateAmount = (float)Convert.ToDouble(txtDSARateDSA.Text) * (float)(Convert.ToDouble(txtPercentageDSA.Text) / 100) * (float)Convert.ToDouble(txtNoOfDaysDSA.Text);
                LocalAmount = (float)Convert.ToDecimal(txtDSARateDSA.Text) * (float)(Convert.ToDecimal(txtPercentageDSA.Text) / 100) * (float)Convert.ToDecimal(txtNoOfDaysDSA.Text) * (float)Convert.ToDecimal(txtExchangeRate.Text);
                tec.InsertUpdateTECItineraryDSA("00000000-0000-0000-0000-000000000000", hfDSAID.Value.ToString(), (float)Convert.ToDecimal(txtNoOfDaysDSA.Text), (float)Convert.ToDecimal(txtDSARateDSA.Text), (float)Convert.ToDecimal(txtPercentageDSA.Text), RateAmount, (float)Convert.ToDecimal(txtExchangeRate.Text), LocalAmount);
                FillDSAGrid();
                FillGrids();
                ClearDSA();
                pnlDSAMsg.Visible = true;
                pnlDSAMsg.CssClass = "alert alert-success alert-dismissable";
                lblDSAMsg.Text = "item has been added successfully";
            }


        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    #endregion
}