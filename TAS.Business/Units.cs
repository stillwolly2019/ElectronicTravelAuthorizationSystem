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
    public class Units
    {
        private Data.Units daUnits = new Data.Units();
        HttpContext context = HttpContext.Current;

        public void GetAllUnits(ref DataTable dt)
        {
            dt.Load(daUnits.GetAllUnits());
        }
        public void GetAllUnitsbyDepID(string DepartmentID, ref DataTable dt)
        {
            dt.Load(daUnits.GetAllUnitsbyDepID(DepartmentID));
        }
        public void InsertUpdateUnits(int PID, string UnitName, int DepartmentID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUnits.InsertUpdateUnits(PID, UnitName, DepartmentID, ui.User_Id);
        }
        public int InsertUnits(string UnitID, string DepartmentID, string MissionID, string UnitName)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daUnits.InsertUnits(UnitID, DepartmentID, MissionID, UnitName, ui.User_Id);
        }
        public void DeleteUnit(string UnitID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daUnits.DeleteUnit(UnitID, ui.User_Id);
        }

    }
}
