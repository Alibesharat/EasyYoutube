using System;
using System.Collections.ObjectModel;
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


      
        public HomeViewModel()
        {
            Title = "Home";
            Items = new ObservableCollection<Video>();
            SearchVideoCommand = new Command(async () => await ExecuteSearchVideoCommand());

        }

      

        async Task ExecuteSearchVideoCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
           
            try
            {
                Items.Clear();
                var items = await DataStore.SearchItemsAsync(SearchQuery, true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
               

            }
            finally
            {
                IsBusy = false;
            }
        }



    }
}