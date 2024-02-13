using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAuthorizationSystem.Utility;

namespace RadioCheckData
{
    public class RadioCheck
    {

        #region Lookups
        public DataSet GetAllLookupsList()
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "rclkp.GetAllLookupsList");
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

        public IDataReader GetLeaveCategoryDescription(string Description)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "rclkp.GetLeaveCategoryDescription", Description);
            return Reader;
        }

        #endregion

        #region Zone Setup

        public IDataReader GetAllLocations()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllLocations");
            return Reader;
        }

        public IDataReader GetWardenLocations(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetWardenLocations", UserID);
            return Reader;
        }

        public IDataReader GetRadioOperatorLocations(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetRadioOperatorLocations", UserID);
            return Reader;
        }

        public IDataReader GetAllZonesByLocationID(string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllZonesByLocationID", LocationID);
            return Reader;
        }

        public IDataReader GetAllStaffByLocationID(string LocationID, string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllStaffByLocationID", LocationID, SearchText);
            return Reader;
        }


        public int InsertZones(string ZoneID, string ZoneName, string LocationID, string ActionBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.InsertZones", ZoneID, LocationID, ZoneName, ActionBy).ToString());
        }

        public void DeleteZone(string ZoneID, string ModifiedBy)
        {
            daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.DeleteZone", ZoneID, ModifiedBy);
        }

        public IDataReader GetAllResidencesbyZoneID(string ZoneID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllResidencesbyZoneID", ZoneID);
            return Reader;
        }

        public DataSet GetAllResidenceForSelectedZone(string ZoneID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllResidenceForSelectedZone", ZoneID);
            return ds;
        }

        public DataSet GetAllResidenceForSelectedLocation(string LocationID)
        {
            DataSet ds = daHelper.ExecuteDataset(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllResidenceForSelectedLocation", LocationID);
            return ds;
        }

        

        public IDataReader GetStaffNumberUnderZoneResidence(string ZoneID, string ResidenceID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetStaffNumberUnderZoneResidence", ZoneID, ResidenceID);
            return Reader;
        }

        public int InsertResidences(string ResidenceID, string ZoneID, string LocationID, string ResidenceName, string ActionBy)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.InsertResidences", ResidenceID, ZoneID, LocationID, ResidenceName, ActionBy).ToString());
        }

        public void DeleteResidence(string ResidenceID, string ModifiedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.DeleteResidence", ResidenceID, ModifiedBy);
        }
        #endregion

        #region Staff Management
        public IDataReader GetAllStaff(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetAllStaff]", SearchText);
            return Reader;
        }

        public IDataReader SearchRegisteredStaff(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[SearchRegisteredStaff]", SearchText);
            return Reader;
        }

        public IDataReader SearchResidences(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.SearchResidences", SearchText);
            return Reader;
        }

        public string GetStaffInformationStep(string PERNO)
        {
            string Step = "";
            Step = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffInformationStep]", PERNO).ToString();
            return Step;
        }

        public IDataReader GetProfileInformation (string PERNO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[SEC].[GetProfileInformation]", PERNO);
            return Reader;
        }


        public IDataReader GetStaffInformationByPersonalNumber(string PERNO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffInformationByPersonalNumber]", PERNO);
            return Reader;
        }

        public int UpdateMyProfile(string UserName, string DisplayName, string Gender, string Title, string UNIDNo, string Residence, string Country, string ModifiedBy)
        {
            int  Step = 0;
            Step = Convert.ToInt32(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.UpdateMyProfile", UserName, DisplayName, Gender, Title, UNIDNo, Residence, Country, ModifiedBy).ToString());
            return Step;
        }

        public IDataReader GetStaffInformationByUserIDForRegistration(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffInformationByUserIDForRegistration]", UserID);
            return Reader;
        }

        public IDataReader GetStaffInformationByUserID(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffInformationByUserID]", UserID);
            return Reader;
        }

        public IDataReader GetRegisteredStaffInformationByUserID(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRegisteredStaffInformationByUserID]", UserID);
            return Reader;
        }

        public string InsertUpdateStaffInformation(string PERNO, string UserName, string DisplayName, string Department, string Unit, string SubUnit, string Location, string Nationality, string CallSign,string Gender, bool Active, string CreatedBy)
        {
            string PRNO = "";
            PRNO = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[InsertUpdateStaffInformation]", PERNO, UserName, DisplayName, Department, Unit, SubUnit, Location, Nationality, CallSign,Gender, Active, CreatedBy).ToString();
            return PRNO;
        }

        public string GetStaffName(string PERNO)
        {
            string StaffName = "";
            StaffName = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffName]", PERNO).ToString();
            return StaffName;
        }

        public string GetStaffLocationName(string LocationID)
        {
            string LocationName = "";
            LocationName = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffLocationName]", LocationID).ToString();
            return LocationName;
        }

        public string GetZoneName(string ZoneID)
        {
            string ZoneName = "";
            ZoneName = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetZoneName]", ZoneID).ToString();
            return ZoneName;
        }

        public string DeactivateStaff(string PERNO)
        {
            string Result = "";
            Result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[DeactivateStaff]", PERNO).ToString();
            return Result;
        }

        public string InsertUpdateStaffResidence(string PERNO, string Residence, string ResidenceType, string CreatedBy)
        {
            string PRNO = "";
            PRNO = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[InsertUpdateStaffResidence]", PERNO, Residence, ResidenceType, CreatedBy).ToString();
            return PRNO;
        }

        public void InsertUpdateStaffInformation(string PERNO, string DutyStation, string UnitDepartment, string Category)
        {
            daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[InsertUpdateStaffInformation2]", PERNO, DutyStation, UnitDepartment, Category).ToString();
        }

        public IDataReader GetStaffContacts(string PERNO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetStaffContacts", PERNO);
            return Reader;
        }

        public IDataReader GetStaffEmergencyContacts(string PERNO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetStaffEmergencyContacts", PERNO);
            return Reader;
        }

        public IDataReader GetStaffResidence(string PERNO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetStaffResidence", PERNO);
            return Reader;
        }

        public void DeleteStaffContacts(string ContactID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.DeleteStaffContact", ContactID);
        }

        public void DeleteStaffEmergencyContacts(string ContactID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.DeleteStaffEmergencyContacts", ContactID);
        }

        public IDataReader GetContactDetailsByContactID(string ContactID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetContactDetailsByContactID", ContactID);
            return Reader;
        }

        public IDataReader GetContactTypeDescription(string Description)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetContactTypeDescription", Description);
            return Reader;
        }

        public IDataReader GetRelationshipTypeDescription(string Description)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetRelationshipTypeDescription", Description);
            return Reader;
        }



        public IDataReader CheckDuplicatedContact(string PERNO, string ContactTypeCode, string ContactDetails)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.CheckDuplicatedContact", PERNO, ContactTypeCode, ContactDetails);
            return Reader;
        }

        public IDataReader CheckDuplicatedEmergencyContact(string PERNO, string RelationTypeCode, string ContactName, string ContactDetails)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.CheckDuplicatedEmergencyContact", PERNO, RelationTypeCode, ContactName, ContactDetails);
            return Reader;
        }

        public string InsertUpdateStaffContact(string ContactID, string PERNO, string ContactTypeCode, string ContactDetails, string CreatedBy, int Ordering)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.InsertUpdateStaffContact", ContactID, PERNO, ContactTypeCode, ContactDetails, CreatedBy, Ordering).ToString();
            return result;
        }

        public string InsertUpdateStaffEmergencyContact(string ContactID, string PERNO, string RelationTypeCode, string NameOfContactPerson, string ContactDetails, string CreatedBy, int Ordering)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.InsertUpdateStaffEmergencyContact", ContactID, PERNO, RelationTypeCode, NameOfContactPerson, ContactDetails, CreatedBy, Ordering).ToString();
            return result;
        }

        #endregion

        #region Radio Check

        public IDataReader GetRadioCheckRollCall(DateTime RadCheckDate, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckRollCall]", RadCheckDate, LocationID);
            return Reader;
        }

        public IDataReader InsertDeleteRadioCheckInformation(string CallSign, string PERNO, string LocationID, bool Responded)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[InsertDeleteRadioCheckInformation]", CallSign, PERNO, LocationID, Responded);
            return Reader;
        }

        public IDataReader GetRadioCheckSummary(DateTime? RadCheckDate, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckSummary]", RadCheckDate, LocationID);
            return Reader;
        }

        public string GetRadioCheckStepByRadioCheckDate(DateTime RadioCheckDate)
        {
            string Step = "";
            Step = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckStepByRadioCheckDate]", RadioCheckDate).ToString();
            return Step;
        }

        public string GetRadioCheckDateByRadioCheckID(int RadioCheckID)
        {
            string Date = "";
            Date = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckDateByRadioCheckID]", RadioCheckID).ToString();
            return Date;
        }

        public string GetRadioCheckStartDateByRadioCheckID(int RadioCheckID)
        {
            string Date = "";
            Date = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckStartDateByRadioCheckID]", RadioCheckID).ToString();
            return Date;
        }

        public string GetRadioCheckEndDateByRadioCheckID(int RadioCheckID)
        {
            string Date = "";
            Date = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckEndDateByRadioCheckID]", RadioCheckID).ToString();
            return Date;
        }

        public string GetLatestRadioCheckIDByRadioCheckDate()
        {
            string RCID = "";
            RCID = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetLatestRadioCheckIDByRadioCheckDate]").ToString();
            return RCID;
        }

        public string GetRadioCheckIDByRadioCheckDate(DateTime? RadioCheckDate)
        {
            string RCID = "";
            RCID = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckIDByRadioCheckDate]", RadioCheckDate).ToString();
            return RCID;
        }

        public string RadioCheckStartIDByRadioCheckDate(DateTime? RadioCheckDate)
        {
            string RCID = "";
            RCID = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckStartIDByRadioCheckDate]", RadioCheckDate).ToString();
            return RCID;
        }

        public string RadioCheckEndIDByRadioCheckDate(DateTime? RadioCheckDate)
        {
            string RCID = "";
            RCID = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetRadioCheckEndIDByRadioCheckDate]", RadioCheckDate).ToString();
            return RCID;
        }

        public IDataReader GetMasterStaffList(string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetMasterStaffList]", LocationID);
            return Reader;
        }

        public IDataReader GetStaffAccountedFor(int RadioCheckID, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffAccountedFor]", RadioCheckID, LocationID);
            return Reader;
        }

        public IDataReader GetStaffUnAccountedFor(int RadioCheckID, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffUnAccountedFor]", RadioCheckID, LocationID);
            return Reader;
        }

        public IDataReader GetStaffMovements(int RadioCheckID, string LocationID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[GetStaffMovements]", RadioCheckID, LocationID);
            return Reader;
        }

        public IDataReader RadioCheckCustomQueryReport(DateTime? StartDate, DateTime? EndDate, int? TargetCount)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "[RC].[RadioCheckCustomQueryReport]", StartDate, EndDate, TargetCount);
            return Reader;
        }
        #endregion

        #region Notification of Absence

        public IDataReader GetUserInfoByUserID(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "rcSec.GetUserInfoByUserID", UserID);
            return Reader;
        }

        public IDataReader GetStaffMembersByDepartmentID(string DepartmentID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "rcSec.[GetStaffMembersByDepartmentID]", DepartmentID, UnitID, SubUnitID);
            return Reader;
        }

        public IDataReader GetStaffMovementRequestsByDepartmentID(string DepartmentID, string UnitID, string SubUnitID)
        {
             IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetStaffMovementRequestsByDepartmentID", DepartmentID, UnitID, SubUnitID);
             return Reader;
        }

        public IDataReader SearchStaffMovementRequestsByDepartmentID(string MovementRequestNumber,string DepartmentID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.SearchStaffMovementRequestsByDepartmentID", MovementRequestNumber, DepartmentID, UnitID, SubUnitID);
            return Reader;
        }

        public IDataReader GetCancellationReasons()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetCancellationReasons");
            return Reader;
        }

        public IDataReader GetMRReturnReasons()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "rclkp.GetMRRejectionReason");
            return Reader;
        }

        public IDataReader GetReturnReasons()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetReturnReasons");
            return Reader;
        }

        public IDataReader GetEmailContent(int EmailTypeID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Noti.GetEmailContent", EmailTypeID);
            return Reader;
        }

        public IDataReader GetEmailReceipients(string TANo)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Noti.GetEmailReceipients", TANo);
            return Reader;
        }


        public void SendTAPendingNotificationEmail(string TANo,int EmailTypeID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Noti.SendTAPendingNotificationEmail", TANo, EmailTypeID);
        }

        public void SendTANotificationToOwner(string TANo, int EmailTypeID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Noti.SendTANotificationToOwner", TANo, EmailTypeID);
        }

        

        public IDataReader GetMREmailReceipients(string MRNo)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Noti.GetMREmailReceipients", MRNo);
            return Reader;
        }




        public IDataReader GetEmailReceiver(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetEmailReceiver", UserID);
            return Reader;
        }

        public IDataReader GetStaffMovementRequest(string UserID, string MovementRequestNumber, string Status)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetStaffMovementRequest", UserID, MovementRequestNumber,Status);
            return Reader;
        }

        public IDataReader SearchStaffMovementRequest(string UserID, string MovementRequestNumber, string Status)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.SearchStaffMovementRequest", UserID, MovementRequestNumber, Status);
            return Reader;
        }

        public IDataReader GetNotificationsOfAbsence(string MRNO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetNotificationsOfAbsence", MRNO);
            return Reader;
        }




        // Added by walter TR
        public IDataReader GetStaffMovementRequestTR(string UserID, string MovementRequestNumber, string Status, string location)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetStaffMovementRequest", UserID, MovementRequestNumber,
                 Status, location);
            return Reader;
        }


        public IDataReader GetLeaveItineraryByLeaveRequestID(string LeaveRequestID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetLeaveItineraryByLeaveRequestID", LeaveRequestID);
            return Reader;
        }



        // added for the transfer of the TA to member

        public IDataReader GetStaffMovementRequestDetails(string UserID, string MovementRequestNumber, string Status, string location, string CreatedBy, string UpdatedBy, string StatusCode)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetStaffMovementRequestDetails", UserID, MovementRequestNumber,
                 Status, location, CreatedBy, UpdatedBy, StatusCode);
            return Reader;
        }

        public IDataReader GetMovementRequestByMovementRequestID(string MovementRequestID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetMovementRequestByMovementRequestID", MovementRequestID);
            return Reader;
        }

        public IDataReader GetMRByMRNo(string MRNo)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetMRByMRNo", MRNo);
            return Reader;
        }

        public IDataReader GetSupervisors(string MovementRequestID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetSupervisors", MovementRequestID);
            return Reader;
        }

        public IDataReader SearchMovementRequest(string MovementRequestNumber, string WBS, string Status, string location)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.SearchMovementRequest", MovementRequestNumber,
                 WBS, Status, location);
            return Reader;
        }

        public IDataReader SearchMovementRequestForManagers(string MovementRequestNumber, string Status, string LocationID, string DepartmentID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.SearchMovementRequestForManagers",
                MovementRequestNumber, Status, LocationID, DepartmentID, UnitID, SubUnitID);
            return Reader;
        }

        public IDataReader GetLeaveItineryByMovementRequestNumber(string MovementRequestNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetLeaveItineryByMovementRequestNumber", MovementRequestNumber);

            return Reader;
        }

        public IDataReader GetLeaveItineryPrivateDeviation(string MovementRequestNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.[GetPrivateDeviation]", MovementRequestNumber);

            return Reader;
        }

        public IDataReader GetRejectionReason(string MovementRequestID, string RejectionReasonType)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.[GetRejectionReason]", MovementRequestID, RejectionReasonType);

            return Reader;
        }

        public IDataReader GetMRReturnReason(string MovementRequestID, string RejectionReasonType)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.[GetMRReturnReason]", MovementRequestID, RejectionReasonType);
            return Reader;
        }

        public IDataReader GetMRRejectionReason(string MovementRequestID, string RejectionReasonType)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.[GetMRRejectionReason]", MovementRequestID, RejectionReasonType);

            return Reader;
        }

        public IDataReader GetMRCancellationReason(string MovementRequestID, string RejectionReasonType)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.[GetMRCancellationReason]", MovementRequestID, RejectionReasonType);

            return Reader;
        }

        public IDataReader GetCheckList(string MovementRequestID, string lookupID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.[GetCheckList]", MovementRequestID, lookupID);
            return Reader;
        }

        public IDataReader GetCheckListByMRID(string MovementRequestID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetCheckListByMRID", MovementRequestID);
            return Reader;
        }

        public IDataReader GetMRStatusHistory(string MovementRequestID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.[GetStatusHistory]", MovementRequestID);

            return Reader;
        }

        public IDataReader GetWBSByMovementRequestID(string MovementRequestID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetWBSByMovementRequestID", MovementRequestID);
            return Reader;
        }

        public void DeleteLeaveItinery(string LeaveItineryID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.DeleteLeaveItinery", LeaveItineryID);
        }

        public void DeleteWBS(string WBSID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.DeleteWBS", WBSID, CreatedBy);
        }

        public void DeleteCheckList(string LeaveItineryID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.DeleteCheckList", LeaveItineryID);
        }

        public string InsertUpdateMovementRequest(string MovementRequestID, string PRISMMissionCode, string UserID, string FirstName, string MiddleName, string LastName, string PurposeOfTravel, string TripSchemaCode, string ModeOfTravelCode, bool SecurityClearance, bool SecurityTraining, string StatusCode, string CreatedBy,
            string CityOfAccommodation, bool IsPrivateStay, string PrivateStayDates, bool IsPrivateDeviation, string PrivateDeviationLegs, bool IsAccommodationProvided, string AccommodationDetails, bool IsTravelAdvanceRequested, string TravelAdvanceCurrency, decimal TravelAdvanceAmount,
            string TravelAdvanceMethod, int IsVisaObtained, string VisaIssued, int IsVaccinationObtained, bool IsSecurityClearanceRequestedByMission, bool TAConfirm, DateTime? PrivateStayDateFrom, DateTime? PrivateStayDateTo, bool IsNotForDSA)
        {
            string MovementRequestIDOut = "";
            MovementRequestIDOut = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertUpdateMovementRequest",
                MovementRequestID,
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
            return MovementRequestIDOut;
        }

        public string InsertUpdateRequesterInformation(string MovementRequestID, string PRISMMissionCode, string UserID, string FirstName, string MiddleName, string LastName, string LeaveCategoryCode, string PurposeOfLeave, DateTime? StartDate, DateTime? EndDate,
             bool? TravellingOut, string CreatedBy, string PRISMNumber
            )
        {
            string MovementRequestIDOut = "";
            MovementRequestIDOut = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertUpdateRequesterInformation",
                MovementRequestID,
                PRISMMissionCode,
                UserID,
                FirstName,
                MiddleName,
                LastName,
                LeaveCategoryCode,
                PurposeOfLeave,
                StartDate,
                EndDate,
                TravellingOut,
                CreatedBy,
                PRISMNumber
                ).ToString();
            return MovementRequestIDOut;
        }

        public string InsertUpdateNotifierInformation(string MovementRequestID, string PRISMMissionCode, string UserID, string FirstName, string MiddleName, string LastName, string LeaveCategoryCode, string PurposeOfLeave,
        string CreatedBy, string PRISMNumber
            )
        {
            string MovementRequestIDOut = "";
            MovementRequestIDOut = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertUpdateNotifierInformation",
                MovementRequestID,
                PRISMMissionCode,
                UserID,
                FirstName,
                MiddleName,
                LastName,
                LeaveCategoryCode,
                PurposeOfLeave,
                CreatedBy,
                PRISMNumber
                ).ToString();
            return MovementRequestIDOut;
        }


        public void UpdateSecurity(string MovementRequestID, bool SecurityClearance, string UserID, bool MRConfirm)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UpdateSecurity", MovementRequestID, SecurityClearance, MRConfirm, UserID);

        }

        public void UpdateMovementRequestDetails(string MovementRequestID, DateTime? StartDate, DateTime? EndDate, string Addresswhileabsent, bool TravellingOut, bool MRConfirm, string UserID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UpdateMovementRequestDetails", MovementRequestID, StartDate, EndDate, Addresswhileabsent, TravellingOut, MRConfirm, UserID);
        }

        public string UserHasDuplicateNotification(string UserID,string MovementRequestID, DateTime? StartDate, DateTime? EndDate)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UserHasDuplicateNotification", UserID, MovementRequestID, StartDate, EndDate).ToString();
            return result;

        }

        public string IsValidDateRange(string MovementRequestID, DateTime? StartDate, DateTime? EndDate)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.IsValidDateRange", MovementRequestID, StartDate, EndDate).ToString();
            if (Convert.ToInt32(result) > 0)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }

        }



        public void UpdateMovementRequestStatus(string MovementRequestID, string StatusCode, string CreatedBy, string Comments)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UpdateMovementRequestStatus", MovementRequestID, StatusCode, CreatedBy, Comments);
        }

        public void UpdateStaffMovementRequestTA(string MovementRequestNumber, string UserID, string CreatedBy, string UpdatedBy, string PRISMNumber)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UpdateStaffMovementRequestMR", MovementRequestNumber, UserID, CreatedBy, UpdatedBy, PRISMNumber);
        }


        public void UpdateMovementRequestIsComplete(string MovementRequestID, bool IsTecComplete, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UpdateMovementRequestIsComplete", MovementRequestID, IsTecComplete, CreatedBy);
        }

        public string InsertUpdateLeaveItinery(string MovementItineraryID, string MovementRequestID, string LeaveCategoryCode, DateTime DateFrom, DateTime DateTo, string CreatedBy, int Ordering)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertUpdateLeaveItinery", MovementItineraryID, MovementRequestID, LeaveCategoryCode, DateFrom, DateTo, CreatedBy, Ordering).ToString();
            return result;
        }

        public string InsertMRStatus(string MovementRequestID, string StatusCode, string RejectionReasons, string RejectionReasonType, string CreatedBy)
        {
            string result;
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertMRStatus",
                MovementRequestID, StatusCode, RejectionReasons, RejectionReasonType, CreatedBy).ToString();
            return result;

        }

        public void InsertRejectionReasons(string MovementRequestID, string RejectionReasonID, string RejectionReasonType, string CreatedBy, string RejectionComment)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertRejectionReason",
                MovementRequestID, RejectionReasonID, RejectionReasonType, CreatedBy, RejectionComment);
        }

        public void InsertCheckList(string MovementRequestID, string LookupID, int Value, string Note, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertCheckList",
                MovementRequestID, LookupID, Value, Note, CreatedBy);
        }

        public string InsertUpdateWBS(string WBSID, string MovementRequestID, string WBSCode, double PercentageOrAmount, string Note, bool IsPercentage, string CreatedBy)
        {
            string result = "";
            result = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertUpdateWBS", WBSID, MovementRequestID, WBSCode, PercentageOrAmount, Note, IsPercentage, CreatedBy).ToString();
            return result;
        }

        public IDataReader GetMovementRequestByMovementRequestNumber(string MovementRequestNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetMovementRequestByMovementRequestNumber", MovementRequestNumber);
            return Reader;
        }

        public void UpdateLeaveItinery(string LeaveItineryID, string ModeOfTravelID, string FromLocationCode, DateTime FromLocationDate, string ToLocationCode, DateTime ToLocationDate, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UpdateLeaveItinery", LeaveItineryID, ModeOfTravelID, FromLocationCode, FromLocationDate, ToLocationCode, ToLocationDate, CreatedBy);
        }

        public void UpdateWBS(string WBSID, string WBSCode, double PercentageOrAmount, string Note, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.UpdateWBS", WBSID, WBSCode, PercentageOrAmount, Note, CreatedBy);
        }

        public string GetMRStepByMovementRequestID(string MovementRequestID)
        {
            string Step = "";
            Step = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetMRStepByMovementRequestID", MovementRequestID).ToString();
            return Step;
        }

        public string GetMRTravelOutStatusByMovementRequestID(string MovementRequestID)
        {
            string Status = "";
            Status = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetMRTravelOutStatusByMovementRequestID", MovementRequestID).ToString();
            return Status;
        }



        public void DuplicateMR(string MovementRequestID, string UserID, string PRISMMissionCode, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.DuplicateMR", MovementRequestID, UserID, PRISMMissionCode, CreatedBy);
        }

        public IDataReader CheckDuplicatedMR(string MovementRequestID, string TravelersName, DateTime DateFrom, DateTime DateTo, string LeaveCategoryCode, string LeaveItineryID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.CheckDuplicatedMR", MovementRequestID, TravelersName, DateFrom, DateTo, LeaveCategoryCode, LeaveItineryID);

            return Reader;
        }

        public IDataReader GetMRStatusByMRID(string MovementRequestID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetMRStatusByMRID", MovementRequestID);
            return Reader;
        }

        // Insert TA Comment

        public void InsertMRComments(string CommentId, string MRNumber, string Comment, string CommentType, string CreatedBy)
        {
            try
            {
                daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertUpdateTAComments",
                    CommentId, MRNumber, CommentType, Comment, CreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public IDataReader GetMRComments(string MovementRequestNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetMRComments", MovementRequestNumber);

            return Reader;
        }

        public IDataReader GetArrivalDateByMovementRequestNumber(string MovementRequestNumber)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.GetArrivalDateByMovementRequestNumber", MovementRequestNumber);
            return Reader;
        }

        public void DeleteMovementRequest(string MovementRequestID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.DeleteMovementRequest", MovementRequestID, CreatedBy);
        }

        public void InsertMRDocumentNumber(string MovementRequestID, string DocumentNumber, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "MR.InsertMRDocumentNumber", MovementRequestID, DocumentNumber, CreatedBy);
        }
        #endregion


    }
}
