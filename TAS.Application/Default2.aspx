<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="TravelAuthorization_Default2" %>
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
                    <h4 class="page-header"><font color="#124a92"> <b>Dashboard:- </b> TAs pending action</font></h4>
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
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Travel Authorization Number</label>
                                        <asp:TextBox ID="txtTANo" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <div class="col-lg-2">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnAdd" runat="server" Text="Add new TA" CssClass="form-control btn-primary" OnClick="btnAdd_Click" />
                                </div>
                            </div>
                        </div>
                    </div>





                    <div class="row">
                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <div style="text-align: right; font-weight: bold; padding-right: 20px;">
                                    <asp:Label ID="lblGVTAsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                                </div>
                                <asp:GridView ID="GVMyTAs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    OnRowCommand="GVMyTAs_RowCommand" DataKeyNames="TravelAuthorizationID,StatusCode,ActionNeeded,TravelAuthorizationNumber" OnRowDeleting="GVMyTAs_RowDeleting"
                                    OnRowDataBound="GVMyTAs_RowDataBound" EmptyDataText="No records to display">
                                    <Columns>
                                        <asp:BoundField DataField="DateOfTravel" HeaderText="DateOfTravelSorting" />
                                        <asp:BoundField DataField="ReturnDate" HeaderText="ReturnDateSorting" />
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Travel Authorization Number" HeaderStyle-CssClass="text-center" ItemStyle-Width="220px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnIsTecComplete" Value='<%# Eval("IsTecComplete") %>' runat="server" />
                                                <div style="text-align: center;">
                                               <asp:LinkButton ID="ibDelete" runat="server" CommandName="VTA" ToolTip="View Travel Authorization" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("TravelAuthorizationNumber") %>'><li class="fa fa-search-plus"></li> View</asp:LinkButton>
                                                
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TravelersName" HeaderText="Traveler's Name" />
                                        <asp:BoundField DataField="PurposeOfTravel" HeaderText="Purpose Of Travel " />

                                        <asp:TemplateField HeaderText="Travel Date" HeaderStyle-CssClass="text-center"  ItemStyle-Width="110px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblDateOfTravel" runat="server"
                                                    Text='<%# Eval("DateOfTravel", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Return Date" HeaderStyle-CssClass="text-center"  ItemStyle-Width="110px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                <asp:Label ID="lblReturnDate" runat="server"
                                                    Text='<%# Eval("ReturnDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                                 </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="StatusDescription" HeaderText="Current Status " />
                                        <asp:BoundField DataField="ActionNeeded"  HeaderText="Action Needed" />

                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>




















                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="GVMyTAs" />
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
                $('#ContentPlaceHolder1_GVMyTAs').DataTable({
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
                        $('#ContentPlaceHolder1_GVMyTAs').DataTable({
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

