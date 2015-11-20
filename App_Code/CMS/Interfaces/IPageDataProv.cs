using System.Data;

namespace CMS {

    /// <summary>
    /// Page Data Provider, implements methods to get the data of a page
    /// </summary>
    public interface IPageDataProv {

        DataRow Load(int pageID);

    }

}