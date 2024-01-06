using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PersonalDailyPhotosFunction.Common;
using WebDav;

namespace PersonalDailyPhotosFunction.WebDav
{
    public class WebDavInterop : IWebDavInterop
    {
        private readonly WebDavClient _client;
        private readonly Uri _webDavUrl;

        public WebDavInterop()
        {

            _webDavUrl = new Uri(Util.GetEnvironmentVariable("WEBDAV_URL"));
            _client = new WebDavClient(new WebDavClientParams()
            {
                BaseAddress = _webDavUrl
            });
        }

        public async Task<IEnumerable<WebDavResource>> GetFolderContentsAsync(string folderPath)
        {
            var folderUri = _webDavUrl.Combine(folderPath);

            return await PropFind(folderUri);
        }

        public async Task<IEnumerable<WebDavResource>> GetFolderContentsAsync(WebDavResource resource)
        {
            if (resource.IsCollection)
            {
                return await PropFind(_webDavUrl.Combine(resource.Uri));
            }

            return Array.Empty<WebDavResource>();
        }

        public Uri GetFileUri(string filePath)
        {
            return _webDavUrl.Combine(filePath);
        }

        private async Task<IEnumerable<WebDavResource>> PropFind(Uri uri)
        {
            var folderContents = await _client.Propfind(uri);

            if (folderContents.IsSuccessful)
            {
                var list = folderContents.Resources.ToList();
                list.RemoveAt(0);
                return list;
            }

            return Array.Empty<WebDavResource>();
        }
    }
}
