using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WBS
/// </summary>
[Serializable()]
public class WBS
{
    public string WBSID { get; set; }
    public string WBSCode { get; set; }
    public double PercentageOrAmount { get; set; }
    public string Note { get; set; }
    public bool IsPercentage { get; set; }
    public bool isDeleted { get; set; }

    public WBS(string WBSID,string WBSCode, double PercentageOrAmount, string Note, bool IsPercentage,bool isDeleted=false)
	{
		//
		// TODO: Add constructor logic here
		//
        this.WBSID = WBSID;
        this.WBSCode = WBSCode;
        this.PercentageOrAmount = PercentageOrAmount;
        this.Note = Note;
        this.IsPercentage = IsPercentage;
        this.isDeleted = isDeleted;
	}
}