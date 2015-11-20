using System.Data;

namespace CMS {

    /// <summary>
    /// Summary description for IComponentDataCollectionProv
    /// </summary>
    public interface IComponentDataCollectionProv {

        DataTable LoadCollection();

        DataTable LoadCollection(int contentID);

    }

}