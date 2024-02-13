using Microsoft.CSharp;
using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace Business
{
    public class Security
    {
        private Data.Security daSecurity = new Data.Security();
        HttpContext context = HttpContext.Current;

        public int ADLogin(string loginame, bool FromButton, Objects.User.eUserType UserType = Objects.User.eUserType.PrivateUser)
        {

            int retval = -1;
            Objects.User user = null;
            HttpContext.Current.Session["userinfo"] = null;
            DataTable dt = new DataTable();
            if (loginame == null)
            {
                context.Response.Redirect("~/login.aspx", false);
            }
            else
            {
                dt.Load(daSecurity.ADLogin(loginame));
                if ((dt != null) && dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsLoggedIn"]) | FromButton)
                    {
                        user = new Objects.User();
                        user.UserName = dt.Rows[0]["Username"].ToString();
                        user.LoginName = dt.Rows[0]["username"].ToString();
                        user.FirstName = dt.Rows[0]["FirstName"].ToString();
                        user.LastName = dt.Rows[0]["LastName"].ToString();
                        user.User_Id = dt.Rows[0]["UserID"].ToString();
                        user.Email = dt.Rows[0]["Email"].ToString();
                        user.DutyStation = dt.Rows[0]["DutyStation"].ToString();
                        user.IsStaffMember = Convert.ToBoolean(dt.Rows[0]["IsStaffMember"]);
                        user.IsSubStaffMember = Convert.ToBoolean(dt.Rows[0]["IsSubStaffMember"]);
                        user.IsSubSupervisor = Convert.ToBoolean(dt.Rows[0]["IsSubSupervisor"]);
                        user.IsManager = Convert.ToBoolean(dt.Rows[0]["IsManager"]);
                        user.IsAdmin = Convert.ToBoolean(dt.Rows[0]["IsHRAdmin"]);
                        user.IsFinAdmin = Convert.ToBoolean(dt.Rows[0]["IsFinAdmin"]);
                        user.IsSupervisor = Convert.ToBoolean(dt.Rows[0]["IsSupervisor"]);
                        user.IsSecReqVerifier = Convert.ToBoolean(dt.Rows[0]["IsSecReqVerifier"]);
                        user.IsHRAttendancePersonnel = Convert.ToBoolean(dt.Rows[0]["IsHRAttendancePersonnel"]);
                        user.IsHOSO = Convert.ToBoolean(dt.Rows[0]["IsHOSO"]);
                        user.IsHOO = Convert.ToBoolean(dt.Rows[0]["IsHOO"]);
                        user.IsCOM = Convert.ToBoolean(dt.Rows[0]["IsCOM"]);
                        user.IsRMO = Convert.ToBoolean(dt.Rows[0]["IsRMO"]);
                        //user.IsLeaveApprover = Convert.ToBoolean(dt.Rows[0]["IsLeaveApprover"]);
                        user.IsRadioOperator = Convert.ToBoolean(dt.Rows[0]["IsRadioOperator"]);
                        user.IsSystAdmin = Convert.ToBoolean(dt.Rows[0]["IsSystAdmin"]);
                        user.IsNationalStaff = Convert.ToBoolean(dt.Rows[0]["IsNationalStaff"]);
                        user.IsInternationalStaff = Convert.ToBoolean(dt.Rows[0]["IsInternationalStaff"]);
                        user.IsRegionalDirector = Convert.ToBoolean(dt.Rows[0]["IsRegionalDirector"]);
                        //user.IsSupport = Convert.ToBoolean(dt.Rows[0]["IsSupport"]);
                        //user.IsOperations = Convert.ToBoolean(dt.Rows[0]["IsOperations"]);
                        user.IsSupervisorManager = Convert.ToBoolean(dt.Rows[0]["IsSupervisorManager"]);
                        user.IsEmergencyTACreator = Convert.ToBoolean(dt.Rows[0]["IsEmergencyTACreator"]);
                        user.HasDelegated = Convert.ToBoolean(dt.Rows[0]["HasDelegated"]);
                        user.HasBeenDelegated = Convert.ToBoolean(dt.Rows[0]["HasBeenDelegated"]);
                        user.LocationID = dt.Rows[0]["LocationID"].ToString();
                        user.DepartmentID = dt.Rows[0]["DepartmentID"].ToString();
                        user.UnitID = dt.Rows[0]["UnitID"].ToString();
                        user.SubUnitID = dt.Rows[0]["SubUnitID"].ToString();
                        user.PRISMNumber = dt.Rows[0]["PRISMNumber"].ToString();
                        retval = 1;
                    }
                    else
                    {
                        retval = -1;
                    }
                }
                if (retval == 1)
                {
                    context.Session["userinfo"] = user;
                    retval = 1;
                }
                else
                {
                    context.Session["userinfo"] = null;
                    retval = -1;
                }

            }

            return retval;

        }

        public bool IsValidUser(string UserID, string Pwd)
        {
            DataTable dt = new DataTable();
            dt.Load(daSecurity.IsValidUser(UserID, Pwd));
            return dt.Rows.Count > 0;
        }

        public void getPagePermissions(string page_url, ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];

            string approot = ConfigurationManager.AppSettings["application.root"];
            page_url = page_url.ToLower().Replace(approot.ToLower(), "");
            getPagePermissions(page_url.Remove(0, 1), ref dt, ref ui);
        }

        public void getPagePermissions(string page_url, ref DataTable dt, ref Objects.User ui)
        {
            dt.Load(daSecurity.getPagePermissions(ui.UserName, page_url));

            if (dt.Rows.Count > 0)
            {
                bool canRead = false;
                bool canEdit = false;
                bool canAdd = false;
                bool canDelete = false;
                bool canAmend = false;
                canRead = false;
                canEdit = false;
                canAdd = false;
                canDelete = false;
                canAmend = false;

                foreach (DataRow dr in dt.Rows)
                {
                    canRead = canRead | (Convert.ToBoolean(dr["Read"]));
                    canAdd = canAdd | (Convert.ToBoolean(dr["Add"]));
                    canEdit = canEdit | (Convert.ToBoolean(dr["Edit"]));
                    canDelete = canDelete | (Convert.ToBoolean(dr["Delete"]));
                    canAmend = canAmend | (Convert.ToBoolean(dr["Amend"]));
                }
                dt.Rows.Clear();

                DataRow dr1 = dt.NewRow();
                dr1["Read"] = canRead;
                dr1["Add"] = canAdd;
                dr1["Edit"] = canEdit;
                dr1["Delete"] = canDelete;
                dr1["Amend"] = canAmend;
                dt.Rows.Add(dr1);
            }
        }

        public void GetUserMenu(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daSecurity.GetUserMenu(ui.User_Id));
        }

        public void GetUserInfoByUserID(string UserID, ref DataTable dt)
        {
            dt.Load(daSecurity.GetUserInfoByUserID(UserID));
        }

        public void GetTAInformationByTANO(string TANo, ref DataTable dt)
        {
            dt.Load(daSecurity.GetTAInformationByTANO(TANo));
        }

        public void GetMRInformationByMRNO(string MRNo, ref DataTable dt)
        {
            dt.Load(daSecurity.GetMRInformationByMRNO(MRNo));
        }

        public void GetRoleNameByUserID(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daSecurity.GetRoleNameByUserID(ui.User_Id));
        }

        public void ADSingleSignOn()
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSecurity.ADSingleSignOn(ui.UserName);
        }

        public void ADLogOut()
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSecurity.ADLogOut(ui.UserName);
        }

        public void GetAllLocations(ref DataTable dt)
        {
            dt.Load(daSecurity.GetAllLocations());
        }
    }
}
