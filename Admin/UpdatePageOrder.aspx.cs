using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UpdatePageOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (string.IsNullOrWhiteSpace(Request["table"]) || string.IsNullOrWhiteSpace(Request["sort-column"]) || string.IsNullOrWhiteSpace(Request["id-column"]) || string.IsNullOrWhiteSpace(Request["records"]))
			Response.End();

		Regex rgx = new Regex("[^a-zA-Z0-9_]");

		string 
			table = rgx.Replace(Request["table"], ""),
			orderColumn = rgx.Replace(Request["sort-column"] ?? "pageOrder", ""),
			idColumn = rgx.Replace(Request["id-column"] ?? "id", "");

		using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL"].ToString()))
		{
			conn.Open();
			using (SqlCommand q = new SqlCommand("UPDATE [" + table + "] SET [" + orderColumn + "] = @i WHERE [" + idColumn + "] = @id", conn))
			{
				string[] records = Request["records"].Split(';');
				foreach (string r in records)
				{
					q.Parameters.Clear();
					if (r.Contains(":"))
					{
						q.Parameters.AddWithValue("i", r.Split(':')[1]);
						q.Parameters.AddWithValue("id", r.Split(':')[0]);
						try
						{
							q.ExecuteNonQuery();
						}
						catch (Exception err)
						{
							Response.Write(err.Message);
							Response.End();
						}
					}
				}
			}
		}

		Response.Write("true");
		Response.End();
    }
}