using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Users_Roles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Roles.IsUserInRole("MasterAdmin"))
        {
            UsersList.DataBind();
        }
        else { Response.Redirect("/admin/"); }
    }
    protected void RemoveRole_Command(object sender, CommandEventArgs e)
    {
        Roles.DeleteRole(e.CommandArgument.ToString());
    }
    protected void AddRole_Click(object sender, EventArgs e)
    {
        Roles.CreateRole(txtRoleName.Text);
        Response.Redirect(Request.RawUrl);
    }
}