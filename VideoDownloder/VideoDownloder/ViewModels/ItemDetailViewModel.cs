using Downloader;
using System;
using System.Threading.Tasks;
using VideoDownloder.Downloader;
using Xamarin.Forms;
using YoutubeExplode.Models;

namespace VideoDownloder.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Command<string> DownloadVideo { get; set; }
        public Command<string> DownloadPlayList { get; set; }

        private string _Result;

        public string Result
        {
            get { return _Result; }
            set { SetProperty(ref _Result, value); }
        }


        private Double _Progress;

        public Double Progress
        {
            get { return _Progress; }
            set { SetProperty(ref _Progress, value); }
        }


        public ItemDetailViewModel(Video item = null)
        {
            Title = item?.Title;
            Item = item;
            DownloadVideo = new Command<string>(async (id) => await ExecuteDownloadVideo(id));
            DownloadPlayList = new Command<string>(async (id) => await ExecuteDownloadPlayList(id));

        }

        private async Task ExecuteDownloadVideo(string id)
        {
            Result = "در حال آماده سازی برای دانلود ...";
            var status = await Helper.CheckPermissionAsync();
            if (status)
            {
                Utube tube = new Utube();
                tube.Progress.ProgressChanged += Progress_ProgressChanged;
                await tube.DownloadVideoAsync(Item);

            }
            else
            {

                Result = "Premisson not granted";
            }

        }

        private void Progress_ProgressChanged(object sender, double e)
        {
            Result = $" دانلود شده  : {(Math.Floor(e * 100))} %";
            if (e <= 100)
            {
                Progress = e;
            }

        }

        private async Task ExecuteDownloadPlayList(string id)
        {
            var status = await Helper.CheckPermissionAsync();
            if (status)
            {
                Utube tube = new Utube();
                tube.Progress.ProgressChanged += Progress_ProgressChanged;
                await tube.DownloadPlayList(Item.GetUrl());

            }
            else
            {

                Result = "Premisson not granted";
            }
        }

        public Video Item { get; set; }

    }
}
