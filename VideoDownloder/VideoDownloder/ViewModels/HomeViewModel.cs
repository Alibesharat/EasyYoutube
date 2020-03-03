using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using YoutubeExplode.Models;

namespace VideoDownloder.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<Video> Items { get; set; }
        public Command SearchVideoCommand { get; set; }

        private string _SearchQuery;

        public string SearchQuery
        {
            get { return _SearchQuery; }
            set { SetProperty(ref _SearchQuery, value); }
        }


        private string _Result;

        public string Result
        {
            get { return _Result; }
            set { SetProperty(ref _Result, value); }
        }

        public HomeViewModel()
        {
            Title = "جست وجو";
            Items = new ObservableCollection<Video>();
            SearchVideoCommand = new Command(async () => await ExecuteSearchVideoCommand());


        }

      

        async Task ExecuteSearchVideoCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Result = $"در حال جست وجو برای  {SearchQuery} : ";
            try
            {
                Items.Clear();
                var items = await DataStore.SearchItemsAsync(SearchQuery, true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                Result = $"نتایج جست و جو برای {SearchQuery} : {items.Count()} مورد";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Result = $"خطا در دریافت اطلاعات";

            }
            finally
            {
                IsBusy = false;
            }
        }



    }
}