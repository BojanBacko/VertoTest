using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomErrors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {

            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {

                switch (Request.QueryString["code"].ToString())
                {

                    case "404":

                        Response.TrySkipIisCustomErrors = true;
                        Response.StatusCode = 404;
                        Response.StatusDescription = "404 Page Not Found";

                        litTitle.Text = "Page Not Found";
                        litDescription.Text = "The page or file requested is no longer available. Please ensure that you have typed the correct URL or that the link provided to you is valid.";

                        break;

                    case "403":

                        Response.TrySkipIisCustomErrors = true;
                        Response.StatusCode = 403;
                        Response.StatusDescription = "403 Access Forbidden";

                        litTitle.Text = "Access Forbidden";
                        litDescription.Text = "The page or file requested is not available without the correct authentication.";

                        break;

                    case "500":

                        Response.TrySkipIisCustomErrors = true;
                        Response.StatusCode = 500;
                        Response.StatusDescription = "500 An error has occured and the developer has been notified";

                        litTitle.Text = "An Error Has Occurred";
                        litDescription.Text = "An error has occured and the developer has been notified. If you continue to receive this error please contact us.";

                        break;

                    default:
                        break;

                }

            }
        }
    }
}