<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="AddPage" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>
<%@ Register Src="~/admin/controls/ImageControl.ascx" TagName="ImageControl" TagPrefix="verto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphBreadcrumb" ID="Content3">
    <a href="/admin/pages.aspx">> Pages</a>
    <a href="/admin/homepage.aspx">> Homepage</a>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="main" class="column">
        <asp:Panel ID="panelMessage" runat="server" Visible="false">
            <h4 id="messagesText" class="alert_info" runat="server"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
         </asp:Panel>
        
        <h2>Homepage </h2>
        <div class="clear"></div>

         <ul class="tabs">
            <li><a href="#tab1">Slider Text</a></li>
            <li><a href="#tab2">Who we are Box</a></li>
            <li><a href="#tab3">Support Box</a></li>
            <li><a href="#tab4">What customers say</a></li>
            <li><a href="#tab5">SEO Meta Data</a></li>
            <li><a href="#tab6">Image Manager</a></li>
        </ul>

        <div class="clear"></div>

        <div class="tab_container">
            <div id="tab1" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_3_quarter">
                    <h3 class="tabs_involved">Slider Text</h3>

                    <Verto:CMS ID="CMS" runat="server" TableName="tblHomepage" IdName="id" SectionTitle="Sub Pages" Mode="Select"
                        BoundColumn1="Name" BoundColumn1Text="Page Name" BoundColumn2="Slug" BoundColumn2Text="URL Name" AllowFiltering="false" AllowOrdering="false" OrderColumn="id" OrderColumnText="Page Order"
                        AddButtonText="Add New Page" SelectButtonEnabled="True" DeleteEnabled="True" InsertEnabled="True" EmptyMessage="There are no sub pages in this section." 
                        ImagesEnabled="False" InsertButtonText="Add New Template" CustomButtonCommandName="PageLayout" CustomButtonName="Select" CustomButtonEnabled="False">
                        <FormFields>
                            <div class="module_content">
                                <fieldset>
                                    <label>Slider Title</label>
                                    <div class="clear"></div>
                                    <CKEditor:CKEditorControl ID="TopTitle" EnterMode="BR" runat="server" Toolbar="Custom" Height="100px"></CKEditor:CKEditorControl>
                                </fieldset>                           
                                <div class="clear"></div>
                                
                                <fieldset>

                                    <label class="ck_title">Slider Text</label>
                                    <div class="clear"></div>
                                    <CKEditor:CKEditorControl ID="TopText" EnterMode="BR" runat="server" Toolbar="Custom"></CKEditor:CKEditorControl>

                                </fieldset>
                
                                <div class="clear"></div>
                                <fieldset>
                                    <label>Slider Link Text</label>
                                    <asp:textBox runat="server" ID="TopLinkText"></asp:textBox>
                                </fieldset> 
                                <fieldset>
                                    <label>Slider Link URL</label>
                                    <asp:textBox runat="server" ID="TopLink"></asp:textBox>
                                </fieldset> 
                                <div class="clear"></div>
                                <fieldset>
                                    <label>Video Code</label>
                                    <asp:textBox runat="server" ID="VideoCode"></asp:textBox>
                                </fieldset> 
                            </div>
                        </FormFields>
                    </Verto:CMS>

                </div>
            </div>

            <div id="tab2" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_3_quarter">
                    <h3 class="tabs_involved">Who we are Box</h3>

                    <Verto:CMS ID="CMS2" runat="server" TableName="tblHomepage" IdName="id" SectionTitle="Sub Pages" Mode="Select"
                        BoundColumn1="Name" BoundColumn1Text="Page Name" BoundColumn2="Slug" BoundColumn2Text="URL Name" AllowFiltering="false" AllowOrdering="false" OrderColumn="id" OrderColumnText="Page Order"
                        AddButtonText="Add New Page" SelectButtonEnabled="True" DeleteEnabled="True" InsertEnabled="True" EmptyMessage="There are no sub pages in this section." 
                        ImagesEnabled="False" InsertButtonText="Add New Template" CustomButtonCommandName="PageLayout" CustomButtonName="Select" CustomButtonEnabled="False">
                        <FormFields>
                            <div class="module_content">
                                <fieldset>
                                    <label>Box 1 Title</label>
                                    <asp:textBox runat="server" ID="Box1Title"></asp:textBox>
                                </fieldset>                           
                                <div class="clear"></div>
                                
                                <fieldset>

                                    <label class="ck_title">Box 1 Text</label>
                                    <div class="clear"></div>
                                    <CKEditor:CKEditorControl ID="Box1Text" EnterMode="BR" runat="server" Toolbar="Custom"></CKEditor:CKEditorControl>

                                </fieldset>
                                <div class="clear"></div>
                                <fieldset>
                                    <label>Box 1 Link Text</label>
                                    <asp:textBox runat="server" ID="Box1LinkText"></asp:textBox>
                                </fieldset> 
                                <fieldset>
                                    <label>Box 1 Link URL</label>
                                    <asp:textBox runat="server" ID="Box1Link"></asp:textBox>
                                </fieldset> 
                            </div>
                        </FormFields>
                    </Verto:CMS>

                </div>
            </div>

            <div id="tab3" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_3_quarter">
                    <h3 class="tabs_involved">Support Box</h3>

                    <Verto:CMS ID="CMS3" runat="server" TableName="tblHomepage" IdName="id" SectionTitle="Sub Pages" Mode="Select"
                        BoundColumn1="Name" BoundColumn1Text="Page Name" BoundColumn2="Slug" BoundColumn2Text="URL Name" AllowFiltering="false" AllowOrdering="false" OrderColumn="id" OrderColumnText="Page Order"
                        AddButtonText="Add New Page" SelectButtonEnabled="True" DeleteEnabled="True" InsertEnabled="True" EmptyMessage="There are no sub pages in this section." 
                        ImagesEnabled="False" InsertButtonText="Add New Template" CustomButtonCommandName="PageLayout" CustomButtonName="Select" CustomButtonEnabled="False">
                        <FormFields>
                            <div class="module_content">
                                <fieldset>
                                    <label>Box 2 Title</label>
                                    <asp:textBox runat="server" ID="Box2Title"></asp:textBox>
                                </fieldset>                           
                                <div class="clear"></div>
                                
                                <fieldset>

                                    <label class="ck_title">Box 2 Text</label>
                                    <div class="clear"></div>
                                    <CKEditor:CKEditorControl ID="Box2Text" EnterMode="BR" runat="server" Toolbar="Custom"></CKEditor:CKEditorControl>

                                </fieldset>
                                <div class="clear"></div>
                                <fieldset>
                                    <label>Box 2 Link Text</label>
                                    <asp:textBox runat="server" ID="Box2LinkText"></asp:textBox>
                                </fieldset> 
                                <fieldset>
                                    <label>Box 2 Link URL</label>
                                    <asp:textBox runat="server" ID="Box2Link"></asp:textBox>
                                </fieldset> 
                            </div>
                        </FormFields>
                    </Verto:CMS>

                </div>
            </div>

            <div id="tab4" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_3_quarter">
                    <h3 class="tabs_involved">What customers say</h3>

                    <Verto:CMS ID="CMS4" runat="server" TableName="tblHomepage" IdName="id" SectionTitle="Sub Pages" Mode="Select"
                        BoundColumn1="Name" BoundColumn1Text="Page Name" BoundColumn2="Slug" BoundColumn2Text="URL Name" AllowFiltering="false" AllowOrdering="false" OrderColumn="id" OrderColumnText="Page Order"
                        AddButtonText="Add New Page" SelectButtonEnabled="True" DeleteEnabled="True" InsertEnabled="True" EmptyMessage="There are no sub pages in this section." 
                        ImagesEnabled="False" InsertButtonText="Add New Template" CustomButtonCommandName="PageLayout" CustomButtonName="Select" CustomButtonEnabled="False">
                        <FormFields>
                            <div class="module_content">
                                <fieldset>
                                    <label>Box 3 Title</label>
                                    <asp:textBox runat="server" ID="Box3Title"></asp:textBox>
                                </fieldset>                           
                                <div class="clear"></div>
                                
                                <fieldset>

                                    <label class="ck_title">Box 3 Text</label>
                                    <div class="clear"></div>
                                    <CKEditor:CKEditorControl ID="Box3Text" EnterMode="BR" runat="server" Toolbar="Custom"></CKEditor:CKEditorControl>

                                </fieldset>
                                <div class="clear"></div>
                                <fieldset>
                                    <label>Box 3 Text</label>
                                    <asp:textBox runat="server" ID="Box3LinkText"></asp:textBox>
                                </fieldset> 
                                <fieldset>
                                    <label>Box 3 URL</label>
                                    <asp:textBox runat="server" ID="Box3Link"></asp:textBox>
                                </fieldset> 
                            </div>
                        </FormFields>
                    </Verto:CMS>

                </div>
            </div>

            <div id="tab5" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_3_quarter">
                    <h3 class="tabs_involved">SEO Meta Data</h3>

                    <Verto:CMS ID="CMS5" runat="server" TableName="tblHomepage" IdName="id" SectionTitle="Sub Pages" Mode="Select"
                        BoundColumn1="Name" BoundColumn1Text="Page Name" BoundColumn2="Slug" BoundColumn2Text="URL Name" AllowFiltering="false" AllowOrdering="false" OrderColumn="id" OrderColumnText="Page Order"
                        AddButtonText="Add New Page" SelectButtonEnabled="True" DeleteEnabled="True" InsertEnabled="True" EmptyMessage="There are no sub pages in this section." 
                        ImagesEnabled="False" InsertButtonText="Add New Template" CustomButtonCommandName="PageLayout" CustomButtonName="Select" CustomButtonEnabled="False">
                        <FormFields>
                            <div class="module_content">
                                <fieldset>
                                    <label>Meta Title</label>
                                    <asp:textBox runat="server" ID="MetaT"></asp:textBox>
                                </fieldset>                  
                                <fieldset>
                                    <label>Meta Description</label>
                                    <asp:textBox runat="server" ID="MetaD" TextMode="MultiLine" CssClass="tarea"></asp:textBox>
                                </fieldset>   
                
                                <div class="clear"></div>
                            </div>
                        </FormFields>
                    </Verto:CMS>

                </div>
            </div>

            <div id="tab6" class="tab_content">                
                <div class="clear"></div>
                <div class="module width_3_quarter">
                    <h3 class="tabs_involved">Image Manager</h3>

                    <Verto:CMS ID="CMS6" runat="server" TableName="tblHomepage" IdName="id" SectionTitle="Sub Pages" Mode="Select" ShowSaveButton="false"
                        BoundColumn1="Name" BoundColumn1Text="Page Name" BoundColumn2="Slug" BoundColumn2Text="URL Name" AllowFiltering="false" AllowOrdering="false" OrderColumn="id" OrderColumnText="Page Order"
                        AddButtonText="Add New Page" SelectButtonEnabled="True" DeleteEnabled="True" InsertEnabled="True" EmptyMessage="There are no sub pages in this section." 
                        ImagesEnabled="False" InsertButtonText="Add New Template" CustomButtonCommandName="PageLayout" CustomButtonName="Select" CustomButtonEnabled="False">
                        <FormFields>
                            <div class="module_content">
                                <asp:textBox Visible="false" runat="server" ID="TopTitle"></asp:textBox>
                                <div class="clear"></div>
                                
                                <verto:ImageControl runat="server" id="id" name="Image One" Width="1900" Height="650" ImgNo="1" CropImage="True" NameOverride="img_1_1" ImagePath="/images/home" />

                            </div>
                        </FormFields>
                    </Verto:CMS>

                </div>
            </div>

        </div>
    </div>
</asp:Content>

