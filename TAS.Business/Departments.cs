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
    public class Departments
    {
        private Data.Departments daDepartments = new Data.Departments();
        HttpContext context = HttpContext.Current;

        public void GetAllDepartment(ref DataTable dt)
        {
            dt.Load(daDepartments.GetAllDepartment());
        }

        public void GetAllCountries(ref DataTable dt)
        {
            dt.Load(daDepartments.GetAllCountries());
        }

        public void GetAllDepartmentByMissionID(string MissionID, ref DataTable dt)
        {
            dt.Load(daDepartments.GetAllDepartmentByMissionID(MissionID));
        }

        public void DeleteDepartment(string DepartmentID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daDepartments.DeleteDepartment(DepartmentID, ui.User_Id);
        }

        public int InsertDepartments(string DepartmentID, string DepartmentName, string MissionID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daDepartments.InsertDepartments(DepartmentID, DepartmentName, MissionID, ui.User_Id);
        }

    }
}
