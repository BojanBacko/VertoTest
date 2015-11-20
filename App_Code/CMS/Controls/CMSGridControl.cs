using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Controls;
using CMS.Controls.Columns;
using CMS.Interfaces;

namespace CMS.Controls {

    [ToolboxData("<{0}:CMSGridControl runat=\"server\" />")]
    public class CMSGridControl : CompositeControl {
        private DataTable _data;
        private Dictionary<int, List<Control>> _controls;
        private HtmlGenericControl _btnBack;
        private CMSModule _form;
        private string _hierarchicalSortField = "sort";
        
        public string ItemModuleFileName { get; set; }
        public int SelectedID { get; private set; }

        public Dictionary<string, object> ModuleBag;

        // hierarchical sort
        public bool HierarchicalSort { get; set; }
        public string HierarchicalParentIDField { get; set; }
        public string HierarchicalUniqueIDField { get; set; }
        public string HierarchicalSortField { get { return _hierarchicalSortField; } set { _hierarchicalSortField = value; } }

        // columns
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Collection<Column> Columns { get; set; }

        // events
        public delegate DataTable GridLoadingEventHandler();
        public event GridLoadingEventHandler GridLoad;

        public delegate void CMSGridEventHandler(string cname, int index, DataRow data);
        public event CMSGridEventHandler ColumnCommand;

        public delegate string StyleEventHandler(int rowIndex, DataRow data);
        public event StyleEventHandler StylingRow;

        // init
        protected override void OnInit(EventArgs e) {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);

            _controls = new Dictionary<int, List<Control>>();
            
        }
        
        // load state
        protected override void LoadControlState(object savedState) {
            if (savedState == null) return;
            var args = (object[])savedState;
            SelectedID = (int) args[0];
            ModuleBag = (Dictionary<string, object>)args[1];
        }

        // load event
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            BindControls();
            
            if (HttpContext.Current.Request.Form["__EVENTTARGET"] != null && HttpContext.Current.Request.Form["__EVENTTARGET"] == _btnBack.ClientID) {
                btnBack_Click();
            }

        }

        private void BindControls() {
            
            if (GridLoad == null)
                return;

            Controls.Clear();

            _btnBack = new HtmlGenericControl("button") {
                ID = "btnBack",
                InnerHtml = "<i class=\"fa fa-caret-up\"></i>&nbsp;&nbsp;&nbsp;&nbsp;Back to List"
            };
            Controls.Add(_btnBack);
            _btnBack.Attributes.Add("style", "margin: 5px 0 10px;");
            _btnBack.Attributes.Add("onclick", string.Format("javascript:__doPostBack('{0}','')", _btnBack.ClientID));

            if (!string.IsNullOrEmpty(ItemModuleFileName) && SelectedID > 0) {

                _form = (CMSModule) ((Page)HttpContext.Current.Handler).LoadControl(string.Format("/admin/controls/modules/{0}.ascx", ItemModuleFileName));
                if (_form != null) {
                    _form.SelectedID = SelectedID;
                    _form.ModuleBag = ModuleBag;
                    Controls.Add(_form);

                }

            }

            _data = GridLoad();

            if (HierarchicalSort) {

                if (HierarchicalUniqueIDField == null || HierarchicalParentIDField == null || HierarchicalSortField == null) {
                    Functions.LogError(new NullReferenceException("Hierarchical sort requires attributes 'HierarchicalUniqueIDField', 'HierarchicalParentIDField' and 'HierarchicalSortField'"));
                    return;
                }

                if (!_data.Columns.Contains(HierarchicalUniqueIDField) || !_data.Columns.Contains(HierarchicalParentIDField) || !_data.Columns.Contains(HierarchicalSortField)) {
                    throw new Exception(string.Format("One or more of the hierarchical sort columns in {0} do not exist in the DataTable.", UniqueID));
                }

                if (_data.Rows.Count > 0) {

                    _data = _data.AsEnumerable()
                        .OrderBy(r => r[_hierarchicalSortField])
                        .CopyToDataTable();
                    
                }

            }

            var rowCount = 0;
            if (_data != null)
                rowCount = _data.Rows.Count;
            var colCount = Columns.Count;

            _controls = new Dictionary<int, List<Control>>();
            
            for (var i = 0; i < colCount; i++) {
                var c = Columns[i];
                IControlEventColumn ctrl;
                if ((ctrl = c as IControlEventColumn) == null)
                    continue;

                _controls.Add(i, new List<Control>());

                for (var j = 0; j < rowCount; j++) {

                    var colControl = ctrl.GetControl(j);

                    if (colControl.GetType() == typeof(Button)) {
                        ((Button)colControl).Click += ColumnCommandEvent;
                    }

                    _controls[i].Add(colControl);
                    Controls.Add(colControl);

                }

            }

        }

        // events
        private void ColumnCommandEvent(object sender, EventArgs eventArgs) {
            
            IButtonControl ctrl;
            if ((ctrl = sender as IButtonControl) == null)
                return;

            var dataIndex = int.Parse(ctrl.CommandArgument);

            if (ColumnCommand != null)
                ColumnCommand(ctrl.CommandName, dataIndex, _data.Rows[dataIndex]);

        }

        public void ChangeSelectedID(int id, Dictionary<string, object> moduleBag = null) {

            SelectedID = id;
            ModuleBag = moduleBag;
            BindControls();

        }

        public void RefreshGrid() {
            ChangeSelectedID(0);
        }

        protected void btnBack_Click() {
            RefreshGrid();
        }

        // save state
        protected override object SaveControlState() {
            return new object[] {
                SelectedID,
                ModuleBag
            };
        }
        
        // render
        protected override void Render(HtmlTextWriter w) {

            w.WriteLine("<div class=\"cms-grid\">");

            if ( SelectedID > 0 ) {

                w.WriteLine("<div class=\"form module_content\">");

                _btnBack.RenderControl(w);
                
                if ( _form != null )
                    _form.RenderControl(w);

                w.WriteLine("</div>");

            }
            else if ( _data != null && _controls != null ) {

                w.WriteLine("<table class=\"grid gv\" cellspacing=\"0\">");
                
                w.WriteLine("<thead><tr>");
                foreach (var c in Columns) {
                    w.WriteLine("<th>{0}</th>", c.HeaderText);
                }
                w.WriteLine("</tr></thead>");

                w.WriteLine("<tbody>");
                for ( var i = 0; i < _data.Rows.Count; i++ ) {

                    var hUID = ""; // hierarchical unique ID
                    var hPID = ""; // hierarchical parent ID
                    var subsort = ""; // hierarchical parent ID

                    if (HierarchicalSort) {
                        hUID = string.Format("data-huid=\"{0}\"", _data.Rows[i][HierarchicalUniqueIDField]);
                        hPID = string.Format("data-hpid=\"{0}\"", _data.Rows[i][HierarchicalParentIDField]);
                        subsort = string.Format("data-subsort=\"{0}\"", _data.Rows[i][HierarchicalSortField]);
                    }

                    if (StylingRow != null) w.WriteLine("<tr class=\"{0}\" {1} {2} {3}>", StylingRow(i, _data.Rows[i]), hUID, hPID, subsort);
                    else w.WriteLine("<tr {0} {1} {2}>", hUID, hPID, subsort);

                    for ( var j = 0; j < Columns.Count; j++ ) {
                        var c = Columns[j];

                        w.WriteLine("<td{0}>", c.Width > 0 ? " style=\"width: " + c.Width + "px;\"" : "");

                        if ( c is IControlEventColumn ) {

                            _controls[j][i].RenderControl(w);

                        }
                        else
                            c.GetColumnInner(w, _data.Rows[i]);

                        w.WriteLine("</td>");

                    }
                    w.WriteLine("</tr>");

                }
                w.WriteLine("</tbody>");

                w.WriteLine("</table>");

            }

            w.WriteLine("</div>");

        }

    }

}