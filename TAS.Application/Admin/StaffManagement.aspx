<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StaffManagement.aspx.cs" Inherits="Admin_StaffManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }

        function openEditStaffModal() {
            $('#DivEditStaff').modal('show');
        }
        function openStaffModal(NameMove) {
            $('#DivManageStaff').modal('show');
            $get("ContentPlaceHolder1_lblEmployeeNameMove").textContent = NameMove;
        }
        function SaveValidation() {
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
        }
        function SaveStaffValidation() {
            var a = 0;
            $(".Req1").each(function () {
                if ($(this).val() == "" || $(this).val() == null || $(this).val() == "0" || $(this).val() == "-- Please Select --" || $(this).val() == "00000000-0000-0000-0000-000000000000") {
                    $(this).addClass("invalid");
                    a = a + 1;
                }
                else {
                    $(this).removeClass("invalid");
                }
                a += validateEmail();
                a += validateNumber();
            });
            if (a > 0) {
                return false;
            }
            else {
                return true;
            }
        }

        function validateNumber(NumberField) {
            var reg = /^([A-Za-z0-9]{1,7})$/;
            var a = 0;
            $(".ReqN").each(function () {
                if (reg.test($(this).val()) == false) {

                    $(this).addClass("invalid");
                    $(this).tooltip("show");
                    a = a + 1;
                }
                else {
                    $(this).removeClass("invalid");
                    $(this).tooltip("hide");

                }
            });

            if (a > 0) {
                return 1;
            }
            else {
                return 0;
            }
        }
        function validateEmail(emailField) {

            var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            var a = 0;
            $(".ReqE").each(function () {

                if (reg.test($(this).val()) == false) {

                    $(this).addClass("invalid");

                    $(this).tooltip({ title: 'Email is incorrect' });
                    $(this).tooltip("show");
                    a = a + 1;
                }
                else {
                    $(this).removeClass("invalid");
                    $(this).tooltip("hide");

                }
            });

            if (a > 0) {
                return 1;
            }
            else {
                return 0;
            }
        }



    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12">
                    <h1 class="page-header">
                        <li class="fa fa-group" style="font-size: 40px; color: #337ab7"></li>
                        Staff Management<h1></h1>
                        <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                            <asp:Label ID="lblmsg" runat="server">&nbsp;</asp:Label>
                        </asp:Panel>
                        <h1></h1>
                        <h1></h1>
                        <h1></h1>
                        <h1></h1>
                        <h1></h1>
                        <h1></h1>
                    </h1>

                </div>
            </div>

            <div class="row" style="padding-bottom: 10px">
                <table style="width: 99%;">
                    <tr>
                        <td style="width: 30%"></td>
                        <td>
                            <asp:Label Text="Reflect changes on attendance monitoring system" runat="server" Visible="false" />
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckAMS" runat="server" Visible="false" />
                        </td>
                        <td style="text-align: right; font-size: 10pt; width: 8%">Mission:&nbsp;&nbsp;  
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMissionsName" CssClass="form-control input-sm" Width="176" DataTextField="Name" DataValueField="MissionID" runat="server"></asp:DropDownList>

                        </td>
                        <td style="text-align: right; font-size: 10pt; width: 10%; padding-left: 8px">Location:&nbsp;&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlLocation" CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="DDLDepartment_SelectedIndexChanged" Width="176" DataTextField="LocationName" DataValueField="LocationID" runat="server"></asp:DropDownList>
                        </td>
                        <td style="text-align: right; font-size: 10pt; width: 8%">Department:&nbsp;&nbsp;  
                        </td>
                        <td>
                            <asp:DropDownList ID="DDLDepartment" runat="server" AutoPostBack="true" Width="176" OnSelectedIndexChanged="DDLDepartment_SelectedIndexChanged" CssClass="form-control input-sm" AppendDataBoundItems="True">
                            </asp:DropDownList>
                        </td>

                    </tr>
                </table>

            </div>
            <div class="row">
                <div class="col-lg-12">

                    <asp:HiddenField ID="hdnEmployeeID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnDepartmentID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnUnitID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnSubUnitID" runat="server" Value="" />
                    <asp:HiddenField ID="hdnPRIMSNumber" runat="server" Value="" />
                    <asp:Label ID="lblGVStaffManagementCount" runat="server" Text="0" CssClass="hidden StaffManagementCount"></asp:Label>
                    <asp:GridView ID="GVStaffManagement" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                        EmptyDataText="No records to display" OnRowCommand="GVStaffManagement_RowCommand" OnRowDataBound="GVStaffManagement_RowDataBound"
                        DataKeyNames="UserID,LocationID,PRISM_Number,UserName,Email,DisplayName,DepartmentID,UnitDepartment,UnitID,SubUnitID,DepartmentName,UNID,CountryID,CountryName,Title,IsManager,IsInternationalStaff">
                        <Columns>

                            <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                    <div class="text-center">
                                    <asp:LinkButton ID="lbUserName" CausesValidation="False" CommandName="EditItem" ToolTip="Edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><%#Eval("UserName") %></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="DisplayName" HeaderText="Staff Name" />

                            <asp:TemplateField HeaderText="PRISMNumber">
                                <ItemTemplate>
                                    <div class="text-center">
                                        <asp:Label ID="lblPRISMNumber" Text='<%# Bind("PRISM_Number") %>' runat="server"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Mission">
                                <ItemTemplate>
                                    <div class="text-center">
                                        <asp:Label ID="lblMissionName" Text='<%# Bind("MissionName") %>' runat="server"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Nationality">
                                <ItemTemplate>
                                    <div class="text-center">
                                        <asp:Label ID="lblCountryName" Text='<%# Bind("CountryName") %>' runat="server"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <div class="text-center">
                                        <asp:Label ID="lblLocationName" Text='<%# Bind("LocationName") %>' runat="server"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="UnitDepartment" HeaderText="Unit/Department" />

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Move
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbMove" runat="server" CommandName="Move" ToolTip="Move Staff" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-rotate-right"></li> Move</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Is Manager?" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <div class="text-center">
                                    <asp:LinkButton ID="ibIsManager" CausesValidation="False" CommandName="Manager" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="text-center" HeaderText="Action">
                                <ItemTemplate>
                                    <div class="text-center">
                                    <asp:LinkButton ID="ibDelete" CausesValidation="False" CommandName="deleteItem" ToolTip="Delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Delete</asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                        <PagerStyle CssClass="PagingIOM" />
                    </asp:GridView>

                    <div class="row">
                        <div class="col-lg-9">
                        </div>
                        <div class="col-lg-3">
                            <asp:LinkButton ID="LnkAddNewItem" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewItem_Click"><li class="fa fa-save"></li>&nbsp;Add New Staff</asp:LinkButton>
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


    <div class="modal fade" id="DivEditStaff" tabindex="-1" role="form" aria-labelledby="editstaff" aria-hidden="true">
        <div class="modal-dialog" style="width: 1200px">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="modal-content" style="width: 100%;">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:Label ID="lblMoidficationType" runat="server"></asp:Label>Staff Member
                         <asp:Label ID="lblEditStaff" Font-Bold="true" runat="server"></asp:Label></h4>
                        </div>

                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="pnlMessageEditStaff" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblMessageEditStaff" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="panel-body">


                                <div class="row">
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Location</label>
                                            <asp:DropDownList ID="ddlLocationNew" CssClass="form-control input-sm Req1" DataTextField="LocationName" DataValueField="LocationID" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Department</label>
                                            <asp:DropDownList ID="ddlStaffDepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStaffDepartment_SelectedIndexChanged" CssClass="form-control  Req1" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Unit</label>
                                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Sub Unit</label>
                                            <asp:DropDownList ID="ddlSubUnit" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Staff Name</label>
                                            <asp:TextBox ID="txtFullName" CssClass="form-control  Req1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>User Name</label>
                                            <asp:TextBox ID="txtUserName" CssClass="form-control  Req1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Email</label>
                                            <asp:TextBox ID="txtEmail" data-toggle="tooltip" onfocusout="validateEmail(this);" CssClass="form-control  Req1 ReqE" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Personal No</label>
                                            <asp:TextBox ID="txtPERNO" data-toggle="tooltip" CssClass="form-control  Req1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>UNID</label>
                                            <asp:TextBox ID="txtUNID" data-toggle="tooltip" CssClass="form-control  Req1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Title</label>
                                            <asp:TextBox ID="txtTitle" data-toggle="tooltip" CssClass="form-control  Req1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Nationality</label>
                                            <asp:DropDownList ID="ddlNationality" runat="server" CssClass="form-control  Req1" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>


                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>Gender</label>
                                            <br />
                                                <asp:RadioButton ID="rdMale" runat="server" Text="Male" CssClass="radio-inline" GroupName="Gender" AutoPostBack="true" />
                                                <asp:RadioButton ID="rdFemale" runat="server" Text="Female" CssClass="radio-inline" GroupName="Gender" AutoPostBack="true" />
                                        </div>
                                    </div>




                                </div>


                                <div class="row">
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:LinkButton ID="lnkbtnEditStaffSave" runat="server" CssClass="form-control btn btn-outline btn-primary" OnClientClick="return SaveStaffValidation();" OnClick="lnkbtnEditStaffSave_Click"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>
                                    </div>
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:LinkButton ID="lnkbtnEditStaffClear" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="lnkbtnEditStaffClear_Click"><li class="fa fa-refresh "></li>&nbsp;Clear</asp:LinkButton>

                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                        </div>

                    </div>
                    <!-- /.modal-content -->
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- /.modal-dialog -->
    </div>






    <div class="modal fade" id="DivManageStaff" tabindex="-1" role="form" aria-labelledby="myNewPage" aria-hidden="true">
        <div class="modal-dialog" style="width: 1200px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Move Staff Members
                         <asp:Label ID="lblEmployeeNameMove" Font-Bold="true" runat="server"></asp:Label></h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageStaff" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgStaff" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label>Business Area</label>
                                            <asp:DropDownList ID="DDLLocationMove" CssClass="form-control input-sm Req" DataTextField="LocationName" DataValueField="LocationID" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label>Department</label>
                                            <asp:DropDownList ID="ddlMoveDepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMoveDepartment_SelectedIndexChanged" CssClass="form-control Req" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label>Unit</label>
                                            <asp:DropDownList ID="ddlMoveUnit" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMoveUnit_SelectedIndexChanged" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label>Sub Unit</label>
                                            <asp:DropDownList ID="ddlMoveSubUnit" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:LinkButton ID="lnkbtnMoveSave" runat="server" CssClass="form-control btn btn-outline btn-primary" OnClientClick="return SaveValidation();" OnClick="lnkbtnMoveSave_Click"><li class="fa fa-save"></li>&nbsp;Save</asp:LinkButton>

                                    </div>
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:LinkButton ID="lnkbtnMoveClear" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="lnkbtnMoveClear_Click"><li class="fa fa-refresh "></li>&nbsp;Clear</asp:LinkButton>

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
            if ($(".StaffManagementCount").text() != "0") {
                $('#ContentPlaceHolder1_GVStaffManagement').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: true
                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".StaffManagementCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVStaffManagement').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: true
                        });
                    }
                }
            });
        };
    </script>
    </asp:Content>


