<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="Templates.aspx.cs" Inherits="Settings_Templates" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('#content').hide();
            $('#content').prev().children('.toggleLink').text('Show');
            $('#settings').show();
            $('#settings').prev().children('.toggleLink').text('Hide');
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadcrumb" Runat="Server">
    <a href="/admin/settings/">> Module Manager</a>
    <a href="/admin/settings/templates.aspx">> Template Manager</a>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="main" class="column">
        <asp:Panel ID="panelMessage" runat="server" Visible="false">
            <h4 id="messagesText" class="alert_info" runat="server"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
         </asp:Panel>
        <h2>Template Manager</h2>

            <div class="clear"></div>

             <%--<ul class="tabs">
                <li><a href="#tab1">Sub Pages</a></li>
                <li><a href="#tab2">Page Content</a></li>
                <li><a href="#tab3">Meta Data</a></li>
                <li><a href="#tab4">Image Manager</a></li>
            </ul>
            <div class="clear"></div>--%>
        
        <ul class="tabs">
            <li><a href="#tab1">Template Manager</a></li>
        </ul>   
        <div class="clear"></div>
        
        <div class="tab_container">
                <div id="tab1" class="tab_content">
                    
                <div class="module">

                        <Verto:CMS ID="CMS" runat="server" TableName="tblTemplates" IdName="id" Mode="Grid" AddType="Inline" AddButtonText="Add New Page" InsertEnabled="True" 
                           EmptyMessage="There are no sub pages in this section." AddPageDestinationURL="/admin/settings/templates" AddPageDestinationType="StayOnPage" InsertButtonText="Add New Template">
                            <Columns>
                                <verto:Column DbField="Name" LabelText="Template Name" />
                            </Columns>

                            <Buttons>
                                <verto:CustomButton CommandName="Select" CssClass="edt" ButtonType="Image" ImageUrl="/admin/images/edit-page.png" />
                                <verto:CustomButton CommandName="Delete" CssClass="del" ButtonType="Image" ImageUrl="/admin/images/delete.png" />
                            </Buttons>

                            <FormFields>
                                <div class="module_content">
                                    <fieldset>
                                        <label>Template Title</label>
                                        <asp:textBox runat="server" ID="name"></asp:textBox>
                                    </fieldset>                                    
                                    <div class="clear"></div>
                                </div>
                            </FormFields>
                        </Verto:CMS>

                        
                    </div>
                                    
                    <div class="clear"></div>
                </div>
                      
        </div>
               
    </div>    
</asp:Content>

