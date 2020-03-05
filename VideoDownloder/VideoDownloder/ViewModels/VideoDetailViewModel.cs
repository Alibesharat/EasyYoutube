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

        private string _Message;

        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
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
                tube.On_Download_Finish += Tube_On_Download_Finish;
                await tube.DownloadVideoAsync(Item);

            }
            else
            {

                Result = "Premisson not granted";
            }

        }

        private void Tube_On_Download_Finish(object sender, int e, string message)
        {
            Message = message;
            Result = "اتمام عملیات";
        }

        private void Progress_ProgressChanged(object sender, double e)
        {

            var rounded = Math.Floor(e * 100);
            Result = $" {rounded} %";
            if (e <= 1)
            {
                Progress = e;

            }

        }





    }
}
