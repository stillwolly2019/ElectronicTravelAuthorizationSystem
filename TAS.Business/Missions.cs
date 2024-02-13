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
    public class Missions
    {

        private Data.Missions daMissions = new Data.Missions();
        HttpContext context = HttpContext.Current;

        public void GetAllMissions(ref DataTable dt)
        {
            dt.Load(daMissions.GetAllMissions());
        }
        public void GetAllLocationsByMissionID(string MissionID, ref DataTable dt)
        {
            dt.Load(daMissions.GetAllLocationsByMissionID(MissionID));
        }

        public void GetAllCountries(ref DataTable dt)
        {
            dt.Load(daMissions.GetAllCountries());
        }
        public void GetAllGenders(ref DataTable dt)
        {
            dt.Load(daMissions.GetAllGenders());
        }

        public void SearchUsersForIMTools(string SearchText, ref DataTable dt)
        {
            dt.Load(daMissions.SearchUsersForIMTools(SearchText));
        }

    }
}
