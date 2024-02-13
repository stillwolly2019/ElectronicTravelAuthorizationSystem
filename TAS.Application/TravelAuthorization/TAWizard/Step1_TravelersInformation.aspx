<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step1_TravelersInformation - Copy.aspx.cs" Inherits="TravelAuthorization_TAWizard_Step1_TravelersInformation" %>

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
                    <div class="panel-heading">
                        <h4 class="panel-title">Step 1 - Travelers Information</h4>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlContent" runat="server">
                            <div class="row" style="width: 99.9%;">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <table>

                                        <tr id="rowcheckIsEmergency">
                                            <td></td>
                                            <td class="tdField" colspan="3"></td>
                                        </tr>



                                        <tr id="userLists" visible="false" runat="server">
                                            <td class="tdLabel">Employee <span style="color: red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:DropDownList ID="DDLUsers" Width="280px" runat="server" OnSelectedIndexChanged="DDLUsers_SelectedIndexChanged" AutoPostBack="true" DataTextField="FullName" DataValueField="UserID" CssClass="form-control Req">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnisemergency" Value='' runat="server" />
                                                <asp:HiddenField ID="hdnstaffcategory" Value='' runat="server" />
                                            </td>

                                            <td class="tdField" colspan="2">
                                                <asp:CheckBox ID="checkIsEmergency" OnCheckedChanged="checkIsEmergency_CheckedChanged" AutoPostBack="true" CssClass="checkbox" Text="Is Emergency" Visible="false" runat="server" />

                                                <asp:CheckBox ID="checkIsNotify" OnCheckedChanged="checkIsEmergency_CheckedChanged" Checked="true" AutoPostBack="true" CssClass="checkbox" Text="Is Emergency" Visible="false" runat="server" />


                                            </td>

                                        </tr>

                                        <tr>
                                            <td class="tdLabel">PRISM Number</td>
                                            <td class="tdField" colspan="6">
                                                <asp:TextBox ID="txtPRISM" placeholder="PRISM Number" runat="server" CssClass="form-control" Width="280px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">Travelers Name (Same as passport name) <span style="color: red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtTravelersFirstName" placeholder="First Name" runat="server" Enabled="false" CssClass="form-control Req" Width="280px"></asp:TextBox>
                                            </td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtTravelerSecondName" placeholder="Middle Name" runat="server" CssClass="form-control" Width="290px"></asp:TextBox>
                                            </td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtTravelerLastName" placeholder="Last Name" runat="server" Enabled="false" CssClass="form-control Req" Width="280px"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="tdLabel">Trip Schema Code <span style="color: red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:DropDownList ID="ddlTripSchemaCode" AutoPostBack="true" OnSelectedIndexChanged="ddlTripSchemaCode_SelectedIndexChanged" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control Req" />
                                            </td>


                                        </tr>



                                        <tr id="trLeaveDays" visible="false" runat="server">
                                            <td class="tdLabel">Specify Leave Days <span style="color: red">*</span></td>
                                            <td class="tdField" colspan="3">
                                                <asp:TextBox ID="txtLeaveStartDate" Enabled="true" placeholder="Leave Start Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtLeaveStartDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>

                                            <td class="tdField">
                                                <asp:TextBox ID="txtLeaveEndDate" Enabled="true" placeholder="Leave End Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtLeaveEndDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>
                                            <td></td>
                                        </tr>

                                        <tr id="trRandRDays" visible="false" runat="server">
                                            <td class="tdLabel">Specify R&R Days <span style="color: red">*</span></td>
                                            <td class="tdField" colspan="3">
                                                <asp:TextBox ID="txtRRStartDate" Enabled="true" placeholder="R&R Start Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtRRStartDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>

                                            <td class="tdField">
                                                <asp:TextBox ID="txtRREndDate" Enabled="true" placeholder="R&R End Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtRREndDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>
                                            <td></td>
                                        </tr>


                                        <tr id="trFamilyMembers" visible="false" runat="server">
                                            <td class="tdLabel">Family Members (Full name comma seperated)</td>
                                            <td class="tdField" colspan="2">
                                                <asp:TextBox ID="txtFamilyMembers" placeholder="Family Members (Full name comma seperated)" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">Mode Of Travel Code <span style="color: red">*</span></td>
                                            <td class="tdField" colspan="2">
                                                <asp:DropDownList ID="ddlModeOfTravelCode" runat="server" DataTextField="Description" AutoPostBack="true" OnSelectedIndexChanged="ddlModeOfTravelCode_SelectedIndexChanged" DataValueField="LookupsID" CssClass="form-control Req" />
                                            </td>

                                        </tr>









                                        <tr>








                                            <tr>


                                                <td class="tdLabel">City(ies) of accommodation</td>
                                                <td class="tdField" colspan="6">
                                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="Please specify"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">Purpose Of Travel <span style="color: red">*</span></td>
                                                <td class="tdField" colspan="6">
                                                    <asp:TextBox ID="txtPurposeOfTravel" placeholder="Briefly describe your duties" runat="server" CssClass="form-control Req"></asp:TextBox>
                                                </td>
                                            </tr>


                                            
                                            <tr>
                                                <td class="tdLabel" style="font-weight: normal">Private Stay/Annual leave (or other leave) provided</td>
                                                <td class="tdField">
                                                    <asp:RadioButton ID="rdYAnnual" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Annual" OnCheckedChanged="rdYAnnual_CheckedChanged" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rdNoAnnual" runat="server" Text="No" CssClass="radio-inline" GroupName="Annual" OnCheckedChanged="rdNoAnnual_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="tdField" colspan="3">
                                                    <asp:TextBox ID="txtPrivateStayDateFrom" Enabled="false" placeholder="Date From" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtPrivateStayDateFrom" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                </td>

                                                <td class="tdField">
                                                    <asp:TextBox ID="txtPrivateStayDateTo" Enabled="false" placeholder="Date To" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtPrivateStayDateTo" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="font-weight: normal">Private Deviation</td>
                                                <td class="tdField">
                                                    <asp:RadioButton ID="rdYesDiv" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Div" OnCheckedChanged="rdYesDiv_CheckedChanged" AutoPostBack="true" />
                                                    <asp:RadioButton ID="rdNoDiv" runat="server" Text="No" CssClass="radio-inline" GroupName="Div" OnCheckedChanged="rdNoDiv_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="tdField" colspan="5">
                                                    <asp:TextBox ID="txtDiv" placeholder="Specify Leg(s)" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel" style="font-weight: normal">Accommodation (board and/or lodging) provided</td>
                                                <td class="tdField">
                                                    <asp:RadioButton ID="rdYesAcc" runat="server" Text="Yes" OnCheckedChanged="rdYesAcc_CheckedChanged" AutoPostBack="true" CssClass="radio-inline" GroupName="Acc" />
                                                    <asp:RadioButton ID="rdNoAcc" runat="server" Text="No" OnCheckedChanged="rdNoAcc_CheckedChanged" AutoPostBack="true" CssClass="radio-inline" GroupName="Acc" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="tdField" colspan="2">
                                                    <asp:TextBox ID="txtAcc" Enabled="false" CssClass="form-control" ToolTip="Accomodation Details" placeholder="Please Specify the Hotel Name" runat="server" />
                                                    <asp:AutoCompleteExtender ID="txtFromLocationCodeAutoCompleteExtender" runat="server"
                                                        BehaviorID="txtFromLocationCodeAutoComplete" CompletionInterval="500" CompletionListCssClass="autocomplete_completionListElement_New"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem_New"
                                                        CompletionListItemCssClass="autocomplete_listItem_New" CompletionSetCount="50"
                                                        DelimiterCharacters="" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1"
                                                        ServiceMethod="GetItemsListAccomodations" ServicePath="~/Services/AutoComplete.asmx"
                                                        ShowOnlyCurrentWordInCompletionListItem="false" TargetControlID="txtAcc" UseContextKey="false">
                                                        <Animations>
<OnShow>
<Sequence>
<OpacityAction Opacity="0" />
<HideAction Visible="true" />
<ScriptAction Script="
// Cache the size and setup the initial size
var behavior = $find('txtFromLocationCodeAutoComplete');
if (!behavior._height) {
var target = behavior.get_completionList();
behavior._height = target.offsetHeight - 2;
target.style.height = '0px';
}" />
<Parallel Duration=".4">
<FadeIn />
<Length PropertyKey="height" StartValue="0" EndValueScript="$find('txtFromLocationCodeAutoComplete')._height" />
</Parallel>
</Sequence>
</OnShow>
<OnHide>
<Parallel Duration=".4">
<FadeOut />
<Length PropertyKey="height" StartValueScript="$find('txtFromLocationCodeAutoComplete')._height" EndValue="0" />
</Parallel>
</OnHide>
                                                        </Animations>
                                                    </asp:AutoCompleteExtender>
                                                </td>

                                                <td class="tdField" colspan="3">
                                                    <asp:TextBox ID="txtCheckinDate" runat="server" Enabled="false" CssClass="form-control ReqG1" placeholder="CheckIn Date"></asp:TextBox>
                                                    <asp:CalendarExtender ID="Calendar1" runat="server" TargetControlID="txtCheckinDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                </td>


                                                <td class="tdField">
                                                    <asp:TextBox ID="txtCheckoutDate" runat="server" Enabled="false" CssClass="form-control ReqG1" placeholder="CheckOutDate"></asp:TextBox>
                                                    <asp:CalendarExtender ID="Calendar2" runat="server" TargetControlID="txtCheckoutDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                                </td>

                                                <td class="tdField" colspan="3">
                                                    <asp:TextBox ID="txtNoNights" placeholder="No of Nights" runat="server" Enabled="false" CssClass="form-control" MaxLength="2" Width="100px"></asp:TextBox>

                                                </td>


                                            </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">Movement and Flight Request</h4>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="Panel2" runat="server">
                        <table style="width:100%">
                            <tr>
                                <td  style="width:50%" class="tdField col-lg-6">
                                    <b>Carrier:</b>
                                    <asp:DropDownList ID="ddlCarrierCode" AutoPostBack="true" OnSelectedIndexChanged="ddlCarrierCode_SelectedIndexChanged" runat="server" Enabled="false" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control" />
                                </td>

                                <td style="width:50%" class="tdField col-lg-6">
                                    <b>Flight/Reference No:</b>
                                    <asp:TextBox ID="txtFlightRefNumber" placeholder="Flight Ref No(KQ350)" runat="server" Enabled="false" CssClass="form-control col-lg-3"></asp:TextBox>
                                </td>
                            </tr>

                            <tr id="trCargo" visible="false" runat="server" style="padding-top:10px;padding-bottom:10px;width:100%">
                                <td class="tdField col-lg-12" colspan="4">
                                <b>Extra Cargo:</b>
                                  <asp:DropDownList ID="ddlExtraCargoCode" ToolTip="Extra luggage for domestic flights" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control" />
                                </td>
                            </tr>

                       <%--     <tr>
                                <td  style="width:25%" class="tdField col-lg-3">
                                    <b>Travelling From:</b>
                                    <asp:TextBox ID="txtTravellingFrom" placeholder="E.g Juba" runat="server" CssClass="form-control Req col-lg-3"></asp:TextBox>
                                </td>

                                <td style="width:25%" class="tdField col-lg-3">
                                    <b>Travelling To:</b>
                                    <asp:TextBox ID="txtTravellingTo" placeholder="E.g Wau" runat="server" CssClass="form-control Req col-lg-3"></asp:TextBox>
                                </td>

                                


                            </tr>--%>


                            <tr>

                                <td class="tdField col-lg-6" style="width:50%;">
                                                <b>ETD / ETA</b><asp:DropDownList ID="ddlETDETACode" ToolTip="Departure time for outbound flights and Arrival time for inbound flights" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control" />
                                            </td>
                                            <td class="tdField col-lg-6" style="width:50%;">
                                                <b>ETD / ETA Pick-up:</b><asp:DropDownList ID="ddlETDETAPickup" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control" />
                                            </td>
                            </tr>



                            

                                        <tr style="padding-top:10px;padding-bottom:10px;">
                                            <td class="tdField col-lg-6" style="width:50%;">
                                                <b>Pick-up / Drop-off Location:</b><asp:DropDownList ID="ddlPickupLocation" ToolTip="Pick-up / Drop-off Location: e.g.(Hotel, NGH, OGH, office, GGH)"
                                                    runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control" />
                                            </td>
                                            <td class="tdField col-lg-6" style="width:50%;">
                                                <b>Booking/Travel Status:</b><asp:DropDownList ID="ddlBookingStatus" runat="server" DataTextField="Description" DataValueField="LookupsID" ToolTip="(Use Inbound and Outbound for International flights)" CssClass="form-control" />
                                            </td>
                                        </tr>

                                        <tr style="padding-top:10px;padding-bottom:10px;height:auto;">
                                            <td class="tdField col-lg-12" style="width:100%;" colspan="2">
                                                <b>Remarks:</b><asp:TextBox ID="txtRemarksOfTravel" TextMode="MultiLine" placeholder="Reasons for  late booking " runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr style="height:10px;">
                                            <td></td>
                                        </tr>

                        </table>
                            <table>

                                <tr style="padding-top:10px;padding-bottom:10px;">
                                            <td class="tdField col-lg-6" style="width:70%;">
                                                </td>
                                            <td class="tdField col-lg-6" style="float: right;width:30%;">
                                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return SaveValidation();" CssClass="form-control btn-primary" Font-Size="14px" Text="Submit & Continue" Width="280px" />
                                            </td>
                                        </tr>

                        </table>
                        </asp:Panel>
                    </div>
                </div>


            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep1" DisplayAfter="100" DynamicLayout="False">
            <ProgressTemplate>
                <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                    <div style="padding-top: 200px;">
                        <img alt="Loading" src="../../images/ajax_loader_blue_512.gif" width="200" />
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
