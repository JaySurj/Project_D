using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Api;
using Microsoft.Maui.Devices;
using Project_D;

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
    public static DropboxClient GetClient()
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            return new DropboxClient(DropboxAPIToken.Token, new DropboxClientConfig()
            {
                HttpClient = new HttpClient(new MyAndroidMessageHandler())
            });
        }
        return new DropboxClient(DropboxAPIToken.Token);
    }
}
