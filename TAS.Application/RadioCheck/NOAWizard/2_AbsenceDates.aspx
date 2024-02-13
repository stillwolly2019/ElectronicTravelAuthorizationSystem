
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="2_AbsenceDates.aspx.cs" Inherits="RadioCheck_NOAWizard_2_AbsenceDates" %>
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
        <script type="text/javascript">
            function ConfirmDelete() {
                return confirm("Are you sure you want to delete?");
            }

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
        <asp:UpdatePanel ID="UpdatePanelStep2" runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">Step 2 - Specification of dates of absence</h4>
                    </div>
                    <div class="panel-body">

                        <div class="row" style="width: 99.9%;">

                            <div class="col-lg-12">
                                <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                                <asp:Panel ID="pnlContent1" runat="server">
                                    <table>

                                       

                                         <tr>
                                            <td class="tdLabel">Days of absence:</td>
                                            <td class="tdField col-lg-4">
                                                <asp:TextBox ID="txtStartDate" Enabled="true" runat="server" CssClass="form-control" Width="280px" placeholder="start date"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarStartDate" runat="server" TargetControlID="txtStartDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>

                                            <td class="tdField col-lg-4">
                                                <asp:TextBox ID="txtEndDate" Enabled="true" runat="server" CssClass="form-control"  Width="280px" placeholder="End Date"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarEndDate" runat="server" TargetControlID="txtEndDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="tdLabel">Contact/address while absent:</td>
                                            <td class="tdField col-lg-4" colspan="2">
                                                <asp:TextBox ID="txtAddresswhileAbsent" TextMode="MultiLine" Columns="50" Rows="3" Enabled="true" runat="server" CssClass="form-control" Width="600px" placeholder="Enter Address while absent" ></asp:TextBox>
                                            </td>
                                        </tr>

                                       <%--  <tr>
                                            <td class="tdLabel">Travelling Out of Country?</td>
                                            <td class="tdField col-lg-4">
                                                <asp:CheckBox ID="CheckTravellingOut" Checked="false" Enabled="true" runat="server" Text="" CssClass="checkbox" AutoPostBack="true" />
                                            </td>
                                        </tr>--%>


                                    </table>
                                </asp:Panel>


                                
                                <asp:Panel ID="pnlContent2" runat="server">
                                    <table style="width:100%">
                                        <tr>
                                            <td colspan="5">Please confirm that you have discussed and Agreed with your Supervisor.&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style="color: red">*</span>
                                                <asp:CheckBox ID="checkConfirm" runat="server" Text="Yes" CssClass="checkbox-inline" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">&nbsp;&nbsp;&nbsp;&nbsp;
                                            <td class="tdLabel" style="text-align: right; float: right">
                                                <asp:Button ID="btnSaveLeaveRequestDetails" OnClick="btnSave_Click" runat="server" CssClass="form-control btn-primary" OnClientClick="return SaveValidation();" Font-Size="14px" Text="Submit & Continue" Width="280px" />
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






