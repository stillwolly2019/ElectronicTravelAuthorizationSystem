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
    public class SubUnits
    {
        private Data.SubUnits daSubUnits = new Data.SubUnits();
        HttpContext context = HttpContext.Current;

        public void GetAllSubUnits(ref DataTable dt)
        {
            dt.Load(daSubUnits.GetAllSubUnits());
        }
        public void InsertUpdateSubUnits(int ID, int UnitID, int DepartmentID, string SubUnitName)
        {
            daSubUnits.InsertUpdateSubUnits(ID, UnitID, DepartmentID, SubUnitName);
        }
        public int InsertSubUnits(string SubUnitID, string UnitID, string DepartmentID, string MissionID, string SubUnitName)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daSubUnits.InsertSubUnits(SubUnitID, UnitID, DepartmentID, MissionID, SubUnitName, ui.User_Id);
        }

        public void DeleteSubUnit(string SubUnitID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daSubUnits.DeleteSubUnit(SubUnitID, ui.User_Id);
        }
        public void GetAllSubUnitByUnitID(string UnitID, ref DataTable dt)
        {
            dt.Load(daSubUnits.GetAllSubUnitByUnitID(UnitID));
        }

    }
}
