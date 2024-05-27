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
    private const string AccessToken = "sl.B10QGo5zrCilHfGcZk7HKIpC8yAgRfNlc1ZpLke521ZPVFJPhYXIzmPehm-6hU-wfroZkmy0p1vSss2ghMAp4a93upraxR7gELT387WLyWHgDrVF5fe-bwOTj9WdO3N3E2ukTHzAlM_SULA";

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
