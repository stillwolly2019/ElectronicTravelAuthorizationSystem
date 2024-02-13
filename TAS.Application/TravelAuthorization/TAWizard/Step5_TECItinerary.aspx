<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step5_TECItinerary.aspx.cs" Inherits="TravelAuthorization_TAWizard_Step5_TECItinerary" %>

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
    <style type="text/css">
        .Left {
            padding: 0;
            margin: 0;
        }
    </style>
</head>
<body style="background-color: #fff">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <script src="<%= Page.ResolveClientUrl("~/js/jquery-1.11.3.min.js") %>" type="text/javascript"></script>
        <script type="text/javascript">
            function ConfirmDelete() {
                return confirm("Are you sure you want to delete?");
            }
            function ConfirmDeleteDSA() {
                return confirm("Are you sure you want to delete?");
            }
            function SaveValidation() {
                var a = 0;
                $(".ReqDSA").each(function ()
                {
                    if ($(this).val() == "" || $(this).val() == null)
                    {
                        $(this).addClass("invalid");
                        a = a + 1;
                    }
                    else
                    {
                        $(this).removeClass("invalid");
                    }
                });
                if (a > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        </script>
        <uc1:WizardHeader runat="server" ID="WizardHeader" />
        <asp:UpdatePanel ID="UpdatePanelStep5" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">Step 6 - TEC Itinerary</h4>
                            </div>
                            <div class="panel-body">
                                <asp:Panel ID="pnlContent" runat="server">
                                    <table>
                                        <tr>
                                            <td class="tdField" style="width: 100px">Exchange rate    
                                            </td>
                                            <td class="tdField">
                                                <asp:TextBox ID="txtExchangeRate" CssClass="form-control" runat="server" Width="300px"></asp:TextBox>
                                                <asp:Label ID="lblNameOfClaimant" runat="server" Text="" Font-Underline="true" Font-Bold="true" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="PanelItenMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                                    <asp:Label ID="lblItenmsg" runat="server" Text=""></asp:Label>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="hdnStatusCode" runat="server" />
                                                <asp:HiddenField ID="hdnUserID" runat="server" />
                                                <asp:HiddenField ID="hdnTravelSchema" runat="server" />
                                                <asp:GridView ID="GVItinerary" CssClass="table table-bordered  table-hover" runat="server" AutoGenerateColumns="False" OnRowCreated="GVItinerary_RowCreated" ShowFooter="True" OnRowDataBound="GVItinerary_RowDataBound" DataKeyNames="TravelItineraryID,TECItineraryID" OnRowCommand="GVItinerary_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="DepArr" HeaderText="City of Departure and Arrival" HeaderStyle-Wrap="false">
                                                            <FooterStyle Font-Bold="True" />

                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FromCity" HeaderText="City of Departure and Arrival"></asp:BoundField>
                                                        <asp:BoundField DataField="FromLocationDate" HeaderText="Date" ItemStyle-Wrap="false"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Local Time (24hr)">
                                                            <ItemTemplate> 
                                                                <asp:TextBox ID="txtFromLocationTime" CssClass="form-control" runat="server" Text='<%# Eval("FromLocationTime") %>'></asp:TextBox>
                                                                <asp:MaskedEditExtender runat="server" CultureDatePlaceholder="" CultureTimePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureDateFormat="" CultureCurrencySymbolPlaceholder="" CultureAMPMPlaceholder="" Century="2000" Enabled="True" TargetControlID="txtFromLocationTime" ID="txtFromLocationTime_MaskedEditExtender" Mask="99:99" MaskType="Time"></asp:MaskedEditExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ModeOfTravelName" HeaderText="Mode Of Travel" HeaderStyle-Wrap="false"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="No. kms (for Car)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtNoOfKms" CssClass="form-control" runat="server" Text='<%# decimal.Round(Convert.ToDecimal(Eval("NoOfKms")), 2, MidpointRounding.AwayFromZero).ToString() %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                TOTAL SECTION 1:
                                                            </FooterTemplate>
                                                            <FooterStyle Font-Bold="true" Wrap="false" />

                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                            </ItemTemplate>
                                                            <HeaderStyle BackColor="#eeeeee" BorderColor="#eeeeee" Width="1px" CssClass="Left" />
                                                            <ItemStyle BackColor="#eeeeee" BorderColor="#eeeeee" Width="1px" CssClass="Left" />
                                                            <FooterStyle BackColor="#eeeeee" BorderColor="#eeeeee" Width="1px" CssClass="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No. of Days" ItemStyle-CssClass="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtNoOfDays" CssClass="form-control" runat="server" Text='<%# decimal.Round(Convert.ToDecimal(Eval("NoOfDays")), 2, MidpointRounding.AwayFromZero).ToString() %>' ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblNoOfDaysTotal" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DSA Rate in USD">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDSARate" CssClass="form-control" Width="140px" runat="server" Text='<%# Eval("AllDSARates") %>' ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblDSARateTotal" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount USD">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAmount" CssClass="form-control" runat="server" Text='<%# decimal.Round(Convert.ToDecimal(Eval("RateAmount")), 2, MidpointRounding.AwayFromZero).ToString()  %>' ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblAmountTotal" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Local Amount">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAmountLocal" CssClass="form-control" runat="server" Text='<%# decimal.Round(Convert.ToDecimal(Eval("LocalAmount")), 2, MidpointRounding.AwayFromZero).ToString() %>' ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblAmountLocalTotal" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibDSABreakdown" runat="server" CommandName="Select" CommandArgument='<%# Container.DisplayIndex %>'>DSA&nbsp;Breakdown</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>
                                                <script type="text/javascript">
                                                    function openModal() {
                                                        $('#myModal').modal('show');
                                                    }

                                                </script>
                                                <asp:HiddenField ID="hfNoOfDaysTotal" runat="server" />
                                                <asp:HiddenField ID="hfDSARateTotal" runat="server" />
                                                <asp:HiddenField ID="hfAmountTotal" runat="server" />
                                                <asp:HiddenField ID="hfRateAmountTotal" runat="server" />

                                                <asp:HiddenField ID="hfDSAID" runat="server" Value="" />
                                                <asp:HiddenField ID="hfDSATAID" runat="server" Value="" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel">
                                                <div style="float: right">
                                                    <asp:Button ID="btnSaveItinerary" CssClass="btn btn-primary form-control" runat="server" Text="Submit & continue" OnClick="btnSaveItinerary_Click" Width="280px" />
                                                    <asp:Button ID="btnSaveItineraryDSA" CssClass="btn btn-primary form-control" runat="server" Text="Save DSA Information" OnClick="btnSaveItineraryDSA_Click" Visible="False" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep5" DisplayAfter="100" DynamicLayout="False">
            <ProgressTemplate>
                <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                    <div style="padding-top: 200px;">
                        <img alt="Loading" src="../../images/ajax_loader_blue_512.gif" width="200px" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="modal fade" id="myModal" tabindex="-1" role="form" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content" style="width: auto;">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalLabel">Daily Subsistence Allowance (DSA) Breakdown</h4>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="panel panel-primary">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        Arriving to:
                                                    <asp:Label ID="lblArr" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        On:
                                                    <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div id="DSARateid" runat="server" class="row">
                                            <div class="col-lg-12">
                                                <div class="form-group">
                                                    <label>DSA Rate</label>
                                                    <asp:TextBox ID="txtDSARateDSA" CssClass="form-control ReqDSA" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtDSARateDSA" ValidationExpression="[\d]{1,4}([.][\d]{1,2})?" Display="Static" EnableClientScript="true" ForeColor="Red" ErrorMessage="Please enter numbers only" ValidationGroup="NumOnly" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel panel-green" id="Divrate" runat="server">
                                            <div class="panel-body">
                                                <div class="row">
                                                    <div class="col-lg-12">
                                                        <asp:Panel ID="pnlDSAMsg" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                                            <asp:Label ID="lblDSAMsg" runat="server" Text=""></asp:Label>
                                                        </asp:Panel>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="form-group">
                                                            <label>No. Of Days</label>
                                                            <asp:TextBox ID="txtNoOfDaysDSA" CssClass="form-control ReqDSA" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtNoOfDaysDSA" ValidationExpression="[\d]{1,4}([.][\d]{1,2})?" Display="Static" EnableClientScript="true" ForeColor="Red" ErrorMessage="Please enter numbers only" ValidationGroup="NumOnly" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-5">
                                                        <div class="form-group">
                                                            <label>Percentage</label>
                                                            <asp:TextBox ID="txtPercentageDSA" CssClass="form-control ReqDSA" MaxLength="3" runat="server"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtPercentageDSA" ValidationExpression="[\d]{1,4}([.][\d]{1,2})?" Display="Static" EnableClientScript="true" ForeColor="Red" ErrorMessage="Please enter numbers only" ValidationGroup="NumOnly" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-1">
                                                        <div class="form-group">
                                                            <label>&nbsp;</label>
                                                            <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" ValidationGroup="NumOnly"><li class="fa fa-plus" style="font-size:26px"></li></asp:LinkButton>
                                                            <%--<asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" ValidationGroup="NumOnly" OnClientClick="return SaveValidation();"><li class="fa fa-plus" style="font-size:26px"></li></asp:LinkButton>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="table table-hover table-responsive">
                                    <asp:GridView ID="gvDSABreakdown" CssClass="table table-bordered  table-hover" runat="server" OnRowDataBound="gvDSABreakdown_RowDataBound" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" EmptyDataText="No data available" OnPageIndexChanging="gvDSABreakdown_PageIndexChanging" OnRowCommand="gvDSABreakdown_RowCommand" OnRowDeleting="gvDSABreakdown_RowDeleting" DataKeyNames="TECItineraryDSAID,TECItineraryID">
                                        <Columns>
                                            <asp:BoundField DataField="NoOfDays" HeaderText="No Of Days" />
                                            <asp:BoundField DataField="Percentage" HeaderText="Percentage" />
                                            <asp:BoundField DataField="DSARate" HeaderText="DSA Rate" />
                                            <asp:BoundField DataField="RateAmount" HeaderText="Amount (USD)" />
                                            <asp:BoundField DataField="LocalAmount" HeaderText="Local Amount" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="ibDelete" runat="server" Width="25px" CommandName="Delete" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDeleteDSA();"><li class="fa fa-times" style="font-size:24px"></li></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="PagingIOM" />
                                    </asp:GridView>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:Label ID="lblActmsg" runat="server" Text="&nbsp;"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GVItinerary" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->

        </div>
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
