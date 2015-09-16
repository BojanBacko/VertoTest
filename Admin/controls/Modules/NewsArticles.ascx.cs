using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_NewsArticles : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ParameterCollection selectParams = new ParameterCollection();
        CMS.SelectCommand = "SELECT * FROM tblContent WHERE parent = @parent";
        selectParams.Add(new QueryStringParameter("parent", "id"));
        CMS.GridviewSelectParameters = selectParams;

        CMS.InsertParameters = selectParams;
        CMS.UpdateParameters = selectParams;

        CMS.AddPageDestinationURL = Request.RawUrl;

    }
}