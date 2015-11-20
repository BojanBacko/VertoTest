using System.Data;
using System.Web.UI;

namespace CMS.Controls.Columns {

    public class HierarchyTextColumn : Column {
        private int _sortPartitionStringLength = 8;

        public string TextFieldName { get; set; }

        public string SortFieldName { get; set; }
        public int SortPartitionStringLength { get { return _sortPartitionStringLength; } set { _sortPartitionStringLength = value; } }

        public override void GetColumnInner(HtmlTextWriter w, DataRow dataRow) {

            var indent = 0;
            var sort = (string) dataRow[SortFieldName];

            if (sort.Length > 0) {
                indent = (sort.Length / SortPartitionStringLength) * 20;
            }

            w.Write("<div style=\"padding-left: {0}px;\">", indent);
            w.Write("<div class=\"hierarchical__control\"><i class=\"fa fa-plus-square-o\"></i></div>");
            w.Write("<div class=\"hierarchical__label\">{0}</div>", dataRow[TextFieldName]);
            w.Write("</div>");
        }

    }

}
