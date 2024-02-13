using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Objects
{
    public class User
    {

        private string _loginName;
        private string _user_id;
        private string _firstname;
        private string _lastname;
        private string _EmpID;
        private string _email;
        private string _missionid;
        private string _dutystation;
        private string _locationid;
        private string _departmentid;
        private string _unitid;
        private string _subunitid;
        private bool _ismanager;
        private bool _isrmo;
        private bool _isradiooperator;
        private bool _isstaffmember;
        private bool _issubstaffmember;
        private bool _iscom;
        private bool _ishoo;
        private bool _ishoso;
        private bool _issupervisor;
        private bool _issubsupervisor;
        private bool _ishrattendancepersonnel;
        private bool _issecreqverifier;
        //private bool _isleaveapprover;
        private bool _issystadmin;
        private bool _isadmin;
        private bool _isfinadmin;
        private bool _isinternationalstaff;
        private bool _isnationalstaff;
        private bool _isregionaldirector;
        //private bool _issupport;
        //private bool _isoperations;
        private bool _issupervisormanager;
        private bool _isemergencytacreator;
        private bool _hasdelegated;
        private bool _hasbeendelegated;
        private eUserType _UserType;
        private string _PRISMNumber;

        public User()
        {
            _loginName = "";
            _user_id = "";
            _firstname = "";
            _lastname = "";
            _EmpID = "";
            _email = "";
            _dutystation = "";
            _missionid = "";
            _locationid = "";
            _departmentid = "";
            _unitid = "";
            _subunitid = "";
            _isstaffmember = false;
            _issubstaffmember = false;
            _iscom = false;
            _ishoo = false;
            _ishoso = false;
            _issupervisor = false;
            _ishrattendancepersonnel = false;
            _issecreqverifier = false;
            _ismanager = false;
            //_isleaveapprover = false;
            _isradiooperator = false;
            _issystadmin = false;
            _isadmin = false;
            _isrmo = false;
            _issupervisor = false;
            _issubsupervisor = false;
            _isinternationalstaff = false;
            _isnationalstaff = false;
            _isregionaldirector = false;
            //_issupport = false;
            //_isoperations = false;
            _issupervisormanager = false;
            _isemergencytacreator = false;
            _hasdelegated = false;
            _hasbeendelegated = false;
            


            _UserType = eUserType.PrivateUser;
            _PRISMNumber = "";
        }
        public enum eUserType
        {
            PrivateUser,
            PublicUser
        }

        public string LoginName
        {
            get
            {
                string functionReturnValue = null;
                if (string.IsNullOrEmpty(_loginName))
                    functionReturnValue = null;
                return _loginName;
                return functionReturnValue;
            }
            set { _loginName = value; }
        }

        /// <value></value>
        public string User_Id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }
        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        public string LastName
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        public string MissionID
        {
            get { return _missionid; }
            set { _missionid = value; }
        }
        public string LocationID
        {
            get { return _locationid; }
            set { _locationid = value; }
        }
        public string DepartmentID
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        public string UnitID
        {
            get { return _unitid; }
            set { _unitid = value; }
        }
        public string SubUnitID
        {
            get { return _subunitid; }
            set { _subunitid = value; }
        }
        public string UserName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }
        public string EmployeeID
        {
            get { return _EmpID; }
            set { _EmpID = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string DutyStation
        {
            get { return _dutystation; }
            set { _dutystation = value; }
        }
        public bool IsManager
        {
            get { return _ismanager; }
            set { _ismanager = value; }
        }
        public bool IsRMO
        {
            get { return _isrmo; }
            set { _isrmo = value; }
        }

        //public bool IsLeaveApprover
        //{
        //    get { return _isleaveapprover; }
        //    set { _isleaveapprover = value; }
        //}
        public bool IsStaffMember
        {
            get { return _isstaffmember; }
            set { _isstaffmember = value; }
        }
        public bool IsSubStaffMember
        {
            get { return _issubstaffmember; }
            set { _issubstaffmember = value; }
        }

        public bool IsCOM
        {
            get { return _iscom; }
            set { _iscom = value; }
        }
        public bool IsHOO
        {
            get { return _ishoo; }
            set { _ishoo = value; }
        }
        public bool IsHOSO
        {
            get { return _ishoso; }
            set { _ishoso = value; }
        }
        public bool IsSupervisor
        {
            get { return _issupervisor; }
            set { _issupervisor = value; }
        }
        public bool IsSubSupervisor
        {
            get { return _issubsupervisor; }
            set { _issubsupervisor = value; }
        }

        public bool IsHRAttendancePersonnel
        {
            get { return _ishrattendancepersonnel; }
            set { _ishrattendancepersonnel = value; }
        }
        public bool IsSecReqVerifier
        {
            get { return _issecreqverifier; }
            set { _issecreqverifier = value; }
        }
        public bool IsSystAdmin
        {
            get { return _issystadmin; }
            set { _issystadmin = value; }
        }
        public bool IsAdmin
        {
            get { return _isadmin; }
            set { _isadmin = value; }
        }
        public bool IsFinAdmin
        {
            get { return _isfinadmin; }
            set { _isfinadmin = value; }
        }
        public bool IsRadioOperator
        {
            get { return _isradiooperator; }
            set { _isradiooperator = value; }
        }
        public bool IsInternationalStaff
        {
            get { return _isinternationalstaff; }
            set { _isinternationalstaff = value; }
        }
        public bool IsNationalStaff
        {
            get { return _isnationalstaff; }
            set { _isnationalstaff = value; }
        }
        public bool IsRegionalDirector
        {
            get { return _isregionaldirector; }
            set { _isregionaldirector = value; }
        }
        //public bool IsSupport
        //{
        //    get { return _issupport; }
        //    set { _issupport = value; }
        //}
        //public bool IsOperations
        //{
        //    get { return _isoperations; }
        //    set { _isoperations = value; }
        //}
        public bool IsSupervisorManager
        {
            get { return _issupervisormanager; }
            set { _issupervisormanager = value; }
        }
        public bool IsEmergencyTACreator
        {
            get { return _isemergencytacreator; }
            set { _isemergencytacreator = value; }
        }
        public bool HasDelegated
        {
            get { return _hasdelegated; }
            set { _hasdelegated = value; }
        }
        public bool HasBeenDelegated
        {
            get { return _hasbeendelegated; }
            set { _hasbeendelegated = value; }
        }
        public eUserType UserType
        {
            get { return _UserType; }
            set { _UserType = value; }
        }
        public string PRISMNumber
        {
            get { return _PRISMNumber; }
            set { _PRISMNumber = value; }
        }
    }
}
