using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Users_NewUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.IsInRole("MasterAdmin"))
            Response.Redirect("/admin/");

        if (!IsPostBack)
        {
            ddlRole.DataSource = Roles.GetAllRoles();
            ddlRole.DataBind();
        }
    }
    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        if (txtPassword.Text == txtConfirmPassword.Text)
        {
            try
            {
                Membership.CreateUser(txtUsername.Text, txtPassword.Text, txtEmail.Text);

                if (ddlRole.SelectedValue.ToString().ToLower() != "none")
                Roles.AddUserToRole(txtUsername.Text, ddlRole.SelectedValue);
            }
            catch (MembershipCreateUserException ex)
            {
                litError.Text = ex.Message.ToString();
            }
        }
        else 
        {
            litError.Text = "The paswords you have entered do not match.";
        }
        Response.Redirect("/admin/users/");
    }

}