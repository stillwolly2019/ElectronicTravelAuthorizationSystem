
<%@ Page Title="Accomodations" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AccomodationLocations.aspx.cs" Inherits="Admin_AccomodationLocations" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function openModal() {
            $('#DivAccomodation').modal('show');
        }
        function openResidenceModal(AccomodationName) {
            $('#DivResidence').modal('show');
            $get("ContentPlaceHolder1_lblAccomodationName").textContent = AccomodationName;
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
                <div class="col-lg-12">
                    <h4 class="page-header" style="color: #337ab7">
                        List of Accomodations by Location
                        <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                            <asp:Label ID="lblmsg" runat="server"></asp:Label>
                        </asp:Panel>
                        <h1></h1>
                        <h1></h1>
                    </h4>
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
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="hdnAccomodationID" runat="server" Value="" />
                            <asp:HiddenField ID="hdnLocationID" runat="server" Value="" />
                            <asp:Label ID="lblGVCount" runat="server" Text="0" CssClass="hidden GVCount"></asp:Label>
                            <asp:GridView ID="GVAccomodations" CssClass="table table-striped table-bordered table-hover"
                                AutoGenerateColumns="False"
                                OnRowCommand="GVAccomodations_RowCommand"
                                DataKeyNames="AccomodationID,LocationID,AccomodationName"
                                OnRowDeleting="GVAccomodations_RowDeleting"
                                OnRowDataBound="GVAccomodations_RowDataBound" EmptyDataText="No records to display" runat="server">
                                <Columns>
                                    
                                    <asp:TemplateField HeaderText="Accomodation Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="liAccomodationName" CausesValidation="False" CommandName="EditItem" ToolTip="Edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><%#Eval("AccomodationName") %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibDelete" CausesValidation="False" CommandName="DeleteItem" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Delete</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                <PagerStyle CssClass="PagingIOM" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9">
                        </div>
                        <div class="col-lg-3">
                            <asp:LinkButton ID="LnkAddNewItem" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewItem_Click"><li class="fa fa-save"></li>&nbsp;Add New Accomodation</asp:LinkButton>
                        </div>
                    </div>
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
    <div class="modal fade" id="DivAccomodation" tabindex="-1" role="form" aria-labelledby="myNewUser" aria-hidden="true">
        <div class="modal-dialog" style="width: 600px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="lblTitle">Accomodation
                    </h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessagePopUp" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgPopUp" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="row">

                                        <div class="col-lg-6">
                                            <div class="form-group">
                                                <label>Accomodation Name</label>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control Req"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-lg-3">
                                            <label>&nbsp;</label>
                                            <asp:LinkButton ID="LnkSave" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkSave_Click" OnClientClick="return SaveValidation();"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>
                                        </div>
                                        <div class="col-lg-3">
                                            <label>&nbsp;</label>
                                            <asp:LinkButton ID="LnkClose" CssClass="form-control btn btn-outline btn-primary" runat="server" data-dismiss="modal" Font-Size="12px"><li class="fa fa-sign-out"></li>&nbsp;Close</asp:LinkButton>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

    <script>
        $(document).ready(function () {
            if ($(".GVCount").text() != "0") {
                $('#ContentPlaceHolder1_GVAccomodations').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: false
                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".GVCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVAccomodations').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false
                        });
                    }
                }
            });
        };
       
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".GVResidenceCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVResidence').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false,
                            "lengthChange": false
                        });
                    }
                }
            });
        };
       
    </script>
</asp:Content>



