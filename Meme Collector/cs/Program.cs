using System;

namespace Meme_Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(">> Meme Collector by VAC Efron#0001\n>> Get memes from various Reddit communities and save them in a folder.\n>> Discord: https://discord.gg/TtR32WT");
            Console.WriteLine("\nAre you ready?\n\n - start\n");
            Read:
            var read = Console.ReadLine().ToLower();
            if (read is "start")
                FileSaver.SaveFile();
            else goto Read;
        }
    }
}
