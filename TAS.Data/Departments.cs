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
    public sealed class Departments
    {
        public IDataReader GetAllDepartment()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllDepartments");
            return Reader;
        }

        public IDataReader GetAllCountries()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllCountries");
            return Reader;
        }

        public IDataReader GetAllDepartmentByMissionID(string MissionID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllDepartmentByMissionID", MissionID);
            return Reader;
        }

        public void DeleteDepartment(string DepartmentID, string ModifiedBy)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.DeleteDepartment", DepartmentID, ModifiedBy);
        }

        public int InsertDepartments(string DepartmentID, string DepartmentName, string MissionID, string ActionBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.InsertDepartments", DepartmentID, MissionID, DepartmentName, ActionBy).ToString());
        }

    }
}
