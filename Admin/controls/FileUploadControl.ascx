<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileUploadControl.ascx.cs" Inherits="Admin_controls_FileUploadControl" %>

<%--<script type="text/javascript">
    $(document).ready(function () {
        $("#messagesText").fadeOut(7000, function () { $("#panelMessage").slideUp(2000); }).delay(2000);
    });
</script>--%>

<div class="control-container">

    <h4><%=Title %></h4>

    <div class="clear"></div>

    <asp:Panel ID="panelMessage" ClientIDMode="Static" runat="server" Visible="false" Height="30px">
        <h4 id="messagesText" class="green" runat="server" clientidmode="Static">Successfully uploaded!</h4>
    </asp:Panel>

    <div class="clear"></div>

    <asp:Literal runat="server" ID="litError" />

    <div class="clear"></div>

    <asp:Panel runat="server" ID="pnlFileUpload" class="image-control">

        <div class="fu">

            <asp:FileUpload runat="server" ID="fu"  />

        </div>

        <div>

            <p class="image-dimensions"><asp:Literal runat="server" ID="litAllowedFiles" /></p>

        </div>

        <div class="btn">

            <asp:Panel runat="server" ID="pnlUploadButton" CssClass="upload">
                <asp:LinkButton runat="server" ID="btnUpload" Text="Upload" OnClick="btnUpload_Click" />
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlViewButton" CssClass="view">
                <asp:HyperLink runat="server" Target="_blank" ID="lnkViewCurrentFile" Text="View Current File" />
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlDeleteButton" CssClass="delete">
                <asp:LinkButton runat="server" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you certain you want to delete this file?');" />
            </asp:Panel>

            <div class="clear"></div>

        </div>

    </asp:Panel>


</div>
