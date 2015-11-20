using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Controls.Form {

    public class Text : CompositeControl, IBindingControl {

        protected string InnerHtml = "";

        public void BindValueToControl(string col, DataRow data, string defaultValue = "") {

            if (data != null) {
                InnerHtml = data[col].ToString();
                return;
            }

            InnerHtml = defaultValue;

        }

        public Dictionary<string, object> GetControlValues(string col) {
            return null;
        }

        protected override void Render(HtmlTextWriter w) {

            w.WriteLine("<div class=\"formatted\">");
            w.WriteLine(InnerHtml);
            w.WriteLine("</div>");

        }

    }

}