<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImageManager.ascx.cs" Inherits="Admin_controls_Layout_ImageManager" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>
<%@ Register Src="~/admin/controls/ImageControl.ascx" TagName="ImageControl" TagPrefix="verto" %>

<div class="module width_full">

    <Verto:CMS ID="CMS" runat="server" TableName="tblContent" IdName="id" Mode="Select">
        <FormFields>

                <h3>Edit Page Images</h3>
                <div class="module_content">
                    
                    <div class="clear"></div>

                        <verto:ImageControl runat="server" id="id_3" ImageNo="1" Title="Article page image" Width="435" Height="350" Method="CropResizeAndSave" Editor="true" ImagePath="/images/content">
                            <Thumbs>
                                <verto:Thumb Width="415" Height="250" Method="CropResizeAndSave" />
                                <verto:Thumb Width="100" Height="100" Method="Resize" />
                            </Thumbs>
                        </verto:ImageControl>

                    <div class="clear"></div>

                </div>
        </FormFields>
    </Verto:CMS>
</div>