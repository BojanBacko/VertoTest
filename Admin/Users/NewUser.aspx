<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="NewUser.aspx.cs" Inherits="Admin_Users_NewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $('#content').hide();
            $('#content').prev().children('.toggleLink').text('Show');
            $('#users').show();
            $('#users').prev().children('.toggleLink').text('Hide');
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBreadcrumb" Runat="Server">
    <a href="/admin/users/">> Users</a>
    <a href="/admin/users/newuser.aspx">> Add New User</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="main" class="column">
        <asp:Panel ID="panelMessage" runat="server" Visible="false">
            <h4 id="messagesText" class="alert_info" runat="server"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
            </asp:Panel>

        <h2>Users</h2>
        <div class="clear"></div>

            <ul class="tabs">
            <li><a href="#tab1">New User</a></li>
        </ul>

        <div class="clear"></div>

        <div class="tab_container">
            <div id="tab1" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_half">
                    <h3 class="tabs_involved">Add new user</h3>
                    <div class="module_content"> 
                            <p style="color:#f00;"><asp:Literal runat="server" ID="litError" /></p>                                
                            <fieldset>
                                <label>Username</label>
                                <asp:TextBox runat="server" ID="txtUsername" />
                            </fieldset>
                            <fieldset>
                                <label>Email</label>
                                <asp:TextBox runat="server" ID="txtEmail" />
                            </fieldset>
                            <fieldset>
                                <label>Password</label>
                                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" />
                            </fieldset>
                            <fieldset>
                                <label>Confirm Password</label>
                                <asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" />
                            </fieldset>
                            <fieldset>
                                <label>User Role</label>
                                
                                <asp:DropDownList runat="server" ID="ddlRole" AppendDataBoundItems="true">
                                    <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                </asp:DropDownList>

                            </fieldset>
                            
                            <fieldset>
                                <asp:Button runat="server" ID="btnAddUser" OnClick="btnAddUser_Click" Text="Add User" />
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>

</asp:Content>

