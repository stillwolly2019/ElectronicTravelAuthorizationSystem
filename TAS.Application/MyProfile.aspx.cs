using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
public partial class MyProfile : AuthenticatedPageClass
{
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    HttpContext context = HttpContext.Current;
    protected void Page_Load(object sender, EventArgs e)
    {
        Objects.User ui = (Objects.User)context.Session["userinfo"];
        if (ui==null)
        {
            Response.Redirect("~/Login.aspx?RetURL=~/Admin/MyProfile.aspx");
        }
        if (!IsPostBack)
        {
            ClearAll();
            FillDDLs();
            GetProfileInformation();
        }
    }
    public void ClearAll()
    {
        txtUserName.Text = "";
        txtDisplayName.Text = "";
        txtPRISMNumber.Text = "";
        txtCallSign.Text = "";
        txtDutyStation.Text = "";
        txtUnitDepartment.Text = "";
        txtUNIDNo.Text = "";
        txtTitle.Text = "";
        hdnResidenceID.Value = "";
        txtResidenceName.Text = "";
        ddlCountry.SelectedIndex = -1;
        ddlGender.SelectedIndex = -1;
    }
    public void GetProfileInformation()
    {
        DataTable dt = new DataTable();
        R.GetProfileInformation(ref dt);
        foreach (DataRow row in dt.Rows)
        {
            txtUserName.Text = row["UserName"].ToString();
            txtPRISMNumber.Text = row["PRISM_Number"].ToString();
            txtDisplayName.Text = row["DisplayName"].ToString();
            txtDutyStation.Text = row["DutyStation"].ToString();
            txtUnitDepartment.Text = row["UnitDepartment"].ToString();
            ddlGender.SelectedValue = row["GenderID"].ToString();
            txtCallSign.Text = row["CallSign"].ToString();
            ddlCountry.SelectedValue = row["CountryID"].ToString();
            txtResidenceName.Text = row["ResidenceName"].ToString();
            hdnResidenceID.Value = row["ResidenceID"].ToString();
            txtTitle.Text = row["Title"].ToString();
            txtUNIDNo.Text = row["UNID"].ToString();
            txtPRISMNumber.Enabled = false;
            txtDisplayName.Enabled = false;
            txtDutyStation.Enabled = false;
            txtUnitDepartment.Enabled = false;
            txtUserName.Enabled = false;
            txtPRISMNumber.Enabled = false;
        }
    }
    void FillDDLs()
    {
        try
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            DataSet ds = new DataSet();
            R.GetAllLookupsList(ref ds);
            ddlCountry.DataSource = ds.Tables[0];
            ddlCountry.DataBind();

            R.GetAllLookupsList(ref ds);
            ddlGender.DataSource = ds.Tables[10];
            ddlGender.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void txtResidenceName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(txtResidenceName.Text))
            {
                DataTable dt = new DataTable();
                R.SearchResidences(txtResidenceName.Text.Trim(), ref dt);
                if (dt.Rows.Count > 0)
                {
                    txtResidenceName.Text = dt.Rows[0]["ResidenceName"].ToString();
                    hdnResidenceID.Value = dt.Rows[0]["ResidenceID"].ToString();
                }
            }
            else
            {
                txtResidenceName.Text = "";
                hdnResidenceID.Value = "";
            }
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(hdnResidenceID.Value) || hdnResidenceID.Value== "00000000-0000-0000-0000-000000000000")
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Please Select a Residence from the list";
                ClearAll();
                GetProfileInformation();
                return;
            }
            DataTable dt = new DataTable();
            bool success = false;
            success = R.UpdateMyProfile(txtUserName.Text, txtDisplayName.Text, ddlGender.SelectedValue,txtTitle.Text,txtUNIDNo.Text, hdnResidenceID.Value, ddlCountry.SelectedValue);

            if (success)
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-success alert-dismissable";
                lblmsg.ForeColor = Color.Green;
                lblmsg.Text = "Profile Updated Successfully";
                ClearAll();
                GetProfileInformation();
            }
            else
            {
                PanelMessage.Visible = true;
                PanelMessage.CssClass = "alert alert-danger alert-dismissable";
                lblmsg.ForeColor = Color.Red;
                lblmsg.Text = "Failed to Save Information. Please contact your administrator";
            }

        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }


}