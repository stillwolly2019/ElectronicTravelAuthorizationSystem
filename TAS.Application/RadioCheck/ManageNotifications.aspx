<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageNotifications.aspx.cs" Inherits="RadioCheck_ManageNotifications" %>
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
                    <h4 class="page-header">Search Staff Member Notification request</h4>
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
                                        <label>NOA No.</label>
                                        <asp:TextBox ID="txtMRNO" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <!--<div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Location </label>
                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"></asp:TextBox>

                                    </div>
                                </div>-->
                                <%--<div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Status</label>
                                        <asp:DropDownList ID="ddlStatusCode" runat="server" DataTextField="Description" DataValueField="Code" CssClass="form-control Req" />
                                    </div>
                                </div>--%>
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
                                    <asp:Label ID="lblGVMRsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                                </div>
                                <asp:GridView ID="GVMRs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    OnRowCommand="GVMRs_RowCommand" DataKeyNames="MovementRequestID,MovementRequestNumber" OnRowDeleting="GVMRs_RowDeleting"
                                    EmptyDataText="No records to display">

                                    <Columns>

                                        <asp:BoundField DataField="StartDate" HeaderText="DateFromSorting" />
                                        <asp:BoundField DataField="EndDate" HeaderText="DateToSorting" />

                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Notification No." ItemStyle-Width="150px" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibDelete" runat="server" CommandName="VMR" ToolTip="View Movement Request" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("MovementRequestNumber") %>'> View</asp:LinkButton>
                                            </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="FullName" HeaderText="Staff Name" />
                                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-CssClass="text-center"  ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateFrom" runat="server"
                                                    Text='<%# Eval("StartDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                    </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Date" HeaderStyle-CssClass="text-center"  ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateto" runat="server"
                                                    Text='<%# Eval("EndDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="PurposeOfLeave" HeaderText="Notification Comments">
                                        </asp:BoundField>

                                         <asp:BoundField DataField="AddressWhileAbsent" HeaderText="Address while absent">
                                        </asp:BoundField>

                                         <asp:BoundField DataField="StatusDescription" HeaderText="Current Status">
                                        </asp:BoundField>

                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Action" HeaderStyle-CssClass="text-center" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="lnkPrintMR" runat="server" CssClass="text-center" CommandName="PrintMR" ToolTip="Print MovementvRequest" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-print"></li> Print MR</asp:LinkButton>
                                            </div></ItemTemplate>
                                        </asp:TemplateField>

                                      
                                    </Columns>
                                </asp:GridView>
                            </div>
                            
                        </div>
                    </div>






                  <%--  <div class="row">
                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <div style="text-align: right; font-weight: bold; padding-right: 20px;">
                                    <asp:Label ID="lblGVMRsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                                </div>
                                <asp:GridView ID="GVMRs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    OnRowCommand="GVMRs_RowCommand" DataKeyNames="MovementRequestID,CurrentStatus,StatusDescription,MovementRequestNumber" OnRowDeleting="GVMRs_RowDeleting"
                                    EmptyDataText="No records to display">
                                    <Columns>

                                        <asp:BoundField DataField="DateFromSorting" HeaderText="DateFromSorting" />
                                        <asp:BoundField DataField="DateToSorting" HeaderText="DateToSorting" />

                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Movement Request No." HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="ibDelete" runat="server" CommandName="VMR" ToolTip="View Movement Request" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("MovementRequestNumber") %>'> View</asp:LinkButton>
                                            </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="TravellorFullName" HeaderText="Requester's Name" />
                                        <asp:TemplateField HeaderText="Leave Start Date" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateFrom" runat="server" 
                                                    Text='<%# Eval("StartDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                    </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Leave End Date" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDepartureDateto" runat="server"
                                                    Text='<%# Eval("EndDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="PurposeOfLeave" HeaderText="Purpose/Comments "/>
                                        <asp:BoundField DataField="StatusDescription" HeaderText="Current Status "/>

                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:LinkButton ID="lnkPrintMR" runat="server" CssClass="text-center" CommandName="PrintMR" ToolTip="Print MovementvRequest" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-print"></li> Print MR</asp:LinkButton>
                                            </div></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;">

                                                <asp:LinkButton ID="ibDeleteMR" runat="server" CommandName="Delete" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();"><li class="fa fa-times" ></li> Delete MR</asp:LinkButton>
                                            </div>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            
                        </div>
                    </div>--%>













                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GVMRs" />
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
                $('#ContentPlaceHolder1_GVMRs').DataTable({
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
                        $('#ContentPlaceHolder1_GVMRs').DataTable({
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
    <div class="modal fade" id="DivDuplicateMR" tabindex="-1" role="form" aria-labelledby="DuplicateMRLabel" aria-hidden="true">
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
                                    <asp:Panel ID="PanelMessageDuplicateMR" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgDuplicateMR" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelMessageDuplicateMRRejection" runat="server" CssClass="alert alert-danger alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgDuplicateMRRejection" runat="server" Text=""></asp:Label>
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



