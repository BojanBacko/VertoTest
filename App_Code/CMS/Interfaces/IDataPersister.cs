using System.Data;

namespace CMS.Interfaces {

    public interface IDataPersister {

        int Persist(DataRow parameters);

    }

}