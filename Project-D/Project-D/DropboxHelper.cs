
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;
using System.Threading.Tasks;

namespace Project_D
{
    public static class DropboxHelper
    {
        public static async Task<string> DownloadDatabaseAsync()
        {
            //helper
            using (var dbx = new DropboxClient(DropboxAPIToken.Token))
            {
                var response = await dbx.Files.DownloadAsync("/path/to/app.db");
                var fileContent = await response.GetContentAsByteArrayAsync();
                var localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db");
                File.WriteAllBytes(localPath, fileContent);
                return localPath;
            }
        }
    }
}