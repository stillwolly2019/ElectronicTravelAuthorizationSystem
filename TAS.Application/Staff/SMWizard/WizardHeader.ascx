<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WizardHeader.ascx.cs" Inherits="Staff_SMWizard_WizardHeader" %>

<asp:UpdatePanel ID="UpdatePanelTA" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-lg-12">
                <h4 class="page-header">
                    <asp:Label ID="lblTitle" Text="Staff Information Management" runat="server"></asp:Label>
                </h4>
                <asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <table style="width: 100%">

                    <%--<tr>
                    <td id="tdStep1" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep1" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-book" Text=" " ToolTip="Personal Information" OnClick="lbStep1_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep2" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep2" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-group" Text=" " ToolTip="Staff Category" OnClick="lbStep2_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep3" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep3" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-phone" Text=" " ToolTip="Staff Contacts" OnClick="lbStep3_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep4" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep4" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-sitemap" Text=" " ToolTip="Staff Location" OnClick="lbStep4_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep5" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep5" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-ambulance" Text=" " ToolTip="Emergency Contacts" OnClick="lbStep5_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep6" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep6" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-download" Text=" " ToolTip="Download" OnClick="lbStep6_Click"></asp:LinkButton>
                    </td>

                    </tr>--%>


                    <tr>
                    <td id="tdStep1" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep1" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-book" Text=" " ToolTip="Personal Information" OnClick="lbStep1_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep2" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep2" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-phone" Text=" " ToolTip="Staff Contacts" OnClick="lbStep2_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep3" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep3" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-sitemap" Text=" " ToolTip="Staff Location" OnClick="lbStep3_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep4" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep4" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-ambulance" Text=" " ToolTip="Emergency Contacts" OnClick="lbStep4_Click"></asp:LinkButton>
                    </td>

                    <td id="tdStep5" runat="server" style="width: 10%; text-align: center">
                    <asp:LinkButton ID="lbStep5" runat="server" CssClass="btn btn-primary btn-circle btn-lg fa fa-download" Text=" " ToolTip="Download" OnClick="lbStep5_Click"></asp:LinkButton>
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








