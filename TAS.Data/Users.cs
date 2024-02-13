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
    public sealed class Users
    {
        private object ValueOrDBNullIfZero(int val)
        {
            if (val == 0) return DBNull.Value;
            return val;
        }

        public IDataReader GetAllUsers(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetAllUsers", SearchText);
            return Reader;
        }

        public IDataReader GetAllWardens(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllWardens", SearchText);
            return Reader;
        }

        public IDataReader GetAllRadioOperators(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.GetAllRadioOperators", SearchText);
            return Reader;
        }

        public IDataReader GetUserDelegations(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetUserDelegations", UserID);
            return Reader;
        }

        public IDataReader GetPossibleDelegatees(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetPossibleDelegatees", UserID);
            return Reader;
        }

        public IDataReader GetAllDelegations(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetAllDelegations", SearchText);
            return Reader;
        }

        public IDataReader GetAllPossibleDelegations(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetAllPossibleDelegations", SearchText);
            return Reader;
        }

        public IDataReader GetMultipleDelegations(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetMultipleDelegations", SearchText);
            return Reader;
        }


        

        public void DeleteDelegation(int ID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteDelegation", ID, CreatedBy);
        }

        public void DeletePossibleDelegation(string UserID, string DelegatedTo)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeletePossibleDelegation", UserID, DelegatedTo);
        }

        public void ManageMultipleDelegations(string UserID, int MaximumDelegations, string CreatedBy, string OperationType)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ManageMultipleDelegations", UserID, MaximumDelegations, CreatedBy, OperationType);
        }

        public void InsertUpdateDelegations(int ID, string UserID, string DeligatedTo, DateTime? DateFrom, DateTime? DateTo, string Remark, string CreatedBy,bool AddMode)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.InsertUpdateDelegations", ID, UserID, DeligatedTo, DateFrom, DateTo, Remark, CreatedBy, AddMode);
        }

        public void RevertDeligation(int ID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.RevertDeligation", ID);
        }

        public void InsertPossibleDelegation(string UserID, string DeligatedTo, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.InsertPossibleDelegation", UserID, DeligatedTo, CreatedBy);
        }

        
        public string InsertUpdateUsers(string UserID, string Username, string FirstName, string LastName, string Email, bool IsManager, string CreatedBy, string PRISMNumber)
        {
            try
            {
                string InsertedUserID;
                InsertedUserID = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.InsertUpdateUsers", UserID, Username, FirstName, LastName, Email, IsManager, CreatedBy, PRISMNumber).ToString();
                return InsertedUserID;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public string GetDisplayName(string UserID)
        {
            try
            {
                string DisplayName;
                DisplayName = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetDisplayName", UserID).ToString();
                return DisplayName;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public IDataReader GetUserRoles(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetUserRoles", UserID);
            return Reader;
        }

       
     
        public IDataReader GetRoleUsers(string RoleID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetRoleUsers", RoleID);
            return Reader;
        }

        public IDataReader GetDepUsers(string RoleID,string DepartmentId)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetDepUsers]", RoleID,DepartmentId);
            return Reader;
        }


        public IDataReader GetStaffMembersByTA(string DepartmentID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetStaffMembersByTA]", DepartmentID, UnitID, SubUnitID);
            return Reader;
        }

        public IDataReader GetStaffMembersByDepartmentID(string DepartmentID, string UnitID, string SubUnitID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetStaffMembersByDepartmentID]", DepartmentID, UnitID, SubUnitID);
            return Reader;
        }

        public IDataReader GetActiveUsers(string TANo)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetActiveUsers]", TANo);
            return Reader;
        }

        public IDataReader GetActiveLocations()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetActiveLocations]");
            return Reader;
        }
        

        public IDataReader GetMissionStaffMembers()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetMissionStaffMembers]");
            return Reader;
        }

        

        public IDataReader GetStaffMembersForRadioOperator()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetStaffMembersForRadioOperator]");
            return Reader;
        }

        

        public void DeleteUserRoles(string UserID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteUserRoles", UserID);
        }

        public void DeleteRoleUsers(string RoleID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteRoleUsers", RoleID);
        }

        public void InsertUsersRoles(string UserID, string RoleID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.InsertUsersRoles", UserID, RoleID);
        }

        public void DeleteUsers(string UserID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteUsers", UserID, CreatedBy);
        }

        public void ActivateDeactivateUsers(string UserID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ActivateDeactivateUsers", UserID, CreatedBy);
        }

        public string AlreadyDelegated(int ID,string deligator, string deligated, DateTime? DateFrom, DateTime? DateTo,bool AddMode)
        {
            try
            {
                string counter;
                counter = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.AlreadyDelegated", ID,deligator,deligated,DateFrom,DateTo,AddMode).ToString();
                return counter;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string PersonalMultipleDelegation(int ID, string deligator, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode)
        {
            try
            {
                string counter;
                counter = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.PersonalMultipleDelegation", ID, deligator, deligated, DateFrom, DateTo, AddMode).ToString();
                return counter;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string UserHasActiveDeligation(int ID, string deligator, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode)
        {
            try
            {
                string counter;
                counter = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.UserHasActiveDeligation", ID, deligator, deligated, DateFrom, DateTo, AddMode).ToString();
                return counter;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public string IsMultipleDelegation(int ID, string deligator, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode)
        {
            try
            {
                string counter;
                counter = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.IsMultipleDelegation", ID, deligator, deligated, DateFrom, DateTo, AddMode).ToString();
                return counter;
            }
            catch (Exception ex)
            {
                return "";
            }
        }



        public string AllowMultipleDelegation(string deligated)
        {
            try
            {
                string counter;
                counter = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.AllowMultipleDelegation", deligated).ToString();
                return counter;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string MaximumDelegationLimitReached(string deligator, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode)
        {
            try
            {
                string counter;
                counter = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "SEC.MaximumDelegationLimitReached", deligator, deligated, DateFrom, DateTo, AddMode).ToString();
                return counter;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //public string CheckMultipleDelegation(int ID, string deligator, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode)
        //{
        //    try
        //    {
        //        string counter;
        //        counter = daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.CheckMultipleDelegation", ID, deligator, deligated, DateFrom, DateTo, AddMode).ToString();
        //        return counter;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}


        public void UpdateUsers(string UserID,bool IsManager, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.UpdateUsers", UserID,IsManager, CreatedBy);
        }

        public IDataReader SearchUsers(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ActiveDirectoryUsersConnectionString, "Sec.SearchUsersForTRavelAuthorizationSystem", SearchText);
            return Reader;
        }

       
        

        public IDataReader SearchUsersForDelegation(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.SearchUsersForDelegation", SearchText);
            return Reader;
        }

        public IDataReader SearchUsers(string Username,string FistName , string LastName)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.SearchUsers", Username, FistName, LastName);
            return Reader;
        }

        public IDataReader SearchWardens(string Username, string FistName, string LastName)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.SearchWardens", Username, FistName, LastName);
            return Reader;
        }

        public IDataReader SearchRadioOperators(string Username, string FistName, string LastName)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "RC.SearchRadioOperators", Username, FistName, LastName);
            return Reader;
        }

        
    }
}
