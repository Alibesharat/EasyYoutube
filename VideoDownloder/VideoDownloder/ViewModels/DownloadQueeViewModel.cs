using Downloader;
using Mapster;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VideoDownloder.Models;
using Xamarin.Forms;
using YoutubeExplode.Models;

namespace VideoDownloder.ViewModels
{
    public class DownloadQueeViewModel : BaseViewModel
    {
        Utube ut;
        Playlist playlist;
        public ObservableCollection<Video> Items { get; set; }
        public Command SearchVideoCommand { get; set; }
        public Command DownloadPlayList { get; set; }

        private string _SearchQuery;

        public string SearchQuery
        {
            get { return _SearchQuery; }
            set { SetProperty(ref _SearchQuery, value); }
        }


        private PLayListInfo _PlayListInfo;

        public PLayListInfo PlayListInfo
        {
            get { return _PlayListInfo; }
            set { SetProperty(ref _PlayListInfo, value); }
        }


        private bool _Visibily;

        public bool Visibily
        {
            get { return _Visibily; }
            set { SetProperty(ref _Visibily, value); }
        }

        private string _Result;

        public string Result
        {
            get { return _Result; }
            set { SetProperty(ref _Result, value); }
        }

        private double _Progress;

        public double Progress
        {
            get { return _Progress; }
            set { SetProperty(ref _Progress, value); }
        }

        private string _VideoDownloadedCount;
        public string VideoDownloadedCount
        {
            get { return _VideoDownloadedCount; }
            set { SetProperty(ref _VideoDownloadedCount, value); }
        }



        private string _VideoDownloadingNumber;

        public string VideoDownloadingNumber
        {
            get { return _VideoDownloadingNumber; }
            set { SetProperty(ref _VideoDownloadingNumber, value); }
        }



        public DownloadQueeViewModel()
        {

            Title = "پلی لیست";
            Items = new ObservableCollection<Video>();
            SearchVideoCommand = new Command(async () => await ExecuteSearchVideoCommand());
            DownloadPlayList = new Command(async () => await DownloadPlayListExcute());
            Visibily = false;
            ut = new Utube();

        }

        private async Task DownloadPlayListExcute()
        {
            VideoDownloadingNumber = "در حال  دانلود ویدیو اول";
            Result = "در حال آماده سازی برای دانلود";
            ut.Progress.ProgressChanged += Progress_ProgressChanged;
            ut.On_Download_Finish += Ut_On_Download_Finish;
            await ut.DownloadPlayListAsync(playlist);

        }

        private void Ut_On_Download_Finish(object sender, int e, string message)
        {
            if (e == -1)
            {
                VideoDownloadedCount = message;
                VideoDownloadingNumber = $"در حال دانلود ویدیو  {(e + 1).ToPersianTextFirndly()} ";
            }
            else
            {
                VideoDownloadingNumber = $"در حال دانلود ویدیو  {(e + 1).ToPersianTextFirndly()} ";
                VideoDownloadedCount = $"ویدیو دانلود شده تا کنون  : {(e)}";

            }
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

        private async Task ExecuteSearchVideoCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {

                Items.Clear();
                playlist = await ut.GetPlayList(SearchQuery);
                foreach (var item in playlist.Videos)
                {
                    Items.Add(item);
                }
                PlayListInfo = playlist.Adapt<PLayListInfo>();
                Visibily = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Visibily = false;

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
