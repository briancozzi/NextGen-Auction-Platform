using System.Collections.Generic;

namespace NextGen.BiddingPlatform.Chat.Dto
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}