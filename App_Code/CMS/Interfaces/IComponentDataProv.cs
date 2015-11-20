using System.Data;

namespace CMS {

    /// <summary>
    /// Summary description for IComponentDataProv
    /// </summary>
    public interface IComponentDataProv {

        DataRow Load(int componentID);

    }

}