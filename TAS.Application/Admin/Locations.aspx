<%@ Page Title="Locations" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Locations.aspx.cs" Inherits="Admin_Locations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function openModal() {
            $('#DivAddNewLocation').modal('show');
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
                    <h4 class="page-header">Manage Locations</h4>
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
                        <div class="col-lg-12">
                             <asp:Label ID="lblGVLocationsCount" runat="server" Text="0" CssClass="hidden PagesCount"></asp:Label>
                            <asp:GridView ID="GVLocations" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                AllowPaging="false" OnRowCommand="GVLocations_RowCommand"
                                DataKeyNames="LocationID,LocationCode,LocationName,MissionID,MissionName,BusinessArea,ConductRadioCheck,AllowAccess" OnRowDeleting="GVLocations_RowDeleting" OnRowDataBound="GVLocations_RowDataBound" EmptyDataText="No records to display">
                                <Columns>
                                    <asp:BoundField DataField="LocationCode" HeaderText="Location Code"  />
                                    <asp:BoundField DataField="LocationName" HeaderText="Location Name" />
                                    <asp:BoundField DataField="BusinessArea" HeaderText="Business Area" />
                                    <asp:BoundField DataField="MissionName" HeaderText="Mission Name" />
                                    <asp:TemplateField HeaderText="RadioCheck?">

                                        <ItemTemplate>
                                        <div style="text-align:center">
                                            <asp:CheckBox ID="chkRadioCheck" Checked='<%# Bind("ConductRadioCheck") %>' runat="server" Enabled="false" />
                                        </div>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Access?">
                                        <ItemTemplate>
                                        <div style="text-align:center">
                                            <asp:CheckBox ID="chkAllowAccess" Checked='<%# Bind("AllowAccess") %>' runat="server" Enabled="false" />
                                        </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                        <div style="text-align:center">
                                            <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="EditItem" ToolTip="Click to edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                        </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                        <div style="text-align:center">
                                            <asp:LinkButton ID="ibDelete" CausesValidation="False" CommandName="deleteItem" ToolTip="Click to delete" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="return ConfirmDelete();" runat="server"><li class="fa fa-times"></li> Click to delete</asp:LinkButton>
                                        </div>
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
                            <asp:LinkButton ID="LnkAddNewLocation" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewLocation_Click"><li class="fa fa-save"></li>&nbsp;Add New Location</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="DivAddNewLocation" tabindex="-1" role="form" aria-labelledby="myNewLocation" aria-hidden="true">
        <div class="modal-dialog" style="width: 610px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="AddNewLocationLabel">Add New Location</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageAddNewLocation" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgAddNewLocation" runat="server" Text=""></asp:Label>
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
                                                        <td class="tdLabel">Location Code</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtLocationCode" CssClass="form-control Req" Width="300px" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td class="tdLabel">Location Name</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtLocationName" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td class="tdLabel">Business Area</td>
                                                        <td class="tdField">
                                                            <asp:TextBox ID="txtBusinessArea" CssClass="form-control Req" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Mission</td>
                                                        <td class="tdField">
                                                            <asp:DropDownList ID="DDLMissions" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">Conduct Radio Check?</td>
                                                        <td class="tdField">
                                                            <asp:RadioButton ID="rdYesRC" runat="server" Text="Yes" CssClass="checkbox-inline" GroupName="RadioCheck" Checked="True" />
                                                            <asp:RadioButton ID="rdNoRC" runat="server" Text="No" CssClass="checkbox-inline" GroupName="RadioCheck" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdLabel">Allow Access?</td>
                                                        <td class="tdField">
                                                            <asp:RadioButton ID="rdYesAC" runat="server" Text="Yes" CssClass="checkbox-inline" GroupName="Access" />
                                                            <asp:RadioButton ID="rdNoAC" runat="server" Text="No" CssClass="checkbox-inline" GroupName="Access"  Checked="True" />
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
    <script>
        $(document).ready(function () {
            if ($(".PagesCount").text() != "0") {
                $('#ContentPlaceHolder1_GVLocations').DataTable({
                    "pagingType": "full_numbers",
                    stateSave: false
                });
            }
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    if ($(".PagesCount").text() != "0") {
                        $('#ContentPlaceHolder1_GVLocations').DataTable({
                            "pagingType": "full_numbers",
                            stateSave: false
                        });
                    }
                }
            });
        };
    </script>
</asp:Content>

