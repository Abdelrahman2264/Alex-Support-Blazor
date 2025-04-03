using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface ITicketRepository
    {
        public Task<Ticket> CreateTicketAsync(Ticket ticket);
        public Task<IEnumerable<Ticket>> GetAllOpendTicketsAsync();
        public Task<Ticket> GetTicketByIdAsync(int id);
        public Task<bool> AssignTicketAsync(Ticket ticket , int Id);
        public Task<IEnumerable<Ticket>> GetAllAssignedTickets();

    }
}
