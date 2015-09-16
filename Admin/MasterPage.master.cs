using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        phBlog.Visible = false;
        litMainSectionTitle.Text = "CONTENT";
        if (Roles.IsUserInRole("MasterAdmin"))
        {
            phRoles.Visible = true;
            phMasterAdmin.Visible = true;
        }
        else if (Roles.IsUserInRole("Blog"))
        {
            phRoles.Visible = false;
            phMasterAdmin.Visible = false;
            phUsers.Visible = false;
            phBlog.Visible = true;
            phPages.Visible = false;
            litMainSectionTitle.Text = "THINGS WE LIKE";
        }

        ds.SelectCommand = string.Format("SELECT {0}, {1} FROM tblContent WHERE {2} = 0", CmsSettings.IdField, CmsSettings.TitleField, CmsSettings.ParentField);
        rptNav.DataSource = ds.Select(DataSourceSelectArguments.Empty);
        rptNav.DataBind();
        //Response.Write(ds.SelectCommand);
        lblLoggedOnUser.Text = HttpContext.Current.User.Identity.Name; 

    }
}
