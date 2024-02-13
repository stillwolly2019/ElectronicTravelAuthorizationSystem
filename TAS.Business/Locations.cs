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
    public class Locations
    {
        private Data.Locations daLocations = new Data.Locations();
        HttpContext context = HttpContext.Current;
        public void GetAllLocations(ref DataTable dt)
        {
            dt.Load(daLocations.GetAllLocations());
        }

        public void GetAllTravelLocations(ref DataTable dt)
        {
            dt.Load(daLocations.GetAllTravelLocations());
        }

        public void GetAllCountries(ref DataTable dt)
        {
            dt.Load(daLocations.GetAllCountries());
        }

        public void GetAllCitiesByCountryCode(string CountryCode, ref DataTable dt)
        {
            dt.Load(daLocations.GetAllCitiesByCountryCode(CountryCode));
        }

        public void DeleteCity(string CityID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLocations.DeleteCity(CityID);
        }

        public void GetTravelItineraryByLocationID(string LocationID, ref DataTable dt)
        {
            dt.Load(daLocations.GetTravelItineraryByLocationID(LocationID));
        }

        public int InsertUpdateCity(string CityID, string CityCode, string CityName, string CountryCode)
        {
            //Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daLocations.InsertUpdateCity(CityID, CityCode, CityName, CountryCode);
        }



        public void GetAllMissions(ref DataTable dt)
        {
            dt.Load(daLocations.GetAllMissions());
        }

        public void InsertUpdateLocations(string LocationID, string LocationCode,string LocationName,string BusinessArea,string MissionID,bool ConductRadioCheck,bool AllowAccess)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLocations.InsertUpdateLocations(LocationID, LocationCode,  LocationName, BusinessArea, MissionID, ConductRadioCheck, AllowAccess);
        }
        public void DeleteLocations(string LocationID)
        {
            //Objects.User ui = (Objects.User)context.Session["userinfo"];
            daLocations.DeleteLocations(LocationID);
        }

    }
}