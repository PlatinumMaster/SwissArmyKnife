using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SwissArmyKnife.Avalonia.Handlers {
    public class Net {
        private static HttpClient Client;
        
        public static void Initialize () {
            Client = new HttpClient(new SocketsHttpHandler {
                PooledConnectionLifetime = TimeSpan.FromSeconds(30)
            });
        }
        
        public static async Task<bool> DownloadFile(string URL, string SavePath) {
            HttpResponseMessage Message = await Client.GetAsync(URL);
            if (Message.IsSuccessStatusCode) {
                using (FileStream fs = new FileStream(SavePath, FileMode.OpenOrCreate)) {
                    await Message.Content.CopyToAsync(fs);
                }
                return true;
            }
            return false;
        }
    }
}