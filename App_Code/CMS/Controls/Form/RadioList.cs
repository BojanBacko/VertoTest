using System.Collections.Generic;
using System.Data;

namespace CMS.Controls.Form {

    public sealed class RadioList : System.Web.UI.WebControls.RadioButtonList, IBindingControl {
        
        public void BindValueToControl(string col, DataRow data, string defaultValue = "") {
            SelectedValue = data[col].ToString();
        }

        public Dictionary<string, object> GetControlValues(string col) {
            return new Dictionary<string, object> { { col, SelectedValue } };
        }

    }

}