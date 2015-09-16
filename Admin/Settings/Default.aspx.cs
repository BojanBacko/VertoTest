using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Settings_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ds.SelectCommand = "SELECT * FROM tblTemplates";
            ds.SelectParameters.Clear();
            ddlTemplate.DataSource = ds.Select(DataSourceSelectArguments.Empty);
            ddlTemplate.DataBind();
        }

        CMS0.SelectCommand = "SELECT * FROM tblModule ORDER BY ModuleOrder";
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        ddlTemplate.SelectedValue = Request.QueryString["id"];
    }
    public void AssignSeries(object sender, EventArgs e)
    {
        CheckBox chkbox = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkbox.NamingContainer;
        string ModuleID = gvModules.DataKeys[gvr.RowIndex].Values["ModuleID"].ToString();
        string TemplateID = ddlTemplate.SelectedValue;

        ds.InsertParameters["ModuleID"].DefaultValue = ModuleID;
        ds.InsertParameters["TemplateID"].DefaultValue = TemplateID;

        ds.DeleteParameters["ModuleID"].DefaultValue = ModuleID;
        ds.DeleteParameters["TemplateID"].DefaultValue = TemplateID;

        if (chkbox.Checked) {ds.Insert();}
        else{ds.Delete();}
    }


    protected void gvModules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkbox = (CheckBox)e.Row.FindControl("chk");
            string ModuleID = DataBinder.Eval(e.Row.DataItem, "ModuleID").ToString();
            string TemplateID = Request.QueryString["id"] ?? "1";
            dsSeriesOptions.SelectParameters.Clear();
            dsSeriesOptions.SelectCommand = "SELECT * FROM tblModuleLink WHERE TemplateID = @TemplateID and ModuleID = @ModuleID";
            dsSeriesOptions.SelectParameters.Add("ModuleID", ModuleID);
            dsSeriesOptions.SelectParameters.Add("TemplateID", TemplateID);
            using (SqlDataReader dr = (SqlDataReader)dsSeriesOptions.Select(DataSourceSelectArguments.Empty))
            {
                if (dr.HasRows)
                {
                    chkbox.Checked = true;
                }
            }
        }
    }
    protected void btnAddModule_Click(object sender, EventArgs e)
    {
        dsModules.Insert();
        Response.Redirect(Request.RawUrl);
    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("default.aspx?id=" + ddlTemplate.SelectedValue);
    }
}