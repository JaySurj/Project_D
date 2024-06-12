using Dropbox.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Dropbox.Api.Auth;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Project_D
{
    class DropboxAPIToken
    {

        private const string DropboxAppKey = "56879fw3gl8xus7";
        private const string DropboxAppSecret = "YOUR_APP_SECRET";
        private const string RedirectUri = "https://www.dropbox.com/oauth2/redirect_receiver"; // Temporary redirect URI
        public static string Token = "sl.B29Ht6ekr9jSrnFTfkip7BlA9Ci7aHi3XhlLG1XB95s7OWVk_cqsMn6lQXCPtvxmBgqYR4bqTGrOqeDLO0XiHEBYx6OtmYuS645rzYLoR2pOcEtvJSAYk5m9mwY9BvrvnVhXbCZ3qMX3VRA";

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(Token))
                    return Token;

                var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, DropboxAppKey, new Uri(RedirectUri));

                var result = await WebAuthenticator.AuthenticateAsync(authorizeUri, new Uri(RedirectUri));

                Token = result.AccessToken;

                return Token;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Authentication failed: {ex.Message}");
                return null;
            }
        }


        public async Task DownloadFileFromDropbox(string dropboxFolder, string dropboxFileName, string localFilename)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                var dbx = new DropboxClient(accessToken);

                var response = await dbx.Files.DownloadAsync(dropboxFolder + "/" + dropboxFileName);
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
