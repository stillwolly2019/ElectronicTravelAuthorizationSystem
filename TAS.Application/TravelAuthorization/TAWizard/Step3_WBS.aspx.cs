using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class TravelAuthorization_TAWizard_Step3_WBS : AuthenticatedPageClass
{
    Business.Lookups l = new Business.Lookups();
    Business.TravelAuthorization t = new Business.TravelAuthorization();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl TAStatus = null;
            TAStatus = (System.Web.UI.HtmlControls.HtmlGenericControl)WizardHeader.FindControl("TAStatusDiv");
            TAStatus.Visible = this.CanAmend;

            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep3");
            lnk.CssClass = "btn btn-success btn-circle btn-lg";

            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());
                FillWBSItems(TravelAuthorizationID);

                // lock the form only if the status is in the TEC
                DataTable dt = new DataTable();
                t.GetTAStatusByTAID(Decrypt(Request["TAID"]), ref dt);
                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0]["StatusCode"].ToString() == "TADC" || dt.Rows[0]["StatusCode"].ToString() == "TECCom" || dt.Rows[0]["StatusCode"].ToString() == "TECDI" || dt.Rows[0]["StatusCode"].ToString() == "SET" || dt.Rows[0]["StatusCode"].ToString() == "TECRTA" || dt.Rows[0]["StatusCode"].ToString() == "TECDC" || dt.Rows[0]["StatusCode"].ToString() == "NDSA")
                    if(Convert.ToBoolean(dt.Rows[0]["IsEditable"].ToString()) ==false)
                    {
                        pnlContent.Enabled = false;
                    }
                }
            }
        }
    }

    #region WBS

    void ClearAll()
    {
        txtWBSCode.Text = "";
        txtPercentageOrAmount.Text = "";
        txtNote.Text = "";

        txtWBSCode.CssClass = "form-control";
        txtPercentageOrAmount.CssClass = "form-control";
        txtNote.CssClass = "form-control";

        gvWBS.SelectedIndex = -1;
        for (int i = 0; i <= gvWBS.Rows.Count - 1; i++)
        {
            gvWBS.Rows[i].BackColor = default(System.Drawing.Color);
            gvWBS.Rows[i].ForeColor = default(System.Drawing.Color);
        }

    }
    void FillWBSItems(string TravelAuthorizationID)
    {
        try
        {
            DataTable dt = new DataTable();
            t.GetWBSByTravelAuthorizationID(TravelAuthorizationID, ref dt);
            gvWBS.DataSource = dt;
            gvWBS.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void gvWBS_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            LinkButton ibD = new LinkButton();
            ibD = (LinkButton)e.Row.FindControl("ibDeleteWBS");
            ibD.Visible = this.CanDelete;

            LinkButton ibe = new LinkButton();
            ibe = (LinkButton)e.Row.FindControl("ibEditWBS");
            ibe.Visible = this.CanEdit;
        }
    }
    protected void gvWBS_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditWBS")
            {

                ClearAll();
                gvWBS.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvWBS.SelectedRow.BackColor = Color.LightGray;
                gvWBS.SelectedRow.ForeColor = Color.Black;

                txtWBSCode.Text = gvWBS.SelectedRow.Cells[0].Text;
                txtPercentageOrAmount.Text = gvWBS.SelectedRow.Cells[1].Text;
                txtNote.Text = gvWBS.SelectedRow.Cells[2].Text;

            }
            if (e.CommandName == "DeleteWBS")
            {
                t.DeleteWBS(gvWBS.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["WBSID"].ToString());
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

                if (!string.IsNullOrEmpty(Request["TAID"] as string))
                {
                    string TravelAuthorizationID = Decrypt(Request["TAID"].ToString());
                    DataTable dt = new DataTable();
                    t.GetWBSByTravelAuthorizationID(TravelAuthorizationID, ref dt);
                    gvWBS.DataSource = dt;
                    gvWBS.DataBind();

                    if (dt.Rows.Count == 0)
                    {
                         Response.Redirect("~/TravelAuthorization/TAWizard/Step3_WBS.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"].ToString() + "&First=1", false);
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
    protected void gvWBS_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request["TAID"] as string))
            {
                string result = "";

                if (!IsValidWBS(txtWBSCode.Text))
                {
                    txtWBSCode.CssClass = "form-control ReqG2 uppercase invalid";
                    return;
                }
                else
                {
                    txtWBSCode.CssClass = "form-control ReqG2 uppercase";
                }

                int nu = txtWBSCode.Text.Trim().Replace("_", "").Length;

                if (nu == 22)
                {
                    if (gvWBS.SelectedIndex == -1)
                    {
                        
                        result = t.InsertUpdateWBS("", Decrypt(Request["TAID"].ToString()), txtWBSCode.Text.Trim(), Convert.ToDouble(txtPercentageOrAmount.Text.Trim()), txtNote.Text.Trim(), rdPercentage.Checked);
                        if (result == "1")
                        {
                            FillWBSItems(Decrypt(Request["TAID"].ToString()));
                            ClearAll();
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
                        else
                        {
                            FillWBSItems(Decrypt(Request["TAID"].ToString()));
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "This item is already added";
                        }

                    }
                    else
                    {
                       
                        result = t.InsertUpdateWBS(gvWBS.DataKeys[gvWBS.SelectedIndex].Values["WBSID"].ToString(), Decrypt(Request["TAID"].ToString()), txtWBSCode.Text.Trim(), Convert.ToDouble(txtPercentageOrAmount.Text.Trim()), txtNote.Text.Trim(), rdPercentage.Checked);
                        if (result == "1")
                        {
                            FillWBSItems(Decrypt(Request["TAID"].ToString()));
                            ClearAll();
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
                        else
                        {
                            FillWBSItems(Decrypt(Request["TAID"].ToString()));
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "This item is already added";
                        }

                    }
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "The WBS should be 22 char";
                    PanelMessage.Focus();
                    txtWBSCode.Focus();
                    return;
                }
            }

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected bool IsValidWBS(string WBS)
    {
        bool IsValid = false;
        IsValid=t.IsValidWBS(WBS);
        return IsValid;
    }

    protected void WBS_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtWBSCode.Text))
            {
                DataTable dt = new DataTable();
                t.SearchWBS(txtWBSCode.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    txtWBSCode.Text = dt.Rows[0]["WBSId"].ToString();
                }
            }
            else
            {
                txtWBSCode.Text = "";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


    //By Lokiri James
    /*
    protected bool IsValidWBS(string WBS)
    {
        try
        {
            int tmpint;
            DataTable dt = new DataTable();
            char[] delimiterChars = { '.' };
            string[] words = WBS.Trim().Split(delimiterChars);
            //Author: Lokiri James
            if (words.Length == 6)
            {
                bool isAlphaBet = Regex.IsMatch(words[0], "[a-z]", RegexOptions.IgnoreCase);
                if (isAlphaBet)
                {
                    isAlphaBet = Regex.IsMatch(words[2].Substring(0, 2), "[a-z]", RegexOptions.IgnoreCase);
                    if (isAlphaBet && Int32.TryParse(words[1], out tmpint) && Int32.TryParse(words[2].Substring(2, 2), out tmpint) && Int32.TryParse(words[3].Substring(1, 1), out tmpint) && Int32.TryParse(words[4], out tmpint) && Int32.TryParse(words[5], out tmpint) && words[0].Length == 2 && words[1].Length == 4 && words[2].Length == 4 && words[3].Length == 2 && words[4].Length == 2 && words[5].Length == 3)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    */
    //Original from Yemen
    /*
    protected bool IsValidWBS(string WBS)
    {
        try
        {
            int tmpint;
            DataTable dt = new DataTable();
            char[] delimiterChars = { '.' };
            string[] words = WBS.Trim().Split(delimiterChars);
            if (words.Length != 6)
            {
                return false;
            }
            else if (words.Length == 6)
            {
                bool isAlphaBet = Regex.IsMatch(words[0], "[a-z]", RegexOptions.IgnoreCase);
                if (isAlphaBet)
                    isAlphaBet = Regex.IsMatch(words[2].Substring(0, 2), "[a-z]", RegexOptions.IgnoreCase);
                if (isAlphaBet && Int32.TryParse(words[1], out tmpint) && Int32.TryParse(words[2].Substring(2, 2), out tmpint) && Int32.TryParse(words[3], out tmpint) && Int32.TryParse(words[4], out tmpint) && Int32.TryParse(words[5], out tmpint) && words[0].Length == 2 && words[1].Length == 4 && words[2].Length == 4 && words[3].Length == 2 && words[4].Length == 2 && words[5].Length == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    */
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["TAID"] as string) && !string.IsNullOrEmpty(Request["TANO"] as string))
        {
            if (txtWBSCode.Text != "" || txtPercentageOrAmount.Text != "" || txtNote.Text != "")
            {
                bool invalid = false;

                if (txtWBSCode.Text == "")
                {
                    txtWBSCode.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtWBSCode.CssClass = "form-control";
                }
                if (txtPercentageOrAmount.Text == "")
                {
                    txtPercentageOrAmount.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtPercentageOrAmount.CssClass = "form-control";
                }
                if (txtNote.Text == "")
                {
                    txtNote.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtNote.CssClass = "form-control";
                }

                if (invalid)
                    return;

                if (!string.IsNullOrEmpty(Request["TAID"] as string))
                {
                    string result = "";

                    if (!IsValidWBS(txtWBSCode.Text))
                    {
                        txtWBSCode.CssClass = "form-control ReqG2 uppercase invalid";
                        return;
                    }
                    else
                    {
                        txtWBSCode.CssClass = "form-control ReqG2 uppercase";
                    }

                    int nu = txtWBSCode.Text.Trim().Replace("_", "").Length;

                    if (nu == 22)
                    {
                        if (gvWBS.SelectedIndex == -1)
                        {

                            result = t.InsertUpdateWBS("", Decrypt(Request["TAID"].ToString()), txtWBSCode.Text.Trim(), Convert.ToDouble(txtPercentageOrAmount.Text.Trim()), txtNote.Text.Trim(), rdPercentage.Checked);
                            if (result == "1")
                            {
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

                                Response.Redirect("~/TravelAuthorization/TAWizard/Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"] + "&First=1", false);

                            }
                            else
                            {
                                FillWBSItems(Decrypt(Request["TAID"].ToString()));
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "This item is already added";
                            }

                        }
                        else
                        {
                            
                            result = t.InsertUpdateWBS(gvWBS.DataKeys[gvWBS.SelectedIndex].Values["WBSID"].ToString(), Decrypt(Request["TAID"].ToString()), txtWBSCode.Text.Trim(), Convert.ToDouble(txtPercentageOrAmount.Text.Trim()), txtNote.Text.Trim(), rdPercentage.Checked);
                            if (result == "1")
                            {
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
                                
                                Response.Redirect("~/TravelAuthorization/TAWizard/Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"] + "&First=1", false);
                            }
                            else
                            {
                                FillWBSItems(Decrypt(Request["TAID"].ToString()));
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "This item is already added";
                            }
                        }
                    }
                    else
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "The WBS should be 22 char";
                        PanelMessage.Focus();
                        txtWBSCode.Focus();
                        return;
                    }
                }
            }
            else
            {
                if (gvWBS.Rows.Count > 0)
                {
                    Response.Redirect("~/TravelAuthorization/TAWizard/Step4_Itinerary.aspx?TAID=" + Request["TAID"].ToString() + "&&TANO=" + Request["TANO"] + "&First=1", false);
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please add WBS before you continue to the next step.";
                }
                
            }

        }
    }
    #endregion

   
}