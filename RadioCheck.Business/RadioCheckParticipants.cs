using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAuthorization;

namespace RadioCheckBusiness
{
    public class RadioCheck
    {
        private RadioCheckData.RadioCheck daRC = new RadioCheckData.RadioCheck();
        HttpContext context = HttpContext.Current;

        #region Lookups

            public void GetAllLookupsList(ref DataSet ds)
            {
                ds = daRC.GetAllLookupsList();
            }

            public void GetUnitsByDepartmentID(string DepartmentID, ref DataSet ds)
            {
                ds = daRC.GetUnitsByDepartmentID(DepartmentID);
            }

            public void GetSubUnitsByUnitID(string UnitID, ref DataSet ds)
            {
                ds = daRC.GetSubUnitsByUnitID(UnitID);
            }

            public void GetZonesByLocationID(string LocationID, ref DataSet ds)
            {
                ds = daRC.GetZonesByLocationID(LocationID);
            }

            public string GetStaffLocation(string PERNO)
            {
                string LocationID = "";
                LocationID = daRC.GetStaffLocation(PERNO).ToString();
                return LocationID;
            }

        public void GetMRReturnReasons(ref DataTable dt)
        {
            dt.Load(daRC.GetMRReturnReasons());
        }

        public void GetReturnReasons(ref DataTable dt)
        {
            dt.Load(daRC.GetReturnReasons());
        }

        public void GetCancellationReasons(ref DataTable dt)
        {
            dt.Load(daRC.GetCancellationReasons());
        }

        public void GetLeaveCategoryDescription(string LeaveCategoryDescription, ref DataTable dt)
        {
            dt.Load(daRC.GetLeaveCategoryDescription(LeaveCategoryDescription));
        }

        #endregion

        #region Zone Setup

        public void GetAllLocations(ref DataTable dt)
        {
            dt.Load(daRC.GetAllLocations());
        }

        public void GetWardenLocations(ref DataTable dt, string UserID)
        {
            dt.Load(daRC.GetWardenLocations(UserID));
        }

        public void GetRadioOperatorLocations(ref DataTable dt, string UserID)
        {
            dt.Load(daRC.GetRadioOperatorLocations(UserID));
        }

        public void GetAllZonesByLocationID(string LocationID, ref DataTable dt)
        {
            dt.Load(daRC.GetAllZonesByLocationID(LocationID));
        }

        public void GetAllStaffByLocationID(string LocationID, string SearchText, ref DataTable dt)
        {
            dt.Load(daRC.GetAllStaffByLocationID(LocationID, SearchText));
        }

        public int InsertZones(string ZoneID, string ZoneName, string LocationID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daRC.InsertZones(ZoneID, ZoneName, LocationID, ui.User_Id);
        }

        public void DeleteZone(string ZoneID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.DeleteZone(ZoneID, ui.User_Id);
        }

        public void GetAllResidenceForSelectedZone(string ZoneNo, ref DataSet ds)
        {
            ds = daRC.GetAllResidenceForSelectedZone(ZoneNo);
        }

        public void GetAllResidenceForSelectedLocation(string LocationID, ref DataSet ds)
        {
            ds = daRC.GetAllResidenceForSelectedLocation(LocationID);
        }

        public void GetAllResidencesbyZoneID(string ZoneID, ref DataTable dt)
        {
            dt.Load(daRC.GetAllResidencesbyZoneID(ZoneID));
        }

        public void GetStaffNumberUnderZoneResidence(string ZoneID, string ResidenceID, ref DataTable dt)
        {
            dt.Load(daRC.GetStaffNumberUnderZoneResidence(ZoneID, ResidenceID));
        }

        public int InsertResidences(string ResidenceID, string ZoneID, string LocationID, string ResidenceName)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daRC.InsertResidences(ResidenceID, ZoneID, LocationID, ResidenceName, ui.User_Id);
        }

        public void DeleteResidence(string ResidenceID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.DeleteResidence(ResidenceID, ui.User_Id);
        }
        #endregion

        #region Staff Management
        public void GetAllStaff(string SearchText, ref DataTable dt)
        {
            dt.Load(daRC.GetAllStaff(SearchText));
        }

        public void SearchResidences(string SearchText, ref DataTable dt)
        {
            dt.Load(daRC.SearchResidences(SearchText));
        }

        public void SearchRegisteredStaff(string SearchText, ref DataTable dt)
        {
            dt.Load(daRC.SearchRegisteredStaff(SearchText));
        }

        public string GetStaffInformationStep(string PERNO)
        {
            string Step = "";
            Step = daRC.GetStaffInformationStep(PERNO).ToString();
            return Step;
        }

        public void GetProfileInformation(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.GetProfileInformation(ui.PRISMNumber));
        }

        public bool UpdateMyProfile(string UserName, string DisplayName, string Gender, string Title, string UNIDNo, string Residence, string Country)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return Convert.ToBoolean(daRC.UpdateMyProfile(UserName, DisplayName, Gender, Title, UNIDNo, Residence, Country, ui.User_Id));
        }



        public void GetStaffInformationByPersonalNumber(string PERNO, ref DataTable dt)
        {
            dt.Load(daRC.GetStaffInformationByPersonalNumber(PERNO));
        }

        public void GetStaffInformationByUserIDForRegistration(string UserID, ref DataTable dt)
        {
            dt.Load(daRC.GetStaffInformationByUserIDForRegistration(UserID));
        }

        public void GetStaffInformationByUserID(string UserID, ref DataTable dt)
        {
            dt.Load(daRC.GetStaffInformationByUserID(UserID));
        }

        public void GetRegisteredStaffInformationByUserID(string UserID, ref DataTable dt)
        {
            dt.Load(daRC.GetRegisteredStaffInformationByUserID(UserID));
        }

        public string DeactivateStaff(string PERNO)
        {
            string Result = "";
            Result = daRC.DeactivateStaff(PERNO).ToString();
            return Result;
        }

        public string GetStaffLocationName(string LocationID)
        {
            string Name = "";
            Name = daRC.GetStaffLocationName(LocationID).ToString();
            return Name;
        }

        public string GetZoneName(string ZoneID)
        {
            string Name = "";
            Name = daRC.GetZoneName(ZoneID).ToString();
            return Name;
        }

        public string GetStaffName(string PERNO)
        {
            string Name = "";
            Name = daRC.GetStaffName(PERNO).ToString();
            return Name;
        }

        public string InsertUpdateStaffInformation(string PERNO, string UserName, string DisplayName, string Department, string Unit, string SubUnit, string Location, string Nationality, string CallSign,string Gender, bool Active)
        {
            string Step = "";
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            Step = daRC.InsertUpdateStaffInformation(PERNO, UserName, DisplayName, Department, Unit, SubUnit, Location, Nationality, CallSign, Gender, Active, ui.User_Id);
            return Step;
        }

        public string InsertUpdateStaffResidence(string PERNO, string Residence, string ResidenceType)
        {
            string Step = "";
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            Step = daRC.InsertUpdateStaffResidence(PERNO, Residence, ResidenceType, ui.User_Id);
            return Step;
        }

        public void InsertUpdateStaffInformation(string PERNO, string DutyStation, string UnitDepartment, string Category)
        {
            daRC.InsertUpdateStaffInformation(PERNO, DutyStation, UnitDepartment, Category);
        }

        public void GetStaffContacts(string PERNO, ref DataTable dt)
        {
            dt.Load(daRC.GetStaffContacts(PERNO));
        }

        public void GetStaffEmergencyContacts(string PERNO, ref DataTable dt)
        {
            dt.Load(daRC.GetStaffEmergencyContacts(PERNO));
        }

        public void DeleteStaffContacts(string ContactID)
        {
            daRC.DeleteStaffContacts(ContactID);
        }

        public void DeleteStaffEmergencyContacts(string ContactID)
        {
            daRC.DeleteStaffEmergencyContacts(ContactID);
        }

        public void GetContactDetailsByContactID(string ContactID, ref DataTable dt)
        {
            dt.Load(daRC.GetContactDetailsByContactID(ContactID));
        }

        public void GetContactTypeDescription(string ContactTypeDescription, ref DataTable dt)
        {
            dt.Load(daRC.GetContactTypeDescription(ContactTypeDescription));
        }

        public void GetRelationshipTypeDescription(string RelationshipTypeDescription, ref DataTable dt)
        {
            dt.Load(daRC.GetRelationshipTypeDescription(RelationshipTypeDescription));
        }

        public void CheckDuplicatedContact(string PERNO, string ContactTypeCode, string ContactDetails, ref DataTable dt)
        {
            dt.Load(daRC.CheckDuplicatedContact(PERNO, ContactTypeCode, ContactDetails));
        }

        public void CheckDuplicatedEmergencyContact(string PERNO, string RelationTypeCode, string ContactName, string ContactDetails, ref DataTable dt)
        {
            dt.Load(daRC.CheckDuplicatedEmergencyContact(PERNO, RelationTypeCode, ContactName, ContactDetails));
        }

        public string InsertUpdateStaffContact(string ContactID, string PERNO, string ContactTypeCode, string ContactDetails, int Ordering)
        {
            string result;
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            result = daRC.InsertUpdateStaffContact(ContactID, PERNO, ContactTypeCode, ContactDetails, ui.User_Id, Ordering).ToString();
            return result;
        }

        public string InsertUpdateStaffEmergencyContact(string ContactID, string PERNO, string ContactTypeCode, string NameOfContactPerson, string ContactDetails, int Ordering)
        {
            string result;
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            result = daRC.InsertUpdateStaffEmergencyContact(ContactID, PERNO, ContactTypeCode, NameOfContactPerson, ContactDetails, ui.User_Id, Ordering).ToString();
            return result;
        }

        #endregion

        #region Radio Check
        public void GetRadioCheckRollCall(ref DataTable dt, DateTime CheckDate, string LocationID)
        {
            dt.Load(daRC.GetRadioCheckRollCall(CheckDate, LocationID));
        }
        public void InsertDeleteRadioCheckInformation(string StaffCallSign, string StaffPERNO, string LocationID, bool HasResponded)
        {
            daRC.InsertDeleteRadioCheckInformation(StaffCallSign, StaffPERNO, LocationID, HasResponded);
        }

        public void GetRadioCheckSummary(ref DataTable dt, DateTime? RadCheckDate, string LocationID)
        {
            dt.Load(daRC.GetRadioCheckSummary(RadCheckDate, LocationID));
        }

        public string GetRadioCheckStepByRadioCheckDate(DateTime RadioCheckDate)
        {
            string Step = "";
            Step = daRC.GetRadioCheckStepByRadioCheckDate(RadioCheckDate).ToString();
            return Step;
        }

        public string GetRadioCheckDateByRadioCheckID(string RadioCheckID)
        {
            string Date = "";
            Date = daRC.GetRadioCheckDateByRadioCheckID(Convert.ToInt32(RadioCheckID)).ToString();
            return Date;
        }
        public string GetRadioCheckStartDateByRadioCheckID(string RadioCheckID)
        {
            string Date = "";
            Date = daRC.GetRadioCheckDateByRadioCheckID(Convert.ToInt32(RadioCheckID)).ToString();
            return Date;
        }

        public string GetRadioCheckEndDateByRadioCheckID(string RadioCheckID)
        {
            string Date = "";
            Date = daRC.GetRadioCheckEndDateByRadioCheckID(Convert.ToInt32(RadioCheckID)).ToString();
            return Date;
        }

        public string GetLatestRadioCheckIDByRadioCheckDate()
        {
            string RCID = "";
            RCID = daRC.GetLatestRadioCheckIDByRadioCheckDate();
            return RCID;
        }


        public string GetRadioCheckIDByRadioCheckDate(DateTime? RadioCheckDate)
        {
            string RCID = "";
            RCID = daRC.GetRadioCheckIDByRadioCheckDate(RadioCheckDate);
            return RCID;
        }

        public string RadioCheckStartIDByRadioCheckDate(DateTime? RadioCheckDate)
        {
            string RCID = "";
            RCID = daRC.RadioCheckStartIDByRadioCheckDate(RadioCheckDate);
            return RCID;
        }

        public string RadioCheckEndIDByRadioCheckDate(DateTime? RadioCheckDate)
        {
            string RCID = "";
            RCID = daRC.RadioCheckEndIDByRadioCheckDate(RadioCheckDate);
            return RCID;
        }

        public void GetMasterStaffList(ref DataTable dt, string LocationID)
        {
            dt.Load(daRC.GetMasterStaffList(LocationID));
        }

        public void GetStaffAccountedFor(ref DataTable dt, string RadioCheckID, string LocationID)
        {
            dt.Load(daRC.GetStaffAccountedFor(Convert.ToInt32(RadioCheckID), LocationID));
        }

        public void GetStaffUnAccountedFor(ref DataTable dt, string RadioCheckDate, string LocationID)
        {
            dt.Load(daRC.GetStaffUnAccountedFor(Convert.ToInt32(RadioCheckDate), LocationID));
        }

        public void GetStaffMovements(ref DataTable dt, string RadioCheckDate, string LocationID)
        {
            dt.Load(daRC.GetStaffMovements(Convert.ToInt32(RadioCheckDate), LocationID));
        }

        #endregion

        #region Notification of Absence

        public void GetUserInfoByUserID(string UserID, ref DataTable dt)
        {
            dt.Load(daRC.GetUserInfoByUserID(UserID));
        }

        public void GetStaffMembersByDepartmentID(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.GetStaffMembersByDepartmentID(ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }

        public void GetStaffMovementRequestsByDepartmentID(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.GetStaffMovementRequestsByDepartmentID(ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }

        public void SearchStaffMovementRequestsByDepartmentID(ref DataTable dt,string MovementRequestNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.SearchStaffMovementRequestsByDepartmentID(MovementRequestNumber,ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }

        public void GetEmailContent(ref DataTable dt, int EmailTypeId)
        {
            dt.Load(daRC.GetEmailContent(EmailTypeId));
        }

        public void GetEmailReceipients(ref DataTable dt, string TANumber)
        {
            dt.Load(daRC.GetEmailReceipients(TANumber));
        }

        public void SendTAPendingNotificationEmail(string TANo,int EmailTypeID)
        {
            daRC.SendTAPendingNotificationEmail(TANo, EmailTypeID);
        }

        public void SendTANotificationToOwner(string TANo, int EmailTypeID)
        {
            daRC.SendTANotificationToOwner(TANo, EmailTypeID);
        }

        public void GetMREmailReceipients(ref DataTable dt, string MRNumber)
        {
            dt.Load(daRC.GetMREmailReceipients(MRNumber));
        }

        public void GetEmailReceiver(ref DataTable dt, string UserID)
        {
            dt.Load(daRC.GetEmailReceiver(UserID));
        }


        public void GetStaffMovementRequest(ref DataTable dt, string MovementRequestNumber, string Status)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.GetStaffMovementRequest(ui.User_Id, MovementRequestNumber, Status));
        }

        public void SearchStaffMovementRequest(ref DataTable dt, string MovementRequestNumber, string Status)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.SearchStaffMovementRequest(ui.User_Id, MovementRequestNumber, Status));
        }

        public void GetNotificationsOfAbsence(ref DataTable dt, string MRNO)
        {
            //Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.GetNotificationsOfAbsence(MRNO));
        }
        //add detail


        public void GetStaffMovementRequestTR(ref DataTable dt, string MovementRequestNumber, string Status)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.GetStaffMovementRequest(ui.User_Id, MovementRequestNumber, Status));
        }
        //

        //added for the transfer of the member TA

        public void GetStaffMovementRequestDetails(ref DataTable dt, string MovementRequestNumber, string Status, string location, string CreatedBy, string UpdatedBy, string StatusCode)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.GetStaffMovementRequestDetails(ui.User_Id, MovementRequestNumber, Status, location, CreatedBy, UpdatedBy, StatusCode));
        }


        public void GetMovementRequestByMovementRequestID(string MovementRequestID, ref DataTable dt)
        {
            dt.Load(daRC.GetMovementRequestByMovementRequestID(MovementRequestID));
        }

        public void GetMRByMRNo(ref DataTable dt, string MRNo)
        {
            dt.Load(daRC.GetMRByMRNo(MRNo));
        }

        public void GetSupervisors(string MovementRequestID, ref DataTable dt)
        {
            dt.Load(daRC.GetSupervisors(MovementRequestID));
        }

        public void SearchMovementRequest(string MovementRequestNumber, string WBS, string Status, string location, ref DataTable dt)
        {
            dt.Load(daRC.SearchMovementRequest(MovementRequestNumber, WBS, Status, location));
        }

        public void SearchMovementRequestForManagers(string MovementRequestNumber, string Status, ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daRC.SearchMovementRequestForManagers(MovementRequestNumber, Status, ui.LocationID, ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }



        public void GetStaffMovementRequestDetails(ref DataTable dt, string v, string status, string location, object createdBy, object updatedBy, object statusCode)
        {
            throw new NotImplementedException();
        }

        public void GetLeaveItineraryByLeaveRequestID(string LeaveRequestID, ref DataTable dt)
        {
            dt.Load(daRC.GetLeaveItineraryByLeaveRequestID(LeaveRequestID));
        }


        public void GetMRStatusHistory(string MovementRequestID, ref DataTable dt)
        {
            dt.Load(daRC.GetMRStatusHistory(MovementRequestID));
        }

        public void GetWBSByMovementRequestID(string MovementRequestID, ref DataTable dt)
        {
            dt.Load(daRC.GetWBSByMovementRequestID(MovementRequestID));
        }

        //delete
        public void DeleteLeaveItinery(string LeaveItineryID)
        {
            daRC.DeleteLeaveItinery(LeaveItineryID);
        }

        public void DeleteWBS(string WBSID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.DeleteWBS(WBSID, ui.User_Id);
        }

        public void DeleteCheckList(string LeaveItineryID)
        {
            daRC.DeleteCheckList(LeaveItineryID);
        }

        //insert - update
        public string InsertUpdateMovementRequest(string MovementRequestID, string FirstName, string MiddleName, string LastName,
            string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, bool SecurityClearance,
            bool SecurityTraining, string StatusCode, string userId,
            string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs,
            bool IsAccommodationProvided, string AccommodationDetails, bool IsTravelAdvanceRequested, string TravelAdvanceCurrency, decimal TravelAdvanceAmount,
            string TravelAdvanceMethod, int IsVisaObtained, string VisaIssued, int IsVaccinationObtained, bool IsSecurityClearanceRequestedByMission, bool TAConfirm, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo, bool IsNotForDSA)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string MovementRequestIDOut = "";
            MovementRequestIDOut = daRC.InsertUpdateMovementRequest(MovementRequestID,
                System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(),
                userId, FirstName, MiddleName, LastName, PurposeOfTravel, TripSchemaCode, ModeOfTravelCode, SecurityClearance, SecurityTraining, StatusCode, ui.User_Id,
                CityOfAccommodation, IsPrivateStay, PrivateStayDates, IsPrivateDeviation, PrivateDeviationLegs, IsAccommodationProvided, AccommodationDetails,
                IsTravelAdvanceRequested, TravelAdvanceCurrency, TravelAdvanceAmount, TravelAdvanceMethod, IsVisaObtained, VisaIssued, IsVaccinationObtained,
                IsSecurityClearanceRequestedByMission, TAConfirm, PrivateStayDateFrom, PrivateStayDateTo, IsNotForDSA).ToString();
            return MovementRequestIDOut;
        }

        public string InsertUpdateRequestersInformation(string MovementRequestID, string UserID, string FirstName, string MiddleName, string LastName, string LeaveCategoryCode, string PurposeOfLeave, DateTime? StartDate, DateTime? EndDate,
            bool? TravellingOut, string PRISMNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string MovementRequestIDOut = "";
            MovementRequestIDOut = daRC.InsertUpdateRequesterInformation(MovementRequestID,
                System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(), UserID, FirstName, MiddleName, LastName, LeaveCategoryCode, PurposeOfLeave, StartDate, EndDate, TravellingOut, ui.User_Id, PRISMNumber).ToString();
            return MovementRequestIDOut;
        }

        public string InsertUpdateNotifierInformation(string MovementRequestID, string UserID, string FirstName, string MiddleName, string LastName, string LeaveCategoryCode, string PurposeOfLeave, string PRISMNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string MovementRequestIDOut = "";
            MovementRequestIDOut = daRC.InsertUpdateNotifierInformation(MovementRequestID,
                System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(), UserID, FirstName, MiddleName, LastName, LeaveCategoryCode, PurposeOfLeave, ui.User_Id, PRISMNumber).ToString();
            return MovementRequestIDOut;
        }

        public void UpdateSecurity(string MovementRequestID, bool SecurityClearance, bool MRConfirm)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.UpdateSecurity(MovementRequestID, SecurityClearance, ui.User_Id, MRConfirm);
        }

        public void UpdateMovementRequestDetails(string MovementRequestID, DateTime? StartDate, DateTime? EndDate, string Addresswhileabsent, bool TravellingOut, bool MRConfirm)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.UpdateMovementRequestDetails(MovementRequestID, StartDate, EndDate, Addresswhileabsent, TravellingOut, MRConfirm, ui.User_Id);
        }

        public string UserHasDuplicateNotification(string MovementRequestID,DateTime? StartDate, DateTime? EndDate)
        {
            string Ret = "";
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            Ret = daRC.UserHasDuplicateNotification(ui.User_Id, MovementRequestID, StartDate, EndDate);
            return Ret;
        }

        public string IsValidDateRange(string MovementRequestID, DateTime? StartDate, DateTime? EndDate)
        {
            string Ret = "";
            Ret = daRC.IsValidDateRange(MovementRequestID, StartDate, EndDate);
            return Ret;
        }


        public void UpdateMovementRequestStatus(string MovementRequestID, string StatusCode, string Comments)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.UpdateMovementRequestStatus(MovementRequestID, StatusCode, ui.User_Id, Comments);
        }


        public void UpdateMovementRequestIsComplete(string MovementRequestID, bool IsTecComplete)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.UpdateMovementRequestIsComplete(MovementRequestID, IsTecComplete, ui.User_Id);
        }

        public string InsertMRStatus(string MovementRequestID, string StatusCode, string RejectionReasons, string RejectionReasonType)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string result;
            result = daRC.InsertMRStatus(MovementRequestID, StatusCode, RejectionReasons, RejectionReasonType, ui.User_Id).ToString();
            return result;
        }
        public void InsertRejectionReasons(string MovementRequestID, string RejectionReasonID, string RejectionReasonType, string RejectionComment)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.InsertRejectionReasons(MovementRequestID, RejectionReasonID, RejectionReasonType, ui.User_Id, RejectionComment);
        }

        public void InsertCheckList(string MovementRequestID, string LookupID, int Value, string Note)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.InsertCheckList(MovementRequestID, LookupID, Value, Note, ui.User_Id);
        }

        public string InsertUpdateLeaveItinery(string MovementItineraryID, string MovementRequestID, string LeaveCategoryCode, DateTime DateFrom, DateTime DateTo, int Ordering)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string result;
            result = daRC.InsertUpdateLeaveItinery(MovementItineraryID, MovementRequestID, LeaveCategoryCode, DateFrom, DateTo, ui.User_Id, Ordering).ToString();
            return result;
        }

        public void UpdateLeaveItinery(string LeaveItineryID, string ModeOfTravelID, string FromLocationCode, DateTime FromLocationDate, string ToLocationCode, DateTime ToLocationDate)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.UpdateLeaveItinery(LeaveItineryID, ModeOfTravelID, FromLocationCode, FromLocationDate, ToLocationCode, ToLocationDate, ui.User_Id);
        }

        public void UpdateWBS(string WBSID, string WBSCode, double PercentageOrAmount, string Note)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.UpdateWBS(WBSID, WBSCode, PercentageOrAmount, Note, ui.User_Id);
        }

        public void InsertMRComments(string CommentId, string MRNumber, string CommentType, string Comment)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.InsertMRComments(CommentId, MRNumber, Comment, CommentType, ui.User_Id);
        }

        public void GetRejectionReason(string MovementRequestID, string RejectionReasonType, ref DataTable dt)
        {
            dt.Load(daRC.GetRejectionReason(MovementRequestID, RejectionReasonType));
        }

        public void GetMRReturnReason(string MovementRequestID, string RejectionReasonType, ref DataTable dt)
        {
            dt.Load(daRC.GetMRReturnReason(MovementRequestID, RejectionReasonType));
        }

       

        public void GetMRCancellationReason(string MovementRequestID, string RejectionReasonType, ref DataTable dt)
        {
            dt.Load(daRC.GetMRCancellationReason(MovementRequestID, RejectionReasonType));
        }

        public void GetCheckList(string MovementRequestID, string lookupID, ref DataTable dt)
        {
            dt.Load(daRC.GetCheckList(MovementRequestID, lookupID));
        }

        public void GetCheckListByMRID(string MovementRequestID, ref DataTable dt)
        {
            dt.Load(daRC.GetCheckListByMRID(MovementRequestID));
        }

        public void GetMRComments(string MovementRequestNumber, ref DataTable dt)
        {
            dt.Load(daRC.GetMRComments(MovementRequestNumber));
        }


        public void UpdateStaffMovementRequestTA(string MovementRequestNumber, string UserID, string CreatedBy, string UpdatedBy, string PRISMNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.UpdateStaffMovementRequestTA(MovementRequestNumber, ui.User_Id, CreatedBy, UpdatedBy, PRISMNumber);
        }

        public string InsertUpdateWBS(string WBSID, string MovementRequestID, string WBSCode, double PercentageOrAmount, string Note, bool IsPercentage)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            string result = "";
            result = daRC.InsertUpdateWBS(WBSID, MovementRequestID, WBSCode, PercentageOrAmount, Note, IsPercentage, ui.User_Id).ToString();
            return result;

        }

        public void GetMovementRequestByMovementRequestNumber(string MovementRequestNumber, ref DataTable dt)
        {
            dt.Load(daRC.GetMovementRequestByMovementRequestNumber(MovementRequestNumber));
        }

        public void GetArrivalDateByMovementRequestNumber(string MovementRequestNumber, ref DataTable dt)
        {
            dt.Load(daRC.GetArrivalDateByMovementRequestNumber(MovementRequestNumber));
        }

        public void DeleteMovementRequest(string MovementRequestID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.DeleteMovementRequest(MovementRequestID, ui.User_Id);
        }

        public string GetMRStepByMovementRequestID(string MovementRequestID)
        {
            string Step = "";
            Step = daRC.GetMRStepByMovementRequestID(MovementRequestID).ToString();
            return Step;
        }

        public string GetMRTravelOutStatusByMovementRequestID(string MovementRequestID)
        {
            string Status = "";
            Status = daRC.GetMRTravelOutStatusByMovementRequestID(MovementRequestID).ToString();
            return Status;
        }



        public void DuplicateMR(string MovementRequestID, string UserID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.DuplicateMR(MovementRequestID, UserID, System.Configuration.ConfigurationManager.AppSettings["PRISMMissionCode"].ToString(), ui.User_Id);
        }

        public void CheckDuplicatedMR(string MovementRequestID, string TravelersName, DateTime DateFrom, DateTime DateTo, string LeaveCategoryCode, string LeaveItineryID, ref DataTable dt)
        {
            dt.Load(daRC.CheckDuplicatedMR(MovementRequestID, TravelersName, DateFrom, DateTo, LeaveCategoryCode, LeaveItineryID));
        }

        public void GetMRStatusByMRID(string MovementRequestID, ref DataTable dt)
        {
            dt.Load(daRC.GetMRStatusByMRID(MovementRequestID));
        }

        public void InsertMRDocumentNumber(string MovementRequestID, string DocumentNumber)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRC.InsertMRDocumentNumber(MovementRequestID, DocumentNumber, ui.User_Id);

        }

        #endregion

    }
}
