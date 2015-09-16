<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SeoMetaData.ascx.cs" Inherits="Admin_controls_Layout_PageContent" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/admin/controls/AdminControl.ascx" TagName="CMS" TagPrefix="verto" %>

<div class="module width_full">

    <Verto:CMS ID="CMS" runat="server" IdName="id" Mode="select" TableName="tblContent">
        <FormFields>
            <h3>Edit page meta data</h3>
            <div class="module_content">

				If you're not quite sure what this page is for, we have an FAQ article <a href="http://scripts.vertouk.com/FAQ/Help.html#search:meta" class="ep">here</a>.<br /><br />

                <fieldset>
                    <label>Meta Title</label>
                    <asp:textBox runat="server" ID="metaT"></asp:textBox>
                </fieldset>

                <fieldset>
                    <label>Meta Description <span>(This should be no more than 150 characters, you've used<span id='count-left'>-1</span>)</span></label>
                    <asp:TextBox runat="server" ID="metaD" CssClass="tarea" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                </fieldset>
                
                <div class="clear"></div>

            </div>
        </FormFields>
    </Verto:CMS>

</div>

<script type="text/javascript">
	$(function () {
		$('#metaD').on('keydown keyup', function (e) {
			$('#count-left').html($(this).val().length);
		}).trigger('keyup');
	});
</script>