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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txt_url.Text)))
            {
                await DisplayAlert("Download", "Please Fill the url", "Ok");

            }
            else
            {

                var status = await Helper.CheckPermissionAsync();
                if (status)
                {

                    tube.Progress.ProgressChanged += Progress_ProgressChanged;
                    await tube.DownloadVideoAsync(txt_url.Text);

                }
                else
                {
                    prg.Text = "Premisson not granted";
                }


            }

        }

        private void Progress_ProgressChanged(object sender, double e)
        {
            prg.Text = $"Download Prgress : {(Math.Floor(e * 100)) } %";
        }



    }
}