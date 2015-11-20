using System.Data;

namespace CMS {

    /// <summary>
    /// Page Data Persister, implements methods to save the data and create a new instance of a page
    /// </summary>
    public interface IPageDataPers {

        bool Save(int pageID, DataRow args);
        int Create(DataRow args);

    }

}