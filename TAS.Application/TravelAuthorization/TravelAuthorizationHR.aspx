<%@ Page Title="Travel Authorizations" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TravelAuthorizationHR.aspx.cs" Inherits="TravelAuthorization_TravelAuthorizationHR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function SaveValidation() {
            var a = 0;
            $(".Req").each(function () {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">Travel Authorizations</h1>
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-green">
                        <div class="panel-heading">
                            Travel Authorization Status - Travel Authorization #: 
                            <asp:Label ID="lblTANo" runat="server"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <div class="row">

                                <div id="divStatus" class="col-lg-3" runat="server">
                                    <label>Status</label>
                                    <asp:DropDownList ID="ddlStatusCode" runat="server" Enabled="true" DataTextField="Description" DataValueField="Code" CssClass="form-control Req" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusCode_SelectedIndexChanged" />
                                </div>
                                <div class="col-lg-3" id="RejectionReasonDiv" runat="server" style="display: none;">

                                    <div class="form-group">
                                        <label>Rejection Reason</label>
                                        <asp:ListBox ID="lstRejectionReason" runat="server" CssClass="form-control" Height="70px" Visible="false" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>



                            </div>

                            <div class="row">
                                <div class="col-lg-3">
                                    <label>&nbsp;</label>
                                    <br />
                                </div>
                                <div class="col-lg-3">
                                    <label>&nbsp;</label>

                                </div>
                                <div class="col-lg-3">
                                    <label>&nbsp;</label>

                                </div>
                                <div class="col-lg-3">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnSaveStatus" runat="server" Text="Save" CssClass="form-control btn-success" OnClick="btnSaveStatus_Click" />
                                </div>

                            </div>

                        </div>
                    </div>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Travel Authorizations Form 
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Travelers Name</label>
                                        <asp:TextBox ID="txtTravelersName" Enabled="false" runat="server" CssClass="form-control Req"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Purpose Of Travel</label>
                                        <asp:TextBox ID="txtPurposeOfTravel" Enabled="false" runat="server" CssClass="form-control Req"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Trip Schema Code</label>
                                        <asp:DropDownList ID="ddlTripSchemaCode" Enabled="false" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control Req" />
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Mode Of Travel Code</label>
                                        <asp:DropDownList ID="ddlModeOfTravelCode" Enabled="false" runat="server" DataTextField="Description" DataValueField="LookupsID" CssClass="form-control Req" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label style="padding-bottom: 5px">
                                            City(ies) of accommodation (specify)<br />
                                            <br />
                                        </label>
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>
                                            Private Stay/Annual leave (or other leave) requested<br />
                                            <asp:RadioButton ID="rdYAnnual" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Annual" />
                                            <asp:RadioButton ID="rdNoAnnual" runat="server" Text="No" CssClass="radio-inline" GroupName="Annual" />
                                        </label>
                                        <asp:TextBox ID="txtAnnual" runat="server" CssClass="form-control"></asp:TextBox>
                                        <p class="help-block">Indicate Dates</p>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>
                                            Private Deviation<br />
                                            <asp:RadioButton ID="rdYesDiv" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Div" />
                                            <asp:RadioButton ID="rdNoDiv" runat="server" Text="No" CssClass="radio-inline" GroupName="Div" />
                                        </label>
                                        <asp:TextBox ID="txtDiv" runat="server" CssClass="form-control"></asp:TextBox>
                                        <p class="help-block">Specify Leg(s)</p>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>
                                            Accommodation (board and/or lodging) provided<br />
                                            <asp:RadioButton ID="rdYesAcc" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Acc" />
                                            <asp:RadioButton ID="rdNoAcc" runat="server" Text="No" CssClass="radio-inline" GroupName="Acc" />
                                        </label>
                                        <asp:TextBox ID="txtAcc" runat="server" CssClass="form-control"></asp:TextBox>
                                        <p class="help-block">Specify</p>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="border: solid 1px #0094ff">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>
                                            Travel advance requested<br />
                                            <asp:RadioButton ID="rdYesAdv" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Adv" />
                                            <asp:RadioButton ID="rdNoAdv" runat="server" Text="No" CssClass="radio-inline" GroupName="Adv" />
                                        </label>
                                        <asp:TextBox ID="txtAdvAmnt" runat="server" CssClass="form-control"></asp:TextBox>
                                        <p class="help-block">Amount</p>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label style="padding-bottom: 5px">
                                            <br />
                                            <br />
                                        </label>
                                        <asp:DropDownList ID="DDLAdvCurr" runat="server" CssClass="form-control" DataTextField="CurrencyName" DataValueField="CurrencyID"></asp:DropDownList>
                                        <p class="help-block">Currency</p>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>
                                            Via<br />
                                            <asp:RadioButton ID="rdViaBank" runat="server" Text="Bank Transfer" CssClass="radio-inline" GroupName="Via" />
                                            <asp:RadioButton ID="rdViaCheck" runat="server" Text="Check" CssClass="radio-inline" GroupName="Via" />
                                            <asp:RadioButton ID="rdViaCash" runat="server" Text="Cash" CssClass="radio-inline" GroupName="Via" />
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="padding-top: 5px">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>
                                            Visa(s) Obtained<br />
                                            <asp:RadioButton ID="rdVisaNA" runat="server" Text="N/A" CssClass="radio-inline" GroupName="Visa" />
                                            <asp:RadioButton ID="rdVisaNo" runat="server" Text="No" CssClass="radio-inline" GroupName="Visa" />
                                            <asp:RadioButton ID="rdVisYes" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Visa" />
                                        </label>
                                        <asp:TextBox ID="txtVisa" runat="server" CssClass="form-control"></asp:TextBox>
                                        <p class="help-block">Visa Issued</p>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>
                                            Health Breifings and vaccination obtained<br />
                                            <asp:RadioButton ID="rdHealthNA" runat="server" Text="N/A" CssClass="radio-inline" GroupName="Health" />
                                            <asp:RadioButton ID="rdHealthNo" runat="server" Text="No" CssClass="radio-inline" GroupName="Health" />
                                            <asp:RadioButton ID="rdHealthYes" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Health" />
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="border: solid 1px #0094ff">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <asp:CheckBox ID="checkSecurityTraining" runat="server" CssClass="checkbox-inline" Text="Security Training needed" Enabled="False" /><br />
                                        <p class="help-block">if yes, requested by</p>
                                        <asp:RadioButton ID="rdMissionYes" runat="server" Text="Mission" CssClass="radio-inline" GroupName="Mission" />
                                        <asp:RadioButton ID="rdMissionNo" runat="server" Text="Headquarters" CssClass="radio-inline" GroupName="Mission" />
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <asp:CheckBox ID="checkSecurityClearance" Checked="true" Enabled="false" runat="server" Text="Security Clearance completed" CssClass="checkbox-inline" AutoPostBack="true" />
                                    </div>
                                </div>


                            </div>
                            <div class="row" style="padding-top: 5px">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label>
                                            Please confirm that the Chief of Mission (COM) at destination has been informed of your arrival. If the function of COM at the duty station of destination does not exist or if the COM is absent, the Officer In Charge of that Mission or the Regional Director at the appropriate Regional Office must be informed.&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="chkConfirm" runat="server" Text="Yes" CssClass="checkbox-inline" />
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <hr />
                                        <asp:GridView ID="gvTravelItinerary" CssClass="table table-bordered  table-hover" runat="server" AllowPaging="false" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="ModeOfTravelName" HeaderText="Mode Of Travel" />
                                                <asp:BoundField DataField="FromLocationCodeName" HeaderText="From Location" />
                                                <asp:BoundField DataField="FromLocationDate" HeaderText="From Location Date" DataFormatString="{0:d MMM yyyy}" />
                                                <asp:BoundField DataField="ToLocationCodeName" HeaderText="To Location" />
                                                <asp:BoundField DataField="ToLocationDate" HeaderText="To Location Date" DataFormatString="{0:d MMM yyyy}" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <hr />
                                        <div class="info">
                                            WBS Info
                                            <asp:RadioButton ID="rdPercentage" runat="server" CssClass="radio-inline" Text="Percentage" Checked="true" Enabled="false" GroupName="Per" />
                                            <asp:RadioButton ID="rdAmount" runat="server" CssClass="radio-inline" Text="Amount" Enabled="false" GroupName="Per" />
                                        </div>
                                        <asp:GridView ID="gvWBS" CssClass="table table-bordered  table-hover" runat="server" AllowPaging="false" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="WBSCode" HeaderText="WBS Code" />
                                                <asp:BoundField DataField="PercentageOrAmount" HeaderText="Percentage Or Amount" />
                                                <asp:BoundField DataField="Note" HeaderText="Note" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-3">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnDownload" runat="server" Text="Download to PDF" CssClass="form-control btn-primary" OnClick="btnDownload_Click" />
                                </div>
                                <div class="col-lg-3">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="form-control btn-success" OnClick="btnSave_Click" OnClientClick="return SaveValidation();" />
                                </div>
                                <div class="col-lg-3">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="form-control btn-warning" OnClick="btnClear_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

