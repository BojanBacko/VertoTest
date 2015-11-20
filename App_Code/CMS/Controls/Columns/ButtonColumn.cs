using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Controls.Columns {

    public class ButtonColumn : Column, IControlEventColumn {

        public string ButtonText { get; set; }
        public string CommandName { get; set; }
        public string OnClientClick { get; set; }

        public Control GetControl(int rowIndex) {

            var btn = new Button {
                ID = string.Format("btn_{0}_{1}", ButtonText.Replace(" ", ""), rowIndex),
                Text = ButtonText,
                CommandName = CommandName,
                CommandArgument = rowIndex.ToString(CultureInfo.InvariantCulture),
                OnClientClick = OnClientClick
            };

            return btn;

        }

    }

}