<%@ Page Title="Users Roles" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UsersRoles.aspx.cs" Inherits="Admin_UsersRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Users Roles</h1>
        </div>
    </div>
    <div class="row">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="col-lg-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Assign by User
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label>User</label>
                                        <asp:DropDownList ID="DDLUsers" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DDLUsers_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label>Roles</label>
                                        <asp:ListBox ID="lstRoles" runat="server" CssClass="form-control" SelectionMode="Multiple" Height="200px">
                                        </asp:ListBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Label ID="lblmsgUser" runat="server">&nbsp;</asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div style="padding-top: 10px">
                                        <div style="padding-bottom: 5px">
                                            <asp:Button ID="btnClearUser" runat="server" Text="Clear" CssClass="form-control btn-warning" OnClick="btnClearUser_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div style="padding-top: 10px">
                                        <div style="padding-bottom: 5px">
                                            <asp:Button ID="btnSaveUser" runat="server" Text="Save" CssClass="form-control btn-success" OnClick="btnSaveUser_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Assign by Role
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label>Role</label>
                                        <asp:DropDownList ID="DDLRoles" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DDLRoles_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label>Users</label>
                                        <asp:ListBox ID="lstUsers" runat="server" CssClass="form-control" SelectionMode="Multiple" Height="200px">
                                        </asp:ListBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Label ID="lblmsgRole" runat="server">&nbsp;</asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div style="padding-top: 10px">
                                        <div style="padding-bottom: 5px">
                                            <asp:Button ID="btnClearRole" runat="server" Text="Clear" CssClass="form-control btn-warning" OnClick="btnClearRole_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div style="padding-top: 10px">
                                        <div style="padding-bottom: 5px">
                                            <asp:Button ID="btnSaveRole" runat="server" Text="Save" CssClass="form-control btn-success" OnClick="btnSaveRole_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

