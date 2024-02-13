using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using TravelAuthorizationSystem.Utility;
namespace Data
{
    public sealed class Security
    {
        public IDataReader ADLogin(string username)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ADLogin", username);
            return Reader;
        }

        public IDataReader IsValidUser(string UserID,string Pwd)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.IsValidUser", UserID, Pwd);
            return Reader;
        }

        public IDataReader getPagePermissions(string UserName, string page_url)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.PermissionsCheck", UserName, page_url);
            return Reader;
        }
        public IDataReader GetUserMenu(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetUserMenu", UserID);
            return Reader;
        }
        public IDataReader GetUserInfoByUserID(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetUserInfoByUserID", UserID);
            return Reader;
        }
        public IDataReader GetTAInformationByTANO (string TANO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetTAInformationByTANO", TANO);
            return Reader;
        }

        public IDataReader GetMRInformationByMRNO(string MRNO)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetMRInformationByMRNO", MRNO);
            return Reader;
        }
        public IDataReader GetRoleNameByUserID(string UserID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetRoleNameByUserID", UserID);
            return Reader;
        }
        public void ADSingleSignOn(string UserName)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ADSingleSignOn", UserName);
        }
        public void ADLogOut(string UserName)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.ADLogOut", UserName);
        }

        public IDataReader GetAllLocations()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "sec.GetAllLocations");
            return Reader;
        }
    }
}
