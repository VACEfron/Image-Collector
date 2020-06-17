using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Image_Collector
{
    public class FileSaver
    {
        public static void SaveFile(string Type)
        {
            string subredditsJson = new StreamReader(Directory.GetCurrentDirectory() + $"/subreddits.json").ReadToEnd();
            var json = (JObject)JsonConvert.DeserializeObject(subredditsJson);
            string[] Subreddits = JsonConvert.DeserializeObject<List<string>>(json[Type].ToString()).ToArray();

            Console.WriteLine($">> Image Collector by VAC Efron\n\n{Type} will be saved in {Directory.GetCurrentDirectory() + $@"\{Type}\"}\n");

            while (true)
            {
                try
                {
                    Task.Delay(TimeSpan.FromSeconds(1));
                    Random r = new Random();

                    string[] SubSections = { "new.json?limit=1000", "rising.json?limit=1000", "top/.json?t=day&limit=1000", "top/.json?t=week&limit=1000", "top/.json?t=month&limit=1000" };
                    var Subname = Subreddits[r.Next(Subreddits.Length)];
                    var SubSection = SubSections[r.Next(SubSections.Length)];

                    string RequestUrl = $"https://www.reddit.com/r/{Subname}/{SubSection}";

                    var request = (HttpWebRequest)WebRequest.Create(RequestUrl);
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var data = (JObject)JsonConvert.DeserializeObject(responseString);
                    int postscount = data["data"]["dist"].Value<int>();
                    if (postscount == 0) continue;
                    string url = data["data"]["children"][r.Next(0, postscount)]["data"]["url"].Value<string>();

                    using (var WebClient = new WebClient())
                    {
                        byte[] Byte = WebClient.DownloadData(url);
                        var stream = new MemoryStream(Byte);
                        var path = Directory.GetCurrentDirectory() + $@"\{Type}\";

                        bool correctFileType = Regex.Match(url, @"^.*\.(png|jpg|jpeg|gif|GIF|mp4)$").Success;
                        if (!correctFileType) continue;

                        if (!Regex.Match(url, @"(https?:\/\/(.+?\.)?(gyphy\.com|imgur\.com|reddit\.com|redd\.it)(\/[A-Za-z0-9\-\._~:\/\?#\[\]@!$&'\(\)\*\+,;\=]*)?)").Success) continue;

                        var filename = Regex.Match(url, @"(?:[^/][\d\w\.]+)$").Value;
                        
                        if (filename is null) continue;
                        
                        SaveStreamAsFile(path, stream, filename);
                    }
                }
                catch { continue; }
            }
        }

        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)           
                info.Create();        

            string path = Path.Combine(filePath, fileName);
            if (File.Exists(path)) return;

            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
                Console.WriteLine($"Saved file: {fileName} | Total files saved: {Directory.GetFiles(filePath).Length}");
            }
        }
    }
}
