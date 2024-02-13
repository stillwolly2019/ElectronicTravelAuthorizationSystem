<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step2_StaffAcountedFor.aspx.cs" Inherits="Reports_Step2_DailyRadioCheck" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Reports/RCWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/css/Site.css" rel="stylesheet" />
    <link href="~/css/bootstrap.css" rel="stylesheet" />
    <!-- MetisMenu CSS -->
    <link href="~/bower_components/metisMenu/dist/metisMenu.min.css" rel="stylesheet" />
    <!-- Custom CSS -->
    <link href="~/dist/css/sb-admin-2.css" rel="stylesheet" />
    <!-- Custom Fonts -->
    <link href="~/bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #fff">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <script src="<%= Page.ResolveClientUrl("~/js/jquery-1.11.3.min.js") %>" type="text/javascript"></script>
        <script type="text/javascript">
            function SaveValidation() {
                var a = 0;
                $(".Req").each(function () {

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
        <uc1:WizardHeader runat="server" ID="WizardHeader" />
        <asp:UpdatePanel ID="UpdatePanelStep1" runat="server">
            <ContentTemplate>


                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">Step 2 - Staff Accounted For<asp:Label ID="NumberOfStaff" runat="server" Text="" ForeColor="red"></asp:Label></h4>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlContent" runat="server">
                            <div class="row" style="width: 99.9%;">
                                

                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <div style="text-align: right; font-weight: bold; padding-right: 20px;">
                                    <asp:Label ID="lblGVRCsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                                </div>
                                <asp:GridView ID="GVMyRCs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    DataKeyNames="PRISMNumber"
                                     EmptyDataText="No records to display">
                                    <Columns>
                                         <asp:TemplateField HeaderText="Radio Check Date" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRadioCheckDate" runat="server"
                                                    Text='<%# Eval("RadioCheckDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="PRISMNumber" HeaderText="PER NO" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DisplayName" HeaderText="Staff Name"/>
                                        <asp:BoundField DataField="DutyStation" HeaderText="Duty Station" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Title" HeaderText="Position Title"/>
                                        <asp:BoundField DataField="UnitDepartment" HeaderText="Unit/Department" />
                                        <asp:BoundField DataField="Category" HeaderText="Staff Category"/>
                                        <asp:BoundField DataField="CallSign" HeaderText="Call Sign" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CurrentLocation" HeaderText="Current Location" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="StatusLocation" HeaderText="Status-Location" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>




                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep1" DisplayAfter="100" DynamicLayout="False">
            <ProgressTemplate>
                <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                    <div style="padding-top: 200px;">
                        <img alt="Loading" src="../../images/ajax_loader_blue_512.gif" width="200px" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
    <!-- jQuery -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/jquery/dist/jquery.min.js") %>" type="text/javascript"></script>
    <!-- Bootstrap Core JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/bootstrap/dist/js/bootstrap.min.js") %>" type="text/javascript"></script>
    <!-- Metis Menu Plugin JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/bower_components/metisMenu/dist/metisMenu.min.js") %>" type="text/javascript"></script>
    <!-- Custom Theme JavaScript -->
    <script src="<%= Page.ResolveClientUrl("~/dist/js/sb-admin-2.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/js/jquery.are-you-sure.js") %>" type="text/javascript"></script>

</body>
</html>



