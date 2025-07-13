using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.Services.Extensions;
using AlexSupport.Services.Models;
using AlexSupport.ViewModels;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using static AlexSupport.Components.Shared.UserRateComponent;

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

                    await alexSupportDB.SaveChangesAsync();
                    await Notification.SendTicketUpdateAsync(ticket.TID, "");
                    await Log.CreateLogAsync(ticket.TID, "Create a new ticket in the system");


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
                          .Include(u => u.Category)
                          .Include(u => u.User)
                          .Include(u => u.Location)
                          .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status != "Closed" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.Category)
                   .Include(u => u.User)
                   .Include(u => u.Location)
                   .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

                   .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all open tickets: " + ex.Message, ex);
                return Enumerable.Empty<Ticket>();
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllAssignedTicketsAsync()
        {

            try
            {
                var currentuserid = await Log.ReturnCurrentUserID();
                var user = await users.GetUserByIdAsync(currentuserid);
                if (user.Role == "Admin")
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Assigned")
                          .Include(u => u.Category)
                          .Include(u => u.User)
                          .Include(u => u.Location)

                          .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Assigned" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.Category)
                   .Include(u => u.User)
                   .Include(u => u.Location)
                   .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

                   .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all open tickets: " + ex.Message, ex);
                return Enumerable.Empty<Ticket>();
            }
        }
        public async Task<IEnumerable<Ticket>> GetAllEscalatedTickets()
        {

            try
            {
                var currentuserid = await Log.ReturnCurrentUserID();
                var user = await users.GetUserByIdAsync(currentuserid);
                if (user.Role == "Admin")
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Escalated")
                          .Include(u => u.Category)
                          .Include(u => u.User)
                          .Include(u => u.Location)
                          .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Escalated" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.Category)
                   .Include(u => u.User)
                   .Include(u => u.Location)
                   .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

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
                          .Include(u => u.Category)
                          .Include(u => u.User)
                          .Include(u => u.Location)
                          .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Closed" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.Category)
                   .Include(u => u.User)
                   .Include(u => u.Location)
                   .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

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
                    .Include(u => u.Category)
                    .Include(u => u.User)
                    .ThenInclude(u => u.Department)
                    .Include(u => u.Location)
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
                        existingTicket.AssignDate = DateTime.Now;
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
                        await Notification.SendTicketUpdateAsync(existingTicket.TID, "Assigned");

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
            var currentuserid = await Log.ReturnCurrentUserID();
            if (currentuserid == toUserId)
            {
                return;
            }
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
                          .Include(u => u.Category)
                          .Include(u => u.User)
                          .Include(u => u.Location)
                          .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

                          .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket.Where(t => t.Status == "Assigned" && (t.UID == user.UID || t.AgentID == user.UID))
                   .Include(u => u.Category)
                   .Include(u => u.User)
                   .Include(u => u.Location)
                   .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

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
                        existingTicket.Status = "Escalated";
                        existingTicket.Due_Minutes = null;
                        existingTicket.AssignDate = null;
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
                        await Notification.SendTicketUpdateAsync(existingTicket.TID, "Escalated");

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
                        await Notification.SendTicketUpdateAsync(existingTicket.TID, "Closed");

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
        public async Task<bool> AddUserRateToTicketAsync(RatingModel rate, int Id)
        {
            try
            {
                if (rate == null)
                {
                    logger.LogError("rate model is null");
                    return false;
                }
                else
                {
                    var existingTicket = await GetTicketByIdAsync(Id);
                    if (existingTicket != null)
                    {
                        existingTicket.UserRate = rate.Rating;
                        existingTicket.UserFeedBack = rate.Comment;
                        alexSupportDB.Update(existingTicket);
                        await Log.CreateLogAsync(existingTicket.TID, "Add a User Rate after close a ticket in the system");
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
                logger.LogError("Error in Add A Rate For A Ticket: " + ex.Message, ex);
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
                       .Include(u => u.Category)
                       .Include(u => u.User)
                       .Include(u => u.Location)
                       .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)
                       .ToListAsync();
                }
                else
                {
                    tickets = await alexSupportDB.Ticket
                       .Where(u => u.UID == user.UID || u.AgentID == user.UID)
                       .Include(u => u.Category)
                       .Include(u => u.User)
                       .Include(u => u.Location)
                       .Include(u => u.Agent)
                       .OrderByDescending(t => t.OpenDate)

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

        public async Task<List<Ticket>> GetNumberOfClosedAvailableTickets()
        {
            try
            {
                var id = await Log.ReturnCurrentUserID();
                var currentuser = await users.GetUserByIdAsync(id);
                if (currentuser.Role == "Admin")
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                        .Where(t => t.Status == "Closed" && !string.IsNullOrEmpty(t.Due_Minutes.ToString()))
                        .OrderByDescending(t => t.OpenDate)

                        .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                                     .Where(t => t.Status == "Closed" && t.AssignDate != null && t.AgentID == currentuser.UID)
                       .OrderByDescending(t => t.OpenDate)

                                     .ToListAsync();
                }
            }
            catch
            {
                logger.LogError("Error In Get Number of available closed tickets");
                return new List<Ticket>();
            }


        }
        public async Task<List<DailyTicketMetric>> GetDailyTicketMetrics()
        {
            try
            {

                var id = await Log.ReturnCurrentUserID();
                var currentuser = await users.GetUserByIdAsync(id);
                if (currentuser.Role == "Admin")
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                        .GroupBy(t => t.OpenDate.Date)
                        .Select(g => new DailyTicketMetric
                        {
                            Date = g.Key,
                            TotalTickets = g.Count(),
                            AgentTickets = g.Count(t => t.Agent != null)
                        })
                        .OrderBy(x => x.Date)
                        .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                        .Where(u => u.AgentID == currentuser.UID)
                   .GroupBy(t => t.OpenDate.Date)
                   .Select(g => new DailyTicketMetric
                   {
                       Date = g.Key,
                       TotalTickets = g.Count(),
                       AgentTickets = g.Count(t => t.Agent != null)
                   })
                   .OrderBy(x => x.Date)
                   .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error In Get Daily Ticket Metric" + ex.Message, ex);
                return new List<DailyTicketMetric>();

            }

        }

        public async Task<List<AgentDailyMetric>> GetAgentDailyMetrics()
        {
            try
            {

                var id = await Log.ReturnCurrentUserID();
                var currentuser = await users.GetUserByIdAsync(id);
                if (currentuser.Role == "Admin")
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                        .Where(t => t.Agent != null && t.OpenDate != null)
                        .GroupBy(t => new { t.OpenDate.Date, t.Agent.LoginName })
                        .Select(g => new AgentDailyMetric
                        {
                            Date = g.Key.Date,
                            AgentName = g.Key.LoginName,
                            TicketCount = g.Count()
                        })
                        .OrderBy(x => x.Date)
                        .ThenByDescending(x => x.TicketCount)
                        .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                      .Where(t => t.Agent != null && t.OpenDate != null && t.AgentID == currentuser.UID)
                      .GroupBy(t => new { t.OpenDate.Date, t.Agent.LoginName })
                      .Select(g => new AgentDailyMetric
                      {
                          Date = g.Key.Date,
                          AgentName = g.Key.LoginName,
                          TicketCount = g.Count()
                      })
                      .OrderBy(x => x.Date)
                      .ThenByDescending(x => x.TicketCount)
                      .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error In get Agent Daily Metric");
                return new List<AgentDailyMetric>();
            }
        }

        public async Task<List<DailyResolutionMetric>> GetDailyResolutionMetrics()
        {
            try
            {

                var id = await Log.ReturnCurrentUserID();
                var currentuser = await users.GetUserByIdAsync(id);
                if (currentuser.Role == "Admin")
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                        .Where(t => t.CloseDate.HasValue) // Only include tickets with a CloseDate
                        .GroupBy(t => t.CloseDate.Value.Date)
                        .Select(g => new DailyResolutionMetric
                        {
                            Date = g.Key,
                            SolvedTickets = g.Count(t => t.IsSolved == true),
                            TotalTickets = g.Count()
                        })
                        .OrderBy(x => x.Date)
                        .ToListAsync();
                }
                else
                {
                    return await alexSupportDB.Ticket
                        .Include(u => u.Location)
                        .Include(u => u.Category)
                        .Where(t => t.CloseDate.HasValue && t.AgentID == currentuser.UID) // Only include tickets with a CloseDate
                        .GroupBy(t => t.CloseDate.Value.Date)
                        .Select(g => new DailyResolutionMetric
                        {
                            Date = g.Key,
                            SolvedTickets = g.Count(t => t.IsSolved == true),
                            TotalTickets = g.Count()
                        })
                        .OrderBy(x => x.Date)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error In Get Daily Resolution Metrics: " + ex.Message, ex);
                return new List<DailyResolutionMetric>();

            }

        }

    }
}
