using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using TravelAuthorizationSystem.Utility;


namespace Data
{
    public class Employees
    {
        public IDataReader GetStaffNumberUnderDepUnitSubUnit(string DepID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(Configuration.ConnectionString, "Lkp.GetStaffNumberUnderDepUnitSubUnit", DepID, UnitID, SubUnitID);
            return Reader;
        }

        public IDataReader GetStaffsByDepartmentID(string DepID, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(Configuration.ConnectionString, "Lkp.GetStaffsByDepartmentID", DepID, LocationID);
            return Reader;
        }

        public void MoveStaffMember(string DepID, string UnitID, string SubUnitID, string UserID, string LocationID)
        {
            daHelper.ExecuteScalar(Configuration.ConnectionString, "Lkp.MoveStaffMember", DepID, UnitID, SubUnitID, UserID, LocationID).ToString();
        }
        public int UpdateStaffInfo(string UserId, string UserName, string DisplayName, string PRISM_Number, string Email, string DepartmentID, string UnitID, string SubUnitID, String MissionID, string ModifiedBy, string LocationID, string Gender, string UNID, string FirstName, string LastName, string Country, string Title, bool IsInternationalStaff)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.UpdateInsertStaffInfo", UserId, UserName, DisplayName, PRISM_Number, Email, DepartmentID, UnitID, SubUnitID, MissionID, ModifiedBy, LocationID, Gender, UNID, FirstName, LastName, Country, Title, IsInternationalStaff).ToString());
        }

        public int UpdateInsertStaffInfo(string UserId, string UserName, string DisplayName, string PRISM_Number, string UNID, string Email, string Title, string Gender, bool IsInternationalStaff, string CountryID, string MissionID, string LocationID, string DepartmentID, string UnitID, string SubUnitID, string ModifiedBy)
        {
            return int.Parse(daHelper.ExecuteScalar(Configuration.ConnectionString, "Lkp.UpdateInsertStaffInfo",
            UserId, UserName, DisplayName, PRISM_Number,UNID, Email, Title,Gender, IsInternationalStaff, CountryID,MissionID,LocationID, DepartmentID, UnitID, SubUnitID, ModifiedBy).ToString());
        }

        public void DeleteStaff(string UserId, string ModifiedBy)
        {
            daHelper.ExecuteScalar(Configuration.ConnectionString, "Lkp.DeleteStaff", UserId, ModifiedBy).ToString();
        }

        public void UpdateUserIsInternationalByUserID(string UserID, bool IsInternationalStaff)
        {
            daHelper.ExecuteScalar(Configuration.ConnectionString, "Sec.UpdateUserIsInternationalByUserID", UserID, IsInternationalStaff);
        }

        public void UpdateUserIsManagerByUserID(string UserID, bool IsManager)
        {
            daHelper.ExecuteScalar(Configuration.ConnectionString, "Sec.UpdateUserIsManagerByUserID", UserID, IsManager);
        }

    }

}
