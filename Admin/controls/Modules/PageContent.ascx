<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageContent.ascx.cs" Inherits="Admin_controls_Layout_PageContent" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>


<div class="module width_full">

    <Verto:CMS ID="CMS" runat="server" TableName="tblContent" IdName="id" Mode="Select">
        <FormFields>
            <h3>Edit Page</h3>
            <div class="module_content">
                <%--<fieldset>
                    <label>Publish?</label>
                    <asp:CheckBox runat="server" ID="published" />
                </fieldset>--%>
                <fieldset>
                    <label>Page Title</label>
                    <asp:TextBox runat="server" ID="name"></asp:TextBox>
                    <asp:TextBox runat="server" ID="slug" Visible="false"></asp:TextBox>
                </fieldset>
                <div class="clear"></div>
                <fieldset>
                    <label>Content</label>
                    <CKEditor:CKEditorControl ID="description" EnterMode="BR" runat="server" Toolbar="Custom"></CKEditor:CKEditorControl>
                </fieldset>

                <div class="clear"></div>
            </div>
        </FormFields>
    </Verto:CMS>
</div>