<%@ Page Title="Manage Lookups" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageLookups.aspx.cs" Inherits="Lookups_ManageLookups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function openModal() {
            $('#DivAddNewLookup').modal('show');
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
                    <h4 class="page-header">Manage Lookups</h4>
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
                             <asp:Label ID="lblCount" runat="server" Text="0" CssClass="hidden GVCount"></asp:Label>
                            <asp:GridView ID="GVLookups" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                OnRowCommand="GVLookups_RowCommand"
                                DataKeyNames="LookupsID,LookupGroupID,SubGroupID,Code,Description,LongDescription" OnRowDeleting="GVLookups_RowDeleting" OnRowDataBound="GVLookups_RowDataBound" EmptyDataText="No records to display">
                                <Columns>
                                    <asp:BoundField DataField="LookupGroup" HeaderText="Lookup Group" />
                                    <asp:BoundField DataField="SubGroupName" HeaderText="Lookup Sub Group" />
                                    <asp:BoundField DataField="Code" HeaderText="Code" />
                                     <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" Text='<%# Eval("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="EditLookup" ToolTip="Click to edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibDelete" CausesValidation="False" CommandName="deleteItem" ToolTip="Click to delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9">
                        </div>
                        <div class="col-lg-3">
                            <asp:LinkButton ID="LnkAddNewLookup" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewLookup_Click"><li class="fa fa-save"></li>&nbsp;Add New Lookup</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="DivAddNewLookup" tabindex="-1" role="form" aria-labelledby="myNewLookup" aria-hidden="true">
        <div class="modal-dialog" style="width: 625px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="AddNewUserLabel">Add New Lookup</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageAddNewLookup" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgAddNewLookup" runat="server" Text=""></asp:Label>
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
                                                        <td class="tdLabel">Lookup Group</td>
                                                        <td class="tdField">
                                                            <asp:DropDownList ID="ddlLookupsGroupName" runat="server" DataTextField="LookupGroup" DataValueField="LookupGroupID" CssClass="form-control Req" />
                                                        </td>
                                                        <td class="tdLabel">Sub Lookup Group</td>
                                                        <td class="tdField">
                                                           <asp:DropDownList ID="ddlLookupsSubGroup" runat="server" DataTextField="LookupGroup" DataValueField="LookupGroupID" CssClass="form-control" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">Code</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtCode" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="tdLabel">Description</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtDescription" CssClass="form-control Req" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">Long Description</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtLongDescription" Height="70px" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
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
             if ($(".GVCount").text() != "0") {
                 $('#ContentPlaceHolder1_GVLookups').DataTable({
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
                         $('#ContentPlaceHolder1_GVLookups').DataTable({
                             "pagingType": "full_numbers",
                             stateSave: false
                         });
                     }
                 }
             });
         };
    </script>
</asp:Content>

