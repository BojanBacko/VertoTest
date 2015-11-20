using System.Web.UI;

namespace CMS.Controls.Columns {

    public interface IControlEventColumn {

        Control GetControl(int rowIndex);

    }

}
