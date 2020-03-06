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
            text = text.Replace('"', '_');

            return text;


           
        }


        public static void EnsureExsit(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }


        public static string ToPersianTextFirndly(this int number)
        {
            return number switch
            {
                1 => "اول",
                2 => "دوم",
                3 => "سوم",
                4 => "چهارم",
                5 => "پنجم",
                6 => "ششم",
                7 => "هفتم",
                8 => "هشتم",
                9 => "نهم",
                10 => "دهم",
                11 => "بازدهم",
                12 => "دوازدهم",
                13 => "سیزدهم",
                14 => "چهاردهم",
                15 => "پانزدهم",
                16 => "شانزدهم",
                17 => "هفدهم",
                18 => "هیجدهم",
                19 => "نوزدهم",
                20 => "بیستم",
                _ => number.ToString(),
            };
        }






    }
}
