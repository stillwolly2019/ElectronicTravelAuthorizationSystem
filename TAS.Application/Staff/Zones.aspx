<%@ Page Title="Manage Zones" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Zones.aspx.cs" Inherits="Staff_Zones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function openModal() {
            $('#DivZone').modal('show');
        }
        function openResidenceModal(ZoneName) {
            $('#DivResidence').modal('show');
            $get("ContentPlaceHolder1_lblZoneName").textContent = ZoneName;
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
                        List of Zones
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
                            <asp:HiddenField ID="hdnZoneID" runat="server" Value="" />
                            <asp:HiddenField ID="hdnLocationID" runat="server" Value="" />
                            <asp:Label ID="lblGVCount" runat="server" Text="0" CssClass="hidden GVCount"></asp:Label>
                            <asp:GridView ID="GVZones" CssClass="table table-striped table-bordered table-hover"
                                AutoGenerateColumns="False"
                                OnRowCommand="GVZones_RowCommand"
                                DataKeyNames="ZoneID,LocationID,ZoneName"
                                OnRowDeleting="GVZones_RowDeleting"
                                OnRowDataBound="GVZones_RowDataBound" EmptyDataText="No records to display" runat="server">
                                <Columns>
                                    
                                    <asp:TemplateField HeaderText="Zone Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="liZoneName" CausesValidation="False" CommandName="EditItem" ToolTip="Edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><%#Eval("ZoneName") %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="ibAdd" CausesValidation="False" CommandName="ManageResidence" ToolTip="Manage Residences" runat="server" CommandArgument='<%# Container.DisplayIndex %>'> <li class="fa fa-sitemap"> </li> Manage Residence</asp:LinkButton>
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
                            <asp:LinkButton ID="LnkAddNewItem" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewItem_Click"><li class="fa fa-save"></li>&nbsp;Add New Zone</asp:LinkButton>
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
    <div class="modal fade" id="DivZone" tabindex="-1" role="form" aria-labelledby="myNewUser" aria-hidden="true">
        <div class="modal-dialog" style="width: 600px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="lblTitle">Zone
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
                                                <label>Zone Name</label>
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





    <div class="modal fade" id="DivResidence" tabindex="-1" role="form" aria-labelledby="myResidenceModalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 700px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Manage Residences for
                   <b>
                       <asp:Label ID="lblZoneName" runat="server"></asp:Label></b>
                    </h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelAmsg" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                                        <asp:Label ID="lblAmsg" ForeColor="Green" runat="server" Text="&nbsp;"></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="row">

                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>Residence Name</label>
                                                <asp:TextBox ID="txtResidenceName" runat="server" CssClass="form-control Req1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>&nbsp;</label>
                                                <asp:LinkButton ID="LnkResidenceave" CssClass="form-control btn btn-outline btn-primary" runat="server" OnClick="LnkResidenceave_Click" OnClientClick="return SaveValidationUnit();"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>

                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:HiddenField ID="hdnResidenceID" runat="server" Value="" />
                                        <asp:Label ID="lblGVResidenceCount" runat="server" Text="0" CssClass="hidden GVResidenceCount"></asp:Label>
                                        <asp:GridView ID="GVResidence" runat="server" CssClass="table table-striped table-bordered table-hover"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="GVResidence_RowCommand"
                                            DataKeyNames="ResidenceID,ResidenceName,ZoneName,ZoneID,LocationID"
                                            OnRowDeleting="GVResidence_RowDeleting"
                                            OnRowDataBound="GVResidence_RowDataBound"
                                            EmptyDataText="No records to display">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Residence Name">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="liResidenceName" CausesValidation="False" CommandName="EditItem" ToolTip="Edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><%#Eval("ResidenceName") %></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" CausesValidation="False" CommandName="DeleteItem" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Delete</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="PagingIOM" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer">
                        </div>
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
                $('#ContentPlaceHolder1_GVZones').DataTable({
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
                        $('#ContentPlaceHolder1_GVZones').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false
                        });
                    }
                }
            });
        };
        $(document).ready(function () {
            if ($(".GVResidenceCount").text() != "0") {
                $('#ContentPlaceHolder1_GVResidence').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: false,
                    "lengthChange": false
                });
            }
        });
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


