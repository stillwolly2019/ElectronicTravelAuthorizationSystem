<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DispatchWizardHeader.ascx.cs" Inherits="Reports_RCWizard_DispatchWizardHeader" %>
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

<asp:UpdatePanel ID="UpdatePanelMR" runat="server">
    <ContentTemplate>
       <div class="row" id="StatusDiv" runat="server">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
