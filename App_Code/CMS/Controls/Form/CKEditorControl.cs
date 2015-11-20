using System.Collections.Generic;
using System.Data;

namespace CMS.Controls.Form {

    public class CKEditorControl : CKEditor.NET.CKEditorControl, IBindingControl {

        public void BindValueToControl(string col, DataRow data, string defaultValue = "") {
            Text = data[col].ToString();
        }

        public Dictionary<string, object> GetControlValues(string col) {
            return new Dictionary<string, object> { { col, Text  } };
        }

    }

}