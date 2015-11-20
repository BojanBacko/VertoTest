using System.Data;
using System.Web.UI;

namespace CMS.Controls.Columns {

    public class HyperlinkColumn : Column {

        public string LinkText { get; set; }
        public string UrlFormatString { get; set; }
        public string FieldName { get; set; }

        public override void GetColumnInner(HtmlTextWriter w, DataRow dataRow) {
            base.GetColumnInner(w, dataRow);
            w.WriteLine("<a href=\"" + UrlFormatString + "\">{1}</a>", dataRow[FieldName], LinkText);
        }

    }

}