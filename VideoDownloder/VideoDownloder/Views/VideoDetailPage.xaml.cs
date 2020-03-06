using System.ComponentModel;
using VideoDownloder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoDownloder.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class VideoDetailPage : ContentPage
    {
       
        public VideoDetailPage(VideoDetailViewModel viewModel)
        {

            InitializeComponent();

            BindingContext = viewModel;

        }

       


    }
}