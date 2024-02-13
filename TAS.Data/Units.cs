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
    public sealed class Units
    {

        public IDataReader GetAllUnits()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllUnits");
            return Reader;
        }

        public IDataReader GetAllUnitsbyDepID(string DepartmentID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllUnitsbyDepID", DepartmentID);
            return Reader;
        }

        public void InsertUpdateUnits(int PID, string UnitName, int DepartmentID, string ActionBy)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.InsertUnit", PID, UnitName, DepartmentID, ActionBy);
        }

        public int InsertUnits(string UnitID, string DepartmentID, string MissionID, string UnitName, string ActionBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.InsertUnits", UnitID, DepartmentID, MissionID, UnitName, ActionBy).ToString());
        }

        public void DeleteUnit(string UnitID, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.DeleteUnit", UnitID, ModifiedBy);
        }


    }
}
