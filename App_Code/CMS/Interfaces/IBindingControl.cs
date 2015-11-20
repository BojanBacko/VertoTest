using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public interface IBindingControl {

    void BindValueToControl(string col, DataRow data, string defaultValue = "");

    Dictionary<string, object> GetControlValues(string col);

}