using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.RealTime;
using NextGen.BiddingPlatform.Friendships;

namespace NextGen.BiddingPlatform.Chat
{
    public class NullChatCommunicator : IChatCommunicator
    {
        public async Task SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message)
        {
            await Task.CompletedTask;
        }

        public async Task SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friend, bool isOwnRequest, bool isFriendOnline)
        {
            await Task.CompletedTask;
        }

        public async Task SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected)
        {
            await Task.CompletedTask;
        }

        public async Task SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState)
        {
            await Task.CompletedTask;
        }

        public async Task SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)
        {
            await Task.CompletedTask;
        }

        public async Task SendReadStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)
        {
            await Task.CompletedTask;
        }
    }
}