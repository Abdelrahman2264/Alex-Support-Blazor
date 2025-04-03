using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;

namespace AlexSupport.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<TicketRepository> logger;

        public TicketRepository(AlexSupportDB alexSupportDB, ILogger<TicketRepository> logger)
        {

            this.alexSupportDB = alexSupportDB;
            this.logger = logger;
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            try
            {
                if (ticket == null)
                {
                    logger.LogError("Ticket is null");
                    return new Ticket();
                }
                else
                {
                    ticket.Status = "Open";
                    ticket.OpenDate = DateTime.Now;
                    ticket.AgentID = 2;
                    ticket.UID =2;
                    await alexSupportDB.Ticket.AddAsync(ticket);
                    await alexSupportDB.SaveChangesAsync();
                    return ticket;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in  creating ticket: " + ex.Message, ex);
                return new Ticket();
            }
        }
    }
}
