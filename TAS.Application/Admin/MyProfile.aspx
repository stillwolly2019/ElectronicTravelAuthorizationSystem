<%@ Page Title="Profile Update" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyProfile.aspx.cs" Inherits="Admin_MyProfile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function SaveValidation() {
            var a = 0;
            $(".Req").each(function () {
                if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0" || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
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
        $(document).ready(function () {
            $(".Req").blur(function () {
                var a = 0;
                $(".Req").each(function () {
                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0" || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
                        $(this).addClass("invalid");
                        a = a + 1;
                    }
                    else {
                        $(this).removeClass("invalid");
                    }
                });
                if (a > 0) {
                    return false;
                }
                else {
                    return true;
                }
            });
        });
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title">Travel Authorization System | Personal Information</h4>
                </div>

                <div class="panel-body">
                    <asp:Panel ID="pnlContent" runat="server">
                        <div class="row" style="width: 99.9%; text-align: left;">
                            <div class="col-lg-12">
                                <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                </asp:Panel>

                                <div class="col-lg-2" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>User Name</b></label>
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>

                                <div class="col-lg-10" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Display Name</b></label>
                                    <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>

                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Duty Station</b></label>
                                    <asp:TextBox ID="txtDutyStation" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>


                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Unit or Department</b></label>
                                    <asp:TextBox ID="txtUnitDepartment" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>

                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Gender</b></label>
                                    <asp:DropDownList ID="ddlGender" AutoPostBack="true" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control" />
                                </div>

                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Title</b></label>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>

                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>PRISM</b></label>
                                    <asp:TextBox ID="txtPRISMNumber" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>

                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>UN ID Number</b></label>
                                    <asp:TextBox ID="txtUNIDNo" placeholder="E.g. JBA-IOM-LS-XXXX" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>

                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Area of Residence</b></label>
                                    <asp:HiddenField ID="hdnResidenceID" runat="server" />
                                    <asp:TextBox ID="txtResidenceName" CssClass="form-control Req" OnTextChanged="txtResidenceName_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="txtResidenceIDAutoCompleteExtender" runat="server"
                                        BehaviorID="txtResidenceIDAutoComplete" CompletionInterval="500" CompletionListCssClass="autocomplete_completionListElement_New"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem_New"
                                        CompletionListItemCssClass="autocomplete_listItem_New" CompletionSetCount="50"
                                        DelimiterCharacters="" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="2"
                                        ServiceMethod="GetItemsListResidences" ServicePath="~/Services/AutoComplete.asmx"
                                        ShowOnlyCurrentWordInCompletionListItem="false" TargetControlID="txtResidenceName" UseContextKey="false">
                                        <animations>
                                                                <OnShow>
                                                                    <Sequence>
                                                                        <OpacityAction Opacity="0" />
                                                                        <HideAction Visible="true" />
                                                                        <ScriptAction Script="
                                                                            // Cache the size and setup the initial size
                                                                            var behavior = $find('txtResidenceIDAutoComplete');
                                                                            if (!behavior._height) {
                                                                                var target = behavior.get_completionList();
                                                                                behavior._height = target.offsetHeight - 2;
                                                                                target.style.height = '0px';
                                                                            }" />
                                                                        <Parallel Duration=".4">
                                                                            <FadeIn />
                                                                            <Length PropertyKey="height" StartValue="0" EndValueScript="$find('txtResidenceIDAutoComplete')._height" />
                                                                        </Parallel>
                                                                    </Sequence>
                                                                </OnShow>
                                                                <OnHide>
                                                                    <Parallel Duration=".4">
                                                                        <FadeOut />
                                                                        <Length PropertyKey="height" StartValueScript="$find('txtResidenceIDAutoComplete')._height" EndValue="0" />
                                                                    </Parallel>
                                                                </OnHide>
                                                                </animations>
                                    </asp:AutoCompleteExtender>


                                </div>


                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Nationality</b></label>
                                    <asp:DropDownList ID="ddlCountry" AutoPostBack="true" runat="server" DataTextField="CountryName" DataValueField="CountryID" CssClass="form-control" />
                                </div>

                                <div class="col-lg-12" style="padding-top: 8px; padding-bottom: 8px;">
                                    <label><b>Call Sign</b></label>
                                    <asp:TextBox ID="txtCallSign" runat="server" CssClass="form-control col-lg-12"></asp:TextBox>
                                </div>


                                <div class="col-lg-10" style="padding-top: 8px; padding-bottom: 8px;">
                                </div>

                                <div class="col-lg-2" style="padding-top: 8px; padding-bottom: 8px; text-align: right;">
                                    <asp:LinkButton ID="BtnSave" CssClass="btn btn-primary" runat="server" Font-Size="12px" OnClick="BtnSave_Click" OnClientClick="return SaveValidation();"><li class="fa fa-save"></li>&nbsp;Save Changes</asp:LinkButton>
                                </div>


                            </div>
                        </div>
                    </asp:Panel>
                </div>

            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

