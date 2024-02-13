<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step4_Itinerary.aspx.cs" Inherits="TravelAuthorization_TAWizard_Step4_Itinerary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/TravelAuthorization/TAWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .uppercase {
            text-transform: uppercase;
        }

        .TheDisp {
            display: block;
        }

        .autocomplete_completionListElement_New {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            font-size: 14px;
            text-align: left;
            list-style-type: none;
            margin-left: 0px;
            padding-left: 0px;
            max-height: 150px;
            width: auto;
        }

            .autocomplete_completionListElement_New: li {
                margin: 0px 0px 0px -20px !important;
            }

        .autocomplete_highlightedListItem_New {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }

        .autocomplete_listItem_New {
            background-color: window;
            color: windowtext;
            padding: 1px;
        }

        .FormatRadioButtonList label {
            margin-right: 15px;
        }
    </style>
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
            function ConfirmDelete() {
                return confirm("Are you sure you want to delete?");
            }
            function SaveValidationG1() {
                var a = 0;
                $(".ReqG1").each(function () {

                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-- Mode Of Travel --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
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
        <asp:UpdatePanel ID="UpdatePanelStep4" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">Step 4 - Travel Itinerary</h4>
                            </div>
                            <div class="panel-body">
                                <asp:Panel ID="pnlContent" runat="server">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                                <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <table>
                                        <tr>
                                            <td class="tdField">
                                                <asp:DropDownList ID="ddlModeOfTravel" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control ReqG1" Width="220px" />
                                            </td>
                                            <td class="tdField">
                                                <asp:TextBox ID="txtFromLocationCode" CssClass="form-control ReqG1" ToolTip="From Location" placeholder="From Location" runat="server" Width="220px" />
                                                <asp:AutoCompleteExtender ID="txtFromLocationCodeAutoCompleteExtender" runat="server"
                                                    BehaviorID="txtFromLocationCodeAutoComplete" CompletionInterval="500" CompletionListCssClass="autocomplete_completionListElement_New"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem_New"
                                                    CompletionListItemCssClass="autocomplete_listItem_New" CompletionSetCount="50"
                                                    DelimiterCharacters="" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1"
                                                    ServiceMethod="GetItemsList" ServicePath="~/Services/AutoComplete.asmx"
                                                    ShowOnlyCurrentWordInCompletionListItem="false" TargetControlID="txtFromLocationCode" UseContextKey="false">
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
                                            <td class="tdField">
                                                <asp:TextBox ID="txtFromLocationDate" runat="server" CssClass="form-control ReqG1" placeholder="From Date" Width="220px"></asp:TextBox>
                                                <asp:CalendarExtender ID="Calendar1" runat="server" TargetControlID="txtFromLocationDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>
                                            <td class="tdField">
                                                <asp:TextBox ID="txtToLocationCode" CssClass="form-control ReqG1" ToolTip="To Location" placeholder="To Location" runat="server" Width="220px" />
                                                <asp:AutoCompleteExtender ID="txtToLocationCodeAutoCompleteExtender" runat="server"
                                                    BehaviorID="txtToLocationCodeAutoComplete" CompletionInterval="500" CompletionListCssClass="autocomplete_completionListElement_New"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem_New"
                                                    CompletionListItemCssClass="autocomplete_listItem_New" CompletionSetCount="50"
                                                    DelimiterCharacters="" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1"
                                                    ServiceMethod="GetItemsList" ServicePath="~/Services/AutoComplete.asmx"
                                                    ShowOnlyCurrentWordInCompletionListItem="false" TargetControlID="txtToLocationCode" UseContextKey="false">
                                                    <Animations>
<OnShow>
<Sequence>
<OpacityAction Opacity="0" />
<HideAction Visible="true" />
<ScriptAction Script="
// Cache the size and setup the initial size
var behavior = $find('txtToLocationCodeAutoComplete');
if (!behavior._height) {
var target = behavior.get_completionList();
behavior._height = target.offsetHeight - 2;
target.style.height = '0px';
}" />
<Parallel Duration=".4">
<FadeIn />
<Length PropertyKey="height" StartValue="0" EndValueScript="$find('txtToLocationCodeAutoComplete')._height" />
</Parallel>
</Sequence>
</OnShow>
<OnHide>
<Parallel Duration=".4">
<FadeOut />
<Length PropertyKey="height" StartValueScript="$find('txtToLocationCodeAutoComplete')._height" EndValue="0" />
</Parallel>
</OnHide>
                                                    </Animations>
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td class="tdField">
                                                <asp:TextBox ID="txtToLocationDate" runat="server" CssClass="form-control ReqG1" placeholder="To Date" Width="220px"></asp:TextBox>
                                                <asp:CalendarExtender ID="Calendar2" runat="server" TargetControlID="txtToLocationDate" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>
                                            <td class="tdLabel">
                                                <asp:LinkButton ID="ibAdd" ToolTip="Click to save" OnClick="ibAdd_Click" OnClientClick="return SaveValidationG1();" runat="server"><li class="fa fa-plus"></li> Click to save</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:GridView ID="gvTravelItinerary" AllowPaging="false" ShowHeaderWhenEmpty="false" CssClass="table table-bordered  table-hover" AutoGenerateColumns="False"
                                                    DataKeyNames="TravelItineraryID,ModeOfTravelID,FromLocationCodeID,FromLocationCodeName,FromLocationDate,ToLocationCodeID,ToLocationCodeName,ToLocationDate,ordering" OnRowCommand="gvTravelItinerary_RowCommand" OnRowDataBound="gvTravelItinerary_RowDataBound" OnRowDeleting="gvTravelItinerary_RowDeleting" runat="server">
                                                    <Columns>
                                                        <asp:BoundField DataField="ModeOfTravelName" HeaderText="Mode Of Travel" />
                                                        <asp:BoundField DataField="FromLocationCodeName" HeaderText="From Location" />
                                                        <asp:BoundField DataField="FromLocationDate" HeaderText="From Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                        <asp:BoundField DataField="ToLocationCodeName" HeaderText="To Location" />
                                                        <asp:BoundField DataField="ToLocationDate" HeaderText="To Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                        <asp:TemplateField ShowHeader="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibEdit" ToolTip="Click to edit" CommandName="EditItinerary" CommandArgument='<%# Container.DisplayIndex %>' runat="server"><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibDelete" runat="server" Width="120px" CommandName="DeleteItem" ToolTip="Click to delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <br /><br />

                                     <td class="tdLabel" colspan="2">
                                                <div style="float: right">
                                                    <asp:Button ID="btnNext" runat="server" CssClass="form-control btn-primary" OnClick="btnNext_Click" Font-Size="14px" Text="Submit & Continue" Width="280px" />
                                                </div>
                                            </td>


                                 






                                </asp:Panel>
                            </div>
                        </div>
                    </div>




                </div>



                <div class="row" style="bottom:100px">
                    <div class="col-lg-12">

                <div class="panel panel-default" id="SecurityClearence" runat="server" style="min-height:500px">
                    <div class="panel-heading">
                        <h4 class="panel-title"> Uploaded Security Clearence</h4>
                    </div>
                    <div class="panel-body" style="bottom:400px">
                        <div class="row">
                            <div class="col-lg-12">
                                <iframe id="IFramePDF" scrolling="no" style="border: none; width: 100%; min-height: 5000px; background-color: #fff" frameborder="0" toolbar="0" runat="server"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
                    </div>
                    </div>

            </ContentTemplate>

        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep4" DisplayAfter="100" DynamicLayout="False">
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
