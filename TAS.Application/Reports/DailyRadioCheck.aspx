<%@ Page Title="Daily Radio Check" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DailyRadioCheck.aspx.cs" Inherits="Reports_DailyRadioCheck" %>
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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-lg-12">
                    <h2 class="page-header">
                        <li class="fa fa-book" style="font-size: 40px; color: #337ab7"></li>
                        Radio Check Overview</h2>
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2">
                    <div class="form-group">
                        <label>Radio Check Date</label>
                        <asp:TextBox ID="txtRadioCheckDate" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtRadioCheckDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
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

            </div>

            <div class="row" style="padding-bottom: 10px">
                <table style="width: 99%;">
                    <tr>
                        <td style="width: 80%"></td>

                        <td style="text-align: right; font-size: 10pt; width: 10%">Location:&nbsp;&nbsp;  
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLocationsName" CssClass="form-control" Width="176" DataTextField="LocationName" DataValueField="LocationID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLocationsName_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="dataTable_wrapper">
                        <div style="text-align: right; font-weight: bold; padding-right: 20px;">
                            <asp:Label ID="lblGVRCsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                        </div>
                        <asp:GridView ID="GVMyRCs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                            OnRowCommand="GVMyRCs_RowCommand" DataKeyNames="RadioCheckID,RadioCheckDate,LocationName,LocationID,StaffCount" OnRowDeleting="GVMyRCs_RowDeleting" EmptyDataText="No records to display">
                            <Columns>
                                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Radio Check Date" SortExpression="RadioCheckID" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnLocationID" Value='<%# Eval("LocationID") %>' runat="server" />
                                        <asp:HiddenField ID="hdnRadioCheckID" Value='<%# Eval("RadioCheckID") %>' runat="server" />
                                        <asp:LinkButton runat="server" CommandName="VRC" ToolTip="View Details" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("RadioCheckDate", "{0:dd-MMM-yyyy}") %>'><li class="fa fa-search-plus"></li> View</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LocationName" HeaderText="Radio Check Location" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="StaffCount" HeaderText="Number of Staff" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AccountedFor" HeaderText="Staff Accounted" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnAccountedFor" HeaderText="Staff Un Accounted" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Arrivals" HeaderText="Arrivals" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Departures" HeaderText="Departures" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                
                                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Action" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPrintRC" runat="server" CommandName="PrintRC" ToolTip="Export to PDF" CommandArgument='<%# Container.DisplayIndex %>'><li class=" fa fa-file-pdf-o"></li> Export to PDF</asp:LinkButton>
                                        &nbsp; &nbsp;| &nbsp; &nbsp;
                                        <asp:LinkButton ID="lnkPrintRCExcel" runat="server" CommandName="PrintExcel" ToolTip="Export to Excel" CommandArgument='<%# Container.DisplayIndex %>'><li class=" fa fa-file-excel-o"></li> Export to Excel</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100" DynamicLayout="false">
                <ProgressTemplate>
                    <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                        <div style="padding-top: 200px;">
                            <img alt="Loading" src="../images/ajax_loader_blue_512.gif" width="300px" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="GVMyRCs" />
        </Triggers>
    </asp:UpdatePanel>

  <script>
        $(document).ready(function () {
            if ($(".IntCount").text() != "0") {
                $('#ContentPlaceHolder1_GVMyRCs').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: true,
                    "order": [[1, "desc"]],
                    responsive: true,
                    "iDisplayLength": 25,
                    "columnDefs": [{ "targets": [0], "visible": true, "searchable": true },
                    { "targets": [1], "visible": true, "searchable": false },
                    { "targets": [4], "orderData": [0] },
                    { "targets": [5], "orderData": [1] }]
                });
            }






            var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".IntCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVMyRCs').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: true,
                            "order": [[1, "desc"]],
                            responsive: true,
                            "iDisplayLength": 25,
                            "columnDefs": [{ "targets": [0], "visible": true, "searchable": true,"retreive":true,"paging":false },
                            { "targets": [1], "visible": true, "searchable": false },
                            { "targets": [4], "orderData": [0] },
                            { "targets": [5], "orderData": [1] }]
                        });
                    }
                }
            });
        };







        }
        );

        //var prm = Sys.WebForms.PageRequestManager.getInstance();
        //if (prm != null) {
        //    prm.add_endRequest(function (sender, e) {
        //        if (sender._postBackSettings.panelsToUpdate != null) {
        //            if ($(".IntCount").text() != "0") {
        //                $('#ContentPlaceHolder1_GVMyRCs').DataTable({
        //                    "pagingType": "full_numbers",
        //                    stateSave: true,
        //                    "order": [[1, "desc"]],
        //                    responsive: true,
        //                    "iDisplayLength": 25,
        //                    "columnDefs": [{ "targets": [0], "visible": true, "searchable": true,"retreive":true,"paging":false },
        //                    { "targets": [1], "visible": true, "searchable": false },
        //                    { "targets": [4], "orderData": [0] },
        //                    { "targets": [5], "orderData": [1] }]
        //                });
        //            }
        //        }
        //    });
        //};

    </script>

</asp:Content>
