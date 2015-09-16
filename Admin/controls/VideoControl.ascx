<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VideoControl.ascx.cs" Inherits="Admin_controls_VideoControl" %>

<asp:Panel runat="server" ID="pnlError" CssClass="alert red" Visible="false"><asp:Literal runat="server" ID="litError" /></asp:Panel>

<asp:Panel runat="server" ID="pnlControl" CssClass="control-container video-control">

	<span class="title">
		<%= Title ?? "Video" %>
		<asp:Panel runat="server" ID="pnlHelp" CssClass="ep" data-ep="http://scripts.vertouk.com/FAQ/Help.html#search:video" style="float:right;font-weight:normal;">(?)</asp:Panel>
	</span>
	<div class="image-control">
		<div style="position:relative;">
			<asp:Panel runat="server" ID="pnlIcon" style="position:absolute;top:-6px;width:100%;height:100%;z-index:50;background: url('/admin/images/video-control-icon.png') no-repeat center;" />
			<asp:Image runat="server" ID="imgPreview" ImageUrl="http://img.vertouk.com/426x240/222D3A/222D3A/222D3A/?logo=&showres=false" style="margin-bottom:12px;" />
			<div style="clear:both;"></div>
		</div>
		<div class="btn">
			<asp:HyperLink runat="server" ID="HyperLink1" CssClass="btnEdit video-edit" ToolTip="Edit video URL" />
			<asp:LinkButton runat="server" ID="lnkDelete" CssClass="btnDelete" ToolTip="Reset video URL" OnClick="lnkDelete_Click" style="margin-left:15px;" />
		</div>
	</div>

	<asp:TextBox runat="server" ID="tbUrl" CssClass="tb-url" style="display:none;" />

	<asp:Button runat="server" ID="btnSubmit" CssClass="btn-save" Text="Save" OnClick="btnSubmit_Click" style="display:none;" />

</asp:Panel>

<script>
	var VCBound;
	$(function () {
		!VCBound && $('body').on('click', '.video-edit', function () {
			var t = $(this).parents('.control-container').find('.tb-url'),
				b = $(this).parents('.control-container').find('.btn-save');

			ep.prompt({
				title: "Video URL", content: "<span class='youtube-result'>Enter the YouTube or Vimeo URL below.</span>", initial: t.val() || '',
				'finally': function (r) {
					var m = r.match(/.*(?:youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=)([^#\&\?]*).*/),
						m2 = r.match(/https?:\/\/(?:www\.|player\.)?vimeo.com\/(?:channels\/(?:\w+\/)?|groups\/([^\/]*)\/videos\/|album\/(\d+)\/video\/|video\/|)(\d+)(?:$|\/|\?)/);
					if ((m && m[1]) || (m2 && m2[3])) {
						t.val(r);
						b.trigger('click');
					}
					else {
						$('.youtube-result').html("<span style='color:#900'>Please enter a valid YouTube or Vimeo URL.</span>");
						return false;
					}
				}
			});
		});
		VCBound = true;
	});
</script>