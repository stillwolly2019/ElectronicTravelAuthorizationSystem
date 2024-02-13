<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManagerTravelAuthorizationsSearch.aspx.cs" Inherits="TravelAuthorization_ManagerTravelAuthorizationsSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function ConfirmDuplicate() {
            return confirm("Are you sure you want to duplicate the TA?");
        }
        function StaffMembersSaveValidation() {
            var a = 0;
            $(".ReqStaff").each(function () {

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <h4 class="page-header">Search Staff Member TA</h4>
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>TA No.</label>
                                        <asp:TextBox ID="txtTANo" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Location </label>
                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Status</label>
                                        <asp:DropDownList ID="ddlStatusCode" runat="server" DataTextField="Description" DataValueField="Code" CssClass="form-control Req" />
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="form-control btn-primary" OnClick="btnSearch_Click" />
                                </div>
                                <div class="col-lg-2">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="form-control btn-primary" OnClick="btnClear_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <div style="text-align: right; font-weight: bold; padding-right: 20px;">
                                    <%--<asp:Label ID="lblCount" runat="server" Text=""></asp:Label>--%>
                                    <asp:Label ID="lblGVTAsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                                </div>
                                <script type="text/javascript">
                                    function openModal() {
                                        $('#DivDuplicateTA').modal('show');
                                    }
                                </script>
                                <asp:GridView ID="GVTAs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="GVTAs_RowCommand" DataKeyNames="TravelAuthorizationID,TravelAuthorizationNumber" OnRowDeleting="GVTAs_RowDeleting" OnRowDataBound="GVTAs_RowDataBound" EmptyDataText="No records to display">
                                    <Columns>
                                        <asp:BoundField DataField="FromLocationDateSorting" HeaderText="FromLocationDateSorting" />
                                        <asp:BoundField DataField="ToLocationDateSorting" HeaderText="ToLocationDateSorting" />
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="TA No.">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnIsTecComplete" Value='<%# Eval("IsTecComplete") %>' runat="server" />
                                                <asp:LinkButton ID="ibDelete" runat="server" CommandName="VTA" ToolTip="View Travel Authorization" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("TravelAuthorizationNumber") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="TravelersName" HeaderStyle-Width="250px" HeaderText="Traveler's Name" />
                                        <asp:TemplateField HeaderText="Departure Date From">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartureDateFrom" runat="server"
                                                    Text='<%# Eval("FromLocationDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Departure Date To">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartureDateto" runat="server"
                                                    Text='<%# Eval("ToLocationDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StatusCode" HeaderText="Status " />
                                        <asp:BoundField DataField="CONCATLegs" HeaderStyle-Width="300px" HeaderText="Travel Itinerary" />
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-Width="85px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPrintTA" runat="server"  CommandName="PrintTA" ToolTip="Print Travel Authorization" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-print"></li> Print TA</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDuplicateTA" runat="server"  CommandName="DuplicateTA" ToolTip="Duplicate TA for another staff member" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-plus"></li> Duplicate TA</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-Width="95px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPrintTE" runat="server"  CommandName="PrintTE" ToolTip="Print Travel Expense Claim" CommandArgument='<%# Container.DisplayIndex %>' ForeColor="Green"><li class="fa fa-print" style="color:green"></li> Print TEC</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--  <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                            <PagerStyle CssClass="PagingIOM" />--%>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GVTAs" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="100" DynamicLayout="False">
        <ProgressTemplate>
            <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                <div style="padding-top: 200px;">
                    <img alt="Loading" src="../images/ajax_loader_blue_512.gif" width="150px" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script>
        $(document).ready(function () {
            if ($(".IntCount").text() != "0") {
                $('#ContentPlaceHolder1_GVTAs').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: false,
                    responsive: true,
                    "iDisplayLength": 25,
                    "order": [[0, "desc"]],
                    "columnDefs": [{ "targets": [0], "visible": false, "searchable": false },
                    { "targets": [1], "visible": false, "searchable": false },
                    { "targets": [4], "orderData": [0] },
                    { "targets": [5], "orderData": [1] }]

                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".IntCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVTAs').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false,
                            responsive: true,
                            "iDisplayLength": 25,
                            "order": [[0, "desc"]],
                            "columnDefs": [{ "targets": [0], "visible": false, "searchable": false },
                            { "targets": [1], "visible": false, "searchable": false },
                            { "targets": [4], "orderData": [0] },
                            { "targets": [5], "orderData": [1] }]
                        });
                    }
                }
            });
        };
    </script>
    <div class="modal fade" id="DivDuplicateTA" tabindex="-1" role="form" aria-labelledby="DuplicateTALabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 450px">
            <div class="modal-content" style="width: auto;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Duplicate TA</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageDuplicateTA" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgDuplicateTA" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelMessageDuplicateTARejection" runat="server" CssClass="alert alert-danger alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgDuplicateTARejection" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <asp:DropDownList ID="ddlStaffMembers" DataTextField="FullName" DataValueField="UserID" CssClass="form-control ReqStaff" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-6">
                                    <label>&nbsp;</label>
                                    <asp:LinkButton ID="ibAddStaff" ToolTip="Click to save" OnClick="ibAddStaff_Click" OnClientClick="return StaffMembersSaveValidation();" runat="server"><li class="fa fa-plus"></li> Click to save</asp:LinkButton>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <br />
                                    <asp:GridView ID="gvStaffMembers" runat="server" CssClass="table table-bordered  table-hover"
                                        AutoGenerateColumns="False" AllowPaging="false"
                                        OnRowCommand="gvStaffMembers_RowCommand"
                                        OnRowDataBound="gvStaffMembers_RowDataBound"
                                        OnRowDeleting="gvStaffMembers_RowDeleting"
                                        DataKeyNames="UserID">
                                        <Columns>
                                            <asp:BoundField DataField="FullName" HeaderText="Staff member to duplicate" />
                                            <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-Width="170px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ibDeleteStaff" runat="server" Width="25px" CommandName="Delete" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();"><li class="fa fa-times" ></li> Delete Staff Member</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="PagingIOM" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="row" style="direction: rtl">
                                <div class="col-lg-5">
                                </div>
                                <div class="col-lg-5">
                                    <asp:Button ID="btnDuplicate" Text="Duplicate TA" CssClass="form-control btn-primary" OnClientClick="return ConfirmDuplicate();" OnClick="btnDuplicate_Click" runat="server" />
                                </div>
                                <div class="col-lg-2">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
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
</asp:Content>

