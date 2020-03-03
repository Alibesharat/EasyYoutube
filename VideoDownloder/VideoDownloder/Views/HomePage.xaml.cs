using System.ComponentModel;
using VideoDownloder.ViewModels;
using Xamarin.Forms;
using YoutubeExplode.Models;

namespace VideoDownloder.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;

        public HomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomeViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ItemsListView.SelectedItem = null;
            if (!(args.SelectedItem is Video item))
                return;
            await Navigation.PushAsync(new VideoDetailPage(new VideoDetailViewModel(item)));

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();


        }


    }
}