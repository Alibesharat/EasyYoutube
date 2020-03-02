using System;
using System.ComponentModel;
using VideoDownloder.ViewModels;
using Xamarin.Forms;
using YoutubeExplode.Models;

namespace VideoDownloder.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {            
            ItemsListView.SelectedItem = null;
            if (!(args.SelectedItem is Video item))
                return;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
        }

       
        protected override void OnAppearing()
        {
            base.OnAppearing();

           
        }

        private void txt_query_Completed(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}