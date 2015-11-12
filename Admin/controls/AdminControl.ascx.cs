using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Columns;
using System.Collections.ObjectModel;
using Buttons;
using System.Web.Configuration;
using System.Web.Services;

public partial class Admin_Controls_AdminControl : AdminFunctions
{
	private int count = 0;


	#region Contstants

	#endregion

	#region properties

	public enum CmsMode { Select, Insert, Grid }

	public CmsMode Mode { get; set; }

	public enum CmsAddType { Inline, Separate }

	public CmsAddType AddType { get; set; }

	public enum CmsAddPageDestinationType { StayOnPage, Redirect }

	public CmsAddPageDestinationType AddPageDestinationType { get; set; }

	public ParameterCollection InsertParameters { get; set; }
	public ParameterCollection SelectParameters { get; set; }
	public ParameterCollection GridviewSelectParameters { get; set; }
	public ParameterCollection UpdateParameters { get; set; }

	public event OnButtonClickEventHandler OnButtonClick;
	public delegate void OnButtonClickEventHandler(string strValue);

	private Collection<Column>
		_columns = new Collection<Column>();

	[PersistenceMode(PersistenceMode.InnerProperty)]
	public Collection<Column> Columns { get { return _columns; } set { _columns = value; } }


	private Collection<CustomButton>
		_buttons = new Collection<CustomButton>();

	[PersistenceMode(PersistenceMode.InnerProperty)]
	public Collection<CustomButton> Buttons { get { return _buttons; } set { _buttons = value; } }

	public bool isInsertable { get; set; }
	public PagedDataSource dtv;

	public int i { get; set; }
	public int ContentID { get; set; }
	public int intCount { get; set; }
	public int CurrentValue { get; set; }
	public int PageSize { get; set; }
	public string ParentQueryStringField { get; set; }

	public string TableName { get; set; }
	public string Filter { get; set; }
	public string ID { get; set; }
	//public string FilePath { get; set; }
	//public string Mode { get; set; }
	public string EmptyMessage { get; set; }
	//public string SectionTitle { get; set; }
	//public string LinksURL { get; set; }
	//public string ImgURL { get; set; }
	public string AddPageDestinationURL { get; set; }

	//1=Other Page ; 2=Same Page
	//public int AddType { get; set; }

	//OverrideFields
	public string ParentFieldOverride { get; set; }
	public string SlugFieldOverride { get; set; }
	public string IdFieldOverride { get; set; }
	public string TitleFieldOverride { get; set; }
	//end override

	public bool CheckDuplicates { get; set; }

	public string[] DuplicateFieldsToCheck { get; set; }
	public string ValidationGroup { get; set; }
	public bool PublishEnabled { get; set; }
	public string PublishColumn { get; set; }

	public string SelectCommand { get; set; }
	//public string OrderColumn { get; set; }
	//public string OrderColumnText { get; set; }
	public string SortColumn { get; set; }

	//public bool AllowOrdering { get; set; }

	public bool ShowSaveButton = true;
	//public bool AllowFiltering { get; set; }
	//public bool DeleteEnabled { get; set; }
	public bool InsertEnabled { get; set; }
	public bool AllowPaging { get; set; }
	public int PageIndex { get; set; }
	public int PageTotal { get; set; }

	//public string templateField { get; set; }
	public string AddButtonText { get; set; }
	public string IdName { get; set; }
	public string[] DataKeyNames { get; set; }
	private string AddedValue = "";

	private string updateString = "";
	private string insertColumnsString = "(";
	private string insertValuesString = "(";

	private string SlugValue = "";

	/// <summary>
	/// Enable drag + drop ordering?
	/// </summary>
	public bool EnableDragOrdering = true;

	/// <summary>
	/// Sort direction (ASC or DESC)
	/// </summary>
	public string SortDirection = "ASC";

	#endregion

	#region ITemplate

	private ITemplate m_messageTemplate = null;
	[TemplateContainer(typeof(MessageContainer))]
	public ITemplate FormFields
	{
		get { return m_messageTemplate; }
		set { m_messageTemplate = value; }
	}

	public MessageContainer ic;


	public class MessageContainer : Control, INamingContainer
	{
	}

	public string GetFormViewValue(string column)
	{
		using (DataView dtv = (DataView)FormViewSource.Select(DataSourceSelectArguments.Empty))
		{
			return dtv[0][column].ToString();
		}

	}
	#endregion

	#region GridViewSettings

	protected void gvPages_Row_Command(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
	{
		if (e.CommandName == "PageLayout")
		{
			int PageID = (int)((GridView)sender).DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value;
			string[] arrArgs = e.CommandArgument.ToString().Split(';');
			Session["currentPath"] = Request.RawUrl.ToString();
			Response.Redirect(String.Format("Page.aspx?id={0}", PageID));
		}
		else if (e.CommandName.ToLower() == "moveup")
		{
			int PageID = (int)((GridView)sender).DataKeys[Convert.ToInt32((e.CommandArgument ?? CmsSettings.IDField).ToString())].Value;
			using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL"].ToString()))
			{
				conn.Open();
				using (SqlCommand q = new SqlCommand(string.Empty, conn))
				{
					q.CommandText = string.Format("UPDATE [{0}] SET [{1}] = ([{1}] + 1) WHERE [{2}] = (SELECT TOP 1 [{2}] FROM [{0}] WHERE [{1}] < (SELECT [{1}] FROM [{0}] WHERE [{2}] = '{3}') AND [{4}] = '{5}' ORDER BY [{1}] DESC); UPDATE [{0}] SET [{1}] = ([{1}] - 1) WHERE [{2}] = '{3}';", TableName, SortColumn, IdFieldOverride ?? CmsSettings.IDField, PageID, ParentFieldOverride ?? CmsSettings.ParentField, Request.QueryString["id"].ToString());
					q.ExecuteNonQuery();
				}
			}
			gvPages.DataBind();
		}
		else if (e.CommandName.ToLower() == "movedown")
		{
			int PageID = (int)((GridView)sender).DataKeys[Convert.ToInt32((e.CommandArgument ?? CmsSettings.IDField).ToString())].Value;
			using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL"].ToString()))
			{
				conn.Open();
				using (SqlCommand q = new SqlCommand(string.Empty, conn))
				{
					q.CommandText = string.Format("UPDATE [{0}] SET [{1}] = ([{1}] - 1) WHERE [{2}] = (SELECT TOP 1 [{2}] FROM [{0}] WHERE [{1}] > (SELECT [{1}] FROM [{0}] WHERE [{2}] = '{3}') AND [{4}] = '{5}' ORDER BY [{1}] ASC); UPDATE [{0}] SET [{1}] = ([{1}] + 1) WHERE [{2}] = '{3}';", TableName, SortColumn, IdFieldOverride ?? CmsSettings.IDField, PageID, ParentFieldOverride ?? CmsSettings.ParentField, Request.QueryString["id"].ToString());
					q.ExecuteNonQuery();
				}
			}
			gvPages.DataBind();
		}
		else if (e.CommandName.ToLower() == "publish")
		{
			int PageID = (int)((GridView)sender).DataKeys[Convert.ToInt32((e.CommandArgument ?? CmsSettings.IDField).ToString())].Value;
			using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL"].ToString()))
			{
				conn.Open();
				using (SqlCommand q = new SqlCommand(string.Empty, conn))
				{
					q.CommandText = string.Format("UPDATE [{0}] SET [{1}] = [{1}] ^ 1 WHERE [{2}] = '{3}'", TableName, PublishColumn ?? "published", IdFieldOverride ?? CmsSettings.IDField, PageID);
					q.ExecuteNonQuery();
				}
			}
		}

	}


	protected void GridViewDeleting(object sender, GridViewDeleteEventArgs e)
	{
		GridViewSource.DeleteParameters.Clear();
		GridViewSource.DeleteParameters.Add(IdName, gvPages.DataKeys[e.RowIndex].Value.ToString());

		try
		{
			GridViewSource.Delete();
		}
		catch (Exception err)
		{
			if (err.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
				Page.ClientScript.RegisterStartupScript(this.GetType(), "load", "$(function() { ep.alert({ type: 'error', size: 32, title: \"Can't delete this record yet\", content: \"<span style='color:#900;'>This record is currently featured on another page (e.g. the home page).<br/>You must remove this record from other pages before deleting it.<br/><br/>You can see the constraint name below, this may aid you in locating the feature page.<br/><br/><strong>" + err.Message.Split('"')[1] + "</strong></span>\" }); });", true);
			else
				throw err;
		}
	}

	protected void gvPages_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		gvPages.PageIndex = e.NewPageIndex;
		gvPages.DataBind();
	}

	protected void gvPages_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			foreach (TableCell c in e.Row.Cells)
			{
				if (c.Controls.Count > 0)
				{
					ImageButton deleteImageButtonField = c.Controls[0] as ImageButton;
					if (deleteImageButtonField != null && (deleteImageButtonField.CommandName.ToLower() == "delete" || deleteImageButtonField.CommandName.ToLower().Contains("delete")))
					{
						deleteImageButtonField.Attributes["onclick"] =
										string.Format("if(confirm('Are you sure you want to delete this item?')) __doPostBack('{0}','{1}${2}'); else return false;",
														gvPages.ClientID,
														deleteImageButtonField.CommandName,
														deleteImageButtonField.CommandArgument);
					}

					Button deleteButtonField = c.Controls[0] as Button;
					if (deleteButtonField != null && (deleteButtonField.CommandName.ToLower() == "delete" || deleteButtonField.CommandName.ToLower().Contains("delete")))
					{
						deleteButtonField.Attributes["onclick"] =
										string.Format("if(confirm('Are you sure you want to delete this item?')) __doPostBack('{0}','{1}${2}'); else return false;",
														gvPages.ClientID,
														deleteButtonField.CommandName,
														deleteButtonField.CommandArgument);
					}

					LinkButton deleteLinkButtonField = c.Controls[0] as LinkButton;
					if (deleteLinkButtonField != null && (deleteLinkButtonField.CommandName.ToLower() == "delete" || deleteLinkButtonField.CommandName.ToLower().Contains("delete")))
					{
						deleteLinkButtonField.Attributes["onclick"] =
										string.Format("if(confirm('Are you sure you want to delete this item?')) __doPostBack('{0}','{1}${2}'); else return false;",
														gvPages.ClientID,
														deleteLinkButtonField.CommandName,
														deleteLinkButtonField.CommandArgument);
					}

					ImageButton ib = c.Controls[0] as ImageButton;
					DataRowView row = (DataRowView)e.Row.DataItem;
					if (ib != null && ib.CommandName.ToLower() == "moveup" && e.Row.RowIndex == 0)
						ib.Visible = false;
					if (ib != null && ib.CommandName.ToLower() == "movedown" && e.Row.RowIndex == row.DataView.Count - 1)
						ib.Visible = false;
				}
			}
		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			DataRow row = ((DataRowView)e.Row.DataItem).Row;
			if (row.Table.Columns.Contains(PublishColumn == null ? "published" : PublishColumn) && PublishEnabled)
			{
                bool name = row.Field<bool>((PublishColumn == null ? "published" : PublishColumn));
				if (!name)
					e.Row.Style["background-color"] = "#FFDBDB";

				foreach (TableCell c in e.Row.Cells)
				{
					if (c.Controls.Count > 0)
					{
						ImageButton ib = c.Controls[0] as ImageButton;
						if (ib != null && ib.CommandName.ToLower() == "publish" && !PublishEnabled)
						{
							ib.Visible = false;
							if (name)
								ib.ToolTip = "Publish";
							else
								ib.ToolTip = "Unpublish";
						}
					}
				}
			}
			e.Row.Attributes["data-id"] = ((DataRowView)e.Row.DataItem).Row.Field<Int32>(IdFieldOverride ?? CmsSettings.IDField).ToString(); // Drag + drop ordering
		}
	}

	#endregion

	#region FormviewProperties

	protected void Cancel(object sender, System.EventArgs e)
	{
		fvControls.Visible = false;
		gvPages.Visible = true;
		btnAdd.Visible = InsertEnabled;
	}

	protected void ChangeMode(object sender, System.EventArgs e)
	{
		fvControls.ChangeMode(FormViewMode.Insert);
		fvControls.DataBind();
		InsertTemplate(false);
		fvControls.Visible = true;
		gvPages.Visible = false;
		btnAdd.Visible = false;
	}

	protected void ChangeRow(object sender, System.EventArgs e)
	{
		fvControls.ChangeMode(FormViewMode.Edit);
		FormViewSource.SelectParameters["id"].DefaultValue = gvPages.SelectedValue.ToString();
		fvControls.DataBind();
		InsertTemplate(true);
		fvControls.Visible = true;
		gvPages.Visible = false;
		btnAdd.Visible = false;
		CurrentValue = Convert.ToInt32(gvPages.SelectedValue);
	}

	protected PagedDataSource GridViewGrab()
	{
		dtv = new PagedDataSource();
		DataView dtv2 = (DataView)GridViewSource.Select(DataSourceSelectArguments.Empty);
		dtv2.Sort = SortColumn + (!string.IsNullOrWhiteSpace(SortDirection) ? " " + SortDirection : "");
		//dtv.AllowPaging = AllowPaging;
		//dtv.PageSize = PageSize;
		dtv.CurrentPageIndex = PageIndex;
		dtv.DataSource = dtv2;
		PageTotal = dtv.PageCount;
		return dtv;
	}

	protected void CheckFull(object sender, System.EventArgs e)
	{
		MessageContainer ItemContainer = new MessageContainer();
		FormFields.InstantiateIn(ItemContainer);

		foreach (Control c in ItemContainer.Controls)
		{
			if (c.GetType().ToString() == "ASP.admin_controls_videocontrol_ascx")
			{
				Admin_controls_VideoControl ctrl = (Admin_controls_VideoControl)c;
				if (string.IsNullOrWhiteSpace(ctrl.Table))
					ctrl.Table = TableName;
				if (string.IsNullOrWhiteSpace(ctrl.IdField))
					ctrl.IdField = IdName;
			}
		}

		((PlaceHolder)sender).Controls.Add(ItemContainer);
	}

	#endregion

	#region Formview Control Templates

	protected void InsertTemplate(bool GrabFromDb)
	{
		try
		{
			MessageContainer ItemContainer = new MessageContainer();
			FormFields.InstantiateIn(ItemContainer);
			if (((DataView)GridViewSource.Select(DataSourceSelectArguments.Empty)).Count > 0 && ((DataView)FormViewSource.Select(DataSourceSelectArguments.Empty)).Count > 0)
			{
				if (GrabFromDb)
				{
					DataRowView RowFields = ((DataView)FormViewSource.Select(DataSourceSelectArguments.Empty))[0];

					foreach (Control c in ItemContainer.Controls)
					{
						i++;
						if (c.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
						{
							TextBox txt = ((TextBox)c);
							if (txt.ID.ToString().Contains("date"))
								txt.Text = String.Format("{0:dd/MM/yyyy}", (DateTime)RowFields[c.ID.ToString()]);
							else if (txt.ID.ToLower().ToString().Contains("time"))
								txt.Text = String.Format("{0:HH:mm}", (DateTime)RowFields[c.ID.ToString()]);
							else
								txt.Text = RowFields[c.ID.ToString()].ToString();
						}
						else if (c.GetType().ToString() == "System.Web.UI.WebControls.Literal")
						{
							((Literal)c).Text = RowFields[c.ID.ToString()].ToString();
						}
						else if (c.GetType().ToString() == "CKEditor.NET.CKEditorControl")
						{
							GetCKF(c);
							((CKEditor.NET.CKEditorControl)c).Text = RowFields[c.ID.ToString()].ToString();
						}
						else if (c.GetType().ToString() == "System.Web.UI.WebControls.RadioButtonList")
						{
							((RadioButtonList)c).SelectedValue = RowFields[c.ID.ToString()].ToString();
						}
						else if (c.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
						{
							((CheckBox)c).Checked = Convert.ToBoolean(RowFields[c.ID.ToString()]);
						}
						else if (c.GetType().ToString() == "System.Web.UI.WebControls.Calendar")
						{
							((Calendar)c).SelectedDate = Convert.ToDateTime(RowFields[c.ID.ToString()].ToString());
						}
						else if (c.GetType().ToString() == "System.Web.UI.WebControls.FileUpload")
						{
							((FileUpload)c).ToolTip = RowFields[c.ID.ToString()].ToString();
						}
						else if (c.GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
						{
							((DropDownList)c).SelectedValue = RowFields[c.ID.ToString()].ToString();
						}
						else if (c.GetType().ToString() == "ASP.admin_controls_imagecontrol_ascx")
						{
							string[] icID = c.ID.ToString().ToLower().Split('_');
							((ASP.admin_controls_imagecontrol_ascx)c).ContentID = (int)RowFields[icID[0].ToString()];
						}
						else if (c.GetType().ToString() == "ASP.admin_controls_fileuploadcontrol_ascx")
						{
							ASP.admin_controls_fileuploadcontrol_ascx ctrl = (ASP.admin_controls_fileuploadcontrol_ascx)c;
							string[] icID = c.ID.ToString().ToLower().Split('_');

							ctrl.ContentID = (int)RowFields[icID[0].ToString()];

							if (ctrl.UseDbValueAsPrefix)
							{
								ctrl.Prefix = RowFields[ctrl.PrefixDbField].ToString();
							}

							//if (fvControls.CurrentMode == FormViewMode.Insert)
							//    ctrl.Visible = false;
						}
						else if (c.GetType().ToString() == "ASP.admin_controls_videocontrol_ascx")
						{
							ASP.admin_controls_videocontrol_ascx ctrl = (ASP.admin_controls_videocontrol_ascx)c;
							ctrl.ContentID = (int)RowFields[c.ID.ToString().ToLower().Split('_').First()];
							ctrl.VideoUrl = (string)RowFields[ctrl.Field];
							if (string.IsNullOrWhiteSpace(ctrl.Table))
								ctrl.Table = TableName;
							if (string.IsNullOrWhiteSpace(ctrl.IdField))
								ctrl.IdField = IdName;
						}
					}

					if (fvControls.FindControl("form2") != null)
					{
						fvControls.FindControl("form2").Controls.Add(ItemContainer);
					}
				}
			}
		}
		catch (Exception ex)
		{
			litError.Text = "<div class=\"alert red\"><strong>There was an error with your request, message returned:</strong> " + ex.Message + " <strong>Line Number: </strong> " + ex.LineNumber() + "</div>";
		}
	}

	protected void FormView_Update(object sender, System.EventArgs e)
	{
		if (Mode == CmsMode.Select)
		{ FormViewSource.UpdateParameters.Add(IdName, ContentID.ToString()); }
		else { FormViewSource.UpdateParameters.Add(IdName, gvPages.SelectedValue.ToString()); }

		int count = 0;
		try
		{
			foreach (Control c2 in fvControls.FindControl("form2").Controls)
			{
				foreach (Control c3 in c2.Controls)
				{
					if (c3.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
					{
						TextBox txt = ((TextBox)c3);
						if (txt.ID.ToLower().ToString() == CmsSettings.TitleField || txt.ID.ToLower().ToString() == TitleFieldOverride)
						{
							SlugValue = DoUrlString(txt.Text).ToLower();
							FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = txt.Text;
						}
						else if (txt.ID.ToLower().Contains("date"))
						{
							FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(txt.Text));
						}
						else if (txt.ID.ToLower().Contains("time"))
						{
							FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = String.Format("{0:HH:mm}", Convert.ToDateTime(txt.Text));
						}
						else { FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = txt.Text; }
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.Literal")
					{
						if (((Literal)c3).ID.ToLower() == CmsSettings.ParentField || ((Literal)c3).ID.ToLower() == ParentFieldOverride)
						{
							if (String.IsNullOrEmpty(((Literal)c3).Text))
							{
								FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = Request.QueryString[ParentQueryStringField];
							}

							else
							{ FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = ((Literal)c3).Text; }
						}
						else
						{ FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = ((Literal)c3).Text; }
					}
					else if (c3.GetType().ToString() == "CKEditor.NET.CKEditorControl")
					{
						FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = ((CKEditor.NET.CKEditorControl)c3).Text;
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.RadioButtonList")
					{
						FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = ((RadioButtonList)c3).SelectedValue;
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
					{
						FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = ((CheckBox)c3).Checked.ToString();
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.Calendar")
					{
						FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = String.Format("{0:yyyy/MM/dd}", ((Calendar)c3).SelectedDate);
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
					{
						FormViewSource.UpdateParameters[(c3.ID.ToString())].DefaultValue = ((DropDownList)c3).SelectedValue;
					}
					else if (c3.GetType().ToString() == "ASP.admin_controls_fileuploadcontrol_ascx")
					{
						ASP.admin_controls_fileuploadcontrol_ascx ctrl = (ASP.admin_controls_fileuploadcontrol_ascx)c3;

						if (!string.IsNullOrEmpty(ctrl.DbFilenameField))
						{
							if (FormViewSource.UpdateParameters.Contains(FormViewSource.InsertParameters[ctrl.DbFilenameField]))
								FormViewSource.UpdateParameters[ctrl.DbFilenameField].DefaultValue = ctrl.ReturnFileName();
						}
					}
					else if (c3.GetType().ToString() == "ASP.admin_controls_videocontrol_ascx")
					{
						ASP.admin_controls_videocontrol_ascx ctrl = ((ASP.admin_controls_videocontrol_ascx)c3);
						ctrl.Update();
					}


				}
				count++;
			}

			if (!string.IsNullOrEmpty(SlugValue))
			{
				if (FormViewSource.UpdateParameters.Contains(FormViewSource.UpdateParameters[CmsSettings.SlugField]))
					FormViewSource.UpdateParameters[CmsSettings.SlugField].DefaultValue = SlugValue.ToLower();

				else if (FormViewSource.UpdateParameters.Contains(FormViewSource.UpdateParameters[SlugFieldOverride]))
					FormViewSource.UpdateParameters[SlugFieldOverride].DefaultValue = SlugValue.ToLower();
			}

			//Response.Write(count);
			//Response.End();
			fvControls.UpdateItem(true);

			if (Mode != CmsMode.Select)
			{
				if (OnButtonClick != null) OnButtonClick(string.Empty);
				fvControls.Visible = false;
				gvPages.DataBind();
				gvPages.Visible = true;
				btnAdd.Visible = InsertEnabled;
			}
			else
			{
				Session["msg"] = "Content succesfully updated.";
				Session["type"] = "1";
				Response.Redirect(Request.RawUrl);
			}
		}
		catch (Exception ex)
		{
			litError.Text = "<div class=\"alert red\"><strong>There was an error with your request, message returned:</strong> " + ex.Message + " <strong>Line Number:</strong> " + ex.LineNumber() + "</div>";
		}
	}

	protected void Formview_Insert(object sender, System.EventArgs e)
	{
		try
		{
			//Page.ClientScript.
			foreach (Control c2 in fvControls.FindControl("form").Controls)
			{
				foreach (Control c3 in c2.Controls)
				{
					if (c3.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
					{
						TextBox txt = ((TextBox)c3);
						if (txt.ID.ToLower().ToString() == CmsSettings.TitleField || txt.ID.ToLower().ToString() == TitleFieldOverride)
						{
							SlugValue = DoUrlString(txt.Text).ToLower();
							FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = txt.Text;
						}
						else if (txt.ID.ToLower().Contains("date"))
						{
							FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(txt.Text));
						}
						else if (txt.ID.ToLower().Contains("time"))
						{
							FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = String.Format("{0:HH:mm}", Convert.ToDateTime(txt.Text));
						}
						else { FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = txt.Text; }

					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.Literal")
					{
						if (((Literal)c3).ID.ToLower() == CmsSettings.ParentField || ((Literal)c3).ID.ToLower() == ParentFieldOverride)
						{
							if (String.IsNullOrEmpty(((Literal)c3).Text))
							{
								FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = Request.QueryString[ParentQueryStringField];
							}
							else
							{
								FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = ((Literal)c3).Text;
							}
						}
						else
						{ FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = ((Literal)c3).Text; }
					}
					else if (c3.GetType().ToString() == "CKEditor.NET.CKEditorControl")
					{
						FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = ((CKEditor.NET.CKEditorControl)c3).Text;
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.RadioButtonList")
					{
						FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = ((RadioButtonList)c3).SelectedValue;
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
					{
						FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = ((CheckBox)c3).Checked.ToString();
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.Calendar")
					{
						FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = String.Format("{0:yyyy/MM/dd}", ((Calendar)c3).SelectedDate);
					}
					else if (c3.GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
					{
						FormViewSource.InsertParameters[(c3.ID.ToString())].DefaultValue = ((DropDownList)c3).SelectedValue;
					}
					else if (c3.GetType().ToString() == "ASP.controls_cms_fileupload01_ascx")
					{
						//if (NumberFiles > 0)
						//{
						//    int x = 0;
						//    while (x++ <= NumberFiles)
						//    {

						//    }
						//}
					}
					else if (c3.GetType().ToString() == "ASP.admin_controls_videocontrol_ascx")
					{
						ASP.admin_controls_videocontrol_ascx ctrl = ((ASP.admin_controls_videocontrol_ascx)c3);

						if (string.IsNullOrWhiteSpace(ctrl.Table))
							ctrl.Table = TableName;
						if (string.IsNullOrWhiteSpace(ctrl.IdField))
							ctrl.IdField = IdName;
					}

				}
			}

			if (!string.IsNullOrEmpty(SlugValue))
			{
				if (FormViewSource.InsertParameters.Contains(FormViewSource.InsertParameters[CmsSettings.SlugField]))
					FormViewSource.InsertParameters[CmsSettings.SlugField].DefaultValue = SlugValue.ToLower();

				else if (FormViewSource.InsertParameters.Contains(FormViewSource.InsertParameters[SlugFieldOverride]))
					FormViewSource.InsertParameters[SlugFieldOverride].DefaultValue = SlugValue.ToLower();
			}

			if (CheckDuplicates)
			{
				string checkSelectCmd = "SELECT {0} FROM {1} WHERE {2}";
				string filedsCheck = "";
				string cols = "";
				int arrCount = 0;

				using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand("CustomerCodes", conn))
					{
						cmd.CommandType = CommandType.Text;

						foreach (string s in DuplicateFieldsToCheck)
						{
							arrCount++;
							cmd.Parameters.AddWithValue(s, FormViewSource.InsertParameters[s].DefaultValue);
							if (arrCount == 1)
							{
								filedsCheck += s + " = @" + s;
								cols += s;
							}
							else
							{
								filedsCheck += " AND " + s + " = @" + s;
								cols += ", " + s;
							}
						}


						cmd.CommandText = string.Format(checkSelectCmd, IdName, TableName, filedsCheck);

						conn.Open();

						using (SqlDataReader dr = cmd.ExecuteReader())
						{
							if (dr.HasRows)
							{
								litError.Text = "<div class=\"alert yellow\"><strong>The item you are attempting to add already exists. Please make sure the item doesn't already exist.</div>";
							}
							else
							{
								FormViewSource.Insert();
							}
						}

						conn.Close();

					}
				}



			}
			else
			{
				FormViewSource.Insert();
			}
		}
		catch (Exception ex)
		{
			litError.Text = "<div class=\"alert red\"><strong>There was an error with your request, message returned:</strong> " + ex.Message + " <strong>Line Number: </strong> " + ex.LineNumber() + "</div>";
		}

	}

	protected void FormViewSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
	{
		if (e.Exception != null)
		{
			litError.Text = "<div class=\"alert red\"><strong>There was an error with your request, message returned:</strong> " + e.Exception.Message + "</div>";
		}
		else
		{
			int ReturnID = Convert.ToInt32(e.Command.Parameters["@ReturnID"].Value);
			MessageContainer ItemContainer = new MessageContainer();
			FormFields.InstantiateIn(ItemContainer);

			foreach (Control c in ItemContainer.Controls)
			{
				if (c.GetType().ToString() == "ASP.admin_controls_imagecontrol_ascx")
				{
					string ImgNo = "";

					ASP.admin_controls_imagecontrol_ascx ImageControl = (ASP.admin_controls_imagecontrol_ascx)c;

					ImgNo = ImageControl.ImageNo;
					ImageControl.ContentID = ReturnID;

					string _imgDimensions = ImageControl.Width + "x" + ImageControl.Height;

					if (!string.IsNullOrWhiteSpace(ImgNo) && ImgNo != "0")
					{
						if (File.Exists(Server.MapPath(string.Format("{0}/0_{1}_original.jpg", ImageControl.ImagePath, ImageControl.ImageNo))))
						{
							File.Copy(Server.MapPath(string.Format("{0}/0_{1}_original.jpg", ImageControl.ImagePath, ImageControl.ImageNo)), Server.MapPath(string.Format("{0}/{1}_{2}_original.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo)));
							File.Delete(Server.MapPath(string.Format("{0}/0_{1}_original.jpg", ImageControl.ImagePath, ImageControl.ImageNo)));
						}
						if (File.Exists(Server.MapPath(string.Format("{0}/0_{1}_unsized.jpg", ImageControl.ImagePath, ImageControl.ImageNo))))
						{
							File.Copy(Server.MapPath(string.Format("{0}/0_{1}_unsized.jpg", ImageControl.ImagePath, ImageControl.ImageNo)), Server.MapPath(string.Format("{0}/{1}_{2}_unsized.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo)));
							File.Delete(Server.MapPath(string.Format("{0}/0_{1}_unsized.jpg", ImageControl.ImagePath, ImageControl.ImageNo)));
						}
						if (File.Exists(Server.MapPath(string.Format("{0}/0_{1}_nowatermark.jpg", ImageControl.ImagePath, ImageControl.ImageNo))))
						{
							File.Copy(Server.MapPath(string.Format("{0}/0_{1}_nowatermark.jpg", ImageControl.ImagePath, ImageControl.ImageNo)), Server.MapPath(string.Format("{0}/{1}_{2}_nowatermark.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo)));
							File.Delete(Server.MapPath(string.Format("{0}/0_{1}_nowatermark.jpg", ImageControl.ImagePath, ImageControl.ImageNo)));
						}
						if (File.Exists(Server.MapPath(string.Format("{0}/0_{1}_{2}.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions))))
						{
							File.Copy(Server.MapPath(string.Format("{0}/0_{1}_{2}.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)), Server.MapPath(string.Format("{0}/{1}_{2}_{3}.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo, _imgDimensions)));
							File.Delete(Server.MapPath(string.Format("{0}/0_{1}_{2}.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)));
							ImageControl.CropThumbs();
						}
					}
					if (File.Exists(Server.MapPath(string.Format("{0}/0_original.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions))))
					{
						File.Copy(Server.MapPath(string.Format("{0}/0_original.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)), Server.MapPath(string.Format("{0}/{1}_original.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo, _imgDimensions)));
						File.Delete(Server.MapPath(string.Format("{0}/0_original.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)));
					}
					if (File.Exists(Server.MapPath(string.Format("{0}/0_unsized.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions))))
					{
						File.Copy(Server.MapPath(string.Format("{0}/0_unsized.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)), Server.MapPath(string.Format("{0}/{1}_unsized.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo, _imgDimensions)));
						File.Delete(Server.MapPath(string.Format("{0}/0_unsized.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)));
					}
					if (File.Exists(Server.MapPath(string.Format("{0}/0_nowatermark.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions))))
					{
						File.Copy(Server.MapPath(string.Format("{0}/0_nowatermark.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)), Server.MapPath(string.Format("{0}/{1}_nowatermark.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo, _imgDimensions)));
						File.Delete(Server.MapPath(string.Format("{0}/0_nowatermark.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)));
					}
					if (File.Exists(Server.MapPath(string.Format("{0}/0_{2}.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions))))
					{
						File.Copy(Server.MapPath(string.Format("{0}/0_{2}.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)), Server.MapPath(string.Format("{0}/{1}_{3}.jpg", ImageControl.ImagePath, ReturnID, ImageControl.ImageNo, _imgDimensions)));
						File.Delete(Server.MapPath(string.Format("{0}/0_{2}.jpg", ImageControl.ImagePath, ImageControl.ImageNo, _imgDimensions)));
						ImageControl.CropThumbs();
					}


				}
				else if (c.GetType().ToString() == "ASP.admin_controls_fileuploadcontrol_ascx")
				{
					ASP.admin_controls_fileuploadcontrol_ascx FileUploadControl = ((ASP.admin_controls_fileuploadcontrol_ascx)c);

					string TempFilePath = string.Format("{0}/{1}temp.pdf", FileUploadControl.FilePath, (!string.IsNullOrEmpty(FileUploadControl.Prefix) ? FileUploadControl.Prefix + "-" : ""));

					if (File.Exists(Server.MapPath(TempFilePath)))
					{
						string ext = "";

						int LastIndex = Convert.ToInt32(TempFilePath.LastIndexOf("/"));

						ext = Path.GetExtension((TempFilePath.Substring(LastIndex).Replace("/", "")));

						string NewFilePath = string.Format("{0}/{2}{1}.{3}", FileUploadControl.FilePath, ReturnID, (!string.IsNullOrEmpty(FileUploadControl.Prefix) ? FileUploadControl.Prefix + "-" : ""), ext.Replace(".", ""));


						File.Copy(Server.MapPath(TempFilePath), Server.MapPath(NewFilePath));

						File.Delete(Server.MapPath(TempFilePath));
					}
				}
				else if (c.GetType().ToString() == "ASP.admin_controls_videocontrol_ascx")
				{
					ASP.admin_controls_videocontrol_ascx ctrl = (ASP.admin_controls_videocontrol_ascx)c;
					if (ctrl != null)
					{
						ctrl.ContentID = ReturnID;

						if (string.IsNullOrWhiteSpace(ctrl.Table))
							ctrl.Table = TableName;
						if (string.IsNullOrWhiteSpace(ctrl.IdField))
							ctrl.IdField = IdName;
						
						ctrl.Update();
					}
				}

			}

			Session["msg"] = "Page succesfully added.";
			Session["type"] = "1";

			if (AddPageDestinationType == CmsAddPageDestinationType.Redirect)
			{
				if (String.IsNullOrEmpty(AddPageDestinationURL))
				{
					Response.Redirect("Page.aspx?id=" + ReturnID);
				}
				else
				{
					if (AddPageDestinationURL.Contains("?"))
						Response.Redirect(AddPageDestinationURL);
					else
						Response.Redirect(AddPageDestinationURL + ".aspx?id=" + ReturnID);
				}
			}
			else
			{
				fvControls.Visible = false;
				gvPages.DataBind();
				gvPages.Visible = true;
				btnAdd.Visible = InsertEnabled;
			}

			if (fvControls.FindControl("form2") != null) fvControls.FindControl("form2").Controls.Add(ItemContainer);
		}
	}
	#endregion


	#region Page Properties


	protected void Page_Load(object sender, EventArgs e)
	{
		if (HttpContext.Current.User.IsInRole("MasterAdmin"))
		{
			if (InsertEnabled && AddType == CmsAddType.Inline && string.IsNullOrEmpty(AddPageDestinationURL))
			{
				litError.Text = "<div class=\"alert blue\">The <strong>AddPageDestinationUrl</strong> property has not been set. When a new page page is added it will automatically redirect to Page.aspx with the querystring ID of the page added.</div>";
			}
		}
		GridViewSource.FilterExpression = Filter;

		gvPages.AllowPaging = AllowPaging;
		if (AllowPaging)
			gvPages.PageSize = PageSize;

		if (PageIndex != null || PageIndex != 0)
			gvPages.PageIndex = PageIndex;
		else
			gvPages.PageIndex = 1;

		if (!IsPostBack)
		{
			if (GridviewSelectParameters != null)
			{
				foreach (var p in GridviewSelectParameters)
				{
					switch (p.GetType().ToString())
					{
					case "System.Web.UI.WebControls.Parameter":
						{
							Parameter NewParam = (Parameter)p;
							if (!GridViewSource.SelectParameters.Contains(NewParam))
								GridViewSource.SelectParameters.Add(NewParam);

							break;
						}
					case "System.Web.UI.WebControls.QueryStringParameter":
						{
							QueryStringParameter NewParam = (QueryStringParameter)p;
							if (!GridViewSource.SelectParameters.Contains(NewParam))
								GridViewSource.SelectParameters.Add(NewParam);

							break;
						}
					case "System.Web.UI.WebControls.SessionParameter":
						{
							SessionParameter NewParam = (SessionParameter)p;
							if (!GridViewSource.SelectParameters.Contains(NewParam))
								GridViewSource.SelectParameters.Add(NewParam);

							break;
						}
					case "System.Web.UI.WebControls.ControlParameter":
						{
							ControlParameter NewParam = (ControlParameter)p;
							if (!GridViewSource.SelectParameters.Contains(NewParam))
								GridViewSource.SelectParameters.Add(NewParam);

							break;
						}
					default:
						{
							break;
						}
					}

				}
			}
		}

		if (!string.IsNullOrEmpty(SelectCommand))
		{ GridViewSource.SelectCommand = SelectCommand; }
		else
		{ GridViewSource.SelectCommand = String.Format(GridViewSource.SelectCommand, TableName); }

		GridViewSource.DeleteCommand = String.Format(GridViewSource.DeleteCommand, TableName, IdName, IdName);
		Parameter para;

		btnAdd.Visible = InsertEnabled;
		if (!string.IsNullOrEmpty(AddButtonText))
			btnAdd.Text = AddButtonText;

		//Response.Write(GridViewSource.SelectCommand);
		//Response.End();
		//GridViewSource.SelectCommand = "select * from tblContent";

		DataView dv = ((DataView)GridViewSource.Select(DataSourceSelectArguments.Empty));
		intCount = dv.Count;

		if (Mode == CmsMode.Select && dv.Count > 0)
		{
			FormViewSource.SelectParameters["id"].DefaultValue = dv[0][IdName].ToString();
		}
		else { FormViewSource.SelectParameters["id"].DefaultValue = ContentID.ToString(); }

		MessageContainer ItemContainer = new MessageContainer();
		FormFields.InstantiateIn(ItemContainer);

		if (ItemContainer.Controls.Count > 0)
		{
			foreach (Control c in ItemContainer.Controls)
			{
				if ((c.GetType().ToString() == "System.Web.UI.WebControls.TextBox" || c.GetType().ToString() == "System.Web.UI.WebControls.Literal" || c.GetType().ToString() == "Verto.ImageControl" ||
					c.GetType().ToString() == "FredCK.FCKeditorV2.FCKeditor" || c.GetType().ToString() == "CKEditor.NET.CKEditorControl" || c.GetType().ToString() == "System.Web.UI.WebControls.RadioButtonList" ||
					c.GetType().ToString() == "System.Web.UI.WebControls.CheckBox" || c.GetType().ToString() == "System.Web.UI.WebControls.DropDownList" || c.GetType().ToString() == "System.Web.UI.WebControls.FileUpload" ||
					c.GetType().ToString() == "System.Web.UI.WebControls.Calendar"))
				{

					updateString += c.ID.ToString() + " = @" + c.ID + ", ";
					insertColumnsString += c.ID.ToString() + ", ";
					insertValuesString += "@" + c.ID.ToString() + ", ";
					para = new Parameter();
					para.ConvertEmptyStringToNull = false;
					para.Name = c.ID.ToString();
					FormViewSource.UpdateParameters.Add(para);
					FormViewSource.InsertParameters.Add(para);
					count++;
				}
			}
		}
		//add parameters from custom collection
		if (InsertParameters != null)
		{
			foreach (var p in InsertParameters)
			{
				switch (p.GetType().ToString())
				{
				case "System.Web.UI.WebControls.Parameter":
					{
						Parameter newParam = (Parameter)p;
						if (!FormViewSource.InsertParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.InsertParameters.Add(newParam);
							insertColumnsString += newParam.Name + ", ";
							insertValuesString += "@" + newParam.Name + ", ";
						}
						break;
					}
				case "System.Web.UI.WebControls.QueryStringParameter":
					{
						QueryStringParameter newParam = (QueryStringParameter)p;
						if (!FormViewSource.InsertParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.InsertParameters.Add(newParam);
							insertColumnsString += newParam.Name + ", ";
							insertValuesString += "@" + newParam.Name + ", ";
						}
						break;
					}
				case "System.Web.UI.WebControls.SessionParameter":
					{
						SessionParameter newParam = (SessionParameter)p;
						if (!FormViewSource.InsertParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.InsertParameters.Add(newParam);
							insertColumnsString += newParam.Name + ", ";
							insertValuesString += "@" + newParam.Name + ", ";
						}
						break;
					}
				case "System.Web.UI.WebControls.ControlParameter":
					{
						ControlParameter newParam = (ControlParameter)p;
						if (!FormViewSource.InsertParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.InsertParameters.Add(newParam);
							insertColumnsString += newParam.Name + ", ";
							insertValuesString += "@" + newParam.Name + ", ";
						}
						break;
					}
				default:
					{
						break;
					}
				}

			}

		}

		if (UpdateParameters != null)
		{

			foreach (var p in UpdateParameters)
			{
				switch (p.GetType().ToString())
				{
				case "System.Web.UI.WebControls.Parameter":
					{
						Parameter newParam = (Parameter)p;
						if (!FormViewSource.UpdateParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.UpdateParameters.Add(newParam);
							updateString += newParam.Name + " = @" + newParam.Name + ", ";
						}
						break;
					}
				case "System.Web.UI.WebControls.QueryStringParameter":
					{
						QueryStringParameter newParam = (QueryStringParameter)p;
						if (!FormViewSource.UpdateParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.UpdateParameters.Add(newParam);
							updateString += newParam.Name + " = @" + newParam.Name + ", ";
						}
						break;
					}
				case "System.Web.UI.WebControls.SessionParameter":
					{
						SessionParameter newParam = (SessionParameter)p;
						if (!FormViewSource.UpdateParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.UpdateParameters.Add(newParam);
							updateString += newParam.Name + " = @" + newParam.Name + ", ";
						}
						break;
					}
				case "System.Web.UI.WebControls.ControlParameter":
					{
						ControlParameter newParam = (ControlParameter)p;
						if (!FormViewSource.UpdateParameters.Contains(newParam))
						{
							newParam.ConvertEmptyStringToNull = false;
							FormViewSource.UpdateParameters.Add(newParam);
							updateString += newParam.Name + " = @" + newParam.Name + ", ";
						}
						break;
					}
				default:
					{
						break;
					}
				}

			}

		}

		//end custom collection parameters

		if (AddedValue.Contains(";"))
		{
			string[] g = AddedValue.Split(';');
			string[] a;
			foreach (string s in g)
			{
				if (s.Contains(","))
				{
					a = s.Split(',');
					insertColumnsString = insertColumnsString + a[0] + ", ";
					insertValuesString = insertValuesString + a[1] + ", ";
				}
			}
		}

		if (count > 0)
		{
			insertColumnsString = insertColumnsString.Substring(0, insertColumnsString.Length - 2);
			insertValuesString = insertValuesString.Substring(0, insertValuesString.Length - 2);

			insertColumnsString = insertColumnsString + ")";
			insertValuesString = insertValuesString + ")";
			FormViewSource.UpdateCommand = String.Format(FormViewSource.UpdateCommand, TableName, updateString.Substring(0, updateString.Length - 2), IdName);
			FormViewSource.InsertCommand = String.Format(FormViewSource.InsertCommand, TableName, insertColumnsString, insertValuesString, IdName);
		}



		ViewState["count"] = count.ToString();

		FormViewSource.SelectCommand = String.Format(FormViewSource.SelectCommand, TableName, IdName);


		if (!IsPostBack)
		{
			int colCount = 0;

			foreach (Column col in _columns)
			{
				colCount++;
				BoundField b = new BoundField();
				b.DataField = col.DbField;
				b.DataFormatString = col.FormatString;
				b.HeaderText = col.LabelText;
				gvPages.Columns.Add(b);
				//gvPages.Columns.Insert(0, b);

				//}
				//gvPages.Columns.RemoveAt(0);
				//gvPages.Columns.Insert(1, _TemplateField);
			}
			//TemplateField tempField = new TemplateField();
			foreach (CustomButton btn in _buttons)
			{
				ButtonField bf = new ButtonField() { CommandName = btn.CommandName, ButtonType = btn.ButtonType, ImageUrl = btn.ImageUrl };

				bf.ItemStyle.CssClass = btn.CssClass;
				//bf.ClientClickEvent = btn.ClientClickEvent;
				gvPages.Columns.Add(bf);

			}

		}

		string[] id = { IdName };
		if (DataKeyNames != null && DataKeyNames.Length > 0)
		{
			int array1OriginalLength = id.Length;
			Array.Resize<string>(ref id, array1OriginalLength + DataKeyNames.Length);
			Array.Copy(DataKeyNames, 0, id, array1OriginalLength, DataKeyNames.Length);
		}
		gvPages.DataKeyNames = id;
		InsertTemplate(true);
		gvPages.EmptyDataText = EmptyMessage;



		if (Mode == CmsMode.Select)
		{
			fvControls.ChangeMode(FormViewMode.Edit);
			FormViewSource.SelectParameters["id"].DefaultValue = ContentID.ToString();
			fvControls.DataBind();
			InsertTemplate(true);
			fvControls.Visible = true;
			gvPages.Visible = false;
			btnAdd.Visible = false;
			CurrentValue = ContentID;

			Button btnCancel = ((Button)fvControls.FindControl("Cancel"));
			if (btnCancel != null)
				btnCancel.Visible = false;
			else
				litError.Text = "<div class=\"alert red\">The ContentID Property is missing or the record doesn't exist in the database.</div>";
		}
		else if (Mode == CmsMode.Insert)
		{
			fvControls.ChangeMode(FormViewMode.Insert);
			fvControls.DataBind();
			fvControls.Visible = true;
			gvPages.Visible = false;
			btnAdd.Visible = false;

			Button btnCancel = ((Button)fvControls.FindControl("Cancel"));
			btnCancel.Visible = false;
		}


	}

	protected void btnAdd_Click(object sender, EventArgs e)
	{
		if (AddType == CmsAddType.Separate)
		{
			if (Request.QueryString.AllKeys.Contains("id"))
				Response.Redirect("/admin/addpage.aspx?id=" + Request.QueryString["id"]);
			else
				Response.Redirect("/admin/addpage.aspx");
		}
		else if (AddType == CmsAddType.Inline)
		{
			fvControls.ChangeMode(FormViewMode.Insert);
			fvControls.DataBind();
			fvControls.Visible = true;
			gvPages.Visible = false;
			btnAdd.Visible = false;
		}
	}

	protected void Page_PreRender(object sender, EventArgs e)
	{
		if (gvPages.Visible == true) gvPages.DataBind();

		// Drag + drop ordering
		gvPages.Attributes["data-sort-column"] = SortColumn ?? "pageOrder";
		gvPages.Attributes["data-id-column"] = IdFieldOverride ?? IdName ?? CmsSettings.IDField;
		gvPages.Attributes["data-table"] = TableName ?? "tblContent";
		gvPages.Attributes["data-current-page"] = gvPages.PageIndex.ToString();
		gvPages.Attributes["data-page-size"] = PageSize.ToString();
		if (EnableDragOrdering)
			gvPages.CssClass += " sortable";
	}

	#endregion

}