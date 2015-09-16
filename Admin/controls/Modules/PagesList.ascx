<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PagesList.ascx.cs" Inherits="controls_PagesList" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>

<div class="module width_full">

<%--	<div class="keys">
		<div><div style="background-color:#FFDBDB;"></div>Unpublished (hidden)</div>
	</div>--%>

    <h3 class="tabs_involved">Content Manager</h3>
    
    <Verto:CMS ID="CMS" runat="server" TableName="tblContent" IdName="id" Mode="Grid" SortColumn="id" EnableDragOrdering="true" PublishEnabled="true"
        AddButtonText="Add New Page" InsertEnabled="False" EmptyMessage="There are no sub pages in this section.">
        <Columns>
            <verto:Column DbField="name" LabelText="Title" />
            <verto:Column DbField="slug" LabelText="URL Friendly Title" />
        </Columns>
        <Buttons>
			<verto:CustomButton CommandName="moveup" ButtonType="Image" ImageUrl="/admin/images/move_up.png" CssClass="thin-cell move-up" />
			<verto:CustomButton CommandName="movedown" ButtonType="Image" ImageUrl="/admin/images/move_down.png" CssClass="thin-cell move-down" />
			<verto:CustomButton CommandName="publish" ButtonType="Image" ImageUrl="/admin/images/publish-page.png" CssClass="thin-cell" />
            <verto:CustomButton CommandName="PageLayout" ButtonType="Image" ImageUrl="/admin/images/edit-page.png" CssClass="thin-cell edt" />
            <verto:CustomButton CommandName="DELETE" ButtonType="Image" ImageUrl="/admin/images/delete.png" CssClass="thin-cell del" />
        </Buttons>
        <FormFields>

        </FormFields>
    </Verto:CMS>
</div>