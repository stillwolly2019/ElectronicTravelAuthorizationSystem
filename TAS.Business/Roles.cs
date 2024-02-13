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
    public class Roles
    {
        private Data.Roles daRoles = new Data.Roles();
        HttpContext context = HttpContext.Current;

        public void GetAllRoles(ref DataTable dt)
        {
            dt.Load(daRoles.GetAllRoles());
        }


       

        public void GetStaffCategories(ref DataTable dt)
        {
            dt.Load(daRoles.GetStaffCategories());
        }

        public void GetReturnableCancellableStatusCodes(ref DataTable dt,string target="")
        {
            dt.Load(daRoles.GetReturnableCancellableStatusCodes(target));
        }

        public void InsertUpdateRoles(string RoleID,string RoleName,string UniqueField)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRoles.InsertUpdateRoles(RoleID, RoleName,UniqueField, ui.User_Id);
        }

        public int CheckDuplicateRoles(string RoleID,string RoleName)
        {
            //Objects.User ui = (Objects.User)context.Session["userinfo"];
            return daRoles.CheckDuplicateRoles(RoleID,RoleName);
        }
        public void DeleteRoles(string RoleID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daRoles.DeleteRoles(RoleID, ui.User_Id);
        }
        public void GetRolePages(string RoleID,string PageID, ref DataTable dt)
        {
            dt.Load(daRoles.GetRolePages(RoleID, PageID));
        }
        public void PermissionsToggle(string RoleID, string PageID, bool Read, bool Edit, bool Add, bool Delete, bool Amend)
        {
            daRoles.PermissionsToggle(RoleID, PageID, Read, Edit, Add, Delete, Amend);
        }
    }
}
