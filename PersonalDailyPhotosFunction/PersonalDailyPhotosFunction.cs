using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ntfy;
using ntfy.Requests;
using PersonalDailyPhotosFunction.Common;
using PersonalDailyPhotosFunction.NTFY;
using PersonalDailyPhotosFunction.WebDav;

namespace PersonalDailyPhotosFunction
{
    public class PersonalDailyPhotosFunction
    {
        private readonly IWebDavInterop _webDav;
        private readonly INTFYInterop _ntfy;

        public PersonalDailyPhotosFunction(IWebDavInterop webDav, INTFYInterop ntfyInterop)
        {
            _webDav = webDav;
            _ntfy = ntfyInterop;
        }

        [FunctionName("PersonalDailyPhotosFunction")]
        public async Task Run([TimerTrigger("0 0 6 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            var randomDailyPhotosCollections = await _webDav.GetFolderContentsAsync(Util.GetEnvironmentVariable("WEBDAV_RANDOMPHOTOSFOLDER"));
            var randomDailyPhotosCollection = randomDailyPhotosCollections.PickRandom();

            var randomDailyPhotos = await _webDav.GetFolderContentsAsync(randomDailyPhotosCollection);
            var randomDailyPhoto = randomDailyPhotos.PickRandom();

            try
            {
                var imageUri = _webDav.GetFileUri(randomDailyPhoto.Uri);

                await _ntfy.SendNotification(new SendingMessage()
                {
                    Title = randomDailyPhotosCollection.DisplayName.DashCaseToSpacedCamelCase(),
                    Message = Path.GetFileNameWithoutExtension(randomDailyPhoto.DisplayName),
                    Attach = imageUri,
                });
            }
            catch (UnauthorizedException uae)
            {
                log.LogError(uae, "NTFY Authentication has failed");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "NTFY notification publish has failed");
            }
        }
    }
}
