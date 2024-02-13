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
    public sealed class Roles
    {
        public IDataReader GetAllRoles()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetAllRoles]");
            return Reader;
        }
        public IDataReader GetStaffCategories()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetStaffCategories]");
            return Reader;
        }

        public IDataReader GetReturnableCancellableStatusCodes(string target)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.[GetReturnableCancellableStatusCodes]",target);
            return Reader;
        }
        public void InsertUpdateRoles(string RoleID,string RoleName, string UniqueField, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.InsertUpdateRoles", RoleID,RoleName,UniqueField, CreatedBy);
        }

        public void DeleteRoles(string RoleID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeleteRoles",RoleID, CreatedBy);
        }


        public int CheckDuplicateRoles(string RoleID,string RoleName)
        {
           return (int)daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.CheckDuplicateRoles", RoleID, RoleName);
        }
        
        public IDataReader GetRolePages(string RoleID,string PageID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetRolePages", RoleID, PageID);
            return Reader;
        }
        public void PermissionsToggle(string RoleID, string PageID, bool Read, bool Edit, bool Add, bool Delete, bool Amend)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.PermissionsToggle",RoleID, PageID, Read, Edit, Add, Delete, Amend);
        }
    }
}
