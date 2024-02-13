<%@ Page Title="TA Status Control" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TAStatuses.aspx.cs" Inherits="Admin_TAStatuses" %>
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
                    <h4 class="page-header">TA Statuses Control</h4>
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
                            <asp:GridView ID="GVTAStatuses" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                OnRowCommand="GVTAStatuses_RowCommand"
                                DataKeyNames="LookupsID" OnRowDataBound="GVTAStatuses_RowDataBound" EmptyDataText="No records to display">
                                <Columns>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Label2"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:BoundField DataField="Code" HeaderText="Status Code " HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />

                                    <asp:TemplateField HeaderText="By Travellor" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ByTravellor" CausesValidation="False" CommandName="MarkAsByTravellor" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="By Supervisor" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="BySupervisor" CausesValidation="False" CommandName="MarkAsBySupervisor" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="By RMO" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ByRMO" CausesValidation="False" CommandName="MarkAsByRMO" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="By Approver" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ByApprover" CausesValidation="False" CommandName="MarkAsByApprover" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Is Actioned" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibIsActioned" CausesValidation="False" CommandName="MarkAsActioned" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Is Editable" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibIsEditable" CausesValidation="False" CommandName="MarkAsEditable" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Is Returnable" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibIsReturnable" CausesValidation="False" CommandName="MarkAsReturnable" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Is Cancellable" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibIsCancellable" CausesValidation="False" CommandName="MarkAsCancellable" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
                                                    </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Is Delegateble" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibIsDelegatable" CausesValidation="False" CommandName="MarkAsDelegatable" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                        </div>
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
            $('#ContentPlaceHolder1_GVTAStatuses').DataTable({
                "ordering": false,
                "pagingType": "full_numbers",
                stateSave: true
            });
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $('#ContentPlaceHolder1_GVTAStatuses').DataTable({
                        "ordering": false,
                        "pagingType": "full_numbers",
                        stateSave: true
                    });
                }
            });
        };
    </script>
</asp:Content>

