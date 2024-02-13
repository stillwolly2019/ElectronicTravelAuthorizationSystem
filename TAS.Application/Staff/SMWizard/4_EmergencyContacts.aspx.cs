using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Staff_SMWizard_4_EmergencyContacts : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    //Business.MovementRequest t = new Business.MovementRequest();
    //Globals g = new Globals();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep4");
            lnk.CssClass = "btn btn-success btn-circle btn-lg  fa fa-ambulance";

            FillDDLs();
            if (!string.IsNullOrEmpty(Request["PERNO"] as string))
            {
                string PERNO = Decrypt(Request["PERNO"].ToString());
                FillStaffEmergencyContacts(PERNO);
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
            ddlRelationshipType.DataSource = ds.Tables[7];
            ddlRelationshipType.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    

    void FillStaffEmergencyContacts(string PERNO)
    {
        try
        {
            DataTable dt = new DataTable();
            R.GetStaffEmergencyContacts(PERNO, ref dt);
            gvStaffEmergencyContacts.DataSource = dt;
            gvStaffEmergencyContacts.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        ddlRelationshipType.SelectedIndex = -1;
        txtNameOfContactPerson.Text = "";
        txtContactDetails.Text = "";

        gvStaffEmergencyContacts.SelectedIndex = -1;
        for (int i = 0; i <= gvStaffEmergencyContacts.Rows.Count - 1; i++)
        {
            gvStaffEmergencyContacts.Rows[i].BackColor = default(System.Drawing.Color);
            gvStaffEmergencyContacts.Rows[i].ForeColor = default(System.Drawing.Color);
        }
    }
    protected void gvStaffEmergencyContacts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            LinkButton ibD = new LinkButton();
            LinkButton ibe = new LinkButton();
            ibe = (LinkButton)e.Row.FindControl("ibEdit");
            ibD = (LinkButton)e.Row.FindControl("ibDelete");
        }
    }
    protected void gvStaffEmergencyContacts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditEmergencyContact")
            {
                ClearAll();
                gvStaffEmergencyContacts.SelectedIndex = Convert.ToInt16(e.CommandArgument);
                gvStaffEmergencyContacts.SelectedRow.BackColor = Color.LightGray;
                gvStaffEmergencyContacts.SelectedRow.ForeColor = Color.Black;

                txtNameOfContactPerson.Text = gvStaffEmergencyContacts.Rows[Convert.ToInt16(e.CommandArgument)].Cells[1].Text.ToString();
                txtContactDetails.Text = gvStaffEmergencyContacts.Rows[Convert.ToInt16(e.CommandArgument)].Cells[2].Text.ToString();
                ddlRelationshipType.SelectedValue = gvStaffEmergencyContacts.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["RelationshipTypeCode"].ToString();
            }
            if (e.CommandName == "DeleteItem")
            {
                R.DeleteStaffEmergencyContacts(gvStaffEmergencyContacts.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["ContactID"].ToString());
                ClearAll();

                if (!string.IsNullOrEmpty(Request["PERNO"] as string))
                {
                    string PERNO = Decrypt(Request["PERNO"].ToString());
                    DataTable dt = new DataTable();
                    R.GetStaffEmergencyContacts(PERNO, ref dt);
                    gvStaffEmergencyContacts.DataSource = dt;
                    gvStaffEmergencyContacts.DataBind();
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
    protected void gvStaffEmergencyContacts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void ibAdd_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["PERNO"] as string))
        {
            DataTable dtRelationType = new DataTable();
            R.GetRelationshipTypeDescription(ddlRelationshipType.Text.Trim(), ref dtRelationType);
            string RelationshipTypeCode = "";
            foreach (DataRow row in dtRelationType.Rows)
            {
                RelationshipTypeCode = row["LookupsID"].ToString();
            }

            if (dtRelationType.Rows.Count > 0)
            {
                string result = "";
                if (gvStaffEmergencyContacts.SelectedIndex == -1)
                {
                    // DataTable dtTa = new DataTable();
                    DataTable dtDup = new DataTable();
                    R.CheckDuplicatedEmergencyContact(Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue,txtNameOfContactPerson.Text.Trim(), txtContactDetails.Text.Trim(), ref dtDup);

                    if (dtDup.Rows.Count > 0)
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "Staff already has the same Contact";
                    }
                    else
                    {
                        result = R.InsertUpdateStaffEmergencyContact("", Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue,txtNameOfContactPerson.Text.Trim(), txtContactDetails.Text.Trim(), gvStaffEmergencyContacts.Rows.Count + 1);
                        if (result == "1")
                        {
                            ClearAll();
                            FillStaffEmergencyContacts(Decrypt(Request["PERNO"].ToString()));

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

                    R.CheckDuplicatedEmergencyContact(Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue, txtNameOfContactPerson.Text.Trim(), txtContactDetails.Text.Trim(), ref dtDup);

                    if (dtDup.Rows.Count > 0)
                    {
                        PanelMessage.Visible = true;
                        PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                        lblmsg.ForeColor = Color.Red;
                        lblmsg.Text = "You already have the same MR form with the same itinerary";
                    }
                    else
                    {
                        result = R.InsertUpdateStaffEmergencyContact(gvStaffEmergencyContacts.DataKeys[gvStaffEmergencyContacts.SelectedIndex].Values["ContactID"].ToString(), Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue, txtNameOfContactPerson.Text.Trim(), txtContactDetails.Text.Trim(), gvStaffEmergencyContacts.SelectedIndex + 1);
                        if (result == "1")
                        {
                            ClearAll();
                            FillStaffEmergencyContacts(Decrypt(Request["PERNO"].ToString()));

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
            if (ddlRelationshipType.SelectedIndex != 0 || txtContactDetails.Text != "")
            {
                bool invalid = false;
                if (ddlRelationshipType.SelectedIndex == 0)
                {
                    ddlRelationshipType.CssClass = "form-control invalid";
                    invalid = true;
                }
                else
                {
                    ddlRelationshipType.CssClass = "form-control";
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
                    R.GetContactTypeDescription(ddlRelationshipType.Text.Trim(), ref dtLm);
                    string ContactTypeCode = "";
                    foreach (DataRow row in dtLm.Rows)
                    {
                        ContactTypeCode = row["LookupsID"].ToString();
                    }



                    if (dtLm.Rows.Count > 0)
                    {
                        //    lblmsg.Text = "From date can't be smaller than to date from the previos record";
                        string result = "";
                        if (gvStaffEmergencyContacts.SelectedIndex == -1)
                        {
                            DataTable dtTa = new DataTable();
                            DataTable dtDup = new DataTable();

                            R.CheckDuplicatedContact(Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue, txtContactDetails.Text.Trim(), ref dtDup);

                            if (dtDup.Rows.Count > 0)
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "Same contact already exists for this Staff";
                            }
                            else
                            {
                                result = R.InsertUpdateStaffContact("", Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue, txtContactDetails.Text.Trim(), gvStaffEmergencyContacts.Rows.Count + 1);
                                if (result == "1")
                                {
                                    ClearAll();
                                    FillStaffEmergencyContacts(Decrypt(Request["PERNO"].ToString()));

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
                            R.CheckDuplicatedEmergencyContact(Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue,txtNameOfContactPerson.Text.Trim(), txtContactDetails.Text.Trim(), ref dtDup);

                            if (dtDup.Rows.Count > 0)
                            {
                                PanelMessage.Visible = true;
                                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                                lblmsg.ForeColor = Color.Red;
                                lblmsg.Text = "This Contact already exists for same staff";
                            }
                            else
                            {
                                result = R.InsertUpdateStaffEmergencyContact(gvStaffEmergencyContacts.DataKeys[gvStaffEmergencyContacts.SelectedIndex].Values["ContactID"].ToString(), Decrypt(Request["PERNO"].ToString()), ddlRelationshipType.SelectedValue,txtNameOfContactPerson.Text.Trim(), txtContactDetails.Text.Trim(), gvStaffEmergencyContacts.Rows.Count + 1);
                                if (result == "1")
                                {
                                    ClearAll();
                                    FillStaffEmergencyContacts(Decrypt(Request["PERNO"].ToString()));

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
                if (gvStaffEmergencyContacts.Rows.Count > 0)
                {
                    Response.Redirect("~/Staff/SMWizard/5_DownloadStaffInformation.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
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