using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using TravelAuthorizationSystem.Utility;


namespace Data
{
    public sealed class AMS
    {
        public void InsertManagerAssignment(string StaffID, string PrismNo, string DepID, string UnitID, string SubUnitID, string CreatedBy)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.AMSConnectionString, "Lkp.InsertManagerAssignment", StaffID, PrismNo, DepID, UnitID, SubUnitID, CreatedBy);
        }

        public void DeleteManagerAssignmentByPRISMNumber(string PRISMNumber, string DepID, string UnitID, string SubUnitID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.AMSConnectionString, "Lkp.DeleteManagerAssignmentByPRISMNumber", PRISMNumber, DepID, UnitID, SubUnitID, CreatedBy);
        }

        public void UpdateEmployeeInfoIsInternationalStaffByPRISMNumber(string PRISMNumber, bool IsInternationalStaff)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.AMSConnectionString, "Att.UpdateEmployeeInfoIsInternationalStaffByPRISMNumber", PRISMNumber, IsInternationalStaff);
        }

        public void MoveStaffMemberByPRISMNumber(string DepartmentID, string UnitID, string SubUnit, string PRISMNumber)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.AMSConnectionString, "Att.MoveStaffMemberByPRISMNumber", DepartmentID, UnitID, SubUnit, PRISMNumber);
        }
    }
}
