using System;

namespace Domain;

public class MeetingAttendee
{
    public string? UserId { get; set; }
    public User User { get; set; } = null!;
    public string? MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    public bool IsHost { get; set; }
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;
}