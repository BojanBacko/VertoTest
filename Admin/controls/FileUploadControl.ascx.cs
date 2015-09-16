using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_controls_FileUploadControl : System.Web.UI.UserControl
{

    public List<string> _allowedFileTypes = new List<string>();

    public string AllowedFileTypes { get; set; }
    public string FileErrorMessage { get; set; }
    public string Title { get; set; }


    public string FilePath { get; set; }
    public int ContentID { get; set; }
    public string Prefix { get; set; }

    public bool UseDbValueAsPrefix { get; set; }
    public string PrefixDbField { get; set; }
    public string DbFilenameField { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(AllowedFileTypes))
        {
            foreach (string s in AllowedFileTypes.Split(','))
            {
                _allowedFileTypes.Add(s);
            }

            litAllowedFiles.Text = string.Format("<strong>Please Note:</strong> Please only upload {0} files.", AllowedFileTypes);           

        }
        else
        {
            litError.Text = "<p class=\"error\">There are no allowed files.</p>";
            pnlFileUpload.Visible = false;
        }

        if (!string.IsNullOrEmpty(ReturnFilePath()))
            lnkViewCurrentFile.NavigateUrl = ReturnFilePath();
        else
        {
            lnkViewCurrentFile.Visible = false;
            btnDelete.Visible = false;
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fu.HasFile)
        {
            if (_allowedFileTypes.Contains(Path.GetExtension(fu.PostedFile.FileName).Substring(1)))
            {
                if (ContentID == 0)
                    fu.SaveAs(Server.MapPath(String.Format("{0}/{1}temp.{2}", FilePath, (!string.IsNullOrEmpty(Prefix) ? Prefix + "-" : ""), Path.GetExtension(fu.PostedFile.FileName).Substring(1))));
                else
                {
                    fu.SaveAs(Server.MapPath((string.Format("{0}/{2}{1}.{3}", FilePath, ContentID, (!string.IsNullOrEmpty(Prefix) ? Prefix + "-" : ""), Path.GetExtension(fu.PostedFile.FileName).Substring(1)))));
                    lnkViewCurrentFile.NavigateUrl = ReturnFilePath();
                    lnkViewCurrentFile.Visible = true;
                    btnDelete.Visible = true;
                    panelMessage.Visible = true;
                }
            }
            else
                litError.Text = "<p class=\"error\">The file you are trying to upload is not supported.</p>";
        }
        else
        {
            litError.Text = "<p class=\"error\">Please select a file first.</p>";
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ReturnFilePath()))
        {
            File.Delete(Server.MapPath(ReturnFilePath()));
            lnkViewCurrentFile.Visible = false;
            btnDelete.Visible = false;
            messagesText.InnerText = "File sucesfully deleted!";
            panelMessage.Visible = true;
        }
    }

    private string ReturnFilePath()
    {
        bool FileExists = false;
        string ext = "";
        foreach (string s in _allowedFileTypes)
        {
            if (File.Exists(Server.MapPath(string.Format("{0}/{2}{1}.{3}", FilePath, ContentID, (!string.IsNullOrEmpty(Prefix) ? Prefix + "-" : ""), s))))
            {
                FileExists = true;
                ext = s;
                break;
            }
            else
                FileExists = false;
        }

        if (FileExists)
            return string.Format("{0}/{2}-{1}.{3}", FilePath, ContentID, Prefix, ext);
        else
            return "";
    }

    public string ReturnFileName()
    {
        bool FileExists = false;
        string ext = "";
        foreach (string s in _allowedFileTypes)
        {
            if (File.Exists(Server.MapPath(string.Format("{0}/{2}{1}.{3}", FilePath, ContentID, (!string.IsNullOrEmpty(Prefix) ? Prefix + "-" : ""), s))))
            {
                FileExists = true;
                ext = s;
                break;
            }
            else
                FileExists = false;
        }

        return string.Format("{2}-{1}.{3}", FilePath, ContentID, Prefix, ext);

    }
}