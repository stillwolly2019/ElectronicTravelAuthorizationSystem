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
    public sealed class TAWorkFlow
    {

        #region WorkFlow

        public IDataReader GetAllStatuses()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetAllStatuses");
            return Reader;
        }


        public IDataReader GetAllSteps()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "sec.GetAllSteps");
            return Reader;
        }
        public IDataReader GetAllTripSchemas()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.GetAllTripSchemas");
            return Reader;
        }

        public void InsertUpdateSteps(int StepID, string StepName,string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "sec.InsertUpdateSteps",
                StepID, StepName, CreatedBy);
        }

        public int CheckDuplicateSteps(string StepID, string StepName)
        {
            return (int)daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "sec.CheckDuplicateSteps",
                 StepID, StepName);

        }

        public void DeleteSteps(string StepID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteSteps",
                StepID);
        }


        /*Permissions*/

        public IDataReader GetRoleStatuses(string RoleID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetRoleStatuses", RoleID);
            return Reader;

        }
        public IDataReader GetRoleTripSchemas(string RoleID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetRoleTripSchemas", RoleID);
            return Reader;

        }

        public IDataReader GetRejectionReasonsByStatusCode(string StatusCode)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetRejectionReasonsByStatusCode", StatusCode);
            return Reader;

        }

        public IDataReader GetCancellationReasonsByStatusCode(string StatusCode)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetCancellationReasonsByStatusCode", StatusCode);
            return Reader;

        }

        public IDataReader GetEmergencyTripSchemas()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetEmergencyTripSchemas");
            return Reader;
        }

        public IDataReader GetTripSchemas()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetTripSchemas");
            return Reader;
        }

        public IDataReader GetReturnableTAStatusCodes()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetReturnableTAStatusCodes");
            return Reader;
        }

        public IDataReader GetCancellableTAStatusCodes()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetCancellableTAStatusCodes");
            return Reader;
        }

        public IDataReader GetTAStatusCodes()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetTAStatusCodes");
            return Reader;
        }

        public IDataReader GetDelegatableStatusCodes()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetDelegatableStatusCodes");
            return Reader;
        }

        public void RoleAccessToStatusesToggle(string RoleID, string StatusID,string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.RoleAccessToStatusesToggle", RoleID, StatusID, CreatedBy);
        }
        public void RoleAccessToTripSchemaToggle(string RoleID, string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.RoleAccessToTripSchemaToggle",RoleID, LookupsID, CreatedBy);
        }

        public void StatusCodeRejectionReasonsToggle(string StatusCode, string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.StatusCodeRejectionReasonsToggle", StatusCode, LookupsID, CreatedBy);
        }

        public void StatusCodeCancellationReasonsToggle(string StatusCode, string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.StatusCodeCancellationReasonsToggle", StatusCode, LookupsID, CreatedBy);
        }

        public void EmergencyTripSchemaToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.EmergencyTripSchemaToggle",LookupsID, CreatedBy);
        }

        public void LeaveTripSchemaToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.LeaveTripSchemaToggle", LookupsID, CreatedBy);
        }

        public void RandRTripSchemaToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.RandRTripSchemaToggle", LookupsID, CreatedBy);
        }

        public void ReturnableTAStatusCodeToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ReturnableTAStatusCodeToggle", LookupsID, CreatedBy);
        }

        public void EditableTAStatusCodeToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.EditableTAStatusCodeToggle", LookupsID, CreatedBy);
        }

        public void ActionedTAStatusCodeToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ActionedTAStatusCodeToggle", LookupsID, CreatedBy);
        }

        public void CancellableTAStatusCodeToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.CancellableTAStatusCodeToggle", LookupsID, CreatedBy);
        }

        public void DelegatableStatusCodeToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DelegatableStatusCodeToggle", LookupsID, CreatedBy);
        }

        public void ByTravellorToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ByTravellorToggle", LookupsID, CreatedBy);
        }

        public void BySupervisorToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.BySupervisorToggle", LookupsID, CreatedBy);
        }

        public void ByRMOToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ByRMOToggle", LookupsID, CreatedBy);
        }

        public void ByApproverToggle(string LookupsID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ByApproverToggle", LookupsID, CreatedBy);
        }

        


        /*Permissions*/

        #endregion



    }
}
