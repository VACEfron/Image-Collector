using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Image_Collector
{
    public class FileSaver
    {
        public static async Task SaveFile(string type, bool firstTime)
        {
            try
            {
                Random r = new Random();

                var path = Directory.GetCurrentDirectory() + $@"\{type}\";

                string subredditsJson = new StreamReader(Directory.GetCurrentDirectory() + $"/subreddits.json").ReadToEnd();
                var json = (JObject)JsonConvert.DeserializeObject(subredditsJson);
                string[] subreddits = JsonConvert.DeserializeObject<List<string>>(json[type].ToString()).ToArray();

                if (firstTime)
                    Console.WriteLine($">> Image Collector by VAC Efron\n\n{type} will be saved in {Directory.GetCurrentDirectory() + $@"\{type}\"}\n");
                else
                    Console.WriteLine("\n>> Loading next wave...\n");

                string[] subSections = { "new.json?limit=1000", "rising.json?limit=1000", "top/.json?t=day&limit=1000", "top/.json?t=week&limit=1000", "top/.json?t=month&limit=1000" };
                var subname = subreddits[r.Next(subreddits.Length)];
                var subSection = subSections[r.Next(subSections.Length)];

                var httpClient = new HttpClient();

                string response = await httpClient.GetStringAsync($"https://www.reddit.com/r/{subname}/{subSection}");
                JObject data = (JObject)JsonConvert.DeserializeObject(response);

                int postCount = data["data"]["dist"].Value<int>();

                for (int i = 0; i < postCount; i++)
                {
                    try
                    {
                        if (postCount == 0) break;
                        string url = data["data"]["children"][r.Next(0, postCount)]["data"]["url"].Value<string>();

                        // Check URL for source & file type
                        if (!Regex.Match(url, @"(https?:\/\/(.+?\.)?(gyphy\.com|imgur\.com|reddit\.com|redd\.it)(\/[A-Za-z0-9\-\._~:\/\?#\[\]@!$&'\(\)\*\+,;\=]*)?)").Success ||
                            !Regex.Match(url, @"^.*\.(png|jpg|jpeg|gif|GIF|mp4)$").Success) continue;

                        var stream = await httpClient.GetStreamAsync(url);

                        var filename = Regex.Match(url, @"(?:[^/][\d\w\.]+)$").Value;
                        if (filename is null) continue;

                        SaveStreamAsFile(path, stream, filename);
                    }
                    catch { continue; }
                }
            }
            catch(Exception e)
            { 
                if (e is FileNotFoundException)
                {
                    Console.WriteLine(">> subreddits.json could not be found...");
                    Program.shouldContinue = false;
                }
                return; 
            }            
        }

        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)           
                info.Create();        

            string path = Path.Combine(filePath, fileName);
            if (File.Exists(path)) return;

            using FileStream outputFileStream = new FileStream(path, FileMode.Create);

            inputStream.CopyTo(outputFileStream);
            Console.WriteLine($"Saved file: {fileName} | Total files saved: {Directory.GetFiles(filePath).Length}");
        }
    }
}
