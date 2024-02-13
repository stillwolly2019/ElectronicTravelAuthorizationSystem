<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TAsByWBS.aspx.cs" Inherits="Reports_TAsByWBS" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <rsweb:ReportViewer ID="RV" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                ProcessingMode="Remote" Width="100%" ShowCredentialPrompts="False"
                ShowDocumentMapButton="False" ShowExportControls="False" ShowFindControls="False"
                ShowPageNavigationControls="False" SizeToReportContent="false">
            </rsweb:ReportViewer>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

