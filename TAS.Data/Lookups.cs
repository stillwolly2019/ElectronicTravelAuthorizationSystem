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
    public sealed class Lookups
    {
        //Disclaimer
        public IDataReader GetDisclaimer()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.IMtoolsConnectionString, "dbo.GetDisclaimer");
            return Reader;
        }
        //get
        public IDataReader CheckDuplicateLookupGroup(string LookupGroupID, string LookupGroup)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.CheckDuplicateLookupGroup", LookupGroupID, LookupGroup);
            return Reader;
        }
        public IDataReader CheckDuplicateLookups(string LookupsID, string LookupGroupID, string SubGroupID, string Description)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.CheckDuplicateLookups", LookupsID, LookupGroupID, SubGroupID, Description);
            return Reader;
        }
        public IDataReader GetAllLookups()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllLookups");
            return Reader;
        }
        public IDataReader GetAllLookupsGroups()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllLookupsGroups");
            return Reader;
        }

        public DataSet GetAllTripSchemas(bool isinternational)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllTripSchemas", isinternational);
            return ds;
        }
        public DataSet GetAllLookupsList(bool isinternational = false)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllLookupsList",isinternational);
            return ds;
        }

        public DataSet GetZonesByLocationID(string LocationID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetZonesByLocationID", LocationID);
            return ds;
        }

        public DataSet GetUnitsByDepartmentID(string DepartmentID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetUnitsByDepartmentID", DepartmentID);
            return ds;
        }

        public DataSet GetSubUnitsByUnitID(string UnitID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetSubUnitsByUnitID", UnitID);
            return ds;
        }



        public string GetStaffLocation(string PERNO)
        {
            string LocationID = "";
            LocationID = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffLocation]", PERNO).ToString();
            return LocationID;
        }

        



        public DataSet GetLookupsForGroup(string LookupGroup)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetLookupsForGroup", LookupGroup);
            return ds;
        }

        public DataSet GetAllLookupsListForLeaveRequest(string StatusCode)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllLookupsListForLeaveRequest", StatusCode);
            return ds;
        }

        public DataSet GetAllResidenceForSelectedZone(string ZoneID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllResidenceForSelectedZone", ZoneID);
            return ds;
        }

        public IDataReader GetRejectionReason(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetRejectionReason", TravelAuthorizationID);
            return Reader;
        }

        public IDataReader GetCancellationReason(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetCancellationReason", TravelAuthorizationID);
            return Reader;
        }

        public IDataReader GetReturnReasons(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetReturnReasons", TravelAuthorizationID);
            return Reader;
        }

        public IDataReader GetReturnReasons()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetReturnReasons");
            return Reader;
        }

        public IDataReader GetTACancellationReason(string TANo)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTACancellationReason", TANo);
            return Reader;
        }

        public IDataReader GetCancellationReasons()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetCancellationReasons");
            return Reader;
        }

        
        public IDataReader GetCheckList()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetCheckList");
            return Reader;
        }
        public IDataReader GetTECRejectionReason()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetTECRejectionReason");
            return Reader;
        }
        public IDataReader GetTECReturnedRejectionReason()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetTECReturnedRejectionReason");
            return Reader;
        }
        public IDataReader SearchLookups(string LookupGroupID, string SubGroupID, string Code, string Description)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.SearchLookups",LookupGroupID,SubGroupID,Code,Description);
            return Reader;
        }
        public IDataReader SearchLookupGroups(string LookupGroup)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.SearchLookupGroups", LookupGroup);
            return Reader;
        }

        public IDataReader SearchCity(string Prefix)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.SearchCity", Prefix);
            return Reader;
        }

        public IDataReader SearchAccomodations(string Prefix)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.SearchAccomodations", Prefix);
            return Reader;
        }

        

        public IDataReader GetCityByDescription(string CityDescription)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetCityByDescription", CityDescription);
            return Reader;
        }

        public IDataReader GetAccomodationByDescription(string AccomodationDescription)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAccomodationByDescription", AccomodationDescription);
            return Reader;
        }

        public IDataReader GetLeaveCategoryDescription(string Description)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetLeaveCategoryDescription", Description);
            return Reader;
        }

        //delete
        public void DeleteLookups(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.DeleteLookups", LookupsID, CreatedBy);
        }
        public void DeleteLookupsGroups(string LookupGroupID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.DeleteLookupsGroups", LookupGroupID, CreatedBy);
        }
        //insert-update
        public void InsertUpdateLookups(string LookupsID, string LookupGroupID, string SubGroupID, string Code, string Description, string LongDescription, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.InsertUpdateLookups",LookupsID,LookupGroupID,SubGroupID,Code,Description,LongDescription,CreatedBy);
        }
        public void InsertUpdateLookupsGroups(string LookupGroupID, string LookupGroup, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.InsertUpdateLookupsGroups", LookupGroupID, LookupGroup, CreatedBy);
        }

        public IDataReader GetAllDocumentTypes()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllDocumentTypes");
            return Reader;
        }

        public IDataReader GetDocumentTypes()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetDocumentTypes]");
            return Reader;
        }

        public IDataReader GetTripSchemaDocuments(string Code)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetTripSchemaDocuments", Code);
            return Reader;
        }

        public void TripSchemaDocumentToggle(string Code, string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.TripSchemaDocumentToggle", Code, LookupsID, CreatedBy);
        }
    }
}
