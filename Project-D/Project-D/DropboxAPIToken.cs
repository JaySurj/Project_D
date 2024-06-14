using Dropbox.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Project_D
{

    class DropboxAPIToken
    {
        public static string Token = "sl.B3LE-ylI0mf9WnxGKiTuX4f42ZzsWn_sZlMmq9WtXV_GKfn-OAtIxXWXjYCU1u7zlfWC0jhu5I5XOfTQeeTwlQ-pg8oC4Tu7nqxHJQxXXc8L_23Ukf7eN5OGNdX-TbpNUPJWWQiClQs8Irg";
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
