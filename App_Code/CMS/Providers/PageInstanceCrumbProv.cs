using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CMS.Providers {

    public class PageInstanceCrumbProv : ICrumbProv {
        private readonly SqlConnection _conn;

        public PageInstanceCrumbProv(string connectionStringIndex = "msSQL") {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringIndex].ConnectionString);
        }

        public IEnumerable<DataStructures.Crumb> GetCrumbs(int pageID) {

            using (var cmd = new SqlCommand("with pages as (select pageID, parentID, slug, title, cast(0 as int) row from cms_Pages a where a.pageID = @pageID union all select r.pageID, r.parentID, r.slug, r.title, cast(row + 1 as int) row from cms_Pages r inner join pages p on p.parentID = r.pageID ) select pageID, slug, title from pages order by row desc;", _conn)) {
                cmd.Parameters.AddWithValue("pageID", pageID);

                _conn.Open();
                using (var r = cmd.ExecuteReader()) {

                    while (r.Read()) {

                        yield return new DataStructures.Crumb {
                            UniqueID = (int) r["pageID"],
                            Text = (string) r["title"]
                        };

                    }

                }
                _conn.Close();

            }

        }

    }

}