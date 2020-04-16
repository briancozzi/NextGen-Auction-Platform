using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}