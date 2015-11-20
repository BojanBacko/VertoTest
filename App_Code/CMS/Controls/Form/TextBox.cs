using System.Collections.Generic;
using System.Data;

namespace CMS.Controls.Form {

    public class TextBox : System.Web.UI.WebControls.TextBox, IBindingControl {

        public void BindValueToControl(string col, DataRow data, string defaultValue = "") {

            if (data != null) {

                Text = data[col].ToString();

            }
            else {

                Text = defaultValue;

            }

        }

        public Dictionary<string, object> GetControlValues(string col) {
            return new Dictionary<string, object> { { col, Text } };
        }

    }

}