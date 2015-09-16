using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Users_Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!User.IsInRole("MasterAdmin"))
            Response.Redirect("/admin/");

        if (!IsPostBack)
        {
            UsersList.DataBind();

            if (Roles.IsUserInRole("MasterAdmin"))
            {
                AllRoles.DataSource = Roles.GetAllRoles();
                AllRoles.DataBind();
            }
            else
            {
                AllRoles.Items.Add(new ListItem("Admin"));
                AllRoles.Items.Add(new ListItem("Blog"));
            }
        }
    }

    protected MembershipUserCollection GetUsers()
    {
        if (AllRoles.SelectedValue == "All")
        {
            return Membership.GetAllUsers();
        }
        else if (AllRoles.SelectedValue == "None")
        {
            MembershipUserCollection muc = new MembershipUserCollection();
            foreach (MembershipUser m in Membership.GetAllUsers())
            {
                if (Roles.GetRolesForUser(m.UserName).Length == 0)
                {
                    muc.Add(m);
                }
            }
            return muc;
        }
        else
        {
            MembershipUserCollection muc = new MembershipUserCollection();
            foreach (MembershipUser m in Membership.GetAllUsers())
            {
                if (Roles.IsUserInRole(m.UserName, AllRoles.SelectedValue))
                {
                    muc.Add(m);
                }
            }
            return muc;
        }
    }

    protected SortedList GetRoles(string b)
    {
        SortedList sl = new SortedList();
        foreach (string a in Roles.GetAllRoles())        
            sl.Add(a, b);
        return sl;
    }

    public string GetText(string role, string username)
    {
        if (Roles.IsUserInRole(username, role))
            return "Remove from " + role;
        else
            return "Add to " + role;
    }

    protected void Remove_Command(object sender, CommandEventArgs e)
    {
        Membership.DeleteUser(e.CommandArgument.ToString());
        UsersList.DataBind();
    }

    protected void Toggle(object sender, CommandEventArgs e)
    {
        if (Roles.IsUserInRole(e.CommandArgument.ToString().Split(',')[1], e.CommandArgument.ToString().Split(',')[0]))
        {
            Roles.RemoveUserFromRole(e.CommandArgument.ToString().Split(',')[1], e.CommandArgument.ToString().Split(',')[0]);
        }
        else
        {
            Roles.AddUserToRole(e.CommandArgument.ToString().Split(',')[1], e.CommandArgument.ToString().Split(',')[0]);
        }
        UsersList.DataBind();
    }
}