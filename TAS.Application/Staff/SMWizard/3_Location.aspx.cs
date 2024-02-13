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

public partial class Staff_SMWizard_3_Location : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LinkButton lnk = new LinkButton();
            lnk = (LinkButton)WizardHeader.FindControl("lbStep3");
            lnk.CssClass = "btn btn-success btn-circle btn-lg  fa fa-sitemap";

            if (!string.IsNullOrEmpty(Request["PERNO"] as string))
            {
                DataTable dt = new DataTable();
                R.GetStaffInformationByPersonalNumber(Decrypt(Request["PERNO"].ToString()), ref dt);
                foreach (DataRow row in dt.Rows)
                {
                    ddlResidenceArea.SelectedValue = row["ResidenceID"].ToString();
                    ddlResidenceType.SelectedValue = row["ResidenceTypeID"].ToString();
                    hdnLocatioID.Value= row["LocationID"].ToString();
                }

            }
            FillDDLs();

        }
    }
    void FillDDLs()
    {
        try
        {
            DataSet dsZ = new DataSet();
            DataSet dsR = new DataSet();
            //Zones
            //R.GetZonesByLocationID(hdnLocatioID.Value,ref dsZ);
            //ddlZone.DataSource = dsZ.Tables[0];
            //ddlZone.DataBind();

            //R.GetAllResidenceForSelectedZone(ddlZone.SelectedValue, ref dsR);
            R.GetAllResidenceForSelectedLocation(hdnLocatioID.Value, ref dsR);
            ddlResidenceArea.DataSource = dsR.Tables[0];
            ddlResidenceArea.DataBind();

            DataSet dsRT = new DataSet();
            R.GetAllLookupsList(ref dsRT);
            ddlResidenceType.DataSource = dsRT.Tables[6];
            ddlResidenceType.DataBind();

            //Enable Disable Select Lists
            //ddlResidenceArea.Enabled = ddlZone.SelectedValue != "00000000-0000-0000-0000-000000000000";
            ddlResidenceType.Enabled = ddlResidenceArea.SelectedValue != "00000000-0000-0000-0000-000000000000";


        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void ClearAll()
    {
        //ddlZone.SelectedIndex = -1;
        ddlResidenceArea.SelectedIndex = -1;
        ddlResidenceType.SelectedIndex = -1;
        //ddlCurrentLocation.SelectedIndex = -1;
        //ddlLocationStatus.SelectedIndex = -1;
    }

    protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
    {
            //DataSet dsR = new DataSet();
            //R.GetAllResidenceForSelectedZone(ddlZone.SelectedValue,ref dsR);
            //ddlResidenceArea.DataSource = dsR.Tables[0];
            //ddlResidenceArea.DataBind();

            DataSet dsRT = new DataSet();
            R.GetAllLookupsList(ref dsRT);
            ddlResidenceType.DataSource = dsRT.Tables[6];
            ddlResidenceType.DataBind();

            //ddlResidenceArea.Enabled = ddlZone.SelectedValue!= "00000000-0000-0000-0000-000000000000";
            ddlResidenceType.Enabled = ddlResidenceArea.SelectedValue!= "00000000-0000-0000-0000-000000000000";

    }

    protected void ddlResidenceArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet dsRT = new DataSet();
        R.GetAllLookupsList(ref dsRT);
        ddlResidenceType.DataSource = dsRT.Tables[6];
        ddlResidenceType.DataBind();

        ddlResidenceType.Enabled = ddlResidenceArea.SelectedValue != "00000000-0000-0000-0000-000000000000";
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //string Zone = string.Empty;
            string ResidenceArea = string.Empty;
            string ResidenceType = string.Empty;

            //Zone = ddlZone.SelectedValue;
            ResidenceArea = ddlResidenceArea.SelectedValue;
            ResidenceType = ddlResidenceType.SelectedValue;

            //if (Zone == "-- Please Select --" || Zone=="")
            //{
            //    PanelMessage.Visible = true;
            //    PanelMessage.CssClass = "alert alert-danger alert-dismissable";
            //    lblmsg.ForeColor = Color.Red;
            //    ddlZone.Style.Add("border-style", "solid !important");
            //    ddlZone.Style.Add("border-color", "#FF0000 !important");
            //    lblmsg.Text = "Please select Zone";
            //    PanelMessage.Focus();
            //    return;
            //}


            if (ResidenceArea == "-- Please Select --" || ResidenceArea == "")
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                ddlResidenceArea.Style.Add("border-style", "solid !important");
                ddlResidenceArea.Style.Add("border-color", "#FF0000 !important");
                lblmsg.Text = "Please select area of residence";
                PanelMessage.Focus();
                return;
            }
            else if (ResidenceType == "-- Please Select --" || ResidenceType=="")
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                ddlResidenceType.Style.Add("border-style", "solid !important");
                ddlResidenceType.Style.Add("border-color", "#FF0000 !important");
                lblmsg.Text = "Please select residence type";
                PanelMessage.Focus();
                return;
            }

            else
            {
                R.InsertUpdateStaffResidence(Decrypt(Request["PERNO"].ToString()), ResidenceArea.Trim(), ResidenceType.Trim());
                Response.Redirect("~/Staff/SMWizard/4_EmergencyContacts.aspx?PERNO=" + Request["PERNO"].ToString() + "&First=1", false);
            }



        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

}
