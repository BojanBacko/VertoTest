using System.Collections.Generic;

namespace CMS {

    public interface ICrumbProv {

        IEnumerable<DataStructures.Crumb> GetCrumbs(int pageID);

    }

}