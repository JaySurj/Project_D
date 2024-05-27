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
    private const string AccessToken = "sl.B1-hRGuqIS0BDotiR-_xkdiMrYSzLRJLK8gJB6r5WFOL0JaqmtvJ9hQX5boR6UH9L6QYQeWb1to69poiQ8eSG_SFxxNzqzCGYg1ZwVxZlKYRuGvl4oivgiDRK0xxtF3ERw_O-hhEaQD2GpI";

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
