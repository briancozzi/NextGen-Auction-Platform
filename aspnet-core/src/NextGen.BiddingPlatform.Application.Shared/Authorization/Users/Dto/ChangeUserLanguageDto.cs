using System.ComponentModel.DataAnnotations;

namespace NextGen.BiddingPlatform.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
