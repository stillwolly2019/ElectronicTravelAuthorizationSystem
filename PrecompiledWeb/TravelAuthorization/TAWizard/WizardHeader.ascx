<%@ control language="C#" autoeventwireup="true" inherits="TravelAuthorization_TAWizard_WizardHeader, App_Web_rwud3phq" %>

<script type="text/javascript">
    function RedirectFinance() {
        window.parent.location.href = '../SearchTravelAuthorization.aspx';
    }
</script>

<asp:UpdatePanel ID="UpdatePanelTA" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-lg-12">
                <h4 class="page-header">
                    <asp:Label ID="lblTitle" Text="Travel Authorization" runat="server"></asp:Label>
                </h4>
                <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </div>
        </div>
        <div class="row" id="TAStatusDiv" runat="server">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <table>
                            <tr>
                                <td class="tdLabel" style="font-size: 14px">Travel Authorization Status</td>
                                <td class="tdField" id="divStatus" runat="server">
                                    <asp:DropDownList ID="ddlStatusCode" runat="server" AutoPostBack="true" CssClass="form-control" DataTextField="Description" DataValueField="Code" Enabled="true" OnSelectedIndexChanged="ddlStatusCode_SelectedIndexChanged" Width="280px" />
                                </td>
                                <td class="tdField">
                                    <asp:Button ID="btnSaveStatus" OnClick="btnSaveStatus_Click" runat="server" CssClass="form-control btn-primary" Text="Save" Font-Size="14px" Width="280px" />
                                    <asp:HiddenField ID="hdnUserID" runat="server" />
                                </td>
                                <td class="tdField">
                                    <asp:Button ID="btnsearchStatus" runat="server" CssClass="form-control btn-primary" data-target="#DisStatusHistory" data-toggle="modal" Text="View History" Font-Size="14px" Width="280px" />
                                    <asp:HiddenField ID="hfLatestStatusID" runat="server" Value="" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top">
                                    <asp:Label ID="lblRejectionReason" runat="server" Font-Bold="false" Visible="false" Text="Rejection Reasons"></asp:Label>
                                </td>
                                <td class="tdField" colspan="3">
                                    <div id="RejectionReasonDiv" runat="server" class="col-lg-12" style="visibility: hidden; overflow-y: scroll; border: 1px solid #cdcdcd; margin-top: 5px;">
                                        <div style="margin-left: 8px;">
                                            <asp:CheckBoxList ID="lstRejectionReason" runat="server" CssClass="checkbox" Height="130px" SelectionMode="Multiple " Visible="false">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trTADocument" visible="false" runat="server">
                                <td class="tdLabel">
                                    <asp:Label ID="lblDocNo" runat="server" Font-Bold="false" Text="Document Number"></asp:Label>
                                </td>
                                <td class="tdField" colspan="3">
                                    <asp:TextBox ID="txtDocNo" AutoPostBack="true" OnTextChanged="txtDocNo_TextChanged" CssClass="form-control" Width="280px" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <table style="width: 100%">
                    <tr>
                        <td id="tdStep1" runat="server"  style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep1" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="1" ToolTip="Travelers Information" OnClick="lbStep1_Click"></asp:LinkButton></td>
                        <td id="tdStep2" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep2" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="2" ToolTip="Advance request and security requirements" OnClick="lbStep2_Click"></asp:LinkButton></td>
                        <td id="tdStep3" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep3" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="3" ToolTip="WBS" OnClick="lbStep3_Click"></asp:LinkButton></td>
                        <td id="tdStep4" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep4" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="4" ToolTip="Travel Itinerary" OnClick="lbStep4_Click"></asp:LinkButton></td>
                        <td id="tdStep5" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStepDownload" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="5" ToolTip="Download TA" OnClick="lbStepDownload_Click"></asp:LinkButton></td>
                        <td style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep5" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="6" ToolTip="TEC Itinerary" OnClick="lbStep5_Click"></asp:LinkButton></td>
                        <td style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep6" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="7" ToolTip="TEC Expenses" OnClick="lbStep6_Click"></asp:LinkButton></td>
                        <td style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep7" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="8" ToolTip="TEC Advances" OnClick="lbStep7_Click"></asp:LinkButton></td>
                        <td style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep8" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="9" ToolTip="Check List" OnClick="lbStep8_Click"></asp:LinkButton></td>
                        <td style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep9" runat="server" CssClass="btn btn-primary btn-circle btn-lg" Text="10" ToolTip="Download TEC" OnClick="lbStep9_Click"></asp:LinkButton></td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <div>
                                <asp:Literal ID="ltrProgress" runat="server"></asp:Literal>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelTA" DisplayAfter="100" DynamicLayout="False">
    <ProgressTemplate>
        <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
            <div style="padding-top: 200px;">
                <img alt="Loading" src="../../images/ajax_loader_blue_512.gif" width="200px" />
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<div class="modal fade" id="DisStatusHistory" tabindex="-1" role="form" aria-labelledby="myStstusHistoryLabel" aria-hidden="true">
    <div class="modal-dialog" style="width: 50%">
        <div class="modal-content" style="width: auto;">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="DisWebsiteModalLabel">TA Status History</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-lg-12">
                        <asp:Panel ID="PanelMessageWebsiteAdd" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                            <asp:Label ID="lblmsgwebsiteAdd" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="table-responsive">
                            <div style="text-align: right; font-weight: bold; padding-right: 20px;" class="alert-success">
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            </div>
                            <asp:GridView ID="gvHistoryStatus" runat="server" CssClass="table table-bordered  table-hover"
                                AutoGenerateColumns="False" AllowPaging="false"
                                OnRowCommand="gvHistoryStatus_RowCommand" DataKeyNames="TravelAuthorizationID" OnRowDataBound="gvHistoryStatus_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="EmployeeName" HeaderText="Created By" />
                                    <asp:BoundField DataField="RejectionReasons" HeaderText="Rejection Reasons" />
                                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />

                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                <PagerStyle CssClass="PagingIOM" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>

        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>



