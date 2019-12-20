using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Meme_Collector
{
    public class FileSaver
    {
        public static void SaveFile()
        {
            Console.WriteLine($"\nMemes will be saved in {Directory.GetCurrentDirectory() + @"\Memes\"}\n");
            while (true)
            {
                try
                {
                    Task.Delay(TimeSpan.FromSeconds(1));
                    Random r = new Random();
                    string[] SubNames = {
                        "memes",
                        "dankmemes",
                        "pewdipiesubmissions",
                        "funny",
                        "me_irl",
                        "bikinibottomtwitter",
                        "bonehurtingjuice",
                        "wholesomememes",
                        "dankchristianmemes",
                        "animemes",
                        "deepfriedmemes",
                        "programmerhumor",
                        "bruhmoment",
                        "historymemes",
                        "expanddong",
                        "okbuddyretard"
                    };
                    var Subname = SubNames[r.Next(SubNames.Length)];
                    string[] random = {
                    $"https://www.reddit.com/r/{Subname}/new.json",
                    $"https://www.reddit.com/r/{Subname}/rising.json",
                    $"https://www.reddit.com/r/{Subname}/top/.json?t=day" };

                    var request = (HttpWebRequest)WebRequest.Create(random[r.Next(random.Length)]);
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var data = (JObject)JsonConvert.DeserializeObject(responseString);
                    int postscount = data["data"]["dist"].Value<int>();
                    if (postscount is 0) continue;
                    var rng = r.Next(0, postscount);
                    string url = data["data"]["children"][rng]["data"]["url"].Value<string>();

                    using (var WebClient = new WebClient())
                    {
                        byte[] Byte = WebClient.DownloadData(url);
                        var stream = new MemoryStream(Byte);
                        var directory = Directory.GetCurrentDirectory() + @"\Memes\";
                        var path = Path.Combine(directory);
                        string filename = Regex.Match(url, @"\/([A-Za-z0-9\-._~:?#\[\]@!$%&'()*+,;=]*)(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG)").Groups[1].Value + ".png";
                        if (filename is "") continue;
                        Console.WriteLine($"Saving file: {filename}");
                        SaveStreamAsFile(path, stream, filename);
                    }
                }
                catch (Exception e) { Console.WriteLine(e); continue; }
            }
        }

        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }
        }
    }
}
