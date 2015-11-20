using System.Data;

namespace CMS {

    /// <summary>
    /// Summary description for IAssetDataProv
    /// </summary>
    public interface IAssetDataProv {

        DataRow Load(int assetID);

    }

}