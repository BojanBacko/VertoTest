<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Tier.aspx.cs" Inherits="Tier" %>

<%@ Register Src="~/controls/Layout/Tier.ascx" TagName="Tier" TagPrefix="verto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:SqlDataSource runat="server" ID="ds" SelectCommand="SELECT c.*, (SELECT TOP 1 name FROM tblContent WHERE tblContent.id = c.parent) AS ParentName FROM tblContent AS c WHERE c.slug = @slug AND published = 1;" DataSourceMode="DataReader" ConnectionString="<%$ConnectionStrings:MSSQL %>"></asp:SqlDataSource>
    
    <asp:PlaceHolder runat="server" ID="phLayout" />

</asp:Content>

