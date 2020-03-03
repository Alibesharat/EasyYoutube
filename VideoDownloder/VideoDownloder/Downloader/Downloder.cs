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

        string path = $@"storage/emulated/0/utubDownloader";
        public Utube()
        {
            client = new YoutubeClient();
        }
        public readonly Progress<double> Progress = new Progress<double>();

        public async Task DownloadPlayList(string Url)
        {
            var id = YoutubeClient.ParsePlaylistId(Url);
            var client = new YoutubeClient();
            var PlayList = await client.GetPlaylistAsync(id);

            path = Path.Combine(path, PlayList.Title.ValidNameForWindows());
            path.EnsureExsit();
            foreach (var video in PlayList.Videos)
            {
                await DownloadVideoAsync(video, path);
                await GenrateSubTitleAsync(video.Id, path, video.Title.ValidNameForWindows());
            }



        }

        public async Task DownloadPlayList(Video Video)
        {
            var id = Video.GetVideoMixPlaylistId();
            var client = new YoutubeClient();
            var PlayList = await client.GetPlaylistAsync(id);

            path = Path.Combine(path, PlayList.Title.ValidNameForWindows());
            path.EnsureExsit();
            foreach (var video in PlayList.Videos)
            {
                await DownloadVideoAsync(video, path);
                await GenrateSubTitleAsync(video.Id, path, video.Title.ValidNameForWindows());
            }



        }

        public async Task DownloadPlayList(Playlist PlayList)
        {
            path = Path.Combine(path, PlayList.Title.ValidNameForWindows());
            path.EnsureExsit();
            foreach (var video in PlayList.Videos)
            {
                await DownloadVideoAsync(video, path);
                await GenrateSubTitleAsync(video.Id, path, video.Title.ValidNameForWindows());
            }



        }


        public async Task DownloadVideoAsync(string url)
        {
            var id = YoutubeClient.ParseVideoId(url);
            var video = await client.GetVideoAsync(id);
            string ValidName = video.Title.ValidNameForWindows();
            await DownloadVideoAsync(video, path);
            await GenrateSubTitleAsync(id, path, ValidName);
        }

        public async Task DownloadVideoAsync(Video video)
        {
            try
            {
                string ValidName = video.Title.ValidNameForWindows();
                var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(video.Id);
                var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
                var ext = streamInfo.Container.GetFileExtension();
                path.EnsureExsit();
                await GenrateSubTitleAsync(video.Id, path, video.Title);
                await client.DownloadMediaStreamAsync(streamInfo, Path.Combine(path, $"{ video.Title}.{ext}"), Progress);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



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




        private async Task DownloadVideoAsync(Video video, string path)
        {
            try
            {

                string ValidName = video.Title.ValidNameForWindows();
                var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(video.Id);
                var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
                var ext = streamInfo.Container.GetFileExtension();
                path.EnsureExsit();
                await client.DownloadMediaStreamAsync(streamInfo, Path.Combine(path, $"{ video.Title}.{ext}"), Progress);

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
