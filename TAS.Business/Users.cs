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
    public class Users
    {
        private Data.Users daUsers = new Data.Users();
        HttpContext context = HttpContext.Current;

        public void GetAllDelegations(string SearchText, ref DataTable dt)
        {
            dt.Load(daUsers.GetAllDelegations(SearchText));
        }

        public void GetAllPossibleDelegations(string SearchText, ref DataTable dt)
        {
            dt.Load(daUsers.GetAllPossibleDelegations(SearchText));
        }

        public void GetMultipleDelegations(string SearchText, ref DataTable dt)
        {
            dt.Load(daUsers.GetMultipleDelegations(SearchText));
        }

        public void GetUserDelegations(string UserID, ref DataTable dt)
        {
            dt.Load(daUsers.GetUserDelegations(UserID));
        }

        public void GetPossibleDelegatees(string UserID, ref DataTable dt)
        {
            dt.Load(daUsers.GetPossibleDelegatees(UserID));
        }

        public void InsertUpdateDelegations(int ID, string UserID, string DelegatedTo, DateTime? DateFrom, DateTime? DateTo, string Remark,bool AddMode=true)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.InsertUpdateDelegations(ID, UserID, DelegatedTo, DateFrom, DateTo, Remark, ui.User_Id,AddMode);
        }

        public void RevertDeligation(int ID)
        {
            daUsers.RevertDeligation(ID);
        }

        public void InsertPossibleDelegation(string UserID, string DelegatedTo)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.InsertPossibleDelegation(UserID, DelegatedTo, ui.User_Id);
        }

        public void DeleteDelegation(int ID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.DeleteDelegation(ID, ui.User_Id);
        }

        public void DeletePossibleDelegation(string UserID,string DelegatedTo)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.DeletePossibleDelegation(UserID, DelegatedTo);
        }

        public void ManageMultipleDelegations(string UserID, int MaximumDelegations, string OperationType)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.ManageMultipleDelegations(UserID, MaximumDelegations, ui.User_Id, OperationType);
        }

        public bool AlreadyDelegated(int ID,string userid, string deligated, DateTime? DateFrom, DateTime? DateTo,bool AddMode=true)
        {
            int counter;
            counter = Convert.ToInt32(daUsers.AlreadyDelegated(ID,userid, deligated, DateFrom, DateTo,AddMode));
            return counter > 0;
        }

        public bool PersonalMultipleDelegation(int ID, string userid, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode = true)
        {
            int counter;
            counter = Convert.ToInt32(daUsers.PersonalMultipleDelegation(ID, userid, deligated, DateFrom, DateTo, AddMode));
            return counter > 0;
        }

        public bool UserHasActiveDeligation(int ID,string userid, string deligated, DateTime? DateFrom, DateTime? DateTo,bool AddMode=true)
        {
            int counter;
            counter = Convert.ToInt32(daUsers.UserHasActiveDeligation(ID,userid, deligated, DateFrom, DateTo, AddMode));
            return counter > 0;
        }

        public bool AllowMultipleDelegation(string deligated)
        {
            int counter;
            counter = Convert.ToInt32(daUsers.AllowMultipleDelegation(deligated));
            return counter > 0;
        }


        public bool IsMultipleDelegation(int ID, string userid, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode = true)
        {
            int counter;
            counter = Convert.ToInt32(daUsers.IsMultipleDelegation(ID, userid, deligated, DateFrom, DateTo, AddMode));
            return counter > 0;
        }

        public bool MaximumDelegationLimitReached(string userid, string deligated, DateTime? DateFrom, DateTime? DateTo, bool AddMode=true)
        {
            int counter;
            counter = Convert.ToInt32(daUsers.MaximumDelegationLimitReached(userid, deligated, DateFrom, DateTo, AddMode));
            return counter > 0;
        }


        public void GetAllUsers(string SearchText, ref DataTable dt)
        {
            dt.Load(daUsers.GetAllUsers(SearchText));
        }

        public void GetAllWardens(string SearchText, ref DataTable dt)
        {
            dt.Load(daUsers.GetAllWardens(SearchText));
        }

        public void GetAllRadioOperators(string SearchText, ref DataTable dt)
        {
            dt.Load(daUsers.GetAllRadioOperators(SearchText));
        }

        public void GetUserRoles(string UserID, ref DataTable dt)
        {
            dt.Load(daUsers.GetUserRoles(UserID));
        }

        public void GetDepUsers(string RoleId, ref DataTable dt,string DepartmentId)
        {
            dt.Load(daUsers.GetDepUsers(RoleId, DepartmentId));
        }
        // Added by walter to get the staff memebers to be assigned Transfered for TA
        public void GetStaffMembersByTA(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daUsers.GetStaffMembersByTA(ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }

        public void GetActiveUsers(ref DataTable dt,string TANo)
        {
            dt.Load(daUsers.GetActiveUsers(TANo));
        }

        public void GetActiveLocations(ref DataTable dt)
        {
            dt.Load(daUsers.GetActiveLocations());
        }

        public void GetStaffMembersByDepartmentID(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daUsers.GetStaffMembersByDepartmentID(ui.DepartmentID, ui.UnitID, ui.SubUnitID));
        }

        public void GetMissionStaffMembers(ref DataTable dt)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            dt.Load(daUsers.GetMissionStaffMembers());
        }


        public void GetStaffMembersForRadioOperator(ref DataTable dt)
        {
            dt.Load(daUsers.GetStaffMembersForRadioOperator());
        }


        public void GetRoleUsers(string RoleID, ref DataTable dt)
        {
            dt.Load(daUsers.GetRoleUsers(RoleID));
        }
        public void DeleteUserRoles(string UserID)
        {
            daUsers.DeleteUserRoles(UserID);
        }
        public void DeleteRoleUsers(string RoleID)
        {
            daUsers.DeleteUserRoles(RoleID);
        }
        public void InsertUsersRoles(string UserID, string RoleID)
        {
            daUsers.InsertUsersRoles(UserID, RoleID);
        }
        public void DeleteUsers(string UserID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.DeleteUsers(UserID, ui.User_Id);
        }
        public void ActivateDeactivateUsers(string UserID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.ActivateDeactivateUsers(UserID, ui.User_Id);
        }
        public string InsertUpdateUsers(string UserID, string Username, string FirstName, string LastName, string Email,bool IsManager, string PRISMNumber)
        {
            string InsertedUserID;
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            InsertedUserID = daUsers.InsertUpdateUsers(UserID, Username, FirstName, LastName, Email,IsManager, ui.User_Id, PRISMNumber);
            return InsertedUserID;
        }

        public string GetDisplayName(string UserID)
        {
            string UserName;
            UserName = daUsers.GetDisplayName(UserID);
            return UserName;
        }

        public void UpdateUsers(string UserID,bool IsManager)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUsers.UpdateUsers(UserID,IsManager, ui.User_Id);
        }

        public void SearchUsers(string SearchText,ref DataTable dt)
        {
            dt.Load(daUsers.SearchUsers(SearchText));
        }

        

        


        public void SearchUsersForDelegation(string SearchText, ref DataTable dt)
        {
            dt.Load(daUsers.SearchUsersForDelegation(SearchText));
        }

        public void SearchUsers(string Username, string FirstName, string LastName, ref DataTable dt)
        {
            dt.Load(daUsers.SearchUsers(Username,FirstName,LastName));
        }

        public void SearchWardens(string Username, string FirstName, string LastName, ref DataTable dt)
        {
            dt.Load(daUsers.SearchWardens(Username, FirstName, LastName));
        }

        public void SearchRadioOperators(string Username, string FirstName, string LastName, ref DataTable dt)
        {
            dt.Load(daUsers.SearchRadioOperators(Username, FirstName, LastName));
        }
    }
}
