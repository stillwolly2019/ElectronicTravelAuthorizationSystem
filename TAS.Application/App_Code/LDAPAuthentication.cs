using System;
using System.Text;
using System.Collections;
using System.Web.Security;

using System.Security.Principal;
using System.DirectoryServices;
/// <summary>
/// Summary description for LDAPAuthentication
/// </summary>
namespace FormsAuthAd
{
    public class LDAPAuthentication
    {
        private string _path;
        private string _filterAttribute, _FullNameAttribute;
        private String userName, pswrd,domainName;
        private string DisplayName, UserGivenName, SurName, 
             EmailAddress,
             DomainName,
             title,
             company,
             memberof;
        private int DepartmentID,UnitID,SubUnitID;

        public LDAPAuthentication(string path)
        {
            _path = path;
        }

        public bool IsAuthenticated(string domain, string username, string pwd, string _adPath)
        {
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_adPath, domainAndUsername, pwd);
            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;
                entry.AuthenticationType = AuthenticationTypes.Secure;
                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                userName = username;
                pswrd = pwd;
                domainName = domain;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }
    }
}