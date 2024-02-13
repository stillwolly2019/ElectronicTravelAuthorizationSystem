using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
[Serializable()]
public class User
{

    public string UserID { get; set; }
    public string FullName { get; set; }
    public bool isDeleted { get; set; }
    public bool isDuplicate { get; set; }

    public User(string UserID, string FullName, bool isDeleted, bool isDuplicate)
    {
        this.UserID = UserID;
        this.FullName = FullName;
        this.isDeleted = isDeleted;
        this.isDuplicate = isDuplicate;
    }
}