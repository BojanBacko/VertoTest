using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Web.UI;

// Last change: Shane -> VIE -> 1.0.7

/// <summary>
/// Version v2.3.0 - KarlTech Inc. (c)
/// </summary>
public class ImagingProcess
{

	public void CropResizeAndSave(Stream stream, string RealtivePathAndFileName, int NewWidth, int NewHeight, bool MaintainTransparency = false, bool WaterMarkImage = false, string WatermarkURL = "", string WatermarkPosition = "")
	{
		int x = 0;
		int y = 0;
		_Save(_Crop(_Resize(stream, NewWidth, NewHeight, ref x, ref y, MaintainTransparency), NewWidth, NewHeight, ref x, ref y, MaintainTransparency), RealtivePathAndFileName, WaterMarkImage, WatermarkURL, WatermarkPosition);
	}

	public void Resize(Stream stream, string RealtivePathAndFileName, int NewWidth, int NewHeight, bool MaintainTransparency = false, bool WaterMarkImage = false, string WatermarkURL = "", string WatermarkPosition = "")
	{
		int x = 0;
		int y = 0;
		_Save(_Resize(stream, NewWidth, NewHeight, ref x, ref y, MaintainTransparency), RealtivePathAndFileName, WaterMarkImage, WatermarkURL, WatermarkPosition);
	}

	public void ResizeLimit(Stream stream, string RealtivePathAndFileName, int NewWidth, int NewHeight, bool MaintainTransparency = false, bool WaterMarkImage = false, string WatermarkURL = "", string WatermarkPosition = "")
	{
		_Save(_ResizeLimit(stream, NewWidth, NewHeight, MaintainTransparency), RealtivePathAndFileName, WaterMarkImage, WatermarkURL, WatermarkPosition);
	}

	public void ResizeLimitWidth(Stream stream, string relativePathAndFileName, int NewWidth, bool MaintainTransparency = false, bool WaterMarkImage = false, string WatermarkURL = "", string WatermarkPosition = "")
	{
		_Save(_ResizeLimitWidth(stream, NewWidth, MaintainTransparency), relativePathAndFileName, WaterMarkImage, WatermarkURL, WatermarkPosition);
	}

	public void ResizeLimitWhiteSpace(Stream stream, string relativePathAndFileName, int NewWidth, int NewHeight, bool MaintainTransparency = false, string BackgroundColour = "#FFFFFF", bool WaterMarkImage = false, string WatermarkURL = "", string WatermarkPosition = "")
	{
		_Save(_ResizeLimitWhiteSpace(stream, NewWidth, NewHeight, MaintainTransparency, BackgroundColour), relativePathAndFileName, WaterMarkImage, WatermarkURL, WatermarkPosition);
	}
	/// <summary>
	/// Resize the image to fit within the required dimensions.  Whitespace is added, no cropping is performed, image is not altered if smaller than required size.
	/// </summary>
	public void ResizeDownWhiteSpace(Stream stream, string relativePathAndFileName, int NewWidth, int NewHeight, bool MaintainTransparency = false, string BackgroundColour = "#FFFFFF", bool WaterMarkImage = false, string WatermarkURL = "", string WatermarkPosition = "")
	{
		_Save(_ResizeDownWhiteSpace(stream, NewWidth, NewHeight, MaintainTransparency, BackgroundColour), relativePathAndFileName, WaterMarkImage, WatermarkURL, WatermarkPosition);
	}
	/// <summary>
	/// Resize the image to fit within the required dimensions.  No whitespace is added, no cropping is performed, image is not altered if smaller than required size.
	/// </summary>
	public void ResizeDownNoWhiteSpace(Stream stream, string relativePathAndFileName, int NewWidth, int NewHeight, bool MaintainTransparency = false, string BackgroundColour = "#FFFFFF", bool WaterMarkImage = false, string WatermarkURL = "", string WatermarkPosition = "")
	{
		_Save(_ResizeDownNoWhiteSpace(stream, NewWidth, NewHeight, MaintainTransparency, BackgroundColour), relativePathAndFileName, WaterMarkImage, WatermarkURL, WatermarkPosition);
	}

	/************Imaging related functions*********/
	Bitmap _ResizeLimitWidth(Stream stream, int NewWidth, bool MaintainTransparency)
	{
		Image Img = Image.FromStream(stream);

		try
		{
			var orientation = (int)Img.GetPropertyItem(274).Value[0];

			switch (orientation)
			{
			case 1:
				// No rotation required.
				break;
			case 2:
				Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
				break;
			case 3:
				Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
				break;
			case 4:
				Img.RotateFlip(RotateFlipType.Rotate180FlipX);
				break;
			case 5:
				Img.RotateFlip(RotateFlipType.Rotate90FlipX);
				break;
			case 6:
				Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
				break;
			case 7:
				Img.RotateFlip(RotateFlipType.Rotate270FlipX);
				break;
			case 8:
				Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
				break;
			}
			// This EXIF data is now invalid and should be removed.
			Img.RemovePropertyItem(274);
		}
		catch { }

		decimal intWidth = Img.Width;
		decimal intHeight = Img.Height;
		intHeight = (NewWidth / intWidth) * intHeight;
		Bitmap ImgResize = new Bitmap(NewWidth, Convert.ToInt32(intHeight), PixelFormat.Format32bppArgb);
		ImgResize.SetResolution(72, 72);
		Graphics GraphicResize = Graphics.FromImage(ImgResize);
		if (MaintainTransparency == true) GraphicResize.Clear(Color.Transparent); else GraphicResize.Clear(Color.White);
		GraphicResize.InterpolationMode = InterpolationMode.HighQualityBicubic;
		GraphicResize.SmoothingMode = SmoothingMode.HighQuality;
		GraphicResize.PixelOffsetMode = PixelOffsetMode.HighQuality;
		GraphicResize.CompositingQuality = CompositingQuality.HighQuality;
		GraphicResize.DrawImage(Img, -1, -1, NewWidth + 2, Convert.ToInt32(intHeight) + 2);
		return ImgResize;
	}

	Bitmap _Resize(Stream stream, int NewWidth, int NewHeight, ref int x, ref int y, bool MaintainTransparency)
	{
		Image Img = Image.FromStream(stream);

		try
		{
			var orientation = (int)Img.GetPropertyItem(274).Value[0];

			switch (orientation)
			{
			case 1:
				// No rotation required.
				break;
			case 2:
				Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
				break;
			case 3:
				Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
				break;
			case 4:
				Img.RotateFlip(RotateFlipType.Rotate180FlipX);
				break;
			case 5:
				Img.RotateFlip(RotateFlipType.Rotate90FlipX);
				break;
			case 6:
				Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
				break;
			case 7:
				Img.RotateFlip(RotateFlipType.Rotate270FlipX);
				break;
			case 8:
				Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
				break;
			}
			// This EXIF data is now invalid and should be removed.
			Img.RemovePropertyItem(274);
		}
		catch { }

		decimal intWidth = Img.Width;
		decimal intHeight = Img.Height;
		decimal targetRatio = (decimal)((double)NewWidth / (double)NewHeight);
		if ((intWidth / intHeight) > targetRatio)
		{
			intWidth = Convert.ToInt32((NewHeight / intHeight) * intWidth);
			intHeight = NewHeight;
			x = (int)Math.Ceiling(((double)(intWidth - NewWidth) / 2));
		}
		else if ((intWidth / intHeight) < targetRatio)
		{
			intHeight = Convert.ToInt32((NewWidth / intWidth) * intHeight);
			intWidth = NewWidth;
			y = (int)Math.Ceiling(((double)(intHeight - NewHeight) / 2));
		}
		else
		{
			intWidth = NewWidth;
			intHeight = NewHeight;
		}
		Bitmap ImgResize = new Bitmap(Convert.ToInt32(intWidth), Convert.ToInt32(intHeight), PixelFormat.Format32bppArgb);
		ImgResize.SetResolution(72, 72);
		Graphics GraphicResize = Graphics.FromImage(ImgResize);
		if (MaintainTransparency == true) GraphicResize.Clear(Color.Transparent); else GraphicResize.Clear(Color.White);
		GraphicResize.InterpolationMode = InterpolationMode.HighQualityBicubic;
		GraphicResize.SmoothingMode = SmoothingMode.HighQuality;
		GraphicResize.PixelOffsetMode = PixelOffsetMode.HighQuality;
		GraphicResize.CompositingQuality = CompositingQuality.HighQuality;
		GraphicResize.DrawImage(Img, -1, -1, Convert.ToInt32(intWidth) + 2, Convert.ToInt32(intHeight) + 2);
		return ImgResize;
	}

	Bitmap _ResizeLimit(Stream stream, int NewWidth, int NewHeight, bool MaintainTransparency)
	{
		Image Img = Image.FromStream(stream);

		try
		{
			var orientation = (int)Img.GetPropertyItem(274).Value[0];

			switch (orientation)
			{
			case 1:
				// No rotation required.
				break;
			case 2:
				Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
				break;
			case 3:
				Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
				break;
			case 4:
				Img.RotateFlip(RotateFlipType.Rotate180FlipX);
				break;
			case 5:
				Img.RotateFlip(RotateFlipType.Rotate90FlipX);
				break;
			case 6:
				Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
				break;
			case 7:
				Img.RotateFlip(RotateFlipType.Rotate270FlipX);
				break;
			case 8:
				Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
				break;
			}
			// This EXIF data is now invalid and should be removed.
			Img.RemovePropertyItem(274);
		}
		catch { }

		decimal intWidth = Img.Width;
		decimal intHeight = Img.Height;
		intHeight = (intHeight * NewWidth) / intWidth;
		intWidth = NewWidth;
		if (intHeight > NewHeight)
		{
			intWidth = (intWidth * NewHeight) / intHeight;
			intHeight = NewHeight;
		}
		else
		{
			intHeight = Convert.ToInt32((NewWidth / intWidth) * intHeight);
		}
		Bitmap ImgResize = new Bitmap(Convert.ToInt32(intWidth), Convert.ToInt32(intHeight), PixelFormat.Format32bppArgb);
		ImgResize.SetResolution(72, 72);
		Graphics GraphicResize = Graphics.FromImage(ImgResize);
		if (MaintainTransparency == true) GraphicResize.Clear(Color.Transparent); else GraphicResize.Clear(Color.White);
		GraphicResize.InterpolationMode = InterpolationMode.HighQualityBicubic;
		GraphicResize.SmoothingMode = SmoothingMode.HighQuality;
		GraphicResize.PixelOffsetMode = PixelOffsetMode.HighQuality;
		GraphicResize.CompositingQuality = CompositingQuality.HighQuality;
		GraphicResize.DrawImage(Img, -1, -1, Convert.ToInt32(intWidth) + 2, Convert.ToInt32(intHeight) + 2);
		return ImgResize;
	}

	Bitmap _ResizeLimitWhiteSpace(Stream stream, int NewWidth, int NewHeight, bool MaintainTransparency, string BackgroundColour)
	{

		Image Img = Image.FromStream(stream);

		try
		{
			var orientation = (int)Img.GetPropertyItem(274).Value[0];

			switch (orientation)
			{
			case 1:
				// No rotation required.
				break;
			case 2:
				Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
				break;
			case 3:
				Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
				break;
			case 4:
				Img.RotateFlip(RotateFlipType.Rotate180FlipX);
				break;
			case 5:
				Img.RotateFlip(RotateFlipType.Rotate90FlipX);
				break;
			case 6:
				Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
				break;
			case 7:
				Img.RotateFlip(RotateFlipType.Rotate270FlipX);
				break;
			case 8:
				Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
				break;
			}
			// This EXIF data is now invalid and should be removed.
			Img.RemovePropertyItem(274);
		}
		catch { }

		decimal intWidth = Img.Width;
		decimal intHeight = Img.Height;

		decimal x = 0;
		decimal y = 0;
		decimal scaledWidth;
		decimal scaledHeight;
		float targetRatio = (float)NewWidth / (float)NewHeight;

		if (((float)intWidth / (float)intHeight) < targetRatio)
		{
			//---- TRIGGERED IF SOURCE FILE IS TALLER RATIO THAN NEW IMAGE
			scaledWidth = intWidth / (intHeight / NewHeight);
			scaledHeight = NewHeight;
			x = (NewWidth - scaledWidth) / 2;
		}
		else if (((float)intWidth / (float)intHeight) > targetRatio)
		{
			//---- TRIGGERED IF SOURCE FILE IS WIDER RATIO THAN NEW IMAGE
			scaledWidth = NewWidth;
			scaledHeight = intHeight / (intWidth / NewWidth);
			y = (NewHeight - scaledHeight) / 2;
		}
		else
		{
			//---- TRIGGERED IF SOURCE FILE IS EQUAL RATIO TO NEW IMAGE
			scaledWidth = NewWidth;
			scaledHeight = NewHeight;
		}

		Bitmap ImgResize = new Bitmap(NewWidth, NewHeight, PixelFormat.Format32bppArgb);
		ImgResize.SetResolution(72, 72);
		Graphics GraphicResize = Graphics.FromImage(ImgResize);
		if (MaintainTransparency == true) GraphicResize.Clear(Color.Transparent); else GraphicResize.Clear(ColorTranslator.FromHtml(BackgroundColour));
		GraphicResize.InterpolationMode = InterpolationMode.HighQualityBicubic;
		GraphicResize.SmoothingMode = SmoothingMode.HighQuality;
		GraphicResize.PixelOffsetMode = PixelOffsetMode.HighQuality;
		GraphicResize.CompositingQuality = CompositingQuality.HighQuality;
		GraphicResize.DrawImage(Img, new Rectangle(Convert.ToInt32(x) - 1, Convert.ToInt32(y) - 1, Convert.ToInt32(scaledWidth) + 2, Convert.ToInt32(scaledHeight) + 2), new Rectangle(0, 0, Convert.ToInt32(intWidth), Convert.ToInt32(intHeight)), GraphicsUnit.Pixel);

		return ImgResize;

	}

	Bitmap _ResizeDownWhiteSpace(Stream stream, int NewWidth, int NewHeight, bool MaintainTransparency, string BackgroundColour)
	{

		Image Img = Image.FromStream(stream);

		try
		{
			var orientation = (int)Img.GetPropertyItem(274).Value[0];

			switch (orientation)
			{
			case 1:
				// No rotation required.
				break;
			case 2:
				Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
				break;
			case 3:
				Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
				break;
			case 4:
				Img.RotateFlip(RotateFlipType.Rotate180FlipX);
				break;
			case 5:
				Img.RotateFlip(RotateFlipType.Rotate90FlipX);
				break;
			case 6:
				Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
				break;
			case 7:
				Img.RotateFlip(RotateFlipType.Rotate270FlipX);
				break;
			case 8:
				Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
				break;
			}
			// This EXIF data is now invalid and should be removed.
			Img.RemovePropertyItem(274);
		}
		catch { }

		decimal intWidth = Img.Width;
		decimal intHeight = Img.Height;

		decimal x = 0;
		decimal y = 0;
		decimal scaledWidth;
		decimal scaledHeight;
		float targetRatio = (float)NewWidth / (float)NewHeight;

		if (intWidth > NewWidth || intHeight > NewHeight)
		{
			if (((float)intWidth / (float)intHeight) < targetRatio)
			{
				//---- TRIGGERED IF SOURCE FILE IS TALLER RATIO THAN NEW IMAGE
				scaledWidth = intWidth / (intHeight / NewHeight);
				scaledHeight = NewHeight;
				x = (NewWidth - scaledWidth) / 2;
			}
			else if (((float)intWidth / (float)intHeight) > targetRatio)
			{
				//---- TRIGGERED IF SOURCE FILE IS WIDER RATIO THAN NEW IMAGE
				scaledWidth = NewWidth;
				scaledHeight = intHeight / (intWidth / NewWidth);
				y = (NewHeight - scaledHeight) / 2;
			}
			else
			{
				//---- TRIGGERED IF SOURCE FILE IS EQUAL RATIO TO NEW IMAGE
				scaledWidth = NewWidth;
				scaledHeight = NewHeight;
			}
		}
		else
		{
			scaledWidth = intWidth;
			scaledHeight = intHeight;
			x = Math.Floor((NewWidth - scaledWidth) / 2);
			y = Math.Floor((NewHeight - scaledHeight) / 2);
		}

		Bitmap ImgResize = new Bitmap(NewWidth, NewHeight, PixelFormat.Format32bppArgb);
		ImgResize.SetResolution(72, 72);
		Graphics GraphicResize = Graphics.FromImage(ImgResize);
		if (MaintainTransparency == true) GraphicResize.Clear(Color.Transparent); else GraphicResize.Clear(ColorTranslator.FromHtml(BackgroundColour));
		GraphicResize.InterpolationMode = InterpolationMode.HighQualityBicubic;
		GraphicResize.SmoothingMode = SmoothingMode.HighQuality;
		GraphicResize.PixelOffsetMode = PixelOffsetMode.HighQuality;
		GraphicResize.CompositingQuality = CompositingQuality.HighQuality;
		GraphicResize.DrawImage(Img, new Rectangle(Convert.ToInt32(x) - 1, Convert.ToInt32(y) - 1, Convert.ToInt32(scaledWidth) + 2, Convert.ToInt32(scaledHeight) + 2), new Rectangle(0, 0, Convert.ToInt32(intWidth), Convert.ToInt32(intHeight)), GraphicsUnit.Pixel);

		return ImgResize;

	}

	Bitmap _ResizeDownNoWhiteSpace(Stream stream, int NewWidth, int NewHeight, bool MaintainTransparency, string BackgroundColour)
	{

		Image Img = Image.FromStream(stream);

		try
		{
			var orientation = (int)Img.GetPropertyItem(274).Value[0];

			switch (orientation)
			{
			case 1:
				// No rotation required.
				break;
			case 2:
				Img.RotateFlip(RotateFlipType.RotateNoneFlipX);
				break;
			case 3:
				Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
				break;
			case 4:
				Img.RotateFlip(RotateFlipType.Rotate180FlipX);
				break;
			case 5:
				Img.RotateFlip(RotateFlipType.Rotate90FlipX);
				break;
			case 6:
				Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
				break;
			case 7:
				Img.RotateFlip(RotateFlipType.Rotate270FlipX);
				break;
			case 8:
				Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
				break;
			}
			// This EXIF data is now invalid and should be removed.
			Img.RemovePropertyItem(274);
		}
		catch { }

		decimal intWidth = Img.Width;
		decimal intHeight = Img.Height;

		decimal x = 0;
		decimal y = 0;
		decimal scaledWidth;
		decimal scaledHeight;
		float targetRatio = (float)NewWidth / (float)NewHeight;

		if (intWidth > NewWidth || intHeight > NewHeight)
		{
			if (((float)intWidth / (float)intHeight) < targetRatio)
			{
				//---- TRIGGERED IF SOURCE FILE IS TALLER RATIO THAN NEW IMAGE
				scaledWidth = intWidth / (intHeight / NewHeight);
				scaledHeight = NewHeight;
			}
			else if (((float)intWidth / (float)intHeight) > targetRatio)
			{
				//---- TRIGGERED IF SOURCE FILE IS WIDER RATIO THAN NEW IMAGE
				scaledWidth = NewWidth;
				scaledHeight = intHeight / (intWidth / NewWidth);
			}
			else
			{
				//---- TRIGGERED IF SOURCE FILE IS EQUAL RATIO TO NEW IMAGE
				NewWidth = Convert.ToInt32(scaledWidth = intWidth);
				NewHeight = Convert.ToInt32(scaledHeight = intHeight);
			}
		}
		else
		{
			NewWidth = Convert.ToInt32(scaledWidth = intWidth);
			NewHeight = Convert.ToInt32(scaledHeight = intHeight);
		}

		x = 0;
		y = 0;

		Bitmap ImgResize = new Bitmap(Convert.ToInt32(scaledWidth), Convert.ToInt32(scaledHeight), PixelFormat.Format32bppArgb);
		ImgResize.SetResolution(72, 72);
		Graphics GraphicResize = Graphics.FromImage(ImgResize);
		if (MaintainTransparency == true) GraphicResize.Clear(Color.Transparent); else GraphicResize.Clear(ColorTranslator.FromHtml(BackgroundColour));
		GraphicResize.InterpolationMode = InterpolationMode.HighQualityBicubic;
		GraphicResize.SmoothingMode = SmoothingMode.HighQuality;
		GraphicResize.PixelOffsetMode = PixelOffsetMode.HighQuality;
		GraphicResize.CompositingQuality = CompositingQuality.HighQuality;
		GraphicResize.DrawImage(Img, new Rectangle(Convert.ToInt32(x) - 1, Convert.ToInt32(y) - 1, Convert.ToInt32(scaledWidth) + 2, Convert.ToInt32(scaledHeight) + 2), new Rectangle(0, 0, Convert.ToInt32(intWidth), Convert.ToInt32(intHeight)), GraphicsUnit.Pixel);

		return ImgResize;

	}


	Bitmap _Crop(Bitmap Img, int NewWidth, int NewHeight, ref int x, ref int y, bool MaintainTransparency)
	{
		Bitmap ImgCrop = new Bitmap(NewWidth, NewHeight, PixelFormat.Format32bppArgb);
		Graphics GraphicCrop = Graphics.FromImage(ImgCrop);
		if (MaintainTransparency == true) GraphicCrop.Clear(Color.Transparent); else GraphicCrop.Clear(Color.White);
		GraphicCrop.InterpolationMode = InterpolationMode.HighQualityBicubic;
		GraphicCrop.SmoothingMode = SmoothingMode.HighQuality;
		GraphicCrop.PixelOffsetMode = PixelOffsetMode.HighQuality;
		GraphicCrop.CompositingQuality = CompositingQuality.HighQuality;
		GraphicCrop.DrawImage(Img, new Rectangle(0, 0, NewWidth, NewHeight), x, y, NewWidth, NewHeight, GraphicsUnit.Pixel);
		return ImgCrop;
	}

	void _Save(Bitmap Img, string RealtivePathAndFileName, bool WaterMarkImage, string WatermarkURL, string WatermarkPosition)
	{
		if (WaterMarkImage == true && WatermarkURL != "" && File.Exists(HttpContext.Current.Server.MapPath(WatermarkURL)))
		{
			Bitmap ImgWaterMark = new Bitmap(Img.Width, Img.Height, PixelFormat.Format32bppArgb);
			Graphics GraphicWatermark = Graphics.FromImage(ImgWaterMark);
			GraphicWatermark.Clear(Color.Transparent);
			GraphicWatermark.InterpolationMode = InterpolationMode.HighQualityBicubic;
			GraphicWatermark.SmoothingMode = SmoothingMode.HighQuality;
			GraphicWatermark.PixelOffsetMode = PixelOffsetMode.HighQuality;
			GraphicWatermark.CompositingQuality = CompositingQuality.HighQuality;
			GraphicWatermark.DrawImage(Img, new Rectangle(0, 0, Img.Width, Img.Height), 0, 0, Img.Width, Img.Height, GraphicsUnit.Pixel);

			if (WatermarkPosition == "") { WatermarkPosition = "center"; }

			Image imageFile = Image.FromFile(HttpContext.Current.Server.MapPath(WatermarkURL));

			decimal x = 0;
			decimal y = 0;
			decimal scaledWidth;
			decimal scaledHeight;
			float targetRatio = (float)ImgWaterMark.Width / (float)ImgWaterMark.Height;

			if (((float)imageFile.Width / (float)imageFile.Height) < targetRatio)
			{
				//---- TRIGGERED IF SOURCE FILE IS TALLER RATIO THAN NEW IMAGE
				scaledWidth = (decimal)imageFile.Width / ((decimal)imageFile.Height / (decimal)ImgWaterMark.Height);
				scaledHeight = ImgWaterMark.Height;
				x = (ImgWaterMark.Width - scaledWidth) / 2;
			}
			else if (((float)imageFile.Width / (float)imageFile.Height) > targetRatio)
			{
				//---- TRIGGERED IF SOURCE FILE IS WIDER RATIO THAN NEW IMAGE
				scaledWidth = ImgWaterMark.Width;
				scaledHeight = (decimal)imageFile.Height / ((decimal)imageFile.Width / (decimal)ImgWaterMark.Width);
				y = (ImgWaterMark.Height - scaledHeight) / 2;
			}
			else
			{
				//---- TRIGGERED IF SOURCE FILE IS EQUAL RATIO TO NEW IMAGE
				scaledWidth = ImgWaterMark.Width;
				scaledHeight = ImgWaterMark.Height;
			}

			switch (WatermarkPosition.ToLower())
			{
			case "center":
				decimal origHeight = scaledHeight;
				decimal origWidth = scaledWidth;
				scaledHeight = scaledHeight * (decimal)0.7;
				scaledWidth = scaledWidth * (decimal)0.7;
				x = x + ((origWidth - scaledWidth) / 2);
				y = y + ((origHeight - scaledHeight) / 2);
				break;
			case "topleft":
				scaledHeight = scaledHeight * (decimal)0.4;
				scaledWidth = scaledWidth * (decimal)0.4;
				x = 0 + ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4);
				y = 0 + ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4);
				break;
			case "topright":
				scaledHeight = scaledHeight * (decimal)0.4;
				scaledWidth = scaledWidth * (decimal)0.4;
				x = ImgWaterMark.Width - ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4) - scaledWidth;
				y = 0 + ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4);
				break;
			case "bottomright":
				scaledHeight = scaledHeight * (decimal)0.4;
				scaledWidth = scaledWidth * (decimal)0.4;
				x = ImgWaterMark.Width - ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4) - scaledWidth;
				y = ImgWaterMark.Height - ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4) - scaledHeight;
				break;
			case "bottomleft":
				scaledHeight = scaledHeight * (decimal)0.4;
				scaledWidth = scaledWidth * (decimal)0.4;
				x = 0 + ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4);
				y = ImgWaterMark.Height - ((scaledWidth > scaledHeight ? scaledHeight : scaledWidth) / 4) - scaledHeight;
				break;
			default:
				decimal origHeight2 = scaledHeight;
				decimal origWidth2 = scaledWidth;
				scaledHeight = scaledHeight * (decimal)0.7;
				scaledWidth = scaledWidth * (decimal)0.7;
				x = x + ((origWidth2 - scaledWidth) / 2);
				y = y + ((origHeight2 - scaledHeight) / 2);
				break;
			}

			GraphicWatermark.DrawImage(imageFile, new Rectangle(Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(scaledWidth), Convert.ToInt32(scaledHeight)), new Rectangle(0, 0, Convert.ToInt32(imageFile.Width), Convert.ToInt32(imageFile.Height)), GraphicsUnit.Pixel);

			Img = ImgWaterMark;
		}

		ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
		EncoderParameters Params = new EncoderParameters(2);
		Params.Param[0] = new EncoderParameter(Encoder.Quality, Convert.ToInt32(100L));

		if (RealtivePathAndFileName.EndsWith(".png"))
		{
			Params.Param[1] = new EncoderParameter(Encoder.ColorDepth, Convert.ToInt32(32L));
			Img.Save(HttpContext.Current.Server.MapPath(RealtivePathAndFileName), Info[4], Params);
		}
		else
		{
			Params.Param[1] = new EncoderParameter(Encoder.ColorDepth, Convert.ToInt32(24L));
			Img.Save(HttpContext.Current.Server.MapPath(RealtivePathAndFileName), Info[1], Params);
		}
	}

}
