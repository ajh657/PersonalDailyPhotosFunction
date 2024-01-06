using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ntfy;
using ntfy.Requests;
using PersonalDailyPhotosFunction.Common;
using NTFYClient = ntfy.Client;

namespace PersonalDailyPhotosFunction.NTFY
{
    public class NTFYInterop : INTFYInterop
    {
        private readonly NTFYClient _client;
        private readonly User _user;
        private readonly string _notificationTopic;

        public NTFYInterop()
        {
            _client = new NTFYClient(Util.GetEnvironmentVariable("NTFY_URL"));
            _user = new User(Util.GetEnvironmentVariable("NTFY_KEY"));
            _notificationTopic = Util.GetEnvironmentVariable("NTFY_TOPIC");
        }

        public async Task SendNotification(SendingMessage message)
        {
            await _client.Publish(_notificationTopic, message, _user);
        }
    }
}
