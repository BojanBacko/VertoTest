using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Last change: Shane -> VIE -> 1.0.7

/// <summary>
/// Summary description for Image
/// </summary>

namespace Images
{

	public enum ImagingProcessMethod
	{
		/// <summary>
		/// Resizes to cover required size, crops off excess - OR - crops with Jcrop/VIE.
		/// </summary>
		CropResizeAndSave,
		/// <summary>
		/// Automatically resizes to cover required size, crops off excess.
		/// </summary>
		AutoCropResizeAndSave,
		Resize,
		/// <summary>
		/// Resize image to fit within dimensions.  No cropping, no whitespace, will stretch smaller images.
		/// </summary>
		ResizeLimit,
		/// <summary>
		/// Resize image to set width; height is dynamic.  No cropping, no whitespace, will stretch smaller images.
		/// </summary>
		ResizeLimitWidth,
		/// <summary>
		/// Resize image to fit dimensions.  No cropping, pads with whitespace, will stretch smaller images.
		/// </summary>
		ResizeLimitWhiteSpace,
		/// <summary>
		/// Saves as a transparent PNG, supports upload of JPEG (will still save as PNG).  Uses VIE/Jcrop.
		/// </summary>
		TransparentCropResizeAndSave,
		/// <summary>
		/// Do not use
		/// </summary>
		Editor,
		/// <summary>
		/// Resize the image to fit within the required dimensions.  Whitespace is added, no cropping is performed, image is not altered if smaller than required size (other than padding with whitespace).
		/// </summary>
		ResizeDownWhiteSpace,
		/// <summary>
		/// Resize the image to fit within the required dimensions.  No whitespace is added, no cropping is performed, image is not altered if smaller than required size.
		/// </summary>
		ResizeDownNoWhiteSpace
	}

	public class Thumb : ITemplate
	{

		public int Width { get; set; }
		public int Height { get; set; }
		public ImagingProcessMethod Method { get; set; }
		public string WatermarkPosition { get; set; }
		public bool WatermarkEnabled { get; set; }
		public string WatermarkUrl { get; set; }

		[TypeConverterAttribute(typeof(StringArrayConverter))]
		public string[] TemplateWhitelist { get; set; }

		public void InstantiateIn(Control container) { }

	}

}