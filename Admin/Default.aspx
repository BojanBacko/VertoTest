<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:SqlDataSource ID="ds" runat="server" DataSourceMode="DataReader" ConnectionString="<%$ ConnectionStrings:MSSQL %>">

</asp:SqlDataSource>

<div id="main" class="column">
    <h2 class="site_title">Welcome to the <%= ConfigurationManager.AppSettings["GenericTitle"].ToString()%> Content Management System</h2>

    <p style="padding-left:35px;">
        In case you encounter any problems using the CMS, please Contact Us by telephone on +44 (0)1536 411153 or by <a style="color:#90133B;" href="mailto:<%= CmsSettings.SupportEmail.ToString()%>">email</a>
    </p>

    <asp:Panel runat="server" ID="pnlAdd" class="module width_quarter">
        <h3 class="tabs_involved">Main Sections</h3>
    
        <table class="gv" cellspacing="0">
            <thead>
                <tr>
                    <th>Page Title</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Homepage</td>
                    <td>
                        <asp:ImageButton runat="server" ID="btnEdit" ImageUrl="/admin/images/edit-page.png" OnClick="btnEdit_Click" />
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rptPages" OnItemCommand="rptPages_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval(CmsSettings.TitleField) %></td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnEdit1" ImageUrl="/admin/images/edit-page.png" CommandArgument='<%#Eval(CmsSettings.IdField) %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </asp:Panel>
    
    
</div>
        
</asp:Content>

