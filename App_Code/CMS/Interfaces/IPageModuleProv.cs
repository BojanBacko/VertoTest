using System.Collections.Generic;
using System.Data;
using CMS.DataStructures;

namespace CMS {

    public interface IPageModuleProv {

        IEnumerable<ModuleProfile> GetModuleProfiles(DataRow args);

    }

}