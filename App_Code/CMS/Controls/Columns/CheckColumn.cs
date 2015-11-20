using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Controls.Columns {

    public class CheckColumn : Column {
        private CheckBox _chkBox;

        protected override void OnInit(EventArgs e) {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        protected override void CreateChildControls() {

            _chkBox = new CheckBox {
                ID = "chk"
            };

        }

        protected override void LoadControlState(object savedState) {
            _chkBox.Checked = (bool) savedState;
        }

        public override void GetColumnInner(HtmlTextWriter w, DataRow dataRow) {
            base.GetColumnInner(w, dataRow);
            EnsureChildControls();
            _chkBox.RenderControl(w);
        }

        protected override object SaveControlState() {
            return _chkBox.Checked;
        }

    }

}
