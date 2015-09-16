using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Specialized;

/// <summary>
/// Summary description for Columns
/// </summary>
namespace Columns
{
	
    public class Column : ITemplate
    {
        public string FormatString { get; set; }
        public string DbField { get; set; }
        public string LabelText { get; set; }
        
        public void InstantiateIn(Control container) { }

    }

}

namespace Buttons
{

    public enum ButtonType { ImageButton, Button }

    public class CustomButton : ITemplate
    {
        public string ImageUrl { get; set; }
        public string CommandArgument { get; set; }
        /// <summary>
        /// If you want to edit inline set the commmand name to "Select" otherwise set it to either "PageLayout" or your custom command name.
        /// </summary>
        public string CommandName { get; set; }
        public string Text { get; set; }
        public string ToolTip { get; set; }
        public string CssClass { get; set; }
        public string ClientClickEvent { get; set; }
        public System.Web.UI.WebControls.ButtonType ButtonType { get; set; }

        [TypeConverterAttribute(typeof(StringArrayConverter))]
        public void InstantiateIn(Control container) { }

    }

}

public static class ExceptionHelper
{
    public static int LineNumber(this Exception e)
    {

        int linenum = 0;
        try
        {
            linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));
        }
        catch
        {
            //Stack trace is not available!
        }
        return linenum;
    }
}


#region CmsConfiguration
public class CmsConfiguration : ConfigurationSection
{
    public static CmsConfiguration GetConfig()
    {
        return (CmsConfiguration)ConfigurationManager.GetSection("CmsConfiguration");
    }


    [ConfigurationProperty("", IsDefaultCollection = true)]
    public CmsConfigurationCollection Settings
    {
        get
        {
            return (CmsConfigurationCollection)base[""];
        }
    }
}

[ConfigurationCollection(typeof(CmsConfigurationElement))]
public class CmsConfigurationCollection : ConfigurationElementCollection
{
    protected override ConfigurationElement CreateNewElement()
    {
        return new CmsConfigurationElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
        return ((CmsConfigurationElement)(element)).Key;
    }

    public CmsConfigurationElement this[int idx]
    {
        get
        {
            return (CmsConfigurationElement)BaseGet(idx);
        }
    }
}

public class CmsConfigurationElement : ConfigurationElement
{
    [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
    public string Key
    {
        get
        {
            return ((string)(base["key"]));
        }
        set
        {
            base["key"] = value;
        }
    }

    [ConfigurationProperty("value", DefaultValue = "", IsKey = false, IsRequired = false)]
    public string Value
    {
        get
        {
            return ((string)(base["value"]));
        }
        set
        {
            base["value"] = value;
        }
    }
}


#endregion

#region CmsSettings

public class CmsSettings
{
    private static string GetValue(string key)
    {
        CmsConfiguration cmsConfiguration = (CmsConfiguration)ConfigurationManager.GetSection("CmsConfiguration");

        foreach (CmsConfigurationElement element in cmsConfiguration.Settings)
        {
            if (element.Key == key)
                return element.Value;
        }
        return string.Empty;
    }

    public static string ParentField
    {
        get
        {
            return GetValue("ParentField");
        }
    }

    public static string SlugField
    {
        get
        {
            return GetValue("SlugField");
        }
    }

    public static string TitleField
    {
        get
        {
            return GetValue("TitleField");
        }
    }

    public static string IdField
    {
        get
        {
            return GetValue("IdField");
        }
    }

    public static string SupportEmail
    {
        get
        {
            return GetValue("SupportEmail");
        }
    }
}


#endregion