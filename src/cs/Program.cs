using System;
using System.Linq;
using System.Threading.Tasks;

namespace Image_Collector
{
    class Program
    {
        static void Main()
            => MainAsync().GetAwaiter().GetResult();

        public static bool shouldContinue = true;

        static async Task MainAsync()
        {
            string[] categories = { "memes", "cats", "dogs", "birds", "battlestations" };
            string chooseCategory = string.Join("\n", categories.Select(x => $" - {x}"));

            Console.WriteLine(
                ">> Image Collector by VAC Efron\n" +
                "Automatically save Images from various Reddit communities.\n\n" +
                "Disclaimer: Reddit proves a limited amount of posts, meaning that eventually there will be no more images to save. You'll have to wait until new posts appear on the subreddits.\n\n" +
                ">> Discord: https://discord.gg/xJ2HRxZ");

        Read:
            Console.WriteLine($"\nChoose a category:\n{chooseCategory}\n");
            var read = Console.ReadLine().ToLower();

            if (categories.Any(x => x == read))
            {
                bool firstTime = true;
                Console.Clear();
                while (shouldContinue)
                {
                    await FileSaver.SaveFile(char.ToUpper(read[0]) + read.Substring(1), firstTime);
                    firstTime = false;
                }
            }
            else goto Read;

            Console.ReadLine();
        }
    }
}
