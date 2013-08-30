using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Text;
using SharpSvn;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;


public partial class _Default : System.Web.UI.Page
{
    string radioButtonSelectedItem = null;
    Uri reposUri = null;
    SvnClient client = null;
    string secMasterVersion = null;
    List<string> radVersion = null;
    List<string> refMasterVersion = null;



    protected void Page_Load(object sender, EventArgs e)
    {
        client = new SvnClient();
    }

    protected Uri GetSvnUri()
    {
        Uri url = new Uri(txtSvnUrl.Text);
        return url;
    }

    protected Uri GetAbsoluteUri(Uri repoPath)
    {
        string url = txtSvnUrl.Text;
        string reppath = repoPath.ToString();
        string result = url.Replace(reppath, "");
        result = result + reppath;
        return new Uri(result);
    }

    protected Uri GetAbsoluteUri(string relativePath)
    {
        string url = txtSvnUrl.Text;
        string temp = url.Replace(relativePath, "");
        StringBuilder result = new StringBuilder(temp + "/" +relativePath);
        return new Uri(result.ToString());
    }

    protected void ReadConfigFile(string secmVersion)
    {
        string tempurl = GetSvnUri().ToString() + "/" + secmVersion + "/DependentVersions.xml";
        Uri absolute = new Uri(tempurl);
        Debug.WriteLine(absolute.ToString());

        string temppath = @"C:\Users\athakur\Documents\Visual Studio 2010\WebSites\SvnWebsite_Test\Temp\";
        string xmlLoadPath = temppath + @"\DependentVersions.xml";
        SvnExportArgs exportargs = new SvnExportArgs { Overwrite = true };
        client.Export(absolute, temppath, exportargs);

        XElement xe = XElement.Load(xmlLoadPath);
        if (!xe.IsEmpty)
        {
            IEnumerable<XElement> rad = xe.Elements("RAD");
            foreach (XElement ra in rad)
            {
                radVersion = new List<string>();
                XAttribute tempatt = ra.Attribute("version");
                string[] str = tempatt.Value.Split(',');
                Debug.Write("RAD Versions :");
                foreach (string s in str)
                {
                    radVersion.Add(s);
                }
            }


            IEnumerable<XElement> refm = xe.Elements("RefMaster");
            foreach (XElement re in refm)
            {
                refMasterVersion = new List<string>();
                XAttribute tempatt = re.Attribute("version");
                string[] str = tempatt.Value.Split(',');
                Debug.Write("RefMaster Versions :");
                foreach (string s in str)
                {
                    refMasterVersion.Add(s);
                }
            }
        }

        ddlRefMasterVersion.DataSource = refMasterVersion;
        ddlRefMasterVersion.DataBind();
        ddlRadVersion.DataSource = radVersion;
        ddlRadVersion.DataBind();
    }



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        reposUri = GetAbsoluteUri(ddlSecMasterVersion.SelectedValue);
        radioButtonSelectedItem = optionList.SelectedValue;

        if (radioButtonSelectedItem == "Updates")
        {
            Collection<SvnLogEventArgs> logcollection = null;
            SvnLogArgs logargs = null;

            logargs = new SvnLogArgs { Range = new SvnRevisionRange(Int32.Parse(txtRevision.Text), SvnRevisionType.Head) };


            client.GetLog(reposUri, logargs, out logcollection);

            int i = 1;
            foreach (SvnLogEventArgs logEventArgs in logcollection)
            {
                Debug.WriteLine("Entry " + i);


                foreach (var changedpaths in logEventArgs.ChangedPaths)
                {
                    Uri absolute = GetAbsoluteUri(reposUri.ToString() + changedpaths.RepositoryPath.ToString());
                    if (changedpaths.Action.ToString() != "Delete")
                    {

                        SvnExportArgs exportargs = new SvnExportArgs();
                        exportargs.Overwrite = true;
                        System.Diagnostics.Debug.WriteLine(changedpaths.Action);

                        string[] directorytemp = changedpaths.RepositoryPath.ToString().Split('/');
                        string directorypath = null;

                        for (int j = 0; j < directorytemp.Length - 1; j++)
                        {
                            directorypath = directorypath + directorytemp[j] + '\\';
                        }

                        string temptarget = "C:\\Back Up\\Crap\\svnexport\\Updates" + directorypath;
                        Directory.CreateDirectory(temptarget);

                        string target = temptarget + directorytemp[directorytemp.Length - 1];
                        Collection<SvnInfoEventArgs> info = null;
                        bool flag = client.GetInfo(absolute, new SvnInfoArgs { ThrowOnError = false }, out info);
                        if (flag)
                        {
                            client.Export(absolute, target, exportargs);
                            Debug.WriteLine("File Created");
                        }
                    }
                    Debug.WriteLine(changedpaths.RepositoryPath);
                }

                i++;
                Debug.WriteLine("\n\n");
            }

        }


        else if (radioButtonSelectedItem == "Install")
        {
            string storepath = "C:\\Back Up\\Crap\\svnexport\\Install";
            SvnUpdateResult exportResult = null;
            client.Export(reposUri, storepath, new SvnExportArgs { Overwrite = true, Revision = SvnRevision.Head },out exportResult);
            Debug.WriteLine(exportResult.Revision);
        }

        else if (radioButtonSelectedItem == "Batch")
        {
            string storepath = "C:\\Back Up\\Crap\\svnexport\\Batch";
            SvnUpdateResult exportResult = null;
            client.Export(reposUri, storepath, new SvnExportArgs { Overwrite = true, Revision = new SvnRevision(long.Parse(txtRevision.Text))}, out exportResult);
            Debug.WriteLine(exportResult.Revision);
        }
    }


    protected void ddlSecMasterVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        secMasterVersion = ddlSecMasterVersion.SelectedValue;
        ReadConfigFile(secMasterVersion);


    }


    protected void btnCheckSvn_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            List<string> secmvers = new List<string>();
            Uri absolute = GetSvnUri();
            Collection<SvnInfoEventArgs> info = null;
            
            bool flag = client.GetInfo(absolute, new SvnInfoArgs { ThrowOnError = false }, out info);
            if (flag)
            {
                updatePanel.Visible = true;
                content.Style.Add("height", "370px");
                //content.Style.Add("width", "700px");
            }

            else
            {
                updatePanel.Visible = false;
            }

            Collection<SvnListEventArgs> infoList = null;
            client.GetList(absolute, out infoList);

            foreach (SvnListEventArgs sev in infoList)
            {
                if (sev.Path != "")
                {
                    Uri temppath = GetAbsoluteUri(sev.Path + "/DependentVersions.xml");
                    Collection<SvnInfoEventArgs> directorycollectioninfo = null;
                    bool chkflag = client.GetInfo(temppath, new SvnInfoArgs { ThrowOnError = false }, out directorycollectioninfo);
                    if (chkflag)
                    {

                        secmvers.Add(sev.Path);
                    }
                }
            }

            ddlSecMasterVersion.DataSource = secmvers;
            ddlSecMasterVersion.DataBind();

            secMasterVersion = secmvers[0];
            ReadConfigFile(secMasterVersion);

            radioButtonSelectedItem = optionList.SelectedValue;

            if (radioButtonSelectedItem == "Install")
            {
                rfvRevision.Enabled = false;
                test1.Style.Add("display", "none");
                test2.Style.Add("display", "none");
                test3.Style.Add("display", "none");
            }

            else
            {
                rfvRevision.Enabled = true;
                test1.Style.Add("display", "table-cell");
                test1.Style.Add("width", "120px");
                test2.Style.Add("display", "table-cell");
                test2.Style.Add("width", "200px");
                test3.Style.Add("display", "table-cell");
                test3.Style.Add("width", "200px");
            }
        }
    }



    protected void customSvnUrl_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            Uri tempp = new Uri(txtSvnUrl.Text);
            Debug.WriteLine(tempp);
            args.IsValid = true;
        }
        catch
        {
            Debug.WriteLine("Invalid Url");
            args.IsValid = false;
        }

    }
}