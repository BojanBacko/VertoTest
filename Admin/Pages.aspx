<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="Pages.aspx.cs" Inherits="_Pages" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphBreadcrumb" ID="Content3">
    <a href="/admin/pages.aspx">> Pages</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:SqlDataSource ID="ds" runat="server" SelectCommand="SELECT id, name FROM tblContent WHERE parent = 0" DataSourceMode="DataReader" ConnectionString="<%$ ConnectionStrings:MSSQL %>">

</asp:SqlDataSource>

<div id="main" class="column">
    <h2 class="site_title">Pages</h2>

    <div class="module width_half">
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
                <asp:Repeater runat="server" ID="rptPages" DataSourceID="ds" OnItemCommand="rptPages_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("name") %></td>
                            <td>
                                <asp:ImageButton runat="server" ID="btnEdit1" ImageUrl="/admin/images/edit-page.png" CommandArgument='<%#Eval("id") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    
</div>
        
</asp:Content>

