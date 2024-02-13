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
    public sealed class Missions
    {
        public IDataReader GetAllMissions()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllMissions");
            return Reader;
        }
        public IDataReader GetAllLocationsByMissionID(string MissionID)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.GetAllLocationsByMissionID", MissionID);
            return Reader;
        }

        public IDataReader GetAllCountries()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllCountries");
            return Reader;
        }
        public IDataReader GetAllGenders()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllGender");
            return Reader;
        }

        public IDataReader SearchUsersForIMTools(string SearchText)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Lkp.SearchUsersForIMTools", SearchText);
            return Reader;
        }


    }
}
