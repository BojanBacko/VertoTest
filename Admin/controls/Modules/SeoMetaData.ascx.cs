using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_controls_Layout_PageContent : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CMS.ContentID = Convert.ToInt32(Request.QueryString["id"]);
    }
}