using System;
using System.Linq;

namespace Image_Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] Categories = { "memes", "cats", "dogs", "birds", "battlestations" };
            string ChooseCategory = string.Join("\n", Categories.Select(x => $" - {x}"));

            Console.WriteLine(
                ">> Image Collector by VAC Efron\n" +
                "Automatically save Images from various Reddit communities.\n\n" +
                "Disclaimer: Reddit proves a limited amount of posts, meaning that eventually there will be no more memes to save. You'll have to wait until new posts appear on the subreddits.\n\n" +
                ">> Discord: https://discord.gg/TtR32WT");
        Read:
            Console.WriteLine($"\nChoose a category:\n{ChooseCategory}");
            var read = Console.ReadLine().ToLower();
            if (Categories.Any(x => x == read))
            {
                Console.Clear();
                FileSaver.SaveFile(char.ToUpper(read[0]) + read.Substring(1));
            }
            else goto Read;
        }
    }
}
