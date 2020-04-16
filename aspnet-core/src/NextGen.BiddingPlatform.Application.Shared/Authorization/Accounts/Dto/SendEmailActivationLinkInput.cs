using System.ComponentModel.DataAnnotations;

namespace NextGen.BiddingPlatform.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}