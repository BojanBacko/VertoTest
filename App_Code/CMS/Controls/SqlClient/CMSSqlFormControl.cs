using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using CMS.Context;

namespace CMS.Controls.SqlClient {
    
    public class CMSSqlFormControl : CMSFormControl {

        public int PageID { get; set; }
       
        public string CommandText { get; set; }
        
        public string TableName { get; set; }        

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            // config

        }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            
            FormLoad += OnFormLoad;

            // Column.Add(new PreviewColumn)

        }

        private DataRow OnFormLoad() {

            var ctx = new PageInstanceDataContext();
            return ctx.Load(PageID);

        }

    }

}