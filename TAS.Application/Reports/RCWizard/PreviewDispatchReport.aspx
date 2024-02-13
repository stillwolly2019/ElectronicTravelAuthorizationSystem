﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreviewDispatchReport.aspx.cs" Inherits="Reports_RCWizard_PreviewDispatchReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Reports/RCWizard/DispatchWizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/css/Site.css" rel="stylesheet" />
    <link href="~/css/bootstrap.css" rel="stylesheet" />
    <!-- MetisMenu CSS -->
    <link href="~/bower_components/metisMenu/dist/metisMenu.min.css" rel="stylesheet" />
    <!-- Custom CSS -->
    <link href="~/dist/css/sb-admin-2.css" rel="stylesheet" />
    <!-- Custom Fonts -->
    <link href="~/bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #fff">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <script src="<%= Page.ResolveClientUrl("~/js/jquery-1.11.3.min.js") %>" type="text/javascript"></script>

        <uc1:WizardHeader runat="server" ID="WizardHeader" />
        <asp:UpdatePanel ID="UpdatePanelStep3" runat="server">
            <ContentTemplate>
                <br />
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title"> Dispatch Report</h4>
                    </div>
                    <div class="panel-body">

                    <div class="row">
                    <div class="col-lg-12">
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    </div>
                    </div>

                        <div class="row">

                            <div class="col-lg-2">
                                <label>Travel Date</label>
                                <asp:TextBox ID="txtTravelDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTravelDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                            </div>

                                 <div class="col-lg-1">
                                <label>&nbsp;</label>
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click"  runat="server" CssClass="form-control btn-primary" Text="Search" Font-Size="14px" Width="100px" OnClientClick="return SaveValidation();" />
                                </div>

                                  <div class="col-lg-2">
                                <label>&nbsp;</label>
                                    <asp:Button ID="btnClear" OnClick="btnClear_Click"  runat="server" CssClass="form-control btn-primary" Text="Clear" Font-Size="14px" Width="100px" OnClientClick="return SaveValidation();" />
                                </div>

                        </div>
                        <br />


                        <div class="row">
                            <div class="col-lg-12">
                                <iframe id="IFramePDF" scrolling="no" style="border: none; width: 100%; min-height: 1050px; background-color: #fff" frameborder="0" runat="server"></iframe>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer" style="padding-top: 0px; padding-bottom: 0px">
                        <div class="row">
                            <div class="col-lg-10">
                                
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep3" DisplayAfter="100" DynamicLayout="False">
            <ProgressTemplate>
                <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                    <div style="padding-top: 200px;">
                        <img alt="Loading" src="../../images/ajax_loader_blue_512.gif" width="200px" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
    <!-- jQuery -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/jquery/dist/jquery.min.js") %>" type="text/javascript"></script>
    <!-- Bootstrap Core JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/bootstrap/dist/js/bootstrap.min.js") %>" type="text/javascript"></script>
    <!-- Metis Menu Plugin JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/metisMenu/dist/metisMenu.min.js") %>" type="text/javascript"></script>
    <!-- Custom Theme JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/dist/js/sb-admin-2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/js/jquery.are-you-sure.js") %>" type="text/javascript"></script>
</body>
</html>






