﻿using System;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using HellionExtendedServer.Common;

namespace HellionExtendedServer.Managers
{
    public class UpdateManager
    {
        private Release m_currentRelease;

        public Release CurrentRelease { get => m_currentRelease; set => m_currentRelease = value; }

        private static string LatestReleaseURL = @"https://api.github.com/repos/HellionCommunity/HellionExtendedServer/releases/latest";

        
        public UpdateManager()
        {
            ServicePointManager.DefaultConnectionLimit = 4;
        }

        public void DownloadLatestRelease()
        {
            Console.WriteLine("Checking for updates...");

            if (!m_currentRelease.IsUpdate)
            {
                Console.WriteLine("HES is up to date!");
                return;
            }

            Console.WriteLine("There's an update!");

            WebClient client = new WebClient();
            client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(ReleaseDownloaded);
            client.DownloadDataAsync(new Uri(m_currentRelease.URL));

        }

        private void ReleaseDownloaded(object sender, DownloadDataCompletedEventArgs e)
        {
            File.WriteAllBytes(@"Hes/updates/update.zip", e.Result);
            Console.WriteLine("Update Downloaded!");
        }

        public void GetLatestRelease()
        {
            string json;

            try
            {
                
                HttpWebRequest request = WebRequest.Create(LatestReleaseURL) as HttpWebRequest;
                request.Method = "GET";
                request.Proxy = null;
                request.UserAgent = "HellionExtendedServer";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        json = reader.ReadToEnd();
                    }
                }

                dynamic task = JObject.Parse(json);

                m_currentRelease = new Release(
                    (string)task["name"],
                    (string)task["assets"][0]["browser_download_url"],
                    (string)task["tag_name"],
                    (int)task["assets"][0]["download_count"],
                    (string)task["body"]                                       
                );               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
        }
    }

    public class Release
    {
        public string Name;
        public string URL;
        public string Version { get; private set; }
        public int DLCount;
        public string Description;
        public bool IsUpdate;


        public Release(string name, string url, string version, int dlCount, string description)
        {
            Name = name;
            URL = url;
            Version = version;
            DLCount = dlCount;
            IsUpdate = new Version(version) > HES.Version;
            Description = description;
        }

    }
}
