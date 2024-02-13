using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TravelAuthorization_TAWizard_Step8_CheckList : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();
    Business.TravelExpenseClaim tec = new Business.TravelExpenseClaim();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl TAStatus = null;
            TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
            TAStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep8");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            DataTable dtTA = new DataTable();
            if (!string.IsNullOrEmpty(Request["TANO"] as string))
            {
                t.GetTravelAuthorizationByTravelAuthorizationNumber(Decrypt(Request["TANO"]), ref dtTA);

                hdnUserID.Value = dtTA.Rows[0]["UserID"].ToString();
                hdnStatusCode.Value = dtTA.Rows[0]["StatusCode"].ToString();
                hdnTravelSchema.Value = dtTA.Rows[0]["TravelSchema"].ToString();
            }

            CheckList();

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
        else
        {
            if (cbConfirmationMessage.Checked == true)
            {
                bool First = false;
                bool Second = false;
                bool Third = false;
                bool Four = false;

                btnSaveCheckList.Enabled = true;
                lblCheckListMsg.Text = "";
                PanelCheckList.Visible = false;

                foreach (GridViewRow row in GVCheckList.Rows)
                {
                    Label lblDescription = (Label)row.FindControl("lblDescription");
                    HiddenField hfCode = (HiddenField)row.FindControl("hfCode");
                    RadioButtonList rb = (RadioButtonList)row.FindControl("rdCheckList");

                    if (hfCode.Value.ToString() == "OBRH")
                    {
                        if (rb.SelectedValue == "1")
                        {
                            First = true;
                        }
                    }
                    else if (hfCode.Value.ToString() == "OREC")
                    {
                        if (rb.SelectedValue == "1")
                        {
                            Second = true;
                        }
                    }
                    else if (hfCode.Value.ToString() == "OTRBP")
                    {
                        if (rb.SelectedValue == "1")
                        {
                            Third = true;
                        }
                    }
                    else if (hfCode.Value.ToString() == "OTA")
                    {
                        if (rb.SelectedValue == "1")
                        {
                            Four = true;
                        }
                    }
                }

                if (First == true && Second == true && Third == true && Four == true)
                {
                    btnSaveCheckList.Enabled = true;
                }
                else
                {
                    PanelCheckList.Visible = true;
                    PanelCheckList.CssClass = "alert alert-danger alert-dismissable";
                    lblCheckListMsg.ForeColor = Color.Red;
                    lblCheckListMsg.Text = "Please insure that (Original bills,  Original receipts, Original ticket, Original Travel Authorization) are part of the TEC";
                    btnSaveCheckList.Enabled = false;
                }
            }
            else
            {
                btnSaveCheckList.Enabled = false;
                lblCheckListMsg.Text = "";
                PanelCheckList.Visible = false;
            }
        }
    }

    #region CheckList
    void CheckList()
    {
        try
        {
            DataTable dt = new DataTable();
            l.GetCheckList(ref dt);
            GVCheckList.DataSource = dt;
            GVCheckList.DataBind();

            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                DataTable dtTA = new DataTable();
                t.GetTravelAuthorizationByTravelAuthorizationID(Decrypt(Request["TAID"]), ref dtTA);

                if (dtTA.Rows[0]["IsTecComplete"].ToString() != "")
                {
                    cbConfirmationMessage.Checked = Convert.ToBoolean(dtTA.Rows[0]["IsTecComplete"]);
                    if (cbConfirmationMessage.Checked == true)
                    {
                        cbConfirmationMessage.Enabled = false;

                        DataSet dsUser = new DataSet();
                        Objects.User ui = (Objects.User)Session["userinfo"];

                        if (!string.IsNullOrEmpty(Request["TANO"] as string))
                        {
                            if (dtTA.Rows[0]["UserID"] == ui.User_Id.ToString())
                            {
                                if (dtTA.Rows[0]["StatusCode"] == "TECDI")
                                {
                                    btnSaveCheckList.Enabled = true;
                                }
                                else
                                {
                                    btnSaveCheckList.Enabled = false;
                                }
                            }
                            else
                            {
                                btnSaveCheckList.Enabled = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVCheckList_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GVCheckList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadioButtonList rb = (RadioButtonList)e.Row.FindControl("rdCheckList");
            TextBox txtNote = (TextBox)e.Row.FindControl("txtNote");
            Objects.User ui = (Objects.User)Session["userinfo"];

            if (hdnUserID.Value == ui.User_Id.ToString() || ui.IsManager==true)
            {
                rb.Enabled = true;
                txtNote.Enabled = true;
            }
            else
            {
                // open to finance or manager in case of escort 
                if (hdnTravelSchema.Value == "Escort")
                {
                    rb.Enabled = true;
                    txtNote.Enabled = true;
                }
                else
                {
                    rb.Enabled = false;
                    txtNote.Enabled = false;
                }
               

            }

            HiddenField hfCode = (HiddenField)e.Row.FindControl("hfCode");
            DataTable dtCheckList = new DataTable();
            string TravelAuthorizationID = Decrypt(Request["TAID"]);
            string lookupID = GVCheckList.DataKeys[Convert.ToInt16(e.Row.RowIndex)].Values["LookupsID"].ToString();

            t.GetCheckList(TravelAuthorizationID, lookupID, ref dtCheckList);
            if (dtCheckList.Rows.Count > 0)
            {
                rb.SelectedValue = dtCheckList.Rows[0]["Value"].ToString();
                txtNote.Text = dtCheckList.Rows[0]["Note"].ToString();

                // to prevent the user from changing the last 4 rd buttons in case of incomplete
                if (hdnStatusCode.Value == "TECDI")
                {
                    if (hfCode.Value.ToString() == "OBRH" || hfCode.Value.ToString() == "OREC" || hfCode.Value.ToString() == "OTRBP" || hfCode.Value.ToString() == "OTA")
                    {
                        rb.Enabled = false;
                        txtNote.Enabled = false;
                    }
                    else
                    {
                        rb.Enabled = true;
                        txtNote.Enabled = true;
                    }
                }
                else
                {
                    rb.Enabled = false;
                }
            }

            //to lock the form
            if (hdnStatusCode.Value == "SET")
            {
                rb.Enabled = false;
                txtNote.Enabled = false;
                rb.Enabled = false;
               
            }
        }
    }
    protected void GVCheckList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVCheckList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnSaveCheckList_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = Decrypt(Request["TAID"]);
                t.DeleteCheckList(TravelAuthorizationID);
                foreach (GridViewRow row in GVCheckList.Rows)
                {
                    string LookupID = GVCheckList.DataKeys[row.RowIndex]["LookupsID"].ToString();
                    RadioButtonList rb = (RadioButtonList)row.FindControl("rdCheckList");
                    if (rb.SelectedValue == "")
                    {
                        rb.SelectedValue = "3";
                    }
                    TextBox txtNote = (TextBox)row.FindControl("txtNote");
                    t.InsertCheckList(TravelAuthorizationID, LookupID, Convert.ToInt32(rb.SelectedValue), txtNote.Text);
                }
                t.UpdateTravelAuthorizationIsComplete(TravelAuthorizationID, cbConfirmationMessage.Checked);
                CheckList();
                PanelCheckList.Visible = true;
                PanelCheckList.CssClass = "alert alert-success alert-dismissable";
                lblCheckListMsg.ForeColor = Color.Green;
                lblCheckListMsg.Text = "Changes have been saved successfully";
                DataTable dtTecCheck = new DataTable();
                if (!string.IsNullOrEmpty(Request["TANO"] as string))
                {
                    tec.CheckTravelIteneraryTime(ref dtTecCheck, Decrypt(Request["TANO"]));
                }

                Response.Redirect("~/TravelAuthorization/TAWizard/DownloadTEC.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);

            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void cbConfirmationMessage_CheckedChanged(object sender, EventArgs e)
    {
        if (cbConfirmationMessage.Checked == true)
        {
            btnSaveCheckList.Enabled = true;
            lblCheckListMsg.Text = "";
            PanelCheckList.Visible = false;
        }
        else
        {
            btnSaveCheckList.Enabled = false;
            lblCheckListMsg.Text = "";
            PanelCheckList.Visible = false;
        }
       
    }
    #endregion
}