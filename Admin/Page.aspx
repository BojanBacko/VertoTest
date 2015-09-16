<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="Page.aspx.cs" Inherits="Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
    	$(document).ready(function () {
    		// $("#messagesText").fadeOut(7000, function () { $("#panelMessage").slideUp(2000); }).delay(2000);
    		setTimeout(function () {
    			$('#messagesText').fadeOut(150);
    		}, 5000);
    	});
    </script>

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphBreadcrumb" ID="Content3">
    <a href="/admin/pages.aspx">> Pages</a>
    <asp:Literal runat="server" ID="litBreadcrumb" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:SqlDataSource runat="server" ID="ds" SelectCommand="SELECT * FROM tblContent WHERE id = @id" DataSourceMode="DataReader" ConnectionString="<%$ConnectionStrings:MSSQL %>">
    <SelectParameters>
        <asp:QueryStringParameter Name="id" QueryStringField="id" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource runat="server" ID="dsModules" SelectCommand="SELECT ml.*, m.* FROM tblModuleLink AS ml INNER JOIN tblModule AS m ON m.ID = ml.ModuleID WHERE ml.TemplateID = @TemplateID ORDER BY m.ModuleOrder" DataSourceMode="DataReader" ConnectionString="<%$ConnectionStrings:MSSQL %>">
    <SelectParameters>
        <asp:Parameter Name="TemplateID" />
    </SelectParameters>
</asp:SqlDataSource>

<div id="main" class="column">
    <asp:Panel ID="panelMessage" ClientIDMode="Static" runat="server" Visible="false" Height="40px">
        <h4 id="messagesText" class="alert_info" runat="server" clientidmode="Static"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
    </asp:Panel>

    <asp:Panel ID="panelMessageError" ClientIDMode="Static" runat="server" Visible="false">
        <h4 id="ErrorMessagesText" class="alert_info" runat="server"><asp:Label ID="lblMessageError" runat="server" Text=""></asp:Label></h4>
    </asp:Panel>

    <h2><asp:Literal runat="server" ID="litTitle" /></h2>
    <div class="clear"></div>

        <ul class="tabs">
        <%--<li><a href="#tab5">Sub Pages</a></li>
        <li><a href="#tab2">Page Content</a></li>
        <li><a href="#tab3">Meta Data</a></li>
        <li><a href="#tab4">Image Manager</a></li>--%>
        <asp:Repeater runat="server" ID="rptTabs" OnItemDataBound="rptTabs_ItemDataBound">
            <ItemTemplate>
                <asp:PlaceHolder runat="server" ID="phLi">
                    <li><a href="#tab<%#Container.ItemIndex + 1 %>"><%#Eval("ModuleName")%></a></li>
                </asp:PlaceHolder>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="clear"></div>
        
    <div class="tab_container">

        <asp:PlaceHolder runat="server" ID="phModules"></asp:PlaceHolder>
                       
    </div>

    <%--<h4 class="alert_error">An Error Message</h4>

    <h4 class="alert_success">A Success Message</h4>--%>
</div>
</asp:Content>

