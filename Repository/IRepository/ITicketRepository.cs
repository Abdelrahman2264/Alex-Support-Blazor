using AlexSupport.Services.Models;
using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface ITicketRepository
    {
        public Task<Ticket> CreateTicketAsync(Ticket ticket);
        public Task<IEnumerable<Ticket>> GetAllOpendTicketsAsync();
        public Task<Ticket> GetTicketByIdAsync(int id);
        public Task<bool> AssignTicketAsync(Ticket ticket, int Id);
        public Task<bool> EsclateTicketAsync(Ticket ticket, int Id);
        public Task<bool> CloseTicketAsync(Ticket ticket, int Id);
        public Task<bool> AddSolutionToTicketAsync(Ticket ticket, int Id);
        public Task<IEnumerable<Ticket>> GetAllAssignedTickets();
        public Task<IEnumerable<Ticket>> GetAllClosedTicketsAsync();
        public Task<IEnumerable<Ticket>> GetAllAssignedTicketsAsync();
        public Task<IEnumerable<Ticket>> GetAllEscalatedTickets();


        public Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        public Task<List<Ticket>> GetNumberOfClosedAvailableTickets();
        Task<List<DailyTicketMetric>> GetDailyTicketMetrics();
        Task<List<AgentDailyMetric>> GetAgentDailyMetrics();
        Task<List<DailyResolutionMetric>> GetDailyResolutionMetrics();
    }






}
