using System.Data;

namespace CMS {

    /// <summary>
    /// Summary description for IFilteredComponentDataCollectionProv
    /// </summary>
    public interface IFilteredComponentDataCollectionProv {
        
        DataTable LoadCollection();

        DataTable LoadCollection(int filterID, string filterField);

        DataTable LoadCollection(string filterKey, string filterField);

    }

}