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
    public sealed class TravelAuthorization
    {
        private object ValueOrDBNullIfZero(int val)
        {
            if (val == 2) return DBNull.Value;
            return val;
        }


        public int IsValidWBS(string WBS)
        {
            int _isvalid = 0;
            _isvalid = Convert.ToInt32(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.IsValidWBS", WBS).ToString());
            return _isvalid;
        }

        public IDataReader SearchWBS(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.SearchWBS", SearchText);
            return Reader;
        }

        public IDataReader GetDispatchItineraryByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetDispatchItineraryByTravelAuthorizationNumber", TravelAuthorizationNumber);

            return Reader;
        }

        public string InsertUpdateDispatchItinerary(
        string DispatchItineraryID, string TravelAuthorizationNumber, string FlightReference, TimeSpan ETAETD, string PickupLocation, DateTime PickupDate, TimeSpan PickupTime, string DropOffLocation,int Ordering, string CreatedBy)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertUpdateDispatchItinerary",
                DispatchItineraryID, TravelAuthorizationNumber, FlightReference, ETAETD, PickupLocation, PickupDate, PickupTime, DropOffLocation, Ordering, CreatedBy).ToString();
            return result;
        }

        public void DeleteDispatchItinerary(string DispatchItineraryID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.DeleteDispatchItinerary", DispatchItineraryID, CreatedBy);
        }

       

        public IDataReader GetAllStatuses()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetAllStatuses]");
            return Reader;
        }

        public IDataReader GetAllWorkFlowSteps()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetAllWorkFlowSteps]");
            return Reader;
        }






        public IDataReader GetAllStepMaps()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetAllStepMaps]");
            return Reader;
        }

        public void InsertUpdateStepMap(string StatusID, int StepID,string ActionNeeded, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.InsertUpdateStepMap",
                StatusID, StepID, ActionNeeded, CreatedBy);
        }
        
        public void DeleteStepMap(string StatusID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteStepMap",
                StatusID);
        }

        public IDataReader GetAllLocations()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllLocations");
            return Reader;
        }
        public IDataReader GetAllAccomodationsByLocationID(string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllAccomodationsByLocationID", LocationID);
            return Reader;
        }

        public int InsertAccomodation(string AccomodationID, string AccomodationName, string LocationID)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.InsertAccomodation", AccomodationID, LocationID, AccomodationName).ToString());
        }

        public void DeleteAccomodation(string AccomodationID, string ModifiedBy)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.DeleteAccomodation", AccomodationID, ModifiedBy);
        }

        public IDataReader GetTravelAuthorizationsByAccomodation(string AccomodationName)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetTravelAuthorizationsByAccomodation", AccomodationName);
            return Reader;
        }

        public IDataReader GetStaffTravelAuthorization(string UserID, string TravelAuthorizationNumber, string status, string Location)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetStaffTravelAuthorization",UserID, TravelAuthorizationNumber, status,Location);
            return Reader;
        }

        public IDataReader GetTravelAuthorizationsForTransfer(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTravelAuthorizationsForTransfer", TravelAuthorizationNumber);
            return Reader;
        }

        public IDataReader GetTravelAuthorizationsForMapping(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTravelAuthorizationsForMapping", TravelAuthorizationNumber);
            return Reader;
        }

        

        public IDataReader GetDashboardItems(string TravelAuthorizationNumber, string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetDashboardItems", TravelAuthorizationNumber,UserID);
            return Reader;
        }

        public IDataReader GetDashboardItemsPendingAction(string TravelAuthorizationNumber, string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetDashboardItemsPendingAction", TravelAuthorizationNumber, UserID);
            return Reader;
        }

        public IDataReader GetDashboardItemsActionedByMe(string TravelAuthorizationNumber, string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetDashboardItemsActionedByMe", TravelAuthorizationNumber, UserID);
            return Reader;
        }

        public IDataReader GetDashboardItemsByMe(string TravelAuthorizationNumber, string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetDashboardItemsByMe", TravelAuthorizationNumber, UserID);
            return Reader;
        }

        public IDataReader GetDelegationStatus(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.GetDelegationStatus",  UserID);
            return Reader;
        }

        

        // Added by walter TR
        public IDataReader GetStaffTravelAuthorizationTR(string UserID, string TravelAuthorizationNumber, string Status, string location)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetStaffTravelAuthorization", UserID, TravelAuthorizationNumber,
                 Status, location);
            return Reader;
        }



        // added for the transfer of the TA to member

        public IDataReader GetStaffTravelAuthorizationDetails(string UserID, string TravelAuthorizationNumber, string Status, string location, string CreatedBy, string UpdatedBy, string StatusCode)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetStaffTravelAuthorizationDetails", UserID, TravelAuthorizationNumber,
                 Status, location,CreatedBy,UpdatedBy,StatusCode);
            return Reader;
        } 


        public IDataReader GetTravelAuthorizationByTravelAuthorizationID(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTravelAuthorizationByTravelAuthorizationID", TravelAuthorizationID);
            return Reader;
        }

        public IDataReader SearchTravelAuthorization(string TravelAuthorizationNumber, string WBS, string Status, string location)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.SearchTravelAuthorization", TravelAuthorizationNumber,
                 WBS, Status, location);
            return Reader;
        }

        public IDataReader SearchTravelAuthorizationManagers(string TravelAuthorizationNumber, string Status, string location, string DepartmentID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.SearchTravelAuthorizationForManagers",
                TravelAuthorizationNumber, Status, location, DepartmentID, UnitID, SubUnitID);
            return Reader;
        }

        public IDataReader GetRoleBasedDashboardItemsByUserID(string UserID, string DepartmentID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetRoleBasedDashboardItemsByUserID",
            UserID, DepartmentID, UnitID, SubUnitID);
            return Reader;
        }
        

        public IDataReader GetTravelItineraryByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTravelItineraryByTravelAuthorizationNumber", TravelAuthorizationNumber);

            return Reader;
        }

        public IDataReader GetTravelItineryPrivateDeviation(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.[GetPrivateDeviation]", TravelAuthorizationNumber);

            return Reader;
        }

        public IDataReader GetRejectionReason(string TravelAuthorizationID, string RejectionReasonType)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.[GetRejectionReason]", TravelAuthorizationID, RejectionReasonType);

            return Reader;
        }

        public IDataReader GetReturnReason(string TravelAuthorizationID, string RejectionReasonType)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.[GetReturnReason]", TravelAuthorizationID, RejectionReasonType);

            return Reader;
        }

        public IDataReader GetCancellationReason(string TravelAuthorizationID, string RejectionReasonType)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.[GetCancellationReason]", TravelAuthorizationID, RejectionReasonType);

            return Reader;
        }

        public IDataReader GetCheckList(string TravelAuthorizationID, string lookupID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.[GetCheckList]", TravelAuthorizationID, lookupID);
            return Reader;
        }

        public IDataReader GetCheckListByTAID(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetCheckListByTAID", TravelAuthorizationID);
            return Reader;
        }

        public IDataReader GetTAStatusHistory(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.[GetStatusHistory]", TravelAuthorizationID);

            return Reader;
        }

        public IDataReader GetWBSByTravelAuthorizationID(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetWBSByTravelAuthorizationID", TravelAuthorizationID);
            return Reader;
        }

        //delete

        public void DeleteTravelItinerary(string TravelItineraryID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.DeleteTravelItinerary", TravelItineraryID, CreatedBy);
        }

        public void DeleteWBS(string WBSID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.DeleteWBS", WBSID, CreatedBy);
        }

        public void DeleteCheckList(string TravelItineraryID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.DeleteCheckList", TravelItineraryID);
        }

        //insert-update

        public string InsertUpdateTravelAuthorization(string TravelAuthorizationID, string PRISMMissionCode, string UserID, string FirstName, string MiddleName, string LastName, string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, bool SecurityClearance, bool SecurityTraining, string StatusCode, string CreatedBy,
            string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs, bool IsAccommodationProvided, string AccommodationDetails, bool IsTravelAdvanceRequested, string TravelAdvanceCurrency, decimal TravelAdvanceAmount,
            string TravelAdvanceMethod, int IsVisaObtained, string VisaIssued, int IsVaccinationObtained, bool IsSecurityClearanceRequestedByMission, bool TAConfirm, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo, bool IsNotForDSA)
        {
            string TravelAuthorizationIDOut = "";
            TravelAuthorizationIDOut = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertUpdateTravelAuthorization",
                TravelAuthorizationID,
                PRISMMissionCode,
                UserID,
                FirstName,
                MiddleName,
                LastName,
                PurposeOfTravel,
                TripSchemaCode,
                ModeOfTravelCode,
                SecurityClearance,
                SecurityTraining,
                StatusCode,
                CreatedBy,
                CityOfAccommodation,
                IsPrivateStay,
                PrivateStayDates,
                IsPrivateDeviation,
                PrivateDeviationLegs,
                IsAccommodationProvided,
                AccommodationDetails,
                IsTravelAdvanceRequested,
                TravelAdvanceCurrency,
                TravelAdvanceAmount,
                TravelAdvanceMethod,
                IsVisaObtained,
                VisaIssued,
                IsVaccinationObtained,
                IsSecurityClearanceRequestedByMission,
                TAConfirm,
                PrivateStayDateFrom,
                PrivateStayDateTo,
                IsNotForDSA
                ).ToString();
            return TravelAuthorizationIDOut;
        }


        public string InsertUpdateTravelersInformation(string TravelAuthorizationID, string PRISMMissionCode, string UserID, string FirstName, string MiddleName, string LastName, string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, string StatusCode, string CreatedBy,
           string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs, bool IsAccommodationProvided, string AccommodationDetails, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo, string FamilyMember, string PRISMNumber, DateTime? LeaveStartDate,
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
bool IsNotify

           )
        {
            string TravelAuthorizationIDOut = "";
            TravelAuthorizationIDOut = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertUpdateTravelersInformation",
                TravelAuthorizationID,
                PRISMMissionCode,
                UserID,
                FirstName,
                MiddleName,
                LastName,
                PurposeOfTravel,
                TripSchemaCode,
                ModeOfTravelCode,
                StatusCode,
                CreatedBy,
                CityOfAccommodation,
                IsPrivateStay,
                PrivateStayDates,
                IsPrivateDeviation,
                PrivateDeviationLegs,
                IsAccommodationProvided,
                AccommodationDetails,
                PrivateStayDateFrom,
                PrivateStayDateTo,
                FamilyMember,
                PRISMNumber,
                LeaveStartDate,
                LeaveEndDate,
                RRStartDate,
                RREndDate, IsEmergency, ExtraCargoCode,
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
IsNotify
                ).ToString();
            return TravelAuthorizationIDOut;
        }


        /*
        public string InsertUpdateTravelersInformation(string TravelAuthorizationID, string PRISMMissionCode, string UserID, string FirstName, string MiddleName, string LastName, string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, string StatusCode, string CreatedBy,
            string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs, bool IsAccommodationProvided, string AccommodationDetails, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo, string FamilyMember, string PRISMNumber, DateTime? LeaveStartDate,  DateTime? LeaveEndDate, DateTime? RRStartDate, DateTime? RREndDate, bool IsEmergency)
        {
            string TravelAuthorizationIDOut = "";
            TravelAuthorizationIDOut = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertUpdateTravelersInformation",
                TravelAuthorizationID,
                PRISMMissionCode,
                UserID,
                FirstName,
                MiddleName,
                LastName,
                PurposeOfTravel,
                TripSchemaCode,
                ModeOfTravelCode,
                StatusCode,
                CreatedBy,
                CityOfAccommodation,
                IsPrivateStay,
                PrivateStayDates,
                IsPrivateDeviation,
                PrivateDeviationLegs,
                IsAccommodationProvided,
                AccommodationDetails,
                PrivateStayDateFrom,
                PrivateStayDateTo,
                FamilyMember,
                PRISMNumber,
                LeaveStartDate,
                LeaveEndDate,
                RRStartDate,
                RREndDate,IsEmergency
                ).ToString();
            return TravelAuthorizationIDOut;
        }


        */
        public void UpdateAdvanceAndSecurity(string TravelAuthorizationID, bool SecurityClearance, bool SecurityTraining, bool IsTravelAdvanceRequested, string TravelAdvanceCurrency, decimal TravelAdvanceAmount,
                  string TravelAdvanceMethod, int IsVisaObtained, string VisaIssued, int IsVaccinationObtained,
                  int IsDocumentsObtained,


                  int IsSecurityClearanceRequestedByMission, bool TAConfirm, bool IsNotForDSA, string ModifiedBy)
        {

            SqlParameter IsSecurityClearanceRequestedBy = new SqlParameter("@IsSecurityClearanceRequestedByMission", ValueOrDBNullIfZero(IsSecurityClearanceRequestedByMission));

            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.UpdateAdvanceAndSecurity",
                TravelAuthorizationID,
                SecurityClearance,
                SecurityTraining,
                IsTravelAdvanceRequested,
                TravelAdvanceCurrency,
                TravelAdvanceAmount,
                TravelAdvanceMethod,
                IsVisaObtained,
                VisaIssued,
                IsVaccinationObtained,
                IsDocumentsObtained,
                IsSecurityClearanceRequestedBy,
                TAConfirm,
                IsNotForDSA,
                ModifiedBy);
        }

        public IDataReader GetTAInfo(string TAID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTAInfo", TAID);
            return Reader;
        }

        public string TransferTA(string TANo, string To)
        {
            return (daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.TransferTA", TANo, To).ToString());
        }

        public string MapTA(string TANo, string Loc)
        {
            return (daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.MapTA", TANo, Loc).ToString());
        }

        public void UpdateTravelAuthorizationStatus(string TravelAuthorizationID, string StatusCode, string CreatedBy, string Comments)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.UpdateTravelAuthorizationStatus", TravelAuthorizationID, StatusCode, CreatedBy, Comments);
        }
        //UPDATED to transfer to owner

        //public void UpdateTravelAuthorizationOwner(string UserID, string CreatedBy, string UpdatedBy)
        //{
        //    daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.UpdateTravelAuthorizationOwner", UserID, CreatedBy, UpdatedBy);
        //}

        public void UpdateStaffTravelAuthorizationTA(string TravelAuthorizationNumber, string UserID, string CreatedBy, string UpdatedBy,string PRISMNumber)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.UpdateStaffTravelAuthorizationTA", TravelAuthorizationNumber, UserID, CreatedBy, UpdatedBy, PRISMNumber);
        }

        //public void TransferTA(string TravelAuthorizationID, string UserID, string CreatedBy, string UpdatedBy,string PRISMMissionCode)
        //{
        //    daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.TransferTAHR", TravelAuthorizationID, UserID, CreatedBy, UpdatedBy, PRISMMissionCode);
        //}






        public void UpdateTravelAuthorizationIsComplete(string TravelAuthorizationID, bool IsTecComplete, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.UpdateTravelAuthorizationIsComplete", TravelAuthorizationID, IsTecComplete, CreatedBy);
        }

        public string InsertUpdateTravelItinerary(string TravelItineraryID, string TravelAuthorizationNumber, string ModeOfTravelID, string FromLocationCode, DateTime FromLocationDate, string ToLocationCode, DateTime ToLocationDate, string CreatedBy, int Ordering)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertUpdateTravelItinerary", TravelItineraryID, TravelAuthorizationNumber, ModeOfTravelID, FromLocationCode, FromLocationDate, ToLocationCode, ToLocationDate, CreatedBy, Ordering).ToString();
            return result;
        }

        public string InsertTAStatus(string TravelAuthorizationID, string StatusCode, string RejectionReasons, string RejectionReasonType, string CreatedBy)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertTAStatus",
                TravelAuthorizationID, StatusCode, RejectionReasons, RejectionReasonType, CreatedBy).ToString();

            return result;

        }

        //public void InsertCancellationReasons(string TravelAuthorizationID, string CancellationReasonID, string CancellationReasonType, string CreatedBy, string CancellationComment)
        //{
        //    daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertCancellationReason",
        //        TravelAuthorizationID, CancellationReasonID, CancellationReasonType, CreatedBy, CancellationComment);
        //}

        public void InsertRejectionReasons(string TravelAuthorizationID, string RejectionReasonID, string RejectionReasonType, string CreatedBy, string RejectionComment)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertRejectionReason",
                TravelAuthorizationID, RejectionReasonID, RejectionReasonType, CreatedBy, RejectionComment);
        }

        public void InsertCheckList(string TravelAuthorizationID, string LookupID, int Value, string Note, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertCheckList",
                TravelAuthorizationID, LookupID, Value, Note, CreatedBy);
        }

        public string InsertUpdateWBS(string WBSID, string TravelAuthorizationID, string WBSCode, double PercentageOrAmount, string Note, bool IsPercentage, string CreatedBy)
        {
            string result = "";
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertUpdateWBS", WBSID, TravelAuthorizationID, WBSCode, PercentageOrAmount, Note, IsPercentage, CreatedBy).ToString();
            return result;
        }

        public IDataReader GetTAByTANo(string TANo,string UserID,int EmailTypeIDOnSubmission)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTAByTANo", TANo, UserID, EmailTypeIDOnSubmission);
            return Reader;
        }

        public IDataReader GetTAByTANoAndStep(string TANo,int Step)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTAByTANoAndStep", TANo,Step);
            return Reader;
        }

        

        public IDataReader GetTravelAuthorizationByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTravelAuthorizationByTravelAuthorizationNumber", TravelAuthorizationNumber);
            return Reader;
        }

        public void UpdateTravelItinerary(string TravelItineraryID, string ModeOfTravelID, string FromLocationCode, DateTime FromLocationDate, string ToLocationCode, DateTime ToLocationDate, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.UpdateTravelItinerary", TravelItineraryID, ModeOfTravelID, FromLocationCode, FromLocationDate, ToLocationCode, ToLocationDate, CreatedBy);
        }

        public void UpdateWBS(string WBSID, string WBSCode, double PercentageOrAmount, string Note, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.UpdateWBS", WBSID, WBSCode, PercentageOrAmount, Note, CreatedBy);
        }

        public string GetTAStepByTravelAuthorizationID(string TravelAuthorizationID)
        {
            string Step = "";
            Step = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTAStepByTravelAuthorizationID", TravelAuthorizationID).ToString();
            return Step;
        }

        public string GetTravelAuthorizationNumberByTANO(string TANO)
        {
            string Step = "";
            Step = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTravelAuthorizationNumberByTANO", TANO).ToString();
            return Step;
        }


        

        public void DuplicateTA(string TravelAuthorizationID, string UserID, string PRISMMissionCode, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.DuplicateTA", TravelAuthorizationID, UserID, PRISMMissionCode, CreatedBy);
        }

        public IDataReader CheckDuplicatedTA(string TravelAuthorizationID, string TravelersName, DateTime FromLocationDate, DateTime ToLocationDate, string FromLocationCode, string ToLocationCode, string TravelItineraryID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.CheckDuplicatedTA", TravelAuthorizationID, TravelersName, FromLocationDate, ToLocationDate, FromLocationCode, ToLocationCode, TravelItineraryID);

            return Reader;
        }

        public IDataReader GetTAStatusByTAID(string TravelAuthorizationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTAStatusByTAID", TravelAuthorizationID);
            return Reader;
        }

        // Insert TA Comment

        public void InsertTAComments(string CommentId, string TANumber, string Comment, string CommentType, string CreatedBy)
        {
            try
            {
                daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertUpdateTAComments",
                    CommentId, TANumber, CommentType, Comment, CreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public IDataReader GetTAComments(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetTAComments", TravelAuthorizationNumber);

            return Reader;
        }

        public IDataReader GetArrivalDateByTravelAuthorizationNumber(string TravelAuthorizationNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.GetArrivalDateByTravelAuthorizationNumber", TravelAuthorizationNumber);
            return Reader;
        }

        public void DeleteTravelathorization(string TravelAuthorizationID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.DeleteTravelathorization", TravelAuthorizationID, CreatedBy);
        }

        public void InsertTADocumentNumber(string TravelAuthorizationID, string DocumentNumber, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "TA.InsertTADocumentNumber", TravelAuthorizationID, DocumentNumber, CreatedBy);
        }


        public string CheckLeaveRelationShip(string code)
        {
            string _out = "0";
            _out = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.CheckLeaveRelationShip", code).ToString();
            return _out;
        }

        public string CheckRandRRelationShip(string code)
        {
            string _out = "0";
            _out = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.CheckRandRRelationShip", code).ToString();
            return _out;
        }

    }
}
