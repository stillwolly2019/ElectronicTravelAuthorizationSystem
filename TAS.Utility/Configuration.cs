using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace TravelAuthorizationSystem.Utility
{
    public class Configuration
    {
        private static string _connectionString;
        public static string ConnectionString
        {
            get { return _connectionString = ConfigurationManager.ConnectionStrings["TravelAuthorizationSystemConnectionString"].ConnectionString; }
        }


        private static string _ActiveDirectoryUsersConnectionString;
        public static string ActiveDirectoryUsersConnectionString
        {
            get { return _ActiveDirectoryUsersConnectionString = ConfigurationManager.ConnectionStrings["ActiveDirectoryUsersConnectionString"].ConnectionString; }
        }


        private static string _IMtoolsConnectionString;
        public static string IMtoolsConnectionString
        {
            get { return _IMtoolsConnectionString = ConfigurationManager.ConnectionStrings["IMtoolsConnectionString"].ConnectionString; }
        }
        private static string _TravelAuthorizationMediaConnectionString;
        public static string TravelAuthorizationMediaConnectionString
        {
            get { return _TravelAuthorizationMediaConnectionString = ConfigurationManager.ConnectionStrings["TravelAuthorizationMediaConnectionString"].ConnectionString; }
        }

        private static string _AMSConnectionString;
        public static string AMSConnectionString
        {
            get { return _AMSConnectionString = ConfigurationManager.ConnectionStrings["AMSConnectionString"].ConnectionString; }
        }

    }
}
