<%@ Page Title="Search Travel Authorizations" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SearchTravelAuthorization.aspx.cs" Inherits="TravelAuthorization_SearchTravelAuthorization" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <h4 class="page-header">Search Travel Authorization</h4>
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
                                        <label>WBS </label>
                                        <asp:TextBox ID="txtWBSCode" runat="server" CssClass="form-control uppercase" placeholder="AA.NNNN.AANN.NN.NN.NNN"></asp:TextBox>
                                        <asp:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" Enabled="True" TargetControlID="txtWBSCode" ID="txtWBSCode_MaskedEditExtender" Mask="LL.9999.LL99.99.99.999"></asp:MaskedEditExtender>
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
                                    <asp:Label ID="lblGVTAsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                                </div>
                                <asp:GridView ID="GVTAs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    OnRowCommand="GVTAs_RowCommand" DataKeyNames="TravelAuthorizationID,TravelAuthorizationNumber" OnRowDeleting="GVTAs_RowDeleting"
                                    OnRowDataBound="GVTAs_RowDataBound" EmptyDataText="No records to display">
                                    <Columns>
                                        <asp:BoundField DataField="FromLocationDateSorting" HeaderText="FromLocationDateSorting" />
                                        <asp:BoundField DataField="ToLocationDateSorting" HeaderText="ToLocationDateSorting" />
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="TA No.">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ibDelete" runat="server"  CommandName="VTA" ToolTip="View Travel Authorization" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("TravelAuthorizationNumber") %>'></asp:LinkButton>
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
                                        <asp:TemplateField HeaderText="Arrival Date">
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
                                    </Columns>
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
</asp:Content>

