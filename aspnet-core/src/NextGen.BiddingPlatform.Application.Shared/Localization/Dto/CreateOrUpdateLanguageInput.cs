using System.ComponentModel.DataAnnotations;

namespace NextGen.BiddingPlatform.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}