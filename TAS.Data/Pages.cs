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
    public sealed class Pages
    {
        public IDataReader GetAllPages()
        {
            IDataReader Reader = daHelper.ExecuteReader(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.GetAllPages");
            return Reader;
        }
        public void InsertUpdatePages(string PageID, string PageName, string PageURL, string ParentID, int PageOrder, bool IsDisplayedInMenu, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.InsertUpdatePages",
                PageID, PageName, PageURL, ParentID, PageOrder, IsDisplayedInMenu, CreatedBy);
        }
        public void DeletePages(string PageID, string CreatedBy)
        {
            daHelper.ExecuteNonQuery(TravelAuthorizationSystem.Utility.Configuration.ConnectionString, "Sec.DeletePages", PageID, CreatedBy);
        }
    }
}
