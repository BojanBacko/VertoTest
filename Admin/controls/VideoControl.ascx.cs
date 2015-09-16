using Images;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Admin_controls_VideoControl : System.Web.UI.UserControl
{
	/// <summary>
	/// Record ID (AdminControl will usually handle this)
	/// </summary>
	public int ContentID = -1;
	/// <summary>
	/// Video number
	/// </summary>
	public int VideoNo = -1;
	/// <summary>
	/// Current video URL (shouldn't need to set this)
	/// </summary>
	public string VideoUrl = null;
	/// <summary>
	/// DB column name for the video URL
	/// </summary>
	public string Field = "videoUrl";
	/// <summary>
	/// DB column name for the id field (AdminControl will usually handle this)
	/// </summary>
	public string IdField = null;
	/// <summary>
	/// DB table name (AdminControl will usually handle this)
	/// </summary>
	public string Table = null;
	/// <summary>
	/// Title to show above the video thumbnail
	/// </summary>
	public string Title = null;
	/// <summary>
	/// Path to store local thumbnails
	/// </summary>
	public string ImagePath = "/images/content/videos";
	/// <summary>
	/// Width of saved thumbnail image
	/// </summary>
	public int Width = 640;
	/// <summary>
	/// Height of saved thumbnail image
	/// </summary>
	public int Height = 360;

	/// <summary>
	/// Additional thumbnails
	/// </summary>
	[PersistenceMode(PersistenceMode.InnerProperty)]
	public Collection<Thumb> Thumbs { get { return _Thumbs; } set { _Thumbs = value; } }
	private Collection<Thumb> _Thumbs = new Collection<Thumb>();


	private System.Uri _ValidUri = null;
	private bool _Failed = false;
	private string _Url { get { return (string)HttpContext.Current.Session["video-url"]; } set { HttpContext.Current.Session["video-url"] = (string)value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
		tbUrl.Text = VideoUrl;
    }
	protected void Page_PreRender(object sender, EventArgs e)
	{
		if (ContentID == null || ContentID < 0)
			Fail("You must set a `ContentID` (Perhaps you haven't copied the code from AdminControl? Or you haven't set `ContentID` to '0' for the FormView insert?)");
		else if (string.IsNullOrWhiteSpace(ImagePath))
			Fail("You must set an `ImagePath`");
		else if (!System.IO.Directory.Exists(Server.MapPath(ImagePath)))
			Fail("The `ImagePath` points to a location that doesn't exist, you'll need to create it");
		else if (string.IsNullOrWhiteSpace(Table) || string.IsNullOrWhiteSpace(Field) || string.IsNullOrWhiteSpace(IdField))
			Fail("You must set `Table` (" + Table + "), `Field` (" + Field + ") and `IdField` (" + IdField + ")");

		if (!string.IsNullOrWhiteSpace(VideoUrl) && File.Exists(Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg"))
		{
			imgPreview.ImageUrl = ImagePath.TrimEnd('/') + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg?_=" + DateTime.Now.Ticks;
			pnlIcon.CssClass += " ep";
			pnlIcon.Attributes["data-ep"] = VideoUrl;
		}
	}
	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		_Url = tbUrl.Text;
		DownloadThumbnail();
		if(ContentID > 0)
			Update();
	}
	protected void lnkDelete_Click(object sender, EventArgs e)
	{
		Update(string.Empty);
		if (File.Exists(Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg"))
			File.Delete(Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg");

		pnlIcon.CssClass = pnlIcon.CssClass.Replace(" ep", string.Empty);
		pnlIcon.Attributes.Remove("data-ep");

		imgPreview.ImageUrl = "http://img.vertouk.com/426x240/222D3A/222D3A/222D3A/?logo=&showres=false";
	}
	public void Update(string Url = null)
	{
		using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL"].ToString()))
		{
			conn.Open();
			using (SqlCommand q = new SqlCommand("UPDATE [" + Table + "] SET [" + Field + "] = @url WHERE [" + IdField + "] = '" + ContentID + "'", conn))
			{
				q.Parameters.AddWithValue("url", Url ?? _Url);
				q.ExecuteNonQuery();
				if (File.Exists(HttpContext.Current.Server.MapPath(ImagePath.TrimEnd('/')) + "/0" + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg"))
					File.Move(HttpContext.Current.Server.MapPath(ImagePath.TrimEnd('/')) + "/0" + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg", HttpContext.Current.Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg");
			}
		}
	}
	public void DownloadThumbnail(string Url = null)
	{
		if (string.IsNullOrWhiteSpace(Url))
			Url = tbUrl.Text;

		if (!Uri.TryCreate(Url, UriKind.Absolute, out _ValidUri)) // Validate URL
			Fail("Please enter a valid YouTube or Vimeo video URL.&nbsp; This should contain the protocol (e.g. 'http://' or 'https://').&nbsp; [Reference: #DT1]", false);
		else
		{
			//Update();
			try
			{
				ImagingProcess cropper = new ImagingProcess();
				using (WebClient web = new WebClient())
				{
					Match vimeo = new Regex("vimeo\\.com/(?:.*#|.*/videos/)?([0-9]+)").Match(Url);
					Match youtube = new Regex("youtu(?:\\.be|be\\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)").Match(Url);
					if (vimeo.Success) // Download XML containing URL dor thumbnail, then save thumb
					{
						string r = web.DownloadString("http://vimeo.com/api/v2/video/" + vimeo.Groups[1].Value + ".xml");
						XmlDocument x = new XmlDocument();
						x.LoadXml(r);
						XmlNode n = x.SelectSingleNode("//video/thumbnail_large/text()");
						if (string.IsNullOrWhiteSpace(n.Value.ToString()))
							n = x.SelectSingleNode("//video/thumbnail_medium/text()");
						if (string.IsNullOrWhiteSpace(n.Value.ToString()))
							n = x.SelectSingleNode("//video/thumbnail_small/text()");
						using (WebClient img = new WebClient())
						{
							img.DownloadFile(n.Value, Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_original.jpg");
						}
					}
					else if (youtube.Success) // Direct thumb download
					{
						try
						{
							web.DownloadFile("http://img.youtube.com/vi/" + youtube.Groups[1].Value + "/maxresdefault.jpg", Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_original.jpg");
						}
						catch (WebException err)
						{ // Failback to crappy thumbnail
							if (((HttpWebResponse)err.Response).StatusCode == HttpStatusCode.NotFound)
								web.DownloadFile("http://img.youtube.com/vi/" + youtube.Groups[1].Value + "/hqdefault.jpg", Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_original.jpg");
							else
								throw err;
						}
					}
					else
						Fail("Please enter a valid YouTube or Vimeo video URL.&nbsp; This should contain the protocol (e.g. 'http://' or 'https://').&nbsp; [Reference: #DT2]", false);
				}
				if (!_Failed) // Crop + resize to specified dimensions
					using (Stream stream = File.OpenRead(Server.MapPath(ImagePath.TrimEnd('/')) + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_original.jpg"))
					{
						cropper.CropResizeAndSave(stream, ImagePath.TrimEnd('/') + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg", Width, Height);
						foreach (Thumb t in _Thumbs)
							cropper.CropResizeAndSave(stream, ImagePath.TrimEnd('/') + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + t.Width + "x" + t.Height + ".jpg", t.Width, t.Height);
						imgPreview.ImageUrl = ImagePath.TrimEnd('/') + "/" + ContentID + (VideoNo != -1 ? "_" + VideoNo.ToString() : "") + "_" + Width + "x" + Height + ".jpg?_=" + DateTime.Now.Ticks;
					}
			}
			catch (Exception err)
			{
				if (err.Message.Contains("The remote server returned an error"))
					Fail("Download error encountered (please check the URL you've entered): " + err.Message, false);
				else
					Fail("Download error encountered [Reference: #DT3, Table: " + Table + ", Field: " + Field + ", IdField: " + IdField + ", ContentID: " + ContentID + "] (" + err.Message + ")");
			}
		}
	}
	private void Fail(string message, bool devGoofage = true)
	{
		_Failed = true;
		pnlError.Visible = true;
		litError.Text = (devGoofage ? "VideoControl: Dev Goofage: " : "") + message;
		if (devGoofage) // Only hide control if it's a dev goofage error
			pnlControl.Visible = false;
	}

}