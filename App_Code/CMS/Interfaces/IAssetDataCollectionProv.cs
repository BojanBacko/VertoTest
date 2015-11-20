using System.Data;

namespace CMS {

    /// <summary>
    /// Summary description for IAssetDataCollectionProv
    /// </summary>
    public interface IAssetDataCollectionProv {

        DataTable LoadCollection();

        DataTable LoadCollection(string directory);

    }

}