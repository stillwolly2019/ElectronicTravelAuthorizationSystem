<%@ Page Title="My delegations" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Delegate - Copy.aspx.cs" Inherits="Admin_Delegate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
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
                    <h4 class="page-header">My delegations</h4>
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
                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <asp:Label ID="lblGVDelegationsCount" runat="server" Text="0" CssClass="hidden UsersCount"></asp:Label>
                                <asp:GridView ID="GVDelegations" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="GVDelegations_RowCommand"
                                    DataKeyNames="ID,UserId,DeligatedTo,Delegator,Delegatee,DateFrom,DateTo,Remark"
                                    OnRowDeleting="GVDelegations_RowDeleting" OnRowDataBound="GVDelegations_RowDataBound" EmptyDataText="No records to display">
                                    <Columns>
                                        <asp:BoundField DataField="Delegator" HeaderText="Role Delegator" />
                                        <asp:BoundField DataField="Delegatee" HeaderText="Delegated To" />
                                        <asp:TemplateField HeaderText="Date From" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateFrom" runat="server"
                                                    Text='<%# Eval("DateFrom", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       
                                          <asp:TemplateField HeaderText="Date To" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateTo" runat="server"
                                                    Text='<%# Eval("DateTo", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                       <asp:TemplateField HeaderText="Date Created" HeaderStyle-CssClass="text-center">
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
                                                <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="EditDelegation" ToolTip="Click to edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
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
                            <asp:LinkButton ID="LnkAddNewDelegation" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewDelegation_Click"><li class="fa fa-save"></li>&nbsp;Add New Delegation</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="DivAddNewDelegation" tabindex="-1" role="form" aria-labelledby="myNewDelegation" aria-hidden="true">
        <div class="modal-dialog" style="width: 610px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="AddNewDelegationLabel">Create new delegation</h4>
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
                                                            <asp:DropDownList ID="DDLUsers" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Date From</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                        </td>
                                                        <td class="tdLabel">Date To</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                        </td>
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
                            </div>
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
            if ($(".PagesCount").text() != "0") {
                $('#ContentPlaceHolder1_GVDelegations').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: false
                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".PagesCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVDelegations').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false
                        });
                    }
                }
            });
        };
    </script>
</asp:Content>

