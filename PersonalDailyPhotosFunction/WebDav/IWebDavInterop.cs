using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDav;

namespace PersonalDailyPhotosFunction.WebDav
{
    public interface IWebDavInterop
    {
        Uri GetFileUri(string filePath);
        Task<IEnumerable<WebDavResource>> GetFolderContentsAsync(string folderPath);
        Task<IEnumerable<WebDavResource>> GetFolderContentsAsync(WebDavResource resource);
    }
}