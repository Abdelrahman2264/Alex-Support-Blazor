using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface ITicketRepository
    {
        public Task<Ticket> CreateTicketAsync(Ticket ticket);
        public Task<IEnumerable<Ticket>> GetAllOpendTicketsAsync();
        public Task<Ticket> GetTicketByIdAsync(int id);
        public Task<bool> AssignTicketAsync(Ticket ticket , int Id);
        public Task<bool> EsclateTicketAsync(Ticket ticket , int Id);
        public Task<bool> CloseTicketAsync(Ticket ticket , int Id);
        public Task<bool> AddSolutionToTicketAsync(Ticket ticket , int Id);
        public Task<IEnumerable<Ticket>> GetAllAssignedTickets();
        public Task<IEnumerable<Ticket>> GetAllClosedTicketsAsync();
    }
}
