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
    public class TAWorkFlow
    {
        private Data.TAWorkFlow daSteps = new Data.TAWorkFlow();
        HttpContext context = HttpContext.Current;

        public void GetAllStatuses(ref DataTable dt)
        {
            dt.Load(daSteps.GetAllStatuses());
        }

        public void GetAllSteps(ref DataTable dt)
        {
            dt.Load(daSteps.GetAllSteps());
        }

        public void GetAllTripSchemas(ref DataTable dt)
        {
            dt.Load(daSteps.GetAllTripSchemas());
        }
        public void InsertUpdateSteps(int StepID, string StepName)
        {

            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.InsertUpdateSteps(StepID, StepName, ui.User_Id);
        }
        public int CheckDuplicateSteps(string StepID,string StepName)
        {
            return daSteps.CheckDuplicateSteps(StepID,StepName);
        }
        public void DeleteSteps(string StepID)
        {
            daSteps.DeleteSteps(StepID);
        }



        /*Permissions*/

        public void GetRoleStatuses(string RoleID, ref DataTable dt)
        {
            dt.Load(daSteps.GetRoleStatuses(RoleID));
        }
        public void GetRoleTripSchemas(string RoleID, ref DataTable dt)
        {
            dt.Load(daSteps.GetRoleTripSchemas(RoleID));
        }

        public void GetRejectionReasonsByStatusCode(string StatusCode, ref DataTable dt)
        {
            dt.Load(daSteps.GetRejectionReasonsByStatusCode(StatusCode));
        }

        public void GetCancellationReasonsByStatusCode(string StatusCode, ref DataTable dt)
        {
            dt.Load(daSteps.GetCancellationReasonsByStatusCode(StatusCode));
        }

        public void GetEmergencyTripSchemas(ref DataTable dt)
        {
            dt.Load(daSteps.GetEmergencyTripSchemas());
        }

        public void GetTripSchemas(ref DataTable dt)
        {
            dt.Load(daSteps.GetTripSchemas());
        }

        public void GetReturnableTAStatusCodes(ref DataTable dt)
        {
            dt.Load(daSteps.GetReturnableTAStatusCodes());
        }

        public void GetCancellableTAStatusCodes(ref DataTable dt)
        {
            dt.Load(daSteps.GetCancellableTAStatusCodes());
        }

        public void GetTAStatusCodes(ref DataTable dt)
        {
            dt.Load(daSteps.GetTAStatusCodes());
        }

        public void GetDelegatableStatusCodes(ref DataTable dt)
        {
            dt.Load(daSteps.GetDelegatableStatusCodes());
        }

        public void RoleAccessToStatusesToggle(string RoleID, string StatusID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.RoleAccessToStatusesToggle(RoleID, StatusID, ui.User_Id);
        }
        public void RoleAccessToTripSchemaToggle(string RoleID, string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.RoleAccessToTripSchemaToggle(RoleID, LookupsID, ui.User_Id);
        }

        public void StatusCodeRejectionReasonsToggle(string StatusCode, string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.StatusCodeRejectionReasonsToggle(StatusCode, LookupsID, ui.User_Id);
        }

        public void StatusCodeCancellationReasonsToggle(string StatusCode, string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.StatusCodeCancellationReasonsToggle(StatusCode, LookupsID, ui.User_Id);
        }

        

        public void EmergencyTripSchemaToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.EmergencyTripSchemaToggle(LookupsID, ui.User_Id);
        }

        public void LeaveTripSchemaToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.LeaveTripSchemaToggle(LookupsID, ui.User_Id);
        }

        public void RandRTripSchemaToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.RandRTripSchemaToggle(LookupsID, ui.User_Id);
        }

        public void ReturnableTAStatusCodeToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.ReturnableTAStatusCodeToggle(LookupsID, ui.User_Id);
        }

        public void ActionedTAStatusCodeToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.ActionedTAStatusCodeToggle(LookupsID, ui.User_Id);
        }

        public void EditableTAStatusCodeToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.EditableTAStatusCodeToggle(LookupsID, ui.User_Id);
        }

        public void CancellableTAStatusCodeToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.CancellableTAStatusCodeToggle(LookupsID, ui.User_Id);
        }

        public void DelegatableStatusCodeToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.DelegatableStatusCodeToggle(LookupsID, ui.User_Id);
        }

        public void ByTravellorToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.ByTravellorToggle(LookupsID, ui.User_Id);
        }

        public void BySupervisorToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.BySupervisorToggle(LookupsID, ui.User_Id);
        }


        public void ByRMOToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.ByRMOToggle(LookupsID, ui.User_Id);
        }


        public void ByApproverToggle(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSteps.ByApproverToggle(LookupsID, ui.User_Id);
        }

       

        

        /*Permissions*/




    }
}
