using AlexSupport.Data;
using AlexSupport.Repository;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace AlexSupport.Services.Extensions
{
    public interface ITicketChatService
    {
        Task SendMessageAsync(int ticketId, string senderId, string message);
        Task<IEnumerable<ChatMessage>> GetTicketMessagesAsync(int ticketId);
        Task MarkMessagesAsReadAsync(int ticketId, string userId);
    }

    public class TicketChatService : ITicketChatService
    {
        private readonly IChatMessageRepoisitory _chatRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<TicketChatService> _logger;
        private readonly AlexSupportDB alexSupportDB;
        private readonly IJSRuntime JS;

        public TicketChatService(
            IChatMessageRepoisitory chatRepository,
            INotificationService notificationService,
            ILogger<TicketChatService> logger,
            AlexSupportDB alexSupportDB,
            IJSRuntime JS)
        {
            _chatRepository = chatRepository;
            _notificationService = notificationService;
            _logger = logger;
            this.alexSupportDB = alexSupportDB;
            this.JS = JS;
        }

        public async Task SendMessageAsync(int ticketId, string senderId, string message)
        {
            try
            {
                // Save to database
                var chatMessage = new ChatMessage
                {
                    TicketId = ticketId,
                    SenderId = int.Parse(senderId),
                    MessageText = message,
                    SentDate = DateTime.Now,
                    IsRead = false
                };

                var savedMessage = await _chatRepository.CreatChatMessageAsync(chatMessage);

                // Get all users involved in this ticket (you'll need to implement this)
                await _notificationService.SendChatMessageAsync(senderId, ticketId.ToString(), chatMessage.MessageText);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending chat message");
            }
        }

        public async Task<IEnumerable<ChatMessage>> GetTicketMessagesAsync(int ticketId)
        {
            return await _chatRepository.GetAllChatMessagesForTicketAsync(ticketId);
        }

        public async Task MarkMessagesAsReadAsync(int ticketId, string userId)
        {
            var messages = await _chatRepository.GetAllChatMessagesForTicketAsync(ticketId);
            foreach (var message in messages.Where(m => m.IsRead == false && m.SenderId.ToString() != userId))
            {
                message.IsRead = true;
                await _chatRepository.UpdateChatMessageAsync(message);
            }
        }


    }
}
