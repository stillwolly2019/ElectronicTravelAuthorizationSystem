<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StaffManagement.aspx.cs" Inherits="Staff_StaffManagement" %>
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
                 <div class="col-lg-12">
                    <h4 class="page-header">Radio Check Participants</h4>
                    <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </asp:Panel>
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
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label>PER NO,User Name, Call Sign or Staff Name</label>
                                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>    
                                    
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="BtnAdvSearch" runat="server" Text="Search" CssClass="form-control btn-primary fa-search" OnClick="BtnAdvSearch_Click" />
                                    </div>
                                    <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="form-control btn-primary fa-refresh" OnClick="BtnClear_Click" />
                                    </div>
                                    <!--
                                      <div class="col-lg-2">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="LnkAddNewUser" runat="server" Text="Add Staff" CssClass="form-control btn-primary fa-plus" OnClick="LnkAddNewUser_Click" />
                                    </div>
                                    -->

                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="row">
                        <div class="col-lg-12">
                            <div class="dataTable_wrapper">
                                <asp:Label ID="lblGVStaffCount" runat="server" Text="0" CssClass="hidden UsersCount"></asp:Label>
                                <asp:GridView ID="GVStaff" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="GVStaff_RowCommand"
                                    DataKeyNames="PRISMNumber,UserName, Active"  OnRowDataBound="GVStaff_RowDataBound" EmptyDataText="No records to display">
                                    <Columns>

                                         <asp:TemplateField ItemStyle-Wrap="false" HeaderText="PER No" ItemStyle-Width="100px" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <div style="text-align: center;width:100px;">
                                                <asp:LinkButton ID="ibDelete" runat="server" CommandName="VSI" ToolTip="View Staff Information" CommandArgument='<%# Container.DisplayIndex %>' Text='<%# Eval("PRISMNumber") %>'> View</asp:LinkButton>
                                            </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CallSign" HeaderText="Call Sign"  ItemStyle-Width="80px" HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="100px" CssClass="text-center" />
                                        </asp:BoundField>
                                       
                                        <asp:BoundField DataField="FullName" HeaderText="Staff Name" />
                                        <asp:BoundField DataField="UnitDepartment" HeaderText="Unit/Department" />

                                        <asp:BoundField DataField="Category" HeaderText="Staff Category"  ItemStyle-Width="150px" HeaderStyle-CssClass="text-center">
                                        <ItemStyle Width="150px" CssClass="text-center" />
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Active" ItemStyle-Width="80px">
                                        <ItemStyle CssClass="text-center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ibIsActive" CausesValidation="False" CommandName="IsActive" CssClass="text-center" runat="server" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
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
                            <asp:LinkButton ID="LnkAddNewItem" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewItem_Click"><li class="fa fa-group"></li>&nbsp;Add New Staff</asp:LinkButton>
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

    <script>
        $(document).ready(function () {
            if ($(".UsersCount").text() != "0") {
                $('#ContentPlaceHolder1_GVStaff').DataTable({
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
                        $('#ContentPlaceHolder1_GVStaff').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false
                        });
                    }
                }
            });
        };
    </script>
    
</asp:Content>


