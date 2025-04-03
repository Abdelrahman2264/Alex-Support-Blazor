using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

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
                    ticket.UID = 11;
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

        public async Task<IEnumerable<Ticket>> GetAllOpendTicketsAsync()
        {

            try
            {
                return await alexSupportDB.Ticket.Where(t => t.Status != "Closed")
                    .Include(u=>u.category)
                    .Include(u => u.User)
                    .Include(u=>u.location)
                    .Include(u=>u.Agent)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all open tickets: " + ex.Message, ex);
                return Enumerable.Empty<Ticket>();
            }
        }
        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            try
            {
                return await alexSupportDB.Ticket
                    .Include(u => u.category)
                    .Include(u => u.User)
                    .Include(u => u.location)
                    .Include(u => u.Agent)
                    .FirstOrDefaultAsync(t => t.TID == id);
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting ticket by id: " + ex.Message, ex);
                return new Ticket();
            }
        }
        public async Task<bool> AssignTicketAsync(Ticket ticket, int Id)
        {
            try
            {
                if (ticket == null)
                {
                    logger.LogError("Ticket is null");
                    return false;
                }
                else
                {
                    var existingTicket = await GetTicketByIdAsync(Id);
                    if (existingTicket != null)
                    {
                        existingTicket.AgentID = ticket.AgentID;
                        existingTicket.Status = "Assigned";
                        existingTicket.Due_Minutes = ticket.Due_Minutes;
                        existingTicket.Assign_Date = DateTime.Now;
                        await alexSupportDB.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        logger.LogError("Ticket not found");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in assigning ticket: " + ex.Message, ex);
                return false;
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllAssignedTickets()
        {
            try
            {
                return await alexSupportDB.Ticket.Where(t => t.Status == "Assigned")
                    .Include(u => u.category)
                    .Include(u => u.User)
                    .Include(u => u.location)
                    .Include(u => u.Agent)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all assigned tickets: " + ex.Message, ex);
                return Enumerable.Empty<Ticket>();
            }
        }
    }
}
