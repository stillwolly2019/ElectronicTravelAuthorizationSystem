<%@ Page Title="View Security Clearence" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewPDF.aspx.cs" Inherits="TravelAuthorization_ViewPDF" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title"> Security Clearence</h4>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <iframe id="IFramePDF" scrolling="no" style="border: none; width: 100%; min-height: 1050px; background-color: #fff" frameborder="0" toolbar="0" runat="server"></iframe>
                            </div>
                        </div>
                    </div>
                    
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

