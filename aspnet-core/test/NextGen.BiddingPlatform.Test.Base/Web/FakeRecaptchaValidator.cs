using System.Threading.Tasks;
using NextGen.BiddingPlatform.Security.Recaptcha;

namespace NextGen.BiddingPlatform.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
