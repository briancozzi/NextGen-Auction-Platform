using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}