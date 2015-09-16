using Images;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Last change: Shane -> VIE -> 1.0.9

public partial class controls_ImageControl : System.Web.UI.UserControl
{
	private Collection<Thumb>
		_thumbs = new Collection<Thumb>();

	[PersistenceMode(PersistenceMode.InnerProperty)]
	public Collection<Thumb> Thumbs { get { return _thumbs; } set { _thumbs = value; } }

	public int ContentID { get; set; }
	public string ImageNo { get; set; }
	public int Height { get; set; }
	public int Width { get; set; }
	public string Title { get; set; }
	public bool WatermarkEnabled { get; set; }
	public string WatermarkUrl { get; set; }
	public string WatermarkPosition { get; set; }

	public string popupWidth;
	public string popupHeight;

	public int UnsizedHeight { get; set; }
	public int UnsizedWidth { get; set; }

	public string CropImage { get; set; }

	public string ImagePath { get; set; }

	public bool HideDelete { get; set; }

	public ImagingProcessMethod Method { get; set; }

	/// <summary>
	/// Set to true to use the editor (VIE) on this control
	/// </summary>
	public bool Editor { get; set; }
	/// <summary>
	/// VIE (image cropper/editor) server-side version
	/// </summary>
	private const string VieVersion = "1.0.9";
	/// <summary>
	/// Local image path for overlaying the final crop (e.g. cut-out shadow slider overlay) - VIE only.  Users can toggle this on/off through VIE.
	/// </summary>
	public string OverlayUrl { get; set; }
	/// <summary>
	/// Allow editing of a specific thumb, without affecting main image or other thumbs (VIE only)
	/// </summary>
	public bool IsolateThumbs { get; set; }
	/// <summary>
	/// Background colour when uploading an image with whitespace
	/// </summary>
	public string BackColour { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		litDims.Text = Width + " x " + Height;
		string _img;

		if (Method == ImagingProcessMethod.TransparentCropResizeAndSave)
		{
			if (File.Exists(Server.MapPath(ReturnImageFilePath(false, true))))
			{
				_img = ReturnImageFilePath(false, true);
			}
			else
				_img = "http://img.vertouk.com/" + Width + "x" + Height + "?logo=http://www.vertouk.com/images/verto.png&showres=true";
		}
		else
		{
			if (File.Exists(Server.MapPath(ReturnImageFilePath())))
			{
				_img = ReturnImageFilePath();
			}
			else
				_img = "http://img.vertouk.com/" + Width + "x" + Height + "?logo=http://www.vertouk.com/images/verto.png&showres=true";
		}

		Image1.ImageUrl = _img;
		Image1.CssClass = "main-img Img_" + ContentID + "_" + ImageNo;
		btnCropThumbs.CssClass = "CropThumbs_" + ContentID + "_" + ImageNo;

		if (string.IsNullOrEmpty(popupWidth))
		{
			popupWidth = "850";
			popupHeight = "450";
		}

		if (UnsizedWidth == 0)
		{
			UnsizedWidth = 850;
			UnsizedHeight = 450;
		}

		btnDelete.Visible = !HideDelete;

		//if()
		lnkUpload.NavigateUrl = string.Format("/admin/controls/ImageCropper.aspx?id={0}&width={6}&height={7}&dimensions={1}&ImagePath={2}&ImageNo={3}&uwidth={4}&uheight={5}&watermark={8}&watermarkurl={9}&watermarkpos={10}&type={11}&name={12}&iframe=true&backColour={13}", ContentID, (Width + "x" + Height), ImagePath, ImageNo, UnsizedWidth, UnsizedHeight, popupWidth, popupHeight, WatermarkEnabled, Server.UrlEncode(WatermarkUrl), WatermarkPosition, (Method == ImagingProcessMethod.TransparentCropResizeAndSave ? "png" : "jpg"), Server.UrlEncode(Title), BackColour ?? "#FFF");
		lnkUpload.Attributes.Add("rel", "ImageCropper");

		//VIE: Update rel, insert thumbs
		lnkUpload.Attributes["data-vie-server-version"] = VieVersion;
		if (Editor)
		{
			lnkUpload.Attributes["rel"] = "Editor";
			lnkUpload.Attributes["data-vie-destination"] = ReturnImageFilePath(transparent: Method == ImagingProcessMethod.TransparentCropResizeAndSave);
			lnkUpload.Attributes["data-vie-thumbs"] = "";
			lnkUpload.Attributes["data-vie-overlay"] = OverlayUrl;
			lnkUpload.Attributes["data-vie-type"] = (Method == ImagingProcessMethod.TransparentCropResizeAndSave ? "png" : "jpg");
			lnkUpload.Attributes["data-vie-server-method"] = Method.ToString();
			lnkUpload.Attributes["data-vie-back-colour"] = BackColour ?? "#FFF";
			foreach (Thumb t in Thumbs)
			{
				lnkUpload.Attributes["data-vie-thumbs"] += t.Width + "x" + t.Height + ",";
				if (IsolateThumbs)
					litThumbs.Text += "<div class='vie-open-thumb' data-width='" + t.Width + "' data-height='" + t.Height + "'>" + t.Width + " x " + t.Height + "</div>";
			}
			if (lnkUpload.Attributes["data-vie-thumbs"].Length > 0)
			{
				lnkUpload.Attributes["data-vie-thumbs"] = lnkUpload.Attributes["data-vie-thumbs"].Substring(0, lnkUpload.Attributes["data-vie-thumbs"].Length - 1);
				if (IsolateThumbs && File.Exists(Server.MapPath(ReturnImageFilePath(transparent: Method == ImagingProcessMethod.TransparentCropResizeAndSave))))
					lnkEditThumbs.Visible = pnlThumbs.Visible = true;
			}
		}

		if (Method == ImagingProcessMethod.CropResizeAndSave || Method == ImagingProcessMethod.TransparentCropResizeAndSave || (Method == ImagingProcessMethod.ResizeDownNoWhiteSpace && Editor) || (Method == ImagingProcessMethod.ResizeDownWhiteSpace && Editor))
		{
			lnkUpload.Visible = true;
		}
		else
		{
			lnkUpload.Visible = false;
			btnUpload.Visible = true;
			phFileUpload.Visible = true;
		}


		litTitle.Text = Title;
		//check if there are temp images and delete them if so
		if (!IsPostBack)
		{
			string _dimensions = Width + "x" + Height;

			if (Width != 0 && Height != 0)
			{

				if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
				{
					if (File.Exists(Server.MapPath(string.Format("{0}/0_{2}_{3}.jpg", ImagePath, ContentID, ImageNo, _dimensions))))
						File.Delete(Server.MapPath(string.Format("{0}/0_{2}_{3}.jpg", ImagePath, ContentID, ImageNo, _dimensions)));
				}
				else
				{
					if (File.Exists(Server.MapPath(string.Format("{0}/0_{2}.jpg", ImagePath, ContentID, _dimensions))))
						File.Delete(Server.MapPath(string.Format("{0}/0_{2}.jpg", ImagePath, ContentID, _dimensions)));
				}

				foreach (Thumb t in _thumbs)
				{
					string _thumbDimensions = t.Width + "x" + t.Height;

					if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
					{
						if (File.Exists(Server.MapPath(string.Format("{0}/0_{2}_{3}.jpg", ImagePath, ContentID, ImageNo, _thumbDimensions))))
							File.Delete(Server.MapPath(string.Format("{0}/0_{2}_{3}.jpg", ImagePath, ContentID, ImageNo, _thumbDimensions)));
					}
					else
					{
						if (File.Exists(Server.MapPath(string.Format("{0}/0_{2}.jpg", ImagePath, ContentID, _thumbDimensions))))
							File.Delete(Server.MapPath(string.Format("{0}/0_{2}.jpg", ImagePath, ContentID, _thumbDimensions)));
					}
				}
			}
			else
			{
				pnlUpload.Visible = false;
				litError.Text = "<p style=\"color:red;\">There are no dimensions setup for this image.</p>";
			}


		}

	}

	public string ReturnImageFilePath(bool ins = false, bool transparent = false)
	{
		string _dimensions = Width + "x" + Height;

		if (!ins)
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/{1}_{2}_{3}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo, _dimensions);
			}
			else
			{
				return string.Format("{0}/{1}_{2}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, _dimensions);
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/{1}_{2}_{3}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo, _dimensions);
			}
			else
			{
				return string.Format("{0}/{1}_{2}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, _dimensions);
			}
		}
	}

	public string ReturnThumbFilePath(string _dimensions, bool ins = false, bool transparent = false)
	{
		if (!ins)
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/{1}_{2}_{3}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo, _dimensions);
			}
			else
			{
				return string.Format("{0}/{1}_{2}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, _dimensions);
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/{1}_{2}_{3}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo, _dimensions);
			}
			else
			{
				return string.Format("{0}/{1}_{2}." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, _dimensions);
			}
		}
	}

	public string ReturnImageUnsizedFilePath(bool ins = false, bool transparent = false)
	{
		if (!ins)
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/{1}_{2}_unsized." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo);
			}
			else
			{
				return string.Format("{0}/{1}_unsized." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID);
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/0_{2}_unsized." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo);
			}
			else
			{
				return string.Format("{0}/0_unsized." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID);
			}
		}
	}
	public string ReturnImageOriginalFilePath(bool ins = false, bool transparent = false)
	{
		if (!ins)
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/{1}_{2}_original." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo);
			}
			else
			{
				return string.Format("{0}/{1}_original." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID);
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/0_{2}_original." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo);
			}
			else
			{
				return string.Format("{0}/0_original." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID);
			}
		}
	}
	public string ReturnImageNoWatermarkFilePath(bool ins = false, bool transparent = false)
	{
		if (!ins)
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/{1}_{2}_nowatermark." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo);
			}
			else
			{
				return string.Format("{0}/{1}_nowatermark." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID);
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(ImageNo) && ImageNo != "0")
			{
				return string.Format("{0}/0_{2}_nowatermark." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID, ImageNo);
			}
			else
			{
				return string.Format("{0}/0_nowatermark." + (transparent == false ? "jpg" : "png"), ImagePath, ContentID);
			}
		}
	}
	protected void btnDelete_Click(object sender, EventArgs e)
	{
		string _dimensions = Width + "x" + Height;

		if (File.Exists(Server.MapPath(ReturnImageFilePath())))
			File.Delete(Server.MapPath(ReturnImageFilePath()));

		if (File.Exists(Server.MapPath(ReturnImageUnsizedFilePath())))
			File.Delete(Server.MapPath(ReturnImageUnsizedFilePath()));

		if (File.Exists(Server.MapPath(ReturnImageOriginalFilePath())))
			File.Delete(Server.MapPath(ReturnImageOriginalFilePath()));

		if (File.Exists(Server.MapPath(ReturnImageNoWatermarkFilePath())))
			File.Delete(Server.MapPath(ReturnImageNoWatermarkFilePath()));

		foreach (Thumb t in _thumbs)
		{
			string _thumbDimensions = t.Width + "x" + t.Height;

			if (File.Exists(Server.MapPath(ReturnThumbFilePath(_thumbDimensions))))
				File.Delete(Server.MapPath(ReturnThumbFilePath(_thumbDimensions)));
		}

		string _img;
		if (File.Exists(Server.MapPath(ReturnImageFilePath())))
			_img = ReturnImageFilePath();
		else
			_img = "http://img.vertouk.com/" + Width + "x" + Height + "?logo=http://www.vertouk.com/images/verto.png&showres=true";


		Image1.ImageUrl = _img + (_img.Contains("?") ? "&nocache" : "?nocache") + DateTime.Now;
	}


	protected void btnCropThumbs_Click(object sender, EventArgs e)
	{
		CropThumbs();
	}

	public void CropThumbs(bool IsIns = false)
	{
		ImagingProcess ip = new ImagingProcess();

		string pth = HttpContext.Current.Server.MapPath(ReturnImageNoWatermarkFilePath());
		if (Method == ImagingProcessMethod.TransparentCropResizeAndSave)
		{
			pth = HttpContext.Current.Server.MapPath(ReturnImageNoWatermarkFilePath(false, true));
		}

		FileStream fr = new FileStream(pth, FileMode.Open);//File.Open(HttpContext.Current.Server.MapPath(ReturnImageUnsizedFilePath()), FileMode.Open, FileAccess.Read);

		foreach (Thumb t in _thumbs)
		{
			string _thumbDimensions = t.Width + "x" + t.Height;

			if (t.Method == ImagingProcessMethod.CropResizeAndSave)
			{
				if (Method == ImagingProcessMethod.CropResizeAndSave)
				{
					ip.CropResizeAndSave(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
				}
				else
				{
					if (fu.FileContent.Length > 0)
					{
						ip.CropResizeAndSave(fu.FileContent, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
					else
					{
						ip.CropResizeAndSave(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
				}
			}
			else if (t.Method == ImagingProcessMethod.Resize)
			{
				if (Method == ImagingProcessMethod.CropResizeAndSave)
				{
					ip.Resize(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
				}
				else
				{
					if (fu.FileContent.Length > 0)
					{
						ip.Resize(fu.FileContent, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
					else
					{
						ip.Resize(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
				}
			}
			else if (t.Method == ImagingProcessMethod.ResizeLimit)
			{
				if (Method == ImagingProcessMethod.CropResizeAndSave)
				{
					ip.ResizeLimit(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
				}
				else
				{
					if (fu.FileContent.Length > 0)
					{
						ip.ResizeLimit(fu.FileContent, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
					else
					{
						ip.ResizeLimit(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
				}
			}
			else if (t.Method == ImagingProcessMethod.ResizeLimitWhiteSpace)
			{
				if (Method == ImagingProcessMethod.CropResizeAndSave)
				{
					ip.ResizeLimitWhiteSpace(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, "#FFFFFF", t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
				}
				else
				{
					if (fu.FileContent.Length > 0)
					{
						ip.ResizeLimitWhiteSpace(fu.FileContent, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, "#FFFFFF", t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
					else
					{
						ip.ResizeLimitWhiteSpace(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, "#FFFFFF", t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
				}
			}
			else if (t.Method == ImagingProcessMethod.ResizeLimitWidth)
			{
				if (Method == ImagingProcessMethod.CropResizeAndSave)
				{
					ip.ResizeLimitWidth(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
				}
				else
				{
					if (fu.FileContent.Length > 0)
					{
						ip.ResizeLimitWidth(fu.FileContent, ReturnThumbFilePath(_thumbDimensions), t.Width, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
					else
					{
						ip.ResizeLimitWidth(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, false, t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
					}
				}
			}
			else if (t.Method == ImagingProcessMethod.ResizeDownWhiteSpace)
			{
				if (fu.FileContent.Length > 0)
					ip.ResizeDownWhiteSpace(fu.FileContent, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, "#FFFFFF", t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
				else
					ip.ResizeDownWhiteSpace(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, "#FFFFFF", t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
			}
			else if (t.Method == ImagingProcessMethod.ResizeDownNoWhiteSpace)
			{
				if (fu.FileContent.Length > 0)
					ip.ResizeDownNoWhiteSpace(fu.FileContent, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, "#FFFFFF", t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
				else
					ip.ResizeDownNoWhiteSpace(fr, ReturnThumbFilePath(_thumbDimensions), t.Width, t.Height, false, "#FFFFFF", t.WatermarkEnabled, t.WatermarkUrl, t.WatermarkPosition);
			}
		}

		if (File.Exists(HttpContext.Current.Server.MapPath(ReturnImageOriginalFilePath())))
			File.Delete(HttpContext.Current.Server.MapPath(ReturnImageOriginalFilePath()));

		fr.Close();
		fr.Dispose();
	}
	protected void btnUpload_Click(object sender, EventArgs e)
	{
		if (fu.HasFile)
		{
			string _dimensions = Width + "x" + Height;
			ImagingProcess ipr = new ImagingProcess();

			if (Method == ImagingProcessMethod.TransparentCropResizeAndSave)
			{
				ipr.ResizeLimit(fu.FileContent, ReturnImageUnsizedFilePath(false, true), 1920, 650, true, WatermarkEnabled, WatermarkUrl, WatermarkPosition);
			}
			else
			{
				ipr.ResizeLimit(fu.FileContent, ReturnImageUnsizedFilePath(), 1920, 650, false, WatermarkEnabled, WatermarkUrl, WatermarkPosition);
			}
			if (Method == ImagingProcessMethod.AutoCropResizeAndSave)
			{
				ipr.CropResizeAndSave(fu.FileContent, ReturnImageFilePath(), Width, Height, false, WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.CropResizeAndSave(fu.FileContent, ReturnImageNoWatermarkFilePath(), Width, Height);
			}
			else if (Method == ImagingProcessMethod.Resize)
			{
				ipr.Resize(fu.FileContent, ReturnImageFilePath(), Width, Height, false, WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.Resize(fu.FileContent, ReturnImageNoWatermarkFilePath(), Width, Height);
			}
			else if (Method == ImagingProcessMethod.ResizeLimit)
			{
				ipr.ResizeLimit(fu.FileContent, ReturnImageFilePath(), Width, Height, false, WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.ResizeLimit(fu.FileContent, ReturnImageNoWatermarkFilePath(), Width, Height);
			}
			else if (Method == ImagingProcessMethod.ResizeLimitWhiteSpace)
			{
				ipr.ResizeLimitWhiteSpace(fu.FileContent, ReturnImageFilePath(), Width, Height, false, "#FFFFFF", WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.ResizeLimitWhiteSpace(fu.FileContent, ReturnImageNoWatermarkFilePath(), Width, Height);
			}
			else if (Method == ImagingProcessMethod.ResizeLimitWidth)
			{
				ipr.ResizeLimitWidth(fu.FileContent, ReturnImageFilePath(), Width, false, WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.ResizeLimitWidth(fu.FileContent, ReturnImageNoWatermarkFilePath(), Width);
			}
			else if (Method == ImagingProcessMethod.TransparentCropResizeAndSave)
			{
				ipr.CropResizeAndSave(fu.FileContent, ReturnImageFilePath(false, true), Width, Height, true, WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.CropResizeAndSave(fu.FileContent, ReturnImageNoWatermarkFilePath(false, true), Width, Height, true);
			}
			else if (Method == ImagingProcessMethod.ResizeDownWhiteSpace)
			{
				ipr.ResizeDownWhiteSpace(fu.FileContent, ReturnImageFilePath(false, false), Width, Height, true, "#FFFFFF", WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.ResizeDownWhiteSpace(fu.FileContent, ReturnImageNoWatermarkFilePath(false, false), Width, Height, true);
			}
			else if (Method == ImagingProcessMethod.ResizeDownNoWhiteSpace)
			{
				ipr.ResizeDownNoWhiteSpace(fu.FileContent, ReturnImageFilePath(false, false), Width, Height, true, "#FFFFFF", WatermarkEnabled, WatermarkUrl, WatermarkPosition);
				ipr.ResizeDownNoWhiteSpace(fu.FileContent, ReturnImageNoWatermarkFilePath(false, false), Width, Height, true);
			}

			CropThumbs();

			if (Method == ImagingProcessMethod.TransparentCropResizeAndSave)
			{
				Image1.ImageUrl = ReturnImageFilePath(false, true) + "?" + DateTime.Now;
			}
			else { Image1.ImageUrl = ReturnImageFilePath() + "?" + DateTime.Now; }

		}
		else
			litError.Text = "<p style=\"color:red;\">Please upload a file first before clicking upload.</p>";
	}
}