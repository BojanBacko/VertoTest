using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tier : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //TEST
        ds.SelectParameters.Clear();
        ds.SelectParameters.Add("slug", Request.QueryString["slug"]);

        using (SqlDataReader dr = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty))
        {
            if (dr.Read())
            {
                Control ctrl;

                int PageLayout = Convert.ToInt32(dr["PageTemplate"]);
                switch (PageLayout)
                {
                    case 1:
                        {
                            ASP.controls_layout_tier_ascx ctrlLayout = new ASP.controls_layout_tier_ascx();
                            ctrlLayout.PageID = Convert.ToInt32(dr["id"]);
                            ctrlLayout.ParentID = Convert.ToInt32(dr["Parent"]);
                            ctrlLayout.PageTitle = dr["name"].ToString();
                            ctrlLayout.PageContent = dr["description"].ToString();

                            ctrl = ctrlLayout;
                            break;
                        }
                    default:
                        {
                            ASP.controls_layout_tier_ascx ctrlLayout = new ASP.controls_layout_tier_ascx();
                            ctrlLayout.PageID = Convert.ToInt32(dr["id"]);

                            ctrl = ctrlLayout;
                            break;
                        }
                }

                phLayout.Controls.Add(ctrl);
            }
        }
    }
}