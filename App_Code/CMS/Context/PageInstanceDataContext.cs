using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CMS.Context {
    
    public class PageInstanceDataContext : IPageDataProv, IPageDataPers, IPageDataEraser {
        private readonly SqlConnection _conn;

        public PageInstanceDataContext(string connectionStringIndex = "msSQL") {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringIndex].ConnectionString);
        }

        public DataRow Load(int pageID) {

            DataRow output = null;

            using (var cmd = new SqlCommand("select a.pageID, a.parentID, a.contentID, a.imageID pageImageID, b.* from cms_Pages a inner join cms_Content b on b.contentID = a.contentID where a.pageID = @pageID", _conn)) {
                cmd.Parameters.AddWithValue("pageID", pageID);

                _conn.Open();
                using (var r = cmd.ExecuteReader()) {

                    if (r.HasRows) {

                        var dt = new DataTable();
                        dt.Load(r);

                        if (dt.Rows.Count > 0)
                            output = dt.Rows[0];

                    }

                }
                _conn.Close();

            }

            return output;

        }

        public bool Save(int pageID, DataRow args) {

            bool saved;
            var dataColumns = args.Table.Columns.Cast<DataColumn>();
            var enumerable = dataColumns as DataColumn[] ?? dataColumns.ToArray();

            using (var cmd = new SqlCommand(
                string.Format("update cms_Content set {0} where contentID = (select contentID from cms_Pages where pageID = @pageID);", enumerable
                    .Select(c => c.ColumnName + " = @" + c.ColumnName)
                    .Aggregate((a, b) => a + ", " + b)),
                _conn)) {

                cmd.Parameters.AddWithValue("pageID", pageID);
                cmd.Parameters.AddRange(enumerable
                    .Select(c => new SqlParameter(c.ColumnName, args[c.ColumnName]))
                    .ToArray()
                );

                _conn.Open();
                saved = cmd.ExecuteNonQuery() > 0;
                _conn.Close();

            }

            return saved;

        }

        public int Create(DataRow args) {
            throw new System.NotImplementedException();
        }

        public bool Remove(int pageID) {
            throw new System.NotImplementedException();
        }

    }

}