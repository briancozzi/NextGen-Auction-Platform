using System.ComponentModel.DataAnnotations;

namespace NextGen-Auction-Platform.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}