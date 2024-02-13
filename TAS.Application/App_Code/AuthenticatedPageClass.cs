using Microsoft.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using System.Web;
using System.Text;
using TravelAuthorizationSystem;
using System.Linq;

public class AuthenticatedPageClass : PageClass
{

    protected Objects.User UserInfo;
    Business.Security sec = new Business.Security();
    private bool _canRead;
    private bool _canEdit;
    private bool _canAdd;
    private bool _canDelete;
    private bool _canAmend;
    private Hashtable _QueryStringVlas;
    private Hashtable _FormVlas;
    private byte[] key = {
		
    };
    private byte[] IV = {
        0x12,
        0x34,
        0x56,
        0x78,
        0x90,
        0xab,
        0xcd,
        0xef
    };

    HttpCookie aCookie;
    protected bool CanRead
    {
        get { return this._canRead; }
    }
    protected bool CanEdit
    {
        get { return this._canEdit; }
    }
    protected bool CanAdd
    {
        get { return this._canAdd; }
    }
    protected bool CanDelete
    {
        get { return this._canDelete; }
    }

    protected bool CanAmend
    {
        get { return this._canAmend; }
    }
    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            UserInfo = (Objects.User)Session["userinfo"];
            if (UserInfo==null)
            {
                string appproot = ConfigurationManager.AppSettings["application.rootOut"];
                Response.Write("<script>");
                Response.Write("window.open('" + appproot + "/Login.aspx','_parent')");
                Response.Write("</script>");
            }
            System.Data.DataTable dt = new System.Data.DataTable();

            sec.getPagePermissions(Request.ServerVariables["Url"], ref dt);

            if (dt.Rows.Count > 0)
            {
                // ===============================Ala Qunaibi=========================

                this._canRead = Convert.ToBoolean(dt.Rows[0]["Read"]);
                this._canEdit = Convert.ToBoolean(dt.Rows[0]["Edit"]);
                this._canAdd = Convert.ToBoolean(dt.Rows[0]["Add"]);
                this._canDelete = Convert.ToBoolean(dt.Rows[0]["Delete"]);
                this._canAmend = Convert.ToBoolean(dt.Rows[0]["Amend"]);
            }
            else
            {
                _canRead = false;
                _canEdit = false;
                _canAdd = false;
                _canDelete = false;
                _canAmend = false;
            }

            string approot = ConfigurationManager.AppSettings["application.root"].ToLower();
            string page_url = Request.ServerVariables["Url"];
            page_url = page_url.ToLower().Replace(approot.ToLower(), "");
        }

        catch (Exception ex)
        {
            string approot = ConfigurationManager.AppSettings["application.rootOut"];
            Response.Write("<script>");
            Response.Write("window.open('/Login.aspx','_parent')");
            Response.Write("</script>");
        }
        // ===============================Ala Qunaibi=========================
        if (!this.CanRead)
        {
            string approot = ConfigurationManager.AppSettings["application.rootOut"];
            Response.Write("<script>");
            Response.Write("window.open('/Login.aspx','_parent')");
            Response.Write("</script>");
        }
    }

    public string Key()
    {
        //return "T@P$3cu7!?Y#";
        return "IMU2021@TA$$!7Y#";
    }

    public string Encrypt(string stringToEncrypt)
    {
        try
        {
            key = System.Text.Encoding.UTF8.GetBytes(Key().Trim().Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public string Decrypt(string stringToDecrypt)
    {
        stringToDecrypt = stringToDecrypt.Replace("%2f", "/");
        stringToDecrypt = stringToDecrypt.Replace("%3d", "=");
        stringToDecrypt = stringToDecrypt.Replace(" ", "+");
        byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
        try
        {
            key = System.Text.Encoding.UTF8.GetBytes(Key().Trim().Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }




    public AuthenticatedPageClass()
    {
        Init += Page_Init;
    }

}

