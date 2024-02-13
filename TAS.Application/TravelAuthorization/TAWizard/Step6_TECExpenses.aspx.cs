using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class TravelAuthorization_TAWizard_Step6_TECExpenses : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.Media Media = new Business.Media();
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
            lnk = (LinkButton)WizardHeader.FindControl("lbStep6");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            FillDDLs();           
            FillGrids();
            

            //lock content
            DataTable dt = new DataTable();
            t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
            if (dt.Rows.Count > 0)
            {
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
                //if (dt.Rows[0]["StatusCode"].ToString() == "SET" || dt.Rows[0]["StatusCode"].ToString() == "TECCom")
                //{
                //    pnlContent.Enabled = false;
                //}
            }
        }
    }

    #region Exp

    protected void FillDDLs()
    {
        DataSet ds = new DataSet();
        l.GetAllLookupsList(ref ds);

        DDLExpCurr.DataSource = ds.Tables[7];
        DDLExpCurr.DataBind();
    }
    public void FillGrids()
    {
        try
        {
            CheckExpFile();

            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
            string TravelAuthorizationID = Decrypt(Request["TAID"]);

            DataTable dt = new DataTable();
            tec.GetTECExpenditureByTravelAuthorizationNumber(ref dt, TravelAuthorizationNumber);
            gvExpenses.DataSource = dt;
            gvExpenses.DataBind();

            DataTable dtM = new DataTable();
            DataTable dtTA = new DataTable();
            t.GetTravelAuthorizationByTravelAuthorizationID(Decrypt(Request["TAID"]), ref dtTA);
            Media.GetTECExpensesFilesTAID(TravelAuthorizationID, ref dtM);

            if (dt.Rows.Count > 0 | dtM.Rows.Count > 0)
            {
                checkNotApplicable.Enabled = false;
            }
            else
            {
                checkNotApplicable.Enabled = true;
            }

            if (Convert.ToBoolean(dtTA.Rows[0]["ExpenditureNotApplicable"]))
            {
                checkNotApplicable.Checked = true;
                txtExpDetails.Enabled = false;
                txtExpDate.Enabled = false;
                DDLExpCurr.Enabled = false;
                txtExpAmount.Enabled = false;
                ibAdd.Enabled = false;
                lblNote.Visible = false;

                fuAttachmentTEC.Style.Clear();
                pnlFileTEC.Style.Clear();
                ibDeleteTEC.Style.Clear();
                lblPleaseAttached.Visible = false;              
                fuAttachmentTEC.Style.Add("display", "none");
                pnlFileTEC.Style.Add("display", "none");
                ibDeleteTEC.Style.Add("display", "none");
                return;
            }

            if (dtTA.Rows[0]["StatusCode"].ToString().ToUpper() == "COM")
            {
                if (dtM.Rows.Count > 0)
                {
                    fuAttachmentTEC.Style.Clear();
                    pnlFileTEC.Style.Clear();
                    ibDeleteTEC.Style.Clear();
                    lblPleaseAttached.Visible = true;
                    fuAttachmentTEC.Style.Add("display", "none");
                    pnlFileTEC.Style.Add("display", "block");
                    ibDeleteTEC.Style.Add("display", "none");
                }
                else
                {
                    fuAttachmentTEC.Style.Clear();
                    pnlFileTEC.Style.Clear();
                    ibDeleteTEC.Style.Clear();
                    lblPleaseAttached.Visible = false;
                    fuAttachmentTEC.Style.Add("display", "none");
                    pnlFileTEC.Style.Add("display", "none");

                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    void gvExpensesCalculation()
    {
        try
        {
            decimal Rowtotam = 0;
            decimal Rowtotaml = 0;
            foreach (GridViewRow r in gvExpenses.Rows)
            {
                TextBox txtNoOfDays = (TextBox)r.FindControl("txtRateAmount");
                TextBox txtAmount = (TextBox)r.FindControl("txtAmountLocal");

                if (txtNoOfDays.Text != "")
                {
                    Rowtotam = Rowtotam + Convert.ToDecimal(txtNoOfDays.Text);
                }

                if (txtAmount.Text != "")
                {
                    Rowtotaml = Rowtotaml + Convert.ToDecimal(txtAmount.Text);
                }
            }
            hfExpRateAmountTotal.Value = Rowtotam.ToString();
            hfExpLocalAmountTotal.Value = Rowtotaml.ToString();

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvExpenses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                tec.DeleteTECExpenditure(gvExpenses.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TECExpenditureID"].ToString());
                PanelExpMessage.Visible = true;
                lblExpMsg.Text = "Travel Expense has been deleted successfully";
                FillGrids();
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvExpenses_RowDataBound(object sender, GridViewRowEventArgs e)
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
                ibDelete.Visible = this.CanDelete;

            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvExpenses_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                gvExpensesCalculation();
                e.Row.Cells[0].ColumnSpan = 4;
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells.RemoveAt(1);
                Label lblRateAmountTotal, lblAmountLocalTotal;
                lblRateAmountTotal = (Label)e.Row.FindControl("lblRateAmountTotal");
                lblAmountLocalTotal = (Label)e.Row.FindControl("lblAmountLocalTotal");
                lblRateAmountTotal.Text = hfExpRateAmountTotal.Value;
                lblAmountLocalTotal.Text = hfExpLocalAmountTotal.Value;
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
            if (g.CheckDate(txtExpDate.Text))
            {
                txtExpDate.CssClass = "form-control ReqExp";
                float Amount;
                if (float.TryParse(txtExpAmount.Text, out Amount) == true)
                {
                    txtExpAmount.CssClass = "form-control ReqExp";
                    string TravelAuthorizationNumber = Decrypt(Request["TANO"]);

                    tec.UpdateTECExpenditure(Decrypt(Request["TAID"].ToString()), false);
                    tec.InsertTECExpenditure(TravelAuthorizationNumber, Convert.ToDateTime(txtExpDate.Text), txtExpDetails.Text, DDLExpCurr.SelectedValue.ToString(), Amount);

                    PanelExpMessage.Visible = true;
                    PanelExpMessage.CssClass = "alert alert-success alert-dismissable";
                    lblExpMsg.ForeColor = Color.Green;
                    lblExpMsg.Text = "Travel Expense has been added successfully";

                    FillGrids();
                    CheckExpFile();

                    txtExpDetails.Text = "";
                    DDLExpCurr.SelectedIndex = -1;
                    txtExpDate.Text = "";
                    txtExpAmount.Text = "";
                }
                else
                {
                    txtExpAmount.CssClass += " invalid";
                }
            }
            else
            {
                txtExpDate.CssClass += " invalid";
            }
           
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void btnSaveExpense_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkNotApplicable.Checked)
            {
                if (gvExpenses.Rows.Count > 0)
                {
                    PanelExpMessage.Visible = true;
                    PanelExpMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblExpMsg.ForeColor = Color.Red;
                    lblExpMsg.Text = "Please remove the TEC expenses before you click on not applicable";
                    return;
                }
                else
                {
                    tec.UpdateTECExpenditure(Decrypt(Request["TAID"].ToString()), checkNotApplicable.Checked);
                    Response.Redirect("~/TravelAuthorization/TAWizard/Step7_TECAdvances.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
                }
            }
            else
            {
                if (txtExpDetails.Text != "" || txtExpDate.Text != "" || DDLExpCurr.SelectedIndex != 0 || txtExpAmount.Text != "")
                {
                    bool invalid = false;

                    txtExpDetails.CssClass = "form-control";
                    txtExpDate.CssClass = "form-control";
                    DDLExpCurr.CssClass = "form-control";
                    txtExpAmount.CssClass = "form-control";

                    if (txtExpDetails.Text == "")
                    {
                        txtExpDetails.CssClass = "form-control invalid";
                        invalid = true;
                    }
                    if (txtExpDate.Text == "")
                    {
                        txtExpDate.CssClass = "form-control invalid";
                        invalid = true;
                    }
                    if (DDLExpCurr.SelectedIndex == 0)
                    {
                        DDLExpCurr.CssClass = "form-control invalid";
                        invalid = true;
                    }
                    if (txtExpAmount.Text == "")
                    {
                        txtExpAmount.CssClass = "form-control invalid";
                        invalid = true;
                    }


                    if (invalid)
                        return;

                    // Insert expenses

                    if (g.CheckDate(txtExpDate.Text))
                    {
                        txtExpDate.CssClass = "form-control ReqExp";
                        float Amount;
                        if (float.TryParse(txtExpAmount.Text, out Amount) == true)
                        {
                            txtExpAmount.CssClass = "form-control ReqExp";
                            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);

                            tec.UpdateTECExpenditure(Decrypt(Request["TAID"].ToString()), false);
                            tec.InsertTECExpenditure(TravelAuthorizationNumber, Convert.ToDateTime(txtExpDate.Text), txtExpDetails.Text, DDLExpCurr.SelectedValue.ToString(), Amount);

                            PanelExpMessage.Visible = true;
                            PanelExpMessage.CssClass = "alert alert-success alert-dismissable";
                            lblExpMsg.ForeColor = Color.Green;
                            lblExpMsg.Text = "Travel Expense has been added successfully";

                            txtExpDetails.Text = "";
                            DDLExpCurr.SelectedIndex = -1;
                            txtExpDate.Text = "";
                            txtExpAmount.Text = "";
                        }
                        else
                        {
                            txtExpAmount.CssClass += " invalid";
                        }
                    }
                    else
                    {
                        txtExpDate.CssClass += " invalid";
                    }
                }
                else if (gvExpenses.Rows.Count == 0)
                {
                    PanelExpMessage.Visible = true;
                    PanelExpMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblExpMsg.ForeColor = Color.Red;
                    lblExpMsg.Text = "Please add TEC expences or click on not applicable";
                    return;
                }

                //update rates
                for (int i = 0; i <= gvExpenses.Rows.Count - 1; i++)
                {
                    TextBox txtRate, txtRateAmount, txtAmountLocal;
                    float Rate, RateAmount, AmounLocal, Amount, LocalExch;
                    txtRate = (TextBox)gvExpenses.Rows[i].FindControl("txtRate");
                    txtRateAmount = (TextBox)gvExpenses.Rows[i].FindControl("txtRateAmount");
                    txtAmountLocal = (TextBox)gvExpenses.Rows[i].FindControl("txtAmountLocal");
                    if (float.TryParse(txtRate.Text, out Rate) == false)
                        Rate = 0;
                    if (float.TryParse(txtRateAmount.Text, out RateAmount) == false)
                        RateAmount = 0;
                    if (float.TryParse(txtAmountLocal.Text, out AmounLocal) == false)
                        AmounLocal = 0;
                    if (float.TryParse(gvExpenses.Rows[i].Cells[3].Text, out Amount) == false)
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
                    tec.UpdateTECExpenditureRates(gvExpenses.DataKeys[i].Values["TECExpenditureID"].ToString(), Rate, RateAmount, AmounLocal);
                }
                Response.Redirect("~/TravelAuthorization/TAWizard/Step7_TECAdvances.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
            }

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void checkNotApplicable_CheckedChanged(object sender, EventArgs e)
    {
        lblUploadMessageTEC.Text = "";

        if (checkNotApplicable.Checked)
        {
            txtExpDetails.Enabled = false;
            txtExpDate.Enabled = false;
            DDLExpCurr.Enabled = false;
            txtExpAmount.Enabled = false;


            txtExpDetails.Text = "";
            txtExpDate.Text = "";
            DDLExpCurr.SelectedIndex = -1;
            txtExpAmount.Text = "";

            ibAdd.Enabled = false;
            lblNote.Visible = false;         

            fuAttachmentTEC.Style.Clear();
            pnlFileTEC.Style.Clear();
            ibDeleteTEC.Style.Clear();
            lblPleaseAttached.Visible = false;
            fuAttachmentTEC.Style.Add("display", "none");
            pnlFileTEC.Style.Add("display", "none");

        }
        else
        {
            txtExpDetails.Enabled = true;
            txtExpDate.Enabled = true;
            DDLExpCurr.Enabled = true;
            txtExpAmount.Enabled = true;
            ibAdd.Enabled = true;
            lblNote.Visible = true;

            DataTable dtM = new DataTable();
            Media.GetTECExpensesFilesTAID(Decrypt(Request["TAID"]), ref dtM);

            if (dtM.Rows.Count > 0)
            {
                fuAttachmentTEC.Style.Clear();
                pnlFileTEC.Style.Clear();
                ibDeleteTEC.Style.Clear();
                lblPleaseAttached.Visible = true;
                fuAttachmentTEC.Style.Add("display", "none");
                pnlFileTEC.Style.Add("display", "block");
            }
            else
            {
                fuAttachmentTEC.Style.Clear();
                pnlFileTEC.Style.Clear();
                ibDeleteTEC.Style.Clear();
                lblPleaseAttached.Visible = false;
                fuAttachmentTEC.Style.Add("display", "block");
                pnlFileTEC.Style.Add("display", "none");

            }
        } 
    }
    #endregion

    #region Upload
    protected void CheckExpFile()
    {
        string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());

        byte[] bytes = { 0 };
        string fileName = "", contentType = "";

        DataTable dt = new DataTable();

        Media.GetTECExpensesFilesTAID(TravelAuthorizationID, ref dt);

        if (dt.Rows.Count > 0)
        {
            fuAttachmentTEC.Style.Clear();
            fuAttachmentTEC.Style.Add("display", "none");
            pnlFileTEC.Style.Clear();
            pnlFileTEC.Style.Add("display", "block");
        }
        else
        {
            fuAttachmentTEC.Style.Clear();
            fuAttachmentTEC.Style.Add("display", "block");
            pnlFileTEC.Style.Clear();
            pnlFileTEC.Style.Add("display", "none");
        }
    }
    protected void ProcessUploadTEC(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        try
        {
            string FileName = Path.GetFileName(e.FileName);
            string FileNameOnly = Path.GetFileNameWithoutExtension(e.FileName);
            string FileExtension = Path.GetExtension(e.FileName);

            if (FileName != string.Empty)
            {
                Regex FilenameRegex = new Regex("(.*?)\\.(doc|docx|pdf|xls|xlsx)$");
                if ((FilenameRegex.IsMatch(FileName, Convert.ToInt16(RegexOptions.IgnoreCase))))
                {
                    Guid gname = default(Guid);
                    gname = Guid.NewGuid();
                    string nameAndType = "Expenses - " + FileNameOnly + FileExtension;
                    string UploadFileName = "Expenses - " + FileNameOnly + " - " + gname.ToString() + FileExtension;
                    if (!Directory.Exists(Server.MapPath("~\\UploadedFiles\\")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~\\UploadedFiles\\"));
                    }
                    if (int.Parse(e.FileSize) < 4194304)
                    {
                        fuAttachmentTEC.SaveAs(Server.MapPath("~\\UploadedFiles\\" + UploadFileName));
                        if (e.State == AjaxControlToolkit.AsyncFileUploadState.Success)
                        {
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), gname.ToString(), "top.$get('ContentPlaceHolder1_hdnlblFileExtTEC').value = '" + FileExtension + "'; top.$get('ContentPlaceHolder1_hdnNameOnlyTEC').value = '" + FileNameOnly + "'; top.$get('ContentPlaceHolder1_hfFileLoadTEC').value = '" + ResolveClientUrl(UploadFileName) + "';", true);
                            DataSet ds = new DataSet();
                            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
                            string TravelAuthorizationID = Decrypt(Request["TAID"]);
                            Media.InsertTECExpensesFiles(TravelAuthorizationID, TravelAuthorizationNumber, FileNameOnly, FileExtension, g.GetPhoto(Server.MapPath("~\\UploadedFiles\\" + UploadFileName)));
                        }
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibViewTEC_Click(object sender, EventArgs e)
    {
        try
        {

            string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
            string TravelAuthorizationID = Decrypt(Request["TAID"]);
            byte[] bytes = { 0 };
            string fileName = "", contentType = "";

            DataTable dt = new DataTable();

            Media.GetTECExpensesFilesTAID(TravelAuthorizationID, ref dt);

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
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + contentType);
            Response.BinaryWrite(bytes);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void ibDeleteTEC_Click(object sender, EventArgs e)
    {
        string TravelAuthorizationNumber = Decrypt(Request["TANO"]);
        string TravelAuthorizationID = Decrypt(Request["TAID"]);

        Media.DeleteTECExpensesFiles(TravelAuthorizationID);

        FillGrids();

        lblUploadMessageTEC.ForeColor = Color.Green;
        fuAttachmentTEC.Style.Clear();
        lblPleaseAttached.Visible = true;
        fuAttachmentTEC.Style.Add("display", "block");
        pnlFileTEC.Style.Clear();
        pnlFileTEC.Style.Add("display", "none");
        lblUploadMessageTEC.Text = "Item has been deleted successfully";

       
    }
    
    #endregion





}