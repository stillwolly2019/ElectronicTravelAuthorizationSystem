using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;

namespace Business
{
    public class UserLocations
    {
        private Data.UserLocations userLocs = new Data.UserLocations();
        HttpContext context = HttpContext.Current;

        public void GetUserAccessLookupsList(string UserID,ref DataSet ds)
        {
            ds = userLocs.GetUserAccessLookupsList(UserID);
        }
        public void GetUserRolesAcceses(string UserID, string RoleID, string LocationID, ref DataTable dt)
        {
            dt.Load(userLocs.GetUserRolesAcceses(UserID, RoleID, LocationID));
        }

        public void GetWardenZoneAcceses(string UserID, string LocationID, ref DataTable dt)
        {
            dt.Load(userLocs.GetWardenZoneAcceses(UserID, LocationID));
        }

        public void GetRadioOperatorLocationsAcceses(string UserID, ref DataTable dt)
        {
            dt.Load(userLocs.GetRadioOperatorLocationsAcceses(UserID));
        }

        public int InsertUserLocations(string UserID, string RoleID, string LocationID, string DepartmentID, string UnitID, String SubUnitID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return userLocs.InsertUserLocations(UserID, RoleID, LocationID, DepartmentID, UnitID, SubUnitID, ui.User_Id);
        }

        public int InsertWardenZoneAccess(string UserID,string RoleID, string LocationID, string ZoneID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return userLocs.InsertWardenZoneAccess(UserID,RoleID, LocationID, ZoneID, ui.User_Id);
        }

        public int InsertRadioOperatorLocationsAccess(string UserID, string LocationID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return userLocs.InsertRadioOperatorLocationsAccess(UserID, LocationID, ui.User_Id);
        }

        public void GetUnitsByDepartmentID(string DepartmentID, ref DataSet ds)
        {
            ds = userLocs.GetUnitsByDepartmentID(DepartmentID);
        }

        public void GetZonesByLocationID(string LocationID, ref DataSet ds)
        {
            ds = userLocs.GetZonesByLocationID(LocationID);
        }

        public void GetSubUnitsByUnitID(string UnitID, ref DataSet ds)
        {
            ds = userLocs.GetSubUnitsByUnitID(UnitID);
        }

        public void DeleteUserRoleAccess(int ID)
        {
            userLocs.DeleteUserRoleAccess(ID);
        }

        public void DeleteWardenAccess(int ID)
        {
            userLocs.DeleteWardenAccess(ID);
        }

        public void DeleteRadioOperatorAccess(int ID)
        {
            userLocs.DeleteRadioOperatorAccess(ID);
        }

        public void GetAllLocations(ref DataTable dt)
        {
            dt.Load(userLocs.GetAllLocations());
        }

        public void GetUserAccessByLocationID(string LocationID, string UserName, ref DataTable dt)
        {
            dt.Load(userLocs.GetUserAccessByLocationID(LocationID, UserName));
        }

        public void SearchUserAccess(string SearchText, ref DataTable dt)
        {
            dt.Load(userLocs.SearchUserAccess(SearchText));
        }

        public string ToggleUserAccess(string uid,string rid,string did,string unid,string sunid)
        {
            string Result = "";
            Result = userLocs.ToggleUserAccess(uid,rid,did,unid,sunid).ToString();
            return Result;
        }

        public void GetAllDepartments(ref DataTable dt)
        {
            dt.Load(userLocs.GetAllDepartments());
        }

        public void GetAllUnitsbyDepartmentID(string DepartmentID, ref DataTable dt)
        {
            dt.Load(userLocs.GetAllUnitsByDepartmentID(DepartmentID));
        }



    }
}
