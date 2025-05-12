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
                    .Include(u => u.category)
                    .Include(u => u.User)
                    .Include(u => u.location)
                    .Include(u => u.Agent)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all open tickets: " + ex.Message, ex);
                return Enumerable.Empty<Ticket>();
            }
        }
        public async Task<IEnumerable<Ticket>> GetAllClosedTicketsAsync()
        {

            try
            {
                return await alexSupportDB.Ticket.Where(t => t.Status == "Closed")
                    .Include(u => u.category)
                    .Include(u => u.User)
                    .Include(u => u.location)
                    .Include(u => u.Agent)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all Closed tickets: " + ex.Message, ex);
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
                    .ThenInclude(u => u.Department)
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
        public async Task<bool> EsclateTicketAsync(Ticket ticket, int Id)
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
                        existingTicket.AgentID = null;
                        existingTicket.Status = "Esclated";
                        existingTicket.Due_Minutes = null;
                        existingTicket.Assign_Date = null;
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

        public async Task<bool> CloseTicketAsync(Ticket ticket, int Id)
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
                        existingTicket.Status = "Closed";
                        existingTicket.IsSolved = ticket.IsSolved;
                        existingTicket.Comments = ticket.Comments;
                        existingTicket.Solution = ticket.Solution;
                        existingTicket.CloseDate = DateTime.Now;
                        var rate = CalculateRate(ticket.OpenDate, existingTicket.CloseDate ?? DateTime.Now, ticket.Due_Minutes ?? 0);
                        existingTicket.TicketRate = rate;
                        alexSupportDB.Update(existingTicket);
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

        public async Task<bool> AddSolutionToTicketAsync(Ticket ticket, int Id)
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
                        existingTicket.Solution = ticket.Solution;
                        alexSupportDB.Update(existingTicket);
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
        private decimal CalculateRate(DateTime openDate, DateTime closeDate, int dueMinutes)
        {
            double actualMinutes = (closeDate - openDate).TotalMinutes;

            if (actualMinutes <= 0)
            {
                return 0;
            }

            decimal rate = (decimal)(dueMinutes / actualMinutes) * 100;

            if (rate > 100)
            {
                rate = 100;
            }

            return Math.Round(rate, 2);
        }

    }
}
