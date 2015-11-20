using System.Data;

namespace CMS.Interfaces {

    public interface IDataCollectionProvider {

        DataTable GetCollection();

    }

}