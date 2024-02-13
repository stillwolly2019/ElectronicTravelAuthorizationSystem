<%@ Page Title="Login" Language="C#" MasterPageFile="~/LoginMaster.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div style="text-align: center; margin-top:-50px">
        <h3 style="color: rgb(0, 97, 170);">Travel Authorization and Settlement System
        </h3>
    </div>
    <div class="container">
        <div class="row" style="padding-top:50px">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <%--<div class="panel-heading">
                        <h3 class="panel-title">
                            <asp:Label ID="lblP1" runat="server" Text="Please Sign In" meta:resourcekey="lblP1Resource1"></asp:Label>
                        </h3>
                    </div>--%>
                    <div class="panel-body" style="text-align:center;border:1px solid black; border-radius:5px 5px">
                        <img id="RSC" src="images/IOMLogo.png" alt="IOM" style="width: 250px;" />
                        <hr />
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtUName" CssClass="form-control" BorderColor="Black" placeholder="User Name" autofocus meta:resourcekey="txtUNameResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill a user name" ControlToValidate="txtUName" ForeColor="Red" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                            <%--<input class="form-control" placeholder="E-mail" name="email" type="email" autofocus>--%>
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" BorderColor="Black" placeholder="Password" TextMode="Password" autofocus meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please fill a Password" ControlToValidate="txtPassword" ForeColor="Red" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                            <%--<input class="form-control" placeholder="Password" name="password" type="password" value="">--%><br />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Invalid Username or Password" meta:resourcekey="lblErrorResource1"></asp:Label>
                        </div>
                      <%--  <div class="form-group">
                            <a href="ForgotPassword.aspx"><asp:Label ID="lblP2" runat="server" Text="I forgot my password" meta:resourcekey="lblP2Resource1"></asp:Label></a>
                        </div>--%>
                        <!-- Change this to a button or input when using this as a form -->
                        <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-lg btn-primary btn-block" Text="Login" OnClick="btnLogin_Click" meta:resourcekey="btnLoginResource1" />
                        <div style="font-size: 12px; padding: 5px;">
                            <p>
                                <asp:Label ID="lblDisclaimer" runat="server" ></asp:Label>
                            </p>
                        </div>
                        <%--<asp:LinkButton ID="LinkButton1" CssClass="btn btn-lg btn-success btn-block" runat="server">Login</asp:LinkButton>--%>                        <%--<a href="index.html" class="btn btn-lg btn-success btn-block">Login</a>--%>
                        <asp:Label ID="Label1" runat="server" Visible="false" meta:resourcekey="Label1Resource1"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>