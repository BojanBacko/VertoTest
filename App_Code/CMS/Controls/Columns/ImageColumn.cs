using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Controls.Columns {

    /// <summary>
    /// Summary description for ImageColumn
    /// </summary>
    public class ImageColumn : Column {

        private string _assetRoot = "/assets";
        private string _imagePrefix = "img-";
        private string _imageSuffix = "-cms";
        private int _maxHeight = 65;

        public string AssetRoot { get { return _assetRoot; } set { _assetRoot = value; } }
        public string ImagePrefix { get { return _imagePrefix; } set { _imagePrefix = value; } }
        public string ImageSuffix { get { return _imageSuffix; } set { _imageSuffix = value; } }

        public int MaxHeight { get { return _maxHeight; } set { _maxHeight = value; } }

        public override void GetColumnInner(HtmlTextWriter w, DataRow dr) {
            
            w.WriteLine("<img src=\"{0}{1}{2}{3}{4}.{5}\" alt=\"image preview\" onerror=\"this.src = 'http://img.vertouk.com/{6}x{7}/?type=mini'\" style=\"max-height:{7}px;\" />", _assetRoot, dr["directory"], _imagePrefix, dr["assetID"], _imageSuffix, dr["extension"], Width > 0 ? Width : 120, _maxHeight);

        }

    }

}