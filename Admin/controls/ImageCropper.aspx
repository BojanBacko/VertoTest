<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageCropper.aspx.cs" Inherits="Admin_controls_ImageCropper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/admin/styles/popup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/admin/scripts/jquery-1.11.2.js"></script>
    <script type="text/javascript" src="/admin/scripts/jquery.Jcrop.min.js"></script>
    <link href="/admin/styles/jquery.Jcrop.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    	var width = <%=mainWidth%>;
    	var height = <%=mainHeight%>;

    	jQuery(function ($) {
    		$('#cropImg').Jcrop({
    			//minSize: [width, height],
    			aspectRatio: width / height,
    			onSelect: updateCoords
    		});
    	});

    	function updateCoords(c) {
    		$('#hidX').val(Math.floor(c.x));
    		$('#hidY').val(Math.floor(c.y));
    		$('#hidW').val(Math.ceil(c.w));
    		$('#hidH').val(Math.ceil(c.h));
    		$('#hidOrigW').val($('#cropImg').width());
    		$('#hidOrigH').val($('#cropImg').height());
    	};

    	function checkCoords() {
    		if (parseInt($('#hidW').val())) return true;
    		alert('Please select a crop region then press submit.');
    		return false;
    	};

    	function updateAndClose() {
    		window.parent.$('.Img_' + <%=ContentID%> + '_' + <%=ImageNo%>).attr('src', '<%=ReturnImageFilePath()%>?nocache=<%=DateTime.Now%>');
        	window.parent.$('.CropThumbs_' + <%=ContentID%> + '_' + <%=ImageNo%>).click();
        	window.parent.$.prettyPhoto.close();
        }

        function update() {
        	window.parent.$('.Img_' + <%=ContentID%> + '_' + <%=ImageNo%>).attr('src', '<%=ReturnImageFilePath()%>?nocache=<%=DateTime.Now%>');
        	window.parent.$('.CropThumbs_' + <%=ContentID%> + '_' + <%=ImageNo%>).click();
        }
    </script>
    <title>Image Cropper</title>
	<style>.jcrop-holder {
	background-color: white !important;
}</style>
</head>
<body>
    <form id="form1" runat="server">

		<asp:HiddenField runat="server" ID="hfType" Value="jpg" />

    <div class="popup">

        <h1>Upload a new image</h1>

        <asp:Panel runat="server" ID="pnlStep1">
        
            <h2>Step 1. Upload a new <%= (type == "png" ? "PNG/JPEG" : type.ToUpper()) %> image <%= !string.IsNullOrWhiteSpace(Request.QueryString["name"]) ? " for \"" + Request.QueryString["name"] + "\"" : "" %></h2>

            <asp:Literal runat="server" ID="litStep1Message" />

            <asp:FileUpload runat="server" ID="uplImage" /><br><br>

            <p>Note: Please ensure the image is of good quality and no larger than 5mb. Images must be in <%= (type == "png" ? "PNG/JPEG" : type.ToUpper()) %> format.</p>

        
            <asp:Button runat="server" ID="btnUpload" Text="Go to step 2 &gt;&gt;" CssClass="styleButton centre" OnClick="btnUpload_Click" />

        </asp:Panel>

        <asp:Panel runat="server" ID="pnlStep2" Visible="false">
        
            <h2>Step 2. Crop your image by dragging the box over the top to the correct size. Position the box exactly where you wish to crop the image and select the ‘Crop and save image’ button.</h2>
            
            <asp:Literal runat="server" ID="litStep2Message" />
            
            <asp:Image runat="server" ID="cropImg" style="max-width: 100%;max-height: 600px;"/>
            
            <asp:HiddenField runat="server" ID="hidX" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hidY" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hidW" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hidH" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hidOrigW" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hidOrigH" ClientIDMode="Static" />

            <br>
        
            <asp:Button runat="server" ID="btnCrop" Text="Crop and save image &gt;&gt;" OnClientClick="return checkCoords();" CssClass="styleButton centre" OnClick="btnCrop_Click" />

        </asp:Panel>

        <asp:Panel runat="server" ID="pnlStep3" Visible="false">
        
            <h2>Crop complete</h2>

            <p>Your image has now been cropped. When viewing the image on the website initially you may need to press CTRL+F5.</p>

            <asp:Literal ID="litTest" runat="server"></asp:Literal>
            
            <asp:Button runat="server" ID="btnClose" Text="Close and return to template" OnClientClick="updateAndClose();" CssClass="styleButton centre" />

        </asp:Panel>

    </div>
    </form>
</body>
</html>
