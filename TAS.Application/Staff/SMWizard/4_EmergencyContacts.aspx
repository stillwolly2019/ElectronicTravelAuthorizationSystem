<%@ Page Language="C#" AutoEventWireup="true" CodeFile="4_EmergencyContacts.aspx.cs" Inherits="Staff_SMWizard_4_EmergencyContacts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Staff/SMWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .uppercase {
            text-transform: uppercase;
        }

        .TheDisp {
            display: block;
        }

        .autocomplete_completionListElement_New {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            font-size: 14px;
            text-align: left;
            list-style-type: none;
            margin-left: 0px;
            padding-left: 0px;
            max-height: 150px;
            width: auto;
        }

            .autocomplete_completionListElement_New: li {
                margin: 0px 0px 0px -20px !important;
            }

        .autocomplete_highlightedListItem_New {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }

        .autocomplete_listItem_New {
            background-color: window;
            color: windowtext;
            padding: 1px;
        }

        .FormatRadioButtonList label {
            margin-right: 15px;
        }
    </style>
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
                $(".ReqG1").each(function () {

                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-- Mode Of Travel --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
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
        <asp:UpdatePanel ID="UpdatePanelStep4" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">Step 3 - Staff Emergency Contacts</h4>
                            </div>
                            <div class="panel-body">
                                <asp:Panel ID="pnlContent" runat="server">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                                <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td class="tdField">
                                                <asp:DropDownList ID="ddlRelationshipType" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control ReqG1" Width="220px" />
                                            </td>
                                            <td class="tdField">
                                                <asp:TextBox ID="txtNameOfContactPerson" runat="server" CssClass="form-control ReqG1" placeholder="Name of contact person" Width="220px"></asp:TextBox>
                                            </td>
                                            <td class="tdField">
                                                <asp:TextBox ID="txtContactDetails" runat="server" CssClass="form-control ReqG1" placeholder="E.g.0925304070, jjlokiri@iom.int" Width="220px"></asp:TextBox>
                                            </td>
                                            <td class="tdLabel">
                                                <asp:LinkButton ID="ibAdd" ToolTip="Click to save" OnClick="ibAdd_Click" OnClientClick="return SaveValidationG1();" runat="server"><li class="fa fa-plus"></li> Click to save</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">&nbsp;</td>
                                        </tr>
                                        <tr>


                                            <td colspan="6">
                                                <asp:GridView ID="gvStaffEmergencyContacts" AllowPaging="false" ShowHeaderWhenEmpty="false" CssClass="table table-bordered  table-hover" AutoGenerateColumns="False"
                                                    DataKeyNames="ContactID,RelationshipTypeCode,Description,NameOfContactPerson,ordering" OnRowCommand="gvStaffEmergencyContacts_RowCommand" OnRowDataBound="gvStaffEmergencyContacts_RowDataBound" OnRowDeleting="gvStaffEmergencyContacts_RowDeleting" runat="server">
                                                    <Columns>
                                                        <asp:BoundField DataField="Description" HeaderText="Relationship Type" />
                                                        <asp:BoundField DataField="NameOfContactPerson" HeaderText="Name Of Contact"/>
                                                        <asp:BoundField DataField="ContactDetails" HeaderText="Contact Details"/>
                                                        <asp:TemplateField ShowHeader="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibEdit" ToolTip="Click to edit" CommandName="EditEmergencyContact" CommandArgument='<%# Container.DisplayIndex %>' runat="server"><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibDelete" runat="server" Width="120px" CommandName="DeleteItem" ToolTip="Click to delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>



                                        </tr>
                                        <tr>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel" colspan="2">
                                                <div style="float: right">
                                                    <asp:Button ID="btnNext" runat="server" CssClass="form-control btn-primary" OnClick="btnNext_Click" Font-Size="14px" Text="Submit & Continue" Width="280px" />
                                                </div>
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
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep4" DisplayAfter="100" DynamicLayout="False">
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



