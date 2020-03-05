using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoDownloder.Downloader;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace Downloader
{
    public class Utube
    {
        YoutubeClient client;

        string path = $@"storage/emulated/0/utubDownloader";

        public delegate void DownloadCounterHandler(object sender, int e, string message);
        public event DownloadCounterHandler On_Download_Finish;
        public Utube()
        {
            client = new YoutubeClient();
        }
        public readonly Progress<double> Progress = new Progress<double>();




        public async Task<Playlist> GetPlayList(string Url)
        {
            if (YoutubeClient.TryParsePlaylistId(Url, out string Id))
            {
                var client = new YoutubeClient();
                var PlayList = await client.GetPlaylistAsync(Id);
                return PlayList;
            }
            return default;

        }

        public async Task DownloadPlayListAsync(Playlist PlayList)
        {
            int count = 0;
            path = Path.Combine(path, PlayList.Title.ValidNameForWindows());
            path.EnsureExsit();
            foreach (var video in PlayList.Videos)
            {

                bool IsExsit = await CheckExsit(video.Title, path);
                if (!IsExsit)
                {
                    await DownloadAsync(video, path);
                    await GenrateSubTitleAsync(video.Id, path, video.Title.ValidNameForWindows());
                    count++;
                    On_Download_Finish(this, count, "دانلود تکمیل شد");
                }
                else
                {
                    int cureent = count;
                    count++;
                    On_Download_Finish(this, -1, $" ویدیو  {cureent.ToPersianTextFirndly()} از قبل دانلود شده بود");
                }

            }

            On_Download_Finish(this, -1, $"پلی لیست کامل دانلود شد");


        }
        public async Task DownloadVideoAsync(Video video)
        {
            if (!await CheckExsit(video.Title, path))
            {
                await DownloadAsync(video);
                On_Download_Finish(this, 1, "دانلود ویدیو انجام شد");
            }
            else
            {
                On_Download_Finish(this, -1, "این ویدیو از قبل دانلود شده است");

            }

        }



        private async Task<bool> CheckExsit(string title, string path)
        {
            var IsGranted = await Helper.CheckPermissionReadAsync();
            if (IsGranted)
            {
                string[] files = Directory.GetFiles(path, $"{title}*");
                return files.Length > 0;
            }
            return IsGranted;
        }
        private async Task DownloadAsync(Video video, string Savepath = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Savepath)) Savepath = path;
                string ValidName = video.Title.ValidNameForWindows();
                var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(video.Id);
                var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
                var ext = streamInfo.Container.GetFileExtension();
                Savepath.EnsureExsit();
                await client.DownloadMediaStreamAsync(streamInfo, Path.Combine(Savepath, $"{ video.Title}.{ext}"), Progress);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private async Task<string> GenrateSubTitleAsync(string id, string path, string Name)
        {
            try
            {
                string FullPath = Path.Combine(path, $"{Name}.srt");

                var trackInfos = await client.GetVideoClosedCaptionTrackInfosAsync(id);
                if (trackInfos != null && trackInfos.Count() > 0)
                {
                    var trackInfo = trackInfos.First(t => t.Language.Code == "en");
                    var track = await client.GetClosedCaptionTrackAsync(trackInfo);
                    using StreamWriter file =
                    new StreamWriter(FullPath);
                    int line = 1;
                    foreach (var item in track.Captions)
                    {
                        string from = $"{item.Offset.Hours.ToString("00")}:{item.Offset.Minutes.ToString("00")}:{item.Offset.Seconds.ToString("00")},{item.Offset.Milliseconds.ToString("000")}";
                        TimeSpan ToSpaon = item.Offset.Add(item.Duration);
                        string to = $"{ToSpaon.Hours.ToString("00")}:{ToSpaon.Minutes.ToString("00")}:{ToSpaon.Seconds.ToString("00")},{ToSpaon.Milliseconds.ToString("000")}";

                        file.WriteLine(line);
                        file.WriteLine($"{from} --> {to}");
                        file.WriteLine(item.Text);
                        file.WriteLine();
                        line++;
                    }
                    return $"subtitle Write success to file : {FullPath}";
                }
                return "subtitle Not Find";
            }
            catch (Exception ex)
            {
                return "subtitle Not Find";
                //throw ex;
            }


        }




    }
}
