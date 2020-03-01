using Downloader;
using System;
using System.Threading.Tasks;

namespace csl
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://www.youtube.com/watch?v=tL4gIcdejLc";
            Utube tuber = new Utube();
            tuber.Progress.ProgressChanged += Progress_ProgressChanged;
            await tuber.DownloadVideoByUrlAsync(url, "D:\\incom\\");

            Console.WriteLine("All Done");
            Console.ReadLine();
        }

        private static void Progress_ProgressChanged(object sender, double e)
        {
            Console.WriteLine($"Downloaded :  { Math.Floor(e * 100)} %");
        }
    }
}
