using System;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public required DbSet<Meeting> Meetings { get; set; }
    public required DbSet<MeetingAttendee> MeetingAttendees { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MeetingAttendee>(x => x.HasKey(ma => new { ma.UserId, ma.MeetingId }));

        builder.Entity<MeetingAttendee>()
            .HasOne(ma => ma.User)
            .WithMany(u => u.Meetings)
            .HasForeignKey(ma => ma.UserId);

        builder.Entity<MeetingAttendee>()
            .HasOne(ma => ma.Meeting)
            .WithMany(m => m.Attendees)
            .HasForeignKey(ma => ma.MeetingId);
    }
}
