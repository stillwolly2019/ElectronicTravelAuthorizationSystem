<%@ Page Title="Trip Schema Documents" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TripSchemaDocuments.aspx.cs" Inherits="Admin_TripSchemaDocuments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
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
                    <h4 class="page-header" style="color:darkblue"><b>Document Types by Trip Schema</b></h4>
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
                                <label>Document Type</label>
                                <asp:DropDownList ID="DDLDocumentTypes" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DDLStatusCodes_SelectedIndexChanged">
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
                            <asp:GridView ID="GVStatusCodeRejectionReasons" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                OnRowCommand="GVStatusCodeRejectionReasons_RowCommand"
                                DataKeyNames="LookupsID,ApplyDocument,Description" OnRowDataBound="GVStatusCodeRejectionReasons_RowDataBound" EmptyDataText="No records to display">
                                <Columns>
                                    
                                    <asp:TemplateField HeaderText="Trip Schema description">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Label2"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Apply Document to this Trip Schema" ShowHeader="False" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibApply" CausesValidation="False" CommandName="GrantPermission" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
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
            $('#ContentPlaceHolder1_GVStatusCodeRejectionReasons').DataTable({
                "ordering": false,
                "pagingType": "full_numbers",
                stateSave: true
            });
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $('#ContentPlaceHolder1_GVStatusCodeRejectionReasons').DataTable({
                        "ordering": false,
                        "pagingType": "full_numbers",
                        stateSave: true
                    });
                }
            });
        };
    </script>
</asp:Content>
