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
    public sealed class UserLocations
    {
        private object ValueOrDBNullIfZero(int val)
        {
            if (val == 0) return DBNull.Value;
            return val;
        }

        public DataSet GetUserAccessLookupsList(string UserID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetUserAccessLookupsList", UserID);
            return ds;
        }

        public IDataReader GetUserRolesAcceses(string UserID, string RoleID, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetUserRolesAcceses", UserID, RoleID, LocationID);
            return Reader;
        }

        public IDataReader GetWardenZoneAcceses(string UserID, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetWardenZoneAcceses", UserID, LocationID);
            return Reader;
        }

        public IDataReader GetRadioOperatorLocationsAcceses(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetRadioOperatorLocationsAcceses", UserID);
            return Reader;
        }
        

        public int InsertUserLocations(string UserID, string RoleID, string LocationID, string DepartmentID, string UnitID, string SubUnitID, string CreatedBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.InsertUserLocations", UserID, RoleID, LocationID, DepartmentID, UnitID, SubUnitID, CreatedBy).ToString());
        }

        public int InsertWardenZoneAccess(string UserID, string RoleID, string LocationID, string ZoneID, string CreatedBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.InsertWardenZoneAccess", UserID,RoleID, LocationID, ZoneID, CreatedBy).ToString());
        }

        public int InsertRadioOperatorLocationsAccess(string UserID, string LocationID, string CreatedBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.InsertRadioOperatorLocationsAccess", UserID, LocationID, CreatedBy).ToString());
        }

        public void DeleteWardenAccess(int ID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.DeleteWardenAccess", ID);
        }

        public void DeleteRadioOperatorAccess(int ID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.DeleteRadioOperatorAccess", ID);
        }
        

        public void DeleteUserRoleAccess(int ID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteUserRoleAccess", ID);
        }

        public DataSet GetUnitsByDepartmentID(string DepartmentID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "rc.GetUnitsByDepartmentID", DepartmentID);
            return ds;
        }

        public DataSet GetZonesByLocationID(string LocationID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetZonesByLocationID", LocationID);
            return ds;
        }

        

        public DataSet GetSubUnitsByUnitID(string UnitID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "rc.GetSubUnitsByUnitID", UnitID);
            return ds;
        }


        public IDataReader GetAllLocations()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetAllLocations");
            return Reader;
        }

        public IDataReader GetUserAccessByLocationID(string LocationID,string UserName="")
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetUserAccessByLocationID", LocationID, UserName);
            return Reader;
        }

        public IDataReader SearchUserAccess(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[TA].[SearchUserAccess]", SearchText);
            return Reader;
        }

        public string ToggleUserAccess(string uid, string rid, string did, string unid, string sunid)
        {
            string Result = "";
            Result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.[ToggleUserAccess]", uid, rid, did, unid, sunid).ToString();
            return Result;
        }

        public IDataReader GetAllDepartments()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[TA].[GetAllDepartments]");
            return Reader;
        }

        public IDataReader GetAllUnitsByDepartmentID(string DepartmentID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetAllUnitsByDepartmentID", DepartmentID);
            return Reader;
        }



    }
}
