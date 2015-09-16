using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Admin_Users_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void ChangePassword_Click(object sender, EventArgs e)
    {
        MembershipUser mu = Membership.GetUser(User.Identity.Name);
        if (mu.GetPassword() == txtCurrentPassword.Text)
        {
            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                mu.ChangePassword(txtCurrentPassword.Text, txtPassword.Text);
            }
            else { litError.Text = "Your new passwords do not match, please try again."; }
        }
        else { litError.Text = "The current password entered is incorrect, please try again."; }
    }
}