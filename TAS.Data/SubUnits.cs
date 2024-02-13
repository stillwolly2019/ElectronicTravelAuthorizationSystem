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
    public sealed class SubUnits
    {
        public IDataReader GetAllSubUnits()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllSubUnits");
            return Reader;
        }

        public void InsertUpdateSubUnits(int ID, int UnitID, int DepartmentID, string SubUnitName)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.InsertSubUnit", ID, UnitID, DepartmentID, SubUnitName);
        }

        public int InsertSubUnits(string SubUnitID, string UnitID, string DepartmentID, string MissionID, string SubUnitName, string ActionBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.InsertSubUnits", SubUnitID, UnitID, DepartmentID, MissionID, SubUnitName, ActionBy).ToString());
        }

        public void DeleteSubUnit(string SubUnitID, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.DeleteSubUnit", SubUnitID, ModifiedBy);
        }

        public IDataReader GetAllSubUnitByUnitID(string UnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllSubUnitByUnitID", UnitID);
            return Reader;
        }


    }
}
