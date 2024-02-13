<%@ Page Title="StepMaps" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StepMapping.aspx.cs" Inherits="Admin_StepMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            return confirm("Are you sure you want to delete?");
        }
        function openModal() {
            $('#DivAddNewStepMap').modal('show');
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
                    <h4 class="page-header">Manage Workflow Step Mapping</h4>
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
                             <asp:Label ID="lblCount" runat="server" Text="0" CssClass="hidden GVCount"></asp:Label>
                            <asp:GridView ID="GVStepMaps" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                AllowPaging="false" OnRowCommand="GVStepMaps_RowCommand"
                                DataKeyNames="StatusID,StatusDescription,StepID,StepName,ActionNeeded" OnRowDeleting="GVStepMaps_RowDeleting" OnRowDataBound="GVStepMaps_RowDataBound" EmptyDataText="No records to display">
                                <Columns>
                                    <asp:BoundField DataField="Code" HeaderText="Status Code" />
                                    <asp:BoundField DataField="StatusDescription" HeaderText="Status Description" />
                                    <asp:BoundField DataField="StepName" HeaderText="Workflow Step" HeaderStyle-CssClass="text-center" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ActionNeeded" HeaderText="Action Needed" HeaderStyle-CssClass="text-left" ItemStyle-HorizontalAlign="left" />
                                    
                                     <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center"  ItemStyle-Width="300px">
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                            <asp:LinkButton ID="ibEdit" CausesValidation="False" CommandName="EditItem" ToolTip="Click to edit" runat="server" CommandArgument='<%# Container.DisplayIndex %>'><li class="fa fa-edit"></li> Click to edit</asp:LinkButton>
                                               &nbsp;&nbsp;|&nbsp;&nbsp;
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
                            <asp:LinkButton ID="LnkAddNewStepMap" CssClass="form-control btn btn-outline btn-primary" runat="server" Font-Size="12px" OnClick="LnkAddNewStepMap_Click"><li class="fa fa-save"></li>&nbsp;Add New StepMap</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="DivAddNewStepMap" tabindex="-1" StepMap="form" aria-labelledby="myNewStepMap" aria-hidden="true">
        <div class="modal-dialog" style="width: 550px">
            <div class="modal-content" style="width: 100%;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="AddNewStepMapLabel">Add New StepMap</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="PanelMessageAddNewStepMap" runat="server" CssClass="alert alert-success alert-dismissable" Visible="false">
                                        <asp:Label ID="lblmsgAddNewStepMap" runat="server" Text=""></asp:Label>
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
                                                        <td class="tdLabel">TA Status Description</td>
                                                        <td class="tdField" colspan="3">
                                                              <asp:DropDownList ID="DDLStatusCodes" runat="server" CssClass="form-control Req" Width="300px" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdLabel">Work Flow Step</td>
                                                        <td class="tdField" colspan="3">
                                                              <asp:DropDownList ID="DDLWorkFlow" runat="server" CssClass="form-control Req" Width="300px" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                     <tr>
                                                        <td class="tdLabel">Step Action Needed</td>
                                                        <td class="tdField" colspan="3">
                                                            <asp:TextBox ID="txtActionNeeded" Height="70px" TextMode="MultiLine" runat="server" Width="300px"  CssClass="form-control Req"></asp:TextBox>
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
             if ($(".GVCount").text() != "0") {
                 $('#ContentPlaceHolder1_GVStepMaps').DataTable({
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
                         $('#ContentPlaceHolder1_GVStepMaps').DataTable({
                             "pagingType": "full_numbers",
                             stateSave: false
                         });
                     }
                 }
             });
         };
    </script>
    
</asp:Content>



