using System.ComponentModel.DataAnnotations;

namespace NextGen.Auction.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}