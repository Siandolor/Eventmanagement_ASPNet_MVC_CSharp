using Eventmanagement.Models.Tickets;

public class EventmanagementContext : DbContext
{
    public EventmanagementContext(DbContextOptions<EventmanagementContext> options) : base(options) { }

    public DbSet<LogIn> LogIns { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<InternalCoworker> InternalCoworkers { get; set; } // Zu beachten: <<<Contains UserRoles: Admin/Coworker>>>
    public DbSet<Location> Locations { get; set; }
    public DbSet<Moderator> Moderators { get; set; }
    public DbSet<Organizer> Organizers { get; set; }
    public DbSet<Performer> Performers { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<SeatUnit> SeatUnits { get; set; } = default!;
    public DbSet<EventBasics> EventBasics { get; set; } = default!;
    public DbSet<EventSession> EventSessions { get; set; }
    public DbSet<EventAttachment> EventAttachments { get; set; }
    public DbSet<EventSeatUnit> EventSeatUnits { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogIn>()
            .HasOne(l => l.Customer)
            .WithOne(c => c.Customer_LogIn)
            .HasForeignKey<Customer>(c => c.Customer_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LogIn>()
            .HasOne(l => l.InternalCoworker)
            .WithOne(o => o.InternalCoworker_LogIn)
            .HasForeignKey<InternalCoworker>(o => o.InternalCoworker_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LogIn>()
            .HasOne(l => l.Location)
            .WithOne(i => i.Location_LogIn)
            .HasForeignKey<Location>(i => i.Location_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LogIn>()
            .HasOne(l => l.Moderator)
            .WithOne(i => i.Moderator_LogIn)
            .HasForeignKey<Moderator>(i => i.Moderator_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LogIn>()
            .HasOne(l => l.Performer)
            .WithOne(i => i.Performer_LogIn)
            .HasForeignKey<Performer>(i => i.Performer_Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InternalCoworker>()
            .Property(e => e.InternalCoworker_MonthlySalary)
            .HasPrecision(7, 2);

        modelBuilder.Entity<Moderator>()
            .Property(e => e.Moderator_EventSalary)
            .HasPrecision(8, 2);

        modelBuilder.Entity<Performer>()
            .Property(e => e.Performer_EventSalary)
            .HasPrecision(9, 2);

        modelBuilder.Entity<SeatUnit>()
            .HasOne(s => s.Location)
            .WithMany(l => l.SeatUnits)
            .HasForeignKey(s => s.Location_Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventSeatUnit>()
            .HasIndex(es => new { es.Event_Id, es.SeatUnit_Id })
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Event)
            .WithMany()
            .HasForeignKey(t => t.Event_Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Session)
            .WithMany()
            .HasForeignKey(t => t.EventSession_Id)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.SeatUnit)
            .WithMany()
            .HasForeignKey(t => t.EventSeatUnit_Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
