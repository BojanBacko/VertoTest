using System.Collections.Generic;
using System.Data;

namespace CMS.Controls.Form {

    public class CheckBox : System.Web.UI.WebControls.CheckBox, IBindingControl {

        public void BindValueToControl(string col, DataRow data, string defaultValue = "") {
            Checked = (bool) data[col];
        }

        public Dictionary<string, object> GetControlValues(string col) {
            return new Dictionary<string, object> { { col, Checked } };
        }

    }

}