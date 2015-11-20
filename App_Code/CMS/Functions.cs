using System;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS {

    public static class Functions {

        public static void LogError(string message, string validationGroup = "console") {

            HttpContext.Current.Session["type"] = "3";
            HttpContext.Current.Session["msg"] = message;

            ((Page)HttpContext.Current.Handler).Validators.Add(new CustomValidator {
                ValidationGroup = validationGroup,
                ErrorMessage = message,
                IsValid = false
            });

        }

        public static string ConnString(string index) {
            return ConfigurationManager.ConnectionStrings[index].ConnectionString;
        }

        public static void LogError(Exception ex, string validationGroup = "console") {

            HttpContext.Current.Session["type"] = "3";
            HttpContext.Current.Session["msg"] = ex.Message;

            var st = new StackTrace(ex, true);
            var sf = st.GetFrame(st.FrameCount - 1);

            ((Page)HttpContext.Current.Handler).Validators.Add(new CustomValidator
            {
                ValidationGroup = validationGroup,
                ErrorMessage = string.Format("Message: {0}<br /> &gt; File: {1}<br /> &gt; Line: {2}<br />", ex.Message, sf.GetFileName(), sf.GetFileLineNumber()),
                IsValid = false
            });

        }

        public static void LogInfo(string message) {

            HttpContext.Current.Session["type"] = "2";
            HttpContext.Current.Session["msg"] = message;

        }

        public static void LogSuccess(string message) {

            HttpContext.Current.Session["type"] = "1";
            HttpContext.Current.Session["msg"] = message;

        }

        public static string ParseSlug(string input) {
            return Regex.Replace(input
                .ToLower()
                .Replace(" & ", " and ")
                .Replace(" / ", " or ")
                , @"[^\w]+", "-").Trim('-');
        }

        public static string ParseColumnIdentifier(string controlID) {
            if (controlID.StartsWith("col_")) {
                var idx = controlID.LastIndexOf('_') + 1;
                return controlID.Substring(idx, controlID.Length - idx);
            }
            if (controlID.StartsWith("binding_"))
                return "";
            return null;
        }

    }

}