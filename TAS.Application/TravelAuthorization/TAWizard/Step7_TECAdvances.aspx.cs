using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class TravelAuthorization_TAWizard_Step7_TECAdvances : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.TravelExpenseClaim tec = new Business.TravelExpenseClaim();
    Globals g = new Globals();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl TAStatus = null;
            TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
            TAStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep7");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            FillDDLs();
            FillGrids();
            //lock content
            DataTable dt = new DataTable();
            t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
            if (dt.Rows.Count > 0)
            {
                //if (dt.Rows[0]["StatusCode"].ToString() == "SET" || dt.Rows[0]["StatusCode"].ToString() == "TECCom")
                //{
                //    pnlContent.Enabled = false;
                //}
                if (Convert.ToBoolean(dt.Rows[0]["IsEditable"].ToString()) == false)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["InitializeTEC"].ToString()) == true)
                    {
                        pnlContent.Enabled = true;
                    }
                    else
                    {
                        pnlContent.Enabled = false;
                    }
                }
            }
        }
    }

    #region Advances

    protected void FillDDLs()
    {
        DataSet ds = new DataSet();
        l.GetAllLookupsList(ref ds);
        DDLAdvCurrency.DataSource = ds.Tables[7];
        DDLAdvCurrency.DataBind();

        ddlAdvPayOffice.DataSource = ds.Tables[9];
        ddlAdvPayOffice.DataBind();
    }
    public void FillGrids()
    {
        try
        {
            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
            string TravelAuthorizationID = Decrypt(Request["TAID"]);

            DataTable dt = new DataTable();
            tec.GetTECAdvancesByTravelAuthorizationNumber(ref dt, TravelAuthorizationNumber);

            GVAdvances.DataSource = dt;
            GVAdvances.DataBind();

            if (dt.Rows.Count > 0)
            {
                checkNotApplicable.Enabled = false;
            }
            else
            {
                checkNotApplicable.Enabled = true;
            }

            DataTable dtTA = new DataTable();
            t.GetTravelAuthorizationByTravelAuthorizationID(Decrypt(Request["TAID"]), ref dtTA);
            hdnStatusCode.Value = dtTA.Rows[0]["StatusCode"].ToString();

            if (Convert.ToBoolean(dtTA.Rows[0]["AdvancesNotApplicable"]))
            {
                checkNotApplicable.Checked = true;
                txtAdvDate.Enabled = false; ;
                DDLAdvCurrency.Enabled = false;
                ddlAdvPayOffice.Enabled = false;
                txtAdvAmount.Enabled = false;
                ibAdd.Enabled = false;

            }

            if (dtTA.Rows[0]["StatusCode"].ToString().ToUpper() == "COM")
            {
                btnSaveAdvances.Visible = false;
                divAdv.Visible = false;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void GVAdvancesCalculation()
    {
        try
        {
            decimal Rowtotam = 0;
            decimal Rowtotaml = 0;
            foreach (GridViewRow r in GVAdvances.Rows)
            {
                TextBox txtRateAmount = (TextBox)r.FindControl("txtRateAmount");
                TextBox txtAmountLocal = (TextBox)r.FindControl("txtAmountLocal");

                if (txtRateAmount.Text != "")
                {
                    Rowtotam = Rowtotam + Convert.ToDecimal(txtRateAmount.Text);
                }

                if (txtAmountLocal.Text != "")
                {
                    Rowtotaml = Rowtotaml + Convert.ToDecimal(txtAmountLocal.Text);
                }
            }
            hfAdvRateAmountTotal.Value = Rowtotam.ToString();
            hfAdvLocalAmountTotal.Value = Rowtotaml.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVAdvances_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                tec.DeleteTECAdvances(GVAdvances.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TECAdvancesID"].ToString());
                PanelAdvMessage.Visible = true;
                lblAdvMsg.Text = "Travel Advances has been deleted successfully";
                FillGrids();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVAdvances_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtRate, txtRateAmount, txtAmountLocal;
                txtRate = (TextBox)e.Row.FindControl("txtRate");
                txtRateAmount = (TextBox)e.Row.FindControl("txtRateAmount");
                txtAmountLocal = (TextBox)e.Row.FindControl("txtAmountLocal");
                txtRate.Enabled = this.CanAmend;
                txtRateAmount.Enabled = this.CanAmend;
                txtAmountLocal.Enabled = this.CanAmend;
                LinkButton ibDelete;
                ibDelete = (LinkButton)e.Row.FindControl("ibDelete");

                Objects.User ui = (Objects.User)Session["userinfo"];
                if (ViewState["TAUserID"].ToString() == ui.User_Id.ToString())
                {
                    ibDelete.Visible = this.CanEdit;
                }
                else
                {
                    ibDelete.Visible = this.CanEdit;
                }

                if (ViewState["TAUserID"].ToString() == ui.User_Id.ToString())
                {
                    txtRate.Enabled = this.CanAmend;
                    txtRateAmount.Enabled = this.CanAmend;
                    txtAmountLocal.Enabled = this.CanAmend;
                    ibDelete.Visible = this.CanEdit;
                }

                if (hdnStatusCode.Value.ToString().ToUpper() == "SET")
                {
                    txtRate.Enabled = false;
                    txtRateAmount.Enabled = false;
                    txtAmountLocal.Enabled = false;
                    ibDelete.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVAdvances_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                GVAdvancesCalculation();
                e.Row.Cells[0].ColumnSpan = 4;
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(1);
                Label lblRateAmountTotal, lblAmountLocalTotal;
                lblRateAmountTotal = (Label)e.Row.FindControl("lblRateAmountTotal");
                lblAmountLocalTotal = (Label)e.Row.FindControl("lblAmountLocalTotal");
                lblRateAmountTotal.Text = hfAdvRateAmountTotal.Value;
                lblAmountLocalTotal.Text = hfAdvLocalAmountTotal.Value;
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVAdvances_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void btnSaveAdvances_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkNotApplicable.Checked)
            {
                if (GVAdvances.Rows.Count > 0)
                {
                    PanelAdvMessage.Visible = true;
                    PanelAdvMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblAdvMsg.ForeColor = Color.Red;
                    lblAdvMsg.Text = "Please remove the TEC advances before you click on not applicable";
                    return;
                }
                else
                {
                    tec.UpdateTECAdvances(Decrypt(Request["TAID"].ToString()), checkNotApplicable.Checked);
                    Response.Redirect("~/TravelAuthorization/TAWizard/Step8_CheckList.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }
            }
            else
            {
                if (txtAdvDate.Text != "" || DDLAdvCurrency.SelectedIndex != 0 || ddlAdvPayOffice.SelectedIndex != 0 || txtAdvAmount.Text != "")
                {
                    bool invalid = false;

                    txtAdvDate.CssClass = "form-control";
                    DDLAdvCurrency.CssClass = "form-control";
                    ddlAdvPayOffice.CssClass = "form-control";
                    txtAdvAmount.CssClass = "form-control";

                    if (txtAdvDate.Text == "")
                    {
                        txtAdvDate.CssClass = "form-control invalid";
                        invalid = true;
                    }
                    if (DDLAdvCurrency.SelectedIndex == 0)
                    {
                        DDLAdvCurrency.CssClass = "form-control invalid";
                        invalid = true;
                    }
                    if (ddlAdvPayOffice.SelectedIndex == 0)
                    {
                        ddlAdvPayOffice.CssClass = "form-control invalid";
                        invalid = true;
                    }
                    if (txtAdvAmount.Text == "")
                    {
                        txtAdvAmount.CssClass = "form-control invalid";
                        invalid = true;
                    }

                    if (invalid)
                        return;

                    //Insert advance

                    if (g.CheckDate(txtAdvDate.Text))
                    {
                        txtAdvDate.CssClass = "form-control ReqAdv";
                        float Amount;
                        if (float.TryParse(txtAdvAmount.Text, out Amount) == true)
                        {
                            txtAdvAmount.CssClass = "form-control ReqAdv";
                            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);

                            tec.UpdateTECAdvances(Decrypt(Request["TAID"].ToString()), false);
                            tec.InsertTECAdvances(TravelAuthorizationNumber, ddlAdvPayOffice.SelectedValue.ToString(), Convert.ToDateTime(txtAdvDate.Text), DDLAdvCurrency.SelectedValue.ToString(), Amount);

                            PanelAdvMessage.Visible = true;
                            lblAdvMsg.Text = "Travel Expense has been added successfully";
                            ddlAdvPayOffice.SelectedIndex = -1;
                            DDLAdvCurrency.SelectedIndex = -1;
                            txtAdvDate.Text = "";
                            txtAdvAmount.Text = "";
                        }
                        else
                        {
                            txtAdvAmount.CssClass += " invalid";
                        }
                    }
                    else
                    {
                        txtAdvDate.CssClass += " invalid";
                    }
                }
                else if (GVAdvances.Rows.Count == 0)
                {
                    PanelAdvMessage.Visible = true;
                    PanelAdvMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblAdvMsg.ForeColor = Color.Red;
                    lblAdvMsg.Text = "Please add TEC advances or click on not applicable";
                    return;
                }
                //Update rates

                for (int i = 0; i <= GVAdvances.Rows.Count - 1; i++)
                {
                    TextBox txtRate, txtRateAmount, txtAmountLocal;
                    float Rate, RateAmount, AmounLocal, Amount, LocalExch;
                    txtRate = (TextBox)GVAdvances.Rows[i].FindControl("txtRate");
                    txtRateAmount = (TextBox)GVAdvances.Rows[i].FindControl("txtRateAmount");
                    txtAmountLocal = (TextBox)GVAdvances.Rows[i].FindControl("txtAmountLocal");
                    if (float.TryParse(txtRate.Text, out Rate) == false)
                        Rate = 0;
                    if (float.TryParse(txtRateAmount.Text, out RateAmount) == false)
                        RateAmount = 0;
                    if (float.TryParse(txtAmountLocal.Text, out AmounLocal) == false)
                        AmounLocal = 0;
                    if (float.TryParse(GVAdvances.Rows[i].Cells[3].Text, out Amount) == false)
                        Amount = 0;

                    DataTable dt = new DataTable();
                    tec.GetTECItineraryExchangeRateByTravelAuthorizationNumber(ref dt, Decrypt(Request["TANO"]));
                    string ExRate = dt.Rows[0]["ExchangeRate"].ToString();

                    if (float.TryParse(ExRate, out LocalExch) == false)
                        LocalExch = 0;

                    if (LocalExch == 0)
                    {
                        if (float.TryParse(ConfigurationManager.AppSettings["ExchangeRate"], out LocalExch) == false)
                            LocalExch = 0;
                    }

                    if (Rate == 0)
                    {
                        RateAmount = 0;
                        AmounLocal = 0;
                    }
                    else
                    {
                        RateAmount = Amount / Rate;
                        AmounLocal = RateAmount * LocalExch;
                    }
                    tec.UpdateTECAdvancesRates(GVAdvances.DataKeys[i].Values["TECAdvancesID"].ToString(), Rate, RateAmount, AmounLocal);
                }
                Response.Redirect("~/TravelAuthorization/TAWizard/Step8_CheckList.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (g.CheckDate(txtAdvDate.Text))
            {
                txtAdvDate.CssClass = "form-control ReqAdv";
                float Amount;
                if (float.TryParse(txtAdvAmount.Text, out Amount) == true)
                {
                    txtAdvAmount.CssClass = "form-control ReqAdv";
                    string TravelAuthorizationNumber = Decrypt(Request["TANO"]);

                    tec.UpdateTECAdvances(Decrypt(Request["TAID"].ToString()), false);
                    tec.InsertTECAdvances(TravelAuthorizationNumber, ddlAdvPayOffice.SelectedValue.ToString(), Convert.ToDateTime(txtAdvDate.Text), DDLAdvCurrency.SelectedValue.ToString(), Amount);

                    PanelAdvMessage.Visible = true;
                    lblAdvMsg.Text = "Travel Expense has been added successfully";
                    ddlAdvPayOffice.SelectedIndex = -1;
                    DDLAdvCurrency.SelectedIndex = -1;
                    txtAdvDate.Text = "";
                    txtAdvAmount.Text = "";

                    FillGrids();
                }
                else
                {
                    txtAdvAmount.CssClass += " invalid";
                }
            }
            else
            {
                txtAdvDate.CssClass += " invalid";
            }
           
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void checkNotApplicable_CheckedChanged(object sender, EventArgs e)
    {
        if (checkNotApplicable.Checked)
        {
            txtAdvDate.Enabled = false; ;
            DDLAdvCurrency.Enabled = false;
            ddlAdvPayOffice.Enabled = false;
            txtAdvAmount.Enabled = false;

            txtAdvDate.Text = "" ;
            DDLAdvCurrency.SelectedIndex = -1;
            ddlAdvPayOffice.SelectedIndex = -1;
            txtAdvAmount.Text = "";

            ibAdd.Enabled = false;
        }
        else
        {
            txtAdvDate.Enabled = true;
            DDLAdvCurrency.Enabled = true;
            ddlAdvPayOffice.Enabled = true;
            txtAdvAmount.Enabled = true;
            ibAdd.Enabled = true;
        }
    }
    #endregion

    
}