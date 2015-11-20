#region

using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;

#endregion

namespace CMS.Controls.Columns {

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Column : Control {
        private DataRow _data;

        public delegate void ColumnBindEvent(HtmlTextWriter w, DataRow dr, Control source);
        public event ColumnBindEvent ColumnBinding;

        public string HeaderText { get; set; }
        public int Width { get; set; }

        protected override void OnInit(EventArgs e) {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        public virtual void GetColumnInner(HtmlTextWriter w, DataRow dr) {
            _data = dr;
            if (ColumnBinding != null)
                ColumnBinding(w, dr, this);
        }
        
    }

}