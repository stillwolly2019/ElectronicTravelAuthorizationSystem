<%@ Page Title="Roles Permissions" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RolesPermissions.aspx.cs" Inherits="Admin_RolesPermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
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
                    <h4 class="page-header">Role Permissions</h4>
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
                <!-- /.col-lg-12 -->
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-lg-6">
                        </div>
                        <div class="col-lg-3">
                            <div class="form-group">
                                <label>Role Name</label>
                                <asp:DropDownList ID="DDLRoles" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DDLRoles_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <div class="form-group">
                                <label>Page Name</label>
                                <asp:DropDownList ID="DDLParents" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DDLParents_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:GridView ID="GVRolesPermissions" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                OnRowCommand="GVRolesPermissions_RowCommand"
                                DataKeyNames="PageID" OnRowDeleting="GVRolesPermissions_RowDeleting" OnRowDataBound="GVRolesPermissions_RowDataBound" EmptyDataText="No records to display">
                                <Columns>
                                    <asp:TemplateField HeaderText="Page">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("PageSeparator") %>' ID="Label1"></asp:Label>
                                            <asp:Label runat="server" Text='<%# Bind("PageName") %>' ID="Label2"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Read" ShowHeader="False">
                                        <ItemTemplate>

                                            <asp:LinkButton ID="ibRead" CausesValidation="False" CommandName="PermissionRead" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="PermissionEdit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibAdd" CausesValidation="False" CommandName="PermissionAdd" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibDelete" CausesValidation="False" CommandName="PermissionDelete" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amend" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibAmend" CausesValidation="False" CommandName="PermissionAmend" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                <PagerStyle CssClass="PagingIOM" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        $(document).ready(function () {
            $('#ContentPlaceHolder1_GVRolesPermissions').DataTable({
                "ordering": false,
                "pagingType": "full_numbers",
                stateSave: true
            });
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $('#ContentPlaceHolder1_GVRolesPermissions').DataTable({
                        "ordering": false,
                        "pagingType": "full_numbers",
                        stateSave: true
                    });
                }
            });
        };
    </script>
</asp:Content>

