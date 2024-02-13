using Microsoft.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;

public abstract class PageClass : System.Web.UI.Page
{
    private string AppName = ConfigurationManager.AppSettings["ApplicationName"];
    private void Page_PreRenderComplete(object sender, System.EventArgs e)
    {
        this.Title = AppName + this.Title;
    }
    public PageClass()
    {
        PreRenderComplete += Page_PreRenderComplete;
    }
}


