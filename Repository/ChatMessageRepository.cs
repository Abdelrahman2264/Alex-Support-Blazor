using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlexSupport.Repository
{
    public class ChatMessageRepository : IChatMessageRepoisitory
    {
        private readonly AlexSupportDB alexsupportdb;
        private readonly ILogger<ChatMessageRepository> logger;
        public ChatMessageRepository(AlexSupportDB alexsupportdb, ILogger<ChatMessageRepository> logger)
        {
            this.alexsupportdb = alexsupportdb;
            this.logger = logger;
        }
        public async Task<ChatMessage> CreatChatMessageAsync(ChatMessage chatMessage)
        {
            try
            {
                if (chatMessage == null)
                {
                    logger.LogError("ChatMessage is null in CreatChatMessageAsync");
                    return new ChatMessage();
                }
                chatMessage.SentDate = DateTime.Now;
                chatMessage.IsRead = false;
                await alexsupportdb.ChatMessages.AddAsync(chatMessage);
                await alexsupportdb.SaveChangesAsync();
                return chatMessage;
            }
            catch (Exception ex)
            {
                logger.LogError("Error In CreatChatMessageAsync: {Message}", ex.Message);
                return new ChatMessage();
            }

        }

        public async Task<IEnumerable<ChatMessage>> GetAllChatMessagesForTicketAsync(int id)
        {

            try
            {
                return await alexsupportdb.ChatMessages
                    .Include(u => u.Sender)
                    .Include(u => u.Ticket)
                    .Where(u => u.TicketId == id)
                    .OrderBy(u => u.SentDate)
                    .ToListAsync();

            }
            catch(Exception ex)
            {
                logger.LogError("Error in GetAllChatMessagesForTicketAsync"+ex.InnerException , ex);
                return Enumerable.Empty<ChatMessage>();
            }
        }

        public async Task<ChatMessage> GetChatMessageByIdAsync(int id)
        {
            try
            {
                var chatMessage = await alexsupportdb.ChatMessages
                    .Include(u => u.Sender)
                    .Include(u => u.Ticket)
                    .FirstOrDefaultAsync(u => u.CHID == id);
                if (chatMessage == null)
                {
                    logger.LogWarning("ChatMessage with ID {Id} not found", id);
                    return new ChatMessage();
                }
                return chatMessage;

            }
            catch (Exception ex)
            {
                logger.LogError("Error in GetChatMessageByIdAsync:" + ex.Message, ex);
                return new ChatMessage();
            }
        }

        public async Task<bool> RemoveChatMessageAsync(int id)
        {
            try
            {
                var chatMessage = alexsupportdb.ChatMessages.FirstOrDefault(u => u.CHID == id);
                if (chatMessage == null)
                {
                    logger.LogError("Ticket Not Found To Remove");
                    return false;
                }
                alexsupportdb.ChatMessages.Remove(chatMessage);
                await alexsupportdb.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                logger.LogError("Error in RemoveChatMessageAsync: " + ex.Message, ex);
                return false;
            }
        }

        public async Task<ChatMessage> UpdateChatMessageAsync(ChatMessage chatMessage)
        {
            try
            {
                var updatedChatMessage = await alexsupportdb.ChatMessages
                    .Include(u => u.Sender)
                    .Include(u => u.Ticket)
                    .FirstOrDefaultAsync(u => u.CHID == chatMessage.CHID);
                if (updatedChatMessage == null)
                {
                    logger.LogWarning("ChatMessage  not found for update");
                    return new ChatMessage();
                }
                updatedChatMessage.MessageText = chatMessage.MessageText;
                alexsupportdb.ChatMessages.Update(updatedChatMessage);
                await alexsupportdb.SaveChangesAsync();
                return updatedChatMessage;

            }
            catch (Exception ex)
            {
                logger.LogError("Error in UpdateChatMessageAsync: " + ex.Message, ex);
                return new ChatMessage();
            }

        }
    }
}
