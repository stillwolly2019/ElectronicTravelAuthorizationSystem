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
    public class Lookups
    {

        private Data.Lookups daLookups = new Data.Lookups();
        HttpContext context = HttpContext.Current;
        //Disclaimer
        public void GetDisclaimer(ref DataTable dt)
        {
            dt.Load(daLookups.GetDisclaimer());
        }
        //get
        public void CheckDuplicateLookupGroup(string LookupGroupID, string LookupGroup, ref DataTable dt)
        {
            dt.Load(daLookups.CheckDuplicateLookupGroup(LookupGroupID, LookupGroup));
        }
        public void CheckDuplicateLookups(string LookupsID, string LookupGroupID, string SubGroupID, string Description, ref DataTable dt)
        {
            dt.Load(daLookups.CheckDuplicateLookups(LookupsID, LookupGroupID, SubGroupID,Description));
        }
        public void GetAllLookups(ref DataTable dt)
        {
            dt.Load(daLookups.GetAllLookups());
        }
        public void GetAllLookupsGroups(ref DataTable dt)
        {
            dt.Load(daLookups.GetAllLookupsGroups());
        }

        public void GetAllLookupsList(ref DataSet ds,bool isinternational=false)
        {
            ds = daLookups.GetAllLookupsList(isinternational);
        }
        public void GetAllTripSchemas(ref DataSet ds,bool isinternational)
        {
            ds = daLookups.GetAllTripSchemas(isinternational);
        }

        public void GetUnitsByDepartmentID(string DepartmentID, ref DataSet ds)
        {
            ds = daLookups.GetUnitsByDepartmentID(DepartmentID);
        }

        public void GetSubUnitsByUnitID(string UnitID, ref DataSet ds)
        {
            ds = daLookups.GetSubUnitsByUnitID(UnitID);
        }

        public void GetZonesByLocationID(string LocationID,ref DataSet ds)
        {
            ds = daLookups.GetZonesByLocationID(LocationID);
        }
        public string GetStaffLocation(string PERNO)
        {
            string LocationID = "";
            LocationID = daLookups.GetStaffLocation(PERNO).ToString();
            return LocationID;
        }

        
        public void GetLookupsForGroup(string lkpId,ref DataSet ds)
        {
            ds = daLookups.GetLookupsForGroup(lkpId);
        }


        public void GetAllLookupsListForLeaveRequest(ref DataSet ds,string StatusCode)
        {
            ds = daLookups.GetAllLookupsListForLeaveRequest(StatusCode);
        }

        public void GetAllResidenceForSelectedZone(string ZoneNo, ref DataSet ds)
        {
            ds = daLookups.GetAllResidenceForSelectedZone(ZoneNo);
        }

        public void GetRejectionReason(ref DataTable dt,string TravelAuthorizationID="")
        {
            dt.Load(daLookups.GetRejectionReason(TravelAuthorizationID));
        }

        public void GetCancellationReason(ref DataTable dt,string TravelAuthorizationID)
        {
            dt.Load(daLookups.GetCancellationReason(TravelAuthorizationID));
        }

        public void GetReturnReasons(ref DataTable dt, string TravelAuthorizationID)
        {
            dt.Load(daLookups.GetReturnReasons(TravelAuthorizationID));
        }

        public void GetReturnReasons(ref DataTable dt)
        {
            dt.Load(daLookups.GetReturnReasons());
        }
        public void GetTACancellationReason(ref DataTable dt,string TANo)
        {
            dt.Load(daLookups.GetTACancellationReason(TANo));
        }
        public void GetCancellationReasons(ref DataTable dt)
        {
            dt.Load(daLookups.GetCancellationReasons());
        }
        public void GetCheckList(ref DataTable dt)
        {
            dt.Load(daLookups.GetCheckList());
        }
        public void GetTECRejectionReason(ref DataTable dt)
        {
            dt.Load(daLookups.GetTECRejectionReason());
        }
        public void GetTECReturnedRejectionReason(ref DataTable dt)
        {
            dt.Load(daLookups.GetTECReturnedRejectionReason());
        }
        public void SearchLookups(string LookupGroupID, string SubGroupID, string Code, string Description, ref DataTable dt)
        {
            dt.Load(daLookups.SearchLookups(LookupGroupID, SubGroupID, Code, Description));
        }
        public void SearchLookupGroups(string LookupGroup, ref DataTable dt)
        {
            dt.Load(daLookups.SearchLookupGroups(LookupGroup));
        }

        public void SearchCity(string Prefix, ref DataTable dt)
        {
            dt.Load(daLookups.SearchCity(Prefix));
        }

        public void SearchAccomodations(string Prefix, ref DataTable dt)
        {
            dt.Load(daLookups.SearchAccomodations(Prefix));
        }

        
        public void GetCityByDescription(string CityDescription, ref DataTable dt)
        {
            dt.Load(daLookups.GetCityByDescription(CityDescription));
        }

        public void GetAccomodationByDescription(string AccomodationDescription, ref DataTable dt)
        {
            dt.Load(daLookups.GetAccomodationByDescription(AccomodationDescription));
        }

        public void GetLeaveCategoryDescription(string LeaveCategoryDescription, ref DataTable dt)
        {
            dt.Load(daLookups.GetLeaveCategoryDescription(LeaveCategoryDescription));
        }

        //delete
        public void DeleteLookups(string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLookups.DeleteLookups(LookupsID, ui.User_Id);
        }
        public void DeleteLookupsGroups(string LookupGroupID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLookups.DeleteLookupsGroups(LookupGroupID, ui.User_Id);
        }

        //insert - update
        public void InsertUpdateLookups(string LookupsID, string LookupGroupID, string SubGroupID, string Code, string Description, string LongDescription)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLookups.InsertUpdateLookups(LookupsID, LookupGroupID, SubGroupID, Code, Description, LongDescription, ui.User_Id); 
        }
        public void InsertUpdateLookupsGroups(string LookupGroupID, string LookupGroup)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLookups.InsertUpdateLookupsGroups(LookupGroupID, LookupGroup, ui.User_Id);
        }

        public void GetAllDocumentTypes(ref DataTable dt)
        {
            dt.Load(daLookups.GetAllDocumentTypes());
        }

        public void GetDocumentTypes (ref DataTable dt)
        {
            dt.Load(daLookups.GetDocumentTypes());
        }

        public void GetTripSchemaDocuments(string Code, ref DataTable dt)
        {
            dt.Load(daLookups.GetTripSchemaDocuments(Code));
        }

        public void TripSchemaDocumentToggle(string Code, string LookupsID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLookups.TripSchemaDocumentToggle(Code, LookupsID, ui.User_Id);
        }


    }
}
