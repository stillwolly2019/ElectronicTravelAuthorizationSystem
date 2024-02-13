<%@ Page Title="Warden Zone Access" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WardenZoneAccess.aspx.cs" Inherits="Admin_WardenZoneAccess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            cursor: default;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
         function ConfirmDeactivation() {
            return confirm("Are you sure you want to change account status?");
        }
        function openModal() {
            $('#DivAddNewUser').modal('show');
        }
         function openRoleAccessModal(FullName) {
            $('#DivUserAccesses').modal('show');
            $get("ContentPlaceHolder1_lblFullName").textContent = FullName;
        }
        function SaveValidation() {
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
            <div class="row">
                <div class="col-lg-12">
                    <h4 class="page-header">Warden List</h4>
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
                <!-- /.col-lg-12 -->
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label>User name</label>
                                            <asp:TextBox ID="txtSearchUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>         
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label>First name</label>
                                            <asp:TextBox ID="txtSearchFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div> 
                                     <div class="col-lg-2">
                                        <div class="form-group">
                                            <label>Last name</label>
                                            <asp:TextBox ID="txtSearchLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>                                  
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="btnAdvSearch" runat="server" Text="Search" CssClass="form-control btn-primary" OnClick="btnAdvSearch_Click" />
                                    </div>
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="form-control btn-primary" OnClick="btnClear_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <asp:Label ID="lblGVUsersCount" runat="server" Text="0" CssClass="hidden UsersCount"></asp:Label>
                                <asp:GridView ID="GVUsers" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="GVUsers_RowCommand"
                                    DataKeyNames="UserID,UserName,Email,FirstName,LastName,RoleID,FullName,IsManager,Active,PRISM_Number"
                                    OnRowDeleting="GVUsers_RowDeleting" EmptyDataText="No records to display">
                                    <Columns>
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                        
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align:center">
                                                <asp:LinkButton ID="ibUserRoles" CausesValidation="False" CommandName="ManageRolesAccesses" ToolTip="Manage Zonal Access" CommandArgument='<%# Container.DisplayIndex %>'  runat="server"> <li class="fa fa-gear"> </li> Zone Access Details</asp:LinkButton>
                                                </div>


                                            </ItemTemplate>
                                            </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="DivUserAccesses" tabindex="-1" role="form" aria-labelledby="UserLocationsModalLabel" aria-hidden="true">
        <%--<div class="modal-dialog" style="width: 700px">--%>
        <div class="modal-dialog" style="width: 50%">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Warden access Information:  <b><asp:Label ID="lblFullName" runat="server"></asp:Label></b></h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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

                                        <div class="col-lg-5">
                                            <div class="form-group">
                                                <label>Location</label>
                                                <asp:DropDownList ID="DDLLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLLocation_SelectedIndexChanged" CssClass="form-control  Req1" AppendDataBoundItems="True" DataTextField="LocationName" DataValueField="LocationID"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-lg-5">
                                            <div class="form-group">
                                                <label>Zone Name</label>
                                                <asp:DropDownList ID="DDLZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLZone_SelectedIndexChanged" CssClass="form-control  Req1" AppendDataBoundItems="True" DataTextField="ZoneName" DataValueField="ZoneID"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-lg-2">
                                            <div class="form-group">
                                                <label>&nbsp;</label>
                                                <asp:LinkButton ID="LnkUserRoleAccessave" CssClass="form-control btn btn-primary" runat="server" OnClick="LnkUserRoleAccessave_Click" OnClientClick="return SaveValidation();"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>

                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:HiddenField ID="hdnRoleID" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnUserID" runat="server" Value="" />
                                        <asp:Label ID="lblGVUserRoleAccessCount" runat="server" Text="0" CssClass="hidden GVUserRoleAccessCount"></asp:Label>
                                        <asp:GridView ID="GVUserRoleAccess" runat="server" CssClass="table table-striped table-bordered table-hover"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="GVUserRoleAccess_RowCommand"
                                            DataKeyNames="ID,UserID,LocationID,ZoneID"
                                            OnRowDeleting="GVUserRoleAccess_RowDeleting"
                                            OnRowDataBound="GVUserRoleAccess_RowDataBound"
                                            EmptyDataText="No records to display">
                                            <Columns>
                                            <asp:BoundField DataField="LocationName" HeaderText="Location" />
                                            <asp:BoundField DataField="ZoneName" HeaderText="Zone Name" />
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center">
                                                        <ItemTemplate>
                                                    <div class="text-center">
                                                        <asp:LinkButton ID="lblDelete" CausesValidation="False" CommandName="DeleteUserRoleAccess" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Delete</asp:LinkButton>
                                                    </div>
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
            if ($(".UsersCount").text() != "0") {
                $('#ContentPlaceHolder1_GVUsers').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: false
                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".UsersCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVUsers').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false
                        });
                    }
                }
            });
        };
    </script>
</asp:Content>

