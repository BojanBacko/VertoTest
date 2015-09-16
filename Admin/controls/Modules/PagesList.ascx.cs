using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_PagesList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CMS.SelectCommand = "SELECT * FROM tblContent WHERE parent = " + Convert.ToInt32(Request.QueryString["id"]);
    }
}