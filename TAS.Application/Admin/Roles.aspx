<%@ Page Title="Roles" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Admin_Roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function openModal() {
            $('#DivAddNewRole').modal('show');
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
                    <h4 class="page-header">Manage Roles</h4>
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
                             <asp:Label ID="lblCount" runat="server" Text="0" CssClass="hidden GVCount"></asp:Label>
                            <asp:GridView ID="GVRoles" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                AllowPaging="false" OnRowCommand="GVRoles_RowCommand"
                                DataKeyNames="RoleID,RoleName,UniqueField" OnRowDeleting="GVRoles_RowDeleting" OnRowDataBound="GVRoles_RowDataBound" EmptyDataText="No records to display">
                                <Columns>
                                    <asp:BoundField DataField="RoleName" HeaderText="Role Name" />
                                    <asp:BoundField DataField="UniqueField" HeaderText="Unique Field" HeaderStyle-CssClass="text-center" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                    
                                     <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center"  ItemStyle-Width="300px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                            <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="EditItem" ToolTip="Click to edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                               &nbsp;&nbsp;|&nbsp;&nbsp;
                                            <asp:LinkButton ID="ibDelete" CausesValidation="False" CommandName="deleteItem" ToolTip="Click to delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                <PagerStyle CssClass="PagingIOM" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9">
                        </div>
                        <div class="col-lg-3">
                            <asp:LinkButton ID="LnkAddNewRole" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewRole_Click"><li class="fa fa-save"></li>&nbsp;Add New Role</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="DivAddNewRole" tabindex="-1" role="form" aria-labelledby="myNewRole" aria-hidden="true">
        <div class="modal-dialog" style="width: 550px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="AddNewRoleLabel">Add New Role</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageAddNewRole" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgAddNewRole" runat="server" Text=""></asp:Label>
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
                                                        <td class="tdLabel">Role Name</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:TextBox ID="txtRoleName" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Unique Field Name</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:TextBox ID="txtUniqueFieldName" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <%--<tr>
                                                        <td class="tdLabel">Set Unique Field as True</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:CheckBox ID="CheckUniqueField" runat="server" />
                                                        </td>
                                                    </tr>--%>

                                                    <!--
                                                     <tr>
                                                        <td class="tdLabel">Is Admin</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:CheckBox ID="CheckIsAdmin" runat="server" />
                                                        </td>
                                                    </tr>

                                                     <tr>
                                                        <td class="tdLabel">Is Finance</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:CheckBox ID="CheckIsFinance"  runat="server" />
                                                        </td>
                                                    </tr>

                                                     <tr>
                                                        <td class="tdLabel">Is Approver</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:CheckBox ID="CheckIsApprover"  runat="server" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Is Radio Operator</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:CheckBox ID="CheckIsRadioOperator"  runat="server" />
                                                        </td>
                                                    </tr>
                                                    -->

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
             if ($(".GVCount").text() != "0") {
                 $('#ContentPlaceHolder1_GVRoles').DataTable({
                     "pagingType": "full_numbers",
                     stateSave: false
                 });
             }
         });
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         if (prm != null) {
             prm.add_endRequest(function (sender, e) {
                 if (sender._postBackSettings.panelsToUpdate != null) {
                     if ($(".GVCount").text() != "0") {
                         $('#ContentPlaceHolder1_GVRoles').DataTable({
                             "pagingType": "full_numbers",
                             stateSave: false
                         });
                     }
                 }
             });
         };
    </script>
    
</asp:Content>

