<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Admin_Users_Roles" %>

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
    <a href="/admin/users/rolemanager.aspx">> Manage Roles</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="main" class="column">
        <asp:Panel ID="panelMessage" runat="server" Visible="false">
            <h4 id="messagesText" class="alert_info" runat="server"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
            </asp:Panel>

        <h2>Users</h2>
        <div class="clear"></div>

            <ul class="tabs">
            <li><a href="#tab1">Role List</a></li>
            <li><a href="#tab2">Create New Role</a></li>
        </ul>

        <div class="clear"></div>

        <div class="tab_container">
            <div id="tab1" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_half">
                    <h3 class="tabs_involved">Role List</h3>
                    <div class="module_content"> 
                            <asp:GridView ID="UsersList" DataSource='<%#Roles.GetAllRoles()%>' GridLines="None" runat="server" EmptyDataText="There a currently no roles setup"
                                AutoGenerateColumns="false" CssClass="gv" AlternatingRowStyle-CssClass="alt" AllowPaging="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Role Name">
                                        <ItemTemplate>
                                            <%#Container.DataItem%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="RemoveRole" runat="server" Text="Remove Role" OnCommand="RemoveRole_Command" CommandArgument='<%#Container.DataItem%>' OnClientClick="javascript:return confirm('Are you sure you want to delete this role from the system?')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="tab2" class="tab_content">                
                    <div class="clear"></div>
                    <div class="module width_half">
                        <h3 class="tabs_involved">Add new role</h3>
                        <div class="module_content"> 
                                <p style="color:#f00;"><asp:Literal runat="server" ID="litError" /></p>                                
                                <fieldset>
                                    <label>Role Name</label>
                                    <asp:TextBox runat="server" ID="txtRoleName" />
                                </fieldset>
                            
                                <fieldset>
                                    <asp:Button runat="server" ID="AddRole" OnClick="AddRole_Click" Text="Add Role" />
                                </fieldset>
                            </div>
                        </div>
                    </div>
            </div>
        </div>

</asp:Content>

