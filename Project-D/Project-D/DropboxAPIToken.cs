using Dropbox.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Project_D
{
    class DropboxAPIToken
    {

        public static string Token = "sl.B3AoLoegsMG7_FTWO9Ui5_-m3PjuHsmBOr8MS5k5LbreIgd97GsoUfHtd2RF4Cge8g3eOxeHNG5mtAo_phqNkxrKBiXecEFMpKWHu70q1BUv24FlSuHdUqk3lkRESNxpRUcp1wq11E3WU4U";
        private readonly DropboxClient _dbx = new DropboxClient(Token);

        public async Task DownloadFileFromDropbox(string dropboxFolder, string dropboxFileName, string localFilename)
        {
            try
            {
                var response = await _dbx.Files.DownloadAsync(dropboxFolder + "/" + dropboxFileName);
                using (var fileContentStream = await response.GetContentAsStreamAsync())
                {
                    var localFilePath = Path.Combine(FileSystem.AppDataDirectory, localFilename);
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileContentStream.CopyToAsync(fileStream);
                    }
                }
                Debug.WriteLine($"Downloaded {dropboxFolder}/{dropboxFileName} to {localFilename}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Download failed: {ex.Message}");
            }
        }

    }
}
