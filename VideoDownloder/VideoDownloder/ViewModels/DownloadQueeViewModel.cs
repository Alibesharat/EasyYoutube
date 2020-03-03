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
        public ObservableCollection<Video> Items { get; set; }
        public Command SearchVideoCommand { get; set; }

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

        public DownloadQueeViewModel()
        {

            Title = "پلی لیست";
            Items = new ObservableCollection<Video>();
            SearchVideoCommand = new Command(async () => await ExecuteSearchVideoCommand());
            Visibily = false;

        }

        private async Task ExecuteSearchVideoCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Result = $"در حال جست وجو برای  برای پلی لیست ";
            try
            {
                Utube ut = new Utube();
                Items.Clear();
                var playlist = await ut.GetPlayList(SearchQuery);
                foreach (var item in playlist.Videos)
                {
                    Items.Add(item);
                }
                Result = $": {playlist.Videos.Count()} مورد";
                PlayListInfo = playlist.Adapt<PLayListInfo>();
                PlayListInfo.Count = playlist.Videos.Count();
                Visibily = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = $"خطا در دریافت اطلاعات";
                Visibily = false;

            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
