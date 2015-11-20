using System.Collections.Generic;
using System.Web.UI;

namespace CMS.Controls {

    public class CMSModule : UserControl {
    
        public int SelectedID { get; set; }
        public Dictionary<string, object> ModuleBag { get; set; }

    }

}