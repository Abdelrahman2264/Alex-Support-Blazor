using AlexSupport.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace AlexSupport.Data
{
    public partial class AlexSupportDB : DbContext
    {
        public AlexSupportDB(DbContextOptions<AlexSupportDB> options) : base(options)
        {

        }

        public virtual DbSet<AppUser> AppUser { get; set; } = null!;
        public virtual DbSet<Ticket> Ticket { get; set; } = null!;
        public virtual DbSet<Department> Department { get; set; } = null!;

        public virtual DbSet<Category> Category { get; set; } = null!;
        public virtual DbSet<Tlog> Tlogs { get; set; } = default!;
        public virtual DbSet<Location> Locations { get; set; } = default!;
        public virtual DbSet<DailyTasks> DailyTasks { get; set; } = default!;
        public virtual DbSet<SystemNotification> Notifications { get; set; } = default!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = default!;
        public virtual DbSet<SystemLogs> SystemLogs { get; set; } = default!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UID).HasName("APPUSERUID_PK");
                entity.HasIndex(e => e.Fingerprint, "APPUSERFINGERPRINT_UQ")
                    .IsUnique();
                entity.HasIndex(e => e.LoginName, "APPUSERLOGINNAME_UQ")
                 .IsUnique();
                entity.HasIndex(e => e.Email, "APPUSEREMAIL_UQ")
                  .IsUnique();
                entity.HasIndex(e => e.Phone, "APPUSERPHONE_UQ")
                 .IsUnique();
                entity.Property(e => e.UID).HasColumnName("UID");
                entity.Property(e => e.Fingerprint).HasColumnName("Fingerprint");
                entity.Property(e => e.LoginName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LoginName");
                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Email");
                entity.Property(e => e.Fname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Fname");
                entity.Property(e => e.Lname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Lname");
                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Password");
                entity.Property(e => e.JobTitle)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("JobTitle");
                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("MobilePhone");
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive)
                   .HasDefaultValue(true);
                entity.Property(e => e.EmailVerified)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.HasOne(e => e.Department).WithMany(d => d.Users)
                .HasForeignKey(e => e.DID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUser_Department");

                entity.Property(e => e.Create_Date)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("(sysdatetime())")
                    .HasColumnName("Create_Date");

            });
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.TID).HasName("TICKETID_PK");
                entity.Property(e => e.Subject)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Subject");
                entity.Property(e => e.Issue)
                    .HasMaxLength(850)
                    .IsUnicode(false)
                    .HasColumnName("Issue");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Status");

                entity.Property(e => e.OpenDate)
                    .HasColumnType("datetime2(0)")
                    .HasColumnName("OpenDate");
                entity.HasOne(e => e.Location)
                    .WithMany(d => d.Ticket)
                    .HasForeignKey(e => e.LID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Location");
                entity.HasOne(e => e.Category)
                    .WithMany(d => d.Ticket)
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Category");
                entity.HasOne(e => e.User)
                    .WithMany(d => d.Ticket)
                    .HasForeignKey(e => e.UID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_UserId");
                entity.HasOne(e => e.Agent)
                    .WithMany(d => d.AgentTicket)
                    .HasForeignKey(e => e.AgentID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_AgentId");
            });
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DID).HasName("DEPARTMENTID_PK");
                entity.Property(e => e.DID).HasColumnName("DID");
                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Department Name");

            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CID).HasName("CATEGORYID_PK");
                entity.Property(e => e.CID).HasColumnName("CID");
                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Category Name");

            });
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.LID).HasName("LOCATIONID_PK");
                entity.Property(e => e.LID).HasColumnName("LID");
                entity.Property(e => e.LocationName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Location Name");
            });
            modelBuilder.Entity<Tlog>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("TLOGID_PK");
                entity.Property(e => e.Id).HasColumnName("TLOG ID");
                entity.Property(e => e.TID)
                    .HasColumnType("INT")
                    .HasColumnName("TID");
                entity.Property(e => e.actionTime)
                    .HasColumnType("datetime2(0)")
                    .HasColumnName("Action Time");
                entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Action");
                entity.HasOne(e => e.Ticket)
                    .WithMany(d => d.Tlogs)
                    .HasForeignKey(e => e.TID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tlog_TicketId");
                entity.HasOne(e => e.AppUser)
                    .WithMany(d => d.Tlogs)
                    .HasForeignKey(e => e.UID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tlog_AppUserId");
            });

            modelBuilder.Entity<DailyTasks>(entity =>
            {
                entity.HasKey(e => e.DTID).HasName("DAILYTASKID_PK");
                entity.Property(e => e.Subject)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Subject");
                entity.Property(e => e.Issue)
                    .HasMaxLength(850)
                    .IsUnicode(false)
                    .HasColumnName("Issue");  
                entity.Property(e => e.RecurrenceDays)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Type");
                entity.HasOne(e => e.Category)
                    .WithMany(d => d.DailyTask)
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DailyTask_Category");
                entity.HasOne(e => e.Agent)
                    .WithMany(d => d.DailyTasks)
                    .HasForeignKey(e => e.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DailyTask_AppUser"); 
                entity.HasOne(e => e.Location)
                    .WithMany(d => d.Tasks)
                    .HasForeignKey(e => e.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DailyTask_Location");  
                entity.HasOne(e => e.User)
                    .WithMany(d => d.UserDailyTasks)
                    .HasForeignKey(e => e.UID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DailyTask_CreatedUser");
                // Delete notifications when user is deleted

            }); modelBuilder.Entity<SystemNotification>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.NID).HasName("NOTIFICATIONNID_PK");

                // FromUserId - required, foreign key
                entity.Property(n => n.FromUserId)
                      .IsRequired();
                // Assuming IdentityUser key length

                entity.HasOne(n => n.FromUser)
                      .WithMany()
                      .HasForeignKey(n => n.FromUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // ToUserId - required, foreign key
                entity.Property(n => n.ToUserId)
                      .IsRequired();

                entity.HasOne(n => n.ToUser)
                      .WithMany()
                      .HasForeignKey(n => n.ToUserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Message - required with a max length (optional tweak)
                entity.Property(n => n.Message)
                      .IsRequired()
                      .HasMaxLength(1000);

                // SentAt - required
                entity.Property(n => n.SentAt)
                      .IsRequired();

                // ReadAt - optional
                entity.Property(n => n.ReadAt)
                      .IsRequired(false);

                // IsRead - required
                entity.Property(n => n.IsRead)
                      .IsRequired();
            });
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(c => c.CHID).HasName("CHATMESSAGEID_PK");
                entity.HasIndex(m => m.TicketId);
                entity.HasIndex(m => m.SenderId);
                entity.HasOne(e => e.Ticket)
                                  .WithMany(d => d.ChatMessages)
                                  .HasForeignKey(e => e.TicketId)
                                  .OnDelete(DeleteBehavior.ClientSetNull)
                                  .HasConstraintName("FK_TICKET_CHATMESSAGES");
                entity.HasOne(e => e.Sender)
                               .WithMany(d => d.ChatMessages)
                               .HasForeignKey(e => e.SenderId)
                               .OnDelete(DeleteBehavior.ClientSetNull)
                               .HasConstraintName("FK_APPUSER_CHATMESSAGES");
                entity.Property(n => n.IsRead)
                     .IsRequired(false);
                entity.Property(n => n.SenderId).IsRequired();
                entity.Property(n => n.TicketId).IsRequired();
                entity.Property(n => n.MessageText).IsRequired();
                entity.Property(n => n.SentDate).IsRequired();
                entity.Property(n => n.MessageText).HasMaxLength(int.MaxValue);
            });
            modelBuilder.Entity<SystemLogs>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("SYSTEMLOGID_PK");
                entity.Property(e => e.Id).HasColumnName("SYSTEMLOGS ID");
                entity.Property(e => e.actionTime)
                    .HasColumnType("datetime2(0)")
                    .HasColumnName("Action Time");
                entity.Property(e => e.Action)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Action");
                entity.HasOne(e => e.AppUser)
                    .WithMany(d => d.SystemLogs)
                    .HasForeignKey(e => e.UID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemTLog_AppUserId");

     
            });
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
