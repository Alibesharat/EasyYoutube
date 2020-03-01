using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace VideoDownloder.Downloader
{
    public static class Helper
    {
        public static async Task<bool> CheckPermissionAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }

                return status == PermissionStatus.Granted;

            }
            catch (Exception ex)
            {
                throw ex;
                //Something went wrong
            }
        }
    }
}
