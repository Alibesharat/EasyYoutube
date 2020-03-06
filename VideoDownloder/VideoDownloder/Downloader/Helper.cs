using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace VideoDownloder.Downloader
{
    public static class Helper
    {
        public static async Task<bool> CheckPermissionWriteAsync()
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
                Analytics.TrackEvent("Write Permission got an Exception", new Dictionary<string, string> {
                { "Eror", ex.Message},});
                return false;

            }
        }


        public static async Task<bool> CheckPermissionReadAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                }

                return status == PermissionStatus.Granted;

            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Read Permission got an Exception", new Dictionary<string, string> {
                { "Eror", ex.Message},});
                return false;
                //Something went wrong
            }
        }
    }
}
