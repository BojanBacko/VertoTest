using System.Data;
using System.Web.UI;

namespace CMS.Controls.Columns {

    public class TextColumn : Column {

        public string FieldName { get; set; }

        public override void GetColumnInner(HtmlTextWriter w, DataRow dataRow) {
            base.GetColumnInner(w, dataRow);
            w.Write(dataRow[FieldName].ToString());
        }

    }

}
