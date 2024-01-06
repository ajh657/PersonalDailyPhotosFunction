using System.Threading.Tasks;
using ntfy.Requests;

namespace PersonalDailyPhotosFunction.NTFY
{
    public interface INTFYInterop
    {
        Task SendNotification(SendingMessage message);
    }
}