using System.Collections.Generic;
using CMS.DataStructures;

namespace CMS.Interfaces {

    public interface IAncestorProvider {

        IEnumerable<Crumb> GetAncestors();

    }

}