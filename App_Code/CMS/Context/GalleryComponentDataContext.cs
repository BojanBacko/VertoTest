using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CMS.Context {

    /// <summary>
    /// Summary description for GalleryComponentDataContext
    /// </summary>
    public class GalleryComponentDataContext : IComponentDataProv, IComponentDataCollectionProv {
        private readonly SqlConnection _conn;

        public GalleryComponentDataContext(string connectionStringIndex = "msSQL") {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringIndex].ConnectionString);
        }

        public DataRow Load(int galleryID) {

            DataRow output = null;

            using (var cmd = new SqlCommand("select * from tbl_Gallery a inner join cms_Assets b on b.assetID = a.imageID where a.galleryID = @galleryID", _conn)) {
                cmd.Parameters.AddWithValue("galleryID", galleryID);

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

            using (var cmd = new SqlCommand("select * from tbl_Gallery a inner join cms_Assets b on b.assetID = a.imageID order by position asc", _conn)) {

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

        public DataTable LoadCollection(int contentID) {

            DataTable output = null;

            using (var cmd = new SqlCommand("select * from tbl_Gallery a inner join cms_Assets b on b.assetID = a.imageID where a.contentID = @contentID order by position asc", _conn)) {
                cmd.Parameters.AddWithValue("contentID", contentID);

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