<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= ConfigurationManager.AppSettings["GenericTitle"].ToString()%> | CMS Login</title>
    <link rel="Stylesheet" href="/admin/styles/Login.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
            <div class="top-nav">
                <div class="logo-login cms" onclick="window.location='/'" style="cursor:pointer;" ></div>          
                <div class="elements">                     
                    <div class="login">
                        <asp:Login runat="server" ID="login" OnLoginError="Login_Error">
                            <LayoutTemplate>
                                <span class="red-text">
                                    <asp:Literal ID="FailureText" runat="server" Visible="false"></asp:Literal>
                                </span>
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="grey-text font1-2">
                                            Username
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" CssClass="login" ID="UserName" Width="250"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="grey-text font1-2">
                                            Password
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" CssClass="login" ID="Password" Width="250" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height:30px;">
                                            <asp:LinkButton runat="server" ID="Login" CssClass="submit lc" CommandName="Login" Text="> Login"></asp:LinkButton>
                                            <a href="/" class="btn-back">> Back to homepage</a>
                                        </td>
                                    </tr>
                                </table>    
                            </LayoutTemplate>
                        </asp:Login> 
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
