<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step7_TECAdvances.aspx.cs" Inherits="TravelAuthorization_TAWizard_Step7_TECAdvances" %>

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
            function ConfirmDeleteAdv() {
                return confirm("Are you sure you want to delete?");
            }
            function SaveValidationAdv() {
                var a = 0;
                $(".ReqAdv").each(function () {

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
        <asp:UpdatePanel ID="UpdatePanelStep7" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">Step 8 - TEC Advances</h4>
                            </div>
                            <div class="panel-body">
                                <asp:Panel ID="pnlContent" runat="server">
                                    <div class="row" style="padding-top: 10px">
                                    <div class="col-lg-12">
                                        <asp:Panel ID="PanelAdvMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                            <asp:Label ID="lblAdvMsg" runat="server" Text=""></asp:Label>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <table>
                                    <tr>
                                        <td class="tdField" colspan="5">
                                            <asp:CheckBox ID="checkNotApplicable" AutoPostBack="true" CssClass="checkbox" OnCheckedChanged="checkNotApplicable_CheckedChanged" Text="No Advances Applicable" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdField" id="divAdv" runat="server">
                                            <asp:DropDownList ID="ddlAdvPayOffice" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control ReqAdv" Width="250px" />
                                        </td>
                                        <td class="tdField">
                                            <asp:TextBox ID="txtAdvDate" CssClass="form-control ReqAdv" runat="server" placeholder="Date Paid: DD-MMM-YYYY" Width="250px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtAdvDate" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="tdField">
                                            <asp:DropDownList ID="DDLAdvCurrency" CssClass="form-control ReqAdv" runat="server" DataValueField="CurrencyID" DataTextField="CurrencyName" Width="250px"></asp:DropDownList>
                                        </td>
                                        <td class="tdField">
                                            <asp:TextBox ID="txtAdvAmount" CssClass="form-control ReqAdv" runat="server" Width="250px" placeholder="Amount"></asp:TextBox>
                                        </td>
                                        <td class="tdField">
                                            <asp:LinkButton ID="ibAdd" ToolTip="Click to save" OnClick="ibAdd_Click" OnClientClick="return SaveValidationAdv();" runat="server"><li class="fa fa-plus"></li> Click to save</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="tdField" colspan="5">
                                            <asp:HiddenField ID="hdnStatusCode" runat="server" />
                                            <asp:GridView ID="GVAdvances" CssClass="table table-bordered  table-hover" runat="server" AutoGenerateColumns="False" OnRowCreated="GVAdvances_RowCreated" ShowFooter="True" OnRowDataBound="GVAdvances_RowDataBound" OnRowDeleting="GVAdvances_RowDeleting" DataKeyNames="TECAdvancesID" OnRowCommand="GVAdvances_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="PayOfficeCodeDesc" HeaderText="Paying Office (Location Code)">
                                                        <FooterStyle Font-Bold="True" Wrap="false" />
                                                        <HeaderStyle Wrap="false" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DatePaid" HeaderText="Date Paid">
                                                        <HeaderStyle Wrap="false" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CurrencyName" HeaderText="Currency">
                                                        <HeaderStyle Wrap="false" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AdvanceAmount" HeaderText="Amount">
                                                        <HeaderStyle Wrap="false" />
                                                        <ItemStyle Wrap="false" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle BackColor="#eeeeee" BorderColor="#eeeeee" Width="1px" CssClass="Left" />
                                                        <ItemStyle BackColor="#eeeeee" BorderColor="#eeeeee" Width="1px" CssClass="Left" />
                                                        <FooterStyle BackColor="#eeeeee" BorderColor="#eeeeee" Width="1px" CssClass="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rate USD">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRate" CssClass="form-control" runat="server" Text='<%# decimal.Round(Convert.ToDecimal(Eval("Rate")), 2, MidpointRounding.AwayFromZero).ToString() %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            TOTAL SECTION 3:
                                                        </FooterTemplate>
                                                        <FooterStyle Font-Bold="true" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount USD">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRateAmount" CssClass="form-control" runat="server" Text='<%# decimal.Round(Convert.ToDecimal(Eval("RateAmount")), 2, MidpointRounding.AwayFromZero).ToString() %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblRateAmountTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Local Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAmountLocal" CssClass="form-control" runat="server" Text='<%# decimal.Round(Convert.ToDecimal(Eval("LocalAmount")), 2, MidpointRounding.AwayFromZero).ToString() %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblAmountLocalTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="ibDelete" runat="server" Width="120px" CommandName="Delete" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDeleteAdv();"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:HiddenField ID="hfAdvRateAmountTotal" runat="server" />
                                            <asp:HiddenField ID="hfAdvLocalAmountTotal" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                        <td class="tdField" colspan="2">
                                            <div style="float: right">
                                                <asp:Button ID="btnSaveAdvances" CssClass="btn btn-primary form-control" runat="server" Text="Submit & continue" OnClick="btnSaveAdvances_Click"  Width="280px" Font-Size="14px"/>
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
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep7" DisplayAfter="100" DynamicLayout="False">
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
