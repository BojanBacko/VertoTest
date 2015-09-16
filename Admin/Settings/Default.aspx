<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Settings_Default" %>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:SqlDataSource ID="ds" runat="server" DataSourceMode="DataReader" ConnectionString="<%$ ConnectionStrings:MSSQL %>" 
        SelectCommand="SELECT * FROM tblTemplates WHERE ID = @ID" InsertCommand="INSERT INTO tblModuleLink (ModuleID, TemplateID, Active) VALUES (@ModuleID, @TemplateID, 1)"
        DeleteCommand="DELETE FROM tblModuleLink WHERE ModuleID = @ModuleID and TemplateID = @TemplateID" DeleteCommandType="Text">
        <SelectParameters>
            <asp:QueryStringParameter Name="ID" QueryStringField="id" DefaultValue="1" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="TemplateID" DefaultValue="1" />
            <asp:Parameter Name="ModuleID" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="TemplateID" DefaultValue="1" />
            <asp:Parameter Name="ModuleID" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsSeriesOptions" runat="server" SelectCommand="SELECT * FROM tblModuleLink WHERE TemplateID = @TemplateID" DataSourceMode="DataReader" ConnectionString="<%$ ConnectionStrings:MSSQL %>">
        <SelectParameters>
            <asp:Parameter Name="TemplateID" /> 
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsModules" runat="server" SelectCommand="SELECT ModuleName, DescriptiveName, ID as ModuleID FROM tblModule Order BY moduleOrder" 
        InsertCommand="INSERT INTO tblModule (ModuleName, ModuleFile, DescriptiveName, ModuleOrder) VALUES (@ModuleName, @ModuleFile, @DescriptiveName, @ModuleOrder)" DataSourceMode="DataReader" ConnectionString="<%$ ConnectionStrings:MSSQL %>">
        <InsertParameters>
            <asp:ControlParameter Name="ModuleName" ControlID="txtModuleName" PropertyName="Text" />
            <asp:ControlParameter Name="DescriptiveName" ControlID="txtDescriptiveName" PropertyName="Text" />
            <asp:ControlParameter Name="ModuleFile" ControlID="txtModuleFile" PropertyName="Text" />
            <asp:ControlParameter Name="ModuleOrder" ControlID="txtModuleOrder" PropertyName="Text" />
        </InsertParameters>
    </asp:SqlDataSource>


<div id="main" class="column">
    <asp:Panel ID="panelMessage" runat="server" Visible="false">
        <h4 id="messagesText" class="alert_info" runat="server"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
     </asp:Panel>
        <h2>Module Manager</h2>
        <div class="clear"></div>
        
         <ul class="tabs">
            <li><a href="#tab1">Assign Modules</a></li>
            <li><a href="#tab2">New Module</a></li>
            <li><a href="#tab3">Edit Modules</a></li>
        </ul>
        <div class="clear"></div>

        <div class="tab_container">
            <div id="tab1" class="tab_content">

                <div class="module ">
                    <h3>Select a Page Template</h3>
                    <div class="module_content">
                        <fieldset style="float: left; margin-right: 3%;">
                            <!-- to make two field float next to one another, adjust values -->
                            <label>Template</label>
                            <asp:DropDownList runat="server" ID="ddlTemplate" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" AutoPostBack="true" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
                        </fieldset>
                        <div class="clear"></div>
                    </div>
                </div>

                <div class="clear"></div>
                
                <div class="module">
                    <h3>Assign a module to the template</h3>
                    <div class="module_content">
                        <asp:GridView runat="server" ID="gvModules" DataSourceID="dsModules" GridLines="None" OnRowDataBound="gvModules_RowDataBound" DataKeyNames="ModuleID" AutoGenerateColumns="false" CssClass="gv" AlternatingRowStyle-CssClass="alt">
                            <Columns>
                                <asp:BoundField DataField="DescriptiveName" HeaderText="Module Name" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" AutoPostBack="true" OnCheckedChanged='AssignSeries' />
                                    </ItemTemplate>            
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
            

            <div id="tab2" class="tab_content">
                <div class="module">
                    <h3>Add New Module</h3>
                    <div class="module_content">
                        <fieldset>
                            <label>Module Title (Tab Title)</label>
                            <asp:TextBox runat="server" ID="txtModuleName" />
                        </fieldset>
                        <fieldset>
                            <!-- to make two field float next to one another, adjust values -->
                            <label>Module File(e.g. Content.ascx)</label>
                            <asp:TextBox runat="server" ID="txtModuleFile" />
                        </fieldset>
                        
                        <fieldset>
                            <!-- to make two field float next to one another, adjust values -->
                            <label>Descriptive Name (e.g. Landing Page)</label>
                            <asp:TextBox runat="server" ID="txtDescriptiveName" />
                        </fieldset>
                        
                        <fieldset>
                            <!-- to make two field float next to one another, adjust values -->
                            <label>Module Order</label>
                            <asp:TextBox runat="server" ID="txtModuleOrder" />
                        </fieldset>
                        <div class="clear"></div>
                    </div>
                    <div class="buttons">
                        <asp:Button ID="btnAddModule" Text="Add Module" OnClick="btnAddModule_Click" runat="server" CssClass="btn-submit" style="margin-left:20px;" />
                    </div>
                </div>
            </div>

            <div id="tab3" class="tab_content">
                <div class="module ">
                    <h3>Add New Module</h3>
                    <asp:UpdatePanel runat="server" ID="up1" RenderMode="Inline">
                        <ContentTemplate>

                            <Verto:CMS ID="CMS0" runat="server" TableName="tblModule" IdName="id" SectionTitle="Sub Pages" Mode="Grid" AddButtonText="Add New Page" InsertEnabled="False" EmptyMessage="There are no sub pages in this section.">
                                <Columns>
                                    <verto:Column DbField="ModuleName" LabelText="Module Name" />
                                    <verto:Column DbField="ModuleFile" LabelText="Filename"/>
                                    <verto:Column DbField="DescriptiveName" LabelText="Descriptive Name"/>
                                </Columns>

                                <Buttons>
                                    <verto:CustomButton CommandName="Select" CssClass="edt" ButtonType="Image" ImageUrl="/admin/images/edit-page.png" />
                                    <verto:CustomButton CommandName="Delete" CssClass="del" ButtonType="Image" ImageUrl="/admin/images/delete.png" />
                                </Buttons>

                                <FormFields>

                                    <div class="module_content">
                                        <fieldset>
                                            <label>Module Title (Tab Title)</label>
                                            <asp:TextBox runat="server" ID="ModuleName" />
                                        </fieldset>
                                        <fieldset>
                                            <label>Module File(e.g. Content.ascx)</label>
                                            <asp:TextBox runat="server" ID="ModuleFile" />
                                        </fieldset>
                        
                                        <fieldset>
                                            <label>Descriptive Name (e.g. Landing Page)</label>
                                            <asp:TextBox runat="server" ID="DescriptiveName" />
                                        </fieldset>
                        
                                        <fieldset>
                                            <label>Module Order</label>
                                            <asp:TextBox runat="server" ID="ModuleOrder" />
                                        </fieldset>
                                        <div class="clear"></div>
                                    </div>
                                </FormFields>
                            </Verto:CMS>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                        <div class="clear"></div>
                    </div>

                </div>
            </div>

            <div class="clear"></div>
        </div>
        
<%--        <h4 class="alert_error">An Error Message</h4>

        <h4 class="alert_success">A Success Message</h4>--%>
</div>
</asp:Content>

