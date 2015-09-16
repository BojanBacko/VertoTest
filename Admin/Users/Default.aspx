<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Users_Default" %>

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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
           <div id="main" class="column">
                <asp:Panel ID="panelMessage" runat="server" Visible="false">
                    <h4 id="messagesText" class="alert_info" runat="server"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
                 </asp:Panel>

                <h2>Users</h2>
                <div class="clear"></div>

                 <ul class="tabs">
                    <li><a href="#tab1">Users</a></li>
                </ul>

                <div class="clear"></div>

                <div class="tab_container">
                    <div id="tab1" class="tab_content">                
                        <div class="clear"></div>
                        <div class="module width_quarter">
                            <h3 class="tabs_involved">filter by role</h3>
                            <div class="module_content">                
                                <fieldset>
                                    <label>Please select a role from the list below</label>
                                    <asp:DropDownList runat="server" DataSource='<%#Roles.GetAllRoles()%>' ID="AllRoles" AutoPostBack="true" AppendDataBoundItems="true">
                                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                    </asp:DropDownList>
                                </fieldset>
                                
                                </div>
                            </div>
                            <div class="module width_half">
                                <h3 class="tabs_involved">users list</h3>
                                <div class="module_content">  
                                    <asp:GridView ID="UsersList" DataSource='<%#GetUsers()%>' runat="server" GridLines="None" EmptyDataText="There are no users in the selected role."
                                        AutoGenerateColumns="false" CssClass="gv" AlternatingRowStyle-CssClass="alt" DataKeyNames="username" AllowPaging="false">
                                        <Columns>
                                            <asp:BoundField DataField="Username" HeaderText="Username" />
                                            <asp:TemplateField HeaderText="Assign to Role">
                                                <ItemTemplate>
                                                    <asp:Repeater runat="server" ID="UsersList" DataSource='<%#GetRoles(Eval("UserName").ToString())%>'>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button2" runat="server" Text='<%#GetText(Eval("key").ToString(), Eval("value").ToString())%>' OnCommand="Toggle" CommandArgument='<%#String.Format("{0},{1}", Eval("key"), Eval("value"))%>' /><br />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove User">
                                                <ItemTemplate>
                                                    <asp:Button ID="Button1" runat="server" Text='Remove User' OnCommand="Remove_Command" CommandArgument='<%#Eval("UserName")%>' OnClientClick="javascript:return confirm('Are you sure you want to delete this user from the system?')" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
               </div>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

