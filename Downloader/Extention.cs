using System.IO;

namespace Downloader
{
    public static class Extention
    {
        public static string ValidNameForWindows(this string text)
        {
            text = text.Replace("|", "_");
            text = text.Replace("<", "_");
            text = text.Replace(">", "_");
            text = text.Replace(":", "_");
            text = text.Replace("/", "_");
            text = text.Replace("\\", "_");
            text = text.Replace("?", "_");
            text = text.Replace("*", "_");

            return text;
        }


        public static void EnsureExsit(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }


    }
}
