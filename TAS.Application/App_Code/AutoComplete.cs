using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Services;
using System.Data;
using TravelAuthorizationSystem;
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService()]
public class AutoComplete : System.Web.Services.WebService
{
    public AutoComplete()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    [WebMethod]
    public string[] GetItemsList(string prefixText, int count)
    {
        Business.Lookups p = new Business.Lookups();
        DataTable DT = new DataTable();
        p.SearchCity(prefixText, ref DT);
        List<string> items = new List<string>(100);
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow Row in DT.Rows)
            {
                string str = null;
                str = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["CityDescription"].ToString(), Row["CityID"].ToString());
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["CityDescription"].ToString(), string.Format("{0},{1}", Row["CityID"].ToString(), Row["CityDescription"].ToString())));
            }
        }

        return items.ToArray();
    }

    [WebMethod]
    public string[] GetItemsListUsers(string prefixText, int count)
    {
        Business.Users u = new Business.Users();
        DataTable DT = new DataTable();
        u.SearchUsers(prefixText, ref DT);
        List<string> items = new List<string>(100);
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow Row in DT.Rows)
            {
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["UserName"].ToString(), Row["UserName"].ToString()));
            }
        }

        return items.ToArray();
    }

    [WebMethod]
    public string[] GetItemsListAccomodations(string prefixText, int count)
    {
        Business.Lookups p = new Business.Lookups();
        DataTable DT = new DataTable();
        p.SearchAccomodations(prefixText, ref DT);
        List<string> items = new List<string>(100);
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow Row in DT.Rows)
            {
                string str = null;
                str = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["AccomodationDescription"].ToString(), Row["AccomodationID"].ToString());
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["AccomodationDescription"].ToString(), string.Format("{0},{1}", Row["AccomodationID"].ToString(), Row["AccomodationDescription"].ToString())));
            }
        }

        return items.ToArray();
    }

    [WebMethod]
    public string[] GetItemsListUsersForDelegation(string prefixText, int count)
    {
        Business.Users u = new Business.Users();
        DataTable DT = new DataTable();
        u.SearchUsersForDelegation(prefixText, ref DT);
        List<string> items = new List<string>(100);
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow Row in DT.Rows)
            {
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["UserName"].ToString(), Row["UserName"].ToString()));
            }
        }

        return items.ToArray();
    }

    [WebMethod]
    public string[] GetUsersForDelegation(string prefixText, int count)
    {
        Business.Users u = new Business.Users();
        DataTable DT = new DataTable();
        u.SearchUsersForDelegation(prefixText, ref DT);
        List<string> items = new List<string>(100);
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow Row in DT.Rows)
            {
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["UserName"].ToString(), Row["UserName"].ToString()));
            }
        }

        return items.ToArray();
    }

    [WebMethod]
    public string[] GetItemsListResidences(string prefixText)
    {
        RadioCheckBusiness.RadioCheck R = new RadioCheckBusiness.RadioCheck();
        DataTable DT = new DataTable();
        R.SearchResidences(prefixText, ref DT);
        List<string> items = new List<string>(100);
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow Row in DT.Rows)
            {
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["ResidenceName"].ToString(), Row["ResidenceID"].ToString()));
            }
        }

        return items.ToArray();
    }


    [WebMethod]
    public string[] GetItemListWBS(string prefixText)
    {
        Business.TravelAuthorization R = new Business.TravelAuthorization();
        DataTable DT = new DataTable();
        R.SearchWBS(prefixText, ref DT);
        List<string> items = new List<string>(100);
        if (DT.Rows.Count > 0)
        {
            foreach (DataRow Row in DT.Rows)
            {
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Row["WBSId"].ToString(), Row["WBSId"].ToString()));
            }
        }

        return items.ToArray();
    }



}