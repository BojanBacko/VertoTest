using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CMS.Context {
    
    public class AssetDataContext : IAssetDataProv, IAssetDataCollectionProv {
        private readonly SqlConnection _conn;

        public AssetDataContext(string connectionStringIndex = "msSQL") {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringIndex].ConnectionString);
        }

        public DataRow Load(int assetID) {

            DataRow output = null;

            using (var cmd = new SqlCommand("select * from cms_Assets where assetID = @assetID", _conn)) {
                cmd.Parameters.AddWithValue("assetID", assetID);

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

        public DataTable LoadCollection() {

            DataTable output = null;

            using (var cmd = new SqlCommand("select * from cms_Assets", _conn)) {

                _conn.Open();
                using (var r = cmd.ExecuteReader()) {

                    if (r.HasRows) {

                        output = new DataTable();
                        output.Load(r);
                        
                    }

                }
                _conn.Close();

            }

            return output;

        }

        public DataTable LoadCollection(string directory) {

            DataTable output = null;

            using (var cmd = new SqlCommand("select * from cms_Assets where directory = @directory", _conn)) {
                cmd.Parameters.AddWithValue("directory", directory);

                _conn.Open();
                using (var r = cmd.ExecuteReader()) {

                    if (r.HasRows) {

                        output = new DataTable();
                        output.Load(r);

                    }

                }
                _conn.Close();

            }

            return output;

        }

    }

}