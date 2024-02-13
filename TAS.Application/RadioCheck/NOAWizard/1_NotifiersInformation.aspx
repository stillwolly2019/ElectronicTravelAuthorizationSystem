<%@ Page Language="C#" AutoEventWireup="true" CodeFile="1_NotifiersInformation.aspx.cs" Inherits="RadioCheck_NOAWizard_1_NotifiersInformation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/RadioCheck/NOAWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>
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
        <script type="text/javascript">
            function SaveValidation() {
                var a = 0;
                $(".Req").each(function () {

                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-- Please Select --" ||
                        $(this).val() == "00000000-0000-0000-0000-000000000000") {
                        $(this).addClass("invalid");
                        a = a + 1;
                    } else {
                        $(this).removeClass("invalid");
                    }
                });
                if (a > 0) {
                    return false;
                }
                else {
                    return true;
                }
            }
        </script>
        <uc1:WizardHeader runat="server" ID="WizardHeader" />
        <asp:UpdatePanel ID="UpdatePanelStep1" runat="server">
            <ContentTemplate>


                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">Step 1 - Notifier's Information</h4>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlContent" runat="server">
                            <div class="row" style="width: 99.9%;">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <table>
                                        <tr id="userLists" visible="false" runat="server">
                                            <td class="tdLabel">Employee <span style="color:red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:DropDownList ID="DDLUsers" Width="280px" runat="server" OnSelectedIndexChanged="DDLUsers_SelectedIndexChanged" AutoPostBack="true" DataTextField="FullName" DataValueField="UserID" CssClass="form-control Req"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">PRISM Number</td>
                                            <td class="tdField" colspan="6">
                                                <asp:TextBox ID="txtPRISM" placeholder="PRISM Number" runat="server" CssClass="form-control" Width="280px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">Staff Name <span style="color:red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtTravelersFirstName" placeholder="First Name" runat="server" Enabled="false" CssClass="form-control Req" Width="280px"></asp:TextBox>
                                            </td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtTravelerSecondName" placeholder="Middle Name" runat="server" CssClass="form-control" Width="290px"></asp:TextBox>
                                            </td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtTravelerLastName" placeholder="Last Name" runat="server" Enabled="false" CssClass="form-control Req" Width="280px"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="tdLabel">Reason of absence <span style="color:red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:DropDownList ID="ddlLeaveCategory" AutoPostBack="true" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control Req" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="tdLabel">Purpose of Absence <span style="color:red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtPurposeOfLeave" placeholder="Purpose of Leave" runat="server" Enabled="true" CssClass="form-control Req" Width="280px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        

                                        <tr>
                                            <td colspan="5"></td>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return SaveValidation();" CssClass="form-control btn-primary" Font-Size="14px" Text="Submit & Continue" Width="280px" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep1" DisplayAfter="100" DynamicLayout="False">
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



