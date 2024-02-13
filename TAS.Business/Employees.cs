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
    public class Employees
    {
        private Data.Employees daEmployees = new Data.Employees();
        HttpContext context = HttpContext.Current;

        public void GetStaffNumberUnderDepUnitSubUnit(string DepID, string UnitID, string SubUnitID, ref DataTable dt)
        {
            dt.Load(daEmployees.GetStaffNumberUnderDepUnitSubUnit(DepID, UnitID, SubUnitID));
        }

        public void GetStaffsByDepartmentID(string DepID, string LocationID, ref DataTable dt)
        {
            dt.Load(daEmployees.GetStaffsByDepartmentID(DepID, LocationID));
        }

        public void MoveStaffMember(string DepID, string UnitID, string SubUnitID, string UserID, string LocationID)
        {
            daEmployees.MoveStaffMember(DepID, UnitID, SubUnitID, UserID, LocationID);
        }
        public int UpdateStaffInfo(string UserId, string UserName, string DisplayName, string PRISM_Number, string Email, string DepartmentID, string UnitID, string SubUnitID, String MissionID, string LocationID, string Gender, string UNID, string FirstName, string LastName, string Country, string Title, bool IsInternationalStaff)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daEmployees.UpdateStaffInfo(UserId, UserName, DisplayName, PRISM_Number, Email, DepartmentID, UnitID, SubUnitID, MissionID, ui.User_Id, LocationID, Gender, UNID, FirstName, LastName, Country, Title, IsInternationalStaff);
        }

        public int UpdateInsertStaffInfo(string UserId, string UserName, string DisplayName, string PRISM_Number, string UNID, string Email, string Title, string Gender, bool IsInternationalStaff, string CountryID, string MissionID, string LocationID, string DepartmentID, string UnitID, string SubUnitID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daEmployees.UpdateInsertStaffInfo(UserId, UserName, DisplayName, PRISM_Number, UNID, Email, Title, Gender, IsInternationalStaff, CountryID, MissionID, LocationID, DepartmentID, UnitID, SubUnitID, ui.User_Id);
        }

        public void DeleteStaff(string UserId)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daEmployees.DeleteStaff(UserId, ui.User_Id);
        }
        public void UpdateUserIsInternationalByUserID(string UserID, bool IsInternationalStaff)
        {
            daEmployees.UpdateUserIsInternationalByUserID(UserID, IsInternationalStaff);
        }
        public void UpdateUserIsManagerByUserID(string UserID, bool IsManager)
        {
            daEmployees.UpdateUserIsManagerByUserID(UserID, IsManager);
        }


    }
}
