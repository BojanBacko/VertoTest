using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Pages : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label mpLabel;
        if (User.IsInRole("Blog"))
            Response.Redirect("/admin/");

        mpLabel =
            (Label)Master.FindControl("lblLoggedOnUser");
        if (mpLabel != null)
        {
            mpLabel.Text = Page.User.Identity.Name;
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