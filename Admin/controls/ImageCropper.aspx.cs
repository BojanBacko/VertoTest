using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Last change: Shane -> VIE -> 1.0.9

public partial class Admin_controls_ImageCropper : System.Web.UI.Page
{

	public int ContentID = 0;
	public string ImageNo = "0";

	public string Dimensions = "";

	public string mainHeight, mainWidth;

	public string UnsizedWidth, UnsizedHeight;
	public string imgWidth, imgHeight;

	public string CropImage = "";
	public string ImagePath = "";

	public bool WatermarkEnabled = false;
	public string WatermarkUrl = "";
	public string WatermarkPosition = "";

	public string type = "jpg";

	public string Method = null;
	public string BackColour = "#FFF";

	protected void Page_Load(object sender, EventArgs e)
	{
		Dimensions = Server.UrlDecode(Request.QueryString["dimensions"]);
		ImagePath = Server.UrlDecode(Request.QueryString["ImagePath"]);
		ContentID = Convert.ToInt32(Request.QueryString["id"]);
		ImageNo = Request.QueryString["ImageNo"];

		WatermarkEnabled = Convert.ToBoolean(Request.QueryString["watermark"]);
		WatermarkUrl = Request.QueryString["watermarkurl"].ToString();
		WatermarkPosition = Request.QueryString["watermarkpos"].ToString();

		imgWidth = Request.QueryString.AllKeys.Contains("width") && !string.IsNullOrEmpty(Request.QueryString["width"]) ? Server.UrlDecode(Request.QueryString["width"]) : "800";
		imgHeight = Request.QueryString.AllKeys.Contains("height") && !string.IsNullOrEmpty(Request.QueryString["height"]) ? Server.UrlDecode(Request.QueryString["height"]) : "500";

		UnsizedWidth = !string.IsNullOrEmpty(Request.QueryString["uwidth"]) ? Server.UrlDecode(Request.QueryString["uwidth"]) : "960";
		UnsizedHeight = !string.IsNullOrEmpty(Request.QueryString["uheight"]) ? Server.UrlDecode(Request.QueryString["uheight"]) : "650";

		string[] arrDimensions = Dimensions.Split(',')[0].Split('x');
		mainWidth = arrDimensions[0];
		mainHeight = arrDimensions[1];

		if (!string.IsNullOrWhiteSpace(Request.Form["vie-type"]) && Request.Form["vie-type"].ToString() == "image/png")
			type = "png";
		else if (!string.IsNullOrWhiteSpace(Request.QueryString["type"]))
			type = (Request.QueryString["type"].ToString() == "png" ? "png" : "jpg");

		if (File.Exists(Server.MapPath(ReturnImageFilePath("unsized"))))
		{
			cropImg.ImageUrl = ReturnImageFilePath("unsized") + "?nocache=" + DateTime.Now.ToString();
		}

		if (!string.IsNullOrWhiteSpace(Request.QueryString["backColour"]))
			BackColour = Request.QueryString["backColour"];

		//VIE: Pass the new cropper through the standard imaging process
		if (!string.IsNullOrWhiteSpace(Request.Form["data-uri"]))
		{
			hidX.Value = Request.Form["vie-offset-x"].ToString();
			hidY.Value = Request.Form["vie-offset-y"].ToString();
			hidW.Value = Request.Form["vie-width"].ToString();
			hidH.Value = Request.Form["vie-height"].ToString();
			hidOrigH.Value = Request.Form["vie-original-height"].ToString();
			hidOrigW.Value = Request.Form["vie-original-width"].ToString();
			if (!string.IsNullOrWhiteSpace(Request.Form["vie-method"]))
				Method = Request.Form["vie-method"].ToString();
			btnUpload_Click(null, null);
			btnCrop_Click(null, null);
		}
	}
	protected void btnUpload_Click(object sender, EventArgs e)
	{
		bool doneUpload = false;

		if (uplImage.HasFile)
		{
			if (System.IO.Path.GetExtension(uplImage.FileName).ToString().ToLower() == ".jpg" || System.IO.Path.GetExtension(uplImage.FileName).ToString().ToLower() == ".jpeg" || (type == "png" && System.IO.Path.GetExtension(uplImage.FileName).ToString().ToLower() == ".png"))
			{
				ImagingProcess cropper = new ImagingProcess();

				uplImage.SaveAs(Server.MapPath(ReturnImageFilePath("original")));
				//cropper.ResizeLimit(uplImage.FileContent, ReturnImageFilePath("original"), 960, 650);
				cropper.ResizeLimitWidth(uplImage.FileContent, ReturnImageFilePath("unsized"), Convert.ToInt32(UnsizedWidth), type == "png", WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				doneUpload = true;
				if (File.Exists(Server.MapPath(ReturnImageFilePath("original"))))
					cropImg.ImageUrl = ReturnImageFilePath("original") + "?nocache=" + DateTime.Now.ToString();
			}
		}
		//VIE: Save the raw file
		else if (!string.IsNullOrWhiteSpace(Request.Form["data-uri"]))
		{
			File.WriteAllBytes(Server.MapPath(ReturnImageFilePath("original")), Convert.FromBase64String(HttpUtility.UrlDecode(Request["data-uri"].ToString()).Replace(' ', '+')));
			doneUpload = true;
		}

		hfType.Value = type;

		//Response.Write(ReturnImageFilePath("unsized"));

		if (doneUpload)
		{
			pnlStep1.Visible = false;
			pnlStep2.Visible = true;
			//Page.ClientScript.RegisterStartupScript(GetType(), "MyScript", "<script>" +
			//    "$(document).ready(function(){" +
			//        "var height = $(document).height();var width = $(document).width();  " +
			//        "$('#pp_full_res', parent.document).children('iframe').width(width + 40);" +
			//        "$('#pp_full_res', parent.document).children('iframe').height(height + 110);" +
			//    "})" +
			//    "</script>");
		}
		else
		{
			if (File.Exists(Server.MapPath(ReturnImageFilePath("original"))))
			{
				cropImg.ImageUrl = ReturnImageFilePath("original") + "?nocache=" + DateTime.Now.ToString();
				pnlStep1.Visible = false;
				pnlStep2.Visible = true;
			}
			else
				litStep1Message.Text = "<p class=\"red\">Please upload a JPG image</p>";
		}
	}

	protected void btnCrop_Click(object sender, EventArgs e)
	{
		type = hfType.Value;

		string[] arrImage = Dimensions.Split(',');
		string[] arrDimensions = arrImage[0].Split('x');

		if (Method == "ResizeDownNoWhiteSpace")
		{
			arrDimensions[0] = hidW.Value;
			arrDimensions[1] = hidH.Value;
			mainWidth = hidW.Value;
			mainHeight = hidH.Value;
		}

		string strLoadPath = ReturnImageFilePath("original");
		System.Drawing.Image imgToCrop = Bitmap.FromFile(Server.MapPath(strLoadPath));

		int x = Convert.ToInt32(hidX.Value);
		int y = Convert.ToInt32(hidY.Value);
		int h = Convert.ToInt32(hidH.Value);
		int w = Convert.ToInt32(hidW.Value);

		// fix minus positions in jcrop
		if (x == -1) { x = 0; w = w - 1; }
		if (y == -1) { y = 0; h = h - 1; }

		int origW = Convert.ToInt32(hidOrigW.Value);
		int origH = Convert.ToInt32(hidOrigH.Value);
		decimal scalingRatio = Convert.ToDecimal(imgToCrop.Width) / origW;
		decimal newW = (decimal)w * scalingRatio;
		decimal newH = (decimal)h * scalingRatio;
		decimal newX = (decimal)x * scalingRatio;
		decimal newY = (decimal)y * scalingRatio;

		string strSavePath = ReturnImageFilePath();

		try
		{

			Bitmap btmap = new Bitmap(Convert.ToInt32(mainWidth), Convert.ToInt32(mainHeight), imgToCrop.PixelFormat);
			Graphics graphcs = Graphics.FromImage(btmap);
			if (type == "png")
				graphcs.Clear(Color.Transparent);
			else
				graphcs.Clear(ColorTranslator.FromHtml(BackColour));
			graphcs.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphcs.SmoothingMode = SmoothingMode.HighQuality;
			graphcs.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphcs.CompositingQuality = CompositingQuality.HighQuality;
			graphcs.DrawImage(imgToCrop, new Rectangle(0, 0, Convert.ToInt32(mainWidth), Convert.ToInt32(mainHeight)), new Rectangle((int)Math.Ceiling(newX), (int)Math.Ceiling(newY), (int)Math.Floor(newW), (int)Math.Floor(newH)), GraphicsUnit.Pixel);

			// UNCOMMENT THE NEXT 4 LINES TO GET A READOUT OF VALUES ON THE IMAGE
			//Font fnt = new Font("Arial", 16F, FontStyle.Bold);
			//SolidBrush b3 = new SolidBrush(ColorTranslator.FromHtml("#FFFFFF"));
			//PointF kp1 = new PointF(0F, 0F);
			//graphcs.DrawString("X/Y: " + x + "x" + y + Environment.NewLine + "W/H: " + w + "x" + h + Environment.NewLine + "NewX/Y: " + Math.Ceiling(newX) + "x" + Math.Ceiling(newY) + Environment.NewLine + "NewW/H: " + Math.Floor(newW) + "x" + Math.Floor(newH) + Environment.NewLine + "scalingRatio: " + scalingRatio, fnt, b3, kp1);

			//litTest.Text = "new Rectangle(0, 0, " + Convert.ToInt32(mainWidth) + ", " + Convert.ToInt32(mainHeight) + ") / new Rectangle(" + (int)Math.Floor(newX) + ", " + (int)Math.Floor(newY) + ", " + (int)Math.Ceiling(newW) + ", " + (int)Math.Ceiling(newH) + ")";

			MemoryStream MemStream = new MemoryStream();
			ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
			EncoderParameters Params = new EncoderParameters(2);
			Params.Param[0] = new EncoderParameter(Encoder.Quality, Convert.ToInt32(100L));
			if (type == "png")
			{
				Params.Param[1] = new EncoderParameter(Encoder.ColorDepth, Convert.ToInt32(32L));
				btmap.Save(MemStream, Info[4], Params);
			}
			else
			{
				Params.Param[1] = new EncoderParameter(Encoder.ColorDepth, Convert.ToInt32(24L));
				btmap.Save(MemStream, Info[1], Params);
			}

			ImagingProcess cropper = new ImagingProcess();

			cropper.CropResizeAndSave(MemStream, strSavePath, Convert.ToInt32(arrDimensions[0]), Convert.ToInt32(arrDimensions[1]), type == "png", WatermarkEnabled, WatermarkUrl, WatermarkPosition);
			cropper.CropResizeAndSave(MemStream, ReturnImageFilePath("nowatermark"), Convert.ToInt32(arrDimensions[0]), Convert.ToInt32(arrDimensions[1]), type == "png");

			foreach (string s in arrImage)
			{
				string[] arrOtherDimensions = s.Split('x');
				if (s != arrImage[0])
				{
					cropper.CropResizeAndSave(MemStream, ReturnImageFilePath(Width: arrOtherDimensions[0], Height: arrOtherDimensions[1]), Convert.ToInt32(arrOtherDimensions[0]), Convert.ToInt32(arrOtherDimensions[1]), type == "png", WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				}
			}

			MemStream.Dispose();
			btmap.Dispose();
			graphcs.Dispose();
			imgToCrop.Dispose();

			//if(strLoadPath == ReturnImageFilePath("unsized"))
			//    File.Delete(Server.MapPath(strLoadPath));
			Page.ClientScript.RegisterStartupScript(GetType(), "MyScript", "<script>update();</script>");

			pnlStep2.Visible = false;
			pnlStep3.Visible = true;
		}
		catch (OutOfMemoryException exOOM)
		{

			litStep2Message.Text = "<span style=\"color: #FF0000; font-weight: bold; font-size: 14px;\">Error: Plese check your image is in RGB format and not using 32-bit colour palette.</span>";
			imgToCrop.Dispose();
		}
		catch (Exception ex)
		{

			litStep2Message.Text = "<span style=\"color: #FF0000; font-weight: bold; font-size: 14px;\">Error: An unspecified error occured.</span>";
			imgToCrop.Dispose();

		}
	}

	public string ReturnImageFilePath(string ImageType = "", string Width = "", string Height = "")
	{
		string[] arrImage = Dimensions.Split(',');
		if (ImageType == "original" || ImageType == "nowatermark")
		{
			if (string.IsNullOrWhiteSpace(ImageNo) || ImageNo == "0")
				return string.Format("{0}/{1}_{2}." + type, ImagePath, ContentID, ImageType);
			else
				return string.Format("{0}/{1}_{2}_{3}." + type, ImagePath, ContentID, ImageNo, ImageType);
		}
		else
		{
			if (ImageType != "" && !string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
				return string.Format("{0}/{1}_{2}_{3}." + type, ImagePath, ContentID, ImageNo, ImageType);

			else if (ImageType != "")
				return string.Format("{0}/{1}_{2}." + type, ImagePath, ContentID, ImageType);

			else if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0" && Width != "" && Height != "")
				return string.Format("{0}/{1}_{2}_{3}x{4}." + type, ImagePath, ContentID, ImageNo, Width, Height);

			else if (Width != "" && Height != "")
				return string.Format("{0}/{1}_{2}x{3}." + type, ImagePath, ContentID, Width, Height);

			else if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
				return string.Format("{0}/{1}_{2}_{3}." + type, ImagePath, ContentID, ImageNo, arrImage[0]);

			else
				return string.Format("{0}/{1}_{2}." + type, ImagePath, ContentID, arrImage[0]);
		}
	}
}