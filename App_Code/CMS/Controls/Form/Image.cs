using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Context;

namespace CMS.Controls.Form {

    public class Image : CompositeControl, IBindingControl {
        private string _directory;
        private string _extension;
        private string _assetRoot = "/assets";
        private string _imagePrefix = "img-";

        public int AssetID { get; set; }
        public string AssetRoot { get { return _assetRoot; } set { _assetRoot = value; } }
        public string ImagePrefix { get { return _imagePrefix; } set { _imagePrefix = value; } }
        public string ImageSuffix { get; set; }

        public new int Width { get; set; }
        public new int Height { get; set; }

        public void BindValueToControl(string col, DataRow data, string defaultValue = "") {
            AssetID = (int)data[col];

            var prov = new AssetDataContext();
            var asset = prov.Load(AssetID);

            if (asset != null) {
                _directory = (string) asset["directory"];
                _extension = (string) asset["extension"];
            }

        }

        public Dictionary<string, object> GetControlValues(string col) {
            return new Dictionary<string, object> { { col, AssetID } };
        }

        protected override void Render(HtmlTextWriter w) {

            w.WriteLine("<div style=\"clear:both\" class=\"control control--cms-image\">");

            w.WriteLine("<img src=\"{0}{1}{2}{3}{4}.{5}\" alt=\"image preview\" onerror=\"this.src = 'http://img.vertouk.com/{6}x{7}/?type=mini'\" />", _assetRoot, _directory, _imagePrefix, AssetID, ImageSuffix, _extension, Width, Height);

            w.WriteLine("</div>");

        }

    }

}