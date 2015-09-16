<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsArticles.ascx.cs" Inherits="controls_NewsArticles" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>
<%@ Register Src="~/admin/controls/ImageControl.ascx" TagName="ImageControl" TagPrefix="verto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div class="module width_3_quarter">
    <h3 class="tabs_involved">News</h3>

    <Verto:CMS ID="CMS" runat="server" TableName="tblContent" IdName="id" Mode="Grid" ValidationGroup="NewsArticleContent" AddType="Inline" InsertEnabled="True" AddButtonText="Add New Article" AddPageDestinationType="Redirect"
        EmptyMessage="There are no sub pages in this section." SortColumn="DatePublished" SortDirection="DESC">
        <Columns>
            <verto:Column DbField="name" LabelText="Title" />
            <verto:Column DbField="datePublished" LabelText="Date" FormatString="{0:dd/MM/yyyy}" />
        </Columns>
        <Buttons>
            <verto:CustomButton CommandName="Select" CssClass="edt" ButtonType="Image" ImageUrl="/admin/images/edit-page.png" />
            <verto:CustomButton CommandName="Delete" CssClass="del" ButtonType="Image" ImageUrl="/admin/images/delete.png" />
        </Buttons>

        <FormFields>
            <div class="module_content">                
                <div class="clear"></div>
                <fieldset>
                    <label>Published?</label>
                    <asp:CheckBox runat="server" ID="published" />
                </fieldset>                                
                <fieldset>
                    <label>Date Published</label>
                    <asp:TextBox runat="server" ID="datePublished" MaxLength="10" />
                    <asp:CalendarExtender runat="server" TargetControlID="datePublished" Format="dd/MM/yyyy"></asp:CalendarExtender>
                    <asp:RequiredFieldValidator runat="server" ID="rfv" ControlToValidate="datePublished" ErrorMessage="Please enter a date." ValidationGroup="NewsArticleContent" CssClass="error" />
                </fieldset> 
                <fieldset>
                    <label>Article Title</label>
                    <asp:TextBox runat="server" ID="name"></asp:TextBox>
                    <asp:TextBox runat="server" ID="slug" Visible="false"></asp:TextBox>         
                    <asp:Literal runat="server" ID="pageTemplate" Text="1" Visible="false" />        
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="name" ErrorMessage="Please enter an article title." ValidationGroup="NewsArticleContent" CssClass="error" />
                </fieldset> 
                <div class="clear"></div>
                <fieldset>
                    <label>Article Content</label>
                    <CKEditor:CKEditorControl ID="description" EnterMode="BR" runat="server" Toolbar="Custom"></CKEditor:CKEditorControl>
                </fieldset>

                <verto:ImageControl runat="server" id="id" ImageNo="1" Title="Article page image" Width="540" Height="461" Method="CropResizeAndSave" Editor="true" ImagePath="/images/content">
                    <Thumbs>
                        <verto:Thumb Width="540" Height="394" Method="CropResizeAndSave" />
                        <verto:Thumb Width="280" Height="210" Method="CropResizeAndSave" />
                    </Thumbs>
                </verto:ImageControl>

                <div class="clear"></div>

            </div>
        </FormFields>
    </Verto:CMS>

</div>