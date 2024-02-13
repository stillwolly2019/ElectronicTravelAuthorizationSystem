<%@ Page Title="My Delegations" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Delegate.aspx.cs" Inherits="Admin_Delegate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            cursor: default;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to cancel?");
        }
        function openModal() {
            $('#DivAddNewDelegation').modal('show');
        }
        function SaveValidation() {
            var a = 0;
            $(".Req").each(function () {
                if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0" || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
                    $(this).addClass("invalid");
                    a = a + 1;
                }
            });
            if (a > 0) {
                return false;
            }
            else {
                return true;
            }
        }
        $(document).ready(function () {
            $(".Req").blur(function () {
                var a = 0;
                $(".Req").each(function () {
                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0" || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
                        $(this).addClass("invalid");
                        a = a + 1;
                    }
                    else {
                        $(this).removeClass("invalid");
                    }
                });
                if (a > 0) {
                    return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <h4 class="page-header">My Delegation history</h4>
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
                <!-- /.col-lg-12 -->
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>User name, First Name or Last Name</label>
                                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>         
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="btnAdvSearch" runat="server" Text="Search" CssClass="form-control btn-primary" OnClick="btnAdvSearch_Click" />
                                    </div>
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="form-control btn-primary" OnClick="btnClear_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <asp:Label ID="lblGVUsersDelegationCount" runat="server" Text="0" CssClass="hidden UsersCount"></asp:Label>
                                <asp:GridView ID="GVUsersDelegation" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="GVUsersDelegation_RowCommand"
                                    DataKeyNames="ID,UserId,IsActive,DeligatedTo,Delegator,Delegatee,DateFrom,DateTo,Remark"
                                    OnRowDeleting="GVUsersDelegation_RowDeleting" OnRowDataBound="GVUsersDelegation_RowDataBound" EmptyDataText="No records to display">
                                    <Columns>
                                        <%--<asp:BoundField DataField="Delegator" HeaderText="Role Delegator" />--%>
                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                        <asp:BoundField DataField="Delegatee" HeaderText="Delegated To" />
                                        <asp:TemplateField HeaderText="Date From" HeaderStyle-CssClass="text-center"  ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateFrom" runat="server"
                                                    Text='<%# Eval("DateFrom", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       
                                          <asp:TemplateField HeaderText="Date To" HeaderStyle-CssClass="text-center"  ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateTo" runat="server"
                                                    Text='<%# Eval("DateTo", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                       <asp:TemplateField HeaderText="Date Created" HeaderStyle-CssClass="text-center"  ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateCreated" runat="server"
                                                    Text='<%# Eval("DateCreated", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                    </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center"  ItemStyle-Width="200px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="RevertDelegation" ToolTip="Click to Revert" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Click to Revert</asp:LinkButton>
                                            </div>
                                                </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9">
                        </div>
                        <div class="col-lg-3">
                            <asp:LinkButton ID="LnkAddNewDelegation" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewDelegation_Click"><li class="fa fa-save"></li>&nbsp;Add new delegation</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="DivAddNewDelegation" tabindex="-1" role="form" aria-labelledby="myNewDelegation" aria-hidden="true">
        <div class="modal-dialog" style="width: 750px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="AddNewDelegationLabel" runat="server">Role delegation</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageAddNewDelegation" runat="server" CssClass="alert alert-success alert-dismissable" Visible="true">
                                        <asp:Label ID="lblmsgAddNewDelegation" runat="server" Text="ddd"></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>

                             <div class="row">
                                <div class="col-lg-12">
                                    <table>
                                        <tr>
                                            <td style="vertical-align: top; padding: 20px; width: 90%">
                                                <table>

                                                     <tr>
                                                        <td class="tdLabel">Delegate to: </td>
                                                        <td class="tdField">
                                                            <asp:DropDownList ID="DDLUsers" CssClass="form-control Req" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Date From</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtDateFrom" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                        </td>
                                                        <td class="tdLabel">Date To</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtDateTo" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Remark</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:TextBox ID="txtRemark" Height="70px" TextMode="MultiLine" runat="server" CssClass="form-control Req"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                           <%-- <div class="row">
                                <div class="col-lg-12">
                                    <table>


                                        <tr>
                                            <td style="vertical-align: top; padding: 20px; width: 90%">
                                                <table>




                                                     <tr>
                                                        <td class="tdLabel">Delegate to: </td>
                                                        <td class="tdField">
                                                            <asp:DropDownList ID="DDLUsers" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td class="tdLabel">Date From</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtDateFrom" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                        </td>
                                                        <td class="tdLabel">Date To</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtDateTo" CssClass="form-control Req" runat="server"></asp:TextBox></td>
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Remark</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:TextBox ID="txtRemark" Height="70px" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </td>
                                        </tr>



                                    </table>
                                </div>
                            </div>--%>
                        </div>
                        <div class="modal-footer">
                            <div class="row">
                                <div class="col-lg-6">
                                    <asp:LinkButton ID="LnkSave" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkSave_Click" OnClientClick="return SaveValidation();"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>
                                </div>
                                <div class="col-lg-6">
                                    <asp:LinkButton ID="LnkClose" CssClass="form-control btn btn-outline btn-primary" runat="server" data-dismiss="modal" Font-Size="12px"><li class="fa fa-sign-out"></li>&nbsp;Close</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <script>
        $(document).ready(function () {
            if ($(".UsersCount").text() != "0") {
                $('#ContentPlaceHolder1_GVUsersDelegation').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: false
                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".UsersCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVUsersDelegation').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false
                        });
                    }
                }
            });
        };
    </script>
</asp:Content>

