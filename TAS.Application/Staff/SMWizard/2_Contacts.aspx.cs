using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Staff_SMWizard_2_Contacts : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep2");
            lnk.CssClass = "btn btn-success btn-circle btn-lg  fa fa-phone";

            FillDDLs();
            if (!string.IsNullOrEmpty(Request["PERNO"] as string))
            {
                string PERNO = Decrypt(Request["PERNO"].ToString());
                FillStaffContacts(PERNO);
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
            ddlContactType.DataSource = ds.Tables[5];
            ddlContactType.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillStaffContacts(string PERNO)
    {
        try
        {
            DataTable dt = new DataTable();
            R.GetStaffContacts(PERNO, ref dt);
            gvStaffContacts.DataSource = dt;
            gvStaffContacts.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        ddlContactType.SelectedIndex = -1;
        txtContactDetails.Text = "";

        gvStaffContacts.SelectedIndex = -1;
        for (int i = 0; i <= gvStaffContacts.Rows.Count - 1; i++)
        {
            gvStaffContacts.Rows[i].BackColor = default(System.Drawing.Color);
            gvStaffContacts.Rows[i].ForeColor = default(System.Drawing.Color);
        }
    }
    protected void gvStaffContacts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            LinkButton ibD = new LinkButton();
            LinkButton ibe = new LinkButton();
            ibe = (LinkButton)e.Row.FindControl("ibEdit");
            ibD = (LinkButton)e.Row.FindControl("ibDelete");
        }
    }
    protected void gvStaffContacts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditContact")
            {
                ClearAll();
                gvStaffContacts.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvStaffContacts.SelectedRow.BackColor = Color.LightGray;
                gvStaffContacts.SelectedRow.ForeColor = Color.Black;

                txtContactDetails.Text = gvStaffContacts.Rows[Convert.ToInt16(e.CommandArgument)].Cells[1].Text.ToString();
                ddlContactType.SelectedValue = gvStaffContacts.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["ContactTypeCode"].ToString();
            }
            if (e.CommandName == "DeleteItem")
            {
                R.DeleteStaffContacts(gvStaffContacts.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["ContactID"].ToString());
                ClearAll();

                if (!string.IsNullOrEmpty(Request["PERNO"] as string))
                {
                    string PERNO = Decrypt(Request["PERNO"].ToString());
                    DataTable dt = new DataTable();
                    R.GetStaffContacts(PERNO, ref dt);
                    gvStaffContacts.DataSource = dt;
                    gvStaffContacts.DataBind();
                    //if (dt.Rows.Count == 0)
                    //{
                    //    Response.Redirect("~/Staff/SMWizard/4_Location.aspx?PERNO=" + Request["PERNO"] + "&First=1", false);
                    //}
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
    protected void gvStaffContacts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void ibAdd_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["PERNO"] as string))
        {
                DataTable dtContactType = new DataTable();
                R.GetContactTypeDescription(ddlContactType.Text.Trim(), ref dtContactType);
                string ContactTypeCode = "";
                foreach (DataRow row in dtContactType.Rows)
                {
                    ContactTypeCode = row["LookupsID"].ToString();
                }

                if (dtContactType.Rows.Count > 0)
                {
                    string result = "";
                    if (gvStaffContacts.SelectedIndex == -1)
                    {
                       // DataTable dtTa = new DataTable();
                        DataTable dtDup = new DataTable();
                        //S.GetContactDetailsByContactID(Decrypt(), ref dtTa);
                        R.CheckDuplicatedContact(Decrypt(Request["PERNO"].ToString()),ddlContactType.SelectedValue, txtContactDetails.Text.Trim(), ref dtDup);
                        //S.CheckDuplicatedContact(Decrypt(Request["CID"].ToString()), dtTa.Rows[0]["PERNO"].ToString(), txtContactDetails.Text.Trim(),ref dtDup);

                        if (dtDup.Rows.Count > 0)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "Staff already has the same Contact";
                        }
                        else
                        {
                        result = R.InsertUpdateStaffContact("", Decrypt(Request["PERNO"].ToString()), ddlContactType.SelectedValue,txtContactDetails.Text.Trim(),gvStaffContacts.Rows.Count + 1);
                        if (result == "1")
                        {
                            ClearAll();
                            FillStaffContacts(Decrypt(Request["PERNO"].ToString()));

                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-success alert-dismissable";
                            lblmsg.ForeColor = Color.Green;
                            lblmsg.Text = "Item has been added successfully";
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

                        //S.GetContactDetailsByContactID(Decrypt(Request["CID"].ToString()), ref dtTa);
                        R.CheckDuplicatedContact(Decrypt(Request["PERNO"].ToString()), ddlContactType.SelectedValue, txtContactDetails.Text.Trim(), ref dtDup);

                        if (dtDup.Rows.Count > 0)
                        {
                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                            lblmsg.ForeColor = Color.Red;
                            lblmsg.Text = "You already have the same MR form with the same itinerary";
                        }
                        else
                        {
                        result = R.InsertUpdateStaffContact(gvStaffContacts.DataKeys[gvStaffContacts.SelectedIndex].Values["ContactID"].ToString(), Decrypt(Request["PERNO"].ToString()), ddlContactType.SelectedValue,txtContactDetails.Text.Trim(),gvStaffContacts.SelectedIndex + 1);
                        if (result == "1")
                        {
                            ClearAll();
                            FillStaffContacts(Decrypt(Request["PERNO"].ToString()));

                            PanelMessage.Visible = true;
                            PanelMessage.CssClass = "alert alert-success alert-dismissable";
                            lblmsg.ForeColor = Color.Green;
                            lblmsg.Text = "Item has been added successfully";
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
                    lblmsg.Text = "You have to select a Contact Catgeory";
                    return;
                }
            
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["PERNO"] as string))
        {
            if (ddlContactType.SelectedIndex != 0 || txtContactDetails.Text != "")
            {
                bool invalid = false;
                if (ddlContactType.SelectedIndex == 0)
                {
                    ddlContactType.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    ddlContactType.CssClass = "form-control";
                }

                if (txtContactDetails.Text == "")
                {
                    txtContactDetails.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    txtContactDetails.CssClass = "form-control";
                }

                if (invalid)
                    return;


                if (!string.IsNullOrEmpty(Request["PERNO"] as string))
                {
                        DataTable dtLm = new DataTable();
                        R.GetContactTypeDescription(ddlContactType.Text.Trim(), ref dtLm);
                        string ContactTypeCode = "";
                        foreach (DataRow row in dtLm.Rows)
                        {
                            ContactTypeCode = row["ContactTypeCode"].ToString();
                        }



                        if (dtLm.Rows.Count > 0)
                        {
                            //    lblmsg.Text = "From date can't be smaller than to date from the previos record";
                            string result = "";
                            if (gvStaffContacts.SelectedIndex == -1)
                            {
                                DataTable dtTa = new DataTable();
                                DataTable dtDup = new DataTable();

                                //S.GetContactDetailsByContactID(Decrypt(Request["CID"].ToString()), ref dtTa);
                                R.CheckDuplicatedContact(Decrypt(Request["PERNO"].ToString()), ddlContactType.SelectedValue, txtContactDetails.Text.Trim(),ref dtDup);

                                if (dtDup.Rows.Count > 0)
                                {
                                    PanelMessage.Visible = true;
                                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                    lblmsg.ForeColor = Color.Red;
                                    lblmsg.Text = "Same contact already exists for this Staff";
                                }
                                else
                                {
                                    result = R.InsertUpdateStaffContact("", Decrypt(Request["PERNO"].ToString()), ddlContactType.SelectedValue,txtContactDetails.Text.Trim(),gvStaffContacts.Rows.Count + 1);
                                    if (result == "1")
                                    {
                                        ClearAll();
                                        FillStaffContacts(Decrypt(Request["PERNO"].ToString()));

                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                        lblmsg.ForeColor = Color.Green;
                                        lblmsg.Text = "Item has been added successfully";
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
                                //S.GetContactDetailsByContactID(Decrypt(Request["CID"].ToString()), ref dtTa);
                                R.CheckDuplicatedContact(Decrypt(Request["PERNO"].ToString()), ddlContactType.SelectedValue, txtContactDetails.Text.Trim(),ref dtDup);

                                if (dtDup.Rows.Count > 0)
                                {
                                    PanelMessage.Visible = true;
                                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                    lblmsg.ForeColor = Color.Red;
                                    lblmsg.Text = "This Contact already exists for same staff";
                                }
                                else
                                {
                                    result = R.InsertUpdateStaffContact(gvStaffContacts.DataKeys[gvStaffContacts.SelectedIndex].Values["ContactID"].ToString(), Decrypt(Request["PERNO"].ToString()), ddlContactType.SelectedValue,txtContactDetails.Text.Trim(), gvStaffContacts.Rows.Count + 1);
                                    if (result == "1")
                                    {
                                        ClearAll();
                                        FillStaffContacts(Decrypt(Request["PERNO"].ToString()));

                                        PanelMessage.Visible = true;
                                        PanelMessage.CssClass = "alert alert-success alert-dismissable";
                                        lblmsg.ForeColor = Color.Green;
                                        lblmsg.Text = "Item has been added successfully";
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
                            lblmsg.Text = "You have to select a contact type, and make sure to select the value from the list";
                        }
                  
                }
            }
            else
            {
                if (gvStaffContacts.Rows.Count > 0)
                {
                    Response.Redirect("~/Staff/SMWizard/3_Location.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
                }
                else
                {
                    PanelMessage.Visible = true;
                    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                    lblmsg.ForeColor = Color.Red;
                    lblmsg.Text = "Please add contacts before you continue to the next step.";
                }

            }

        }
    }


    #endregion

}