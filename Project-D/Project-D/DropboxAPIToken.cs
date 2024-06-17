using Dropbox.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Project_D
{

    class DropboxAPIToken
    {
        public static string Token = "sl.B3UkmDQ8RpMYvJTm71MJwWq9sOErud1w-CeD1snWi2Zbxjw5OFr6J2uqq2LUzmLJ_ZtRjUbzbD4uMRjuZmgqTxpEIfyFxQoolr3Ki-IqYu3OKLcXydxDshEUIGW7FCinFZE_sY3N_oR4f2E";
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
