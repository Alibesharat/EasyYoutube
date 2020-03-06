using Plugin.Multilingual;
using System.Globalization;
using VideoDownloder.Services;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace VideoDownloder
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            CultureInfo.DefaultThreadCurrentCulture = CrossMultilingual.Current.DeviceCultureInfo;
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=015f5fdd-9033-487a-8043-6e34bed6e9b2;",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
