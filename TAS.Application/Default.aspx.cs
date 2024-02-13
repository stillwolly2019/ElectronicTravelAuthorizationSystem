using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WebForms;

public partial class TravelAuthorization_Default : AuthenticatedPageClass
{
    Business.TravelAuthorization TA = new Business.TravelAuthorization();
    Globals g = new Globals();
    Business.Lookups l = new Business.Lookups();
    HttpContext context = HttpContext.Current;
    Business.Users u = new Business.Users();

    protected void Page_Load(object sender, EventArgs e)
    {
        Objects.User ui = (Objects.User)Session["userinfo"];
        GVATAsPendingAction.PreRender += new EventHandler(GVATAsPendingAction_PreRender);
        GVATAsActionedByMe.PreRender += new EventHandler(GVATAsActionedByMe_PreRender);
        GVATAsByMe.PreRender += new EventHandler(GVATAsByMe_PreRender);
        if (!IsPostBack)
        {
            if(ui!=null)
            {
            DataTable dt = new DataTable();
            TA.GetDelegationStatus(ref dt,ui.User_Id);
            if (dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool HasDelegated = row["HasDelegated"].ToString()=="1"?true:false;
                    bool HasBeenDelegated = row["HasBeenDelegated"].ToString()=="1"?true: false;
                    if (HasDelegated && !HasBeenDelegated)
                    {
                        btnAdd.Visible = false;
                    }
                    else
                    {
                        FillGridTAsPendingAction();
                        FillGridTAActionedByMe();
                        FillGridMyTAs();
                        FillDDLs();
                        Clear();
                    }

                }
            }
            }

        }
    }
    void Clear()
    {
        txtTANo.Text = "";
    }
    void GVATAsPendingAction_PreRender(object sender, EventArgs e)
    {
        if (GVATAsPendingAction.Rows.Count > 0)
        {
            GVATAsPendingAction.UseAccessibleHeader = true;
            GVATAsPendingAction.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void GVATAsActionedByMe_PreRender(object sender, EventArgs e)
    {
        if (GVATAsActionedByMe.Rows.Count > 0)
        {
            GVATAsActionedByMe.UseAccessibleHeader = true;
            GVATAsActionedByMe.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
    void GVATAsByMe_PreRender(object sender, EventArgs e)
    {
        if (GVATAsByMe.Rows.Count > 0)
        {
            GVATAsByMe.UseAccessibleHeader = true;
            GVATAsByMe.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

    }
    void FillGridTAsPendingAction()
    {
        try
        {
            DataTable dtp = new DataTable();
            string TANo = "";
            if (txtTANo.Text == "") { TANo = ""; } else { TANo = txtTANo.Text.Trim(); }
            TA.GetDashboardItemsPendingAction(ref dtp, TANo);
            lblGVATAsPendingActionCount.Text = dtp.Rows.Count.ToString();
            GVATAsPendingAction.DataSource = dtp;
            GVATAsPendingAction.DataBind();
            //TAsPending.Visible = Convert.ToInt32(dtp.Rows.Count.ToString())>0;

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillGridTAActionedByMe()
    {
        try
        {
            DataTable dtabm = new DataTable();
            string TANo = "";
            if (txtTANo.Text == "") { TANo = ""; } else { TANo = txtTANo.Text.Trim(); }
            TA.GetDashboardItemsActionedByMe(ref dtabm, TANo);
            lblGVATAsActionedByMe.Text = dtabm.Rows.Count.ToString();
            GVATAsActionedByMe.DataSource = dtabm;
            GVATAsActionedByMe.DataBind();
            //TasActioned.Visible = Convert.ToInt32(dtabm.Rows.Count.ToString())>0;
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillGridMyTAs()
    {
        try
        {
            DataTable dtfm = new DataTable();
            string TANo = "";
            if (txtTANo.Text == "") { TANo = ""; } else { TANo = txtTANo.Text.Trim(); }
            TA.GetDashboardItemsByMe(ref dtfm, TANo);
            lblGVATAsByMe.Text = dtfm.Rows.Count.ToString();
            GVATAsByMe.DataSource = dtfm;
            GVATAsByMe.DataBind();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    void FillDDLs()
    {
        try
        {
            DataTable dt = new DataTable();
            u.GetStaffMembersByDepartmentID(ref dt);
            ddlStaffMembers.DataSource = dt;
            ddlStaffMembers.DataBind();
        }
        catch (Exception ex)
        {

            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }
    protected void GVATAsPendingAction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVATAsPendingAction.PageIndex = e.NewPageIndex;
        FillGridTAsPendingAction();
    }
    protected void GVATAsPendingAction_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VTA")
        {
            Response.Redirect("~/TravelAuthorization/TravelAuthorizationFormWizard.aspx?TANO=" + Encrypt(GVATAsPendingAction.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString()) + "&&TAID=" + Encrypt(GVATAsPendingAction.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString()), false);
        }
    }
    protected void GVATAsPendingAction_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVATAsPendingAction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Objects.User ui = (Objects.User)Session["userinfo"];

            //HiddenField hdnIsTecComplete = new HiddenField();
            //hdnIsTecComplete = (HiddenField)e.Row.FindControl("hdnIsTecComplete");

            //LinkButton lnkPrintTE;
            //lnkPrintTE = (LinkButton)e.Row.FindControl("lnkPrintTE");

            //LinkButton ibDeleteTA;
            //ibDeleteTA = (LinkButton)e.Row.FindControl("ibDeleteTA");
            //string StatusCode = GVMyTAs.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values["StatusCode"].ToString();

            //if (StatusCode == "TA Pending")
            //{
            //    ibDeleteTA.Visible = true;
            //}
            //else
            //{
            //    ibDeleteTA.Visible = false;
            //}

            //if (hdnIsTecComplete.Value.Trim() == "True")
            //{
            //    lnkPrintTE.Visible = true;
            //    ibDeleteTA.Visible = false;
            //}
            //else
            //{
            //    lnkPrintTE.Visible = false;
            //}

        }
    }
    protected void GVATAsActionedByMe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVATAsActionedByMe.PageIndex = e.NewPageIndex;
        FillGridTAActionedByMe();
    }
    protected void GVATAsActionedByMe_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VTA")
        {
            Response.Redirect("~/TravelAuthorization/TravelAuthorizationFormWizard.aspx?TANO=" + Encrypt(GVATAsActionedByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString()) + "&&TAID=" + Encrypt(GVATAsActionedByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString()), false);
        }
    }
    protected void GVATAsActionedByMe_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVATAsActionedByMe_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Objects.User ui = (Objects.User)Session["userinfo"];

            //HiddenField hdnIsTecComplete = new HiddenField();
            //hdnIsTecComplete = (HiddenField)e.Row.FindControl("hdnIsTecComplete");

            //LinkButton lnkPrintTE;
            //lnkPrintTE = (LinkButton)e.Row.FindControl("lnkPrintTE");

            //LinkButton ibDeleteTA;
            //ibDeleteTA = (LinkButton)e.Row.FindControl("ibDeleteTA");
            //string StatusCode = GVMyTAs.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values["StatusCode"].ToString();

            //if (StatusCode == "TA Pending")
            //{
            //    ibDeleteTA.Visible = true;
            //}
            //else
            //{
            //    ibDeleteTA.Visible = false;
            //}

            //if (hdnIsTecComplete.Value.Trim() == "True")
            //{
            //    lnkPrintTE.Visible = true;
            //    ibDeleteTA.Visible = false;
            //}
            //else
            //{
            //    lnkPrintTE.Visible = false;
            //}

        }
    }
    protected void GVATAsByMe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVATAsActionedByMe.PageIndex = e.NewPageIndex;
        FillGridMyTAs();
    }
    protected void GVATAsByMe_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "VTA")
        {
            Response.Redirect("~/TravelAuthorization/TravelAuthorizationFormWizard.aspx?TANO=" + Encrypt(GVATAsByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString()) + "&&TAID=" + Encrypt(GVATAsByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString()), false);
        }
        if (e.CommandName == "PrintTA")
        {

            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/Travel Authorization Form";
            string TravelAuthorizationNumber = GVATAsByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString();
            ReportParameter p = new ReportParameter("TravelAuthorizationNumber", TravelAuthorizationNumber);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.Refresh();
            Export(viewer, "PDF", "Travel Authorizatoin Form");

        }
        if (e.CommandName == "PrintTE")
        {
            ReportViewer viewer = new ReportViewer();
            string ReportMainPath2 = System.Configuration.ConfigurationManager.AppSettings["ReportServerPath"].ToString();
            viewer.ServerReport.ReportServerUrl = new System.Uri(ReportMainPath2);
            viewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "/Travel Expense Claim";
            string TravelAuthorizationNumber = GVATAsByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString();
            ReportParameter p = new ReportParameter("TravelAuthorizationNumber", TravelAuthorizationNumber);
            viewer.ServerReport.SetParameters(new ReportParameter[] { p });
            viewer.ServerReport.Refresh();
            ExportTE(viewer, "PDF", "Travel Expense Claim");

        }

        if (e.CommandName == "DuplicateTA")
        {
            Clear();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            ViewState["TAID"] = GVATAsByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationID"].ToString();
            ViewState["TANO"] = GVATAsByMe.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["TravelAuthorizationNumber"].ToString();
        }
    }
    protected void GVATAsByMe_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GVATAsByMe_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Objects.User ui = (Objects.User)Session["userinfo"];
             LinkButton lnkPrintTE;
            lnkPrintTE = (LinkButton)e.Row.FindControl("lnkPrintTE");

            HiddenField hdnIsTecComplete = new HiddenField();
            hdnIsTecComplete = (HiddenField)e.Row.FindControl("hdnIsTecComplete");

            LinkButton ibPrintTA;
            ibPrintTA = (LinkButton)e.Row.FindControl("lnkPrintTA");

            LinkButton ibDuplicateTA;
            ibDuplicateTA = (LinkButton)e.Row.FindControl("lnkDuplicateTA");

            bool Approved = Convert.ToBoolean(GVATAsByMe.DataKeys[Convert.ToInt32(e.Row.RowIndex)].Values["Approved"].ToString());

            if (Approved == true)
            {
                ibPrintTA.Visible = true;
                ibDuplicateTA.Visible = true;
            }
            else
            {
                ibPrintTA.Visible = false;
                ibDuplicateTA.Visible = false;
            }

            if (hdnIsTecComplete.Value.Trim() == "True")
            {
                lnkPrintTE.Visible = true;
            }
            else
            {
                lnkPrintTE.Visible = false;
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGridTAsPendingAction();
        FillGridTAActionedByMe();
        FillGridMyTAs();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
        FillGridTAsPendingAction();
        FillGridTAActionedByMe();
        FillGridMyTAs();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TravelAuthorization/TravelAuthorizationFormWizard.aspx?First=1", false);
    }
    protected void ibAddStaff_Click(object sender, EventArgs e)
    {
        PanelMessageDuplicateTA.Visible = false;
        lblmsgDuplicateTA.Text = "";
        PanelMessageDuplicateTARejection.Visible = false;
        lblmsgDuplicateTARejection.Text = "";

        List<User> User;
        if (ViewState["Users"] == null)
        {
            User = new List<User>();
        }
        else
        {
            User = (List<User>)ViewState["Users"];
            User = User.Where(item => item.isDeleted == false).ToList();
        }


        User singleUser = new User("", "", false, false);
        bool Exists = false;

        if (User.Count != 0)
        {
            foreach (var item in User)
            {
                if (item.UserID != ddlStaffMembers.SelectedValue)
                {
                    singleUser = new User(ddlStaffMembers.SelectedValue.ToString(), ddlStaffMembers.SelectedItem.Text, false, false);
                }
                else
                {
                    Exists = true;
                    break;
                }
            }
        }
        else
        {
            singleUser = new User(ddlStaffMembers.SelectedValue.ToString(), ddlStaffMembers.SelectedItem.Text, false, false);
        }

        if (singleUser.UserID != "" && Exists != true)
        {
            User.Add(singleUser);
            ddlStaffMembers.SelectedIndex = -1;
            //PanelMessageDuplicateTA.Visible = true;
            //PanelMessageDuplicateTA.CssClass = "alert alert-success alert-dismissable";
            //lblmsgDuplicateTA.Text = "Staff member has been added successfully";
            //lblmsgDuplicateTA.ForeColor = System.Drawing.Color.Green;

        }
        else
        {
            PanelMessageDuplicateTARejection.Visible = true;
            PanelMessageDuplicateTARejection.CssClass = "alert alert-danger alert-dismissable";
            lblmsgDuplicateTARejection.ForeColor = Color.Red;
            lblmsgDuplicateTARejection.Text = "Staff member is already added";
            PanelMessageDuplicateTARejection.Focus();
            return;
        }

        ViewState["Users"] = User;
        User = User.Where(item => item.isDeleted == false).ToList();
        gvStaffMembers.DataSource = User.ToList();
        gvStaffMembers.DataBind();
    }
    protected void btnDuplicate_Click(object sender, EventArgs e)
    {
        if (gvStaffMembers.Rows.Count > 0)
        {
            PanelMessageDuplicateTA.Visible = false;
            lblmsgDuplicateTA.Text = "";
            PanelMessageDuplicateTARejection.Visible = false;
            lblmsgDuplicateTARejection.Text = "";
            List<User> user;
            if (ViewState["Users"] != null)
            {
                user = (List<User>)ViewState["Users"];
                user = user.Where(item => item.isDeleted == false).ToList();
            }
            else
            {
                user = new List<User>();
            }

            DataTable dtDup = new DataTable();
            string NamesOfUsersWithDuplicate = "";
            string NamesOfUsersWithCreated = "";

            foreach (var item in user)
            {
                if (item.isDeleted == false)
                {
                    TA.DuplicateTA(ViewState["TAID"].ToString(), item.UserID);
                }
            }
            FillGridTAsPendingAction();
            FillGridTAActionedByMe();
            FillGridMyTAs();
            foreach (var item in user)
            {
                if (item.isDuplicate && item.isDeleted == false)
                {
                    NamesOfUsersWithDuplicate = NamesOfUsersWithDuplicate + "<br>" + item.FullName;
                }
                else
                {
                    NamesOfUsersWithCreated = NamesOfUsersWithCreated + "<br>" + item.FullName;
                }
            }

            if (NamesOfUsersWithDuplicate != "")
            {
                PanelMessageDuplicateTARejection.Visible = true;
                PanelMessageDuplicateTARejection.CssClass = "alert alert-danger alert-dismissable";
                lblmsgDuplicateTARejection.ForeColor = Color.Red;
                lblmsgDuplicateTARejection.Text = "TA is already created for the following staff members" + NamesOfUsersWithDuplicate;
                PanelMessageDuplicateTARejection.Focus();
            }

            if (NamesOfUsersWithCreated != "")
            {
                PanelMessageDuplicateTA.Visible = true;
                PanelMessageDuplicateTA.CssClass = "alert alert-success alert-dismissable";
                lblmsgDuplicateTA.Text = "TA has been created successfully for the following staff members" + NamesOfUsersWithCreated;
                lblmsgDuplicateTA.ForeColor = System.Drawing.Color.Green;
            }
        }
        else
        {
            PanelMessageDuplicateTARejection.Visible = true;
            PanelMessageDuplicateTARejection.CssClass = "alert alert-danger alert-dismissable";
            lblmsgDuplicateTARejection.ForeColor = Color.Red;
            lblmsgDuplicateTARejection.Text = "Press \"+ click to save\" button to add staff member to the list.";
            PanelMessageDuplicateTARejection.Focus();
        }
    }
    public bool Export(ReportViewer viewer, string exportType, string reportsTitle)
    {
        try
        {
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;

            filetype = "Pdf";
            byte[] bytes = viewer.ServerReport.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "MyTA.pdf");
            Response.Flush();
            Response.BinaryWrite(bytes);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
        return true;
    }
    public bool ExportTE(ReportViewer viewer, string exportType, string reportsTitle)
    {
        try
        {
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string filetype = string.Empty;
            filetype = "PDF";
            byte[] bytes = viewer.ServerReport.Render(filetype, null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "xls";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + "TEC.Pdf");
            Response.Flush();
            Response.BinaryWrite(bytes);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
        return true;
    }

    protected void gvStaffMembers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton ibDeleteStaff;
            ibDeleteStaff = (LinkButton)e.Row.FindControl("ibDeleteStaff");
        }
    }
    protected void gvStaffMembers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            PanelMessageDuplicateTA.Visible = false;
            lblmsgDuplicateTA.Text = "";
            PanelMessageDuplicateTARejection.Visible = false;
            lblmsgDuplicateTARejection.Text = "";

            List<User> user = (List<User>)ViewState["Users"];

            var itemToRemove = user.Single(r => r.UserID == gvStaffMembers.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["UserID"].ToString());
            itemToRemove.isDeleted = true;
            ViewState["Users"] = user;

            user = user.Where(item => item.isDeleted == false).ToList();
            gvStaffMembers.DataSource = user.ToList();
            gvStaffMembers.DataBind();

            ddlStaffMembers.SelectedIndex = -1;

            PanelMessageDuplicateTA.Visible = true;
            PanelMessageDuplicateTA.CssClass = "alert alert-success alert-dismissable";
            lblmsgDuplicateTA.Text = "Staff member has been deleted successfully";
            lblmsgDuplicateTA.ForeColor = System.Drawing.Color.Green;
        }
        if (e.CommandName == "DuplicateTA")
        {
            //PanelMessageDuplicateTA.Visible = false;
            //lblmsgDuplicateTA.Text = "";
            //PanelMessageDuplicateTARejection.Visible = false;
            //lblmsgDuplicateTARejection.Text = "";

            //List<User> user = (List<User>)ViewState["Users"];

            //var itemToRemove = user.Single(r => r.UserID == gvStaffMembers.DataKeys[Convert.ToInt16(e.CommandArgument)].Values["UserID"].ToString());
            //itemToRemove.isDeleted = true;
            //ViewState["Users"] = user;

            //user = user.Where(item => item.isDeleted == false).ToList();
            //gvStaffMembers.DataSource = user.ToList();
            //gvStaffMembers.DataBind();

            //ddlStaffMembers.SelectedIndex = -1;

            //PanelMessageDuplicateTA.Visible = true;
            //PanelMessageDuplicateTA.CssClass = "alert alert-success alert-dismissable";
            //lblmsgDuplicateTA.Text = "Staff member has been deleted successfully";
            //lblmsgDuplicateTA.ForeColor = System.Drawing.Color.Green;
        }
    }
    protected void gvStaffMembers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
}