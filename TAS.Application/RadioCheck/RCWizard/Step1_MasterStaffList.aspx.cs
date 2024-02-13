﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RadioCheck_Step1_MasterStaffList : AuthenticatedPageClass
{
    //Business.Lookups l = new Business.Lookups();
    RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
    //Business.Users u = new Business.Users();
    //Business.Security Sec = new Business.Security();
    //Globals g = new Globals();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GVMyRCs.PreRender += new EventHandler(GVMyRCs_PreRender);
            if (!IsPostBack)
            {
                FillGrid();
            }
        }

    }

    void GVMyRCs_PreRender(object sender, EventArgs e)
    {
        if (GVMyRCs.Rows.Count > 0)
        {
            GVMyRCs.UseAccessibleHeader = true;
            GVMyRCs.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            R.GetMasterStaffList(ref dt,Decrypt(Request["loc"]).ToString());
            lblGVRCsCount.Text = dt.Rows.Count.ToString();
            GVMyRCs.DataSource = dt;
            GVMyRCs.DataBind();
            NumberOfStaff.Text = " - "+dt.Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            IOM.Common.Logging.Log.WriteError(ex, System.Configuration.ConfigurationManager.AppSettings["ProjectID"].ToString());
        }
    }

   

}