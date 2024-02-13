<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConductRadioCheck.aspx.cs" Inherits="RadioCheck_ConductRadioCheck" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDeactivation() {
            return confirm("Are you sure you want to deactivate?");
        }
        
        function SaveValidation() {
            var a = 0;
            $(".Req").each(function () {
                if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0") {
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
        function SaveValidationUnit() {
            var a = 0;
            $(".Req1").each(function () {
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
        function SaveValidationSubUnit() {
            var a = 0;
            $(".Req2").each(function () {
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
                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0") {
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
            $(".Req1").blur(function () {
                var a = 0;
                $(".Req1").each(function () {
                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-1" || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
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
            $(".Req2").blur(function () {
                var a = 0;
                $(".Req2").each(function () {
                    if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-1" || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
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
    <style>
        .summary-header {
            text-align: left;
            font-weight: bold;
            width: 10%;
            font-size: 18px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="row">
                <div class="col-sm-12">
                    <h3 class="page-header">
                        <li class="fa fa-motorola_footer_logo" style="font-size: 40px; color: #337ab7"></li>
                        Daily Radio Check<h3></h3>
                    </h3>
                </div>
            </div>
            </div>

            <div class="row" style="padding-bottom: 10px">
                <table style="width: 99%;">
                    <tr>
                        <td style="width: 80%"></td>

                        <td style="text-align: right; font-size: 10pt; width: 10%">Location:&nbsp;&nbsp;  
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLocationsName" CssClass="form-control" Width="176" DataTextField="LocationName" DataValueField="LocationID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLocationsName_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                </table>

            </div>

            <div class="row">
                <div class="col-lg-12">
                    <asp:HiddenField ID="hdnPERNO" runat="server" Value="" />
                    <asp:HiddenField ID="hdnLocationID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnCallSign" runat="server" Value="" />

                    <asp:Label ID="lblGVStaffRadioCheckCount" runat="server" Text="0" CssClass="hidden StaffRadioCheckCount"></asp:Label>
                    <asp:GridView ID="GVStaffRadioCheck" runat="server"  CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                        EmptyDataText="No records to display" OnRowCommand="GVStaffRadioCheck_RowCommand" OnRowDataBound="GVStaffRadioCheck_RowDataBound"
                        DataKeyNames="PRISMNumber,StaffName,LocationID,DutyStation,CallSign,CallNumber,IsAccountedFor" PageSize="300">
                        <Columns>
                         
                            <asp:BoundField DataField="PRISMNumber" HeaderText="PRISM Number" ItemStyle-Width="120px" HeaderStyle-CssClass="text-center" >
                            <ItemStyle Width="120px" CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="StaffName" HeaderText="Staff Name"/>

                            <asp:BoundField DataField="DutyStation" HeaderText="Duty Station"  ItemStyle-Width="200px" HeaderStyle-CssClass="text-center">
                            <ItemStyle Width="200px" CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="CallSign" HeaderText="Call Sign" HeaderStyle-CssClass="text-center">
                            <ItemStyle Width="100px" CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="CallNumber" HeaderText="Call ID" HeaderStyle-CssClass="text-center">
                            <ItemStyle Width="100px" CssClass="text-center" />
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="Status accounted" HeaderStyle-CssClass="text-center" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <div style="text-align:center">
                                        <asp:LinkButton ID="ibIsAccountedFor" CausesValidation="False" CommandName="AccountedFor" CssClass="text-center" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                </div>
                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" PageButtonCount="1" />
                        <PagerStyle CssClass="PagingIOM" />
                    </asp:GridView>


 
                </div>
            </div>
                    
              <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100" DynamicLayout="false">
                <ProgressTemplate>
                    <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                        <div style="padding-top: 200px;">
                            <img alt="Loading" src="../images/ajax_loader_blue_512.gif" width="300px" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
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
    
    
</asp:Content>


