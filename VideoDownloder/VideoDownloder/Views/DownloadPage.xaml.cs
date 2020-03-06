using Downloader;
using System;
using System.Threading.Tasks;
using VideoDownloder.Downloader;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoDownloder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DownloadPage : ContentPage
    {
        Utube tube;
        public DownloadPage()
        {
            InitializeComponent();
            tube = new Utube();

        }

       

    }
}