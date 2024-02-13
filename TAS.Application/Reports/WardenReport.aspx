<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WardenReport.aspx.cs" Inherits="Reports_WardenReport" %>
<%@ Register Src="~/Reports/RCWizard/ReportWizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelWizard" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <iframe id="IFrameStep1" scrolling="no" style="border: none; width: 100%; height: 1250px; background-color: #fff" runat="server"></iframe>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelWizard" DisplayAfter="100" DynamicLayout="False">
        <ProgressTemplate>
            <div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
                <div style="padding-top: 200px;">
                    <img alt="Loading" src="../images/ajax_loader_blue_512.gif" width="300px" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>








