<%@ Page Language="C#" AutoEventWireup="true" CodeFile="3_Location.aspx.cs" Inherits="Staff_SMWizard_3_Location" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Staff/SMWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>
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
    <style type="text/css">
        .auto-style1 {
            vertical-align: middle;
            background-color: transparent;
            text-align: justify;
            padding: 6px;
            font-size: 12px;
        }
    </style>
</head>
<body style="background-color: #fff">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <script src="<%= Page.ResolveClientUrl("~/js/jquery-1.11.3.min.js") %>" type="text/javascript"></script>

        <uc1:WizardHeader runat="server" ID="WizardHeader" />
        <asp:UpdatePanel ID="UpdatePanelStep2" runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">Step 4 - Staff Location & Residence</h4>
                    </div>
                    <div class="panel-body">

                        <div class="row" style="width: 99.9%;">

                            <div class="col-lg-12">
                                <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                                <asp:Panel ID="pnlContent1" runat="server">
                                    <table style="width:60%">

                                          <%--<tr>
                                            <td class="tdLabel">Zone of Residence<span style="color: red">*</span></td>
                                            <td class="tdField" colspan="6">
                                               <asp:HiddenField ID="hdnLocatioID" runat="server" />
                                                <asp:DropDownList ID="ddlZone" AutoPostBack="true" runat="server" DataTextField="ZoneName" DataValueField="ZoneID" OnTextChanged="ddlZone_SelectedIndexChanged"  CssClass="form-control Req" />
                                            </td>
                                       </tr>--%>

                                          <tr>
                                            <td class="tdLabel">Residential Area<span style="color: red">*</span></td>
                                            <td class="tdField" colspan="6">
                                                
                                               <asp:HiddenField ID="hdnLocatioID" runat="server" />
                                                <asp:DropDownList ID="ddlResidenceArea" AutoPostBack="true" runat="server" DataTextField="ResidenceName" DataValueField="ResidenceID" OnTextChanged="ddlResidenceArea_SelectedIndexChanged" CssClass="form-control Req" />
                                            </td>
                                       </tr>

                                          <tr>
                                            <td class="tdLabel">Residence Type<span style="color: red">*</span></td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlResidenceType" AutoPostBack="true" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control Req" />
                                            </td>
                                       </tr>

                                     <%--  <tr>
                                            <td class="tdLabel">Current Staff  Location <span style="color: red">*</span></td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlCurrentLocation" AutoPostBack="true" runat="server" DataTextField="Description" DataValueField="Description" CssClass="form-control Req" />
                                            </td>
                                       </tr>

                                        <tr>
                                            <td class="tdLabel">Location Status</td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlLocationStatus" AutoPostBack="true" runat="server" DataTextField="Description" DataValueField="Description" CssClass="form-control Req" />
                                            </td>
                                        </tr>--%>
                                        

                                    </table>
                                </asp:Panel>


                                
                                <asp:Panel ID="pnlContent2" runat="server">
                                    <table style="width:100%">
                                        <tr>
                                            <td colspan="8">&nbsp;&nbsp;&nbsp;&nbsp;
                                            <td class="tdLabel" style="text-align: right; float: right">
                                                <asp:Button ID="BtnSaveLocations" OnClick="BtnSave_Click" runat="server" CssClass="form-control btn-primary" OnClientClick="return SaveValidation();" Font-Size="14px" Text="Submit & Continue" Width="280px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </div>


                    </div>
                </div>
            </ContentTemplate>
           
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep2" DisplayAfter="100" DynamicLayout="False">
            <ProgressTemplate>
                <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                    <div style="padding-top: 200px;">
                        <img alt="Loading" src="../../images/ajax_loader_blue_512.gif" width="200px" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
    <script src="<%= Page.ResolveClientUrl("~/RadioCheck/NOAWizard/NOAUpload.js") %>" type="text/javascript"></script>
    <!-- jQuery -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/jquery/dist/jquery.min.js") %>" type="text/javascript"></script>
    <!-- Bootstrap Core JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/bootstrap/dist/js/bootstrap.min.js") %>" type="text/javascript"></script>
    <!-- Metis Menu Plugin JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/metisMenu/dist/metisMenu.min.js") %>" type="text/javascript"></script>
    <!-- Custom Theme JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/dist/js/sb-admin-2.js") %>" type="text/javascript"></script>

</body>
</html>













