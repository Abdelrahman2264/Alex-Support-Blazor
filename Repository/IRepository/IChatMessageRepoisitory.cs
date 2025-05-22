using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface IChatMessageRepoisitory
    {
        public Task<ChatMessage> CreatChatMessageAsync(ChatMessage chatMessage);
        public Task<ChatMessage> GetChatMessageByIdAsync(int id);
        public Task<IEnumerable<ChatMessage>> GetAllChatMessagesForTicketAsync(int id);
        public Task<ChatMessage> UpdateChatMessageAsync(ChatMessage chatMessage);
        public Task<bool> RemoveChatMessageAsync(int id);

    }
}
