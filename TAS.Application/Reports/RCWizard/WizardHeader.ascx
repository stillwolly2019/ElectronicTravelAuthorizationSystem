<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WizardHeader.ascx.cs" Inherits="Reports_RCWizard_WizardHeader" %>
<script type="text/javascript">
   
    function SaveValidation() {
        var a = 0;
        $(".Req").each(function () {

            if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-- Please Select --" ||
                $(this).val() == "00000000-0000-0000-0000-000000000000") {
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

<asp:UpdatePanel ID="UpdatePanelRC" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-lg-12">
                <h4 class="page-header">
                    <asp:Label ID="lblTitle" Text="Radio Check Results" runat="server"></asp:Label>
                </h4>
                <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </div>
        </div>
        
        <div class="row">
            <div class="col-lg-12">
                <table style="width: 100%">
                    <tr>
                        <td id="tdStep1" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep1" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-group" Text="" ToolTip="Master Staff List" OnClick="lbStep1_Click"></asp:LinkButton>
                        </td>

                        <td id="tdStep2" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep2" runat="server" CssClass="btn btn-success btn-circle btn-lg fa fa-check" Text="" ToolTip="Staff Accounted For" OnClick="lbStep2_Click"></asp:LinkButton>
                        </td>

                        <td id="tdStep3" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep3" runat="server" CssClass="btn btn-danger btn-circle btn-lg fa fa-close" Text="" ToolTip="Staff Unaccounted For" OnClick="lbStep3_Click"></asp:LinkButton>
                        </td>

                        <td id="tdStep4" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStep4" runat="server" CssClass="btn btn-warning btn-circle btn-lg fa fa-plane" Text="" ToolTip="In/Out Movements" OnClick="lbStep4_Click"></asp:LinkButton>
                        </td>

                        <td id="tdStep5" runat="server" style="width: 10%; text-align: center">
                            <asp:LinkButton ID="lbStepDownload" runat="server" CssClass="btn btn-info btn-circle btn-lg fa fa-download" Text="" ToolTip="Download Results" OnClick="lbStepDownload_Click"></asp:LinkButton>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="10">
                            <div>
                                <asp:Literal ID="ltrProgress" runat="server"></asp:Literal>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>






