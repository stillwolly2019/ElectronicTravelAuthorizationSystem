using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace Business
{
    public class TravelAuthorization
    {
        private Data.TravelAuthorization daTA = new Data.TravelAuthorization();
        HttpContext context = HttpContext.Current;

        #region Dispatch Itinineray
        public void GetDispatchItineraryByTravelAuthorizationNumber(string TravelAuthorizationNumber, ref DataTable dt)
        {
            dt.Load(daTA.GetDispatchItineraryByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }

        public string InsertUpdateDispatchItinerary(string DispatchItineraryID, string TravelAuthorizationNumber, string FlightReference, TimeSpan ETAETD, string PickupLocation, DateTime PickupDate, TimeSpan PickupTime, string DropOffLocation,int Ordering)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string result;
            result = daTA.InsertUpdateDispatchItinerary(DispatchItineraryID, TravelAuthorizationNumber, FlightReference, ETAETD, PickupLocation, PickupDate, PickupTime, DropOffLocation, Ordering, ui.User_Id);
            return result;
        }

        public void DeleteDispatchItinerary(string DispatchItineraryID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.DeleteDispatchItinerary(DispatchItineraryID, ui.User_Id);
        }
        #endregion


        public bool IsValidWBS(string WBS)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return Convert.ToBoolean(daTA.IsValidWBS(WBS));
        }

        public void SearchWBS(string SearchText, ref DataTable dt)
        {
            dt.Load(daTA.SearchWBS(SearchText));
        }

        public void GetAllStepMaps(ref DataTable dt)
        {
            dt.Load(daTA.GetAllStepMaps());
        }
        public void InsertUpdateStepMap(string StatusID, int StepID,string ActionNeeded)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.InsertUpdateStepMap(StatusID, StepID, ActionNeeded, ui.User_Id);
        }

        public void DeleteStepMap(string StatusID)
        {
            daTA.DeleteStepMap(StatusID);
        }

        public void GetAllStatuses(ref DataTable dt)
        {
            dt.Load(daTA.GetAllStatuses());
        }

        public void GetAllWorkFlowSteps(ref DataTable dt)
        {
            dt.Load(daTA.GetAllWorkFlowSteps());
        }

        

        public void GetAllLocations(ref DataTable dt)
        {
            dt.Load(daTA.GetAllLocations());
        }

        public void GetAllAccomodationsByLocationID(string LocationID, ref DataTable dt)
        {
            dt.Load(daTA.GetAllAccomodationsByLocationID(LocationID));
        }

        public int InsertAccomodation(string AccomodationID, string AccomodationName, string LocationID)
        {
            //Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daTA.InsertAccomodation(AccomodationID, AccomodationName, LocationID);
        }

        public void DeleteAccomodation(string AccomodationID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.DeleteAccomodation(AccomodationID, ui.User_Id);
        }

        public void GetTravelAuthorizationsByAccomodation(string AccomodationID, ref DataTable dt)
        {
            dt.Load(daTA.GetTravelAuthorizationsByAccomodation(AccomodationID));
        }

        public void GetStaffTravelAuthorization(ref  DataTable dt, string TravelAuthorizationNumber, string Status, string location)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetStaffTravelAuthorization(ui.User_Id, TravelAuthorizationNumber, Status, location));
        }

        public void GetTravelAuthorizationsForTransfer(ref DataTable dt, string TravelAuthorizationNumber)
        {
            dt.Load(daTA.GetTravelAuthorizationsForTransfer(TravelAuthorizationNumber));
        }

        public void GetTravelAuthorizationsForMapping(ref DataTable dt, string TravelAuthorizationNumber)
        {
            dt.Load(daTA.GetTravelAuthorizationsForMapping(TravelAuthorizationNumber));
        }

        public void GetDashboardItems(ref DataTable dt, string TravelAuthorizationNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetDashboardItems(TravelAuthorizationNumber,ui.User_Id));
        }

       

        public void GetDashboardItemsPendingAction(ref DataTable dt, string TravelAuthorizationNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetDashboardItemsPendingAction(TravelAuthorizationNumber, ui.User_Id));
        }

        public void GetDashboardItemsActionedByMe(ref DataTable dt, string TravelAuthorizationNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetDashboardItemsActionedByMe(TravelAuthorizationNumber, ui.User_Id));
        }

        public void GetDashboardItemsByMe(ref DataTable dt, string TravelAuthorizationNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetDashboardItemsByMe(TravelAuthorizationNumber, ui.User_Id));
        }

        public void GetDelegationStatus(ref DataTable dt,string UId)
        {
            dt.Load(daTA.GetDelegationStatus(UId));
        }
        //add detail


        public void GetStaffTravelAuthorizationTR(ref DataTable dt, string TravelAuthorizationNumber, string Status, string location)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetStaffTravelAuthorization(ui.User_Id, TravelAuthorizationNumber, Status, location));
        }
        //

        //added for the transfer of the member TA

        public void GetStaffTravelAuthorizationDetails(ref DataTable dt, string TravelAuthorizationNumber, string Status, string location,string CreatedBy,string UpdatedBy,string StatusCode)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetStaffTravelAuthorizationDetails(ui.User_Id, TravelAuthorizationNumber, Status, location,CreatedBy,UpdatedBy,StatusCode));
        }


        public void GetTravelAuthorizationByTravelAuthorizationID(string TravelAuthorizationID, ref DataTable dt)
        {
            dt.Load(daTA.GetTravelAuthorizationByTravelAuthorizationID(TravelAuthorizationID));
        }

        public void GetTAByTANo(string TANo, string UserID,int EmailTypeIDOnSubmission, ref DataTable dt)
        {
            dt.Load(daTA.GetTAByTANo(TANo, UserID, EmailTypeIDOnSubmission));
        }

        public void GetTAByTANoAndStep(ref DataTable dt,string TANo,int Step )
        {
            dt.Load(daTA.GetTAByTANoAndStep(TANo, Step));
        }

        public void SearchTravelAuthorization(string TravelAuthorizationNumber, string WBS, string Status, string location, ref DataTable dt)
        {
            dt.Load(daTA.SearchTravelAuthorization(TravelAuthorizationNumber, WBS, Status, location));
        }

        public void SearchTravelAuthorizationMangers(string TravelAuthorizationNumber, string Status, string location, ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.SearchTravelAuthorizationManagers(TravelAuthorizationNumber, Status, location, ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }

        public void GetRoleBasedDashboardItemsByUserID(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daTA.GetRoleBasedDashboardItemsByUserID(ui.User_Id,ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }

        public void GetTravelItineraryByTravelAuthorizationNumber(string TravelAuthorizationNumber, ref DataTable dt)
        {
            dt.Load(daTA.GetTravelItineraryByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }

        public void GetStaffTravelAuthorizationDetails(ref DataTable dt, string v, string status, string location, object createdBy, object updatedBy, object statusCode)
        {
            throw new NotImplementedException();
        }

        public void GetTravelItineryPrivateDEviation(string TravelAuthorizationNumber, ref DataTable dt)
        {
            dt.Load(daTA.GetTravelItineryPrivateDeviation(TravelAuthorizationNumber));
        }

        public void GetTAStatusHistory(string TravelAuthorizationID, ref DataTable dt)
        {
            dt.Load(daTA.GetTAStatusHistory(TravelAuthorizationID));
        }

        public void GetWBSByTravelAuthorizationID(string TravelAuthorizationID, ref DataTable dt)
        {
            dt.Load(daTA.GetWBSByTravelAuthorizationID(TravelAuthorizationID));
        }

        //delete
        public void DeleteTravelItinerary(string TravelItineraryID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.DeleteTravelItinerary(TravelItineraryID, ui.User_Id);
        }

        public void DeleteWBS(string WBSID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.DeleteWBS(WBSID, ui.User_Id);
        }

        public void DeleteCheckList(string TravelItineraryID)
        {
            daTA.DeleteCheckList(TravelItineraryID);
        }

        //insert - update
        public string InsertUpdateTravelAuthorization(string TravelAuthorizationID, string FirstName, string MiddleName, string LastName,
            string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, bool SecurityClearance,
            bool SecurityTraining, string StatusCode, string userId,
            string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs,
            bool IsAccommodationProvided, string AccommodationDetails, bool IsTravelAdvanceRequested, string TravelAdvanceCurrency, decimal TravelAdvanceAmount,
            string TravelAdvanceMethod, int IsVisaObtained, string VisaIssued, int IsVaccinationObtained, bool IsSecurityClearanceRequestedByMission, bool TAConfirm, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo, bool IsNotForDSA)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string TravelAuthorizationIDOut = "";
            TravelAuthorizationIDOut = daTA.InsertUpdateTravelAuthorization(TravelAuthorizationID,
                System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(),
                userId, FirstName, MiddleName, LastName, PurposeOfTravel, TripSchemaCode, ModeOfTravelCode, SecurityClearance, SecurityTraining, StatusCode, ui.User_Id,
                CityOfAccommodation, IsPrivateStay, PrivateStayDates, IsPrivateDeviation, PrivateDeviationLegs, IsAccommodationProvided, AccommodationDetails,
                IsTravelAdvanceRequested, TravelAdvanceCurrency, TravelAdvanceAmount, TravelAdvanceMethod, IsVisaObtained, VisaIssued, IsVaccinationObtained,
                IsSecurityClearanceRequestedByMission, TAConfirm, PrivateStayDateFrom, PrivateStayDateTo, IsNotForDSA).ToString();
            return TravelAuthorizationIDOut;
        }

        public string InsertUpdateTravelersInformation(string TravelAuthorizationID, string FirstName, string MiddleName, string LastName,
            string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, string StatusCode, string userId,
            string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs,
            bool IsAccommodationProvided, string AccommodationDetails, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo,
            string FamilyMembers, string PRISMNumber, DateTime? LeaveStartDate,
            DateTime? LeaveEndDate, DateTime? RRStartDate, DateTime? RREndDate, bool IsEmergency,

               string ExtraCargoCode,
string FlightCarrierCode,
string FlightRefNo,
string ETDETACode,
string ETDETAPCode,
string PickupDropCode,
string BookingCode,
string Remarks,
DateTime? CheckinDate,
DateTime? CheckOutDate,
string Nights,
bool IsNotify)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string TravelAuthorizationIDOut = "";
            TravelAuthorizationIDOut = daTA.InsertUpdateTravelersInformation(TravelAuthorizationID,
                System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(),
                userId, FirstName, MiddleName, LastName, PurposeOfTravel, TripSchemaCode, ModeOfTravelCode, StatusCode, ui.User_Id,
                CityOfAccommodation, IsPrivateStay, PrivateStayDates, IsPrivateDeviation, PrivateDeviationLegs, IsAccommodationProvided, AccommodationDetails,
                PrivateStayDateFrom, PrivateStayDateTo, FamilyMembers, PRISMNumber, LeaveStartDate,
                LeaveEndDate, RRStartDate, RREndDate, IsEmergency,
                              ExtraCargoCode,
FlightCarrierCode,
FlightRefNo,
ETDETACode,
ETDETAPCode,
PickupDropCode,
BookingCode,
Remarks,
CheckinDate,
CheckOutDate,
Nights,
IsNotify).ToString();
            return TravelAuthorizationIDOut;
        }

        /*
        public string InsertUpdateTravelersInformation(string TravelAuthorizationID, string FirstName, string MiddleName, string LastName,
            string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, string StatusCode, string userId,
            string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs,
            bool IsAccommodationProvided, string AccommodationDetails, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo, string FamilyMembers, string PRISMNumber, DateTime? LeaveStartDate, DateTime? LeaveEndDate, DateTime? RRStartDate, DateTime? RREndDate,bool IsEmergency)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string TravelAuthorizationIDOut = "";
            TravelAuthorizationIDOut = daTA.InsertUpdateTravelersInformation(TravelAuthorizationID,
                System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(),
                userId, FirstName, MiddleName, LastName, PurposeOfTravel, TripSchemaCode, ModeOfTravelCode, StatusCode, ui.User_Id,
                CityOfAccommodation, IsPrivateStay, PrivateStayDates, IsPrivateDeviation, PrivateDeviationLegs, IsAccommodationProvided, AccommodationDetails,
                PrivateStayDateFrom, PrivateStayDateTo, FamilyMembers, PRISMNumber, LeaveStartDate, LeaveEndDate, RRStartDate, RREndDate, IsEmergency).ToString();
            return TravelAuthorizationIDOut;
        }
        */

        public void UpdateAdvanceAndSecurity(string TravelAuthorizationID, bool SecurityClearance, bool SecurityTraining, bool IsTravelAdvanceRequested, string TravelAdvanceCurrency, decimal TravelAdvanceAmount,
            string TravelAdvanceMethod, int IsVisaObtained, string VisaIssued, int IsVaccinationObtained, int IsDocumentsObtained, int IsSecurityClearanceRequestedByMission, bool TAConfirm, bool IsNotForDSA)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.UpdateAdvanceAndSecurity(TravelAuthorizationID,
                SecurityClearance,
                SecurityTraining,
                IsTravelAdvanceRequested,
                TravelAdvanceCurrency,
                TravelAdvanceAmount,
                TravelAdvanceMethod,
                IsVisaObtained,
                VisaIssued,
                IsVaccinationObtained, IsDocumentsObtained,

                IsSecurityClearanceRequestedByMission,
                TAConfirm,
                IsNotForDSA,
                ui.User_Id
                );
        }

        public void GetTAInfo(string TAID, ref DataTable dt)
        {
            dt.Load(daTA.GetTAInfo(TAID));
        }

        public string TransferTA(string TANo, string To)
        {
            return daTA.TransferTA(TANo, To).ToString();
        }

        public string MapTA(string TANo, string Loc)
        {
            return daTA.MapTA(TANo, Loc).ToString();
        }

        public void UpdateTravelAuthorizationStatus(string TravelAuthorizationID, string StatusCode, string Comments)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.UpdateTravelAuthorizationStatus(TravelAuthorizationID, StatusCode, ui.User_Id, Comments);
        }
        

        public void UpdateTravelAuthorizationIsComplete(string TravelAuthorizationID, bool IsTecComplete)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.UpdateTravelAuthorizationIsComplete(TravelAuthorizationID, IsTecComplete, ui.User_Id);
        }

        public string InsertTAStatus(string TravelAuthorizationID, string StatusCode, string RejectionReasons, string RejectionReasonType)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string result;
            result = daTA.InsertTAStatus(TravelAuthorizationID, StatusCode, RejectionReasons, RejectionReasonType, ui.User_Id).ToString();
            return result;
        }

        public void InsertRejectionReasons(string TravelAuthorizationID, string RejectionReasonID, string RejectionReasonType, string RejectionComment)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.InsertRejectionReasons(TravelAuthorizationID, RejectionReasonID, RejectionReasonType, ui.User_Id, RejectionComment);
        }

        public void InsertCheckList(string TravelAuthorizationID, string LookupID, int Value, string Note)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.InsertCheckList(TravelAuthorizationID, LookupID, Value, Note, ui.User_Id);
        }

        public string InsertUpdateTravelItinerary(string TravelItineraryID, string TravelAuthorizationNumber, string ModeOfTravelID, string FromLocationCode, DateTime FromLocationDate, string ToLocationCode, DateTime ToLocationDate, int Ordering)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string result;
            result = daTA.InsertUpdateTravelItinerary(TravelItineraryID, TravelAuthorizationNumber, ModeOfTravelID, FromLocationCode, FromLocationDate, ToLocationCode, ToLocationDate, ui.User_Id, Ordering).ToString();
            return result;
        }

        public void UpdateTravelItinerary(string TravelItineraryID, string ModeOfTravelID, string FromLocationCode, DateTime FromLocationDate, string ToLocationCode, DateTime ToLocationDate)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.UpdateTravelItinerary(TravelItineraryID, ModeOfTravelID, FromLocationCode, FromLocationDate, ToLocationCode, ToLocationDate, ui.User_Id);
        }

        public void UpdateWBS(string WBSID, string WBSCode, double PercentageOrAmount, string Note)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.UpdateWBS(WBSID, WBSCode, PercentageOrAmount, Note, ui.User_Id);
        }

        public void InsertTAComments(string CommentId, string TANumber, string CommentType, string Comment)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.InsertTAComments(CommentId, TANumber, Comment, CommentType, ui.User_Id);
        }

        public void GetRejectionReason(string TravelAuthorizationID, string RejectionReasonType, ref DataTable dt)
        {
            dt.Load(daTA.GetRejectionReason(TravelAuthorizationID, RejectionReasonType));
        }

        public void GetReturnReason(string TravelAuthorizationID, string RejectionReasonType, ref DataTable dt)
        {
            dt.Load(daTA.GetReturnReason(TravelAuthorizationID, RejectionReasonType));
        }

        public void GetCancellationReason(string TravelAuthorizationID, string RejectionReasonType, ref DataTable dt)
        {
            dt.Load(daTA.GetCancellationReason(TravelAuthorizationID, RejectionReasonType));
        }



        public void GetCheckList(string TravelAuthorizationID, string lookupID, ref DataTable dt)
        {
            dt.Load(daTA.GetCheckList(TravelAuthorizationID, lookupID));
        }

        public void GetCheckListByTAID(string TravelAuthorizationID, ref DataTable dt)
        {
            dt.Load(daTA.GetCheckListByTAID(TravelAuthorizationID));
        }

        public void GetTAComments(string TravelAuthorizationNumber, ref DataTable dt)
        {
            dt.Load(daTA.GetTAComments(TravelAuthorizationNumber));
        }


        //

        //added update to owner transfer TA to owner

        //public void UpdateTravelAuthorizationOwner( string UserID, string CreatedBy, string UpdatedBy)
        //{
        //    Objects.User ui = (Objects.User)context.Session["userinfo"];
        //    daTA.UpdateTravelAuthorizationOwner(ui.User_Id, CreatedBy, UpdatedBy);
        //}



        public void UpdateStaffTravelAuthorizationTA(string TravelAuthorizationNumber,string UserID, string CreatedBy, string UpdatedBy,string PRISMNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.UpdateStaffTravelAuthorizationTA(TravelAuthorizationNumber,ui.User_Id, CreatedBy, UpdatedBy, PRISMNumber);
        }

        //public void TransferTA(string TravelAuthorizationID, string UserID)
        //{
        //    Objects.User ui = (Objects.User)context.Session["userinfo"];
        //    daTA.TransferTA(TravelAuthorizationID, UserID, ui.User_Id, ui.User_Id, System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString());
        //}






        public string InsertUpdateWBS(string WBSID, string TravelAuthorizationID, string WBSCode, double PercentageOrAmount, string Note, bool IsPercentage)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string result = "";
            result = daTA.InsertUpdateWBS(WBSID, TravelAuthorizationID, WBSCode, PercentageOrAmount, Note, IsPercentage, ui.User_Id).ToString();
            return result;

        }

        public void GetTravelAuthorizationByTravelAuthorizationNumber(string TravelAuthorizationNumber, ref DataTable dt)
        {
            dt.Load(daTA.GetTravelAuthorizationByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }

        public void GetArrivalDateByTravelAuthorizationNumber(string TravelAuthorizationNumber, ref DataTable dt)
        {
            dt.Load(daTA.GetArrivalDateByTravelAuthorizationNumber(TravelAuthorizationNumber));
        }

        public void DeleteTravelathorization(string TravelAuthorizationID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.DeleteTravelathorization(TravelAuthorizationID, ui.User_Id);
        }

        public string GetTAStepByTravelAuthorizationID(string TravelAuthorizationID)
        {
            string Step = "";
            Step = daTA.GetTAStepByTravelAuthorizationID(TravelAuthorizationID).ToString();
            return Step;
        }

        public string GetTravelAuthorizationNumberByTANO(string TANO)
        {
            string Step = "";
            Step = daTA.GetTravelAuthorizationNumberByTANO(TANO).ToString();
            return Step;
        }

        

        public void DuplicateTA(string TravelAuthorizationID, string UserID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.DuplicateTA(TravelAuthorizationID, UserID, System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(), ui.User_Id);
        }

        public void CheckDuplicatedTA(string TravelAuthorizationID, string TravelersName, DateTime FromLocationDate, DateTime ToLocationDate, string FromLocationCode, string ToLocationCode, string TravelItineraryID, ref DataTable dt)
        {
            dt.Load(daTA.CheckDuplicatedTA(TravelAuthorizationID, TravelersName, FromLocationDate, ToLocationDate, FromLocationCode, ToLocationCode, TravelItineraryID));
        }

        public void GetTAStatusByTAID(string TravelAuthorizationID, ref DataTable dt)
        {
            dt.Load(daTA.GetTAStatusByTAID(TravelAuthorizationID));
        }

        public void InsertTADocumentNumber(string TravelAuthorizationID, string DocumentNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daTA.InsertTADocumentNumber(TravelAuthorizationID, DocumentNumber, ui.User_Id);

        }

        public bool CheckLeaveRelationShip(string code)
        {
            string ct = "0";
            ct = daTA.CheckLeaveRelationShip(code).ToString();
            return Convert.ToBoolean(Convert.ToInt32(ct));
        }

        public bool CheckRandRRelationShip(string code)
        {
            string ct = "0";
            ct = daTA.CheckRandRRelationShip(code).ToString();
            return Convert.ToBoolean(Convert.ToInt32(ct));
        }
    }
}
