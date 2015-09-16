using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)login.FindControl("Username");
        TextBox txtSearch = (TextBox)login.FindControl("Password");
        LinkButton btnSubmit = (LinkButton)login.FindControl("Login");

        txt.Focus();
        txtSearch.Attributes.Add("onfocus", "removeText(this.id)");
        txtSearch.Attributes.Add("onblur", "addText(this.id)");
        string strScript = "if(event.which || event.keyCode){{if ((event.which == 13) || (event.keyCode == 13)) {{document.getElementById('{0}').click();return false;}}}} else {{return true}};";
        string strSearchScript = String.Format(strScript, btnSubmit.ClientID);
        txtSearch.Attributes.Add("onkeydown", strSearchScript);
        txtSearch.Attributes.Add("onfocus", String.Format("RemoveText('{0}')", txtSearch.ClientID));
        txtSearch.Attributes.Add("onblur", String.Format("AddText('{0}')", txtSearch.ClientID));
    }

    protected void Login_Error(object sender, EventArgs e)
    {
        Literal litError = (Literal)login.FindControl("FailureText");
        litError.Visible = true;
        litError.Text = "Login Failed! Please check your details and try again.";
    }
}