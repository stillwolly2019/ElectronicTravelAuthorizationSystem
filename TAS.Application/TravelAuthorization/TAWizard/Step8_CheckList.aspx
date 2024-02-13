<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step8_CheckList.aspx.cs" Inherits="TravelAuthorization_TAWizard_Step8_CheckList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/TravelAuthorization/TAWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<style>
        .rdButtons {
            width: 150px !important;
        }
    </style>--%>
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
            function ConfirmDelete() {
                return confirm("Are you sure you want to delete?");
            }
            function SaveValidationG1() {
                var a = 0;
                $(".ReqExp").each(function () {

                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
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
        <asp:UpdatePanel ID="UpdatePanelStep7" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">Step 9 - Check List</h4>
                            </div>
                            <div class="panel-body">
                                <asp:Panel ID="pnlContent" runat="server">
                                    <div id="divTACheckList" runat="server">
                                        <div class="row" style="padding-top: 10px">
                                            <div class="col-lg-12">
                                                <asp:Panel ID="PanelCheckList" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                                    <asp:Label ID="lblCheckListMsg" runat="server" Text=""></asp:Label>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <table>
                                            <tr>
                                                <td class="tdField" colspan="2">
                                                    <asp:HiddenField ID="hdnStatusCode" runat="server" />
                                                    <asp:HiddenField ID="hdnUserID" runat="server" />
                                                    <asp:HiddenField ID="hdnTravelSchema" runat="server" />

                                                    <asp:GridView ID="GVCheckList" CssClass="table table-bordered  table-hover" runat="server" AutoGenerateColumns="False" OnRowCreated="GVCheckList_RowCreated" ShowFooter="false" OnRowDataBound="GVCheckList_RowDataBound" OnRowDeleting="GVCheckList_RowDeleting" DataKeyNames="LookupsID" OnRowCommand="GVCheckList_RowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Check List">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblDescription" Text='<%# Bind("Description") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hfCode" runat="server" Value='<%# Bind("Code") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Options">
                                                                <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rdCheckList" AutoPostBack="true" CssClass="radio-inline" RepeatDirection="Horizontal" runat="server" Width="200px">
                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                        <%--<asp:ListItem Text="No" Value="2"></asp:ListItem>--%>
                                                                        <asp:ListItem Text="N/A" Value="3"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Notes/Comments">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtNote" CssClass="form-control" runat="server" Text='<%# Bind("Note") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdField">
                                                    <span style="color:red">*</span><asp:CheckBox ID="cbConfirmationMessage" runat="server" Text="I confirm the completeness of TEC" CssClass="checkbox-inline" AutoPostBack="true" OnCheckedChanged="cbConfirmationMessage_CheckedChanged" />
                                                </td>
                                                <td class="tdField">
                                                    <div style="float: right">
                                                        <asp:Button ID="btnSaveCheckList" CssClass="btn btn-primary form-control" Enabled="false" runat="server" Text="Submit & continue" OnClick="btnSaveCheckList_Click" Width="280px" Font-Size="14px" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep7" DisplayAfter="100" DynamicLayout="False">
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
