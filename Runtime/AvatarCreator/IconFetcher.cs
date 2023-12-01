using System.Threading;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.Networking;

public class IconFetcher
{
    private readonly CancellationToken ctx;
    private readonly WebRequestDispatcher webRequestDispatcher = new WebRequestDispatcher();
    private readonly int size;

    public IconFetcher(int iconSize = 64, CancellationToken ctx = default)
    {
        size = iconSize;
        ctx = ctx;
    }

    public async Task<Texture> GetIcon(string url)
    {
        url = $"{url}?w={size.ToString()}";
        Debug.Log($"Get icon {url}");
        var downloadHandler = new DownloadHandlerTexture();
        var response = await webRequestDispatcher.SendRequest<ResponseTexture>(url, HttpMethod.GET, downloadHandler: downloadHandler, ctx: ctx);
        response.ThrowIfError();
        return response.Texture;
    }
}
