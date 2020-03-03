using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoDownloder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YoutubeExplode.Models;

namespace VideoDownloder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DownloadQuee : ContentPage
    {
        DownloadQueeViewModel viewModel;
        public DownloadQuee()
        {
            InitializeComponent();
            BindingContext = viewModel = new DownloadQueeViewModel() ;

        }

       

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ItemsListView.SelectedItem = null;
            if (!(e.SelectedItem is Video item))
                return;
            await Navigation.PushAsync(new VideoDetailPage(new VideoDetailViewModel(item)));

        }
    }
}