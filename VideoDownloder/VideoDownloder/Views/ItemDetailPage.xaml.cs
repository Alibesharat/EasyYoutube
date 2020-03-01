using System.ComponentModel;
using VideoDownloder.ViewModels;
using Xamarin.Forms;
using YoutubeExplode.Models;

namespace VideoDownloder.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;
        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            string url = viewModel.Item.GetEmbedUrl();
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
          
        }

        public ItemDetailPage(Video item)
        {
            InitializeComponent();
            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        
    }
}