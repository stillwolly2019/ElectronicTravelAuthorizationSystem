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
    public class Pages
    {
        private Data.Pages daPages = new Data.Pages();
        HttpContext context = HttpContext.Current;
        public void GetAllPages(ref DataTable dt)
        {
            dt.Load(daPages.GetAllPages());
        }
        public void InsertUpdatePages(string PageID, string PageName, string PageURL, string ParentID, int PageOrder, bool IsDisplayedInMenu)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daPages.InsertUpdatePages(PageID, PageName,  PageURL, ParentID, PageOrder, IsDisplayedInMenu, ui.User_Id);
        }
        public void DeletePages(string PageID)
        {
            Objects.User ui = (Objects.User)context.Session["userinfo"];
            daPages.DeletePages(PageID, ui.User_Id);
        }

    }
}