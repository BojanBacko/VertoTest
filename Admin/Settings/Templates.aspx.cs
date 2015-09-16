using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Settings_Templates : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CMS.SelectCommand = "SELECT t.name, t.id AS pageTemplate, t.id FROM tblTemplates AS t";
    }
}