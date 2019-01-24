/* Filename: UpdateFormDesigner.cs
 * Author: Marcin Ostrowski
 * Email: <ostrowski.marcin.gno@gmail.com>
 * Description: This form update application.
 * License: LGPL v3
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpBucket;
using Bitbucket.Net;

namespace L3DS_Application
{
    public partial class UpdateForm : Form
    {
        #region Bitbucket settings.
        private static readonly string consumerKey = "Pjc9z3HJryn2m7GXTm";
        private static readonly string consumerSecretKey = "Z4zYYSjZknjwDCFajQWQybmeb2753xwZ";
        private static readonly string repoName = "l3ds-software";
        private static readonly string deploymentPath = "deployment";
        private static readonly string userName = "Printer3D";
        private static readonly string zipPattern = "package-{0}.zip";
        #endregion

        #region Structures

        // Store information about packages.
        public class VersionInfo
        {
            public readonly int major;
            public readonly int minor;
            public readonly int compilation;
            public readonly int revision;

            // Setting constructor.
            public VersionInfo(int _major, int _minor, int _compilation, int _revision)
            {
                major = _major;
                minor = _minor;
                compilation = _compilation;
                revision = _revision;
            }

            // toString function.
            public String ConvertToString()
            {
                return major + "-" + minor + "-" + compilation + "-" + revision;
            }

            // Sort functions.
            public static int Sort(VersionInfo a, VersionInfo b)
            {
                if (a.major > b.major)
                    return 1;

                if (a.minor > b.minor && a.major == b.major)
                    return 1;

                if (a.compilation > b.compilation && a.major == b.major && a.minor == b.minor)
                    return 1;

                return 0;
            }
        };
        #endregion

        List<VersionInfo> vers;

        // Default Constructor.
        public UpdateForm()
        {
            InitializeComponent();
            LoadUpdates();
        }

        // Load string list deployment package.
        private void LoadUpdates()
        {

            DownloadListDeploymentPackages(out vers);

            foreach (var ver in vers)
                updateListBox.Items.Add(ver.ConvertToString());

            if(vers.Count > 0)
                updateListBox.SelectedIndex = 0;
        }

        // Get list package.
        private void DownloadListDeploymentPackages(out List<VersionInfo> infos)
        {
            infos = new List<VersionInfo>();

            var client = new Bitbucket.Net.BitbucketClient("https://bitbucket.org/site/oauth2/access_token", consumerKey, consumerSecretKey);
       

            var files = client.GetRepositoryFilesAsync("l3ds-software", "master/src/deployment");

            Console.WriteLine(files.Result.Count());

            /*
            try
            {
                // initialize connect to bitbucket.org
                var sharpBucket = new SharpBucket.V1.SharpBucketV1();
                sharpBucket.OAuth2LeggedAuthentication(consumerKey, consumerSecretKey);

                // gettings endpoints.
                var userEndpoint = sharpBucket.UserEndPoint();
                var repoEndpoint = sharpBucket.RepositoriesEndPoint(userName, repoName);
                var mainBranch = repoEndpoint.GetMainBranch();

                // get list directories.
                var lists = repoEndpoint.ListSources(mainBranch, deploymentPath);

                // get packages list.
                foreach(var package in lists.files)
                {
                    string versionPathWithoutExtension = Path.GetFileNameWithoutExtension(package.path);
                    string[] version = versionPathWithoutExtension.Split('-');
                    if(version.Count() == 5)
                    {
                        int major, minor, compilation, revision = 0;
                        major = int.Parse(version[1]);
                        minor = int.Parse(version[2]);
                        compilation = int.Parse(version[3]);
                        revision = int.Parse(version[4]);

                        infos.Add(new VersionInfo(major, minor, compilation, revision));
                    }
                }

                // sort versions.
                infos.Sort((x, y) => VersionInfo.Sort(x, y));

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        private void InstallPackage(VersionInfo info)
        {
            try
            {
                // initialize connect to bitbucket.org
                var sharpBucket = new SharpBucket.V1.SharpBucketV1();
                sharpBucket.OAuth2LeggedAuthentication(consumerKey, consumerSecretKey);

                // gettings endpoints.
                var userEndpoint = sharpBucket.UserEndPoint();
                var repoEndpoint = sharpBucket.RepositoriesEndPoint(userName, repoName);
                var mainBranch = repoEndpoint.GetMainBranch();


                // find pacakge.
                string packageName = "package-" + info.ConvertToString() + ".zip";

                // get list directories.
                var lists = repoEndpoint.ListSources(mainBranch, deploymentPath);

                // get package & download.

                var package = lists.files.Find(x => x.path.ToLower().Contains(packageName));
                var src = repoEndpoint.GetSrcFile(mainBranch.name, package.path);
                // save to temp directory.
                UpdateApplicationFiles(Encoding.Default.GetBytes(src.data));

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateApplicationFiles(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {

                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    // save data files
                    foreach (var entry in zip.Entries)
                    {
                        string localPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + entry.FullName;
                        string archivePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + "Old_" + entry.FullName;

                        // renamed file.
                        if (File.Exists(localPath) == true)
                        {
                            File.Move(localPath, archivePath);
                        }

                        // save file.
                        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(localPath)))
                        {

                            using (Stream fileStream = entry.Open())
                            {
                                fileStream.CopyTo(writer.BaseStream);
                                fileStream.Close();
                            }

                            writer.Close();
                        }

                        // File.Move
                    }
                }
            }
        }




        private void updateButton_Click(object sender, EventArgs e)
        {
            InstallPackage(vers[updateListBox.SelectedIndex]);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void updateListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
