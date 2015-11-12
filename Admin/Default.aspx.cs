using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label mpLabel;


        mpLabel =
            (Label)Master.FindControl("lblLoggedOnUser");
        if (mpLabel != null)
        {
            mpLabel.Text = Page.User.Identity.Name;
        }

        if (!IsPostBack)
        {
            ds.SelectCommand = string.Format("SELECT {0}, {1} FROM tblContent WHERE {2} = 0", CmsSettings.IDField, CmsSettings.TitleField, CmsSettings.ParentField);
            rptPages.DataSource = ds.Select(DataSourceSelectArguments.Empty);
            rptPages.DataBind();
        }
    }


    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("/admin/homepage.aspx");
    }
    protected void rptPages_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Response.Redirect("/admin/page.aspx?id=" + e.CommandArgument);
    }
}