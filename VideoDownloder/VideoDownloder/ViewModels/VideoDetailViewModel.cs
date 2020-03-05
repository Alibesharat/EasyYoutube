using Downloader;
using System;
using System.Threading.Tasks;
using VideoDownloder.Downloader;
using Xamarin.Forms;
using YoutubeExplode.Models;

namespace VideoDownloder.ViewModels
{
    public class VideoDetailViewModel : BaseViewModel
    {
        Utube tube;
        public Video Item { get; set; }
        public Command DownloadVideo { get; set; }


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




        public VideoDetailViewModel(Video item = null)
        {
            Title = item?.Title;
            Item = item;
            DownloadVideo = new Command(async () => await ExecuteDownloadVideo());
            tube = new Utube();
        }

        private async Task ExecuteDownloadVideo()
        {
            Result = "در حال آماده سازی برای دانلود ...";
            var status = await Helper.CheckPermissionWriteAsync();
            if (status)
            {

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
            Result = $"{(Math.Floor(e * 100))} %";
            if (e <= 100)
            {
                Progress = e;
            }

        }





    }
}
