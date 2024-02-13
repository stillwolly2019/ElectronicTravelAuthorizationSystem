<%@ Page Title="My Notifications" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StaffNotifications.aspx.cs" Inherits="RadioCheck_StaffNotifications" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function SaveValidation() {
            var a = 0;
            $(".Req").each(function () {
                if ($(this).val() == "") {
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
        
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <h4 class="page-header">My Notifications of absence</h4>
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
                                        <label>Notification No.</label>
                                        <asp:TextBox ID="txtMRNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <%--<div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Location</label>
                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>--%>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Status</label>
                                        <asp:DropDownList ID="ddlStatusCode" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control Req" />
                                    </div>
                                </div>
                                <div class="col-lg-1">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="form-control btn-primary" OnClick="BtnSearch_Click" />
                                </div>
                                <div class="col-lg-1">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="form-control btn-primary" OnClick="BtnClear_Click" />
                                </div>
                                <div class="col-lg-2">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="BtnAdd" runat="server" Text="Add New Notification" CssClass="form-control btn-primary" OnClick="BtnAdd_Click" />
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
                    "order": [[0, "desc"]],
                    responsive: true,
                    "iDisplayLength": 25,
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
                            "order": [[0, "desc"]],
                            responsive: true,
                            "iDisplayLength": 25,
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
</asp:Content>






