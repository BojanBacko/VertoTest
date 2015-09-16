using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class controls_Layout_Tier : ControlProperties
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        litTitle.Text = PageTitle;
        litDescription.Text = PageContent;
    }
}