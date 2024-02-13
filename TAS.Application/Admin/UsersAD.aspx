<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UsersAD.aspx.cs" Inherits="Admin_UsersAD" %>

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
            <div class="row">
                <div class="col-lg-12">
                    <h4 class="page-header">User Registration</h4>
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
                                    DataKeyNames="UserID,UserName,Email,FirstName,LastName,IsManager,Active,PRISM_Number"
                                    OnRowDeleting="GVUsers_RowDeleting" OnRowDataBound="GVUsers_RowDataBound" EmptyDataText="No records to display">
                                    <Columns>
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" ControlStyle-CssClass="cssColor" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                        
                                         <asp:TemplateField HeaderText="Active" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align:center">
                                                <asp:LinkButton ID="ibDeactivate" CausesValidation="False" CommandName="ActivateDeactivateUser" ToolTip="Click to Deactivate" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDeactivation();" runat="server"> &nbsp;&nbsp;</asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align:center">
                                                    <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="EditUser" ToolTip="Click to edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                                   &nbsp;&nbsp; &nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ibDelete" CausesValidation="False" CommandName="deleteItem" ToolTip="Click to delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                                   &nbsp;&nbsp; &nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="ibUserRoles" CausesValidation="False" CommandName="ManageRolesAccesses" ToolTip="Manage Role Accesses" CommandArgument='<%# Container.DisplayIndex %>'  runat="server"> <li class="fa fa-gear"> </li> Role Access Locations</asp:LinkButton>
                                                </div>


                                            </ItemTemplate>
                                            </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9">
                        </div>
                        <div class="col-lg-3">
                            <asp:LinkButton ID="LnkAddNewUser" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewUser_Click"><li class="fa fa-save"></li>&nbsp;Add New User</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="DivAddNewUser" tabindex="-1" role="form" aria-labelledby="myNewUser" aria-hidden="true">
        <div class="modal-dialog" style="width: 750px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="AddNewUserLabel">Add New User</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageAddNewUser" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgAddNewUser" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <table>
                                        <tr>
                                            <td style="vertical-align: top; padding: 20px; width: 90%">
                                                <table>
                                                    <tr>
                                                        <td class="tdLabel">User Name</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:HiddenField ID="hfUserName" runat="server" />
                                                            <asp:TextBox ID="txtUserName" CssClass="form-control Req" OnTextChanged="txtUserName_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="txtUserNameAutoCompleteExtender" runat="server"
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
                                                        <td class="tdLabel">First Name</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtFirstName" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="tdLabel">Last Name</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtLastName" CssClass="form-control Req" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" style="vertical-align: top">Roles</td>
                                                        <td class="tdField">
                                                            <asp:CheckBoxList ID="chkRoles" runat="server" CssClass="checkbox-inline">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                        <td class="tdLabel" style="vertical-align: top">Email</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtEmail" CssClass="form-control Req" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel" style="vertical-align: top">Is Manager</td>
                                                        <td class="tdField">
                                                            <asp:CheckBox ID="chkIsManager" runat="server" />
                                                        </td>
                                                        <td class="tdLabel" style="vertical-align: top">Personnel Number</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtPersNo" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="row">
                                <div class="col-lg-6">
                                    <asp:LinkButton ID="LnkSave" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkSave_Click" OnClientClick="return SaveValidation();"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>
                                </div>
                                <div class="col-lg-6">
                                    <asp:LinkButton ID="LnkClose" CssClass="form-control btn btn-outline btn-primary" runat="server" data-dismiss="modal" Font-Size="12px"><li class="fa fa-sign-out"></li>&nbsp;Close</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

    <div class="modal fade" id="DivUserAccesses" tabindex="-1" role="form" aria-labelledby="UserLocationsModalLabel" aria-hidden="true">
        <%--<div class="modal-dialog" style="width: 700px">--%>
        <div class="modal-dialog" style="width: 90%">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Roles and access Information:  <b><asp:Label ID="lblFullName" runat="server"></asp:Label></b></h4>
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

                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>Role Name</label>
                                                <asp:DropDownList ID="DDLUserRoles" runat="server" CssClass="form-control Req1" AutoPostBack="True" DataTextField="RoleName" DataValueField="RoleID"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>Location</label>
                                                <asp:DropDownList ID="DDLLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLLocation_SelectedIndexChanged" CssClass="form-control  Req1" AppendDataBoundItems="True" DataTextField="LocationName" DataValueField="LocationID"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>Department</label>
                                                <asp:DropDownList ID="DDLDepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLDepartment_SelectedIndexChanged" CssClass="form-control  Req1" AppendDataBoundItems="True" DataTextField="DepartmentName" DataValueField="DepartmentID"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>Unit</label>
                                                <asp:DropDownList ID="DDLUnit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLUnit_SelectedIndexChanged" CssClass="form-control  Req1" AppendDataBoundItems="True" DataTextField="UnitName" DataValueField="UnitID"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>Sub Unit</label>
                                                <asp:DropDownList ID="DDLSubUnit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDLSubUnit_SelectedIndexChanged" CssClass="form-control  Req1" AppendDataBoundItems="True" DataTextField="SubUnitName" DataValueField="SubUnitID">
                                                </asp:DropDownList>
                                            </div>
                                        </div>


                                        <div class="col-lg-4">
                                            <div class="form-group">
                                                <label>&nbsp;</label>
                                                <asp:LinkButton ID="LnkUserRoleAccessave" CssClass="form-control btn btn-outline btn-primary" runat="server" OnClick="LnkUserRoleAccessave_Click" OnClientClick="return SaveValidationUnit();"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>

                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:HiddenField ID="hdnUserID" runat="server" Value="" />
                                        <asp:Label ID="lblGVUserRoleAccessCount" runat="server" Text="0" CssClass="hidden GVUserRoleAccessCount"></asp:Label>
                                        <asp:GridView ID="GVUserRoleAccess" runat="server" CssClass="table table-striped table-bordered table-hover"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="GVUserRoleAccess_RowCommand"
                                            DataKeyNames="ID,UserID,RoleID,LocationID,DepartmentID,UnitID,SubUnitID"
                                            OnRowDeleting="GVUserRoleAccess_RowDeleting"
                                            OnRowDataBound="GVUserRoleAccess_RowDataBound"
                                            EmptyDataText="No records to display">
                                            <Columns>
                                            <asp:BoundField DataField="RoleName" HeaderText="Role" />
                                            <asp:BoundField DataField="LocationName" HeaderText="Location" />
                                            <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                                            <asp:BoundField DataField="UnitName" HeaderText="Unit" />
                                            <asp:BoundField DataField="SubUnitName" HeaderText="Sub Unit" />

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblDelete" CausesValidation="False" CommandName="DeleteUserRoleAccess" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Delete</asp:LinkButton>
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

