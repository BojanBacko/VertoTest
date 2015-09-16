<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImageControl.ascx.cs" Inherits="controls_ImageControl" %>
<%--<script type="text/javascript">
    function cropthumbs(x, y) {
        $('.CropThumbs_' + x + '_' + y).click();
    }
</script>--%>

<%--<asp:UpdatePanel runat="server">
    <ContentTemplate>--%>
        <div class="control-container">
            <span class="title">
                <asp:Literal runat="server" ID="litTitle" /></span>
				<span class="ep" data-ep="http://scripts.vertouk.com/FAQ/Help.html#search:image" style="float:right;margin-top:-18px;">(?)</span>
            <asp:Panel runat="server" ID="pnlUpload" CssClass="image-control">
                <div>
					<div style="position:absolute;margin: 5px 0 0 5px;border-radius:3px;background-color:rgba(0,0,0,.5);padding:3px 5px;color:#FFF;pointer-events:none;" class="dims"><asp:Literal runat="server" ID="litDims" /></div>
                    <asp:Image ID="Image1" runat="server" CssClass="main-img" />
                </div>
                <div class="btn">
                    <asp:PlaceHolder runat="server" ID="phFileUpload" Visible="false">
                        <p>Please make sure you click the save icon to save your image, otherwise your changes will be discarded.</p>

                        <asp:FileUpload runat="server" ID="fu" CssClass="fu" />
                    </asp:PlaceHolder>

                    <div class="upload">
                        <asp:HyperLink runat="server" ID="lnkUpload" CssClass="btnEdit" ToolTip="Edit main image"></asp:HyperLink>
                        <asp:LinkButton runat="server" ID="btnUpload" CssClass="btnSave" ToolTip="Edit main image" Visible="false" OnClick="btnUpload_Click"></asp:LinkButton>

                        <asp:UpdatePanel runat="server" ID="upCropThumbs">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="btnCropThumbs" CssClass="crop-thumbs" Style="display: none;" OnClick="btnCropThumbs_Click" Text="crop thumbs" /></ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="delete">
                        <asp:LinkButton runat="server" CssClass="btnDelete" ID="btnDelete" Style="margin-left: 15px;" ToolTip="Delete images" OnClick="btnDelete_Click"></asp:LinkButton>
                    </div>

					<span style="display:inline-block;position:relative;overflow:visible;">
						<asp:HyperLink runat="server" ID="lnkEditThumbs" CssClass="btnThumbs" ToolTip="Edit thumbnails" Visible="false" />
						<asp:Panel runat="server" ID="pnlThumbs" CssClass="vie-thumb-list" Visible="false"><asp:Literal runat="server" ID="litThumbs" /></asp:Panel>
					</span>

                    <div class="clear"></div>

                    <asp:PlaceHolder runat="server" ID="phThumbs" />
                </div>
            </asp:Panel>
            <asp:Literal runat="server" ID="litError" />
        </div>
  <%--  </ContentTemplate>
</asp:UpdatePanel>--%>

