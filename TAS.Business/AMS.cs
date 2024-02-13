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
    public class AMS
    {
        private Data.AMS daEmployees = new Data.AMS();
        HttpContext context = HttpContext.Current;

        public void InsertManagerAssignment(string StaffID, string PrismNo, string DepID, string UnitID, string SubUnitID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daEmployees.InsertManagerAssignment(StaffID, PrismNo, DepID, UnitID, SubUnitID, ui.User_Id);
        }

        public void DeleteManagerAssignmentByPRISMNumber(string PRISMNumber, string DepID, string UnitID, string SubUnitID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daEmployees.DeleteManagerAssignmentByPRISMNumber(PRISMNumber, DepID, UnitID, SubUnitID, ui.User_Id);
        }

        public void UpdateEmployeeInfoIsInternationalStaffByPRISMNumber(string PRISMNumber, bool IsInternationalStaff)
        {
            daEmployees.UpdateEmployeeInfoIsInternationalStaffByPRISMNumber(PRISMNumber, IsInternationalStaff);
        }

        public void MoveStaffMemberByPRISMNumber(string DepID, string UnitID, string SubUnitID, string PrismNo)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daEmployees.MoveStaffMemberByPRISMNumber(DepID, UnitID, SubUnitID, PrismNo);
        }
    }
}
