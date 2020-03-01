using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace Downloader
{
    public class Utube
    {
        YoutubeClient client;
        public Utube()
        {
            client = new YoutubeClient();
        }
        public readonly Progress<double> Progress = new Progress<double>();
        public async Task DownloadVideoByUrlAsync(string url, string path)
        {
            var id = YoutubeClient.ParseVideoId(url);
            var video = await client.GetVideoAsync(id);
            string ValidName = video.Title.ValidNameForWindows();
            await DownloadVideoAsync(video, path);
            await GenrateSubTitleAsync(id, path, ValidName);
        }




        public async Task DownloadPlayList(string url, string Path)
        {
            var id = YoutubeClient.ParsePlaylistId(url);
            var client = new YoutubeClient();
            var PlayList = await client.GetPlaylistAsync(id);
            Path += $"//{PlayList.Title.ValidNameForWindows()}";
            Path.EnsureExsit();
            foreach (var video in PlayList.Videos)
            {

                await DownloadVideoAsync(video, Path);
                await GenrateSubTitleAsync(video.Id, Path, video.Title.ValidNameForWindows());
            }

           

        }




        private async Task DownloadVideoAsync(Video video, string path)
        {
            string ValidName = video.Title.ValidNameForWindows();
            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(video.Id);
            var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
            var ext = streamInfo.Container.GetFileExtension();
            path.EnsureExsit();
            await client.DownloadMediaStreamAsync(streamInfo, $"{path}\\{ValidName}.{ext}", Progress);
           
        }

        private async Task<string> GenrateSubTitleAsync(string id, string Path, string Name)
        {
            try
            {
                string FullPath = $@"{Path}\\{ Name.ValidNameForWindows()}.srt";

                var trackInfos = await client.GetVideoClosedCaptionTrackInfosAsync(id);

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
            catch (Exception ex)
            {

                throw ex;
            }


        }




    }
}
