<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step3_WBS.aspx.cs" Inherits="TravelAuthorization_TAWizard_Step3_WBS" %>
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
            function ConfirmDeleteWBS() {
                return confirm("Are you sure you want to delete?");
            }
            function SaveValidationG2() {
                var a = 0;
                $(".ReqG2").each(function () {
                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
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
        <asp:UpdatePanel ID="UpdatePanelStep3" runat="server">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">Step 3 - WBS</h4>
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="pnlContent" runat="server">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <table style="width:70%">
                                        <tr>
                                            <td class="tdField">
                                                <asp:RadioButton ID="rdPercentage" runat="server" CssClass="radio-inline" Text="Percentage" Checked="true" GroupName="Per" />
                                                <asp:RadioButton ID="rdAmount" runat="server" CssClass="radio-inline" Text="Amount" GroupName="Per" />
                                            </td>
                                            <td class="tdField">
                                                <asp:TextBox ID="txtPercentageOrAmount" runat="server" CssClass="form-control ReqG2" placeholder="Percentage / Amount"></asp:TextBox>

                                            </td>
                                            <td class="tdField" style="width:25%">
                                                <%--<asp:TextBox ID="txtWBSCode" runat="server" CssClass="form-control ReqG2 uppercase"  placeholder="WBS Code: AA.NNNN.AANN.NN.NN.NNN"></asp:TextBox>--%>

                                    <asp:TextBox ID="txtWBSCode" CssClass="form-control ReqG2 uppercase" OnTextChanged="WBS_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="txtWBSCodeAutoCompleteExtender" runat="server"
                                        BehaviorID="txtWBSCodeAutoComplete" CompletionInterval="500" CompletionListCssClass="autocomplete_completionListElement_New"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem_New"
                                        CompletionListItemCssClass="autocomplete_listItem_New" CompletionSetCount="50"
                                        DelimiterCharacters="" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="2"
                                        ServiceMethod="GetItemListWBS" ServicePath="~/Services/AutoComplete.asmx"
                                        ShowOnlyCurrentWordInCompletionListItem="false" TargetControlID="txtWBSCode" UseContextKey="false">
                                        <animations>
                                                                <OnShow>
                                                                    <Sequence>
                                                                        <OpacityAction Opacity="0" />
                                                                        <HideAction Visible="true" />
                                                                        <ScriptAction Script="
                                                                            var behavior = $find('txtWBSCodeAutoComplete');
                                                                            if (!behavior._height) {
                                                                                var target = behavior.get_completionList();
                                                                                behavior._height = target.offsetHeight - 2;
                                                                                target.style.height = '0px';
                                                                            }" />
                                                                        <Parallel Duration=".4">
                                                                            <FadeIn />
                                                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('txtWBSCodeAutoComplete')._height" />
                                                                        </Parallel>
                                                                    </Sequence>
                                                                </OnShow>
                                                                <OnHide>
                                                                    <Parallel Duration=".4">
                                                                        <FadeOut />
                                                                        <Length PropertyKey="height" StartValueScript="$find('txtWBSCodeAutoComplete')._height" EndValue="0" />
                                                                    </Parallel>
                                                                </OnHide>
                                                                </animations>
                                    </asp:AutoCompleteExtender>

                                            </td>


                                            <td class="tdField">
                                                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control ReqG2"  placeholder="Notes"></asp:TextBox>
                                            </td>
                                            <td class="tdLabel">
                                                <asp:LinkButton ID="ibAdd" ToolTip="Click to save" ValidationGroup="NumOnly" OnClick="btnSave_Click" OnClientClick="return SaveValidationG2();" runat="server"><li class="fa fa-plus"></li> Click to save</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtPercentageOrAmount" ValidationExpression="\d+" Display="Static" EnableClientScript="true" ForeColor="Red" ErrorMessage="Please enter numbers only" ValidationGroup="NumOnly" runat="server" />
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:GridView ID="gvWBS" Width="100%" CssClass="table table-bordered  table-hover " AllowPaging="false" AutoGenerateColumns="False" DataKeyNames="WBSID" OnRowCommand="gvWBS_RowCommand" OnRowDataBound="gvWBS_RowDataBound" OnRowDeleting="gvWBS_RowDeleting" runat="server">
                                                    <Columns>
                                                        <asp:BoundField DataField="WBSCode" HeaderText="WBS Code" />
                                                        <asp:BoundField DataField="PercentageOrAmount" HeaderText="Percentage Or Amount" />
                                                        <asp:BoundField DataField="Note" HeaderText="Note" />
                                                        <asp:TemplateField ShowHeader="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibEditWBS" CommandName="EditWBS" ToolTip="Click to edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="ibDeleteWBS" runat="server"  CommandName="DeleteWBS" ToolTip="Click to delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDeleteWBS();"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel"></td>
                                            <td class="tdLabel" colspan="2">
                                                <div style="float: right">
                                                    <asp:Button ID="btnNext" runat="server" CssClass="form-control btn-primary" OnClick="btnNext_Click" Font-Size="14px" Text="Submit & Continue" Width="280px" />
                                                </div>
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
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep3" DisplayAfter="100" DynamicLayout="False">
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
