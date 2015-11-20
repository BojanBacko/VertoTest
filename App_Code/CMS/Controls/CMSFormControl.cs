using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Interfaces;

namespace CMS.Controls {

    public class CMSFormControl : CompositeControl {
        private ControlContainer _formContainer;
        private ControlContainer _buttonContainer;

    #region Properties

        public bool IsControlPostBack;

        public bool DuplicateButtonsAtTop { get; set; }

        // form data
        public DataRow Data { get; private set; }

        // form container
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            PersistenceMode(PersistenceMode.InnerProperty),
            TemplateContainer(typeof(ControlContainer)),
            TemplateInstance(TemplateInstance.Single)]
        public ITemplate Form { get; set; }
        public ControlContainer FormContainer { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            PersistenceMode(PersistenceMode.InnerProperty),
            TemplateContainer(typeof(ControlContainer)),
            TemplateInstance(TemplateInstance.Single)]
        public ITemplate Buttons { get; set; }
        public ControlContainer ButtonContainer { get; set; }

        // events
        public delegate DataRow FormLoadingEventHandler();
        public event FormLoadingEventHandler FormLoad;

    #endregion

    #region Page Life Cycle

        // init
        protected override void OnInit(EventArgs e) {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        // create controls
        protected override void CreateChildControls() {

            // create form container from ITemplate or dynamic control
            _formContainer = new ControlContainer();
            if (Form != null) Form.InstantiateIn(_formContainer);
            else if (FormContainer != null) _formContainer = FormContainer;

            if (_formContainer != null) Controls.Add(_formContainer);

            _buttonContainer = new ControlContainer();
            if (Buttons != null) Buttons.InstantiateIn(_buttonContainer);
            else if (ButtonContainer != null) _buttonContainer = ButtonContainer;
            
            if (_buttonContainer != null) Controls.Add(_buttonContainer);

        }

        // load state
        protected override void LoadControlState(object savedState) {
            IsControlPostBack = true;
            base.LoadControlState(savedState);
        }

        protected override object SaveControlState() {
            return 1;
        }

        // load event
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            // get data from event
            if (FormLoad != null)
                Data = FormLoad();

            EnsureChildControls();

            if (Data == null || IsControlPostBack) {
                return;
            }

            foreach (Control c in _formContainer.Controls) {

                IBindingControl ctrl;

                if ((ctrl = c as IBindingControl) == null) continue;

                var col = Functions.ParseColumnIdentifier(c.ID);
                if (col == null) continue;

                ctrl.BindValueToControl(col, Data);

            }

        }

        // render as html
        protected override void Render(HtmlTextWriter writer) {
            if (_formContainer == null)
                return;

            writer.WriteLine("<div class=\"form module_content\">");

            if (DuplicateButtonsAtTop && _buttonContainer != null) {
                writer.WriteLine("<div style=\"margin-top:10px\">");
                _buttonContainer.RenderControl(writer);
                writer.WriteLine("</div>");
            }

            // render form
            _formContainer.RenderControl(writer);

            if (_buttonContainer != null) {
                _buttonContainer.RenderControl(writer);
            }

            writer.WriteLine("</div>");

        }

    #endregion

    #region Interface

        public DataRow GetFormValues() {

            var data = new Dictionary<string, object>();

            // collect data from form
            if (_formContainer == null)
                return Data;

            foreach (Control c in _formContainer.Controls) {

                IBindingControl ctrl;
                if ((ctrl = c as IBindingControl) == null)
                    continue;

                var col = Functions.ParseColumnIdentifier(c.ID);
                if (col == null) continue;

                var parameters = ctrl.GetControlValues(col);
                if (parameters != null) {

                    foreach (var p in parameters) data.Add(p.Key, p.Value);

                }

            }

            // build the table and row
            var dt = new DataTable();
            foreach (var kvp in data) dt.Columns.Add(kvp.Key, kvp.Value.GetType());

            var r = dt.NewRow();
            foreach (var kvp in data) r[kvp.Key] = kvp.Value;

            return r;

        }
        
        public int Save(IDataPersister persister) {

            var data = new Dictionary<string, object>();

            // collect data from form
            if (_formContainer == null)
                return 0;

            foreach (Control c in _formContainer.Controls) {

                IBindingControl ctrl;
                if ((ctrl = c as IBindingControl) == null)
                    continue;

                var col = Functions.ParseColumnIdentifier(c.ID);
                if (col == null) continue;

                var parameters = ctrl.GetControlValues(col);
                if (parameters != null) {

                    foreach (var p in parameters) data.Add(p.Key, p.Value);

                }

            }

            // build the table and row
            var dt = new DataTable();
            foreach (var kvp in data) dt.Columns.Add(kvp.Key, kvp.Value.GetType());

            var r = dt.NewRow();
            foreach (var kvp in data) r[kvp.Key] = kvp.Value;

            return persister.Persist(r);

        }

        #endregion

    }

}