using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page : System.Web.UI.Page
{

    private bool hasChildren = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Label mpLabel;


        //mpLabel = (Label)Master.FindControl("lblLoggedOnUser");
        //if (mpLabel != null)
        //{
        //    mpLabel.Text = Page.User.Identity.Name;
        //}
        if (Session["msg"] != null && Session["type"] != null)
        {
            if (Session["type"] == "1")
            { 
                AlertSuccess(Session["msg"].ToString());
                Session["msg"] = null;
                Session["type"] = null;
            }
        }
        if (!IsPostBack)
        {
            using (SqlDataReader dr = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    dsModules.SelectParameters["TemplateID"].DefaultValue = dr["pageTemplate"].ToString();
                    litTitle.Text = functions.RemoveHTML(dr[CmsSettings.TitleField].ToString());
                }
            }
        }

        ds.SelectCommand = "SELECT COUNT(" + CmsSettings.IdField + ") AS count FROM tblContent WHERE " + CmsSettings.ParentField + " = " + Convert.ToInt32(Request.QueryString["id"]);

        using (SqlDataReader drCount = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty))
        {
            if (drCount.Read())
            {
                if (Convert.ToInt32(drCount["count"]) > 0)
                    hasChildren = true;
            }
        }


        GetModules();

        GetBreadcrumb();

    }

    private void GetModules()
    {
        using (SqlDataReader dr = (SqlDataReader)dsModules.Select(DataSourceSelectArguments.Empty))
        {

            rptTabs.DataSource = dsModules.Select(DataSourceSelectArguments.Empty);
            rptTabs.DataBind();

            int tabNo = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tabNo++;
                    Panel div = new Panel();
                    div.ID = "tab" + tabNo;
                    div.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    div.CssClass = "tab_content";
                    div.Controls.Add(this.LoadControl("/admin/controls/modules/" + dr["ModuleFile"].ToString()));

                    if (dr["ModuleFile"].ToString().ToLower() == "pageslist.ascx" && !hasChildren)
                    { }
                    else
                        phModules.Controls.Add(div);

                }
            }
        }

    }

    private void GetBreadcrumb()
    {
        ds.DataSourceMode = SqlDataSourceMode.DataSet;
        ds.SelectCommand = string.Format("SELECT {0}, {1}, {2}, {3} FROM tblContent", CmsSettings.IdField, CmsSettings.ParentField, CmsSettings.TitleField, CmsSettings.SlugField);
        DataView dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
        DataTable dt = dv.Table;
        StringBuilder sb = new StringBuilder();
        StringBuilder sbBreadcrumb = new StringBuilder();
        DataRow []r = dt.Select(CmsSettings.IdField + "=" + Convert.ToInt32(Request.QueryString["id"]));

        int count = 0;

        while (r.GetUpperBound(0) > -1)
        {
            count++;
            if (count == 1)
            {
                sb.Append(" " + CmsSettings.IdField + " = " + r[0][CmsSettings.IdField].ToString());
            }
            else
            {
                sb.Append(" OR " + CmsSettings.IdField + " = " + r[0][CmsSettings.IdField].ToString());
            }
            r = dt.Select(CmsSettings.IdField + "=" + Convert.ToInt32(r[0][CmsSettings.ParentField]));
        }

        ds.DataSourceMode = SqlDataSourceMode.DataReader;
        ds.SelectCommand = string.Format("SELECT {0}, {1}, {2} FROM tblContent WHERE " + sb.ToString() + " ORDER BY {0}, {1}", CmsSettings.IdField, CmsSettings.ParentField, CmsSettings.TitleField);

        using (SqlDataReader drB = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty))
        {
            if (drB.HasRows)
            {
                while (drB.Read())
                {
                    sbBreadcrumb.AppendLine(String.Format("<a href=\"/admin/page.aspx?id={0}\">> {1}</a>", drB[CmsSettings.IdField].ToString(), drB[CmsSettings.TitleField].ToString()));
                }
            }
        }

        litBreadcrumb.Text = sbBreadcrumb.ToString();
    }

    protected void AlertInfo(string msg)
    {
        messagesText.Attributes["class"] = "alert_info";
        lblMessage.Text = msg;
        panelMessage.Visible = true;
    }

    protected void AlertWarning(string msg)
    {
        messagesText.Attributes["class"] = "alert_warning";
        lblMessage.Text = msg;
        panelMessage.Visible = true;
    }

    protected void AlertError(string msg)
    {
        ErrorMessagesText.Attributes["class"] = "alert_error";
        lblMessageError.Text = msg;
        panelMessageError.Visible = true;
    }

    protected void AlertSuccess(string msg)
    {
        messagesText.Attributes["class"] = "alert_success";
        lblMessage.Text = msg;
        panelMessage.Visible = true;
    }
    protected void rptTabs_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            PlaceHolder phLi = (PlaceHolder)e.Item.FindControl("phLi");

            if (DataBinder.Eval(e.Item.DataItem, "modulefile").ToString().ToLower() == "pageslist.ascx" && !hasChildren)
            {
                phLi.Visible = false;
            }
        }
    }
}