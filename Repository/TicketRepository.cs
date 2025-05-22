using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.Services.Extensions;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace AlexSupport.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<TicketRepository> logger;
        private readonly INotificationService Notification;
        private readonly INotificationRepoisitory notificationRepoisitory;
        private readonly IAppUserRepoistory users;
        private readonly ILogService Log;
        private readonly IJSRuntime JS;
        public TicketRepository(AlexSupportDB alexSupportDB
            , ILogger<TicketRepository> logger
            , INotificationService Notification
            , INotificationRepoisitory notificationRepoisitory
            , IAppUserRepoistory users
            , ILogService Log
            , IJSRuntime JS)
        {

            this.alexSupportDB = alexSupportDB;
            this.logger = logger;
            this.Notification = Notification;
            this.notificationRepoisitory = notificationRepoisitory;
            this.users = users;
            this.Log = Log;
            this.JS = JS;
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
                    ticket.UID = await Log.ReturnCurrentUserID();
                    await alexSupportDB.Ticket.AddAsync(ticket);
                    var admins = await users.GetAllAdminsAsync();
                    var user = await users.GetUserByIdAsync(ticket.UID);
                    foreach (var admin in admins)
                    {
                        var message = $"Hello Mr.{admin.Fname} {admin.Lname}\n {user.Fname} {user.Lname} just created a ticket.";
                        await SendNotificationAsync(user.UID, admin.UID, message);
                    }
                    await Log.CreateLogAsync(ticket.TID, "Create a new ticket in the system");
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
                var currentuserid = await Log.ReturnCurrentUserID();
                var user = await users.GetUserByIdAsync(currentuserid);
                if (user.Role == "Admin")
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status != "Closed")
                          .Include(u => u.category)
                          .Include(u => u.User)
                          .Include(u => u.location)
                          .Include(u => u.Agent)
                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status != "Closed" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.category)
                   .Include(u => u.User)
                   .Include(u => u.location)
                   .Include(u => u.Agent)
                   .ToListAsync();
                }
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
                var currentuserid = await Log.ReturnCurrentUserID();
                var user = await users.GetUserByIdAsync(currentuserid);
                if (user.Role == "Admin")
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Closed")
                          .Include(u => u.category)
                          .Include(u => u.User)
                          .Include(u => u.location)
                          .Include(u => u.Agent)
                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Closed" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.category)
                   .Include(u => u.User)
                   .Include(u => u.location)
                   .Include(u => u.Agent)
                   .ToListAsync();
                }
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
                        alexSupportDB.Update(existingTicket);
                        var adminid = await Log.ReturnCurrentUserID();
                        var admin = await users.GetUserByIdAsync(adminid);
                        var agent = await users.GetUserByIdAsync(Convert.ToInt32(existingTicket.AgentID));
                        var user = await users.GetUserByIdAsync(existingTicket.UID);

                        // Notification to agent
                        var agentMessage = $"Hello Eng:{agent.Fname} {agent.Lname}\n Mr.{admin.Fname} {admin.Lname} just assign a ticket for you.";
                        await SendNotificationAsync(adminid, agent.UID, agentMessage);

                        // Notification to user
                        var userMessage = $"Hello {user.Fname} {user.Lname}\n the ticket you created just assigned to Eng:{agent.Fname} {agent.Lname}";
                        await SendNotificationAsync(adminid, user.UID, userMessage);
                        await Log.CreateLogAsync(existingTicket.TID, "Assign an open ticket in the system");
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
                logger.LogError("Error in assigning ticket: " + ex.InnerException, ex);
                return false;
            }
        }
        public async Task SendNotificationAsync(int fromUserId, int toUserId, string message)
        {
            // Create and save the notification
            var note = new SystemNotification()
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Message = message,
                SentAt = DateTime.Now,
            };

            await notificationRepoisitory.CreateNotificationAsync(note);
            await Notification.SendToUserAsync(toUserId.ToString(), message);

            // Play sound if JS runtime is provided
            if (JS != null)
            {
                await JS.InvokeVoidAsync("audioPlayer.play", "Sounds/Helllo.mp3");
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllAssignedTickets()
        {
            try
            {
                var currentuserid = await Log.ReturnCurrentUserID();
                var user = await users.GetUserByIdAsync(currentuserid);
                if (user.Role == "Admin")
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Assigned")
                          .Include(u => u.category)
                          .Include(u => u.User)
                          .Include(u => u.location)
                          .Include(u => u.Agent)
                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Assigned" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.category)
                   .Include(u => u.User)
                   .Include(u => u.location)
                   .Include(u => u.Agent)
                   .ToListAsync();
                }
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
                        alexSupportDB.Update(existingTicket);
                        var CurrentUserRole = await Log.ReturnCurrentUserRole();
                        if (CurrentUserRole == "Admin")
                        {
                            var adminid = await Log.ReturnCurrentUserID();
                            var admin = await users.GetUserByIdAsync(adminid);
                            var agent = await users.GetUserByIdAsync(Convert.ToInt32(existingTicket.AgentID));
                            var user = await users.GetUserByIdAsync(existingTicket.UID);
                            // Notification to agent
                            if (agent.UID != 0)
                            {
                                var agentMessage = $"Hello Eng:{agent.Fname} {agent.Lname}\n Mr.{admin.Fname} {admin.Lname} just escalated a ticket which was assigned for you.";
                                await SendNotificationAsync(adminid, agent.UID, agentMessage);
                            }
                            // Notification to user
                            var userMessage = $"Hello {user.Fname} {user.Lname}\n the ticket you created and was  assigned to Eng:{agent.Fname} {agent.Lname} was escalated by Admin";
                            await SendNotificationAsync(adminid, user.UID, userMessage);

                        }
                        if (CurrentUserRole == "Agent")
                        {
                            var admins = await users.GetAllAdminsAsync();
                            var user = await users.GetUserByIdAsync(existingTicket.UID);
                            var agent = await users.GetUserByIdAsync(Convert.ToInt32(existingTicket.AgentID));
                            foreach (var admin in admins)
                            {
                                var message = $"Hello Mr.{admin.Fname} {admin.Lname}\n Eng:{agent.Fname} {agent.Lname} just closed a ticket which was escalated to him.";
                                await SendNotificationAsync(agent.UID, admin.UID, message);
                            }
                            var userMessage = $"Hello {user.Fname} {user.Lname}\n the ticket you created and was  assigned to Eng:{agent.Fname} {agent.Lname} was escalated by the agent support";
                            await SendNotificationAsync(agent.UID, user.UID, userMessage);
                        }
                        if (CurrentUserRole == "User")
                        {
                            var admins = await users.GetAllAdminsAsync();
                            var user = await users.GetUserByIdAsync(existingTicket.UID);
                            var agent = await users.GetUserByIdAsync(Convert.ToInt32(existingTicket.AgentID));
                            foreach (var admin in admins)
                            {
                                var message = $"Hello Mr.{admin.Fname} {admin.Lname}\n {user.Fname} {user.Lname} just escalated a ticket which was created by him.";
                                await SendNotificationAsync(user.UID, admin.UID, message);

                            }
                            if (agent.UID != 0)
                            {
                                var agentMessage = $"Hello Eng:{agent.Fname} {agent.Lname}\n {user.Fname} {user.Lname}  just escalated a ticket which was created by him.";
                                await SendNotificationAsync(user.UID, agent.UID, agentMessage);
                            }
                        }
                        await Log.CreateLogAsync(existingTicket.TID, "Escalate an open ticket in the system");
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
                        var CurrentUserRole = await Log.ReturnCurrentUserRole();
                        if (CurrentUserRole == "Admin")
                        {
                            var adminid = await Log.ReturnCurrentUserID();
                            var admin = await users.GetUserByIdAsync(adminid);
                            var agent = await users.GetUserByIdAsync(Convert.ToInt32(existingTicket.AgentID));
                            var user = await users.GetUserByIdAsync(existingTicket.UID);
                            // Notification to agent
                            if (agent.UID != 0)
                            {
                                var agentMessage = $"Hello Eng:{agent.Fname} {agent.Lname}\n Mr.{admin.Fname} {admin.Lname} just closed a ticket which was assigned for you.";
                                await SendNotificationAsync(adminid, agent.UID, agentMessage);
                            }
                            // Notification to user
                            var userMessage = $"Hello {user.Fname} {user.Lname}\n the ticket you created and was  assigned to Eng:{agent.Fname} {agent.Lname} was closed by Admin";
                            await SendNotificationAsync(adminid, user.UID, userMessage);

                        }
                        if (CurrentUserRole == "Agent")
                        {
                            var admins = await users.GetAllAdminsAsync();
                            var user = await users.GetUserByIdAsync(existingTicket.UID);
                            var agent = await users.GetUserByIdAsync(Convert.ToInt32(existingTicket.AgentID));
                            foreach (var admin in admins)
                            {
                                var message = $"Hello Mr.{admin.Fname} {admin.Lname}\n Eng:{agent.Fname} {agent.Lname} just closed a ticket which was assigned to him.";
                                await SendNotificationAsync(agent.UID, admin.UID, message);
                            }
                            var userMessage = $"Hello {user.Fname} {user.Lname}\n the ticket you created and was  assigned to Eng:{agent.Fname} {agent.Lname} was closed by the agent support";
                            await SendNotificationAsync(agent.UID, user.UID, userMessage);
                        }
                        if (CurrentUserRole == "User")
                        {
                            var admins = await users.GetAllAdminsAsync();
                            var user = await users.GetUserByIdAsync(existingTicket.UID);
                            var agent = await users.GetUserByIdAsync(Convert.ToInt32(existingTicket.AgentID));
                            foreach (var admin in admins)
                            {
                                var message = $"Hello Mr.{admin.Fname} {admin.Lname}\n {user.Fname} {user.Lname} just closed a ticket which was created by him.";
                                await SendNotificationAsync(user.UID, admin.UID, message);

                            }
                            if (agent.UID != 0)
                            {
                                var agentMessage = $"Hello Eng:{agent.Fname} {agent.Lname}\n {user.Fname} {user.Lname}  just closed a ticket which was created by him.";
                                await SendNotificationAsync(user.UID, agent.UID, agentMessage);
                            }
                        }
                        await Log.CreateLogAsync(existingTicket.TID, "Close an open ticket in the system");
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
                        await Log.CreateLogAsync(existingTicket.TID, "Add a solution after close a ticket in the system");
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





        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                var currentuserid = await Log.ReturnCurrentUserID();
                var user = await users.GetUserByIdAsync(currentuserid);
                IEnumerable<Ticket> tickets;
                if (user.Role == "Admin")
                {
                    tickets = await alexSupportDB.Ticket
                       .Include(u => u.category)
                       .Include(u => u.User)
                       .Include(u => u.location)
                       .Include(u => u.Agent)
                       .ToListAsync();
                }
                else
                {
                    tickets = await alexSupportDB.Ticket
                       .Where(u => u.UID == user.UID || u.AgentID == user.UID)
                       .Include(u => u.category)
                       .Include(u => u.User)
                       .Include(u => u.location)
                       .Include(u => u.Agent)
                       .ToListAsync();
                }
                if (tickets.Any())
                {
                    return tickets;
                }
                else
                {
                    logger.LogInformation($"No  tickets found for");
                    return Enumerable.Empty<Ticket>();

                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all tickets: " + ex.Message, ex);
                return Enumerable.Empty<Ticket>();
            }
        }


    }
}
