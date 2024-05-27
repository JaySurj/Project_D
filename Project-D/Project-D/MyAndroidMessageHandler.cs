using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Api;
using Microsoft.Maui.Devices;

public class MyAndroidMessageHandler : HttpClientHandler
{
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri.AbsolutePath.Contains("files/download"))
        {
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        }
        return await base.SendAsync(request, cancellationToken);
    }
}

public static class DropboxClientFactory
{
    private const string AccessToken = "sl.B2Co7f-lbnKfyc-OKac4Ue6ylfvflmfe2i-eafSZdc0MufOKileTw099rj_I48csgBLk9DBRfvRnuzNzGS4mroV5KkwlaDJAPKbJUYIz7F2mJHOY6uzLX6RSdaJTHFyy_T7vCKO65atvjSM";

    public static DropboxClient GetClient()
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            return new DropboxClient(AccessToken, new DropboxClientConfig()
            {
                HttpClient = new HttpClient(new MyAndroidMessageHandler())
            });
        }
        return new DropboxClient(AccessToken);
    }
}
