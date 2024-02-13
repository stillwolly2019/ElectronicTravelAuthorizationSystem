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
    public sealed class Locations
    {
        public IDataReader GetAllTravelLocations()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllTravelLocations");
            return Reader;
        }

        public IDataReader GetAllLocations()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllLocations");
            return Reader;
        }

        public IDataReader GetAllCountries()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllCountries");
            return Reader;
        }

        public IDataReader GetAllCitiesByCountryCode(string CountryCode)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllCitiesByCountryCode", CountryCode);
            return Reader;
        }

        public void DeleteCity(string CityID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.DeleteCity", CityID);
        }

        public int InsertUpdateCity(string CityID, string CityCode, string CityName, string CountryCode)
        {
            return int.Parse(daHelper.ExecuteScalar(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.InsertUpdateCity", CityID, CityCode, CityName, CountryCode).ToString());
        }

        public IDataReader GetTravelItineraryByLocationID(string Id)
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetTravelItineraryByLocationID", Id);
            return Reader;
        }

        public IDataReader GetAllMissions()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.GetAllMissions");
            return Reader;
        }
        public void InsertUpdateLocations(string LocationID, string LocationCode, string LocationName, string BusinessArea, string MissionID, bool ConductRadioCheck, bool AllowAccess)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.InsertUpdateLocations",
                LocationID, LocationCode, LocationName, BusinessArea, MissionID, ConductRadioCheck, AllowAccess);
        }
        public void DeleteLocations(string LocationID)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "lkp.DeleteLocations", LocationID);
        }
    }
}
