using System;
using System.Web.UI;

public partial class Mastertemplate_MasterPage : MasterPage {

    protected void Page_Load(object sender, EventArgs e) {
        form1.Action = Request.RawUrl; 
    }

}
