using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CMS.DataStructures;

namespace CMS.Providers {
    
    public class WebsiteSettingsModuleProv : IPageModuleProv {
        private readonly SqlConnection _conn;

        public WebsiteSettingsModuleProv(string connectionStringIndex = "msSQL") {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringIndex].ConnectionString);
        }

        public IEnumerable<ModuleProfile> GetModuleProfiles(DataRow args) {

            if (args.Table.Columns.Contains("templateKey")) {

                var template = WebsiteSettings.Templates[(string) args["templateKey"]];

                if (template != null && template.Modules != null) {

                    foreach (var module in template.Modules) {

                        yield return new ModuleProfile {
                            UniqueID = string.Format("module_{0}", module.Key),
                            TabTitle = module.TabName,
                            FileName = string.Format("{0}.ascx", module.Key),
                            FilePath = "/admin/controls/modules/",
                        };

                    }


                }
                
            }

        }

    }

}