using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CKFinder;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

public static class functions
{
    static public string RemoveHTML(string strText)
    {
        Regex Reg = new Regex("<[^>]*>");
        string res = Reg.Replace(strText, "");
        return res;
    }

    static public string ChopLastWord(string strText, int Length)
    {
        string s = RemoveHTML(strText.ToString());

        s = s.Length > Length ? s.Substring(0, Length) + "..." : s;

        if (s.Length > Length) s = s.LastIndexOf(' ') > 0 ? s.Substring(0, s.LastIndexOf(' ')) + "..." : s;

        return s;
    }

    static public string DoUrlString(string strText)
    {
        return Regex.Replace(strText, @"[^A-Za-z0-9_]+", "-");
    }

    static public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    static public string getFullPath(string slug, int parent)
    {
        string url = "/";

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString))
        {
            SqlCommand command = new SqlCommand("WITH cte (id,parent,lvl,fullslug) AS ( SELECT id, parent, lvl=1, slug as fullslug FROM tblContent WHERE slug = @slug AND parent = @parent AND published = 'True' UNION ALL SELECT t.id, t.parent, lvl=c.lvl+1, t.slug + '/' + c.fullslug as fullslug FROM tblContent t JOIN cte c ON t.id = c.parent AND t.parent <> c.parent WHERE published = 'True' ) SELECT TOP 1 '/' + fullslug as slug FROM cte ORDER BY lvl DESC", con);
            command.Parameters.Add(new SqlParameter("slug", slug));
            command.Parameters.Add(new SqlParameter("parent", parent));
            try
            {
                con.Open();
                url = (string)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        return url;
    }

    static public string PopulateBreadcrumb(string CurrentPage)
    {
        System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
        System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine(string.Format(Breadcrumb(), "/", "", "Home"));

        string strLink = "";
        foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
        {
            if (!strLink.Contains("/" + HttpContext.Current.Request.QueryString[key]))
            {
                if (!BreadcrumbBlackList().Contains(key))
                {
                    if (CurrentPage == HttpContext.Current.Request.QueryString[key])
                    {
                        strLink += "/" + HttpContext.Current.Request.QueryString[key];
                        string strName = textInfo.ToTitleCase(HttpContext.Current.Request.QueryString[key].Replace("-", " "));
                        sb.AppendLine(string.Format(Breadcrumb(), strLink, "selected", "<span class='skiptranslate'>&gt;</span> " + strName.Replace("And", "and")));
                    }
                    else
                    {
                        strLink += "/" + HttpContext.Current.Request.QueryString[key];
                        string strName = textInfo.ToTitleCase(HttpContext.Current.Request.QueryString[key].Replace("-", " "));
                        sb.AppendLine(string.Format(Breadcrumb(), strLink, "", "<span class='skiptranslate'>&gt;</span> " + strName.Replace("And", "and")));
                    }
                }
            }
        }

        return sb.ToString();
    }

    static private List<string> BreadcrumbBlackList()
    {
        List<string> l = new List<string>();
        l.Add("id");
        l.Add("page");
        l.Add("sort");
        l.Add("show");
        l.Add("returnurl");
        return l;
    }

    static public string Breadcrumb()
    {
        return "<a href=\"{0}\" class=\"{1}\">{2}</a>";
    }

    public static string SendMail(string mailFrom, string mailTo, string subject, string body, string successMsg, ref bool isError, FileUpload fileUpload = null, string mailFromDisplayName = "", string mailCc = "")
    {
        try
        {
            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("mail.netdash.net"))
            {
                string to = (!string.IsNullOrEmpty(mailTo) ? string.Join(",", mailTo) : null);
                string cc = (!string.IsNullOrEmpty(mailCc) ? string.Join(",", mailCc) : null);

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new System.Net.Mail.MailAddress(mailFrom, !string.IsNullOrEmpty(mailFromDisplayName) ? mailFromDisplayName : mailFrom);
                mail.To.Add(to);

                mail.Body = body;

                //add our attachment if we have a valid file
                if (fileUpload != null)
                {
                    //add our attachment chosen from the user
                    string fileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
                    System.Net.Mail.Attachment myAttachment = new System.Net.Mail.Attachment(fileUpload.FileContent, fileName);
                    mail.Attachments.Add(myAttachment);
                }

                if (!string.IsNullOrEmpty(cc))
                    mail.Bcc.Add(cc);

                mail.Subject = subject;
                mail.IsBodyHtml = false;
                client.Send(mail);
                isError = false;
                return successMsg;
            }
        }
        //unsuccessful, leave and return false
        catch (Exception ex)
        {
            isError = true;
            return "<p style=\"color:Red;\"><br /><br />" + ex.Message + "</p>";
        }
    }
}

public class ControlProperties : System.Web.UI.UserControl
{
    public int PageID { get; set; }
    public int ParentID { get; set; }
    public string PageTitle { get; set; }
    public string PageSubTitle { get; set; }
    public string PageSlug { get; set; }
    public string PageSummary { get; set; }
    public string PageContent { get; set; }
    public DateTime DatePublished { get; set; }
}

public class AdminFunctions : System.Web.UI.UserControl
{
    static public string RemoveHTML(string strText)
    {
        Regex Reg = new Regex("<[^>]*>");
        string res = Reg.Replace(strText, "");
        return res;
    }

    static public string ChopLastWord(string strText, int Length)
    {
        string s = RemoveHTML(strText.ToString());

        s = s.Length > Length ? s.Substring(0, Length) + "..." : s;

        if (s.Length > Length) s = s.LastIndexOf(' ') > 0 ? s.Substring(0, s.LastIndexOf(' ')) + "..." : s;

        return s;
    }

    static public string DoUrlString(string strText)
    {
        Regex reg = new Regex("[^a-zA-Z0-9 -]");

        string s = reg.Replace(strText, "");

        return s = (RemoveHTML(s).ToLower()).Replace(" ", "-").Replace("--", "-").Replace("---", "-");
    }
    
    public void GetCKF(object name)
    {
        CKFinder.FileBrowser FileBrowser = new CKFinder.FileBrowser();
        FileBrowser.BasePath = "/admin/ckfinder/";
        FileBrowser.SetupCKEditor(name);
    }

}