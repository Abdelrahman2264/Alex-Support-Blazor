using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface ITicketRepository
    {
        public Task<Ticket> CreateTicketAsync(Ticket ticket);
    }
}
