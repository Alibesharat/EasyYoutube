using Downloader;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace VideoDownloder.Services
{
    public class MockDataStore : IDataStore<Video>
    {
        readonly List<Video> items;

        public MockDataStore()
        {

        }

        public async Task<bool> AddItemAsync(Video item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Video item)
        {
            var oldItem = items.Where((Video arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Video arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Video> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Video>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Video>> SearchItemsAsync(string Query,bool forceRefresh = false)
        {
            YoutubeClient client = new YoutubeClient();
            var data = await client.SearchVideosAsync(Query,1);
            
            return data;
        }
    }
}