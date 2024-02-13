<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="TravelAuthorization_TAWizard_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/TravelAuthorization/TAWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>
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

                    <div class="panel-body">
                        <asp:Panel ID="pnlContent" runat="server">
                        <div class="row">
                            <div class="col-lg-12">
                                 <div class="panel-heading">
                        <table>
                            <tr>
                                <td class="tdLabel" style="font-size: 14px">Travel Authorization Number</td>
                                <td class="tdField" id="divStartDate" runat="server">
                                    <asp:TextBox ID="txtTANo" runat="server" CssClass="form-control" Width="280px"></asp:TextBox>
                                </td>

                                <td class="tdField">
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click"   runat="server" CssClass="form-control btn-primary" Text="Search" Font-Size="14px" Width="140px" />
                                </td>
                                <td class="tdField">
                                    <asp:Button ID="btnClear" OnClick="btnClear_Click"  runat="server" CssClass="form-control btn-primary" Text="Clear" Font-Size="14px" Width="140px"/>
                                </td>
                                <td class="tdField">
                                    <asp:Button ID="BtnAdd" OnClick="btnAdd_Click"  runat="server" CssClass="form-control btn-primary" Text="Add new TA" Font-Size="14px" Width="140px"/>
                                </td>


                            </tr>

                        </table>
                    </div>
                            </div>
                        </div>


                            <div class="row" style="width: 99.9%;">
                                

                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <div style="text-align: right; font-weight: bold; padding-right: 20px;">
                                    <asp:Label ID="lblGVTAsCount" runat="server" Text="0" CssClass="hidden IntCount"></asp:Label>
                                </div>

                             
                                <asp:GridView ID="GVMyTAs" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                    OnRowCommand="GVMyTAs_RowCommand" DataKeyNames="TravelAuthorizationID,StatusCode,TravelAuthorizationNumber"
                                    EmptyDataText="No records to display">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Travel Authorization Number">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnIsTecComplete" Value='<%# Eval("IsTecComplete") %>' runat="server" />
                                                <asp:LinkButton ID="ibDelete" runat="server" CommandName="VTA" ToolTip="View Travel Authorization" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("TravelAuthorizationNumber") %>'><li class="fa fa-search-plus"></li> View</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TravellorFullName" HeaderText="Traveler's Name" />
                                        <asp:TemplateField HeaderText="Departure Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartureDateFrom" runat="server"
                                                    Text='<%# Eval("StartDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Return Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartureDateto" runat="server"
                                                    Text='<%# Eval("EndDate", "{0:dd-MMM-yyyy}") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PurposeOfTravel"  HeaderText="Purpose of Travel" />
                                        <asp:BoundField DataField="StatusDescription" HeaderText="Current status " />

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

        <script>
        $(document).ready(function () {
            if ($(".StaffRadioCheckCount").text() != "0") {
                $('#ContentPlaceHolder1_GVStaffRadioCheck').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: true
                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".StaffRadioCheckCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVStaffRadioCheck').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: true
                        });
                    }
                }
            });
        };
    </script>

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

