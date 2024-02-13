<%@ Page Language="C#" AutoEventWireup="true" CodeFile="1_PersonalInformation.aspx.cs" Inherits="Staff_SMWizard_1_PersonalInformation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Staff/SMWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>
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
                        <h4 class="panel-title">Step 1 - Personal Information</h4>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlContent" runat="server">
                            <div class="row" style="width: 99.9%;">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <table style="width:80%">

                                        <tr>
                                                        <td class="tdLabel">User Name</td>
                                                        <td class="tdField" colspan="6">
                                                            <asp:HiddenField ID="hfUserName" runat="server" />
                                                            <asp:TextBox ID="txtUserName" CssClass="form-control Req" OnTextChanged="txtUserName_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="txtUserIDAutoCompleteExtender" runat="server"
                                                                BehaviorID="txtToLocationCodeAutoComplete" CompletionInterval="500" CompletionListCssClass="autocomplete_completionListElement_New"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem_New"
                                                                CompletionListItemCssClass="autocomplete_listItem_New" CompletionSetCount="50"
                                                                DelimiterCharacters="" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="2"
                                                                ServiceMethod="GetItemsListUsers" ServicePath="~/Services/AutoComplete.asmx"
                                                                ShowOnlyCurrentWordInCompletionListItem="false" TargetControlID="txtUserName" UseContextKey="false">
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
                                                    </tr>

                                        <tr>
                                            <td class="tdLabel">Staff Name</td>
                                            <td class="tdField" colspan="6">
                                                <asp:TextBox ID="txtDisplayName" placeholder="Staff Name" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <%--<tr>
                                            <td class="tdLabel">Last Name</td>
                                            <td class="tdField" colspan="6">
                                                <asp:TextBox ID="txtLastName" placeholder="Last Name" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>--%>


                                        <tr>
                                            <td class="tdLabel">PRISM Number</td>
                                            <td class="tdField" colspan="6">
                                                <asp:TextBox ID="txtPRISMNumber" placeholder="PRISM Number" runat="server" CssClass="form-control Req"></asp:TextBox>
                                                <asp:HiddenField ID="hdnDepartment" runat="server" />
                                                <asp:HiddenField ID="hdnUnit" runat="server" />
                                                <asp:HiddenField ID="hdnSubUnit" runat="server" />
                                            </td>
                                        </tr>

                                         <tr>
                                            <td class="tdLabel">Gender<span style="color: red">*</span></td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlGender" AutoPostBack="true" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control" />
                                            </td>
                                            </tr>

                                        <tr>
                                            <td class="tdLabel">Call Sign</td>
                                            <td class="tdField" colspan="6">
                                                <asp:TextBox ID="txtCallSign" placeholder="Enter call sign" runat="server" CssClass="form-control Req"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="tdLabel">Country of Nationality<span style="color: red">*</span></td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlCountry" AutoPostBack="true" runat="server" DataTextField="CountryName" DataValueField="CountryID" CssClass="form-control" />
                                            </td>
                                            </tr>

                                          <tr>
                                            <td class="tdLabel">Duty Station<span style="color: red">*</span></td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlLocation" AutoPostBack="true" runat="server" DataTextField="LocationName" DataValueField="LocationID" CssClass="form-control" />
                                            </td>
                                            </tr>

                                         <tr>
                                            <td class="tdLabel">Department</td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlDepartment" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" runat="server" DataTextField="DepartmentName" DataValueField="DepartmentID" CssClass="form-control" />
                                            </td>
                                        </tr>

                                     

                                         <tr>
                                            <td class="tdLabel">Unit</td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlUnit" AutoPostBack="true" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged" runat="server" DataTextField="UnitName" DataValueField="UnitID" CssClass="form-control" />
                                            </td>
                                        </tr>

                                         <tr>
                                            <td class="tdLabel">Sub Unit</td>
                                            <td class="tdField" colspan="6">
                                                <asp:DropDownList ID="ddlSubUnit" AutoPostBack="true" runat="server" DataTextField="SubUnitName" DataValueField="SubUnitID" CssClass="form-control" />
                                            </td>
                                        </tr>

                                        
                                        <tr>
                                            <td colspan="5"></td>
                                            <td>
                                                <asp:Button ID="BtnSave" runat="server" OnClick="BtnSave_Click" OnClientClick="return SaveValidation();" CssClass="form-control btn-primary" Font-Size="14px" Text="Save & Continue" Width="280px" />
                                            </td>
                                        </tr>
                                    </table>
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




